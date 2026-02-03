using UnityEngine;
using System.Collections;
using StansAssets.Animation;

public class Rotate : MonoBehaviour {

	public float speed;
	void Update () {
		transform.Rotate(new Vector3(0f,0f,speed * Time.deltaTime));
	}
}
