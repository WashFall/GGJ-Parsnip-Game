using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Update()
    {
        transform.position = PlayerPosition.Instance.GetPlayerTransform().position;
    }
}
