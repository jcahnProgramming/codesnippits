using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PullData : MonoBehaviour
{
    public Dropdown test;
    private string locationPath = "https://hoth-files.s3.amazonaws.com/";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://hoth-files.s3.amazonaws.com/");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            byte[] results = www.downloadHandler.data;
        }

        
    }
}
