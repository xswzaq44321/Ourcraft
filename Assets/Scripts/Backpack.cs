using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour {

    public bool infinite_block, show_gameinfo;
    public float touchable_distance;
    public Canvas itembox_image;
    public GameObject item_number_ui, game_info_ui;
    private Vector3 select_frame_pos = new Vector3(77.875f, 0, 0);
    int selector = 0;
    List<string> itembox_lite = new List<string>();
    Dictionary<string, int> material_num = new Dictionary<string, int>()
    {
        {"brick",       materiall_type.brick},
        {"cobblestone", materiall_type.cobblestone},
        {"diamond_ore", materiall_type.diamond_ore},
        {"dirt",        materiall_type.dirt},
        {"gold_ore",    materiall_type.gold_ore},
        {"grass",       materiall_type.grass},
        {"iron_ore",    materiall_type.iron_ore},
        {"log_oak",     materiall_type.log_oak},
        {"planks_oak",  materiall_type.planks_oak},
        {"stone",       materiall_type.stone},
        {"sand",        materiall_type.sand},
    };
    struct Material
    {
        public int brick, cobblestone, diamond_ore, dirt, gold_ore,
                    grass, iron_ore, log_oak, planks_oak, stone,
                    sand;
    }
    static Material materiall_type = new Material();

    // Use this for initialization
    void Start () {
        itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position - 4 * select_frame_pos;   
        for (int i = 0; i < 9; i++)
            item_number_ui.GetComponent<Transform>().GetChild(i).GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update () {

        if (show_gameinfo)
            game_info_ui.GetComponent<Transform>().GetChild(0).GetComponent<Text>().text = "Player Position: " + transform.position.ToString();
        else
        {
            game_info_ui.GetComponent<Transform>().GetChild(0).GetComponent<Text>().text = "";
            game_info_ui.GetComponent<Transform>().GetChild(1).GetComponent<Text>().text = "";
        }

        //update itembox information//
        for (int i = 0; i < itembox_lite.Count; i++)
            item_number_ui.GetComponent<Transform>().GetChild(i).GetComponent<Text>().text = material_num[itembox_lite[i]].ToString();
        for (int i = itembox_lite.Count; i < 9; i++)
            item_number_ui.GetComponent<Transform>().GetChild(i).GetComponent<Text>().text = "";

        //select itembox//
        if (Input.GetKey(KeyCode.Alpha1))
        {
            selector = 0;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position - 4 * select_frame_pos;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            selector = 1;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position - 3 * select_frame_pos;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            selector = 2;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position - 2 * select_frame_pos;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            selector = 3;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position - select_frame_pos;
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            selector = 4;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position;
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            selector = 5;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position + select_frame_pos;
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            selector = 6;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position + 2 * select_frame_pos;
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            selector = 7;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position + 3 * select_frame_pos;
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            selector = 8;
            itembox_image.GetComponent<Transform>().GetChild(1).transform.position = itembox_image.GetComponent<Transform>().GetChild(0).transform.position + 4 * select_frame_pos;
        }

        //get blocks//
        if (Input.GetMouseButtonDown(0))
        {
            Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch;
            if (Physics.Raycast(ra, out rch))
            {
                if (rch.collider.gameObject.tag == "Block" && Vector3.Distance(rch.point, transform.position) <= touchable_distance)
                {
                    string[] separator = { "(", " (" };
                    string item = rch.collider.gameObject.name.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)[0];
                    if (material_num[item] <= 0) itembox_lite.Add(item);
                    material_num[item]++;
                    Destroy(rch.transform.gameObject);
                }

                if (show_gameinfo)
                    game_info_ui.GetComponent<Transform>().GetChild(1).GetComponent<Text>().text = "Clicked Position: " + rch.point.ToString();
            }
        }

        //put blocks//
        if (selector < itembox_lite.Count && Input.GetMouseButtonDown(1))
        {
            Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch;
            if (Physics.Raycast(ra, out rch))
            {
                if(Vector3.Distance(rch.point, transform.position) <= touchable_distance)
                {
                    if (rch.collider.gameObject.tag == "Ground")
                    {
                        GameObject block = Instantiate(Resources.Load("blocks/" + itembox_lite[selector]) as GameObject);
                        float PosX = Mathf.Floor(rch.point.x) + block.transform.localScale.x / 2;
                        float PosY = rch.point.y + block.transform.localScale.y / 2;
                        float PosZ = Mathf.Floor(rch.point.z) + block.transform.localScale.z / 2;
                        block.transform.position = new Vector3(PosX, PosY, PosZ);
                    }
                    else if (rch.collider.gameObject.tag == "Block")
                    {
                        GameObject block = Instantiate(Resources.Load("blocks/" + itembox_lite[selector]) as GameObject);
                        if (rch.collider.gameObject.transform.position.x - rch.point.x == block.transform.localScale.x / 2)
                            block.transform.position = rch.collider.gameObject.transform.position - new Vector3(block.transform.localScale.x, 0, 0);
                        else if (rch.collider.gameObject.transform.position.x - rch.point.x == -block.transform.localScale.x / 2)
                            block.transform.position = rch.collider.gameObject.transform.position + new Vector3(block.transform.localScale.x, 0, 0);
                        else if (Mathf.Abs(rch.collider.gameObject.transform.position.y - rch.point.y - block.transform.localScale.y / 2) < 0.01f)
                            block.transform.position = rch.collider.gameObject.transform.position - new Vector3(0, block.transform.localScale.y, 0);
                        else if (Mathf.Abs(rch.collider.gameObject.transform.position.y - rch.point.y + block.transform.localScale.y / 2) < 0.01f)
                            block.transform.position = rch.collider.gameObject.transform.position + new Vector3(0, block.transform.localScale.y, 0);
                        else if (rch.collider.gameObject.transform.position.z - rch.point.z == block.transform.localScale.z / 2)
                            block.transform.position = rch.collider.gameObject.transform.position - new Vector3(0, 0, block.transform.localScale.z);
                        else if (rch.collider.gameObject.transform.position.z - rch.point.z == -block.transform.localScale.z / 2)
                            block.transform.position = rch.collider.gameObject.transform.position + new Vector3(0, 0, block.transform.localScale.z);
                    }
                    if (--material_num[itembox_lite[selector]] <= 0 && !infinite_block)
                        itembox_lite.RemoveAt(selector);
                }

                if (show_gameinfo)
                    game_info_ui.GetComponent<Transform>().GetChild(1).GetComponent<Text>().text = "Clicked Position: " + rch.point.ToString();
            }
        }
    }

}
