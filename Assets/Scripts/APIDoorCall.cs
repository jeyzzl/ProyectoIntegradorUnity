using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class APIDoorCall : MonoBehaviour
{

    private String doorState = " ";

    private Animator myDoor = null;

    // Start is called before the first frame update
    void Start()
    {

        myDoor = GetComponent<Animator>();
        StartCoroutine(GetRequest("https://basic-api-lock.vercel.app/checkDoor"));
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetRequest("https://basic-api-lock.vercel.app/checkDoor")); 

        if(doorState == "true"){
           if(myDoor != null){
            myDoor.Play("Base Layer.DoorAnim", 1, 0.0f);
            Debug.Log("Entre");
           }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page]+ "Error: {0}" + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                     Debug.LogError(pages[page]+"HTTP Error: {0}" + webRequest.error);
                     break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(String.Format(webRequest.downloadHandler.text));
                    doorState = String.Format(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
