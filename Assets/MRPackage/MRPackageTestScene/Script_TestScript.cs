using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_TestScript : MonoBehaviour {


	public void Button_ShowInterstitialClicked() {
		MRUtilities.Instance.ShowInterstitialAd(false);
	}

	public void Button_ShowRewardedVideoClicked() {
		MRUtilities.Instance.ShowVideoAd(null,null);
	}


	public void Button_RestartLevelClicked() {
		SceneManager.LoadScene(0);
	}
		
}
