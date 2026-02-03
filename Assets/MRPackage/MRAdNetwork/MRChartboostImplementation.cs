using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartboostSDK;

public class MRChartboostImplementation : BasicAdNetwork
{

	bool interstitialRequested = false;
	bool interstitialLoaded = false;

	bool videoRequested = false;
	bool videoLoaded = false;

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

	void OnEnable()
	{
		// Listen to all impression-related events
		Chartboost.didFailToLoadInterstitial += Chartboost_didFailToLoadInterstitial;

		Chartboost.didDismissInterstitial += Chartboost_didDismissInterstitial;

		Chartboost.didCloseInterstitial += Chartboost_didCloseInterstitial;

		Chartboost.didClickInterstitial += Chartboost_didClickInterstitial;

		Chartboost.didCacheInterstitial += Chartboost_didCacheInterstitial;

		Chartboost.didFailToLoadRewardedVideo += Chartboost_didFailToLoadRewardedVideo;

		Chartboost.didDismissRewardedVideo += Chartboost_didDismissRewardedVideo;

		Chartboost.didCloseRewardedVideo += Chartboost_didCloseRewardedVideo;

		Chartboost.didClickRewardedVideo += Chartboost_didClickRewardedVideo;

		Chartboost.didCacheRewardedVideo += Chartboost_didCacheRewardedVideo;

		Chartboost.didCompleteRewardedVideo += Chartboost_didCompleteRewardedVideo;

		#if UNITY_IPHONE
		//Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
		#endif
	}

	void OnDisable()
	{
		Chartboost.didFailToLoadInterstitial -= Chartboost_didFailToLoadInterstitial;

		Chartboost.didDismissInterstitial -= Chartboost_didDismissInterstitial;

		Chartboost.didCloseInterstitial -= Chartboost_didCloseInterstitial;

		Chartboost.didClickInterstitial -= Chartboost_didClickInterstitial;

		Chartboost.didCacheInterstitial -= Chartboost_didCacheInterstitial;

		Chartboost.didFailToLoadRewardedVideo -= Chartboost_didFailToLoadRewardedVideo;

		Chartboost.didDismissRewardedVideo -= Chartboost_didDismissRewardedVideo;

		Chartboost.didCloseRewardedVideo -= Chartboost_didCloseRewardedVideo;

		Chartboost.didClickRewardedVideo -= Chartboost_didClickRewardedVideo;

		Chartboost.didCacheRewardedVideo -= Chartboost_didCacheRewardedVideo;

		Chartboost.didCompleteRewardedVideo -= Chartboost_didCompleteRewardedVideo;

		#if UNITY_IPHONE
		//Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
		#endif
	}

	void Chartboost_didCompleteRewardedVideo(CBLocation arg1, int arg2)
	{
		MR.Log("Chartboost Video Completed");
		videoRequested = false;
		videoLoaded = false;
		GetComponent<MRUtilities>().VideoAdCompleted(this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_CB_RV_AD_Completed);
	}

	void Chartboost_didCacheRewardedVideo(CBLocation obj)
	{
		MR.Log("Chartboost Video Loaded");
		videoRequested = false;
		videoLoaded = true;
		GetComponent<MRUtilities>().VideoAdLoadSuccess(this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_CB_RV_AD_Cached);
	}

	void Chartboost_didClickRewardedVideo(CBLocation obj)
	{
			
	}

	void Chartboost_didCloseRewardedVideo(CBLocation obj)
	{
			
	}

	void Chartboost_didDismissRewardedVideo(CBLocation obj)
	{
		//AndroidMessage.Create("Chartboost","Video Dismissed");

	}

	void Chartboost_didFailToLoadRewardedVideo(CBLocation arg1, CBImpressionError arg2)
	{
		MR.Log("Chartboost Video Failed");
		videoRequested = false;
		videoLoaded = false;
		GetComponent<MRUtilities>().VideoAdLoadFailed(this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_CB_RV_AD_Failed);
	}

	void Chartboost_didCacheInterstitial(CBLocation obj)
	{
		MR.Log("Chartboost Inter Loaded");
		interstitialRequested = false;
		interstitialLoaded = true;
		GetComponent<MRUtilities>().InterstitialLoadSuccess(this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_CB_DL_AD_Cached);
	}

	void Chartboost_didClickInterstitial(CBLocation obj)
	{
			
	}

	void Chartboost_didCloseInterstitial(CBLocation obj)
	{
		interstitialRequested = false;
		interstitialLoaded = false;
		GetComponent<MRUtilities>().InterstitialOpened();

	}

	void Chartboost_didDismissInterstitial(CBLocation obj)
	{
			
	}

	void Chartboost_didFailToLoadInterstitial(CBLocation arg1, CBImpressionError arg2)
	{
		MR.Log("Chartboost Interstitial Failed");
		interstitialRequested = false;
		interstitialLoaded = false;
		GetComponent<MRUtilities>().InterstitialLoadFailed(this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_CB_DL_AD_Failed);
	}

	IEnumerator CheckTimeOutInterstitial() {
		yield return new WaitForSeconds(4f);
		if (!interstitialLoaded)
		{
			MR.Log("Chartboost Interstitial Failed Timed Out");
			//Interstitial Failed Loading Case
			interstitialRequested = false;
			interstitialLoaded = false;
			GetComponent<MRUtilities>().InterstitialLoadFailed(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_CB_DL_AD_Failed);
		}
	}

	public override void RequestInterstitial()
	{
		if (!interstitialRequested)
		{
			MR.Log("Chartboost Interstitital Requested");
			Chartboost.cacheInterstitial(CBLocation.Default);
			StartCoroutine(CheckTimeOutInterstitial());
			interstitialRequested = true;
			interstitialLoaded = false;
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_CB_DL_AD_Requested);
		}
	}

	IEnumerator CheckTimeOutVideoAd() {
		yield return new WaitForSeconds(5f);
		if (!videoLoaded)
		{
			MR.Log("Chartboost VideoAd Failed Timed Out");
			videoRequested = false;
			videoLoaded = false;
			GetComponent<MRUtilities>().VideoAdLoadFailed(this);
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_REWARDED_VIDEO, Constants.GA_LABEL_CB_DL_AD_Failed);
		}
	}

	public override void RequestVideoAd()
	{
		if (!videoRequested)
		{
			MR.Log("Chartboost Video Requested");
			videoRequested = true;
			videoLoaded = false;
			Chartboost.cacheRewardedVideo(CBLocation.Default);
			StartCoroutine(CheckTimeOutVideoAd());
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_CB_RV_AD_Requested);
		}
	}

	public override void ShowInterstitialAd()
	{
		Chartboost.showInterstitial(CBLocation.Default);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_CB_DL_AD_Shown);
	}

	public override void ShowVideoAd()
	{
		Chartboost.showRewardedVideo(CBLocation.Default);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_CHARTBOOST, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_CB_RV_AD_Shown);
	}
}
