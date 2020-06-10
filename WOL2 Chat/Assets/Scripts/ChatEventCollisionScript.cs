using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatEventCollisionScript : MonoBehaviour { // used to trigger a conversation on trigger enter.
	public bool isFreeOnStart;
	public float messageDelay;
	public string myName;
	public string lines;

	public bool onceOnly;
	public bool isFreeOnEnd;
	public float newVol = 0.25f;
	public float finVol = 1;
	ReadDialogueScript dia;
	//	public Vector3 returnPos;
	void awake() {
		if (messageDelay == 0) {
			messageDelay = 0.2f;
		}
		dia = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadDialogueScript>();
	}

	// Update is called once per frame
	void Update() {

	}
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") && isActiveAndEnabled) {
			dia = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadDialogueScript>();
			//dia.ActivateBox(true,myName);
			//	Invoke("DelayMessage", messageDelay);
			DelayMessage();
			
		//	GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelControllerScript>().BGMVolume(newVol, false, 0.5f);
		}
	}

	private void DelayMessage (){
		 dia = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadDialogueScript>();
		if (!dia.IsTalking() && isActiveAndEnabled) {
			dia.enabled = true;
			dia.ChatStart(myName, lines, gameObject,true);
			//Debug.Log("Sending: " + myName + ": " + lines +", "+ dia.isActiveAndEnabled);
		}
	}
	public void ChatEnd() {
		if (onceOnly) { GameObject.Destroy(gameObject); }

	}
}

