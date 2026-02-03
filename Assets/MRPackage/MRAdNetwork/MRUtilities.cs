using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

public class MRUtilities : MonoBehaviour
{

	#if UNITY_ANDROID
	string platform = "Android";
	
	#else
	string platform = "IOS";
	#endif


	Texture2D crossPromoImage;
	bool local_isConsistent;

	List<BasicAdNetwork> interstitialImplementsPrioritized = null;
	List<BasicAdNetwork> interstitialQueue = new List<BasicAdNetwork>();

	List<BasicAdNetwork> videoAdImplementsPrioritized = null;
	List<BasicAdNetwork> videoAdQueue = new List<BasicAdNetwork>();


	List<GameItem> promoGamesList = new List<GameItem>();
	GameObject mrPromoGamesCanvas;
	GameObject promoGamesGridRoot;
	GameObject promoGameItemPrefab;
	GameObject bannerCanvas;
	GameObject topGamesCanvas;
	GameObject topGamesGridRoot;
	GameObject topGameItemPrefab;

	int INTERSTITIALRATE = 1;
	int INTERSTITALCOUNTER = 0;
	int RATERATE = 10;
	int RATECOUNTER = 0;

	BasicAdNetwork FORCEDVIDEOADTEMPLATE = null;

	bool ADNETWORKSTARTED = false;

	[HideInInspector]
	public static MRUtilities Instance = null;

	Sprite socialShareTexture;
	[HideInInspector]
	public Image image_socialShareTexture = null;
	[HideInInspector]
	public GameObject socialSharePanel;

	GameObject facebookConnectCanvas;
	bool showingFacebookConnect = false;
	Text textLabelFacebookConnectFeedback;

	GameObject customLeaderboardCanvas;
	[HideInInspector]
	public GameObject leaderboardItemPrefab;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		if (GameObject.FindObjectsOfType<MRUtilities>().Length > 1)
		{
			Destroy(this.gameObject);
			return;
		}

		if (!Instance)
			Instance = this;

//		DataProvider.InitializeDataProvider();
		
