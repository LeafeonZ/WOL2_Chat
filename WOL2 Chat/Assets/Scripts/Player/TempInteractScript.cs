using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInteractScript : MonoBehaviour
{
	public enum GameStates { normal,talking,paused};
	public static GameStates CurrentState = GameStates.normal; // these should be in the GC, really. 
	// Start is called before the first frame update
	//[SerializeField] private GameObject interactThing;
    void Start()
    {
    }

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown("a") && CurrentState == GameStates.normal) { // needs to be set to proper input stuff


			List<Vector3> directions = new List<Vector3>() { Vector3.forward,AngleDirection(-30f),AngleDirection(30f) }; // directions to check, cone!
			float dist = 2f; // ease of access
			int layermask = 1 << 8; // Interact layer only!! 
			for (int i = 0; i< directions.Count; i++) {
				RaycastHit hit;
				if (Physics.Raycast(transform.position, transform.TransformDirection(directions[i]), out hit, dist, layermask)) {
					Debug.DrawRay(transform.position, transform.TransformDirection(directions[i]) * hit.distance, Color.green, 1f);
					hit.collider.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
					Debug.Log("Hit");
					break;
				} else { Debug.DrawRay(transform.position, transform.TransformDirection(directions[i]) * dist, Color.red.gamma, 0.5f); }
			}
		}
	}
	
	private Vector3 AngleDirection(float angle) {return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;}
}
