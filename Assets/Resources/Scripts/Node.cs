using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public bool isEmpty
    {
        get; set;
    }

    // Use this for initialization
    void Awake()
    {
        isEmpty = true;
    }

    public void setNode()
    {
        isEmpty = false;
    }

    public void freeNode()
    {
        isEmpty = true;
    }

    public int[] getCoordinates()
    {
        return new int[2] { Convert.ToInt32(transform.position.x / 4.5f), Convert.ToInt32(transform.position.z / 4.5f) };
    }

}
