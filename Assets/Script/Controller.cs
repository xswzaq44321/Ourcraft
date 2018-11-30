﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Controller : MonoBehaviour {
    [DllImport("user32")] // 使用 user32.dll ，這是系統的 Dll 檔，所以Unity會自動匯入，不用再手動加入 dll 檔
    static extern bool SetCursorPos(int X, int Y);

    // Use this for initialization
    void Start () {
        Quaternion init_angle = Quaternion.identity;
        init_angle.eulerAngles = new Vector3(1, 0, 0);
        transform.GetChild(0).rotation = init_angle; 
        //Cursor.visible = false;
    }

    private bool onGround = false;
	// Update is called once per frame
	void Update () {
        //character moving + jumping
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

        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0) * Time.deltaTime, Space.World);
        transform.GetChild(0).Rotate(new Vector3(-Input.GetAxis("Mouse Y") * sensitivity, 0, 0) * Time.deltaTime);

       // SetCursorPos(Screen.height, Screen.width);
       /*if (!(transform.GetChild(0).eulerAngles.x >= 280 && transform.GetChild(0).eulerAngles.x <= 360) || !(transform.GetChild(0).eulerAngles.x >= 0 && transform.GetChild(0).eulerAngles.x <= 89))
        {
            if (transform.GetChild(0).eulerAngles.x < 270 && !(transform.GetChild(0).eulerAngles.x > 0 && transform.GetChild(0).eulerAngles.x < 90))
            {
                Quaternion max_angle = Quaternion.identity;
                max_angle.eulerAngles = new Vector3(271, 0, 0);
                transform.GetChild(0).rotation = max_angle;
            }
            else
            {
                Quaternion max_angle = Quaternion.identity;
                max_angle.eulerAngles = new Vector3(89, 0, 0);
                transform.GetChild(0).rotation = max_angle;
            }
        }*/
        Debug.Log(transform.GetChild(0).eulerAngles.x);
        Debug.Log(transform.GetChild(0).localRotation.x);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
            onGround = true;
    }

    public float speed, jump, sensitivity;
}
