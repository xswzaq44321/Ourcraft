using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGen : MonoBehaviour
{

	public GameObject player;
	public Light sun;
	public GameObject monster;
	public float minRange, maxRange;
	public Transform MainLand;
	public float generateInterval;
	public int maxCount;
	private float elapseTime;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 dist = new Vector3(1, 0, 0);
		Vector3 endPoint;
		elapseTime += Time.deltaTime * sun.GetComponent<time>().delta_time;
		if (time.interval(sun.GetComponent<time>().world_time, 18000, 6000) && elapseTime > generateInterval
            && this.transform.childCount < maxCount)
		{ // night time
			elapseTime = 0;
			// decide where and how far from player that the monsters'll spawn
			dist *= Random.Range(minRange, maxRange);
			dist = Quaternion.Euler(0, Random.Range(0, 360), 0) * dist;
			endPoint = player.transform.position + dist;
			// spawn only on ground
			endPoint.y = MainLand.transform.position.y;
			GameObject enemy = Instantiate(monster);
			enemy.transform.parent = this.transform;
			enemy.transform.position = endPoint;
            enemy.transform.Rotate(0, Random.Range(0, 259), 0);
			// target lock to player
			enemy.GetComponent<MonsterController>().player = player;
		}
		else if(time.interval(sun.GetComponent<time>().world_time, 6000, 18000))
		{ // day time
			if (this.transform.childCount > 0)
			{
				for (int i = 0; i < 10 && i < this.transform.childCount; i++)
				{
					Destroy(this.transform.GetChild(0).gameObject);
					this.transform.GetChild(0).parent = null; 
				}
			}
		}
	}
}
