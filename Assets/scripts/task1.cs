using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using TMPro;
using muk;
using System.Text;

public class task1 : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TMP_Dropdown filterDropdown;
    public GameObject buttonPrefab; // Reference to the button prefab
    private Root clientsData;
    private Data clientname;
    private Dictionary<int, GameObject> clientButtons = new Dictionary<int, GameObject>();
    public GameObject canvas1;
    public GameObject canvas2;
    public Transform buttonContainer; // Reference to the parent object for the buttons
    public TextMeshProUGUI nameTextMeshPro;
    public TextMeshProUGUI pointsTextMeshPro;
    public TextMeshProUGUI addressTextMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        buttonPrefab.SetActive(true);
        FetchDataAndCreateButtons();
    }

    public void OnRefresh()
    {
        FetchDataAndCreateButtons();
    }

    private void FetchDataAndCreateButtons()
    {
        // Clear the previous buttons before fetching new data
        ClearButtons();

        StartCoroutine(GetRequest("https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data"));
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
                    clientsData = JsonConvert.DeserializeObject<Root>(webRequest.downloadHandler.text);
                  

                    // Clear the previous text
                    text.text = "";

                    // Displaying each client's information based on the selected filter
                    ApplyFilter();
                    break;
            }
        }
    }

    public void OnFilterChanged()
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        // Clear the previous buttons
        ClearButtons();

        // Get the selected option from the Dropdown
        string selectedFilter = filterDropdown.options[filterDropdown.value].text;

        // Activate the buttonPrefab before creating buttons
        buttonPrefab.SetActive(true);

        // Displaying each client's information based on the selected filter
        bool anyButtonCreated = false;
        foreach (var client in clientsData.clients)
        {
            if (selectedFilter == "All" || (selectedFilter == "Is Manager" && client.isManager) || (selectedFilter == "Is Not Manager" && !client.isManager))
            {
                string line = $"ID: {client.id}, IsManager: {client.isManager}, Label: {client.label}\n";
                CreateButton(line);

                anyButtonCreated = true;
            }
        }

        // Deactivate the buttonPrefab if no button is created
        if (anyButtonCreated)
        {
            buttonPrefab.SetActive(false);
        }
    }


    public  void CreateButton(string buttonText)
    {
        // Extract the client ID from the button text
        int clientId = GetClientIdFromText(buttonText);

        // Check if the client ID is valid
        if (clientId >= 0)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, buttonContainer); // Instantiate the button under the buttonContainer
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI buttonTextMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonTextMeshPro.text = buttonText;
            // Log the created client ID for debugging
            Debug.Log($"Created button for Client ID: {clientId}");

            // Add an event listener to the button to trigger OnButtonClicked method with the corresponding client ID
            button.onClick.AddListener(() => OnButtonClicked(clientId));

            // Store the button reference in the dictionary using the client ID as the key
            clientButtons.Add(clientId, buttonGO);
        }
        else
        {
            Debug.LogError($"Invalid client ID for button text: {buttonText}");
        }
       
        
    }
    public void OnButtonClicked(int clientId)
    {
        if (clientsData == null)
        {
            return; // If the data is not fetched yet, do nothing
        }

        // Find the button using the client ID
        if (clientButtons.TryGetValue(clientId, out GameObject buttonGO))
        {
            // Get the class data for the corresponding button ID (Class 1, Class 2, Class 3)
            switch (clientId)
            {
                case 1:

                    _1 class1Data = clientsData.data._1;
                    // Display the class data (You can use class1Data properties as needed)
                    Debug.Log("Class 1 Data:");
                    Debug.Log("Address: " + class1Data.address);
                    Debug.Log("Name: " + class1Data.name);
                    Debug.Log("Points: " + class1Data.points);

                    // Update Unity TextMeshProUGUI elements with class1Data properties
                    canvas1.SetActive(false);
                    canvas2.SetActive(true);
                    nameTextMeshPro.text = class1Data.name;
                    pointsTextMeshPro.text = class1Data.points.ToString();
                    addressTextMeshPro.text = class1Data.address;
                    break;
                case 2:
                    _2 class2Data = clientsData.data._2;
                    // Display the class data (You can use class2Data properties as needed)
                    Debug.Log("Class 2 Data:");
                    Debug.Log("Address: " + class2Data.address);
                    Debug.Log("Name: " + class2Data.name);
                    Debug.Log("Points: " + class2Data.points);

                    // Update Unity TextMeshProUGUI elements with class2Data properties
                    canvas1.SetActive(false);
                    canvas2.SetActive(true);
                    nameTextMeshPro.text = class2Data.name;
                    pointsTextMeshPro.text = class2Data.points.ToString();
                    addressTextMeshPro.text = class2Data.address;
                    break;
                case 3:
                    _3 class3Data = clientsData.data._3;
                    // Display the class data (You can use class3Data properties as needed)
                    Debug.Log("Class 3 Data:");
                    Debug.Log("Address: " + class3Data.address);
                    Debug.Log("Name: " + class3Data.name);
                    Debug.Log("Points: " + class3Data.points);

                    // Update Unity TextMeshProUGUI elements with class3Data properties
                    canvas1.SetActive(false);
                    canvas2.SetActive(true);
                    nameTextMeshPro.text = class3Data.name;
                    pointsTextMeshPro.text = class3Data.points.ToString();
                    addressTextMeshPro.text = class3Data.address;
                    break;
                default:
                    Debug.LogError("Invalid button ID: " + clientId);
                    break;
            }
        }
    }



    private int GetClientIdFromText(string buttonText)
    {
        // Extract the client ID from the button text (Assuming "ID: clientId, ...")
        int startIndex = buttonText.IndexOf("ID: ") + 4;
        int endIndex = buttonText.IndexOf(",", startIndex);
        if (startIndex >= 4 && endIndex > startIndex)
        {
            string clientIdString = buttonText.Substring(startIndex, endIndex - startIndex);
            int clientId;
            if (int.TryParse(clientIdString, out clientId))
            {
                return clientId;
            }
        }
        return 0;
        
    }

    private void ClearButtons()
    {
        // Destroy all existing buttons and clear the dictionary
        foreach (var buttonGO in clientButtons.Values)
        {
            Destroy(buttonGO);
        }
        clientButtons.Clear();
    }

    public void back()
    {
        canvas1.SetActive(true);
        canvas2.SetActive(false);
    }
   
}