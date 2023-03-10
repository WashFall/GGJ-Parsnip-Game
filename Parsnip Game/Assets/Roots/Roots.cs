using System.Collections;
using UnityEngine;
using SplineMesh;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Roots : MonoBehaviour
{

    public Spline[] roots;

    // USE THIS TO CONTROL GROWTH FROM EXTERNAL SCRIPT
    public float rate = 0;

    // CALL THIS TO TRIGGER GROWTH
    public bool enableGrow;

    // SET THIS TO DISABLE USE RATE FROM EXTERNAL SCRIPT
    [Tooltip("This should be set for the script gets rate feed from external script")]
    public bool externalControl;


    [Tooltip("Can use other mesh if needed")]
    public Mesh mesh;
    [Tooltip("Change all roots materials here")]
    public Material material;
    public Vector3 rotation;

    [Range(1f, 2.5f)]
    public float maxGirth = 1f;

    // scale of original root
    private Vector3 scale;


    // this set's scale relative to scale i think :)
    public float startScale = 1;
    private bool previewRoots;
    public float growthTime = 3;
    public float reverseGrowthTime = 2;

    public AnimationCurve growthCurve;

    // for testing in editor
    public bool loopPreview;
    public bool pingPong;

    // reverse growth
    public bool reverseMode;

    // Start is called before the first frame update
    private void OnEnable() {
        previewRoots = false;
        Init();
    }

    private void SetScale() {
        scale = new Vector3(maxGirth, maxGirth, maxGirth);
    }
    public void SetsplineChildren() {
        List<Spline> tempList = new();
        foreach(Transform t in transform) {
            var foundSpline = t.GetComponent<Spline>();
            if (t.parent == transform && foundSpline != null) {
                tempList.Add(foundSpline);
            }
        }
        roots = tempList.ToArray();
    }

    #if UNITY_EDITOR
    public void PreviewRoots() {

        if (rate > 1) {
            reverseMode = false;
            rate = 0;
            previewRoots = false;
            EditorApplication.update -= EditorUpdate;
        }

        previewRoots = !previewRoots;


        if(previewRoots) {
            EditorApplication.update += EditorUpdate;
        } else {
            EditorApplication.update -= EditorUpdate;
        }
    }
#endif
    private void EditorUpdate() {
        Grow();
    }


    private bool GotsplineChildren() {
        return roots != null && roots.Length > 0;
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif

        if(enableGrow) {
            Grow();
        }
    }

    void UpdateRate() {

        if (pingPong && loopPreview) {
            // check reverse mode
            if (rate > 1 && !reverseMode) {
                reverseMode = true;
            } else if (rate < 0 && reverseMode) {
                reverseMode = false;
            }
        }


        // reverse or forward
        if (reverseMode && rate > 0) {
            rate -= Time.deltaTime / reverseGrowthTime;
        } else if (!reverseMode && rate < 1) {
            rate += Time.deltaTime / growthTime;
            // only reset if looping and pingPong is off
            if (rate > 1 && loopPreview && !pingPong) {
                rate = 0;
            }

        }

    }

    void Grow() {

        if (!GotsplineChildren())
            return;
        
        if(!externalControl)
            UpdateRate();

        if(rate <= 1)
            Contort();
    }

    private void Contort() {

        float sampledRate = rate;
        if (growthCurve != null) {
            sampledRate = growthCurve.Evaluate(Mathf.Clamp(rate,0.0001f,1));
        }

        foreach (Spline spline in roots) {

            float nodeDistance = 0;
            int i = 0;
            foreach (var n in spline.nodes) {
                float nodeDistanceRate = nodeDistance / spline.Length;
                float nodeScale = startScale * (sampledRate - nodeDistanceRate);
                n.Scale = new Vector2(nodeScale, nodeScale);
                // increase distance to next curve
                if (i < spline.curves.Count) {
                    nodeDistance += spline.curves[i++].Length;
                }
            }

            GameObject generated = GetGenerated(spline);
            if (generated != null) {
                MeshBender meshBender = generated.GetComponent<MeshBender>();
                meshBender.SetInterval(spline, 0, spline.Length * sampledRate);
                meshBender.ComputeIfNeeded();
            }
        }

    }

    private GameObject GetGenerated(Spline spline) {
        string generatedName = "generated by " + GetType().Name;
        var generatedTranform = spline.transform.Find(generatedName);

        GameObject generated = generatedTranform != null ? generatedTranform.gameObject : UOUtility.Create(generatedName, spline.gameObject,
            typeof(MeshFilter),
            typeof(MeshRenderer),
            typeof(MeshBender));
        return generated;
    }

    public void ResetRoots() {
        foreach(Spline spline in roots) {
            string generatedName = "generated by " + GetType().Name;
            var generatedTranform = spline.transform.Find(generatedName);
            if(generatedTranform != null) {
                DestroyImmediate(generatedTranform.gameObject);
            }
        }

        Init();
    }
    public void Init() {
        rate = 0;
        SetScale();

        foreach (Spline spline in roots) {
            GameObject generated = GetGenerated(spline);

            generated.GetComponent<MeshRenderer>().material = material;

            MeshBender meshBender = generated.GetComponent<MeshBender>();

            meshBender.Source = SourceMesh.Build(mesh)
                .Rotate(Quaternion.Euler(rotation))
                .Scale(scale);
            meshBender.Mode = MeshBender.FillingMode.StretchToInterval;
            meshBender.SetInterval(spline, 0, 0.01f);
        }

    }

#if UNITY_EDITOR
    private float maxGirthOld;
    public void OnValidate() {
        if(maxGirthOld != maxGirth) {
            Init();
            maxGirthOld = maxGirth;
        }

        if(pingPong && !loopPreview) {
            loopPreview = true;
        }
    }

    public void OnDrawGizmos() {
        
    }
#endif

}

#if UNITY_EDITOR
[CustomEditor(typeof(Roots))]
public class RootsEditor : Editor
{

    public override void OnInspectorGUI() {

        Roots targetRoots = (Roots)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Preview Roots")) {
            targetRoots.PreviewRoots();
        }


        if (GUILayout.Button("Refresh")) {
            targetRoots.SetsplineChildren();
            targetRoots.Init();
        }

        if (GUILayout.Button("Regenerate")) {
            targetRoots.SetsplineChildren();
            targetRoots.ResetRoots();
        }
        GUILayout.EndHorizontal();

        DrawDefaultInspector();
        // if something more
    }
}
#endif
