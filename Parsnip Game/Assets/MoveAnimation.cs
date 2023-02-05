using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
public class MoveAnimation : MonoBehaviour
{
  [SerializeField] private Rigidbody rb;
  [SerializeField] private Animator anim;


  private void Update()
  {
    if (rb.velocity != Vector3.zero)
    {
      anim.enabled = true;
    }
    else
    {
      anim.enabled = false;
    }

  }
}
