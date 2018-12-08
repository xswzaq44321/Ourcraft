using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapSaveRead : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//map.openMap(@"d:\documents\123.json");
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool flag = true;
	private Map map;

	// instantiate all objects
	void loadMap(string path)
	{
		map = new Map();
		map.openMap(path);
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
		player.transform.position = new Vector3(map.player.X, map.player.Y, map.player.Z);
		player.GetComponent<Controller>().set_HP(map.player.HP);
		player.GetComponent<Backpack>().load_backpack(map.player.backpack);
	}
	void saveMap(string path)
	{
		map = new Map();
		// get all block's position
		var blocks = GameObject.FindGameObjectsWithTag("block");
		foreach (var it in blocks)
		{
			map.blocks.Add(new Block(it.name, it.transform.position.x, it.transform.position.y, it.transform.position.z));
		}
		map.count = blocks.GetLength(0);
		// get player informations
		var player = GameObject.Find("player");
		map.player.X = player.transform.position.x;
		map.player.Y = player.transform.position.y;
		map.player.Z = player.transform.position.z;
		map.player.HP = player.GetComponent<Controller>().get_HP();
		map.player.backpack = player.GetComponent<Backpack>().save_backpack().ConvertAll(s => string.Copy(s));

		map.saveMap(path);
	}
}

[Serializable]
public class Map
{
	public Map()
	{
		this.blocks = new List<Block>();
	}

	public int count;
	public List<Block> blocks;
	public Player player;

	// transform map to json
	public void saveMap(string path)
	{
		string json = JsonUtility.ToJson(this);
		FileStream fs = new FileStream(path, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(json);

		sw.Close();
		fs.Close();
	}
	// transform json to map
	public void openMap(string path)
	{
		FileStream fs = new FileStream(path, FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		string json = sr.ReadToEnd();
		var oldMap = JsonUtility.FromJson<Map>(json);
		/* transfer informations from oldMap to this */
		// blocks
		this.blocks = oldMap.blocks.ConvertAll(block => new Block(block.name, block.X, block.Y, block.Z));
		// player informations
		this.player.X = oldMap.player.X;
		this.player.Y = oldMap.player.Y;
		this.player.Z = oldMap.player.Z;
		this.player.HP = oldMap.player.HP;
		// deep copy string
		this.player.backpack = oldMap.player.backpack.ConvertAll(s => string.Copy(s));

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
	public float X, Y, Z;
	public int HP;
	public List<string> backpack;
}
