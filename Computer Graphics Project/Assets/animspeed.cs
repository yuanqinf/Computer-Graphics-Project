using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animspeed : MonoBehaviour {
	Animator anim;
	AnimatorStateInfo animatorinfo;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//a["Take 001"].time = a["Take 001"].length;
		animatorinfo=anim.GetCurrentAnimatorStateInfo(0);
		if(animatorinfo.IsName("Take 001")){
			anim.speed = 5;
	}
}
}