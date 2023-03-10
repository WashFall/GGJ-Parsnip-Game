using UnityEngine;

public class FOV : MonoBehaviour
{
    [Header("FOV Settings")]
    [Range(0, 50)]
    public float innerRadius;
    [Range(0, 50)]
    public float outerRadius;
    [Space]

    [Range(0, 50)]
    public float fovRadius;
    [Range(0, 360)]
    public float fovAngle;
    [Space]

    public LayerMask obstacleMask;

    public bool TargetInView(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist > fovRadius) { return false; }

        Vector3 dir = (target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, dir) < fovAngle / 2 || dist < outerRadius)
        {
            if (!Physics.Raycast(transform.position + Vector3.up * 2, dir, dist, obstacleMask))
            {
                //Return true if the target lies within the fov cone or the aggor/inner radius.
                return true;
            }
        }

        return false;
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}