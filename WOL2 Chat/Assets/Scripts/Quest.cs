using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
	public string QuestName;
	public List<string> chat = new List<string>() { "Ask", "Accept", "Decline", "Unfinished", "Finished" };
	public bool autoQuest = false;


	public Quest(string name, List<string> chatLines, bool aQ) {
		QuestName = name;
		chat = chatLines;
		autoQuest = aQ;
	}
}
