using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	public float walk_speed, run_speed, jump, sensitivity, atk_range, heal_speed;
	public readonly int MAX_HP = 20;
	public GameObject health_bar;
	public Canvas consoleCanvas;
	public Canvas mainCanvas;
	public Canvas deadCanvas;
	private int HP = 0;
	private bool onGround = false;
	private Animator an;
	private float trigger_time = 1, speed, heal_time = 0;

	// Use this for initialization
	void Start()
	{
        Cursor.visible = false;
		speed = walk_speed;
		Add_HP(MAX_HP);
		an = GetComponent<Animator>();
		GetComponents<AudioSource>()[0].Stop();
		GetComponents<AudioSource>()[1].Stop();
		GetComponents<AudioSource>()[2].Stop();
		consoleCanvas.enabled = false;
		deadCanvas.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		heal_time += heal_speed * Time.deltaTime;
		if (heal_time >= 25)
		{
			heal_time = 0;
			Add_HP(1);
		}

		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			if (consoleCanvas.enabled)
			{
				consoleCanvas.enabled = false;
				mainCanvas.enabled = true;
				consoleCanvas.GetComponent<LuaConsoleControll>().disable();
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				consoleCanvas.enabled = true;
				mainCanvas.enabled = false;
				consoleCanvas.GetComponent<LuaConsoleControll>().enable();
			}
		}
		if (!consoleCanvas.enabled && HP > 0)
		{
			handleMove();
		}
	}

	void handleMove()
	{
		Cursor.lockState = CursorLockMode.None;

        //to menu, don't want it to work when editing//
#if !UNITY_EDITOR
		if (Input.GetKey(KeyCode.Escape))
		{
            Cursor.visible = true;
			SceneManager.LoadScene(0);
			return;
		}
#endif

        //character moving + jumping//
        an.SetFloat("speed", 0);
		trigger_time += Time.deltaTime;
		if (speed == run_speed && Input.GetKeyUp(KeyCode.W))
		{
			speed = walk_speed;
			GetComponents<AudioSource>()[0].pitch = 1;
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (speed == walk_speed && trigger_time <= 0.5f)
			{
				speed = run_speed;
				GetComponents<AudioSource>()[0].pitch = 1.3f;
			}
			else trigger_time = 0;
		}
		if (Input.GetKey(KeyCode.W))
		{
			transform.localPosition += speed * transform.forward * Time.deltaTime;
			an.SetFloat("speed", speed);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			transform.localPosition -= speed * transform.forward * Time.deltaTime;
			an.SetFloat("speed", -speed);
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.localPosition -= speed * transform.right * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.localPosition += speed * transform.right * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Space) && onGround)
		{
			GetComponent<Rigidbody>().AddForce(Vector3.up * jump);
			onGround = false;
		}

		//camera angle//
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0) * Time.deltaTime, Space.World);
		float max = transform.GetChild(0).eulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
		if (max >= 270 || max <= 90)
			transform.GetChild(0).Rotate(new Vector3(-Input.GetAxis("Mouse Y") * sensitivity, 0, 0) * Time.deltaTime);

		//attack monster//
		if (Input.GetMouseButtonDown(0))
		{
			Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rch;
			if (Physics.Raycast(ra, out rch) && rch.collider.gameObject.tag == "Monster"
				&& Vector3.Distance(rch.point, transform.position) <= atk_range)
			{
				rch.collider.gameObject.GetComponent<MonsterController>().addHP(-1);
				GetComponents<AudioSource>()[1].Play();
			}
		}

		Cursor.lockState = CursorLockMode.Locked;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Ground" || (col.gameObject.tag == "Block" && col.gameObject.transform.position.y + col.gameObject.transform.localScale.y / 2 < gameObject.transform.position.y))
			onGround = true;
	}

	public void Add_HP(int delataHP)
	{
		HP += delataHP;
		if (HP > MAX_HP) HP = MAX_HP;
		int i;
		for (i = 0; i < HP / 2; i++)
			health_bar.GetComponent<Transform>().GetChild(i).GetComponent<RawImage>().texture = Resources.Load("icon/heart") as Texture;
		if (HP % 2 == 1)
		{
			health_bar.GetComponent<Transform>().GetChild(i).GetComponent<RawImage>().texture = Resources.Load("icon/half_heart") as Texture;
			i++;
		}
		for (; i < MAX_HP / 2; i++)
			health_bar.GetComponent<Transform>().GetChild(i).GetComponent<RawImage>().texture = Resources.Load("icon/hollow_heart") as Texture;
		if (delataHP < 0)
		{
			GetComponent<Rigidbody>().AddForce((-transform.forward + transform.up) * 200);
			GetComponents<AudioSource>()[2].Play();
		}
		if (HP <= 0)
		{
            Cursor.visible = true;
            GameObject.Find("GameSaveReadObject").GetComponent<MapSaveRead>().saveMap(null);
			Cursor.lockState = CursorLockMode.None;
			mainCanvas.enabled = false;
			consoleCanvas.enabled = false;
			deadCanvas.enabled = true;
		}
	}

	public void set_HP(int health)
	{
		HP = 0;
		Add_HP(health);
	}

	public int get_HP()
	{
		return HP;
	}

}
