using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenRoadBuilder : MonoBehaviour
{
    private const int X = 0;
    private const int Y = 1;

    private GameObject start;
    private GameObject end;

    private int[] startPos;
    private int[] currentPos;
    private int[] endPos;

    public int maxRoadBlocks;
    private int population = 0;
    public GameObject mapGO;
    private Map map;

    public GameObject[,] road;

    // Use this for initialization
    void Start()
    {
        start = GameObject.FindGameObjectWithTag("Start");
        end = GameObject.FindGameObjectWithTag("End");

        map = mapGO.GetComponent<Map>();

        road = new GameObject[map.length, map.width];

        StartCoroutine(BuildRoad());
        BuildRoad();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator BuildRoad()
    {
        while (true)
        {
            setUpStartCoordinates(); // Sets up current position at the start positon
            addBlockAtCurrentPos(); // Creating the block at starting position

            for (int i = 0; i < maxRoadBlocks + 1; i++) // Maximum road blocks to build are set in editor
            {
                List<Node> nodeNeighbours = findNodeNeighboursOfCurrentPos();

                if (nodeNeighbours.Count > 0)
                {
                    Node selectedNode = findRandomNode(nodeNeighbours);
                    currentPos = findNodeCoordinates(selectedNode.GetComponent<Node>());
                    addBlockAtCurrentPos();
                    yield return new WaitForSeconds(0.001f);
                }
                else
                {
                    destroyRoad();
                    i = maxRoadBlocks + 1;
                }
            }

            population++;
            showPopulation();
            destroyRoad();
        }
    }

    private void setUpStartCoordinates()
    {
        startPos = findNodeCoordinates(start.GetComponent<Node>());
        currentPos = startPos;
    }
    private void addBlock(int x, int y)
    {
        if (nodeIsEmpty(x, y))
        {
            road[x, y] = (GameObject)Instantiate(Resources.Load(@"Prefabs/Road"), new Vector3(4.5f * x, 0.5f, 4.5f * y), Quaternion.identity);
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
            neighbours.Add(findLeftNode(x, y) );

        if (findTopNode(x, y) != null)
            neighbours.Add(findTopNode(x, y) );

        if (findRightNode(x, y) != null)
            neighbours.Add(findRightNode(x, y) );

        if (findBottomNode(x, y) != null)
            neighbours.Add(findBottomNode(x, y) );

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
    private int[] findNodeCoordinates(Node node)
    {
        return new int[2] { Convert.ToInt32(node.transform.position.x / 4.5f), Convert.ToInt32(node.transform.position.z / 4.5f) };
    }
    private void destroyRoad()
    {
        var roadBlocks = GameObject.FindGameObjectsWithTag("Road");

        for (var i = 0; i < roadBlocks.Length; i++)
        {
            GameObject block = roadBlocks[i];

            currentPos = new int[2] { Convert.ToInt32(block.transform.position.x / 4.5f), Convert.ToInt32(block.transform.position.z / 4.5f) };
            Node node = map.map[currentPos[0], currentPos[1]].GetComponent<Node>();
            node.freeNode();
            Destroy(roadBlocks[i]);
        }
    }
    public void showPopulation()
    {
        GameObject populationGO = GameObject.Find("Population");
        Text populationText = populationGO.GetComponent<Text>();
        populationText.text = population.ToString();
    }

}
