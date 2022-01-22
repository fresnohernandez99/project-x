using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetIndicator : MonoBehaviour
{
    public GameObject indicator;
    public GameObject retrayBtn;

    private bool isConnected = false;
    public bool startWatch = false;
    private NetworkManager nm = (NetworkManager) NetworkManager.Instance;
    // Start is called before the first frame update
    void Start()
    {
        EnviromentGameData.Instance.LoadInitialData(
            SecurePlayerPrefs.GetJson<PlayerSavedData>(Constants.PLAYER_DATA_SAVED, Constants.PRIVATE_KEY)
        );

        ClickRetray();
    }

    // Update is called once per frame
    void Update()
    {
        isConnected = nm.isConnected;
        WatchConnection();
    }

    private void WatchConnection(){
        if (startWatch){
            if (isConnected) indicator.GetComponent<Image>().color = new Color32(0, 254, 0, 255);
            else indicator.GetComponent<Image>().color = new Color32(254, 0, 0, 255);
        } else {
            indicator.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void ClickRetray(){
        retrayBtn.SetActive(false);
        nm.TrayConnection();
        StartCoroutine(ShowRetrayBtn());
    }

    public IEnumerator ShowRetrayBtn()
    {
        yield return new WaitForSeconds(5);

        startWatch = true;
        if(!isConnected) {
            retrayBtn.SetActive(true);
        }
        StartCoroutine(ShowRetrayBtn());
    }
}
