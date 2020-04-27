using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AITV : TV
{
	[Tooltip("Delay of reaction for bonus")]
	public float delay = 1;

	float delayTimer;
	Vector3 targetPosition;

	private void Start()
	{
		moveSpeed *= Game.instance.aiSpeed;
		targetPosition = transform.position;
	}

	public override void Update()
    {
		if (Game.instance.started && !Game.instance.ended)
		{
			base.Update();

			//Target at bonus if any exists and delay passed
			if (Bonus.list.Count > 0)
			{
				delayTimer += Time.deltaTime;
				if (delayTimer > delay)
				{
					delayTimer = 0;
					targetPosition = Bonus.list[0].transform.position;
				}
			}
			else
			{
				delayTimer = 0;
			}

			//If target aquired
			if ((targetPosition - transform.position).sqrMagnitude < 2f)
			{
				//Target random brain to paint
				List<Person> toPaint = Person.list.FindAll(p => p.color != color).ToList();
				if (toPaint.Count > 0)
					targetPosition = toPaint[Random.Range(0, toPaint.Count)].transform.position;	
			}
			
			//Rotate towards target
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((targetPosition - transform.position).normalized, Vector3.up), rotateSpeed * Time.deltaTime);
		}

	}
}
