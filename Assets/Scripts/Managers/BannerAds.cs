using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
	public string gameId = "3566678";
	public string placementId = "NewBannerAd";

	// Start is called before the first frame update
	void Start()
    {
		Advertisement.Initialize(gameId, false);
		StartCoroutine(ShowBannerWhenReady());
	}

	IEnumerator ShowBannerWhenReady()
	{
		while (!Advertisement.IsReady(placementId))
		{
			yield return new WaitForSeconds(0.5f);
			//Debug.Log("Trying Banner");
		}
		Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
		Advertisement.Banner.Show(placementId);
		
		
	}
}
