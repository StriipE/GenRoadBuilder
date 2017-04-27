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
}
