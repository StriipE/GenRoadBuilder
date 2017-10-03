using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private const int VERTICAL_BUTTON_OFFSET = -32;
    private const int FIRST_BUTTON_OFFSET = 16;

    public void showPopulation(int population)
    {
        GameObject populationGO = GameObject.Find("Population");
        Text populationText = populationGO.GetComponent<Text>();
        populationText.text = population.ToString();
    }

    public void showListOfRoads()
    {
        Incubator incubator = GameObject.Find("Incubator").GetComponent<Incubator>();
        GameObject mapsSV = GameObject.Find("MapsSVContent");

        mapsSV.GetComponent<RectTransform>().sizeDelta = 
                        new Vector2(0, incubator.generatedRoads.Count * -VERTICAL_BUTTON_OFFSET);

        int roadNumber = 0;

        foreach (Road road in incubator.generatedRoads)
        {
            roadNumber++;
            GameObject SVButtonGO = (GameObject)Instantiate(Resources.Load(@"Prefabs/RoadButton"), 
                                    new Vector3(0, roadNumber * VERTICAL_BUTTON_OFFSET + FIRST_BUTTON_OFFSET, 0), Quaternion.identity);

            SVButtonGO.transform.GetChild(0).GetComponent<Text>().text = road.gameObject.name;
            SVButtonGO.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            SVButtonGO.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);

            SVButtonGO.transform.SetParent(mapsSV.transform, false);
        }
    }
}

