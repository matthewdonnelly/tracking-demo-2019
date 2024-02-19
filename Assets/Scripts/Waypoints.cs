using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{

    public Main main;


    void OnTriggerEnter()
    {
        main.WaypointReached();

    }

}
