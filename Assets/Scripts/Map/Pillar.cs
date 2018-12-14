using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour {

    public Transform MainLand, material;
    public Vector2 rangeX, rangeZ, rangeHeight;
    public int quantity;
    // Use this for initialization
    void Start()
    {
        for (float i = 0; i < quantity; i++)
        {
            float PosX = Mathf.Floor(Random.Range(rangeX.x, rangeX.y)) + 0.5f;
            float PosZ = Mathf.Floor(Random.Range(rangeZ.x, rangeZ.y)) + 0.5f;
            for (float j = (int)Random.Range(rangeHeight.x, rangeHeight.y) + 0.5f; j > 0; j--)
            {
                Transform forest = Instantiate(material);
                forest.transform.position = new Vector3(PosX, j - 0.2f, PosZ);
                //get -0.2 from rch.point.y at ground from game, don't know how to get it in code//
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
