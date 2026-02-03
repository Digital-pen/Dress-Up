using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	List<GameObject> panels;
	Stack<GameObject> navPanel = new Stack<GameObject>();

	static GUIManager _instance;
	public static GUIManager Instance
	{
		get {
			return _instance;
		}
	}

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
		if (GameObject.FindObjectsOfType<GUIManager>().Length > 1)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;

		panels = new List<GameObject>();
		foreach (ScreenName obj in FindObjectsOfType<ScreenName>())
			panels.Add(obj.gameObject);	

		#if UNITY_IOS
		Application.targetFrameRate = 60;
		#endif
	}

	void Start()
	{
		StartGUI();
	}

	public void btnBack()
	{
		if (navPanel != null && navPanel.Count > 0)
		{
			GameObject toHidePanel = navPanel.Pop();
			toHidePanel.SetActive(false);
			showCurrentPanel();
		}
		else
		{
			Debug.Log("MRERROR - nav panel is null or zero count in BACK");
		}
	}

	public GameObject CURRENTPANEL
	{
		get
		{
			GameObject currentPanel;
			currentPanel = navPanel.Peek();
			return currentPanel;
		}
		set
		{
		}
	}

	public ScreenNameEnum CURRENTSCREENNAME
	{
		get
		{
			GameObject currentPanel;
			currentPanel = navPanel.Peek();
			return currentPanel.GetComponent<ScreenName>().screenName;
		}
	}

	private void hideCurrentPanel()
	{
		GameObject currentPanel;
		currentPanel = navPanel.Peek();
		currentPanel.SetActive(false);
	}

	private void clearStack()
	{
		foreach(GameObject panel in navPanel)
			panel.SetActive(false);
		navPanel.Clear();
	}

	private void showCurrentPanel()
	{
		if (navPanel != null && navPanel.Count > 0)
		{
			GameObject currentPanel;
			currentPanel = navPanel.Peek();
			currentPanel.SetActive(true);
		}
		else
		{
			Debug.Log("MRERROR - nav panel is null or zero count in ShowCurrentPanel");
		}
	}

	public void OpenScreenExplicitly(ScreenNameEnum screenName)
	{
		GameObject screenToOpen = panels.Find(asd => asd.GetComponent<ScreenName>().screenName == screenName);
		if (navPanel != null && navPanel.Count > 0)
		{
			if (navPanel.Peek().name != screenToOpen.name)
				OpenScreen(screenToOpen);
		}
		else
		{
			Debug.Log("MRERROR - nav panel is null or zero count in OpenScreenExplicitly");
		}
	}

	public void OpenScreen(GameObject screenToOpen)
	{
		bool fullScreen = screenToOpen.GetComponent<ScreenName>().fullScreen;

		int index = panels.FindIndex(p => p.GetComponent<ScreenName>().screenName == screenToOpen.GetComponent<ScreenName>().screenName);
		if (fullScreen) {

			CURRENTPANEL.SetActive(false);
		}

		navPanel.Push(panels[index]);
		showCurrentPanel();

	}

	private void StartGUI()
	{
		foreach(GameObject panel in panels)
			panel.SetActive(false);
		navPanel.Clear();

		navPanel.Push (panels.Find(asd => asd.GetComponent<ScreenName>().firstScreen));
		showCurrentPanel ();
	}
}
