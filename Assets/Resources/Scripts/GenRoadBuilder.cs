using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenRoadBuilder : MonoBehaviour
{
    private GameObject start;
    private GameObject end;

    private int[,] startPos;
    private int[,] currentPos;
    private int[,] endPos;

    public int maxRoadBlocks;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator BuildRoad()
    {
       // while (true)
        {
            startPos = new int[1, 2] { { Convert.ToInt32(start.transform.position.x / 4.5f), Convert.ToInt32(start.transform.position.z / 4.5f) } };
            endPos = new int[1, 2] { { Convert.ToInt32(end.transform.position.x / 4.5f), Convert.ToInt32(end.transform.position.z / 4.5f) } };
            currentPos = startPos;

            addBlock(currentPos[0, 0], currentPos[0, 1]);

            for (int i = 0; i < maxRoadBlocks + 1; i++)
            {
                List<Node> nodeNeighbours = findNodeNeighbours(currentPos[0, 0], currentPos[0, 1]);
                Debug.Log(nodeNeighbours.Count);
                if (nodeNeighbours.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, nodeNeighbours.Count);
                    Node selectedNode = nodeNeighbours[randomIndex];
                    currentPos = new int[1, 2] { { Convert.ToInt32(selectedNode.transform.position.x / 4.5f), Convert.ToInt32(selectedNode.transform.position.z / 4.5f) } };
                    addBlock(currentPos[0, 0], currentPos[0, 1]);
                    yield return new WaitForSeconds(0.05f);
                }
                else
                {
                    destroyRoad();
                    i = maxRoadBlocks;
                }
            }

        }
    }

    public void addBlock(int x, int y)
    {
        if (nodeIsEmpty(x, y))
        {
            road[x, y] = (GameObject)Instantiate(Resources.Load(@"Prefabs/Road"), new Vector3(4.5f * x, 0.5f, 4.5f * y), Quaternion.identity);
            map.map[x, y].GetComponent<Node>().setNode();
        }
    }

    public bool nodeIsEmpty(int x, int y)
    {
        return map.map[x, y].GetComponent<Node>().isEmpty;
    }

    public List<Node> findNodeNeighbours(int x, int y)
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

    public Node findLeftNode(int x, int y)
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
    public Node findTopNode(int x, int y)
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
    public Node findRightNode(int x, int y)
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
    public Node findBottomNode(int x, int y)
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

    public void destroyRoad()
    {
        var roadBlocks = GameObject.FindGameObjectsWithTag("Road");

        for (var i = 0; i < roadBlocks.Length; i++)
        {
            Destroy(roadBlocks[i]);
        }
    }
}
