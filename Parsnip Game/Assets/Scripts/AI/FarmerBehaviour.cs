using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerBehaviour : MonoBehaviour
{
    public Transform target;

    Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();

        SearchForTarget();
    }
    
    void SearchForTarget()
    {
        //PATROL
        //Get nodes
        //Choose a random node as destination
        //Set path to destination

        //IF sound is heard
        //Move faster towards sound(target)
        //IF target is reached (and no player is found)
        //=> SearchForTarget()
        //ELSE IF target is reached (and colliding with player catch area)
        //CatchTarget()

        //seeker.StartPath(transform.position, target.position);
    }

    void CatchTarget()
    {
        //Player lose life
        //Carry player off screen => OnComplete SearchForTarget();
        //Spawn new player
    }

    void GetNavNodes()
    {

    }
}
