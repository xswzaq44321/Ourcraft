using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

    public float speed, jump, sensitivity;
    private bool onGround = false;

    // Use this for initialization
    void Start () {
        Quaternion init_angle = Quaternion.identity;
        init_angle.eulerAngles = new Vector3(1, 0, 0);
        transform.GetChild(0).rotation = init_angle;
    }

	// Update is called once per frame
	void Update () {
        Cursor.lockState = CursorLockMode.None;

        //to menu//
        if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene(0);

        //character moving + jumping//
        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += speed * transform.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition -= speed * transform.right * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition -= speed * transform.forward * Time.deltaTime;
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



}
