using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
	public GameObject PlayerPrefab;

	GameObject[] spawnPoints;

	private void Start()
	{
		spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		if(spawnPoints.Length > 0)
		{
			int index = Random.Range(0, spawnPoints.Length);
			PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoints[index].transform.position, Quaternion.identity);
		}
	}
}
