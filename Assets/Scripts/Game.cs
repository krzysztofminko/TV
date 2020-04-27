using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static Game instance;

	public static Dictionary<string, AudioSource> sfx = new Dictionary<string, AudioSource>();

	public bool started;
	public bool ended;
	public int duration = 60;
	public float time;

	[Range(12,32)]
	public int size = 12;
	[Range(1,4)]
	public int players = 1;
	[Range(0, 10)]
	public int bonuses = 2;
	public float aiSpeed = 1.2f;

	public float cameraRotateDuration = 1;

	public int bestClientsCount;

	[Header("References")]
	public Transform SFXContainer;
	public Person personPrefab;
	public Transform personsParent;
	public List<Transform> playerSpawns;
	public List<Material> colors;

	Vector3 cameraOrigin;
	

	private void OnValidate()
	{
		if (size % 2 > 0)
			size++;
	}

	void Awake()
	{
		instance = this;

		for (int i = 0; i < SFXContainer.childCount; i++)
			sfx.Add(SFXContainer.GetChild(i).name, SFXContainer.GetChild(i).GetComponent<AudioSource>());

		cameraOrigin = Camera.main.transform.position;
		Person.InitPool(32 * 32);
		InitGame(size, players);
	}

	private void Update()
	{
		if (started)
		{
			time = Mathf.Max(0, time - Time.deltaTime);
			if (time == 0 && !ended)
				EndGame();
		}
		if (ended)
		{
			if(Camera.main.transform.eulerAngles.x > 40)
				Camera.main.transform.Rotate(Vector3.right, - Time.deltaTime * 5 / cameraRotateDuration);
		}
		else
		{
			if (Camera.main.transform.eulerAngles.x < 45)
				Camera.main.transform.Rotate(Vector3.right, Time.deltaTime * 5 / cameraRotateDuration);
		}


		bestClientsCount = Mathf.Max(Shop.list.Select(s => s.clientsCount).ToArray());

	}

	public void SpawnBonus()
	{
		if (Bonus.list.Count == 0)
		{
			int clamp = size / 2;
			Vector3 position = new Vector3(Random.Range(-clamp, clamp - 1), 0, Random.Range(-clamp, clamp - 1));
			Instantiate(Resources.Load("Bonuses/PowerBonus"), position, Quaternion.identity);
		}
	}

	public void InitPlayers(int players)
	{
		for (int i = TV.list.Count - 1; i >= 0 ; i--)
			DestroyImmediate(TV.list[i].gameObject);
		for (int i = Shop.list.Count -1; i >= 0 ; i--)
			DestroyImmediate(Shop.list[i].gameObject);
		

		Instantiate(Resources.Load<TV>("PlayerTV"), playerSpawns[0].position, Quaternion.identity).color = colors[0];
		for (int i = 1; i < players; i++)
			Instantiate(Resources.Load<TV>("AITV"), playerSpawns[i].position, Quaternion.identity).color = colors[i];

		for (int i = 0; i < players; i++)
		{
			TV.list[i].shop = Instantiate(Resources.Load<Shop>("Shop"), Vector3.forward * size + Vector3.right * i * 4 + Vector3.left * ((players - 1) * 2 + 0.5f), Quaternion.identity);
			TV.list[i].shop.SetColor(colors[i]);
		}

		this.players = players;
	}

	public void InitGame(int size, int players)
	{
		for (int i = Person.list.Count - 1; i >= 0; i--)
			Person.Despawn(Person.list[i]);

		for (int x = -size / 2; x < size / 2; x++)
			for (int z = -size / 2; z < size / 2; z++)
			{
				int clamp = size / 2;
				Person.Spawn(new Vector3(x, 0, z));
			}

		Camera.main.transform.position = new Vector3(cameraOrigin.x, cameraOrigin.y * size / 16, cameraOrigin.z * size / 16);

		this.size = size;
		InitPlayers(players);
	}
	
	public void StartGame()
	{
		started = true;
		ended = false;
		time = duration;
		InitGame(size, players);
		for (int i = 1; i < bonuses; i++)
			Invoke("SpawnBonus", i * duration / (bonuses + 1));
	}

	public void MainMenu()
	{
		started = false;
		ended = false;
		CancelInvoke();
		Bonus.ClearAll();
	}

	public void EndGame()
	{
		ended = true;
		CancelInvoke();
		Bonus.ClearAll();
		sfx["GameBG"].Stop();
		sfx["EndGame"].Play();
	}
}
