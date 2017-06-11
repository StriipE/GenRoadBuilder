using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public enum Direction
{
    RIGHT,
    UP,
    LEFT,
    DOWN,
    NO_DIRECTION
};

public class GenRoadBuilder : MonoBehaviour
{
    private const int X = 0;
    private const int Y = 1;

    private GameObject start;
    private GameObject end;

    private int[] startPos;
    private int[] currentPos;
    private int[] endPos;

    private Direction previousDirection = Direction.NO_DIRECTION;
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
                    Direction nextDirection = getDirectionOfNextNode( selectedNode );
                    setMaterialFromNextDirection(nextDirection);
                    previousDirection = nextDirection;
                    currentPos = selectedNode.GetComponent<Node>().getCoordinates();
                    addBlockAtCurrentPos();
                    yield return new WaitForSeconds(0.2f);
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
        startPos = start.GetComponent<Node>().getCoordinates();
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
    private Direction getDirectionOfNextNode(Node nextNode)
    {
        int[] nextCoordinates;
        Direction directionOfNextNode = Direction.NO_DIRECTION;

        nextCoordinates = nextNode.getCoordinates();

        int differenceX = nextCoordinates[X] - currentPos[X];
        int differenceY = nextCoordinates[Y] - currentPos[Y];

        if (differenceX == 1)
            directionOfNextNode = Direction.RIGHT;
        else if (differenceX == -1)
            directionOfNextNode = Direction.LEFT;
        else if (differenceY == 1)
            directionOfNextNode = Direction.UP;
        else if (differenceY == -1)
            directionOfNextNode = Direction.DOWN;

        return directionOfNextNode;
    }

    private void setMaterialFromNextDirection(Direction nextDirection)
    {
        Material material;

        if (previousDirection == nextDirection)
        {
            material = Resources.Load("Materials/Straight", typeof(Material)) as Material;
            road[currentPos[X], currentPos[Y]].GetComponent<Renderer>().material = material;

            if (nextDirection == Direction.UP || nextDirection == Direction.DOWN)
                rotateRoadAtCurrentPos(90);

        }
        else
        {
            material = Resources.Load("Materials/Turn", typeof(Material)) as Material;
            road[currentPos[X], currentPos[Y]].GetComponent<Renderer>().material = material;

            if (previousDirection == Direction.LEFT)
            {
                if (nextDirection == Direction.DOWN)
                    rotateRoadAtCurrentPos(90);
                else if (nextDirection == Direction.UP)
                    rotateRoadAtCurrentPos(0);
            }
            else if (previousDirection == Direction.UP)
            {
                if (nextDirection == Direction.LEFT)
                    rotateRoadAtCurrentPos(180);
                else if (nextDirection == Direction.RIGHT)
                    rotateRoadAtCurrentPos(90);
            }
            else if (previousDirection == Direction.RIGHT)
            {
                if (nextDirection == Direction.UP)
                    rotateRoadAtCurrentPos(-90);
                else if (nextDirection == Direction.DOWN)
                    rotateRoadAtCurrentPos(180);
            }
            else if (previousDirection == Direction.DOWN)
            {
                if (nextDirection == Direction.RIGHT)
                    rotateRoadAtCurrentPos(0);
                else if (nextDirection == Direction.LEFT)
                    rotateRoadAtCurrentPos(-90);
            }
        }

    }

    public void rotateRoadAtCurrentPos(int degrees)
    {
        road[currentPos[X], currentPos[Y]].transform.Rotate(new Vector3(0, degrees, 0));
    }
}
