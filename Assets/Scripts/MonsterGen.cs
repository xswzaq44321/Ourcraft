using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGen : MonoBehaviour {

	public GameObject player;
	public Light sun;
	public GameObject monster;
	public float minRange, maxRange;
	public Transform MainLand;
	private float time;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dist = new Vector3(1, 0, 0);
		Vector3 endPoint;
		time += Time.deltaTime;
		if(sun.transform.eulerAngles.x > 270f && time > 1f)
		{
			Debug.Log("party time!");
			time = 0;
			dist *= Random.Range(minRange, maxRange);
			dist = Quaternion.Euler(0, Random.Range(0, 360), 0) * dist;
			endPoint = player.transform.position + dist;
			endPoint.y = MainLand.transform.position.y;
			GameObject enemy = Instantiate(monster);
			enemy.transform.parent = this.transform;
			enemy.GetComponent<MonsterController>().player = player;
			enemy.transform.position = endPoint;
		}
		else if(sun.transform.eulerAngles.x < 90)
		{
			for(int i = 0; i < this.transform.childCount; ++i)
			{
				Destroy(this.transform.GetChild(i).gameObject);
			}
			this.transform.DetachChildren();
		}
	}
}
