using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;

public class MapSaveRead : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.M))
		{
			saveMap("map1.json");
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			var blocks = GameObject.FindGameObjectsWithTag("Block");
			for(int i = 0; i < blocks.Length; ++i)
			{
				Destroy(blocks[i]);
			}
			loadMap("map1.json");
		}
	}

	public bool flag = true;
	private Map map;

	// instantiate all objects
	void loadMap(string fileName)
	{
		map = new Map();
		map.openJson(fileName);
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
		var player = GameObject.Find("player");
		player.transform.position = new Vector3(map.player.pos.X, map.player.pos.Y, map.player.pos.Z);
		player.GetComponent<Controller>().set_HP(map.player.HP);
		player.GetComponent<Backpack>().load_backpack(map.player.backpack);
		// load time
		GameObject.Find("World").GetComponent<time>().set_time(map.time);
	}
	// save blocks & player informations
	void saveMap(string fileName)
	{
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
		map.player.HP = player.GetComponent<Controller>().get_HP();
		map.player.backpack = player.GetComponent<Backpack>().save_backpack().ConvertAll(s => string.Copy(s));
		// get world time
		map.time = GameObject.Find("World").GetComponent<time>().get_time();

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
	}

	public int count;
	public List<Block> blocks;
	public Player player;
	public float time;

	// transform map to json
	public void saveToJson(string fileName)
	{
		string json = JsonUtility.ToJson(this);
		FileStream fs = new FileStream("Assets/Resources/Maps/" + fileName, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(json);
		AssetDatabase.ImportAsset("Assets/Resources/Maps/" + fileName);

		sw.Close();
		fs.Close();
	}
	// transform json to map
	public void openJson(string fileName)
	{
		FileStream fs = new FileStream("Assets/Resources/Maps/" + fileName, FileMode.Open);
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
