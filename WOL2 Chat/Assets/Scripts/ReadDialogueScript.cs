using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadDialogueScript : MonoBehaviour {
	// attach this to game controller.
	private string chatlog; // Name \n  >(text here)
	private bool isTalking;
	private bool isTyping; // is the update supposed tobe putting out text?   //CONVERT BOOLS TO none/talking/typing/choice enum? or check anim instead?
	private int textIndex; // charactor/letter index.
	private int lineIndex; // line in list 

	//private Vector2Int textBoxSize = new Vector2Int(3, 32); // lines, char. with.
	[SerializeField] private Animator textBox;
	private Text nameDisplay;
	private Text textDisplay;

	private Vector2 interval;

	private GameObject source; // gameobject this came from
							   //private string currLine;
	private string speaker;// name of speaker
	private List<string> conversation;

	//private List<ProfileScript> activeBoxes; // active dialogue boxes. for placement.

	private AudioSource audi;
	public AudioClip blip;
	private bool endChat = false;

	//private float skipCoolDown = 0; // cooldown between skips when not typing
	void Start() {
		audi = gameObject.GetComponent<AudioSource>();
		interval = new Vector2(2, 2);
		textBox = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<MainCanvasScript>().GetDisplay("Chat").GetComponent<Animator>();
		textDisplay = textBox.gameObject.transform.GetChild(textBox.gameObject.transform.childCount - 1).GetComponent<Text>();
		nameDisplay = textBox.gameObject.transform.GetChild(textBox.gameObject.transform.childCount - 2).GetComponent<Text>();
		textBox.SetBool("IsActive", false);
	}

	// Update is called once per frame
	void FixedUpdate() { 
		//fixed so if paused, text pauses.
		 /*if (Input.GetKeyDown("A")) {
			 if (isTyping) {
				 isTyping = false;
				 Invoke("NextLine", 5);
				 textDisplay.text = "" + conversation[lineIndex];
				 audi.PlayOneShot(blip,0.5f);
			 } else {
				 NextLine();
				 //audi.PlayOneShot(blip, 1f);
			 }
		 }*/

		if (Input.GetKeyDown("a") && lineIndex != -1) {
			if (!isTyping && isTalking) {
				NextLine();
				audi.PlayOneShot(blip, 1f);
			}

		}

		if (isTyping) {
			if (interval.y == 0) {
				interval.y = interval.x;
				textDisplay.text += conversation[lineIndex][textIndex];
				audi.PlayOneShot(blip, 0.25f);

				if (Input.GetKey("b")) { // back BTn to prevent accidents?
					if (textIndex + 1 < conversation[lineIndex].Length) {
						textIndex++;
						textDisplay.text += conversation[lineIndex][textIndex];
					}
				}

				if (textIndex + 1 < conversation[lineIndex].Length) {
					textIndex++;
				} else {
					isTyping = false;
					 if (!endChat && conversation.Count <= lineIndex+1) { source.SendMessage("LineEnd"); }
				}
			} else { interval.y--; }
		}
	}
	void AddToChatlog(string line, string npcName) {
		// add to chatlog string
		chatlog += npcName + "\n" + "     > " + conversation[lineIndex] + "\n";
	}
	private void NextLine() {
		CancelInvoke("NextLine");
		interval.y = interval.x;
		textIndex = 0;
		lineIndex++;
		if (conversation != null) {
			if (conversation.Count > lineIndex) {// if has more lines
				AddToChatlog(conversation[lineIndex], speaker); // add last line to log
				textDisplay.text = ""; // clear text
				isTyping = true;

			} else if (endChat) { ChatOver(); }// else if (!endChat) { source.SendMessage("LineEnd"); }
		}
	}
	public void ChatStart(string npcName, string script, GameObject obj, bool endOfChat) {
		///	GameControllerScript.canPause= false;
		gameObject.GetComponent<ReadDialogueScript>().enabled = true;
		ActivateBox(true, npcName); // show box

		
		endChat = endOfChat;
		speaker = npcName;

		conversation = SplitLines(script); // splits into line array
		source = obj;
		isTalking = true;
		TempInteractScript.CurrentState = TempInteractScript.GameStates.talking;
		lineIndex = -1;
		Invoke("NextLine", 0.5f);
	}
	private void ChatOver() {
		isTalking = false;
		textBox.SetBool("IsActive", false);
		Invoke("ChatRelease", 0.5f);
	}
	private void ChatRelease() {
		TempInteractScript.CurrentState = TempInteractScript.GameStates.normal;
		textDisplay.text = "";
		if (source != null) { source.SendMessage("ChatEnd", SendMessageOptions.DontRequireReceiver); }
		gameObject.GetComponent<ReadDialogueScript>().enabled = false;

		//GameControllerScript.canPause = true;

	}
	private List<string> SplitLines(string s) {
		List<string> lineList = new List<string>();
		int firstIndex = s.IndexOf("&") + 1; // index of first char of next word
		int lastindex = s.LastIndexOf("&");
		string[] split = s.Split('&'); // NOTE: use '' NOT "" for char!++

		string test = "";
		for (int i = 0; i < split.Length; i++) {
			split[i].TrimStart(); // assumes its whitespace. 
			lineList.Add(split[i]);
			test += lineList[i] + "\n";
		}
		//Debug.Log(test);
		return lineList;

		/// Formatting: &= token separator

		//https://docs.microsoft.com/en-us/dotnet/api/system.string?redirectedfrom=MSDN&view=netcore-3.1
	}

	public bool IsTalking() {
		return isTalking;
	}
	private void ActivateBox(bool active, string npcName) {
		textBox.SetBool("IsActive", active);
		speaker = npcName;
		nameDisplay.text = npcName;
	}
	public void ForceChatEnd() { ChatOver(); }
	
}
