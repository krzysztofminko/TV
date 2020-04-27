using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
	public static List<Bonus> list = new List<Bonus>();

	public GameObject model;
	public float totalDuration;

	internal float duration;
	internal TV executor;
	internal bool executed;

	AudioSource audioSource;

	private void Awake()
	{
		list.Add(this);
		audioSource = GetComponent<AudioSource>();
	}

	private void OnDestroy()
	{
		list.Remove(this);
	}

	private void Update()
	{		
		if (executed)
		{
			duration += Time.deltaTime;
			if (duration > totalDuration)
				Destroy(gameObject);
		}
		else
		{
			transform.Rotate(Vector3.up, Time.deltaTime * 180);
		}
	}

	public virtual void Execute(TV executor)
	{
		if (!executed)
		{
			executed = true;
			this.executor = executor;
			Destroy(model.gameObject);
			audioSource.Play();
		}
	}

	public static void ClearAll()
	{
		for (int i = list.Count - 1; i >= 0 ; i--)
			Destroy(list[i].gameObject);
	}
}
