using UnityEngine;

[System.Serializable]
public class ProfileData
{
    [Header("Scores")]
    public int totalScore; // Total score persists across monthly resets
    public int monthlyScore; // Monthly score resets automatically at the start of each month

    [Header("Badges")]
    public int totalBadgesEarned;

    [Header("Saved Styles")]
    public int savedStylesThisMonth;
}

public enum ProfileDataType
{
    TotalScore,
    MonthlyScore,
    BadgesEarned,
    SavedStylesThisMonth,
}
