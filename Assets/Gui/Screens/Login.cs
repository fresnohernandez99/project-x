using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    public GameObject lineEdit;
    public GameObject placeholder;
    public GameObject dialog;
    public GameObject classBtnText;

    private string selectedClass = "";
    // Start is called before the first frame update
    void Start()
    {
        var actualName = EnviromentGameData.Instance.playerSavedData.playerName;
        placeholder.GetComponent<Text>().text = actualName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue(){
        var newName = lineEdit.GetComponent<InputField>().text;
        bool checkName = checkUsername(newName);

        if (!checkName) {
            var notificationMsg = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "DIALOG_BAD_NAME");
            dialog.GetComponent<DialogAcceptCancel>().CreateNotification(notificationMsg);
        } else if (selectedClass == "") {
            var notificationMsg = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "DIALOG_NOT_CLASS_SELECTED");    
            dialog.GetComponent<DialogAcceptCancel>().CreateNotification(notificationMsg);
        } else {
            EnviromentGameData.Instance.UpdatePlayer(newName, selectedClass);
            SecurePlayerPrefs.SaveGameData();
            SceneManager.LoadScene(0);
        }
    }

    private bool checkUsername(string arg){
        bool checking = true;

        if (arg.Length == 0) checking = false;

        if (!Regex.Match(arg, "^[a-zA-Z0-9]+$").Success) checking = false;

        return checking;
    }

    public void SelectClass(){
        if(selectedClass == "" || selectedClass == "thief") {
            selectedClass = "warrior";
            var className = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "WARRIOR");
            classBtnText.GetComponent<Text>().text = className;
        } else if (selectedClass == "warrior") {
            selectedClass = "archer";
            var className = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "ARCHER");
            classBtnText.GetComponent<Text>().text = className;
        } else if (selectedClass == "archer") {
            selectedClass = "thief";
            var className = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "THIEF");
            classBtnText.GetComponent<Text>().text = className;
        }}
}
