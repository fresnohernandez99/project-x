using Socket.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
	public const string LISTEN_PLAYER_ID = "listen_player_id"; 
	public const string LISTEN_UPDATE_POSITIONS = "update_positions";
	public const string SEND_SHARED_DATA = "shared_data";

	public bool isListenPositions = false;

	private QSocket socket;
    EventManager em = (EventManager) EventManager.Instance;
    
    public bool isConnected = false;

    public NetworkManager() {}

	public void TrayConnection(){
		if (!isConnected) {
			ConnectSocket();
		}
	}

    private void ConnectSocket(){
		var server = EnviromentGameData.Instance.playerSavedData.serverIp;
		socket = IO.Socket ("http://"+ server +":3000");
		Debug.Log("http://" + server + ":3000");
		socket.On (QSocket.EVENT_CONNECT, () => {
			isConnected = true;
			ListenPlayerId();
		});

		socket.On (QSocket.EVENT_DISCONNECT, () => {
			isConnected = false;
			
		});

		socket.On ("chat", data => {
		});
    }

	//Listenners
	public void ListenPlayerId()
    {
		socket.On(LISTEN_PLAYER_ID, data => {
			Debug.Log(data.ToString());
			EnviromentGameData.Instance.playerSharedData.id = data.ToString();
			socket.Off(LISTEN_PLAYER_ID);
			Debug.Log(JsonUtility.ToJson(EnviromentGameData.Instance.playerSharedData));
			socket.Emit(SEND_SHARED_DATA, JsonUtility.ToJson(EnviromentGameData.Instance.playerSharedData));
			ListenUpdatePositions();
		});
	}

	public void ListenUpdatePositions()
	{
		if (!isListenPositions)
        {
			Debug.Log("escuchando posisiones");
			socket.On(LISTEN_UPDATE_POSITIONS, data => {
				em.TriggerEvent(EventManager.PLAYER_POSITIONS, data.ToString());
			});

			isListenPositions = true;
		}
	}

	public void CloseListenUpdatePositions()
	{
		socket.Off(LISTEN_UPDATE_POSITIONS);

		isListenPositions = false;
	}

	//Emisors
	public void EmitPlayerUpdate()
    {
		socket.Emit(SEND_SHARED_DATA, JsonUtility.ToJson(EnviromentGameData.Instance.playerSharedData));
	}

	public void DisconnectSocket(){
		isConnected = false;
		socket.Disconnect();
	}

}
