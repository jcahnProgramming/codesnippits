using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Text;
using Amazon;
using AWSSDK;
using Amazon.S3;
using Amazon.CognitoIdentity;

public class ServerConnection : MonoBehaviour
{
    public GameObject LoginScreen;
    public GameObject MainMenuScreen;
    public Dropdown BrainScanUI;
    public CurrentUser currentUser;
    public User user;
    public string usernameString;
    private string apiKey = "haPTf8dnMt578zPItAaLF9Krn4PEl40N1HFti4Po";
    private string loginEndPoint = "https://za3usq9al5.execute-api.us-east-1.amazonaws.com/prod/login";

    private string token;

    public GameObject NavigationPane;
    public GameObject UpperBar;

    public Utility utility;
    public UserInterface ui;

    [Header("UI")]
    public Text errorMsg;

    public InputField username;
    public InputField password;

[HideInInspector]
    public LoginRequest loginRequest;
    public GameManager gm;
    public AWSSDK.Examples.S3Connection s3Connection;

    // Start is called before the first frame update
    void Start()
    {
        errorMsg.text = "";
        loginRequest = new LoginRequest();
        utility.LoggingFromOtherScripts("Server Connection Script Initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginAttempt()
    {
        utility.LoggingFromOtherScripts("User is attempting to log in...");
        string issue = null;

        if (username.text != null && password.text != null)
        {
            gm.ResetStreamingAssets();
            StartCoroutine(Upload());
            utility.LoggingFromOtherScripts("User has typed in a valid username/password combo > Checking with connection server.");
        }
        else
        {
            if (username.text == null)
            {
                issue = "Error: Missing Username";
                utility.LoggingFromOtherScripts(issue);
            }
            if (password.text == null)
            {
                issue = "Error: Missing Password";
                utility.LoggingFromOtherScripts(issue);
            }
            if (username.text == null && password.text == null)
            {
                issue = "Error: Missing Username and Password";
                utility.LoggingFromOtherScripts(issue);
            }
            errorMsg.text = issue;
            utility.LoggingFromOtherScripts(issue);
        }
    }
    public string ConvertToJSon()
    {
        User tmp = new User();
        tmp.username = username.text;
        tmp.password = password.text;
        usernameString = username.text;

        string json = JsonConvert.SerializeObject(tmp);
        Debug.Log(json);
        utility.LoggingFromOtherScripts("Json has been converted and serialized.");
        return json;
    }
    public void errorMsgUpdate(string msg)
    {
        errorMsg.color = new Color(0, 0, 0, 225);
        errorMsg.text = msg;
    }
    public void ResetPassword()
    {
        Application.OpenURL("http://hoth-app.s3-website-us-east-1.amazonaws.com/"); // update this w/ the forgot password link once set up
        utility.LoggingFromOtherScripts("User has been sent to Staging Website.");
    }
    IEnumerator Upload()
    {
        utility.LoggingFromOtherScripts("User has qualified Username and Password | Login System Initializing...");
        string myData = ConvertToJSon();
        var request = new UnityWebRequest(loginEndPoint, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(myData);
        utility.LoggingFromOtherScripts("Byte Array is being Processed with User Credentials.");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-api-key", apiKey);
        yield return request.SendWebRequest();
        utility.LoggingFromOtherScripts("Web Request to Amazon AWS Cognito Services Intialized...");
        Debug.Log("Status Code: " + request.responseCode);
        utility.LoggingFromOtherScripts(request.responseCode.ToString());

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            errorMsg.color = new Color(255, 0, 15, 255);
            errorMsg.text = request.error.ToString();
            utility.LoggingFromOtherScripts(errorMsg.text);
        }
        else
        {
            utility.LoggingFromOtherScripts("User has successfully accessed Cognito Database with Active Credentials | Logging User In...");
            Debug.Log("Form Upload Complete!");
            errorMsg.color = new Color(58, 225, 0, 225);
            errorMsg.text = "Login Successful!";
            token = request.downloadHandler.text;
            PlayerPrefs.SetString("token", token);
            utility.LoggingFromOtherScripts("User Token added to local registry.");
            currentUser = CurrentUser.CreateFromJson(token);
            ui.username = username.text.ToLower();
            username.text = "";
            password.text = "";
            utility.LoggingFromOtherScripts("new test log");
            s3Connection.GetObjects(ui.username);
            utility.LoggingFromOtherScripts("Gaining .obj file data from S3 Bucket");
            LoginScreen.SetActive(false);
            ui.Launch();
            ui.panelTitle.text = "Launch";
            utility.LoggingFromOtherScripts("Launching Main Navigation and Launch Screen");
            utility.LoggingFromOtherScripts("S3 Objects have been gained, adding to /StreamingAssets/ local folder.");
            gm.TrackingBtnEnableDisable(false);
            utility.LoggingFromOtherScripts("Closing S3 Connection");
        }
    }
}
