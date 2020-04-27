using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
	public static List<Shop> list = new List<Shop>();

	public int clientsCount;

	[Header("References")]
	public Renderer neon;
	public Renderer building;
	public TextMeshPro clientsText;

	Animator animator;

	private void Awake()
	{
		list.Add(this);
		animator = neon.GetComponent<Animator>();
	}

	private void Update()
	{
		clientsText.text = clientsCount.ToString();
		animator.SetBool("Flash", clientsCount == Game.instance.bestClientsCount && clientsCount != 0);		
	}

	private void OnDestroy()
	{
		list.Remove(this);
	}

	public void SetColor(Material color)
	{
		/*
		Material m = neon.material;
		m.color = color.color;
		m.SetColor("_EmissionColor", color.color);
		neon.material = m;
		*/
		building.material = color;
	}
}
