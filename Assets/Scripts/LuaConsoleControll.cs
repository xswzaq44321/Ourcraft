﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.UI;
using System;

public class LuaConsoleControll : MonoBehaviour
{

	public InputField inputField;
	public Text message;
	Script script;
	List<string> prevCommands;
	int prevCommandsIter;

	// Use this for initialization
	void Start()
	{
		script = new Script();
		prevCommands = new List<string>();
		prevCommandsIter = 0;

		script.Options.DebugPrint = s => printMessage(s);
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
		}
	}

	public void enable()
	{
		inputField.ActivateInputField();
	}

	public void disable()
	{
		inputField.text = "";
		inputField.DeactivateInputField();
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
