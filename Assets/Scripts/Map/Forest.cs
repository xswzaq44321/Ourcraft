using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour {

    public Transform MainLand;
    public Vector2 rangeX, rangeZ, rangeHeight;
    // Use this for initialization
    void Start () {
        for (float i = 0; i < 100; i++)
        {
            float PosX = Random.Range(rangeX.x, rangeX.y) + 0.5f, PosY = Random.Range(rangeZ.x, rangeZ.y) + 0.5f;
            for (float j = Random.Range(rangeHeight.x, rangeHeight.y) + 0.5f; j > 0; j--)
            {
                GameObject forest = Instantiate(Resources.Load("blocks/log_oak") as GameObject);
                forest.transform.position = new Vector3(PosX, j + MainLand.transform.position.y, PosY);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
