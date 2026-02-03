using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MRAppLovinImplementation : BasicAdNetwork
{

	bool interstitialRequested = false;
	bool interstitialLoaded = false;

	bool videoRequested = false;
	bool videoLoaded = false;

	public string SDKKEY;

	public override bool IsVideoAdRequested()
	{
		return videoRequested;
	}

	public override bool IsVideoAdLoaded()
	{
		return videoLoaded;
	}

	public override bool IsInterstitialRequested()
	{
		return interstitialRequested;
	}

	public override bool IsInterstitialLoaded()
	{
		return interstitialLoaded;
	}

	void Start()
	{
		#if !UNITY_EDITOR
			AppLovin.SetSdkKey(SDKKEY);
			AppLovin.InitializeSdk();
			AppLovin.SetUnityAdListener("MRUtilities");
		#endif
	}

	public override void RequestInterstitial()
	{
		if (!interstitialRequested)
		{
			#if !UNITY_EDITOR
			MR.Log("AppLovin Interstitial Requested");
			AppLovin.PreloadInterstitial();
			interstitialRequested = true;
			interstitialLoaded = false;
			StartCoroutine(CheckTimeOutInterstitial());
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN,Constants.GA_ACTION_INTERSTITIAL,Constants.GA_LABEL_APPLOVIN_INT_Requested);
			#endif
		}
	}

	IEnumerator CheckTimeOutInterstitial()
	{
		yield return new WaitForSeconds(4f);
		if (!interstitialLoaded)
		{
			MR.Log("AppLovin Interstitial Failed Timed Out");
			interstitialRequested = false;
			interstitialLoaded = false;
			GetComponent<MRUtilities>().InterstitialLoadFailed(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_APPLOVIN_INT_Failed);
		}
	}

	public override void RequestVideoAd()
	{
		#if !UNITY_EDITOR
		MR.Log("AppLovin Video Requested");
		AppLovin.LoadRewardedInterstitial();
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN,Constants.GA_ACTION_VIDEO,Constants.GA_LABEL_APPLOVIN_VIDEO_Requested);
		#endif
	}

	public override void ShowInterstitialAd()
	{
		AppLovin.ShowInterstitial();
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_APPLOVIN_INT_Shown);
	}

	public override void ShowVideoAd()
	{
		AppLovin.ShowRewardedInterstitial();
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_APPLOVIN_VIDEO_Shown);
	}

	void onAppLovinEventReceived(string ev)
	{
		if (ev.Contains("REWARDAPPROVEDINFO"))
		{
			MR.Log("AppLovin Video Completed");
			videoRequested = false;
			videoLoaded = false;
			GetComponent<MRUtilities>().VideoAdCompleted(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_APPLOVIN_VIDEO_Completed);
		}
		else if (ev.Contains("LOADEDREWARDED"))
		{
			MR.Log("AppLoving Video Loaded");
			videoRequested = false;
			videoLoaded = true;
			GetComponent<MRUtilities>().VideoAdLoadSuccess(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_APPLOVIN_VIDEO_Cached);
		}
		else if (ev.Contains("LOADREWARDEDFAILED"))
		{
			MR.Log("AppLovin Video Failed");
			videoRequested = false;
			videoLoaded = false;
			GetComponent<MRUtilities>().VideoAdLoadFailed(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_APPLOVIN_VIDEO_Failed);
		}
		else if (ev.Contains("HIDDENREWARDED"))
		{
		}
		else if (ev.Contains("DISPLAYEDINTER"))
		{
			interstitialRequested = false;
			interstitialLoaded = false;
			GetComponent<MRUtilities>().InterstitialOpened();
		}
		else if (ev.Contains("LOADEDINTER"))
		{
			MR.Log("AppLovin Loaded");
			interstitialRequested = false;
			interstitialLoaded = true;
			GetComponent<MRUtilities>().InterstitialLoadSuccess(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_APPLOVIN_INT_Cached);
		}
		else if (string.Equals(ev, "LOADINTERFAILED"))
		{
			MR.Log("AppLovin Failed");
			interstitialRequested = false;
			interstitialLoaded = false;
			GetComponent<MRUtilities>().InterstitialLoadFailed(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_APPLOVIN, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_APPLOVIN_INT_Failed);
		}
	}
}
