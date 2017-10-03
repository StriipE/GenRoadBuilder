using UnityEngine;
using UnityEngine.EventSystems;

public enum Direction
{
    RIGHT,
    UP,
    LEFT,
    DOWN,
    NO_DIRECTION
};


public class RoadRenderer : MonoBehaviour
{
    private const int X = 0;
    private const int Y = 1;

    private Direction previousDirection = Direction.NO_DIRECTION;
    private int[] currentPos;
    private GameObject currentNode;

    // TODO : RenderRoad has to render a block according to the previous block
    public void renderRoad()
    {
        Road road = GameObject.Find("Road 1").GetComponent<Road>();
        currentPos = new int[] { road.RoadBlocks[0][X], road.RoadBlocks[0][Y] };
        currentNode = (GameObject)Instantiate(Resources.Load(@"Prefabs/Road"), 
                        new Vector3(4.5f * road.RoadBlocks[0][X], 0.5f, 4.5f * road.RoadBlocks[0][Y]), Quaternion.identity);

        for (int i = 0; i < road.RoadBlocks.Count; i++)
        {
            int x = road.RoadBlocks[i][X];
            int y = road.RoadBlocks[i][Y];            
            int[] nextCoordinates = new int[] { x, y };

            renderMaterialOnNextNode(nextCoordinates);
            currentNode = (GameObject)Instantiate(Resources.Load(@"Prefabs/Road"), new Vector3(4.5f * x, 0.5f, 4.5f * y), Quaternion.identity);
            currentPos = nextCoordinates;
        }

        renderMaterialOnNextNode(currentPos);
    }

    // Compares the next coordinates and the current block coordinates to find the next direction
    private Direction getDirectionOfNextNode(int[] nextCoordinates)
    {
        Direction directionOfNextNode = Direction.NO_DIRECTION;

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
            currentNode.GetComponent<Renderer>().material = material;

            if (nextDirection == Direction.UP || nextDirection == Direction.DOWN)
                rotateRoadAtCurrentPos(90);

        }
        else
        {
            material = Resources.Load("Materials/Turn", typeof(Material)) as Material;
            currentNode.GetComponent<Renderer>().material = material;

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
        currentNode.transform.Rotate(new Vector3(0, degrees, 0));
    }

    public void renderMaterialOnNextNode(int[] nextCoordinates)
    {
        Direction nextDirection = getDirectionOfNextNode(nextCoordinates);
        setMaterialFromNextDirection(nextDirection);
        previousDirection = nextDirection;
    }


}

