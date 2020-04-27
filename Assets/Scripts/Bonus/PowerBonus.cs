using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBonus : Bonus
{
	public GameObject[] thunderParticles;
	
	public override void Execute(TV executor)
	{
		base.Execute(executor);

		int spawnMax = Game.instance.size / 8;

		for (int i = 0; i < spawnMax; i++)
		{
			Invoke("SpawnThunder", (1.0f * i * totalDuration) / spawnMax);
		}

	}

	public void SpawnThunder()
	{
		int range = Game.instance.size / 4;

		int clamp = Game.instance.size / 2 - range / 2;
		Vector3 position = new Vector3(Random.Range(-clamp, clamp - 1), 0, Random.Range(-clamp, clamp - 1));

		GameObject particle = Instantiate(thunderParticles[Random.Range(0, thunderParticles.Length)]);
		particle.transform.position = position;
		Destroy(particle, 2);

		List<Person> persons = Person.list.FindAll(p => (position - p.transform.position).sqrMagnitude < range * range);
		for (int p = 0; p < persons.Count; p++)
		{
			persons[p].SetTV(executor);
			persons[p].immunityTime = totalDuration - duration;
		}
	}
}
