using UnityEngine;
using System.Collections;

public class GamePlay : BasePanel {
	
	private LockUnlockItems itemToUnlock;
	public GameObject[] hairs;
	public GameObject[] lens;
	public GameObject[] lips;
	public GameObject[] glasses;
	public GameObject[] dresses;
	public GameObject[] necklaces;
	public GameObject[] shoes;

	public UITexture glassTxt;
	public UITexture necklacTxt;
	public UITexture dressTxt;
	public UITexture lensTxt;
	public UITexture lipsTxt;
	public UITexture hairTxt;
	public UITexture shoeTxt;
	public GameObject[] grids;
	public GameObject sideGrid;
	public GameObject baseGrid;
	public GameObject finalButtons;
	public GameObject nextBtn;
	public GameObject shiningParticle;
	public GameObject celebrationParticle;

	void Start()
	{
		Invoke("ShowMyAd",1f);

		applyGameStateOnDecorationList(ref hairs, GameManager.Instance.hairsList);
		applyGameStateOnDecorationList(ref lens, GameManager.Instance.lensList);
		applyGameStateOnDecorationList(ref lips, GameManager.Instance.lipsList);
		applyGameStateOnDecorationList(ref dresses, GameManager.Instance.dressesList);
		applyGameStateOnDecorationList(ref glasses, GameManager.Instance.glassesList);
		applyGameStateOnDecorationList(ref necklaces, GameManager.Instance.necklacesList);
		applyGameStateOnDecorationList(ref shoes, GameManager.Instance.shoesList);
	}

	void ShowMyAd()
	{
		MRUtilities.Instance.ShowInterstitialAd();
	}

	void applyGameStateOnDecorationList(ref GameObject [] gameObjects,LockUnlockItems[] dataArray)
	{
		for (int index = 0; index < dataArray.Length; index++) {
			if(gameObjects[index] != null)
			{
				LockUnlockItems uiObject = gameObjects[index].GetComponent<LockUnlockItems>();
				uiObject.makeClone(ref dataArray[index]);
				uiObject.applyUIChanges();
			}            
		}
	}

	public void BaseButtonsClicked(GameObject btn)
	{
		SoundManager.Instance.PlayButtonClickSound();
		foreach(GameObject obj in grids)
		{
			if(obj.name == btn.name)
			{
				obj.SetActive(true);
				obj.transform.GetChild(0).GetComponent<TweenPosition>().ResetToBeginning();
				obj.transform.GetChild(0).GetComponent<TweenPosition>().PlayForward();
			}
			else
				obj.SetActive(false);
		}
	}

	public void DressItemClicked(GameObject item)
	{
		SoundManager.Instance.PlayButtonClickSound();
		itemToUnlock = item.GetComponent<LockUnlockItems>();
		if(itemToUnlock.isLocked)
		{
			MRUtilities.Instance.ShowVideoAd(RewardUser,null);
		}
		else
		{

			dressTxt.mainTexture = Resources.Load("dresses/"+item.name) as Texture;
			dressTxt.gameObject.SetActive(true);
			dressTxt.GetComponent<TweenScale>().ResetToBeginning();
			dressTxt.GetComponent<TweenScale>().PlayForward();
			ShowParticles();
		}
	}

	public void ShoeItemClicked(GameObject item)
	{
		SoundManager.Instance.PlayButtonClickSound();
		itemToUnlock = item.GetComponent<LockUnlockItems>();
		if(itemToUnlock.isLocked)
		{
			MRUtilities.Instance.ShowVideoAd(RewardUser,null);
		}
		else
		{

			shoeTxt.mainTexture = Resources.Load("shoes/"+item.name) as Texture;
			shoeTxt.gameObject.SetActive(true);
			shoeTxt.GetComponent<TweenScale>().ResetToBeginning();
			shoeTxt.GetComponent<TweenScale>().PlayForward();
			ShowParticles();
		}
	}

