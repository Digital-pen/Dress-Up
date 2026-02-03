using UnityEngine;
using System.Collections;

public static class DataProvider
{
	public static LockUnlockItems[] GetHairsData()
	{
		ArrayList hairs = new ArrayList ();
		
		PlayerPrefs.SetString("01_hair","True");
		PlayerPrefs.SetString("02_hair","True");
		PlayerPrefs.SetString("03_hair","True");
//		PlayerPrefs.SetString("04_hair","True");
//		PlayerPrefs.SetString("05_hair","True");
//		PlayerPrefs.SetString("06_hair","True");
//		PlayerPrefs.SetString("07_hair","True");
//		PlayerPrefs.SetString("08_hair","True");

		PlayerPrefs.Save();
		
		hairs.Add (new LockUnlockItems ("01", "hair","01",0, DataProvider.ValidateItem ("01","hair")));
		hairs.Add (new LockUnlockItems ("02", "hair","02",0, DataProvider.ValidateItem ("02","hair")));
		hairs.Add (new LockUnlockItems ("03", "hair","03",0, DataProvider.ValidateItem ("03","hair")));
		hairs.Add (new LockUnlockItems ("04", "hair","04",0, DataProvider.ValidateItem ("04","hair")));
		hairs.Add (new LockUnlockItems ("05", "hair","05",0, DataProvider.ValidateItem ("05","hair")));
		hairs.Add (new LockUnlockItems ("06", "hair","06",0, DataProvider.ValidateItem ("06","hair")));
		hairs.Add (new LockUnlockItems ("07", "hair","07",0, DataProvider.ValidateItem ("07","hair")));
		hairs.Add (new LockUnlockItems ("08", "hair","08",0, DataProvider.ValidateItem ("08","hair")));

		return (LockUnlockItems[])hairs.ToArray (typeof(LockUnlockItems));
	}

	public static LockUnlockItems[] GetLensData()
	{
		ArrayList lens = new ArrayList ();
		
		PlayerPrefs.SetString("01_lens","True");
		PlayerPrefs.SetString("02_lens","True");
		PlayerPrefs.SetString("03_lens","True");
//		PlayerPrefs.SetString("04_lens","True");
//		PlayerPrefs.SetString("05_lens","True");
//		PlayerPrefs.SetString("06_lens","True");
//		PlayerPrefs.SetString("07_lens","True");
//		PlayerPrefs.SetString("08_lens","True");
		
		PlayerPrefs.Save();
		
		lens.Add (new LockUnlockItems ("01", "lens","01",0, DataProvider.ValidateItem ("01","lens")));
		lens.Add (new LockUnlockItems ("02", "lens","02",0, DataProvider.ValidateItem ("02","lens")));
		lens.Add (new LockUnlockItems ("03", "lens","03",0, DataProvider.ValidateItem ("03","lens")));
		lens.Add (new LockUnlockItems ("04", "lens","04",0, DataProvider.ValidateItem ("04","lens")));
		lens.Add (new LockUnlockItems ("05", "lens","05",0, DataProvider.ValidateItem ("05","lens")));
		lens.Add (new LockUnlockItems ("06", "lens","06",0, DataProvider.ValidateItem ("06","lens")));
		lens.Add (new LockUnlockItems ("07", "lens","07",0, DataProvider.ValidateItem ("07","lens")));
		lens.Add (new LockUnlockItems ("08", "lens","08",0, DataProvider.ValidateItem ("08","lens")));
		
		return (LockUnlockItems[])lens.ToArray (typeof(LockUnlockItems));
	}

