using UnityEngine;
using System.Collections;

public class LockUnlockItems : BaseItem
{
	
		public GameObject lock_image;

		public LockUnlockItems (string id, string name, string imageName, int coinPrice, bool isLocked) : base(id, name, imageName, coinPrice,isLocked)
		{
		
		}
	
		public void makeClone (ref LockUnlockItems cloneObject)
		{
				this.id = cloneObject.id;
				this.name = cloneObject.name;
				this.imageName = cloneObject.imageName;
				this.coinPrice = cloneObject.coinPrice;
				this.isLocked = cloneObject.isLocked;
		}
	
		public void applyUIChanges ()
		{
			isLocked = (PlayerPrefs.GetString (id + "_" + name, "false").CompareTo ("True") != 0) ? true : false; 
			
			if (lock_image != null) 
			{
				if (isLocked)
					lock_image.SetActive (true);
				else
					lock_image.SetActive (false);
			}
		}

		public void SaveData ()
		{
				PlayerPrefs.SetString (id + "_" + name, "True");
				PlayerPrefs.Save ();
				isLocked = false;
				applyUIChanges ();
		}
	

}
