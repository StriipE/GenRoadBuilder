using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class GenRoadBuilder : MonoBehaviour
{
    private const int X = 0;
    private const int Y = 1;
    private const int NUMBER_OF_ROADS_IN_GENERATION = 10;

    private GameObject start;
    private GameObject end;
    private UI UI;
    private Incubator incubator;


    private int[] startPos;
    private int[] currentPos;
    private int[] endPos;

    public int maxRoadBlocks;
    public int population = 0;
    public GameObject mapGO;
    private Map map;

    public Road road;

    // Use this for initialization
    void Start()
    {
        start = GameObject.FindGameObjectWithTag("Start");
        end = GameObject.FindGameObjectWithTag("End");
        map = mapGO.GetComponent<Map>();
        incubator = GameObject.Find("Incubator").GetComponent<Incubator>();
        UI = GameObject.Find("Canvas").GetComponent<UI>();


        for (int i = 0; i < NUMBER_OF_ROADS_IN_GENERATION; i++)
        {
            GameObject roadGO = new GameObject("Road " + (i+1).ToString());

            road = roadGO.AddComponent<Road>();

            BuildRoad();

            incubator.addRoadToIncubator(road);

            population++;
            UI.showPopulation(population);

            resetMapStates();
        }
        UI.showListOfRoads();
    }

    public void BuildRoad()
    {
        setUpStartCoordinates(); // Sets up current position at the start positon
        addBlockAtCurrentPos(); // Creating the block at starting position

        for (int i = 0; i < maxRoadBlocks + 1; i++) // Maximum road blocks to build are set in editor
        {
            List<Node> nodeNeighbours = findNodeNeighboursOfCurrentPos();

            if (nodeNeighbours.Count > 0)
            {
                Node selectedNode = findRandomNode(nodeNeighbours);

                currentPos = selectedNode.GetComponent<Node>().getCoordinates();
                addBlockAtCurrentPos();

            }
            else
                i = maxRoadBlocks + 1;
        }

    }

    private void setUpStartCoordinates()
    {
        startPos = start.GetComponent<Node>().getCoordinates();
        currentPos = startPos;
    }
    private void addBlock(int x, int y)
    {
        if (nodeIsEmpty(x, y))
        {
            road.RoadBlocks.Add( new int[] { x, y } );
            road.incrementRoadLength();
            map.map[x, y].GetComponent<Node>().setNode();
        }
    }
    private void addBlockAtCurrentPos()
    {
        addBlock(currentPos[X], currentPos[Y]);
    }
    private bool nodeIsEmpty(int x, int y)
    {
        return map.map[x, y].GetComponent<Node>().isEmpty;
    }
    private List<Node> findNodeNeighbours(int x, int y)
    {
        List<Node> neighbours = new List<Node>();

        if (findLeftNode(x, y) != null)
            neighbours.Add(findLeftNode(x, y));

        if (findTopNode(x, y) != null)
            neighbours.Add(findTopNode(x, y));

        if (findRightNode(x, y) != null)
            neighbours.Add(findRightNode(x, y));

        if (findBottomNode(x, y) != null)
            neighbours.Add(findBottomNode(x, y));

        return neighbours;
    }
    private List<Node> findNodeNeighboursOfCurrentPos()
    {
        return findNodeNeighbours(currentPos[X], currentPos[Y]);
    }
    private Node findLeftNode(int x, int y)
    {
        if (x - 1 >= 0)
        {
            if (nodeIsEmpty(x - 1, y))
            {
                return map.map[x - 1, y].GetComponent<Node>();
            }
            else
                return null;
        }
        else
            return null;
    }
    private Node findTopNode(int x, int y)
    {
        if (y - 1 >= 0)
        {
            if (nodeIsEmpty(x, y - 1))
            {
                return map.map[x, y - 1].GetComponent<Node>();
            }
            else
                return null;
        }
        else
            return null;
    }
    private Node findRightNode(int x, int y)
    {
        if (x + 1 < map.width)
        {
            if (nodeIsEmpty(x + 1, y))
            {
                return map.map[x + 1, y].GetComponent<Node>();
            }
            else
                return null;
        }
        else
            return null;
    }
    private Node findBottomNode(int x, int y)
    {
        if (y + 1 < map.length)
        {
            if (nodeIsEmpty(x, y + 1))
            {
                return map.map[x, y + 1].GetComponent<Node>();
            }
            else
                return null;
        }
        else
            return null;
    }
    private Node findRandomNode(List<Node> nodes)
    {
        int randomIndex = UnityEngine.Random.Range(0, nodes.Count);
        Node selectedNode = nodes[randomIndex];
        return selectedNode;
    }
    private void resetMapStates()
    {

        foreach (GameObject nodeGO in map.map)
        {
            Node node = nodeGO.GetComponent<Node>();
            node.freeNode();
        }
    }

}
