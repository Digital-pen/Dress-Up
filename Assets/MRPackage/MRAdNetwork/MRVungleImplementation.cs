using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRVungleImplementation : BasicAdNetwork
{
	
	public string ANDROIDID;
	public string IOSID;

	bool videoRequested = false;
	bool videoLoaded = false;

	bool videoForced = false;

	public override bool IsVideoAdRequested()
	{
		return videoRequested;
	}

	public override bool IsVideoAdLoaded()
	{
		return videoLoaded;
	}

	void OnEnable()
	{
		Vungle.adPlayableEvent += HandleadPlayableEvent;
		Vungle.onAdFinishedEvent += HandleonAdFinishedEvent;
	}

	void OnDisable()
	{
		Vungle.adPlayableEvent -= HandleadPlayableEvent;
		Vungle.onAdFinishedEvent -= HandleonAdFinishedEvent;
	}

	IEnumerator CheckTimeOutVideoAd() {
		yield return new WaitForSeconds(5f);
		if (!videoLoaded)
		{
			MR.Log("Vungle VideoAd Failed Timed Out");
			videoRequested = false;
			videoLoaded = false;
			GetComponent<MRUtilities>().VideoAdLoadFailed(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_REWARDED_VIDEO, Constants.GA_LABEL_CB_DL_AD_Failed);
		}
	}

	public override void RequestVideoAd()
	{
		if (!videoRequested)
		{
			MR.Log("Vungle Video Requested");
			videoRequested = true;
			videoLoaded = false;

			Vungle.clearCache();
			Vungle.clearSleep();
			Vungle.init(ANDROIDID, IOSID, "");

			StartCoroutine(CheckTimeOutVideoAd());

			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_VUNGLE_IV_Requested);
		}
	}

	public override void ShowVideoAd()
	{
		Vungle.playAd(true);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_VUNGLE_IV_Shown);
	}

	public override void ShowForcedVideoAd()
	{
		if (!videoRequested && !videoLoaded)
		{
			MR.Log("Vungle Video Forced");
			Vungle.clearCache();
			Vungle.clearSleep();
			Vungle.init(ANDROIDID, IOSID, "");

			videoRequested = true;
			videoLoaded = false;

			videoForced = true;
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_REWARDED_VIDEO, Constants.GA_LABEL_VUNGLE_FORCEDVID_REQUESTED);
		}
	}

	void HandleadPlayableEvent(bool isReady)
	{
		if (isReady)
		{
			//loaded
			videoRequested = false;
			videoLoaded = true;

			if (!videoForced)
			{
				MR.Log("Vungle Video Loaded");
				GetComponent<MRUtilities>().VideoAdLoadSuccess(this);
				MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_VUNGLE_IV_Loaded);
			}
			else
			{
				MR.Log("Vungle Forced Video Played");
				Vungle.playAd(true);
				MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_VUNGLE_IV_Shown);
			}
		}
		else
		{
			//failed
			MR.Log("Vungle Video Failed");
			videoRequested = false;
			videoLoaded = false;
			GetComponent<MRUtilities>().VideoAdLoadFailed(this);
			if (videoForced)
				videoForced = false;
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_VUNGLE_IV_Failed);
		}
	}

	void HandleonAdFinishedEvent(AdFinishedEventArgs adFinished)
	{
		if (adFinished.IsCompletedView)
		{
			MR.Log("Vungle Video Completed");
			videoRequested = false;
			videoLoaded = false;

			if (videoForced)
				videoForced = false;

			GetComponent<MRUtilities>().VideoAdCompleted(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_VUNGLE, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_VUNGLE_IV_Completed);
	
		}
	}
}
