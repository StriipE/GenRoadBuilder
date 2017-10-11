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
           score += calculateDistanceToEnd(position);
        }


        return score;
    }

    private int[] getBlockCoordinates(GameObject block)
    {
        int[] coordinates = new int[2];


        return coordinates;
    }
    private int calculateDistanceToEnd(int[] position)
    {
        // Square root of a² + b² for distance
        return Convert.ToInt32(Math.Sqrt(Math.Pow((position[0] - endPosition[0]), 2d) + Math.Pow((position[1] - endPosition[1]), 2d)));
    }
}
