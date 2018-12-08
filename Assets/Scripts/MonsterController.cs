using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

	public GameObject player;
	public float speed;
	private Animator an;
	public int MAX_HP;
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
		var bar = (endPoint - transform.position).normalized;
		bar.y = 0;
		GetComponent<Rigidbody>().velocity = bar * speed;
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
		GetComponent<Rigidbody>().AddForce((-transform.forward + transform.up)*200);
	}
}
