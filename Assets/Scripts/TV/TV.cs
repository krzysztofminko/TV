using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TV : MonoBehaviour
{
	public static List<TV> list = new List<TV>();
	
	public Shop shop;

	[Header("Parameters")]
	[HideInInspector]
	public float moveSpeed;
	public float rotateSpeed = 800;

	[Header("References")]
	public Renderer tvScreen;
	public Renderer tvCone;
	public Light tvLight;
	public Material color;
	
	void Awake()
	{
		list.Add(this);
		moveSpeed = Game.instance.size / 6;
	}

	public virtual void OnDestroy()
	{
		list.Remove(this);
	}

	public virtual void Update()
	{
		//Move
		int clamp = Game.instance.size / 2;
		transform.position = new Vector3(Mathf.Clamp(transform.position.x + transform.forward.x * moveSpeed * Time.deltaTime, -clamp, clamp - 1), transform.position.y, Mathf.Clamp(transform.position.z + transform.forward.z * moveSpeed * Time.deltaTime, -clamp, clamp - 1));
		
		//Paint
		List<Person> persons = Person.list.FindAll(p => Mathf.Abs(transform.position.x + transform.forward.x * 2- p.transform.position.x) + Mathf.Abs(transform.position.z + transform.forward.z * 2 - p.transform.position.z) < 2f).ToList();
		for (int i = 0; i < persons.Count; i++)
			persons[i].SetTV(this);

		//Execute bonus
		Bonus bonus = Bonus.list.Find(b => !b.executed && Mathf.Abs(transform.position.x - b.transform.position.x) + Mathf.Abs(transform.position.z - b.transform.position.z) < 2f);
		if (bonus)
			bonus.Execute(this);
	}

	public void SetColor(Color color)
	{
		Material m = tvCone.material;
		m.color = color;
		m.SetColor("_EmissionColor", color);
		tvCone.material = m;
		m = tvScreen.material;
		m.color = color;
		tvScreen.material = m;
		tvLight.color = color;
	}
}
