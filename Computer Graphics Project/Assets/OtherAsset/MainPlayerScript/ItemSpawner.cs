using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : NetworkBehaviour {
	public GameObject itemPrefab;
	public float xPosition;
	public float yPosition;
	public float zPosition;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnStartServer()
	{
		var spawnPosition = new Vector3 (xPosition, yPosition, zPosition);
		var spawnRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		var item = (GameObject)Instantiate(itemPrefab, spawnPosition, spawnRotation);
		NetworkServer.Spawn (item);
	}
}
