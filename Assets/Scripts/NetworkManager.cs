using Socket.Quobject.SocketIoClientDotNet.Client;

public class NetworkManager : Singleton<NetworkManager>
{
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

		socket.On (QSocket.EVENT_CONNECT, () => {
			isConnected = true;
		});

		socket.On (QSocket.EVENT_DISCONNECT, () => {
			isConnected = false;
			
		});

		socket.On ("chat", data => {
		});
    }

	public void DisconnectSocket(){
		isConnected = false;
		socket.Disconnect();
	}

}
