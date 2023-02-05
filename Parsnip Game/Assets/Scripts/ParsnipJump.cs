using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsnipJump : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 direction;

    void Start()
    {
        startPosition = transform.position;
        direction = new Vector3(0, 1, 0);
        StartCoroutine(FlipDirection());
    }

    void Update()
    {
        MoveCubeBackAndForth();
    }

    private void MoveCubeBackAndForth()
    {
        transform.Translate(direction * Time.deltaTime * 100f);
    }

    private IEnumerator FlipDirection()
    {
        while (true)
        {
            direction *= -1;
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }
}