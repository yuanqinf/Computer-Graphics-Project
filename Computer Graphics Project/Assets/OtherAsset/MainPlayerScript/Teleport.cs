using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Teleport : NetworkBehaviour {
	public float x = -4.0f;
	GameObject item;
	GameObject player;
	bool removed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider other)
	{
		// If items stay in the "Teleport" trigger, get their reference and translate them
		if (other.gameObject.CompareTag ("Item")) {
			removed = false;                       // set the "removed" flag back to default 
			item = other.gameObject;               // get item object's reference
			if (item.transform.position.y <= 0.0f) {
				// Translate this item to Receiver's position
				item.transform.position = new Vector3 (x, 0, 11.0f);
			}
		} 
		// If player stay in this trigger, get player's reference for next step use
		if (other.gameObject.CompareTag ("Player")) {
			player = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other){
		// If player exit the trigger, send message to player to call helper function to remove client authority
		if (other.gameObject.CompareTag ("Player")) {
			// Check if this script has item object's reference and if this object is removed or player didn't hold anything
			if (item && !removed) {
				// Tell player's script to run the helper function "RemoveAuthority"
				player.SendMessage ("RemoveAuthority");
				removed = true;                    // set the "removed" flag to true (player now doesn't hold anything)
			}
		}
	}
}
