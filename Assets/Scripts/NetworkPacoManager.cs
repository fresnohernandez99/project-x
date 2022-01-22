using Server.Base;
using Server.Base.Payloads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


public class NetworkPacoManager : Singleton<NetworkPacoManager>
{
    EventManager em = (EventManager) EventManager.Instance;
    
    public bool isConnected = false;

	public ClientEngine ce = new ClientEngine();

    public NetworkPacoManager() {}

	public void TrayConnection(){
		if (!isConnected) {
			ConnectSocket();
		}
	}

    private void ConnectSocket(){
		ce.ServerIp = EnviromentGameData.Instance.playerSavedData.serverIp;

		Debug.Log(ce.ServerIp);
		ce.ServerPort = 9999;

		ce.Connected += (obj, args) =>
		{
			isConnected = true;
		};


		ce.Connect(new ConnectionRequestPayload()
		{
			Name = "GOku99",
			Level = 1,
			Class = "warrior"
		});
    }

	public void DisconnectSocket(){
		isConnected = false;
		
	}

}
