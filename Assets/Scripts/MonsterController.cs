using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

	public GameObject player;
	public float speed;
	private Animator an;
	private int MAX_HP;
	private int HP;

	// Use this for initialization
	void Start () {
		an = GetComponent<Animator>();
		an.speed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		chase();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "player")
		{
			collision.gameObject.GetComponent<Controller>().Add_HP(-1);
		}
	}

	void chase()
	{
		Vector3 endPoint = player.transform.position;
		GetComponent<Rigidbody>().velocity = (endPoint - transform.position).normalized * speed;
		an.SetFloat("speed", speed);
		transform.forward = (endPoint - transform.position);
	}

	public void addHP(int deltaHP)
	{
		HP += deltaHP;
		if (HP > MAX_HP)
			HP = MAX_HP;
		else if (HP < 0)
			Destroy(this);
		GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up)*200);
	}
}
