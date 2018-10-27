using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : MonoBehaviour {

	// Use this for initialization
	public void Generated(int[] index)
	{
		this.transform.position += this.transform.up * this.transform.localScale.y;
	}
}
