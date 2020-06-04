using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
	public GameObject playerObj;
	public GameObject[] obstacles;
	GameObject currentObstacle;

	int obstacleCount;
	int obstacleIndex = 0;

	private void Start()
	{
		obstacleCount = obstacles.Length;
		SpawnObstacles();
	}

	private void Update()
	{
		
		if(playerObj.transform.position.y - currentObstacle.transform.position.y > 0)
		{
			SpawnObstacles();
		}
		
	}

	//void SpawnObstaclesOld()
	//{
	//	int RandomIndex = Random.Range(0, obstacleCount);
	//	GameObject newObstacle = Instantiate(obstacles[RandomIndex], new Vector3(0, obstacleIndex * distanceToNext), Quaternion.identity);
	//	newObstacle.transform.SetParent(transform);
	//	obstacleIndex++;
	//}
	void SpawnObstaclesStart()
	{
		int RandomIndex = Random.Range(0, obstacleCount);
		GameObject newObstacle = Instantiate(obstacles[RandomIndex]) as GameObject;
		newObstacle.transform.SetParent(transform);
		newObstacle.transform.position = new Vector3(0, playerObj.transform.position.y + 10);
		currentObstacle = newObstacle;
	}
	void SpawnObstacles()
	{
		int RandomIndex = Random.Range(0, obstacleCount);
		GameObject newObstacle = Instantiate(obstacles[RandomIndex]) as GameObject;
		newObstacle.transform.SetParent(transform);
		int dFactor = 0;
		if(obstacleIndex > 0)
		{
			dFactor = 50;
		}
		else
		{
			dFactor = 10;
		}
		newObstacle.transform.position = new Vector3(0, playerObj.transform.position.y + dFactor);
		obstacleIndex++;
		currentObstacle = newObstacle;
	}
}
