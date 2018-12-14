using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameinfo : MonoBehaviour {

    private bool show_gameinfo = false;
    public GameObject gameinfo_ui;
    public Light daylight;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //show gameinfo//
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (show_gameinfo) show_gameinfo = false;
            else
            {
                show_gameinfo = true;
                gameinfo_ui.GetComponent<Transform>().GetChild(1).GetComponent<Text>().text = "Clicked Position: ";
                gameinfo_ui.GetComponent<Transform>().GetChild(2).GetComponent<Text>().text = "Distance: ";
            }
        }

        if (show_gameinfo)
        {
            gameinfo_ui.GetComponent<Transform>().GetChild(0).GetComponent<Text>().text = "Player Position: " + transform.position.ToString();
            gameinfo_ui.GetComponent<Transform>().GetChild(3).GetComponent<Text>().text = "Time: " + daylight.GetComponent<time>().world_time.ToString();
            gameinfo_ui.GetComponent<Transform>().GetChild(4).GetComponent<Text>().text = "FPS: " + (1 / Time.deltaTime).ToString();
        }
        else
        {
            for (int i = 0; i < gameinfo_ui.GetComponent<Transform>().childCount; i++)
                gameinfo_ui.GetComponent<Transform>().GetChild(i).GetComponent<Text>().text = "";
        }

        if (show_gameinfo && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch;
            if (Physics.Raycast(ra, out rch))
            {
                gameinfo_ui.GetComponent<Transform>().GetChild(1).GetComponent<Text>().text = "Clicked Position: " + rch.point.ToString();
                gameinfo_ui.GetComponent<Transform>().GetChild(2).GetComponent<Text>().text = "Distance: " + Vector3.Distance(rch.point, transform.position).ToString();
            }
        }
    }
}
