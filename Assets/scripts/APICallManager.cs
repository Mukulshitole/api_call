using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using muk;
using TMPro;



    public class APICallManager : MonoBehaviour
    {
        // URL to the API endpoint
        public string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    // Reference to the UI Text components to display the information
    public TextMeshProUGUI class1Text;
    public TextMeshProUGUI class2Text;
    public TextMeshProUGUI class3Text;

    void Start()
        {
            // Start the coroutine to fetch data from the API
            StartCoroutine(GetDataFromAPI());
        }

        IEnumerator GetDataFromAPI()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
            {
                // Send the request and wait for the response
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("API request failed: " + webRequest.error);
                }
                else
                {
                    // Deserialize the JSON response into a Root object
                    string jsonResponse = webRequest.downloadHandler.text;
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonResponse);

                    // Access the data for class 1, class 2, and class 3
                    _1 class1Data = myDeserializedClass.data._1;
                    _2 class2Data = myDeserializedClass.data._2;
                    _3 class3Data = myDeserializedClass.data._3;

                    // Display the information on the canvas
                    class1Text.text = "Class 1:\n" +
                        "Address: " + class1Data.address + "\n" +
                        "Name: " + class1Data.name + "\n" +
                        "Points: " + class1Data.points;

                    class2Text.text = "Class 2:\n" +
                        "Address: " + class2Data.address + "\n" +
                        "Name: " + class2Data.name + "\n" +
                        "Points: " + class2Data.points;

                    class3Text.text = "Class 3:\n" +
                        "Address: " + class3Data.address + "\n" +
                        "Name: " + class3Data.name + "\n" +
                        "Points: " + class3Data.points;
                }
            }
        }
    }

