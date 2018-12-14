using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.UI;
using System;

public class LuaConsoleControll : MonoBehaviour
{

	public InputField inputField;
	public GameObject chatPanel, textObject;
	public ObjectHolder objectHolder;
	public Scrollbar verticleScrollBar;
	Script script;
	List<string> prevCommands;
	int prevCommandsIter;
	[SerializeField]
	List<string> helpMessageList = new List<string>();
	[SerializeField]
	List<Message> messageList = new List<Message>();

	// Use this for initialization
	void Start()
	{
		script = new Script();
		prevCommands = new List<string>();
		prevCommandsIter = 0;

		script.Options.DebugPrint = s => printMessage(s);
		loadCommands();
	}

	// Update is called once per frainputField
	void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			if (inputField.text != "")
			{
				prevCommands.Add(inputField.text);
				prevCommandsIter = prevCommands.Count;
			}

			if (inputField.text == "clear") // magic words
			{
				while (messageList.Count > 0)
				{
					Destroy(messageList[0].textObject.gameObject);
					messageList.Remove(messageList[0]);
				}
			}
			else if (inputField.text == "help")
			{
				printMessage("> help");
				foreach (string msg in helpMessageList)
				{
					printMessage(msg, Color.yellow);
				}
			}
			else // execute lua
			{
				printMessage("> " + inputField.text);
				try
				{
					script.DoString(inputField.text);
				}
				catch (Exception err)
				{
					printMessage("Error: " + err.Message, Color.red);
				}
			}
			inputField.text = "";
			inputField.ActivateInputField();
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (prevCommands.Count == 0)
				return;
			inputField.text = prevCommands[prevCommandsIter == 0 ? 0 : --prevCommandsIter];
			inputField.selectionAnchorPosition = inputField.text.Length;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (prevCommands.Count == 0)
				return;
			inputField.text = ++prevCommandsIter >= prevCommands.Count ? "" : prevCommands[prevCommandsIter];
			inputField.selectionAnchorPosition = inputField.text.Length;
		}
	}

	public void enable()
	{
		inputField.enabled = true;
		inputField.ActivateInputField();
	}

	public void disable()
	{
		inputField.text = "";
		inputField.DeactivateInputField();
		inputField.enabled = false;
	}

	void printMessage(string msg)
	{
		Message newMessage = new Message(msg);
		GameObject newText = Instantiate(textObject, chatPanel.transform);
		newMessage.textObject = newText.GetComponent<Text>();
		newMessage.textObject.text = newMessage.text;
		messageList.Add(newMessage);
		verticleScrollBar.value = 0f;
	}
	void printMessage(string msg, Color color)
	{
		Message newMessage = new Message(msg);
		GameObject newText = Instantiate(textObject, chatPanel.transform);
		newMessage.textObject = newText.GetComponent<Text>();
		newMessage.textObject.text = newMessage.text;
		newMessage.textObject.color = color;
		messageList.Add(newMessage);
		verticleScrollBar.value = 0f;
	}

	void loadCommands()
	{
		script.Globals["setTime"] = (Action<float>)((a) =>
		{
			if (a < 0 || a > 24000)
			{
				printMessage("Error, expected input: 0 <= time <= 24000", Color.red);
				return;
			}
			objectHolder.world.GetComponent<time>().set_time(a);
		});
		helpMessageList.Add("setTime(time): set time to specific time.");
		script.Globals["saveMap"] = (Action<string>)((name) =>
		{
			objectHolder.saveRead.GetComponent<MapSaveRead>().saveMap(name);
		});
		helpMessageList.Add("saveMap(filename): save map as filename, filename could be spared.");
		script.Globals["loadMap"] = (Action<string>)((name) =>
		{
			objectHolder.saveRead.GetComponent<MapSaveRead>().loadMap(name);
		});
		helpMessageList.Add("loadMap(filename): load map filename, filename could be spared.");
		script.Globals["setHP"] = (Action<int>)((a) =>
		{
			if (a < 0 || a > 20)
			{
				printMessage("Error, expected input: 0 <= hp <= 20", Color.red);
				return;
			}
			objectHolder.player.GetComponent<Controller>().set_HP(a);
		});
		helpMessageList.Add("setHP(hp): set player's health to HP.");
		script.Globals["setWalkSpeed"] = (Action<float>)((a) =>
		{
			if (a <= 0)
			{
				printMessage("Error, expected input: speed > 0", Color.red);
				return;
			}
			objectHolder.player.GetComponent<Controller>().walk_speed = a;
		});
		helpMessageList.Add("setWalkSpeed(speed): set player's walking speed.");
		script.Globals["setRunSpeed"] = (Action<float>)((a) =>
		{
			if (a <= 0)
			{
				printMessage("Error, expected input: speed > 0", Color.red);
				return;
			}
			objectHolder.player.GetComponent<Controller>().run_speed = a;
		});
		helpMessageList.Add("setRunSpeed(speed): set player's running speed.");
		script.Globals["setTimeSpeed"] = (Action<float>)((a) =>
		{
			if (a < 0 || a > 1000)
			{
				printMessage("Error, expected input: 0 <= speed <= 1000", Color.red);
				return;
			}
			objectHolder.world.GetComponent<time>().delta_time = a;
		});
		helpMessageList.Add("setTimeSpeed(speed): set world time speed.");
		script.Globals["addItem"] = (Action<string, int>)((name, count) =>
		{
			if (count < 0 || count > 9999)
			{
				printMessage("Error, expected input: 0 <= count <= 9999", Color.red);
				return;
			}
			objectHolder.player.GetComponent<Backpack>().insert_item(name, count);
		});
		helpMessageList.Add("addItem(name, count): get count items.");
		script.Globals["infinityItem"] = (Action<bool>)((a) =>
		{
			objectHolder.player.GetComponent<Backpack>().infinite_block = a;
		});
		helpMessageList.Add("infinityItem(boolean): set infinity block mode.");
		script.Globals["setHealSpeed"] = (Action<float>)((a) =>
		{
			if (a < 0)
			{
				printMessage("Error, expected input: 0 <= speed", Color.red);
				return;
			}
			objectHolder.player.GetComponent<Controller>().heal_speed = a;
		});
		helpMessageList.Add("setHealSpeed(speed): set player healing speed.");
		script.Globals["rain"] = (Action<float, uint>)((last_time, scale) =>
		{
			if (last_time < 100 || last_time > 23799)
			{
				printMessage("Error, expected input: 100 <= last_time <= 23799", Color.red);
				return;
			}
			if (scale < 10 || scale > 40)
			{
				printMessage("Error, expected input: 10 <= scale <= 40", Color.red);
				return;
			}
			objectHolder.world.GetComponent<Weather>().rain(last_time, scale);
		});
		helpMessageList.Add("rain(last_time, heavy): start rainning for last_time, and how heavy it is.");
		script.Globals["setRainingRate"] = (Action<float>)((a) =>
		{
			if (a < 0 || a > 1)
			{
				printMessage("Error, expected input: 0 <= rate <= 1", Color.red);
				return;
			}
			objectHolder.world.GetComponent<Weather>().raining_rate = a;
		});
		helpMessageList.Add("setRainingRate(rate): set raining rate.");
		script.Globals["getRainInfo"] = (Action)(() =>
		{
			printMessage("rain_start: " +
				objectHolder.world.GetComponent<Weather>().rain_start.ToString()
			);
			printMessage("rain_end: " +
				objectHolder.world.GetComponent<Weather>().rain_end.ToString()
			);
			printMessage("rainfall: " +
				objectHolder.world.GetComponent<Weather>().rainfall.ToString()
			);
		});
		helpMessageList.Add("getRainInfo(): print weather info.");
		script.Globals["setAtkRange"] = (Action<float>)((a) =>
		{
			if (a < 0)
			{
				printMessage("Error, expected input: rate >= 0", Color.red);
				return;
			}
			objectHolder.player.GetComponent<Controller>().atk_range = a;
		});
		helpMessageList.Add("setAtkRange(range): set player attack range.");
		script.Globals["setTouchDistance"] = (Action<float>)((a) =>
		{
			if (a < 0)
			{
				printMessage("Error, expected input: rate >= 0", Color.red);
				return;
			}
			objectHolder.player.GetComponent<Backpack>().touchable_distance = a;
		});
		helpMessageList.Add("setTouchDistance(distance): set digging distance.");
		script.Globals["enableFly"] = (Action<bool>)((a) =>
		{
			objectHolder.player.GetComponent<Controller>().enable_fly = a;
		});
		helpMessageList.Add("enableFly(boolean): enable flying mode.");
	}
}

[System.Serializable]
public class Message
{
	public Message()
	{
	}
	public Message(string text)
	{
		this.text = text;
	}
	public string text;
	public Text textObject;
}
