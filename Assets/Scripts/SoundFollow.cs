﻿using UnityEngine;
using System.Collections;

public class SoundFollow : MonoBehaviour
{

    //Assign in the editor
    public GameObject parentWaypoints;

    Vector3[] waypoints;
    private Transform player;
    private Transform trans;

    void Awake()
    {
        // specific functionality
        waypoints = new Vector3[parentWaypoints.transform.childCount];
        int i = 0;
        foreach(Transform child in parentWaypoints.transform)
        {
            waypoints[i] = child.position;
            i++;
        }
        //Get the CameraRig
        player = GameObject.FindGameObjectWithTag("Player").transform;
        trans = transform;
    }

    // Update is called once per frame
    void Update()
    {

        // sort waypoints by distance
        System.Array.Sort<Vector3>(waypoints, delegate (Vector3 way1, Vector3 way2) {
            return Vector3.Distance(way1, player.position).CompareTo(Vector3.Distance(way2, player.position));
        });

        // get the two closest waypoints and find a point in between them
        trans.position = Vector3.Lerp(trans.position, ClosestPointOnLine(waypoints[0], waypoints[1], player.position), Time.deltaTime * 2);
    }

    // thanks to: http://forum.unity3d.com/threads/math-problem.8114/#post-59715
    Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        var vVector1 = vPoint - vA;
        var vVector2 = (vB - vA).normalized;

        var d = Vector3.Distance(vA, vB);
        var t = Vector3.Dot(vVector2, vVector1);

        if (t <= 0)
            return vA;

        if (t >= d)
            return vB;

        var vVector3 = vVector2 * t;

        var vClosestPoint = vA + vVector3;

        return vClosestPoint;
    }
}