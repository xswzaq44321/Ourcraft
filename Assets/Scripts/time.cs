using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour {

	// Use this for initialization
	void Start () {
        set_time(8000);
	}

    public bool pause;
    public float world_time;
    public float delta_time = 20; // real time 1sec == 20 (ourcraft 1 day == real time 1200sec) 
    public static float world_day = 24000;
	// Update is called once per frame
	void Update () {
        if (!pause)
        {
            world_time += delta_time * Time.deltaTime;
            if (world_time >= world_day) world_time = 0;
            set_time(world_time);
        }
    }

    //set time between 0 - 24000 (24hr)
    public void set_time(float time)
    {
        world_time = add(0, time);
        Quaternion angle = new Quaternion();
        angle.eulerAngles = new Vector3((world_time - 6000) / world_day * 360.0f, 0, 0);
        transform.rotation = angle;
    }

	// return time for saving purposes
	public float get_time()
	{
		return world_time;
	}

    //maxtime = world_day
    static public float add(float timeA, float timeB)
    {
        float timeC = timeA + timeB;
        while (timeC >= world_day) timeC -= world_day;
        while (timeC < 0) timeC += world_day;
        return timeC;
    }

    //0 <= begin, end < world_day
    static public bool interval(float now, float begin, float end)
    {
        if (begin < 0 || end >= world_day) return false;
        else if (end < begin)
        {
            if ((now >= begin && now < world_day) || (now >= 0 && now <= end)) return true;
            else return false;
        }
        else
        {
            if (now >= begin && now <= end) return true;
            else return false;
        }
    }
}