using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour {

    public Transform MainLand;
    private string[] material = new string[] { "grass", "sand", "dirt", "stone", "log_oak"};
	// Use this for initialization
	void Start () {
        for (float i = 0; i < 100; i++)
        {
            float PosX = Random.Range(-50, 50) + 0.5f, PosY = Random.Range(-50, 50) + 0.5f;
            for (float j = Random.Range(3, 40) + 0.5f; j > 0; j--)
            {
                GameObject forest = Instantiate(Resources.Load("blocks/log_oak") as GameObject);
                forest.transform.position = new Vector3(PosX, j + MainLand.transform.position.y, PosY);
            }
        }


        /* // big floor
		for(float i = -30.5f; i < 30; i++)
            for(float j = -30.5f; j < 30; j++)
            {
                GameObject block = Instantiate(Resources.Load("blocks/" + material[Random.Range(0, 3)]) as GameObject);
                block.transform.position = new Vector3(i, 0.3238416f, j);
            }*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
