using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

    public Transform MainLand;
    private List<string> material = new List<string>();
    public bool grass, sand, dirt, stone;
    public Vector2 rangeX, rangeZ;
    public float altitude;

    // Use this for initialization
    void Start () {
        if (grass) material.Add("grass");
        if (sand) material.Add("sand");
        if (dirt) material.Add("dirt");
        if (stone) material.Add("stone");
        for(float i = rangeX.x + 0.5f; material.Count > 0 &&  i < rangeX.y; i++)
            for (float j = rangeZ.x + 0.5f; j < rangeZ.y; j++)
            {
                GameObject block = Instantiate(Resources.Load("blocks/" + material[Random.Range(0, material.Count - 1)]) as GameObject);
                block.transform.position = new Vector3(i, 0.5f + altitude, j);
            }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
