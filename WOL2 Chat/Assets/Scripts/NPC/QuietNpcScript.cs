using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuietNpcScript : MonoBehaviour
{
	[SerializeField] private string myName = "Name"; // display name for NPC
	[SerializeField] private string chatLine = "Hello!";
	[SerializeField] private string chatAgainLine = "Hello again!";


	//private bool isTalking = false;
	private bool again = false; // activate when spoken to, deactivate w/activate/scedual.

	// NOTE: will be filled in by sheet later

	public List<Quest> quests; // list of quests to give! 
	private bool autoquest;

	ReadDialogueScript dia;
	// Start is called before the first frame update
	void Start() {
		dia = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadDialogueScript>();
	}
	private void OnEnable() {
		again = false;
	}
	public void Interact() {
		Debug.Log("Quiet");
		if (isActiveAndEnabled) {

			if (dia == null) { dia = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadDialogueScript>(); }
			// quest check/clear: remove finished quests, check for an auto. 
			//	if(quests.Count>0 && // questName)
			// if no auto...
			// send line to reader
			if (dia != null && !dia.IsTalking() && isActiveAndEnabled) {
				// send message.
				//isTalking = true;
				dia.enabled = true;
				if (again) { dia.ChatStart(myName, chatAgainLine, this.gameObject, true); } else { dia.ChatStart(myName, chatLine, this.gameObject, true); }
			} else { Debug.LogError("ReadDialogueScript isn't on a thing w/ GameController tag!!"); }// just in case
		}
	}

	void LineEnd() {
		again = true;
	}
	void ChatEnd() {
		again = true;
		//isTalking = false;
	}
	private void OnDisable() {
		again = false;
	}

}
