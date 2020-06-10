using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryStorageScript : MonoBehaviour
{
	private static int newThing = 0;

	public static List<string> keywords = new List<string>() { "Terrabris" };
	// scratched keywords?

	private static List<Quest> finishedQuests = new List<Quest>();
	private static List<Quest> activeQuests = new List<Quest>();

	private static List<string> flags = new List<string>(); // list of flags for stuff. 
	/// graveyard, rats, Sewerkey, DrinkingCaptain

	public static bool HasQuest(Quest q) { return (finishedQuests.Contains(q) || activeQuests.Contains(q)); }
	public static void AddQuest(Quest q) {
		if (!HasQuest(q)) {
			activeQuests.Add(q);
			// mark diary as new?
			newThing++;
			// notify?
		}
	}
	public static void FinishQuest(Quest q) {
		finishedQuests.Add(q);
		activeQuests.Remove(q);
	}

	public static bool HasKey(string s) { return keywords.Contains(s); }

	public static bool FlagCheck(string f) { return flags.Contains(f); }
	public static void AddFlag(string f) { if (!FlagCheck(f)) { flags.Add(f); } }
}
