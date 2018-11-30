using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Controller : MonoBehaviour {
    [DllImport("user32")] // 使用 user32.dll ，這是系統的 Dll 檔，所以Unity會自動匯入，不用再手動加入 dll 檔
    static extern bool SetCursorPos(int X, int Y);

    public float speed, jump, sensitivity;
    Dictionary<string, int> material = new Dictionary<string, int>()
    {
        {"brick",       backpack.brick},
        {"cobblestone", backpack.cobblestone},
        {"diamond_ore", backpack.diamond_ore},
        {"dirt",        backpack.dirt},
        {"gold_ore",    backpack.gold_ore},
        {"grass",       backpack.grass},
        {"iron_ore",    backpack.iron_ore},
        {"log_oak",     backpack.log_oak},
        {"planks_oak",  backpack.planks_oak},
        {"stone",       backpack.stone},
        {"sand(Clone)", backpack.sand},
    };

    struct Backpack
    {
        public int brick;
        public int cobblestone;
        public int diamond_ore;
        public int dirt;
        public int gold_ore;
        public int grass;
        public int iron_ore;
        public int log_oak;
        public int planks_oak;
        public int stone;
        public int sand;
    }
    static Backpack backpack = new Backpack();
    
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

        //camera angle
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
       // Debug.Log(transform.GetChild(0).eulerAngles.x);
       // Debug.Log(transform.GetChild(0).localRotation.x);


        //take and place block
        if (Input.GetMouseButton(0))
        {
            Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch;
            if (Physics.Raycast(ra, out rch))
                if (rch.collider.gameObject.tag == "Block")
                {
                    string[] separator = { "(Clone)" };
                    var key = rch.collider.gameObject.name.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)[0];
                    material[key]++;
                    Destroy(rch.transform.gameObject);
                    Debug.Log(key);
                }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch;
            if (Physics.Raycast(ra, out rch))
                if (rch.collider.gameObject.tag == "Ground")
                {/*
                    Transform d = Instantiate(dirt);
                    d.parent = transform;
                    d.localScale = new Vector3(100, 2, 2);
                    d.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 10f);
                    d.localPosition = new Vector3(Random.Range(-50, 50), Random.Range(0, 100), Random.Range(-50, 50));
                    d.rotation = Random.rotation;
                    d = Instantiate(dirt);*/
                }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground" || (col.gameObject.tag == "Block" && col.gameObject.transform.position.y + col.gameObject.transform.localScale.y / 2 < gameObject.transform.position.y))
            onGround = true;
    }

}
