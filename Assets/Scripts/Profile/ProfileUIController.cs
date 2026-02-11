using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using Unity.Services.CloudCode.GeneratedBindings;
using System;
using Unity.Services.CloudCode;

public class ProfileUIController : MonoBehaviour
{
    [SerializeField] private ProfileUiElement[] profileElements;

    [Header("Buttons")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;


    [Header("Testing")]
    [SerializeField] private bool simulateEndOfMonth = false; // For testing purposes
    [SerializeField] private bool resetData = false; // For testing purposes

    // SERVER COMMUNICATION
    private PlayerDataServiceBindings bindings;



    private async void Awake()
    {
        saveButton.interactable = false;
        loadButton.interactable = false;
        deleteButton.interactable = false;

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        bindings = new PlayerDataServiceBindings(CloudCodeService.Instance);

        await Load();

        saveButton.interactable = true;
        loadButton.interactable = true;
        deleteButton.interactable = true;
    }

    #region Save and Load Methods
    public async void Save()
    {
        ProfileData data = new ProfileData
        {
            TotalScore = GetProfileDataValue(profileElements, ProfileDataType.TotalScore),
            MonthlyScore = GetProfileDataValue(profileElements, ProfileDataType.MonthlyScore),
            TotalBadgesEarned = GetProfileDataValue(profileElements, ProfileDataType.BadgesEarned),
            SavedStylesThisMonth = GetProfileDataValue(profileElements, ProfileDataType.SavedStylesThisMonth)
        };

        await bindings.SaveProfileData(ProfileDataConverter.ToServerProfile(data));
    }

    public async Task Load()
    {
        var resultFromCloud = await bindings.GetOrCreateProfileData();
        ProfileData loadedData = ProfileDataConverter.ToClientProfile(resultFromCloud);

        if (loadedData != null)
        {
            foreach (var element in profileElements)
            {
                switch (element.dataType)
                {
                    case ProfileDataType.TotalScore:
                        element.SetValue(loadedData.TotalScore);
                        break;
                    case ProfileDataType.MonthlyScore:
                        element.SetValue(loadedData.MonthlyScore);
                        break;
                    case ProfileDataType.BadgesEarned:
                        element.SetValue(loadedData.TotalBadgesEarned);
                        break;
                    case ProfileDataType.SavedStylesThisMonth:
                        element.SetValue(loadedData.SavedStylesThisMonth);
                        break;
                }
            }
        }
    }

    public async void DeleteProfile()
    {
        await bindings.DeletePlayerProfile();
        Debug.Log("Profile deleted.");
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
