using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScript : MonoBehaviour
{
	public string QuestName;
	public List<string> chat = new List<string>() { "Ask", "Accept", "Decline", "Unfinished", "Finished" };
	public bool autoQuest;
	public string keyword;
	public string flagReq;
	public string NotFlag;

	public bool hideQuest; // to hide quest from player. Useful for events, rewards. 
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
