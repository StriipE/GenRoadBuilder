using UnityEngine;

public class GenRoadBuilder : MonoBehaviour {

    private GameObject start;
    private GameObject end;

    public int maxRoadBlocks;
    public GameObject map;

	// Use this for initialization
	void Start () {
        start = GameObject.FindGameObjectWithTag("Start");
        end = GameObject.FindGameObjectWithTag("End");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildRoad()
    {

        for (int i = 0; i < maxRoadBlocks; i++)
        {

        }
    }


}
