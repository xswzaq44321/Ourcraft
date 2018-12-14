using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.UI;
using System;

public class LuaConsoleControll : MonoBehaviour
{

	public InputField inputField;
	public Text message;
	public ObjectHolder objectHolder;
	Script script;
	List<string> prevCommands;
	int prevCommandsIter;
	string helpMessage;

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
				message.text = "";
			}
			else if (inputField.text == "help")
			{
				printMessage(helpMessage);
			}
			else // execute lua
			{
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
		message.text += (msg + "\r\n");
	}
	void printMessage(string msg, Color color)
	{
		string tag = "#"
			+ ((int)(color.r * 255)).ToString("X2")
			+ ((int)(color.g * 255)).ToString("X2")
			+ ((int)(color.b * 255)).ToString("X2");
		message.text += ("<color=" + tag + ">" + msg + "\r\n" + "</color>");
	}

	void loadCommands()
	{
		script.Globals["setTime"] = (Action<float>)((a) =>
		{
			objectHolder.world.GetComponent<time>().set_time(a);
		});
		helpMessage += "setTime(time): set time to specific time.\r\n";
		script.Globals["saveMap"] = (Action<string>)((name) =>
		{
			objectHolder.saveRead.GetComponent<MapSaveRead>().saveMap(name);
		});
		helpMessage += "saveMap(filename): save map as filename, filename could be spared.\r\n";
		script.Globals["loadMap"] = (Action<string>)((name) =>
		{
			objectHolder.saveRead.GetComponent<MapSaveRead>().loadMap(name);
		});
		helpMessage += "loadMap(filename): load map filename, filename could be spared.\r\n";
		script.Globals["setHP"] = (Action<int>)((a) =>
		{
			objectHolder.player.GetComponent<Controller>().set_HP(a);
		});
		helpMessage += "setHP(hp): set player's health to HP.\r\n";
		script.Globals["setWalkSpeed"] = (Action<float>)((a) =>
		{
			objectHolder.player.GetComponent<Controller>().walk_speed = a;
		});
		helpMessage += "setWalkSpeed(speed): set player's walking speed.\r\n";
		script.Globals["setRunSpeed"] = (Action<float>)((a) =>
		{
			objectHolder.player.GetComponent<Controller>().run_speed = a;
		});
		helpMessage += "setRunSpeed(speed): set player's running speed.\r\n";
		script.Globals["setTimeSpeed"] = (Action<float>)((a) =>
		{
			objectHolder.world.GetComponent<time>().delta_time = a;
		});
		helpMessage += "setTimeSpeed(speed): set world time speed.\r\n";
		script.Globals["getItem"] = (Action<string, int>)((name, count) =>
		{
			objectHolder.player.GetComponent<Backpack>().insert_item(name, count);
		});
		helpMessage += "getItem(name, count): get count items.\r\n";
		script.Globals["infinityItem"] = (Action<bool>)((a) =>
		{
			objectHolder.player.GetComponent<Backpack>().infinite_block = a;
		});
		helpMessage += "infinityItem(boolean): set infinity block mode.\r\n";
		script.Globals["setHealSpeed"] = (Action<float>)((a) =>
		{
			objectHolder.player.GetComponent<Controller>().heal_speed = a;
		});
		helpMessage += "setHealSpeed(speed): set player healing speed.\r\n";
		script.Globals["rain"] = (Action<float, uint>)((last_time, scale) =>
		{
			objectHolder.world.GetComponent<Weather>().rain(last_time, scale);
		});
		helpMessage += "rain(last_time, heavy): start rainning for last_time, and how heavy it is.\r\n";
		script.Globals["setRainingRate"] = (Action<float>)((a) =>
		{
			objectHolder.world.GetComponent<Weather>().raining_rate = a;
		});
		helpMessage += "setRainingRate(rate): set raining rate.\r\n";
		script.Globals["getRainInfo()"] = (Action)(() =>
		{
			printMessage("rain_start: " +
				objectHolder.world.GetComponent<Weather>().rain_start.ToString()
				 + "\r\n"
			);
			printMessage("rain_end: " +
				objectHolder.world.GetComponent<Weather>().rain_end.ToString()
				 + "\r\n"
			);
			printMessage("rainfall: " +
				objectHolder.world.GetComponent<Weather>().rainfall.ToString()
				 + "\r\n"
			);
		});
		helpMessage += "getRainInfo(): print weather info.\r\n";
		script.Globals["setAtkRange"] = (Action<float>)((a) =>
		{
			objectHolder.player.GetComponent<Controller>().atk_range = a;
		});
		helpMessage += "setAtkRange(range): set player attack range.\r\n";
		script.Globals["setTouchDistance"] = (Action<float>)((a) =>
		{
			objectHolder.player.GetComponent<Backpack>().touchable_distance = a;
		});
		helpMessage += "setTouchDistance(distance): set digging distance.\r\n";
	}
}
