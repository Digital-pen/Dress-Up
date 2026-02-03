using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Debug = UnityEngine.Debug;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;

/// <summary>
/// Game class holds all the game specific methods and data. E.g game settings data, InApps. The User object will also be initialized from this class and this class will contain the user object.
/// </summary>
public class Game
{

	#region singleton initialization

	private static Game instance;

	private Game()
	{
	}

	public static Game Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Game();
				if (!IsGameInitialized)
					InitializeGame();
			}
			return instance;
		}
	}

	static bool IsGameInitialized = false;
	/// <summary>
	/// Initializes the Game. Should only be called once.
	/// </summary>
	public static void InitializeGame()
	{
		if (IsGameInitialized)
			return;

		if (!PlayerPrefs.HasKey("prefsSet"))
		{
			PlayerPrefs.SetInt("prefsSet", 1);
			IsGameInitialized = true;
		} 
	}

	#endregion

	public bool soundEnabled
	{
		get
		{
			if (!PlayerPrefs.HasKey("soundEnabled"))
				PlayerPrefs.SetInt("soundEnabled", 1);

			if (PlayerPrefs.GetInt("soundEnabled") == 1)
				return true;
			return false;
		}
		set
		{
			if (value)
				PlayerPrefs.SetInt("soundEnabled", 1);
			else
				PlayerPrefs.SetInt("soundEnabled", 0);
		}
	}

	public bool showAds
	{
		get
		{
			if (!PlayerPrefs.HasKey("showAds"))
				PlayerPrefs.SetInt("showAds", 1);

			if (PlayerPrefs.GetInt("showAds") == 1)
				return true;
			return false;
		}
		set
		{
			if (value)
				PlayerPrefs.SetInt("showAds", 1);
			else
				PlayerPrefs.SetInt("showAds", 0);
		}
	}

	public bool rateClicked
	{
		get
		{
			if (!PlayerPrefs.HasKey("rateClicked"))
				PlayerPrefs.SetInt("rateClicked", 0);

			if (PlayerPrefs.GetInt("rateClicked") == 1)
				return true;
			return false;
		}
		set
		{
			if (value)
				PlayerPrefs.SetInt("rateClicked", 1);
			else
				PlayerPrefs.SetInt("rateClicked", 0);
		}
	}

	public void AllAdsRemoved()
	{
		Game.Instance.showAds = false;
		MRUtilities.Instance.DestroyBannerAd();
	}
}


