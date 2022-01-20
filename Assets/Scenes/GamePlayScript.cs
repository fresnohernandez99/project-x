using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScript : MonoBehaviour
{
    public GameObject clouds1;
    public GameObject clouds2;
    public GameObject clouds3;
    public GameObject clouds4;

    public float Cloud1Speed = 0.0000000002f;
    public float Cloud2Speed = 0.0000000007f;
    public float Cloud3Speed = 0.0000000005f;
    public float Cloud4Speed = 0.0000000006f;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        MoveClouds();
    }

    private void MoveClouds(){
        if (clouds1.transform.position.x > -60) {
            clouds1.transform.position = new Vector3(clouds1.transform.position.x - Cloud1Speed * Time.deltaTime, clouds1.transform.position.y, clouds1.transform.position.z);
        } else clouds1.transform.position = new Vector3(230, clouds1.transform.position.y, clouds1.transform.position.z);

        if (clouds2.transform.position.x > -60) {
            clouds2.transform.position = new Vector3(clouds2.transform.position.x - Cloud2Speed * Time.deltaTime, clouds2.transform.position.y, clouds2.transform.position.z);
        } else clouds2.transform.position = new Vector3(230, clouds2.transform.position.y, clouds2.transform.position.z);

        if (clouds3.transform.position.x > -60) {
            clouds3.transform.position = new Vector3(clouds3.transform.position.x - Cloud3Speed * Time.deltaTime, clouds3.transform.position.y, clouds3.transform.position.z);
        } else clouds3.transform.position = new Vector3(230, clouds3.transform.position.y, clouds3.transform.position.z);

        if (clouds4.transform.position.x > -60) {
            clouds4.transform.position = new Vector3(clouds4.transform.position.x - Cloud4Speed * Time.deltaTime, clouds4.transform.position.y, clouds4.transform.position.z);
        } else clouds4.transform.position = new Vector3(230, clouds4.transform.position.y, clouds4.transform.position.z);
    }
}
