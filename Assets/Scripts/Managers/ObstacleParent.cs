using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleParent : MonoBehaviour
{
	GameObject playerObj;
	float speed = 1.5f;
	public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
		playerObj = GameObject.FindGameObjectWithTag("Player");
		StartCoroutine(CalculateDistanceToPlayer());
    }

    IEnumerator CalculateDistanceToPlayer()
	{
		while (true)
		{
			if(playerObj.transform.position.y - transform.position.y > 50)
			{
				Destroy(this.gameObject);
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	private void FixedUpdate()
	{
		if (isDead) return;
		if (GameManager.isGameStarted)
		{
			transform.position += Vector3.down * speed * Time.deltaTime;
		}
	}
}
