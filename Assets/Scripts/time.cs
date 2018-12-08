using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float time_flys = 100000;
    private float delta_time = 1.0f / 24000.0f;
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(delta_time * time_flys, 0, 0) * Time.deltaTime);
    }

    void set_time(int time)
    {
        Quaternion angle = new Quaternion();
        angle.eulerAngles = new Vector3(time * delta_time, 0, 0);
    }
}