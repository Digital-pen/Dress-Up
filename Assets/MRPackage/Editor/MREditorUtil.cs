using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor the "old" way by modifying the script variables directly.
// No handling of multi-object editing, undo, and prefab overrides!
[CustomEditor (typeof(MREditorUtil))]
public class MREditorUtil : Editor {
	
	[MenuItem ("MREditorUtilities/Selection/Toggle Selected Objects %#a")]
	static void Toggle()
	{
		foreach (GameObject go in Selection.gameObjects)
		{
			if (go.activeSelf)
				go.SetActive(false);
			else
				go.SetActive(true);
		}
	}
	
	[MenuItem ("MREditorUtilities/Delete Preferences %#-")]
	static void DeletePreferences()
	{
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();

		Caching.ClearCache ();
	}

	[MenuItem ("MREditorUtilities/GameObject/Reset Local Position %#r")]
	static void ResetPosition()
	{
		foreach (GameObject go in Selection.gameObjects)
		{
			go.transform.localPosition = Vector3.zero;
		}
	}
}