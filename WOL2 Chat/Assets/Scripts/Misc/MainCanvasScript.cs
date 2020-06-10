using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasScript : MonoBehaviour
{ // mostly just to tracking stuff so this doesn't have to be an issue. Uses names so that stuff can be found
	// even if children were re-ordered orsomething. 
   
	private List<string> names;
	private List<GameObject> children;
	void Awake() {

		if (names == null) { // aka if this hasnt been done yet
			names = new List<string>();
			children = new List<GameObject>();
			int count = gameObject.transform.childCount;
			for (int i = 0; i < count; i++) {
				children.Add(gameObject.transform.GetChild(i).gameObject);
				names.Add(children[i].name);
			}
			
		}
		
	}

	public GameObject GetDisplay(string find) { // to retrieve info
		if (names.Contains(find)) {
			int x = names.IndexOf(find);
			return children[x];
		} else { return null; }

	}
}
