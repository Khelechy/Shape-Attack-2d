using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RewardedAds : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "3566679";
#elif UNITY_ANDROID
	private string gameId = "3566678";
#endif

	Button myButton;
	public string myPlacementId = "rewardedVideo";
	public GameObject plusIcon;

	void Start()
	{
		myButton = GetComponent<Button>();

		// Set interactivity to be dependent on the Placement’s status:
		myButton.interactable = Advertisement.IsReady(myPlacementId);
		plusIcon.SetActive(Advertisement.IsReady(myPlacementId));

		// Map the ShowRewardedVideo function to the button’s click listener:
		if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

		// Initialize the Ads listener and service:
		Advertisement.AddListener(this);
		Advertisement.Initialize(gameId, false);
	}

	// Implement a function for showing a rewarded video ad:
	void ShowRewardedVideo()
	{
		Advertisement.Show(myPlacementId);
	}

	// Implement IUnityAdsListener interface methods:
	public void OnUnityAdsReady(string placementId)
	{
		// If the ready Placement is rewarded, activate the button: 
		if (placementId == myPlacementId)
		{
			myButton.interactable = true;
			plusIcon.SetActive(true);
		}
	}

	public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
	{
		// Define conditional logic for each ad completion status:
		if (showResult == ShowResult.Finished)
		{
			GameManager.Instance.GameContinueEvent();
		}
		else if (showResult == ShowResult.Skipped)
		{
			// Do not reward the user for skipping the ad.
		}
		else if (showResult == ShowResult.Failed)
		{
			// do nothing
		}
	}

	public void OnUnityAdsDidError(string message)
	{
		throw new System.NotImplementedException();
	}

	public void OnUnityAdsDidStart(string placementId)
	{
		throw new System.NotImplementedException();
	}
}
