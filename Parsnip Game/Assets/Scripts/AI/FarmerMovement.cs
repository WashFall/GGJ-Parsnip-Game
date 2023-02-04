using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerMovement : MonoBehaviour
{
    public Transform target;

    Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();

        if(target != null) { SearchForTarget(); }
    }

    void SearchForTarget()
    {
        //Patrol

        //IF sound is heard
        //Move faster towards sound

        //seeker.StartPath(transform.position, target.position);
    }

    void CatchTarget()
    {
        //IF within cathing range
        //Player lose life
        //Carry parsnip out of screen
        //New player spawn
        //On complete => SearchForTarget();
    }

    
}