		StartCoroutine(InitializeMRUtilitiesHeavyFunctions());
	}

	IEnumerator InitializeMRUtilitiesHeavyFunctions()
	{
		yield return new WaitForSeconds(1f);

		SelectAdNetwork();	
	}
		
	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
			SetSoundOff();
		else
		{
			if (Game.Instance.soundEnabled)
				SetSoundOn();
			else
				SetSoundOff();
		}
	}

	void SetSoundOn()
	{
		StartCoroutine(SetSoundOnWithDelay());
	}

	void SetSoundOff()
	{
		StartCoroutine(SetSoundOffWithDelay());
	}


	IEnumerator SetSoundOnWithDelay()
	{
		yield return new WaitForSeconds(0.01f);
		AudioListener.pause = false;
	}

	IEnumerator SetSoundOffWithDelay()
	{
		yield return new WaitForSeconds(0.01f);
		AudioListener.pause = true;
	}


	void ShowFacebookConnectScreen()
	{
		facebookConnectCanvas.SetActive(true);
		showingFacebookConnect = true;
	}

	void HideFacebookConnectScreen()
	{ 
		StartCoroutine(HideWithDelay());
	}

	IEnumerator HideWithDelay()
	{
		yield return new WaitForSeconds(2f);
		facebookConnectCanvas.SetActive(false);
		showingFacebookConnect = false;
	}

	void SelectAdNetwork()
	{
		if (interstitialImplementsPrioritized == null)
		{
			interstitialImplementsPrioritized = new List<BasicAdNetwork>();
					
			interstitialImplementsPrioritized.Add(GetComponent<MRAppLovinImplementation>());
			interstitialImplementsPrioritized.Add(GetComponent<MRChartboostImplementation>());
			interstitialImplementsPrioritized.Add(GetComponent<MRAdmobImplementation>());

		}
			
		if (videoAdImplementsPrioritized == null)
		{
			videoAdImplementsPrioritized = new List<BasicAdNetwork>();
			videoAdImplementsPrioritized.Add(GetComponent<MRVungleImplementation>());
			videoAdImplementsPrioritized.Add(GetComponent<MRUnityAdsImplementation>());
			videoAdImplementsPrioritized.Add(GetComponent<MRChartboostImplementation>());
					
			StartAdNetwork();
		}
	}

	void StartAdNetwork()
	{
		if (ADNETWORKSTARTED)
			return;

		if (Game.Instance.showAds)
		{
			if (interstitialImplementsPrioritized == null)
			{
				//Load Default
				interstitialImplementsPrioritized = new List<BasicAdNetwork>();
				interstitialImplementsPrioritized.Add(GetComponent<MRChartboostImplementation>());
				interstitialImplementsPrioritized.Add(GetComponent<MRAppLovinImplementation>());
				interstitialImplementsPrioritized.Add(GetComponent<MRAdmobImplementation>());
				MRUtilities.Instance.LogEvent("AdNetwork", "DefaultIIntTempLoaded", "");

				MR.Log("Default Interstitial Template Loaded");
			}

			//request intestitial for the top priority network
			RequestTopPriorityInterstitial();
		}

		if (videoAdImplementsPrioritized == null)
		{
			//Load Default
			videoAdImplementsPrioritized = new List<BasicAdNetwork>();
			videoAdImplementsPrioritized.Add(GetComponent<MRVungleImplementation>());
			videoAdImplementsPrioritized.Add(GetComponent<MRUnityAdsImplementation>());
			videoAdImplementsPrioritized.Add(GetComponent<MRChartboostImplementation>());
			MRUtilities.Instance.LogEvent("AdNetwork", "DefaultIVideoAdTempLoaded", "");

			MR.Log("Default Interstitial Template Loaded");
		}

		//request video ad for the top pirority network
		RequestTopPriorityVideoAd();

		ADNETWORKSTARTED = true;

	}

	public void ShowBannerAd()
	{
		GetComponent<MRAdmobImplementation>().ShowBanner();
	}

	public void HideBannerAd()
	{
		GetComponent<MRAdmobImplementation>().HideBanner();
	}

	public void DestroyBannerAd()
	{
		GetComponent<MRAdmobImplementation>().DestroyBanner();
	}

	public void InterstitialLoadFailed(BasicAdNetwork p_adNetworkImplement)
	{
		int failedNetworkIndex = interstitialImplementsPrioritized.FindIndex(asd => asd.GetType() == p_adNetworkImplement.GetType());
		for (int i = failedNetworkIndex + 1; i < interstitialImplementsPrioritized.Count; i++)
		{
			if (!interstitialImplementsPrioritized[i].IsInterstitialRequested() && !interstitialImplementsPrioritized[i].IsInterstitialLoaded())
			{
				interstitialImplementsPrioritized[i].RequestInterstitial();
				break;
			}
		}
	}

	public void InterstitialLoadSuccess(BasicAdNetwork p_adNetworkImplement)
	{
		interstitialQueue.Add(p_adNetworkImplement);
		RequestTopPriorityInterstitial();
	}

	public void InterstitialOpened()
	{
		RequestTopPriorityInterstitial();
	}

	public void ShowInterstitialAd(bool p_rateContolled = false)
	{
		if (Game.Instance.showAds)
		{
			if (p_rateContolled)
			{
				INTERSTITALCOUNTER++;
				if (INTERSTITALCOUNTER % INTERSTITIALRATE != 0)
				{
					MR.Log("Rate Controlled But Dont Show");
					return;
				}
				else
					MR.Log("Rate Controlled And Show");
			}

			if (interstitialQueue.Count == 0)
			{
				MR.Log("Interstitial Queue Empty, Forcing Admob Now");
				if (interstitialImplementsPrioritized != null)
					interstitialImplementsPrioritized.Find(asd => asd.GetType() == typeof(MRAdmobImplementation)).ShowForcedInterstitial();
			}
			else
			{
				interstitialQueue[0].ShowInterstitialAd();
				interstitialQueue.RemoveAt(0);
				RequestTopPriorityInterstitial();
			}
		}
	}

	void RequestTopPriorityInterstitial()
	{
		if (!interstitialImplementsPrioritized[0].IsInterstitialRequested() && !interstitialImplementsPrioritized[0].IsInterstitialLoaded())
		{
			interstitialImplementsPrioritized[0].RequestInterstitial();
		}
	}

	void RequestTopPriorityVideoAd()
	{
		if (!videoAdImplementsPrioritized[0].IsVideoAdRequested() && !videoAdImplementsPrioritized[0].IsVideoAdLoaded())
		{
			videoAdImplementsPrioritized[0].RequestVideoAd();
		}
	}

	public void VideoAdLoadFailed(BasicAdNetwork p_adNetworkImplement)
	{
		int failedNetworkIndex = videoAdImplementsPrioritized.FindIndex(asd => asd.GetType() == p_adNetworkImplement.GetType());
		for (int i = failedNetworkIndex + 1; i < videoAdImplementsPrioritized.Count; i++)
		{
			if (!videoAdImplementsPrioritized[i].IsVideoAdRequested() && !videoAdImplementsPrioritized[i].IsVideoAdLoaded())
			{
				videoAdImplementsPrioritized[i].RequestVideoAd();
				break;
			}
		}
	}

	public void VideoAdLoadSuccess(BasicAdNetwork p_adNetworkImplement)
	{
		videoAdQueue.Add(p_adNetworkImplement);
		RequestTopPriorityVideoAd();
		//AndroidMessage.Create ("Admob", "VideoAdQueue Count = " + videoAdQueue.Count);
	}


	public void VideoAdCompleted(BasicAdNetwork p_adNetworkImplement)
	{
		RequestTopPriorityVideoAd();
		if (rewardedVideoCompleteAction != null)
			rewardedVideoCompleteAction.Invoke();
	}

	public delegate void RewardedVideoAction();

	RewardedVideoAction rewardedVideoCompleteAction;
	RewardedVideoAction rewardedVideoNotAvailableAction;

	public void ShowVideoAd(RewardedVideoAction p_rewardedVideoCompleteAction, RewardedVideoAction p_rewardedVideoNotAvailableAction)
	{
		rewardedVideoCompleteAction = p_rewardedVideoCompleteAction;
		rewardedVideoNotAvailableAction = p_rewardedVideoNotAvailableAction;

		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			if (rewardedVideoCompleteAction != null)
				rewardedVideoCompleteAction.Invoke();
			return;
		}
		else
		{
			if (videoAdQueue.Count == 0)
			{
				if (FORCEDVIDEOADTEMPLATE.GetType() == typeof(MRVungleImplementation))
				{
					MR.Log("VideoAd Queue Empty, Forcing Vungle Now");
					if (videoAdImplementsPrioritized != null)
						videoAdImplementsPrioritized.Find(asd => asd.GetType() == typeof(MRVungleImplementation)).ShowForcedVideoAd();
				}
				else if (FORCEDVIDEOADTEMPLATE.GetType() == typeof(MRUnityAdsImplementation))
				{
					MR.Log("VideoAd Queue Empty, Forcing Unity Now");
					if (videoAdImplementsPrioritized != null)
						videoAdImplementsPrioritized.Find(asd => asd.GetType() == typeof(MRUnityAdsImplementation)).ShowForcedVideoAd();
				}
			}
			else
			{
				videoAdQueue[0].ShowVideoAd();
				videoAdQueue.RemoveAt(0);
				RequestTopPriorityVideoAd();
			}
		}
	}

	public void LogEvent(string p_name, string p_parameterName, string p_value)
	{
		var parameters = new Dictionary<string, object>();
		parameters["parameter"] = p_value;
	}

	public void LogScreen(string p_screenName)
	{
		var parameters = new Dictionary<string, object>();
		parameters["parameter"] = p_screenName;
	}
		
}

