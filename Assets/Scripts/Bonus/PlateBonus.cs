using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBonus : Bonus
{
	public override void Execute(TV executor)
	{
		base.Execute(executor);

		for(int i = 0; i < Game.instance.size / 2; i++)
		{
			int clamp = Game.instance.size / 2;
			Vector3 position = new Vector3(Random.Range(-clamp, clamp - 1), Random.Range(-clamp, clamp - 1), Random.Range(-clamp, clamp - 1));

			List<Person> persons = Person.list.FindAll(p => (position - p.transform.position).sqrMagnitude < Game.instance.size / 6 * Game.instance.size / 6);
			for (int p = 0; p < persons.Count; p++)
				persons[p].SetTV(executor);
		}

	}
}
