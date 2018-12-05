using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour {

    private string[] material = new string[] { "grass", "sand", "dirt", "stone" };
	// Use this for initialization
	void Start () {
		for(float i = -30.5f; i < 30; i++)
            for(float j = -30.5f; j < 30; j++)
            {
                GameObject block = Instantiate(Resources.Load("blocks/" + material[Random.Range(0, 3)]) as GameObject);
                block.transform.position = new Vector3(i, 0.3238416f, j);
            }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
