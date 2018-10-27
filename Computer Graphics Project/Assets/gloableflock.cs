using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gloableflock : MonoBehaviour {
	public GameObject fishprefab;
	public  static int tanksize =7;
    static int numFish =20;
	public static Vector3 goalpos = Vector3.zero;
	public static GameObject[] allfish = new GameObject[numFish ];
	public Transform fishtank;
		
	// Use this for initialization
	void Start () {
		
//		RenderSettings.fogColor = Camera.main.backgroundColor;
//		RenderSettings.fogDensity = 0.03F;
//		RenderSettings.fog = true;
		for (int i = 0; i < numFish; i++) {
			//10 fish at random position. vector3 pos means local coordinate such as(5,3 ,2) 
			Vector3 pos = new Vector3 (Random.Range (-tanksize, tanksize),
				             Random.Range (-tanksize, tanksize),
				             Random.Range (-tanksize, tanksize));
	
			//allfish [i] = (GameObject)Instantiate (fishprefab, fishtank.position+pos, Quaternion.identity);}
			allfish [i] = (GameObject)Instantiate (fishprefab, pos, Quaternion.identity);}
	}
	
	// Update is called once per frame
	void Update () {
		bool fishfollow = fishspeed.fishfollow;
		if (fishfollow) {
			tanksize = 5;
		}
		if (fishfollow != true) {
			if (Random.Range (0, 10000) < 50) { //5%chance to change the center 
				goalpos = new Vector3 (Random.Range (-tanksize, tanksize),
					Random.Range (-tanksize, tanksize),
					Random.Range (-tanksize, tanksize));
			
				//if(Player pick up food){
				//	goalpos.transform.position = food;}
			}
		}
	}
}