	public static LockUnlockItems[] GetLipsData()
	{
		ArrayList lips = new ArrayList ();
		
		PlayerPrefs.SetString("01_lip","True");
		PlayerPrefs.SetString("02_lip","True");
		PlayerPrefs.SetString("03_lip","True");
//		PlayerPrefs.SetString("04_lip","True");
//		PlayerPrefs.SetString("05_lip","True");
//		PlayerPrefs.SetString("06_lip","True");
//		PlayerPrefs.SetString("07_lip","True");
//		PlayerPrefs.SetString("08_lip","True");
		
		PlayerPrefs.Save();
		
		lips.Add (new LockUnlockItems ("01", "lip","01",0, DataProvider.ValidateItem ("01","lip")));
		lips.Add (new LockUnlockItems ("02", "lip","02",0, DataProvider.ValidateItem ("02","lip")));
		lips.Add (new LockUnlockItems ("03", "lip","03",0, DataProvider.ValidateItem ("03","lip")));
		lips.Add (new LockUnlockItems ("04", "lip","04",0, DataProvider.ValidateItem ("04","lip")));
		lips.Add (new LockUnlockItems ("05", "lip","05",0, DataProvider.ValidateItem ("05","lip")));
		lips.Add (new LockUnlockItems ("06", "lip","06",0, DataProvider.ValidateItem ("06","lip")));
		lips.Add (new LockUnlockItems ("07", "lip","07",0, DataProvider.ValidateItem ("07","lip")));
		lips.Add (new LockUnlockItems ("08", "lip","08",0, DataProvider.ValidateItem ("08","lip")));
		
		return (LockUnlockItems[])lips.ToArray (typeof(LockUnlockItems));
	}

	public static LockUnlockItems[] GetDressesData()
	{
		ArrayList dresses = new ArrayList ();
		
		PlayerPrefs.SetString("01_dress","True");
		PlayerPrefs.SetString("02_dress","True");
		PlayerPrefs.SetString("03_dress","True");
//		PlayerPrefs.SetString("04_dress","True");
//		PlayerPrefs.SetString("05_dress","True");
//		PlayerPrefs.SetString("06_dress","True");
//		PlayerPrefs.SetString("07_dress","True");
//		PlayerPrefs.SetString("08_dress","True");
		
		PlayerPrefs.Save();
		
		dresses.Add (new LockUnlockItems ("01", "dress","01",0, DataProvider.ValidateItem ("01","dress")));
		dresses.Add (new LockUnlockItems ("02", "dress","02",0, DataProvider.ValidateItem ("02","dress")));
		dresses.Add (new LockUnlockItems ("03", "dress","03",0, DataProvider.ValidateItem ("03","dress")));
		dresses.Add (new LockUnlockItems ("04", "dress","04",0, DataProvider.ValidateItem ("04","dress")));
		dresses.Add (new LockUnlockItems ("05", "dress","05",0, DataProvider.ValidateItem ("05","dress")));
		dresses.Add (new LockUnlockItems ("06", "dress","06",0, DataProvider.ValidateItem ("06","dress")));
		dresses.Add (new LockUnlockItems ("07", "dress","07",0, DataProvider.ValidateItem ("07","dress")));
		dresses.Add (new LockUnlockItems ("08", "dress","08",0, DataProvider.ValidateItem ("08","dress")));
		
		return (LockUnlockItems[])dresses.ToArray (typeof(LockUnlockItems));
	}

	public static LockUnlockItems[] GetNecklacesData()
	{
		ArrayList necklaces = new ArrayList ();
		
		PlayerPrefs.SetString("01_necklac","True");
		PlayerPrefs.SetString("02_necklac","True");
		PlayerPrefs.SetString("03_necklac","True");
//		PlayerPrefs.SetString("04_necklac","True");
//		PlayerPrefs.SetString("05_necklac","True");
//		PlayerPrefs.SetString("06_necklac","True");
//		PlayerPrefs.SetString("07_necklac","True");
//		PlayerPrefs.SetString("08_necklac","True");
		
		PlayerPrefs.Save();
		
		necklaces.Add (new LockUnlockItems ("01", "necklac","01",0, DataProvider.ValidateItem ("01","necklac")));
		necklaces.Add (new LockUnlockItems ("02", "necklac","02",0, DataProvider.ValidateItem ("02","necklac")));
		necklaces.Add (new LockUnlockItems ("03", "necklac","03",0, DataProvider.ValidateItem ("03","necklac")));
		necklaces.Add (new LockUnlockItems ("04", "necklac","04",0, DataProvider.ValidateItem ("04","necklac")));
		necklaces.Add (new LockUnlockItems ("05", "necklac","05",0, DataProvider.ValidateItem ("05","necklac")));
		necklaces.Add (new LockUnlockItems ("06", "necklac","06",0, DataProvider.ValidateItem ("06","necklac")));
		necklaces.Add (new LockUnlockItems ("07", "necklac","07",0, DataProvider.ValidateItem ("07","necklac")));
		necklaces.Add (new LockUnlockItems ("08", "necklac","08",0, DataProvider.ValidateItem ("08","necklac")));
		
		return (LockUnlockItems[])necklaces.ToArray (typeof(LockUnlockItems));
	}