	public void LensItemClicked(GameObject item)
	{
		SoundManager.Instance.PlayButtonClickSound();
		itemToUnlock = item.GetComponent<LockUnlockItems>();
		if(itemToUnlock.isLocked)
		{
			MRUtilities.Instance.ShowVideoAd(RewardUser,null);
		}
		else
		{

			lensTxt.mainTexture = Resources.Load("lens/"+item.name) as Texture;
			ShowParticles();
		}
	}

	public void LipsItemClicked(GameObject item)
	{
		SoundManager.Instance.PlayButtonClickSound();
		itemToUnlock = item.GetComponent<LockUnlockItems>();
		if(itemToUnlock.isLocked)
		{
			MRUtilities.Instance.ShowVideoAd(RewardUser,null);
		}
		else
		{

			lipsTxt.mainTexture = Resources.Load("lips/"+item.name) as Texture;
			ShowParticles();
		}
	}

	public void HairsItemClicked(GameObject item)
	{
		SoundManager.Instance.PlayButtonClickSound();
		itemToUnlock = item.GetComponent<LockUnlockItems>();
		if(itemToUnlock.isLocked)
		{
			MRUtilities.Instance.ShowVideoAd(RewardUser,null);
		}
		else
		{

			hairTxt.mainTexture = Resources.Load("hairs/"+item.name) as Texture;
			hairTxt.GetComponent<TweenScale>().ResetToBeginning();
			hairTxt.GetComponent<TweenScale>().PlayForward();
			ShowParticles();
		}
	}

	public void NecklacItemClicked(GameObject item)
	{
		SoundManager.Instance.PlayButtonClickSound();
		itemToUnlock = item.GetComponent<LockUnlockItems>();
		if(itemToUnlock.isLocked)
		{
			MRUtilities.Instance.ShowVideoAd(RewardUser,null);
		}
		else
		{

			necklacTxt.mainTexture = Resources.Load("necklaces/"+item.name) as Texture;
			necklacTxt.gameObject.SetActive(true);
			necklacTxt.GetComponent<TweenScale>().ResetToBeginning();
			necklacTxt.GetComponent<TweenScale>().PlayForward();
			ShowParticles();
		}
	}


	public void GlassItemClicked(GameObject item)
	{
		SoundManager.Instance.PlayButtonClickSound();
		itemToUnlock = item.GetComponent<LockUnlockItems>();
		if(itemToUnlock.isLocked)
		{
			MRUtilities.Instance.ShowVideoAd(RewardUser,null);
		}
		else
		{

			glassTxt.mainTexture = Resources.Load("glasses/"+item.name) as Texture;
			glassTxt.gameObject.SetActive(true);
			glassTxt.GetComponent<TweenScale>().ResetToBeginning();
			glassTxt.GetComponent<TweenScale>().PlayForward();
			ShowParticles();
		}
	}

	public void NextBtnClicked()
	{
		SoundManager.Instance.PlayButtonClickSound();
		Invoke("ShowAd",1f);
		nextBtn.SetActive(false);
		sideGrid.SetActive(false);
		baseGrid.SetActive(false);
		finalButtons.SetActive(true);
		celebrationParticle.SetActive(true);
	}

	void ShowAd()
	{
		MRUtilities.Instance.ShowInterstitialAd();
	}

	public void HomeBtnClicked()
	{
		SoundManager.Instance.PlayButtonClickSound();
		GameNavigationController.Instance.PopMenuToState(GameNavigationController.GameState.MainMenu);
	}

	public void ReplayBtnClicked()
	{
		SoundManager.Instance.PlayButtonClickSound();
		GameNavigationController.Instance.PopMenuToState(GameNavigationController.GameState.PlayScene);
	}

	public void RateUsBtnClicked()
	{
		SoundManager.Instance.PlayButtonClickSound();
		Application.OpenURL("market://details?id=com.xyz.fashiondressup");
	}

	public void RewardUser()
	{
		itemToUnlock.SaveData();
	}

	void ShowParticles()
	{
		shiningParticle.GetComponent<ParticleSystem>().Play();
		SoundManager.Instance.PlaySparkleSound();
		Invoke("HideParticle",1f);
	}

	void HideParticle()
	{
		shiningParticle.GetComponent<ParticleSystem>().Play();
	}
}
