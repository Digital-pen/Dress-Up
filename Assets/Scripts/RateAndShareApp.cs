using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections; // <-- REQUIRED for Coroutines

#if UNITY_ANDROID
using Google.Play.Review;
#endif

public class RateAndShareApp : MonoBehaviour
{
    [SerializeField] string appLinkAndroid;
    [SerializeField] string appLink_iOS;

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _RequestReview();

    [DllImport("__Internal")]
    private static extern void _ShareToIOS(string content);
#endif

    // This is the public method you hook up to your Button
    public void StartReviewFlow()
    {
#if UNITY_ANDROID
        // Start the Coroutine for Android
        StartCoroutine(ExecuteAndroidReview());
#elif UNITY_IOS
        _RequestReview();
#endif
    }

#if UNITY_ANDROID
    private IEnumerator ExecuteAndroidReview()
    {
        var reviewManager = new ReviewManager();

        // 1. Request the review flow
        var requestFlowOperation = reviewManager.RequestReviewFlow();

        // Wait until the request is finished
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError("Review request failed: " + requestFlowOperation.Error.ToString());
            yield break;
        }

        // 2. Get the result and launch the flow
        var reviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(reviewInfo);

        // Wait until the UI flow is finished
        yield return launchFlowOperation;

        // Note: The API does not tell you if the user actually rated it or not.
        Debug.Log("Review flow complete.");
    }
#endif

    public void ShareAppLink()
    {
#if UNITY_ANDROID
        ShareToAndroid(appLinkAndroid);
#elif UNITY_IOS
        ShareToiOS(appLink_iOS);
#endif
    }

#if UNITY_ANDROID
    private void ShareToAndroid(string content)
    {
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.SEND");
        intent.Call<AndroidJavaObject>("setType", "text/plain");
        intent.Call<AndroidJavaObject>("putExtra", "android.intent.extra.SUBJECT", "Check out this app!");
        intent.Call<AndroidJavaObject>("putExtra", "android.intent.extra.TEXT", content);

        AndroidJavaClass unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intent);
    }
#endif

#if UNITY_IOS
    private void ShareToiOS(string content)
    {
        _ShareToIOS(content);
    }
#endif
}