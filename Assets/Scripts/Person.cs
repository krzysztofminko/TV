using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
	public static List<Person> list = new List<Person>();
	public static Queue<Person> pool = new Queue<Person>();

	public float speed = 5;

	public float immunityTime;


	public Material color;
	public TV tv;
	public Transform brain;

	static Material defaultColor;

	float timeOffset;
	
	void Awake()
    {
		defaultColor = brain.GetComponent<Renderer>().sharedMaterial;
	}

	void Update()
	{
		transform.position = new Vector3(transform.position.x, Mathf.PingPong(timeOffset + Time.time * 0.5f, 0.3f), transform.position.z);

		//Immunity
		immunityTime = Mathf.Max(0, immunityTime - Time.deltaTime);

		if (Game.instance.ended && tv)
		{
			//Go to shop
			if (Vector3.ProjectOnPlane(transform.position, Vector3.up) != tv.shop.transform.position)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(tv.shop.transform.position - transform.position, Vector3.up)), 360 * Time.deltaTime);
				transform.position = Vector3.MoveTowards(transform.position, tv.shop.transform.position, speed * Time.deltaTime);
			}
			else
			{
				tv.shop.clientsCount++;
				Despawn(this);
			}

		}
		else if (tv && (tv.transform.position - transform.position).sqrMagnitude < 16)
		{
			//Watch TV
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(tv.tvScreen.transform.position - transform.position, Vector3.up)), 360 * Time.deltaTime);
		}
	}

	public void SetColor(Material color)
	{
		this.color = color;
		brain.GetComponent<Renderer>().sharedMaterial = color;
	}

	public void SetTV(TV tv)
	{
		if (immunityTime == 0)
		{
			this.tv = tv;
			SetColor(tv.color);
		}
	}

	public static void InitPool(int maxCount)
	{
		for (int i = 0; i < maxCount; i++)
		{
			Person p = Instantiate(Game.instance.personPrefab, Game.instance.personsParent);
			p.gameObject.SetActive(false);
			pool.Enqueue(p);
		}
	}

	public static Person Spawn(Vector3 position)
	{
		Person person = pool.Dequeue();
		list.Add(person);
		person.transform.position = position;
		person.gameObject.SetActive(true);
		person.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Vector3.back, Vector3.up));
		person.timeOffset = Random.Range(0.0f, 1.0f);
		return person;		
	}

	public static void Despawn(Person person)
	{
		person.gameObject.SetActive(false);
		list.Remove(person);
		pool.Enqueue(person);
		person.SetColor(defaultColor);		
	}
}
