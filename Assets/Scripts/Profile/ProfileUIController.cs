using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProfileUIController : MonoBehaviour
{
    [SerializeField] private ProfileUiElement[] profileElements;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    #region Save and Load Methods

    public async void Save()
    {
        ProfileData data = new ProfileData
        {
            totalScore = GetProfileDataValue(profileElements, ProfileDataType.TotalScore),
            monthlyScore = GetProfileDataValue(profileElements, ProfileDataType.MonthlyScore),
            totalBadgesEarned = GetProfileDataValue(profileElements, ProfileDataType.BadgesEarned),
            savedStylesThisMonth = GetProfileDataValue(profileElements, ProfileDataType.SavedStylesThisMonth)
        };

        await SavePlayerData(data);
    }

    private async Task SavePlayerData(ProfileData data)
    {
        string jsonData = JsonUtility.ToJson(data);

        var saveData = new Dictionary<string, object>
        {
            { "profileData", jsonData }
        };

        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
            Debug.Log("Data saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save: " + e.Message);
        }
    }

    public async void Load()
    {
        ProfileData loadedData = await LoadPlayerData();

        if (loadedData != null)
        {
            foreach (var element in profileElements)
            {
                switch (element.dataType)
                {
                    case ProfileDataType.TotalScore:
                        element.SetValue(loadedData.totalScore);
                        break;
                    case ProfileDataType.MonthlyScore:
                        element.SetValue(loadedData.monthlyScore);
                        break;
                    case ProfileDataType.BadgesEarned:
                        element.SetValue(loadedData.totalBadgesEarned);
                        break;
                    case ProfileDataType.SavedStylesThisMonth:
                        element.SetValue(loadedData.savedStylesThisMonth);
                        break;
                }
            }
        }
    }

    private async Task<ProfileData> LoadPlayerData()
    {
        try
        {
            var savedData = await CloudSaveService.Instance.Data.Player.LoadAsync(
                new HashSet<string> { "profileData" }
            );

            if (savedData.TryGetValue("profileData", out var jsonString))
            {
                ProfileData data = JsonUtility.FromJson<ProfileData>(jsonString.Value.GetAsString());
                Debug.Log("Data loaded successfully!");
                return data;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load: " + e.Message);
        }

        return null;
    }
    #endregion

    #region Helper Methods
    private int GetProfileDataValue(ProfileUiElement[] profileUiElements, ProfileDataType dataType)
    {
        int value = 0;
        foreach (var uiElement in profileUiElements)
        {
            if (uiElement.dataType == dataType)
            {
                value = uiElement.GetValue();
                break;
            }
        }
        return value;
    }
    #endregion
}
