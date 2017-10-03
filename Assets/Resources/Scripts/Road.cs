using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Road : MonoBehaviour
{
    private int RoadLength = 0;

    public List<int[]> RoadBlocks;

    private void Awake()
    {
        RoadBlocks = new List<int[]>();
    }

    public void incrementRoadLength()
    {
        ++RoadLength;
    }

    public int getRoadLength()
    {
        return RoadLength;
    }
}

