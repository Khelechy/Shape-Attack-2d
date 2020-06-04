using Assets.SimpleAndroidNotifications;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notify : MonoBehaviour
{
	private string title = "Shape Attack 2D: Come and play!";
	private string content = "Hey there, shapes are attacking, gather enough coins and buy that sheild!";

	private void OnApplicationPause(bool pause)
	{

		NotificationManager.CancelAll();
		if (pause)
		{
			DateTime timeToNotify = DateTime.Now.AddHours(1.0);
			TimeSpan time = timeToNotify - DateTime.Now;
			NotificationManager.SendWithAppIcon(time, title, content, Color.green, NotificationIcon.Bell);
		}

	}
}
