using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Controller : MonoBehaviour {
    [DllImport("user32")] // 使用 user32.dll ，這是系統的 Dll 檔，所以Unity會自動匯入，不用再手動加入 dll 檔
    static extern bool SetCursorPos(int X, int Y);

    public float speed, jump, sensitivity;
    private int selector = 2;
    private bool[] backpack_space = new bool[9] {false, false, false, false, false, false, false, false, false};
    public Canvas itembox;

    //(a)// //we're trying a new method to store these blocks.......
 
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
        {"sand",        backpack.sand},
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
    //private Item[] space = new Item[9];
    private int MAX_ITEM = 11;
    struct Item
    {
        public string material;
        public int index, num;
    }
    Item[] space = new Item[]
    {
        new Item {material = "brick"      , index = 0, num = 0},
        new Item {material = "cobblestone", index = 0, num = 0},
        new Item {material = "diamond_ore", index = 0, num = 0},
        new Item {material = "dirt"       , index = 0, num = 0},
        new Item {material = "gold_ore"   , index = 0, num = 0},
        new Item {material = "grass"      , index = 0, num = 0},
        new Item {material = "iron_ore"   , index = 0, num = 0},
        new Item {material = "log_oak"    , index = 0, num = 0},
        new Item {material = "planks_oak" , index = 0, num = 0},
        new Item {material = "stone"      , index = 0, num = 0},
        new Item {material = "sand"       , index = 0, num = 0}
    };
    //(b)//List<Item> backpack; //we suck at list...

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

        if (Input.GetKey(KeyCode.Alpha1))
        {
            selector = 1;
            //itembox.transform.GetChild(1).position = new Vector3(-348.5f, -299, 0);
        }

            //camera angle
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0) * Time.deltaTime, Space.World);
        //float max = transform.GetChild(0).eulerAngles.x - Input.GetAxis("Mouse Y");
        //if ((max > 270 && max < 360) || (max > 0 && max < 90))
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
                    string[] separator = { "(Clone)", " (" };
                    var key = rch.collider.gameObject.name.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)[0];
                    //material[key]++;
                    //Debug.Log(material[key]);
                    //Debug.Log(key);
                    int id;
                    for (id = 0; id < space.Length && space[id].material != key; id++) ;
                    if (space[id].num == 0)
                    {
                        space[id].num++;
                        for (int i = 0; i < backpack_space.Length; i++)
                            if (backpack_space[i])
                            {
                                space[id].index = i;
                                backpack_space[i] = false;
                                break;
                            }
                    }
                    else space[id].num++;
                    Debug.Log(space[id].num);


                    /* // we don't know what's going on anymore....
                    int i;
                    for(i = 0; i < 9; i++)
                    {
                        if (key == space[i].mateiral)
                        {
                            space[i].num++;
                            break;
                        }
                    }
                    if(i >= 9)
                    {
                        for (i = 0; i < 9 && space[i].num != 0; i++) ;
                        space[i].mateiral = key;
                        space[i].num++;
                    }
                    Debug.Log(space[0]);
                    Debug.Log(space[1]);
                    */

                    /*//(a) WE can't even...
                    if (material[key].mateiral == key)
                        material[key].num++;
                    */

                    /* //(b)
                     * //WE wanna try Find(), but it doesn't seem like a good idea...
                    Item new_item = backpack.Find(stuff => stuff.mateiral == key);
                    if(new_item == null)
                    {
                    }
                    */

                    /*//(b)
                     * //and then WE try to find the item by myself, but ffs it fails...
                    Item new_block;
                    for(int i = 0; i < backpack.Count; i++)
                    {
                        if(backpack[i].mateiral == key)
                        {
                            backpack[i].num = 1;
                            // = backpack[i].num + 1;
                            break;
                        }
                    }*/
                    Destroy(rch.transform.gameObject);
                }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch;
            if (Physics.Raycast(ra, out rch))
                if (rch.collider.gameObject.tag == "Ground" || rch.collider.gameObject.tag == "Block")
                {
                    GameObject block = Instantiate(Resources.Load("blocks/" + space[selector].material) as GameObject);
                    block.transform.position = rch.point + new Vector3(0, block.transform.localScale.y / 2, 0);
                    space[selector].num--;
                    Debug.Log(space[selector].num);
                }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground" || (col.gameObject.tag == "Block" && col.gameObject.transform.position.y + col.gameObject.transform.localScale.y / 2 < gameObject.transform.position.y))
            onGround = true;
    }

}