	public static LockUnlockItems[] GetShoesData()
	{
		ArrayList shoes = new ArrayList();
		
		PlayerPrefs.SetString("01_shoe","True");
		PlayerPrefs.SetString("02_shoe","True");
		PlayerPrefs.SetString("03_shoe","True");
//		PlayerPrefs.SetString("04_shoe","True");
//		PlayerPrefs.SetString("05_shoe","True");
//		PlayerPrefs.SetString("06_shoe","True");
//		PlayerPrefs.SetString("07_shoe","True");
//		PlayerPrefs.SetString("08_shoe","True");
		
		PlayerPrefs.Save();
		
		shoes.Add (new LockUnlockItems ("01", "shoe","01",0, DataProvider.ValidateItem ("01","shoe")));
		shoes.Add (new LockUnlockItems ("02", "shoe","02",0, DataProvider.ValidateItem ("02","shoe")));
		shoes.Add (new LockUnlockItems ("03", "shoe","03",0, DataProvider.ValidateItem ("03","shoe")));
		shoes.Add (new LockUnlockItems ("04", "shoe","04",0, DataProvider.ValidateItem ("04","shoe")));
		shoes.Add (new LockUnlockItems ("05", "shoe","05",0, DataProvider.ValidateItem ("05","shoe")));
		shoes.Add (new LockUnlockItems ("06", "shoe","06",0, DataProvider.ValidateItem ("06","shoe")));
		shoes.Add (new LockUnlockItems ("07", "shoe","07",0, DataProvider.ValidateItem ("07","shoe")));
		shoes.Add (new LockUnlockItems ("08", "shoe","08",0, DataProvider.ValidateItem ("08","shoe")));
		
		return (LockUnlockItems[])shoes.ToArray (typeof(LockUnlockItems));
	}

	public static LockUnlockItems[] GetGlassesData()
	{
		ArrayList glasses = new ArrayList();

		PlayerPrefs.SetString("01_glass","True");
		PlayerPrefs.SetString("02_glass","True");
		PlayerPrefs.SetString("03_glass","True");
//		PlayerPrefs.SetString("04_glass","True");
//		PlayerPrefs.SetString("05_glass","True");
//		PlayerPrefs.SetString("06_glass","True");
//		PlayerPrefs.SetString("07_glass","True");
//		PlayerPrefs.SetString("08_glass","True");

		PlayerPrefs.Save();

		glasses.Add (new LockUnlockItems ("01", "glass","01",0, DataProvider.ValidateItem ("01","glass")));
		glasses.Add (new LockUnlockItems ("02", "glass","02",0, DataProvider.ValidateItem ("02","glass")));
		glasses.Add (new LockUnlockItems ("03", "glass","03",0, DataProvider.ValidateItem ("03","glass")));
		glasses.Add (new LockUnlockItems ("04", "glass","04",0, DataProvider.ValidateItem ("04","glass")));
		glasses.Add (new LockUnlockItems ("05", "glass","05",0, DataProvider.ValidateItem ("05","glass")));
		glasses.Add (new LockUnlockItems ("06", "glass","06",0, DataProvider.ValidateItem ("06","glass")));
		glasses.Add (new LockUnlockItems ("07", "glass","07",0, DataProvider.ValidateItem ("07","glass")));
		glasses.Add (new LockUnlockItems ("08", "glass","08",0, DataProvider.ValidateItem ("08","glass")));

		return (LockUnlockItems[])glasses.ToArray (typeof(LockUnlockItems));
	}

	public static bool ValidateItem (string id, string name)
	{
		if (PlayerPrefs.GetString (id + "_" + name, "false").CompareTo ("True") != 0) 
		{
			return true;
		}
		return false;
	}
	
}

