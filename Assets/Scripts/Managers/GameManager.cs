using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{

	public LockUnlockItems[] hairsList;
	public LockUnlockItems[] lensList;
	public LockUnlockItems[] lipsList;
	public LockUnlockItems[] dressesList;
	public LockUnlockItems[] necklacesList;
	public LockUnlockItems[] shoesList;
	public LockUnlockItems[] glassesList;

	#region Player
	// The Player
	public Player player;
	
	#endregion Player
	
	#region Pause
	
	public bool isPaused = false;
	
	// The Sound Manager	
	public bool isSoundPaused;
	
	public void PauseGame ()
	{
		isPaused = true;
		Time.timeScale = 0.0f;
	}
	
	public void UnPauseGame ()
	{
		isPaused = false;
		Time.timeScale = 1.0f;
	}
	
	#endregion Pause
	
	#region Mono Life Cycle
	
	void Awake ()
	{
		GetData();
		if (GameObject.FindGameObjectsWithTag ("GameManager").Length > 1) 
		{
			Destroy (gameObject);
		} 
		else 
		{
			DontDestroyOnLoad (gameObject);
		}
		
		GameManager.Instance.isSoundPaused = false;
	}

	void GetData()
	{
		hairsList = DataProvider.GetHairsData();
		lensList = DataProvider.GetLensData();
		lipsList = DataProvider.GetLipsData();
		dressesList = DataProvider.GetDressesData();
		shoesList = DataProvider.GetShoesData();
		necklacesList = DataProvider.GetNecklacesData();
		glassesList = DataProvider.GetGlassesData();
	}

	public void OnDestroy ()
	{
		if (GameObject.FindGameObjectsWithTag ("GameManager").Length < 1) {
			applicationIsQuitting = true;
		}
	}
	
	
	#endregion Mono Life Cycle
}
