using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Rigidbody2D rb;
	CircleCollider2D coll2d;
	float angle = 0;
	int xAxisSpeed = 3;
	int yAxisSpeed = 30;

	public bool isDead = false;
	public GameObject Brain;
	public Animator ScreenAnim;
	CinemachineCameraShake cinemachineCameraShake;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		coll2d = GetComponent<CircleCollider2D>();
		cinemachineCameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineCameraShake>();
		
	}


	private void FixedUpdate()
	{
		if (isDead)
		{
			rb.velocity = new Vector2(0, 0);
			return;
		}
		Vector2 pos = transform.position;
		pos.x = Mathf.Cos(angle) * 3;
		transform.position = pos;
		angle += Time.deltaTime * xAxisSpeed;
		if (GameManager.isGameStarted)
		{
			if (Input.GetMouseButton(0))
			{
				rb.AddForce(new Vector2(0, yAxisSpeed));
			}
			else
			{
				if (rb.velocity.y > 0)
				{
					rb.AddForce(new Vector2(0, -yAxisSpeed / 2f));
				}
				else
				{
					rb.velocity = new Vector2(rb.velocity.x, 0);
				}
			}
		}
	}

	public void EnableCollider()
	{
		coll2d.enabled = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Corona")
		{
			GameManager.Instance.IncrementInfectedCells();
			cinemachineCameraShake.ShakeCamera();
			ScreenAnim.SetTrigger("Corona");
			if (GameManager.Instance.gameInfectedCells == 1)
			{
				GameManager.Instance.PlayWarningTwo();
			}else if (GameManager.Instance.gameInfectedCells == 2)
			{
				GameManager.Instance.PlayWarningOne();
			}
			else if (GameManager.Instance.gameInfectedCells == 0)
			{
				GameManager.Instance.PlayWarningThree();
			}
			if (GameManager.Instance.gameInfectedCells < 0)
			{
				GameManager.Instance.Death();
				coll2d.enabled = false;
				return;
			}

		}else if (collision.gameObject.tag == "Vaccine")
		{
			GameManager.Instance.PlayCoin();
			GameManager.Instance.IncrementCollectedVaccines();
			ScreenAnim.SetTrigger("Vaccine");
			Destroy(collision.gameObject);
		}
	}
}
