using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

	public GameObject player;
	public float speed;
	private Animator an;
	public int MAX_HP;
	private int HP;
    AudioSource arrr;

    // Use this for initialization
    void Start () {
        arrr = GetComponents<AudioSource>()[1];
        an = GetComponent<Animator>();
		an.speed = 1/3f * speed;
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "player") arrr.Play();
    }

    private void OnTriggerStay(Collider collision)
    {
         if (collision.gameObject.name == "player")
        {
            arrr.volume = (5 - Vector3.Distance(transform.position, collision.gameObject.transform.position)) / 5;
           float angle = Vector3.Angle(transform.right, collision.gameObject.transform.position);
           if (angle <= 45)
               arrr.panStereo = (45 - angle) / 45;
           else
           {
               angle = Vector3.Angle(-transform.right, collision.gameObject.transform.position);
               arrr.panStereo = (45 - angle) / 45;
           }
        }
    }

   // private void OnTriggerExit(Collider collision)
   // {
   //     if (collision.gameObject.name == "player") arrr.Stop();
   // }

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
