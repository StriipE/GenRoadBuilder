using UnityEngine;

public class Map : MonoBehaviour {

    public int width;
    public int length;

    public GameObject[ , ] map;

  
    // Use this for initialization
    void Start () {

        createMap(width, length);
        addStartBlock(13, 3);
        addEndBlock(11, 13);
        addCheckpointBlock(2, 6);
        addCheckpointBlock(5, 11);

    }
    
    private void createMap (int width, int length)
    {
        map = new GameObject[width, length];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                map[i, j] = (GameObject)Instantiate(Resources.Load(@"Prefabs/Node"), new Vector3(4.5f * i, 0, 4.5f * j), Quaternion.identity);
            }
        }
    }
    private void addStartBlock(int xPos, int yPos)
    {
        Destroy(map[xPos, yPos]);
        map[xPos, yPos] = (GameObject)Instantiate(Resources.Load(@"Prefabs/Start"), new Vector3(4.5f * xPos, 0, 4.5f * yPos), Quaternion.identity);
    }
    private void addEndBlock(int xPos, int yPos)
    {
        Destroy(map[xPos, yPos]);
        map[xPos, yPos] = (GameObject)Instantiate(Resources.Load(@"Prefabs/End"), new Vector3(4.5f * xPos, 0, 4.5f * yPos), Quaternion.identity);
    }
    private void addCheckpointBlock(int xPos, int yPos)
    {
        Destroy(map[xPos, yPos]);
        map[xPos, yPos] = (GameObject)Instantiate(Resources.Load(@"Prefabs/Checkpoint"), new Vector3(4.5f * xPos, 0, 4.5f * yPos), Quaternion.identity);
    }
    
    // Update is called once per frame
    void Update () {
		
	}
}
