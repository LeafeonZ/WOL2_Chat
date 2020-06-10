using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChoiceScript : MonoBehaviour {

	/// Component on BG of menu, with a blank text box in the center as child 0.

	private GameObject choiceBase; // child 0
	private Image bG; // BG image on this gameObject.
	private GameObject pointer; // index 1
	///private Animator Anim; // animator for it. uses "MenuOpen" Bool to open/close. (enable/disable text/images)
	// NOTE: just enable/disable it. Seriously. 

	//private bool resize = true; // resize this box? 
	[Header("Resize Options")]
	private float vSpace; // vertical spacing to maintain at top/btm. Size boxes to self-space. 
	private Vector2 choiceSize;

	private List<Text> choiceText;
	//private List<string> choices; //not actually needed!

	private bool isChoosing = false;
	private int currentIndex = 0; //index of current choice

	private GameObject returnObject;
	private string returnMethod;
	private string backMethod;
	/// </summary>
	private float holdTimer = 0f;

	public enum textStyle { normal,hover,selected};
	// sizing: Should only need 2 options, a y/n and a keywords list, which can use its own special display.
	void Start() {
		choiceBase = transform.GetChild(0).gameObject;
		pointer = transform.GetChild(1).gameObject;
		choiceBase.GetComponent<Text>().text = "";
		bG = gameObject.GetComponent<Image>();
		choiceSize = choiceBase.GetComponent<RectTransform>().sizeDelta;

		vSpace = gameObject.GetComponent<RectTransform>().sizeDelta.y;
		vSpace = (vSpace - choiceSize.y) / 2;

		CloseMenu();
	}

	// Update is called once per frame
	void Update() { // NOTE: INTEGRATE W/UI system when that's working. 
		if (isChoosing) {
			if (holdTimer > 0) { holdTimer = Mathf.Clamp(holdTimer - Time.unscaledDeltaTime, 0, 10); }
			if (Input.GetKeyDown("a")) {// selection
										// Note: play sound via UI?
				returnObject.SendMessage(returnMethod, choiceText[currentIndex].text); // or index?
				Debug.Log("SendChoice: " + returnMethod + " " + choiceText[currentIndex].text);
				TextStyle(choiceText[currentIndex], textStyle.selected);
				isChoosing = false;
				///hiight, delay, close menu! 
				CloseMenu();
			} else if (Input.GetKeyDown("b") && backMethod != "") { // back if applicable
			
				// Note: play sound via UI?

				isChoosing = false;
				returnObject.SendMessage(backMethod);
				// if backmethod == return methon, send return w/ 0 index?
				// send back message, close menu
				CloseMenu();
			} else if (Input.GetAxis("Vertical") != 0 && holdTimer<=0.01f) {
				// Note: play sound via UI?
				holdTimer = 0.25f;
				// scroll: move pointer, hilight new, unhilight old
				int newIndex = Mathf.RoundToInt( Input.GetAxis("Vertical") / Mathf.Abs(Input.GetAxis("Vertical")));
				newIndex = Mathf.RoundToInt( Mathf.Clamp(currentIndex + newIndex, 0, choiceText.Count - 1));

				pointer.transform.localPosition = choiceText[newIndex].GetComponent<RectTransform>().localPosition;
				TextStyle(choiceText[currentIndex], textStyle.normal);
				TextStyle(choiceText[newIndex], textStyle.hover);
				currentIndex = newIndex;

			} else if(Input.GetAxis("Vertical") == 0) { holdTimer = 0; }
		}
	}


	void TextStyle(Text t,textStyle style) { // text effects
		switch (style) {
			case textStyle.normal:
				// normal, scale: 1,
				t.color = Color.black;
				t.fontStyle = FontStyle.Normal;
				t.GetComponent<RectTransform>().localScale = Vector3.one;
				break;
			case textStyle.hover:
				t.color = Color.gray;
				t.fontStyle = FontStyle.Bold;
				t.GetComponent<RectTransform>().localScale = Vector3.one * 1.01f;
				break;
			case textStyle.selected:
				t.color = Color.red;
				t.fontStyle = FontStyle.Bold;
				t.GetComponent<RectTransform>().localScale = Vector3.one * 1.05f;
				// scale 1.2, bold
				break;
		}
	}
	void CloseMenu() {
		// hide menu , clear out text boxes, etc
		if (choiceText != null && choiceText.Count > 0) {
			for (int i = 0; i < choiceText.Count; i++) {
				Destroy(choiceText[i].gameObject);
			}
		}
		//send choice to source here? X
		choiceText = new List<Text>();
		backMethod = "";
		returnMethod = "";
		returnObject = null;
		gameObject.SetActive(false);
	}

	public void NewChoice(List<string> choices, int startIndex, GameObject source, string returnMethodName) {
		choiceText = new List<Text>();
		currentIndex = startIndex; /// NOTE: 0= bottom of list!!
		returnMethod = returnMethodName;
		returnObject = source;
		backMethod = "";
		isChoosing = true;

		gameObject.SetActive(true); // make visible

		// resize BG
		RectTransform bgSize = gameObject.GetComponent<RectTransform>();
		bgSize.sizeDelta = new Vector2(bgSize.sizeDelta.x, vSpace * 2 + choiceSize.y*choices.Count);

		for (int i = 0; i < choices.Count; i++) {// set up menu choices
			GameObject box = Instantiate(choiceBase, gameObject.transform);

			//box.GetComponent<RectTransform>().localPosition = choiceText[Mathf.Clamp(current - 1, 0, choices.Count)].GetComponent<RectTransform>().localPosition + new Vector3(0, choiceSize.y, 0);
			box.GetComponent<RectTransform>().localPosition = choiceBase.GetComponent<RectTransform>().localPosition + new Vector3(0, choiceSize.y * i, 0);

			// fill in
			choiceText.Add(box.GetComponent<Text>());
			choiceText[i].text = choices[i];
		}


		// pointer location + hilight first choice
		pointer.transform.localPosition = choiceText[currentIndex].GetComponent<RectTransform>().localPosition;
		TextStyle(choiceText[currentIndex], textStyle.hover);
		isChoosing = true;
	}
}
