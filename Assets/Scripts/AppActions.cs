using UnityEngine;

public class AppActions : MonoBehaviour
{
    public RateAndShareApp rateAndShareApp;

    public void OnRateAndShareClicked()
    {
        rateAndShareApp.StartReviewFlow();
        rateAndShareApp.ShareAppLink();
    }

    public void OnRateApp()
    {
        rateAndShareApp.StartReviewFlow();
    }

    public void OnShareApp()
    {
        rateAndShareApp.ShareAppLink();
    }
}
