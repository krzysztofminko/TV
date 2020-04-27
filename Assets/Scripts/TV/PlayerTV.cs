using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTV : TV
{
	//[Header("Player")]

	public Transform cameraTransform;

	public static int humanPlayers;

	public int humanId;
	Vector3 targetPosition;
	Vector3 direction;

	public override void OnDestroy()
	{
		base.OnDestroy();
		humanPlayers--;
	}

	private void Start()
	{
		humanPlayers++;
		humanId = humanPlayers;
		SetColor(color.color);
		direction = -transform.forward;
		targetPosition = transform.position;// + direction;
		cameraTransform = Camera.main.transform.parent;
	}

	public override void Update()
	{
		if (Game.instance.started && !Game.instance.ended)
		{
			base.Update();

			//Movement
			transform.Rotate(Vector3.up, Input.GetAxis("Horizontal" + humanId) * rotateSpeed * Time.deltaTime);
		}

	}
}
