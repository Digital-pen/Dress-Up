using UnityEngine;
using System.Collections;

public class MainMenuPanel : BasePanel {

	void Start()
	{
		
	}
	
	public void PlayBtnClicked()
	{
		SoundManager.Instance.PlayButtonClickSound();
		GameNavigationController.Instance.PushPanel (GameNavigationController.GameState.PlayScene);
	}
	
	public void RateUsBtnClicked()
	{
		SoundManager.Instance.PlayButtonClickSound();
		Application.OpenURL("market://details?id=com.xyz.fashiondressup");
	}

}


