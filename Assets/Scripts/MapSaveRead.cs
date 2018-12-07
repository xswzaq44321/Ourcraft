using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapSaveRead : MonoBehaviour {

	// Use this for initialization
	void Start () {
		map = new Map();
		//map.openMap(@"d:\documents\123.json");
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool flag = true;
	private Map map;

	void loadMap(string path)
	{
		map.openMap(path);
		var blocks = map.blocks;
		string[] separator = { "(", " (" };
		foreach (var block in blocks)
		{
			string item = block.name.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)[0];
			GameObject forest = Instantiate(Resources.Load(string.Format("blocks/{0}", item)) as GameObject);
			forest.transform.position = new Vector3(block.X, block.Y, block.Z);
		}
	}
	void saveMap(string path)
	{
		var bar = GameObject.FindGameObjectsWithTag("block");
		foreach (var it in bar)
		{
			map.blocks.Add(new Block(it.name, it.transform.position.x, it.transform.position.y, it.transform.position.z));
		}
		map.count = bar.GetLength(0);
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

	public void saveMap(string path)
	{
		string json = JsonUtility.ToJson(this);
		FileStream fs = new FileStream(path, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(json);

		sw.Close();
		fs.Close();
	}
	public void openMap(string path)
	{
		FileStream fs = new FileStream(path, FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		string json = sr.ReadToEnd();
		var bar = JsonUtility.FromJson<Map>(json);
		this.blocks = bar.blocks.ConvertAll(block => new Block(block.name, block.X, block.Y, block.Z));

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
