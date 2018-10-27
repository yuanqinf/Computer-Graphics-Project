using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	// Use this for initialization
	public float angle = 30;
	public void Generated(int[] index)
	{
		if (index[1] == 2)
		{
			this.transform.rotation *= Quaternion.Euler((angle * ((index[0]*2) -1)), Random.Range(30, 80), Random.Range(30, 80));
		}
		if (index[1] == 3)
		{
			this.transform.rotation *= Quaternion.Euler((angle * ((index[0]*2) -1))-45, Random.Range(30, 80), Random.Range(30, 80));
		}
		else
		{
			this.transform.rotation *= Quaternion.Euler((angle * ((index[0]*2) -1))-(90-90/index[1]), Random.Range(30, 80)*index[0], Random.Range(30, 80)*index[0]);
		}
		

	}
}
