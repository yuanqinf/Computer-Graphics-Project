using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishspeed : MonoBehaviour {
	public float speed = 0.001f;
	float rotationspeed=2.0f;//fish make turn
	Vector3 average_Heading;// average heading direction of the whole fish group
	Vector3 average_Position;
	float neighbour_Distance=3.0f;
	public Vector3 newGoalPos;
	public Vector3 goalpos;
	public GameObject food; 
	public static bool fishfollow= false;
	bool turning =false ;//will be set to true if get the edge of tank(鱼群）

	void Start () {

		food =  GameObject.Find("Cube");

		speed = Random.Range (0.8f, 1);// **change**
		//speed = Random.Range (0.8f, 2.0f);
		//this.GetComponent<Animation> () ["Take 001"].speed = speed;
	}

//	void OnTriggerEnter(Collider other)
//	{ if (fishfollow != true) {
//			if (!turning) {
//				newGoalPos = this.transform.position - other.gameObject.transform.position;
//			 
//			}
//			speed = 5.0f;
//			turning = true;
//		}
//	}
//
//	void OnTriggerExit(Collider other)
//	{if(fishfollow!=true){
//		turning = false;// no longer inside a collider. goes back to flocking behavior
//		}
//	}
	// Update is called once per frame
	void Update () {
		if(fishfollow){
			speed = Random.Range (0.8f, 2.0f);

		}
			
		if (food.transform.position.z < 14.0f && food.transform.position.y < 1.0f) {
			fishfollow = true;

		}

			if (Vector3.Distance (transform.position, Vector3.zero) >= gloableflock.tanksize) {
				turning = true;
			} else {
				turning = false;
			}
	


		if (turning) {
			Vector3 direction = Vector3.zero - transform.position;
			transform.rotation = Quaternion.Slerp (transform.rotation,
				Quaternion.LookRotation (direction),
				rotationspeed * Time.deltaTime);
				speed=Random.Range(0.1f,0.5f);
			if(fishfollow){
				speed = Random.Range (0.8f, 2.0f);

			}


		}
//		if (turning) {
//						Vector3 direction = newGoalPos - transform.position;
//						transform.rotation = Quaternion.Slerp (transform.rotation,
//							Quaternion.LookRotation (direction),
//							rotationspeed * Time.deltaTime);
//							speed=Random.Range(0.8f,2.0f); //change speed after turn
//			this.GetComponent<Animation>()["Take 001"].speed=speed;
//					}

		else{

		//1 in 5 times apply the rules to randomize the fish behavior
       
		if(Random.Range(0,5)<1) //**change**
			{ ApplyRules();}
		}
		transform.Translate (0, 0, Time.deltaTime * speed);
	}
	void ApplyRules(){
		GameObject[] gos;// fish prefab array in the "gloableflock" script
		gos = gloableflock.allfish;
		Vector3 vcentre = Vector3.zero;// fishes swim to the center
		Vector3 vavoid = Vector3.zero; //avoid hitting each other 
		float gSpeed = 0.1f;// *****change

		goalpos = gloableflock.goalpos;
		if (fishfollow) {
			goalpos = food.transform.position;
		}
		float dist;

		int groupSize = 0;
		foreach (GameObject go in gos) {
			//compare the distance with other fish.
			if (go != this.gameObject) {
				dist = Vector3.Distance (go.transform.position, this.transform.position);
				if (dist <= neighbour_Distance) {
					vcentre += go.transform.position;
					groupSize++;

					//if the distance if < 1  then 2 fish is going to collide, avoid collision.
					if(fishfollow){
					if (dist < 0.5f) {//***change***
						vavoid = vavoid + (this.transform.position - go.transform.position);
						}}
					if (dist < 1.0f) {//***change***
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}
					// go is the fish prefab inside the fish array 
					fishspeed anotherflock = go.GetComponent<fishspeed> ();// fish prefab
					gSpeed = gSpeed + anotherflock.speed;
				}
			}
		}
			if(groupSize>0)
			{
				vcentre =vcentre /groupSize +(goalpos -this.transform.position);
				speed = gSpeed / groupSize;
			 //this.GetComponent<Animation>()["Take 001"].speed=speed;
				Vector3 direction= (vcentre+vavoid)- transform.position;
				if (direction != Vector3.zero) {
					// slowly make the fish turn to the direction  slerp is interpolating
					transform.rotation=Quaternion.Slerp(transform.rotation,
						Quaternion.LookRotation(direction), 
						rotationspeed *Time.deltaTime);}

			}
		}
     }

