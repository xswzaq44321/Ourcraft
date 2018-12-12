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
		script.Globals["timeSet"] = (Action<float>)((a) =>
		{
			objectHolder.world.GetComponent<time>().set_time(a);
		});
		helpMessage += "timeSet(time): set time to specific time.\r\n";
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
}
