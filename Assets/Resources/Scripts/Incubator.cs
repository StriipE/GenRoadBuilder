using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Incubator : MonoBehaviour
{
    public List<Road> generatedRoads;

    public Incubator()
    {
        generatedRoads = new List<Road>();
    }

    public void addRoadToIncubator(Road road)
    {
        generatedRoads.Add(road);
    }
}

