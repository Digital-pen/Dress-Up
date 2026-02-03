using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StansAssets.Animation;

public class ScaleUpDown : MonoBehaviour {

	Vector3 bigScale = new Vector3(0.9f,0.9f,0.9f);
	Vector3 normalScale = Vector3.one;
	Hashtable ht = null;
	void OnEnable() {
		ht = new Hashtable();
		ToBig();
	}

	void ToBig() {
		ht.Clear();
		ht.Add("scale",bigScale);
		ht.Add("time",0.5f);
		ht.Add("oncomplete","ToBigDone");
		ht.Add("easetype",iTween.EaseType.linear);
		iTween.ScaleTo(gameObject,ht);
	}

	void ToBigDone() {
		ToNormal();
	}

	void ToNormal() {
		ht.Clear();
		ht.Add("scale",normalScale);
		ht.Add("time",0.5f);
		ht.Add("oncomplete","ToNormalDone");
		ht.Add("easetype",iTween.EaseType.linear);
		iTween.ScaleTo(gameObject,ht);
	}

	void ToNormalDone() {
		ToBig();
	}
}
