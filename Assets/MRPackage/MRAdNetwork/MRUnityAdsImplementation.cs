using UnityEngine;
using UnityEngine.Advertisements;

public class MRUnityAdsImplementation : BasicAdNetwork,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    public string ANDROIDID;
    public string IOSID;
    public string rewardedVideoPlacementString = "rewardedVideo";

    bool videoRequested = false;
    bool videoLoaded = false;
    bool videoForced = false;

    void Start()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(ANDROIDID, false, this);
#elif UNITY_IOS
        Advertisement.Initialize(IOSID, false, this);
#endif
    }

    // ---------------- INITIALIZATION ----------------

    public void OnInitializationComplete()
    {
        MR.Log("Unity Ads Initialized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        MR.Log("Unity Ads Init Failed: " + message);
    }

    // ---------------- LOADING ----------------

    public override void RequestVideoAd()
    {
        MR.Log("UnityAds Video Requested");
        videoRequested = true;
        Advertisement.Load(rewardedVideoPlacementString, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId != rewardedVideoPlacementString) return;

        MR.Log("Unity Video Loaded");
        videoLoaded = true;
        videoRequested = false;

        if (!videoForced)
        {
            GetComponent<MRUtilities>().VideoAdLoadSuccess(this);
        }
        else
        {
            Advertisement.Show(rewardedVideoPlacementString, this);
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        MR.Log("Unity Video Load Failed: " + message);
        videoRequested = false;
        videoLoaded = false;
        videoForced = false;

        GetComponent<MRUtilities>().VideoAdLoadFailed(this);
    }

    // ---------------- SHOWING ----------------

    public override void ShowVideoAd()
    {
        if (!videoLoaded) return;

        MR.Log("UnityAds Video Show");
        Advertisement.Show(rewardedVideoPlacementString, this);
    }

    public override void ShowForcedVideoAd()
    {
        if (videoRequested || videoLoaded) return;

        MR.Log("UnityAds Forced Video Requested");
        videoForced = true;
        videoRequested = true;

        Advertisement.Load(rewardedVideoPlacementString, this);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showResult)
    {
        if (placementId != rewardedVideoPlacementString) return;

        if (showResult == UnityAdsShowCompletionState.COMPLETED)
        {
            MR.Log("Unity Ads Video Completed");
            GetComponent<MRUtilities>().VideoAdCompleted(this);
        }

        videoLoaded = false;
        videoForced = false;
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        MR.Log("Unity Ads Show Failed: " + message);
        videoLoaded = false;
        videoForced = false;
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }

    // ---------------- STATUS ----------------

    public override bool IsVideoAdRequested() => videoRequested;
    public override bool IsVideoAdLoaded() => videoLoaded;
}
