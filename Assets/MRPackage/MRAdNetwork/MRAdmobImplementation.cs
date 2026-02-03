using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class MRAdmobImplementation : BasicAdNetwork
{
	public string bannerIDAndroid;
	public AdPosition bannerPositionAndroid;
	public string interstitialIDAndroid;
	public string rewardedVideoIDAndroid;

	public string bannerIDIOS;
	public AdPosition bannerPositionIOS;
	public string interstitialIDIOS;
	public string rewardedVideoIDIOS;
	
	string BANNERID;
	AdPosition BANNERPOSITION;
	string INTERSTITIALID;
	string REWARDEDVIDEOID;
	bool interstitialRequested = false;
	bool interstitialLoaded = false;
	bool interstitialForced = false;
	bool videoRequested = false;
	bool videoLoaded = false;

	BannerView smartBanner = null;
	InterstitialAd interstitial = null;
	InterstitialAd forcedInterstitial = null;
	RewardBasedVideoAd rewardBasedVideo  = null;

	void Start()
	{
		#if UNITY_ANDROID
		BANNERID = bannerIDAndroid;
		BANNERPOSITION = bannerPositionAndroid;
		INTERSTITIALID = interstitialIDAndroid;
		REWARDEDVIDEOID = rewardedVideoIDAndroid;
		#endif
		#if UNITY_IOS
		BANNERID = bannerIDIOS;
		BANNERPOSITION = bannerPositionIOS;
		INTERSTITIALID = interstitialIDIOS;
		REWARDEDVIDEOID = rewardedVideoIDIOS;
		#endif

		if (Game.Instance.showAds)
			StartCoroutine(RequestBanner());
	}

	IEnumerator RequestBanner()
	{
		yield return new WaitForSeconds(2f);

		smartBanner = new BannerView(BANNERID, AdSize.SmartBanner, BANNERPOSITION);
		AdRequest request = new AdRequest.Builder().Build();
		smartBanner.LoadAd(request);

		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB, Constants.GA_ACTION_BANNER, Constants.GA_LABEL_ADMOB_BANNER_Requested);
	}

	public override void RequestInterstitial()
	{
		if (!interstitialRequested)
		{
			MR.Log("Admob Interstitial Requested");
			interstitial = new InterstitialAd(INTERSTITIALID);
			interstitial.OnAdLoaded += OnInterstitialLoaded;
			interstitial.OnAdOpening += OnInterstitialOpened;
			interstitial.OnAdFailedToLoad += OnInterstitialFailedLoading;
			AdRequest request = new AdRequest.Builder().Build();
			interstitial.LoadAd(request);

			interstitialRequested = true;
			interstitialLoaded = false;
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_ADMOB_INT_Requested);
		}
	}

	public override void RequestVideoAd()
	{
		if (!videoRequested)
		{
			MR.Log("Admob Video Requested");
			rewardBasedVideo = RewardBasedVideoAd.Instance;
			rewardBasedVideo.OnAdLoaded += OnRewardedVideoLoaded;
			rewardBasedVideo.OnAdFailedToLoad += OnRewardedVideoAdFailedToLoad;
			rewardBasedVideo.OnAdRewarded += OnRewardedVideoAdClosed;
			AdRequest request = new AdRequest.Builder().Build();
			rewardBasedVideo.LoadAd(request, REWARDEDVIDEOID);

			videoRequested = true;
			videoLoaded = false;
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_ADMOB_VIDEO_Requested);
		}
	}

	public override void ShowInterstitialAd()
	{
		MR.Log("Admob Interstitial Show");
		interstitial.Show();
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_ADMOB_INT_Shown);
	}

	public override void ShowVideoAd()
	{
		MR.Log("Admob Video Show");
		rewardBasedVideo.Show();
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB, Constants.GA_ACTION_VIDEO, Constants.GA_LABEL_ADMOB_VIDEO_Shown);
	}

	public override void ShowForcedInterstitial()
	{
		if (!interstitialRequested && !interstitialLoaded)
		{
			MR.Log("Admob Forced");
			forcedInterstitial = new InterstitialAd(INTERSTITIALID);
			forcedInterstitial.OnAdLoaded += OnForcedInterstitialLoaded;
			forcedInterstitial.OnAdOpening += OnInterstitialOpened;
			AdRequest request = new AdRequest.Builder().Build();
			forcedInterstitial.LoadAd(request);

			interstitialForced = true;
			MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB, Constants.GA_ACTION_INTERSTITIAL, Constants.GA_LABEL_ADMOB_FORCEDINT_REQUESTED);
		}
	}

	public override bool IsInterstitialRequested()
	{
		return interstitialRequested;
	}

	public override bool IsInterstitialLoaded()
	{
		return interstitialLoaded;
	}

	public override bool IsVideoAdRequested()
	{
		return videoRequested;
	}

	public override bool IsVideoAdLoaded()
	{
		return videoLoaded;
	}

	void OnForcedInterstitialOpened (object sender, EventArgs args)
	{
		interstitialRequested = false;
		interstitialLoaded = false;
		if (interstitialForced)
			interstitialForced = false;
		GetComponent<MRUtilities>().InterstitialOpened();
	}

	void OnInterstitialOpened (object sender, EventArgs args)
	{
		interstitialRequested = false;
		interstitialLoaded = false;
		if (interstitialForced)
			interstitialForced = false;
		GetComponent<MRUtilities>().InterstitialOpened();
	}

	void OnInterstitialFailedLoading (object sender, EventArgs args)
	{
		MR.Log("Admob Failed");
		interstitialRequested = false;
		interstitialLoaded = false;
		GetComponent<MRUtilities> ().InterstitialLoadFailed (this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB,Constants.GA_ACTION_INTERSTITIAL,Constants.GA_LABEL_ADMOB_INT_Failed);
	} 

	void OnInterstitialLoaded (object sender, EventArgs args)
	{
		MR.Log("Admob Loaded");

		interstitialRequested = false;
		interstitialLoaded = true;

		if (!interstitialForced) {
			GetComponent<MRUtilities> ().InterstitialLoadSuccess (this);
		}

		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB,Constants.GA_ACTION_INTERSTITIAL,Constants.GA_LABEL_ADMOB_INT_Cached);
	}

	void OnForcedInterstitialLoaded (object sender, EventArgs args)
	{
		forcedInterstitial.Show();
	}

	void OnRewardedVideoLoaded(object sender, EventArgs args) {
		MR.Log("Admob Video Loaded");

		videoRequested = false;
		videoLoaded = true;
		GetComponent<MRUtilities> ().VideoAdLoadSuccess (this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB,Constants.GA_ACTION_VIDEO,Constants.GA_LABEL_ADMOB_VIDEO_Cached);
	}

	void OnRewardedVideoAdFailedToLoad(object sender, EventArgs args) {
		MR.Log("Admob Video Failed");

		videoRequested = false;
		videoLoaded = false;
		GetComponent<MRUtilities> ().VideoAdLoadFailed (this);
		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB,Constants.GA_ACTION_VIDEO,Constants.GA_LABEL_ADMOB_VIDEO_Failed);
	}

	void OnRewardedVideoAdClosed(object sender, EventArgs args) {
		videoRequested = false;
		videoLoaded = false;
		GetComponent<MRUtilities>().VideoAdCompleted(this);

		MRUtilities.Instance.LogEvent(Constants.GA_CATEGORY_ADMOB,Constants.GA_ACTION_VIDEO,Constants.GA_LABEL_ADMOB_VIDEO_Completed);

	}

	public void ShowBanner() {
		if (smartBanner != null)
			smartBanner.Show ();
	}

	public void HideBanner() {
		if (smartBanner != null)
			smartBanner.Hide ();
	}

	public void DestroyBanner() {
		if (smartBanner != null)
		{
			smartBanner.Hide();
			smartBanner.Destroy();
			smartBanner = null;
		}
		
	}
}
