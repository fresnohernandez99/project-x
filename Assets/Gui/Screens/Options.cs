using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Options : MonoBehaviour
{
    public GameObject lineEdit;
    public GameObject placeholder;
    public GameObject dialog;
    public GameObject indicator;
    public GameObject musicSlider;
    public GameObject sfxSlider;

    private NetworkManager nm = (NetworkManager) NetworkManager.Instance;
    // Start is called before the first frame update
    void Start()
    {
        var actualServer = EnviromentGameData.Instance.playerSavedData.serverIp;
        placeholder.GetComponent<Text>().text = actualServer;

        musicSlider.GetComponent<Slider>().value = EnviromentGameData.Instance.playerSavedData.music;
        sfxSlider.GetComponent<Slider>().value = EnviromentGameData.Instance.playerSavedData.sfx;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoMain(){
        SecurePlayerPrefs.SaveGameData();
        SceneManager.LoadScene(0);
    }

    public void CheckServer(){
        var posibleIp = lineEdit.GetComponent<InputField>().text;
        
        bool checking = true;

        if (posibleIp.Length == 0) checking = false;

        if (!checkIfServerIsIp(posibleIp) && !checkIfServerIsHost(posibleIp)) checking = false;

        if (!checking) {
            var notificationMsg = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "DIALOG_BAD_ADDRESS");
            dialog.GetComponent<DialogAcceptCancel>().CreateNotification(notificationMsg);
        } else {
            EnviromentGameData.Instance.playerSavedData.serverIp = posibleIp;
            SecurePlayerPrefs.SaveGameData();
            indicator.GetComponent<NetIndicator>().startWatch = false;
            nm.DisconnectSocket();
            indicator.GetComponent<NetIndicator>().ClickRetray();
        }

    }

    public void UpdateMusicVolume(float arg){
        EnviromentGameData.Instance.playerSavedData.music = arg;
    }

    public void UpdateSoundVolume(float arg){
        EnviromentGameData.Instance.playerSavedData.sfx = arg;
    }

    public void ResetSaves(){
        lineEdit.GetComponent<InputField>().text = "";
        musicSlider.GetComponent<Slider>().value = 100.0F;
        sfxSlider.GetComponent<Slider>().value = 100.0F;

        PlayerPrefs.DeleteAll();
        EnviromentGameData.Instance.ResetData();

        var actualServer = EnviromentGameData.Instance.playerSavedData.serverIp;
        placeholder.GetComponent<Text>().text = actualServer;
        indicator.GetComponent<NetIndicator>().startWatch = false;
        nm.DisconnectSocket();
        indicator.GetComponent<NetIndicator>().ClickRetray();
    }

    private bool checkIfServerIsIp(string arg){
        bool checking = true;

        if (!Regex.Match(arg, "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$").Success) checking = false;

        return checking;
    }

    private bool checkIfServerIsHost(string arg){
        bool checking = true;

        if (!Regex.Match(arg, "^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]*[a-zA-Z0-9])\\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\\-]*[A-Za-z0-9])$").Success) checking = false;

        return checking;
    }
}