#region Helper Classes

public class BasicAdNetwork : MonoBehaviour
{
	public virtual void RequestInterstitial()
	{
		//Override
	}

	public virtual void RequestVideoAd()
	{
		//Override
	}

	public virtual void ShowInterstitialAd()
	{
		//override
	}

	public virtual void ShowVideoAd()
	{
		//override
	}

	public virtual void ShowForcedInterstitial()
	{
		//override
	}

	public virtual void ShowForcedVideoAd() 
	{
		//override
	}

	public virtual bool IsInterstitialRequested()
	{
		return false;
	}

	public virtual bool IsInterstitialLoaded()
	{
		return false;
	}

	public virtual bool IsVideoAdRequested()
	{
		return false;
	}

	public virtual bool IsVideoAdLoaded()
	{
		return false;
	}
}


public enum CrossPromoAdType
{
	BannerTopLeft,
	BannerTop,
	BannerTopRight,
	BannerLeft,
	BannerCenter,
	BannerRight,
	BannerBottomLeft,
	BannerBottom,
	BannerBottomRight,
}


public class CrossPromoGame
{
	public string Name;
	public string Bundle;
	public string ImageURL;
	public Texture2D Image;
	public int index;
	public string appID;
}

public class Param
{
	public CrossPromoGame game;
	public int index;
	public CrossPromoAdType crossPromoAdType;
	public bool isConsistent;

