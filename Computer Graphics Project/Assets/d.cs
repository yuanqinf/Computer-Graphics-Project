using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class d : MonoBehaviour {
	public Animator anim2;
	public int a2;

	// Use this for initialization
	void Start () {
		anim2 = GetComponent<Animator>();

	}
	void OnTriggerEnter(Collider other)
	{ if (other.tag == "bullet") {

			a2=1;

		}
	}

	// Update is called once per frame
	void Update () {
		if (a2 == 1)
		{

			anim2.Play("Take 001");


		}
	}
}