using UnityEngine;

[System.Serializable]
public class ProfileData
{
    [Header("Scores")]
    public int TotalScore; // Total score persists across monthly resets
    public int MonthlyScore; // Monthly score resets automatically at the start of each month

    [Header("Badges")]
    public int TotalBadgesEarned;

    [Header("Saved Styles")]
    public int SavedStylesThisMonth;
}

#region Profile Data Converter
public static class ProfileDataConverter
{
    // Server → Client
    public static ProfileData ToClientProfile(
        Unity.Services.CloudCode.GeneratedBindings.CloudSaveManager.ProfileData serverData)
    {
        return new ProfileData
        {
            TotalScore = serverData.TotalScore,
            MonthlyScore = serverData.MonthlyScore,
            TotalBadgesEarned = serverData.TotalBadgesEarned,
            SavedStylesThisMonth = serverData.SavedStylesThisMonth
        };
    }

    // Client → Server (for when you need to send data up)
    public static Unity.Services.CloudCode.GeneratedBindings.CloudSaveManager.ProfileData ToServerProfile(
        ProfileData clientData)
    {
        return new Unity.Services.CloudCode.GeneratedBindings.CloudSaveManager.ProfileData
        {
            TotalScore = clientData.TotalScore,
            MonthlyScore = clientData.MonthlyScore,
            TotalBadgesEarned = clientData.TotalBadgesEarned,
            SavedStylesThisMonth = clientData.SavedStylesThisMonth
        };
    }
}
#endregion

public enum ProfileDataType
{
    TotalScore,
    MonthlyScore,
    BadgesEarned,
    SavedStylesThisMonth,
}
