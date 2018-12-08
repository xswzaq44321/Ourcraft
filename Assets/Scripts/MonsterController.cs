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
		HP = MAX_HP;
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
		an.SetFloat("speed", 0);
		Vector3 endPoint = player.transform.position;
		var bar = (endPoint - transform.position).normalized;
		bar.y = 0;
		transform.forward = bar;
		transform.localPosition += speed * transform.forward * Time.deltaTime;
		an.SetFloat("speed", speed);
	}

	public void addHP(int deltaHP)
	{
		HP += deltaHP;
        if (HP > MAX_HP)
            HP = MAX_HP;
        else if (HP <= 0)
            Destroy(this.gameObject);
		GetComponent<Rigidbody>().AddForce((-transform.forward + transform.up)*200);
	}
}
