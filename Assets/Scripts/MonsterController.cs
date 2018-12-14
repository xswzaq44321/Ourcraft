using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

	public GameObject player;
	public float speed, sight_distance, sight_angle;
	public int MAX_HP;
	private Animator an;
	private int HP;
    AudioSource arrr;
    private float arrr_delay = 0;

    // Use this for initialization
    void Start () {
        arrr = GetComponents<AudioSource>()[1];
        an = GetComponent<Animator>();
		an.speed = 1/3f * speed;
		HP = MAX_HP;
	}
	
	// Update is called once per frame
	void Update () {
        arrr_delay += Time.deltaTime;
        if (Vector3.Distance(player.transform.position, this.transform.position) <= sight_distance
            && Vector3.Angle(player.transform.position - this.transform.position, this.transform.forward) <= sight_angle / 2)
            chase();
        else if (Vector3.Distance(player.transform.position, this.transform.position) <= 3)
            chase();
        else walk();
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
        arrr_delay = 0;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (arrr_delay > 10)
        {
            arrr_delay = 0;
            arrr.Play();
        }
        if (collision.gameObject.name == "player")
        {
            arrr.volume = (10 - Vector3.Distance(transform.position, collision.gameObject.transform.position)) / 10;
            float angle = Vector3.Angle(transform.position - player.transform.position, player.transform.forward);
            arrr.panStereo = (90 - Mathf.Abs(90 - angle)) / 90;
            if (Vector3.Dot(transform.position - player.transform.position, player.transform.right) < 0)
                arrr.panStereo *= -1;
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

    void walk()
    {
        transform.forward += transform.right / 10 * Time.deltaTime;
        transform.position += transform.forward * speed * Time.deltaTime;
        an.SetFloat("speed", speed);
    }

	public void addHP(int deltaHP)
	{
        var bar = player.transform.position - transform.position;
        bar.y = 0;
        transform.forward = bar;
		HP += deltaHP;
        if (HP > MAX_HP)
            HP = MAX_HP;
        else if (HP <= 0)
            Destroy(this.gameObject);
		GetComponent<Rigidbody>().AddForce((-transform.forward + transform.up)*200);
	}
}
