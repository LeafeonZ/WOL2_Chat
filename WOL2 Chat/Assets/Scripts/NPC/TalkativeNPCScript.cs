using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkativeNPCScript : MonoBehaviour {
	[SerializeField] private string myName = "Name"; // display name for NPC

	[SerializeField] private string helloLine = "Hello!";
	[SerializeField] private string helloAgainLine = "Hello again! & helloagain2";
	[SerializeField] private string keyLine = "KeyLine & k2";
	[SerializeField] private string chatLine = "chatLine & C2";
	[SerializeField] private string nevermindLine = "Nevermind line"; // when 'cancel' chosen

	[SerializeField] private string chatOption = "Whats Up?", ShopOption = "Shop", keyOption = "Ask About...", endOption = "Nevermind";
	private List<string> questOption;
	//private bool isTalking = false;
	private bool again = false; // activate when spoken to, deactivate w/activate/scedual.

	// NOTE: will be filled in by sheet later
	private List<string> menu = new List<string>();
	

	public enum TalkingState { none,hello, menu, chat, keyword,quest,nevermind};
	private TalkingState currentTalkState = TalkingState.none;
	ReadDialogueScript dia;

	// new quest list
	// ongoing quest list

	// Start is called before the first frame update
	void Start() {
		dia = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadDialogueScript>();
		//Get base lines
		DataStorage.npcChat.TryGetValue(name + "Hello", out helloLine);
		DataStorage.npcChat.TryGetValue(name + "Hello Again", out helloAgainLine);
		DataStorage.npcChat.TryGetValue(name + "Key", out keyLine);
		DataStorage.npcChat.TryGetValue(name + "Chat", out chatLine);
		DataStorage.npcChat.TryGetValue(name + "Nevermind", out nevermindLine);
	}
	private void OnEnable() {
		again = false;
	}
	public void Interact() {
		if (isActiveAndEnabled) {
			
			if (dia == null) { dia = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadDialogueScript>(); }
			// quest cycle+ check! 
			// menu Setup
			menu = new List<string>();
			menu.Add(chatOption);
			// If shopscript, get it's line. 
			menu.Add(keyOption);
			// IF ongoingQuest/TurnIn, getline(s)
			menu.Add(endOption);
			menu.Reverse();

			// if no auto...
			// send line to reader
			if (dia != null) {
				// send message.
				if (again) { dia.ChatStart(myName, helloAgainLine, this.gameObject, false); } else { dia.ChatStart(myName, helloLine, this.gameObject, false); }
				currentTalkState = TalkingState.hello;
			} else { Debug.LogError("ReadDialogueScript isn't on a thing w/ GameController tag!!"); }// just in case
		}
	}

	public void LineEnd() { // endof sent text. 
		switch (currentTalkState) {///NOTE: when line is over!!
			case TalkingState.hello: // starting pulls up menu!
				ChoiceScript c = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<MainCanvasScript>().GetDisplay("Choice").GetComponent<ChoiceScript>();
				c.NewChoice(menu, 0, gameObject, "ChoiceMade");
				currentTalkState = TalkingState.menu;
				break;
			case TalkingState.chat:// chat over. event check. 
				currentTalkState = TalkingState.none;
				break;
			case TalkingState.keyword:// keyword over. event/quest check
				currentTalkState = TalkingState.none;
				break;
			case TalkingState.nevermind:// end. thats all. 
				currentTalkState = TalkingState.none;
				break;
			case TalkingState.none:
				Debug.LogError("Line end when not talking!!");
				break;
		}
	}
	public void ChoiceMade(string s) {
		if (currentTalkState == TalkingState.menu) {
			if (s == chatOption) {
				// Event Check! 
				bool end = true;
				// if event/quest, end = false;
				///If no quest:
				dia.ChatStart(myName, chatLine, this.gameObject, end);// chat line
				currentTalkState = TalkingState.chat;
				/// else hand off to quest!
			} else if (s == ShopOption) {
				// hand off to shoppingScript!!
				currentTalkState = TalkingState.none;
			} else if (s == keyOption) {
				dia.ChatStart(myName, keyLine, this.gameObject, false);// ask about line.
				currentTalkState = TalkingState.keyword;
				// pull up keyword list. 
			} else if (s == endOption) {
				dia.ChatStart(myName, nevermindLine, this.gameObject, true);// send line. end.
				currentTalkState = TalkingState.none;
			} else {
				//for(int i = 0; i<quests.count;i++){
				// if(s == quest.QuestName){hand off to that script!}
				//}
				currentTalkState = TalkingState.none;
				// quest deeals with it now!
			}
		} else if (currentTalkState == TalkingState.keyword) {
			// check for keyword quest, 
			//else :Check for event
			//check for text!
			dia.ChatStart(myName, "KeySTuff", this.gameObject, true);// send line. end.
			currentTalkState = TalkingState.none;
		}
	}

	void ChatEnd() {
		again = true;
	//	isTalking = false;
	}
	private void OnDisable() {
		again = false;
	}

}
