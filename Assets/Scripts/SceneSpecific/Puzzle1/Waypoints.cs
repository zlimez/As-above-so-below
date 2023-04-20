using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    int currentIndex = 0;
    public bool reachedEnd = false;
    public bool reachedStart = false;
    public Transform currentWaypoint;

    private void Start()
    {
        Debug.Log("Total Waypoints: " + GetTotalWaypoints());
        //currentWaypoint = transform.GetChild(0);
    }

    public int GetTotalWaypoints()
    {
        return transform.childCount;
    }

    public Transform GetNextWaypoint()
    {
        currentIndex += 1;
        reachedStart = false;
        if (currentIndex >= transform.childCount)
        {
            currentIndex = transform.childCount - 1;
            reachedEnd = true;
        }
        currentWaypoint = transform.GetChild(currentIndex);
        return currentWaypoint;
    }

    public Transform GetPrevWaypoint()
    {
        currentIndex -= 1;
        reachedEnd = false;
        if (currentIndex <= 0)
        {
            currentIndex = 0;
            reachedStart = true;
        }
        currentWaypoint = transform.GetChild(currentIndex);
        return currentWaypoint;
    }
}
