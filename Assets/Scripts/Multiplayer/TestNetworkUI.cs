using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNetworkUI : MonoBehaviour
{
	[SerializeField] 
	private NetworkManager networkManager;

	[SerializeField]
	private Button b_StartServer;

	[SerializeField]
	private Button b_StartHost;

	[SerializeField]
	private Button b_StartClient;

	private void Awake()
	{
		b_StartServer.onClick.AddListener(() => networkManager.StartServer());
		b_StartHost.onClick.AddListener(() => networkManager.StartHost());
		b_StartClient.onClick.AddListener(() => networkManager.StartClient());

		networkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
	}

	private void NetworkManager_OnClientConnectedCallback(ulong obj)
	{
		networkManager.NetworkUpdate(NetworkUpdateStage.FixedUpdate);
	}
}
