using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using TMPro;
using Asset;


public class getrequest : MonoBehaviour
{
    public class Fact
    {
        public string fact { get; set; }
        public int length { get; set; }
    }

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("https://catfact.ninja/facts"));
    }
    public void onrefresh()
    {
        Start();
    }
   IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(string.Format("something went wrong:{0}", webRequest.error));
                    break;
                case UnityWebRequest.Result.Success:
                    //  Fact fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);
                   
                     Factroot facts = JsonConvert.DeserializeObject<Factroot>(webRequest.downloadHandler.text);
                    text.text = facts.data[0].fact;
                    foreach(var fact in facts.data)
                    {
                        Debug.Log(fact.fact);
                    }
                    break;


            }
        }
    }
}