	public Param(CrossPromoGame game, int index, CrossPromoAdType p_cpat, bool p_isConsistent)
	{
		this.game = game;
		this.index = index;
		this.crossPromoAdType = p_cpat;
		this.isConsistent = p_isConsistent;
	}
}

public class GameItem : MonoBehaviour
{
	public int id;
	public string gameName;
	public string gameBundle;
	public string platform;
	public bool showCrossPromo;
	public int crossPromoGameID;
	public string crossPromoGameName;
	public string crossPromoGameBundle;
	public string crossPromoGameIOSID;
	public string crossPromoImageName;
	public string crossPromoGameOwnership;
	public string crossPromoGamePlatform;
	public string orientation;
	public string crossPromoAdType;
	public int isConsistent;
	public int updatedToLatestCrossPromo;
	public Sprite icon;
	public Sprite promoBanner;
	public string promoBannerImageName;
	public int gameRank;
	public int isTopGame;


	public GameItem(
		int i,
		string gn,
		string gb,
		string p,
		string ntu,
		string scp,
		int cpgid,
		string cpgn,
		string cpgb,
		string cpgiosid,
		string cpgin,
		string cpgown,
		string cpgp,
		string ori,
		string cpat,
		int p_isConsistent,
		int p_gameRank,
		int p_isTopGame)
	{
		id = i;
		gameName = gn;
		gameBundle = gb;
		platform = p;

		if (scp == "1")
			showCrossPromo = true;
		else
			showCrossPromo = false;

		crossPromoGameID = cpgid;
		crossPromoGameName = cpgn;
		crossPromoGameBundle = cpgb;
		crossPromoGameIOSID = cpgiosid;
		crossPromoImageName = cpgin;
		crossPromoGameOwnership = cpgown;
		crossPromoGamePlatform = cpgp;
		orientation = ori;
		crossPromoAdType = cpat;
		isConsistent = p_isConsistent;
		gameRank = p_gameRank;
		isTopGame = p_isTopGame;
	}
}

public enum SocialPlatformEnum
{
	FACEBOOK,
	TWITTER,
	INSTAGRAM,
	WATSAPP,
	EMAIL
}

public class SocialSharing : MonoBehaviour
{
	private static SocialSharing instance;

	private SocialSharing()
	{
	}

