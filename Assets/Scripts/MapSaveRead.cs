using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapSaveRead : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	public bool flag = true;
	private Map map;

	// instantiate all objects
	public void loadMap(string fileName)
	{
		if (fileName == null)
			fileName = "map1";
		// if fileName not ended with extension ".json", make it so.
		if (fileName.Length - 5 < 0 || fileName.Substring(fileName.Length - 5) != ".json")
			fileName += ".json";

		// open file first to tease exceptions
		map = new Map();
		map.openJson(fileName);

		// get player
		var player = GameObject.Find("player");

		// destroying old blocks
		var oldBlocks = GameObject.FindGameObjectsWithTag("Block");
		for (int i = 0; i < oldBlocks.Length; ++i)
		{
			Destroy(oldBlocks[i]);
		}

		// load map blocks
		var blocks = map.blocks;
		string[] separator = { "(", " (" };
		foreach (var block in blocks)
		{
			string item = block.name.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)[0];
			GameObject forest = Instantiate(Resources.Load(string.Format("blocks/{0}", item)) as GameObject);
			forest.transform.position = new Vector3(block.X, block.Y, block.Z);
		}
		// load player informations
		player.transform.position = new Vector3(map.player.pos.X, map.player.pos.Y, map.player.pos.Z);
		player.GetComponent<Controller>().set_HP(map.player.HP);
		player.GetComponent<Backpack>().load_backpack(map.player.backpack);
		// load time
		GameObject.Find("World").GetComponent<time>().set_time(map.time);
		// load gravity
		if (Physics.gravity.y != map.gravity_y)
		{
			Physics.gravity *= -1;
		}
		if(Physics.gravity.y > 0)
		{
			RenderSettings.skybox = Resources.Load("Viking/Skyboxes/Skybox_sunset") as Material;
			player.transform.Rotate(new Vector3(180, 0, 0));
			player.GetComponent<Controller>().side = -1;
		}
		else
		{
			RenderSettings.skybox = player.GetComponent<Controller>().default_skybox;
		}
	}
	// save blocks & player informations
	public void saveMap(string fileName)
	{
		if (fileName == null)
			fileName = "map1";
		// if fileName not ended with extension ".json", make it so.
		if (fileName.Length - 5 < 0 || fileName.Substring(fileName.Length - 5) != ".json")
			fileName += ".json";

		map = new Map();
		// get all block's position
		var blocks = GameObject.FindGameObjectsWithTag("Block");
		foreach (var it in blocks)
		{
			map.blocks.Add(new Block(it.name, it.transform.position.x, it.transform.position.y, it.transform.position.z));
		}
		map.count = blocks.GetLength(0);
		// get player informations
		var player = GameObject.FindGameObjectWithTag("Player");
		map.player.pos.X = player.transform.position.x;
		map.player.pos.Y = player.transform.position.y;
		map.player.pos.Z = player.transform.position.z;
		int HP = player.GetComponent<Controller>().get_HP();
		if (HP <= 0)
		{
			map.player.HP = player.GetComponent<Controller>().MAX_HP;
			map.time = 8000;
		}
		else
		{
			map.player.HP = HP;
			// get world time
			map.time = GameObject.Find("World").GetComponent<time>().get_time();
		}
		map.player.backpack = player.GetComponent<Backpack>().save_backpack().ConvertAll(s => string.Copy(s));
		// gravity
		map.gravity_y = Physics.gravity.y;

		map.saveToJson(fileName);
	}
}

[Serializable]
public class Map
{
	public Map()
	{
		this.blocks = new List<Block>();
		player = new Player();
		string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		savePath = documentPath + "/Ourcraft Maps";
		System.IO.Directory.CreateDirectory(savePath);
	}

	public int count;
	public List<Block> blocks;
	public Player player;
	public float time;
	public float gravity_y;
	string savePath;

	// transform map to json
	public void saveToJson(string fileName)
	{
		string json = JsonUtility.ToJson(this);
		FileStream fs = new FileStream(savePath + "/" + fileName, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(json);

		sw.Close();
		fs.Close();
	}
	// transform json to map
	public void openJson(string fileName)
	{
		FileStream fs = new FileStream(savePath + "/" + fileName, FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		string json = sr.ReadToEnd();
		var oldMap = JsonUtility.FromJson<Map>(json);
		/* transfer informations from oldMap to this */
		// blocks
		this.blocks = oldMap.blocks.ConvertAll(block => new Block(block.name, block.X, block.Y, block.Z));
		// player informations
		this.player = new Player(oldMap.player);
		// time
		this.time = oldMap.time;
		// gravity
		this.gravity_y = oldMap.gravity_y;

		sr.Close();
		fs.Close();
	}
}

[Serializable]
public class Block
{
	public Block(string name, float X, float Y, float Z)
	{
		this.name = name;
		this.X = X;
		this.Y = Y;
		this.Z = Z;
	}
	public string name;
	public float X, Y, Z;
}

[Serializable]
public class Player
{

	public Player()
	{
		backpack = new List<string>();
	}
	public Player(Player old)
	{
		this.pos = old.pos;
		this.HP = old.HP;
		// deep copy string
		this.backpack = old.backpack.ConvertAll(s => string.Copy(s));
	}

	public Tri pos;
	public int HP;
	public List<string> backpack;
}

[Serializable]
public struct Tri
{
	public float X, Y, Z;
}
