using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MR {

	static int logCounter = 1;

	public static void Log(string p_text) {

		if (GameObject.Find("MRLogScrollView"))
		{
			GameObject logItemPrefab = Resources.Load<GameObject>("MRResources/LogItem");
			GameObject newLogItem = GameObject.Instantiate(logItemPrefab, GameObject.Find("MRLogScrollView").transform.Find("Grid").gameObject.transform);
			newLogItem.transform.localScale = logItemPrefab.transform.localScale;
			newLogItem.transform.localPosition = Vector3.zero;
			newLogItem.transform.SetAsFirstSibling();
			newLogItem.transform.Find("Text").GetComponent<Text>().text = "MRINFO - " + p_text;
			newLogItem.transform.Find("TextNumber").GetComponent<Text>().text = logCounter++.ToString();

		}

		Debug.Log("<color=yellow>"+"MRINFO - \""+p_text+"\"</color>");
	}

	public static void LogError(string p_text) {
		if (GameObject.Find("MRLogScrollView"))
		{
			GameObject logItemPrefab = Resources.Load<GameObject>("MRResources/LogItem");
			GameObject newLogItem = GameObject.Instantiate(logItemPrefab, GameObject.Find("MRLogScrollView").transform.Find("Grid").gameObject.transform);
			newLogItem.transform.localScale = logItemPrefab.transform.localScale;
			newLogItem.transform.localPosition = Vector3.zero;
			newLogItem.transform.SetAsFirstSibling();
			newLogItem.transform.Find("Text").GetComponent<Text>().text = "MRERROR - " + p_text;
			newLogItem.transform.Find("TextNumber").GetComponent<Text>().text = logCounter++.ToString();
		}

		Debug.Log("<color=orange>"+"MRERROR - \""+p_text+"\"</color>");
	}

}
