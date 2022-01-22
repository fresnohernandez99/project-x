using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    public GameObject voice;
    private Animator anim;
    public GameObject text;

    private string[] texts;

    private int actualText = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = voice.GetComponent<Animator>();
        texts = new string[] {
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_1"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_2"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_3"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_4"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_5"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_6"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_7"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_8"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_9"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_10"),
            LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CINE_11")
        };
    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = texts[actualText];
    }

    public void Next()
    {
        if (actualText < 10)
        {
            actualText++;

            switch(actualText)
            {
                case 1:
                    anim.SetBool("isDuding", true);
                    break;
                case 2:
                    anim.SetBool("isDuding", false);
                    anim.SetBool("isHappy", true);
                    break;
                case 3:
                    anim.SetBool("isHappy", false);
                    anim.SetBool("isNormal", true);
                    break;
                case 4:
                    anim.SetBool("isNormal", false);
                    anim.SetBool("isUff", true);
                    break;
                case 6:
                    anim.SetBool("isNormal", true);
                    anim.SetBool("isUff", false);
                    break;
                case 7:
                    anim.SetBool("isNormal", false);
                    anim.SetBool("isUff", true);
                    break;
                case 8:
                    anim.SetBool("isUff", false);
                    anim.SetBool("isBad", true);
                    break;
                case 9:
                    anim.SetBool("isSayingYes", true);
                    anim.SetBool("isBad", false);
                    break;
                case 10:
                    anim.SetBool("isSayingYes", false);
                    anim.SetBool("isBad", true);
                    break;

            }

        } else
        {
            SceneManager.LoadScene(0);
        }
    }
}
