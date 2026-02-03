using UnityEngine;
using System.Collections;

public class DoughController : MonoBehaviour {
	
	public GameObject colName;
	public GameObject objToNotify;

	void OnTriggerEnter(Collider col)
	{
		if(col.name == colName.name)
		{
			if(col.transform.localScale.x < 1.5)
				TweenScale.Begin(col.gameObject,1f,new Vector3(col.transform.localScale.x+.1f,col.transform.localScale.y+.1f,0));
			else
			{
				col.enabled=false;
				objToNotify.SendMessage("ToolApplyDone",SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
