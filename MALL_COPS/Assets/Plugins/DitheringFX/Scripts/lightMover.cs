using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightMover : MonoBehaviour {
	public float y = 1;
	public float speed = 1;
	public float radius = 4;
	void Update () {
		transform.position = new Vector3( 
			Mathf.Sin(Time.time*speed)*radius,
			y,
			Mathf.Cos(Time.time*speed)*radius
		);
	}
}
