using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public float speed, jump, sensitivity;
    public readonly int MAX_HP = 20;
    public GameObject health_bar;
    private int HP = 0;
    private bool onGround = false;
	private Animator an;

    // Use this for initialization
    void Start () {
        Quaternion init_angle = Quaternion.identity;
        init_angle.eulerAngles = new Vector3(1, 0, 0);
        transform.GetChild(0).rotation = init_angle;
        Add_HP(MAX_HP);
		an = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
        Cursor.lockState = CursorLockMode.None;

		//to menu//
		//if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene(0);

		//character moving + jumping//
		an.SetFloat("speed", 0);
        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += speed * transform.forward * Time.deltaTime;
			an.SetFloat("speed", speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition -= speed * transform.right * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition -= speed * transform.forward * Time.deltaTime;
			an.SetFloat("speed", -speed);
		}
		if (Input.GetKey(KeyCode.D))
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
        if(HP % 2 == 1)
        {
            health_bar.GetComponent<Transform>().GetChild(i).GetComponent<RawImage>().texture = Resources.Load("icon/half_heart") as Texture;
            i++;
        }
        for(; i < MAX_HP / 2; i++)
            health_bar.GetComponent<Transform>().GetChild(i).GetComponent<RawImage>().texture = Resources.Load("icon/hollow_heart") as Texture;
		if(delataHP < 0)
			GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up)*200);
	}

}
