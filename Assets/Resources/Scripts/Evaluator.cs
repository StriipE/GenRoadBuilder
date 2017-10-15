using System;
using UnityEngine;

public class Evaluator : MonoBehaviour {

    public Map map;
    public int EvaluationScore { get; set; }
    public int[] endPosition { get; set; }
    
    public void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
    }                              

    public int evaluateRoad(Road road)
    {
        int score = 0;
        endPosition = getBlockCoordinates(map.endBlock);

        foreach (int[] position in road.RoadBlocks)
        {
            score -= calculateDistanceToEnd(position);
            if (isCheckpoint(position))
                score += 1000;
        }


        return score;
    }

    private int[] getBlockCoordinates(GameObject block)
    {
        return new int[2] { Convert.ToInt32(block.transform.position.x / 4.5f), Convert.ToInt32(block.transform.position.z / 4.5f) };
    }

    private bool isCheckpoint(int[] position)
    {
        foreach (GameObject checkpoint in map.checkpoints)
        {
            if (getBlockCoordinates(checkpoint) == position)
                return true;
        }

        return false;
    }
    private int calculateDistanceToEnd(int[] position)
    {
        // Square root of a² + b² for distance
        return Convert.ToInt32(Math.Sqrt(Math.Pow((position[0] - endPosition[0]), 2d) + Math.Pow((position[1] - endPosition[1]), 2d)));
    }
}
