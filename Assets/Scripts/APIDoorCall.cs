using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class APIDoorCall : MonoBehaviour
{

    private String doorState = " ";
    private Boolean state = false;

    private Animator myDoor = null;

    // Start is called before the first frame update
    void Start()
    {

        myDoor = GetComponent<Animator>();
        //StartCoroutine(GetRequest("https://basic-api-lock.vercel.app/checkDoor"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 240 == 0){
        StartCoroutine(GetRequest("https://basic-api-lock.vercel.app/checkDoor")); 
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            //yield return new WaitForSeconds(3);
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
                    Debug.Log(String.Format(webRequest.downloadHandler.text));
                    doorState = String.Format(webRequest.downloadHandler.text);
                    break;
            }
            
            if(!state){
                if(doorState == "true"){
                    if(myDoor != null){
                        state = true;
                        myDoor.Play("Base Layer.DoorAnim", 0, 0.0f);
                        Debug.Log("Open");
                    }
                }
            }else{{
                
                if(doorState == "false"){
                    state = false;
                    if(myDoor != null){
                        myDoor.Play("Base Layer.DoorAnimClose", 0, 0.0f);
                        Debug.Log("Close");
                    }
                }
            }}
            
        }
    }
}