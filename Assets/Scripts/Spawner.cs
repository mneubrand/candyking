using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject newsPaper;

	private float nextSpawn;

	// Use this for initialization
	void Start () {
		nextSpawn = Time.time + 3;
	}
	
	// Update is called once per frame
	void Update () {
		// Spawn newspaper randomly
		if (Time.time > nextSpawn) {
			nextSpawn = Time.time + Random.Range (6, 8);

			float x = Random.value > 0.5f ? Random.Range (-26f, -10.32f) : Random.Range(9.6f, 26.34f);
			Instantiate(newsPaper, new Vector3(x, 13.2f, 0), Quaternion.identity);
		}
	}
}