using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UnityAction onTest;
    private EventManager em = (EventManager) EventManager.Instance;
    private NetworkManager nm = (NetworkManager) NetworkManager.Instance;

    public static PlayerSharedData playerSharedData = new PlayerSharedData();
    
    void Start () {
        initSaves();
    }

    private void initSaves(){
        //Load initial data is not exist create new one
        EnviromentGameData.Instance.LoadInitialData(
            SecurePlayerPrefs.GetJson<PlayerSavedData>(Constants.PLAYER_DATA_SAVED, Constants.PRIVATE_KEY)
        );
        //Checking if is new data
        if (EnviromentGameData.Instance.playerSavedData.isNoob) {
            Debug.Log("No Hay salvas del juego");
            EnviromentGameData.Instance.playerSavedData.isNoob = false;
            SecurePlayerPrefs.SetJson(Constants.PLAYER_DATA_SAVED, EnviromentGameData.Instance.playerSavedData, Constants.PRIVATE_KEY);
            PlayerPrefs.Save();
            Debug.Log("Creadas salvas de iniciales");
            SceneManager.LoadScene(1);
        }
    }

    public void GoPlay(){
        SceneManager.LoadScene(3);
    }

    public void GoOptions(){
        SceneManager.LoadScene(2);
    }

}