	public static SocialSharing Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new SocialSharing();
			}
			return instance;
		}
	}

	public delegate void SocialSharingDelegate();

	private SocialSharingDelegate socialSharingSuccessCallback;

	#if UNITY_ANDROID
	void HandleOnShareIntentCallback (bool status, string package)
	{
		AndroidSocialGate.OnShareIntentCallback -= HandleOnShareIntentCallback;
		MR.Log("CallBack , [HandleOnShareIntentCallback] " + status.ToString() + " " + package);
		//if (status)
		socialSharingSuccessCallback.Invoke ();
	}
	#endif

	#if UNITY_IOS
	void HandleOnSocialMediaPostResult(SA.Common.Models.Result res)
	{
		IOSSocialManager.OnFacebookPostResult -= HandleOnSocialMediaPostResult;
		IOSSocialManager.OnTwitterPostResult -= HandleOnSocialMediaPostResult;
		IOSSocialManager.OnInstagramPostResult -= HandleOnSocialMediaPostResult;

		if (res.IsSucceeded)
		{
			//IOSNativePopUpManager.showMessage("Posting example", "Post Success!");
			MR.Log("Posting example , Post Success!");
			socialSharingSuccessCallback.Invoke();
		}
		else
		{
			//IOSNativePopUpManager.showMessage("Posting example", "Post Failed :(");
			MR.Log("Posting example , Post Failed!");
		}
	}
	#endif

	public void ShareOnSocialPlatform(SocialSharingDelegate p_socialSharingSuccessCallBack, string p_title, string p_message, SocialPlatformEnum p_socialPlatform)
	{
		#if UNITY_ANDROID
		AndroidSocialGate.OnShareIntentCallback += HandleOnShareIntentCallback;
		#elif UNITY_IOS
		IOSSocialManager.OnFacebookPostResult += HandleOnSocialMediaPostResult;
		IOSSocialManager.OnTwitterPostResult += HandleOnSocialMediaPostResult;
		IOSSocialManager.OnInstagramPostResult += HandleOnSocialMediaPostResult;
		#endif

		socialSharingSuccessCallback = p_socialSharingSuccessCallBack;
		MRUtilities.Instance.StartCoroutine(Post(p_title, p_message, p_socialPlatform));
	}

	private IEnumerator Post(string p_title, string p_message, SocialPlatformEnum p_socialPlatform, Texture2D p_texture = null)
	{

		if (MRUtilities.Instance.socialSharePanel)
			MRUtilities.Instance.socialSharePanel.SetActive(true);
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		if (MRUtilities.Instance.socialSharePanel)
			MRUtilities.Instance.socialSharePanel.SetActive(false);

		#if UNITY_ANDROID
		string packageNamePattern = "";
		#endif
		if (p_socialPlatform == SocialPlatformEnum.FACEBOOK)
		{
			#if UNITY_ANDROID
			packageNamePattern = "facebook.katana";
			AndroidSocialGate.StartShareIntent(p_title, p_message, tex, packageNamePattern);
			MR.Log("started intent");
			#endif
			#if UNITY_IOS
			IOSSocialManager.Instance.FacebookPost(p_message, null, tex);
			#endif
		}
		else if (p_socialPlatform == SocialPlatformEnum.TWITTER)
		{
			#if UNITY_ANDROID
			packageNamePattern = "twi";
			AndroidSocialGate.StartShareIntent(p_title, p_message, tex, packageNamePattern);
			#endif
			#if UNITY_IOS
			IOSSocialManager.Instance.TwitterPost(p_message, null, tex);
			#endif
		}
		else if (p_socialPlatform == SocialPlatformEnum.INSTAGRAM)
		{
			#if UNITY_ANDROID
			packageNamePattern = "insta";
			AndroidSocialGate.StartShareIntent(p_title, p_message, tex, packageNamePattern);
			#endif
			#if UNITY_IOS
			IOSSocialManager.Instance.InstagramPost(tex, p_message);
			#endif
		}

		Destroy(tex);
	}
}
#endregion
