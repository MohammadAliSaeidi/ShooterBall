using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
	public InputField i_CreateRoom;
	public InputField i_JoinRoom;

	public Button b_CreateRoom;
	public Button b_JoinRoom;

	private void Start()
	{
		b_CreateRoom.onClick.AddListener(() => { CreateRoom(); });
		b_JoinRoom.onClick.AddListener(() => { JoinRoom(); });
	}

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(i_CreateRoom.text);
	}

	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(i_JoinRoom.text);
	}

	public override void OnJoinedRoom()
	{
		PhotonNetwork.LoadLevel("MechanicTestScene");
	}
}
