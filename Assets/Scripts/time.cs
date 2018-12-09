using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //set_time(24000);
	}

    public float world_time = 8000;
    public float delta_time;
    private float world_day = 24000;
	// Update is called once per frame
	void Update () {
        world_time += delta_time * Time.deltaTime;
        if (world_time >= world_day) world_time = 0;
        set_time(world_time);
        //transform.Rotate(new Vector3(delta_time * time_flys, 0, 0) * Time.deltaTime);
    }

    void set_time(float time)
    {
        world_time = time;
        Quaternion angle = new Quaternion();
        angle.eulerAngles = new Vector3((world_time - 6000) / world_day * 360.0f, 0, 0);
        transform.rotation = angle;
    }
}