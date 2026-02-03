using UnityEngine;
using System.Collections;


public class BaseItem : MonoBehaviour {

	public string id;
	public string name;
	public string imageName;
	public int coinPrice;
	public bool isLocked;
	
	public BaseItem(string id, string name, string imageName, int coinPrice, bool isLocked)
	{
		this.id = id;
		this.name = name;
		this.imageName = imageName;
		this.coinPrice = coinPrice;
		this.isLocked = isLocked;
		
	}
	

	public void SaveData()
	{
		PlayerPrefs.SetString(id + "_"+name,"True");
		PlayerPrefs.Save();
		isLocked = false;
	}
}
