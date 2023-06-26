using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System.IO;
using Unity.MARS;
using System.Linq;
using Amazon;
using AWSSDK;


public class GameManager : MonoBehaviour
{
    [Header("C# References")]
    public UserInterface ui;
    public Utility utility;
    public AWSConnectionVariables aws;
    public ChangeView changeView;
    public Tracking tracking;
    public SessionUI sessionUI;

    [Header("Camera Systems")]
    public bool cameraFacingUser = false; //true = Face Tracking | false = World Tracking
    public GameObject arSessionOrigin;

    [Header("User Selections")]
    public int selectedObj = 0;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        sessionUI.TogglePaused();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetStreamingAssets()
    {
#if UNITY_EDITOR
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.streamingAssetsPath);
#elif UNITY_IOS
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.persistentDataPath;
#endif
        foreach (System.IO.FileInfo file in di.EnumerateFiles())
        {
            file.Delete();
        }
        foreach (System.IO.DirectoryInfo dir  in di.EnumerateDirectories())
        {
            dir.Delete(true);
        }
    }

    void SetUITrackingSystem(bool value)
    {
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("Tracking System Initializing...");
        }
        ui.uiScreen.SetActive(false);
        ui.ResetNavigationPane();
        ui.AllPanelsOff();
        ui.faceTrackingUIPanel.SetActive(true);
        ui.launchBtn.GetComponent<Image>().color = ui.activeBtn;
        ui.UIToggleReset(true);
    }

    public void BodyTrackingActive()
    {
        sessionUI.TogglePaused();
        cameraFacingUser = false;
        ToggleARCamera(true);
        SetUITrackingSystem(true);
        tracking.isBodyTracking(true, selectedObj);
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("Body Tracking System Successfully Loaded...");
        }
    }

    public void CustomOBJTrackingActive()
    {
        sessionUI.TogglePaused();
        cameraFacingUser = false;
        ToggleARCamera(true);
        SetUITrackingSystem(true);
        tracking.isOBJTracking(true, selectedObj);
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("Custom Tracking System Successfully Loaded...");
        }
    }

    public void FaceTrackingActive()
    {
        sessionUI.TogglePaused();
        cameraFacingUser = true;
        ToggleARCamera(true);
        SetUITrackingSystem(true);
        tracking.isFaceTracking(true);
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("Default Face Tracking System Successfully Loaded...");
        }
    }

    public void TrackingBtnEnableDisable(bool value)
    {
        ui.bodyTrackingBtn.GetComponent<Button>().interactable = value;
        ui.objUploadTrackingBtn.GetComponent<Button>().interactable = value;
        if (value)
        {
            ui.bodyTrackingBtn.GetComponent<Button>().interactable = value;
            ui.objUploadTrackingBtn.GetComponent<Button>().interactable = value;
            if (utility.isLoggingEnabled)
            {
                utility.LoggingFromOtherScripts($"Body Tracking Btn Interactable Status: {value} | Custom Object Upload Tracking Btn Interactable Status: {value}");
            }
        }
    }

    public void ObjUploadTrackingBtnInteraction(bool value)
    {
        ui.objUploadTrackingBtn.GetComponent<Button>().interactable = value;
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts($"Custom Object Upload Tracking Btn Interactable Status: {value}");
        }
    }

    
    void ToggleARCamera(bool value)
    {
        arSessionOrigin.GetComponent<ARPlaneManager>().enabled = false;
        arSessionOrigin.GetComponent<ARFaceManager>().enabled = false;

        if (value)
        {
            utility.LoggingFromOtherScripts("Camera Facing User: [ " + cameraFacingUser + " ]");
            if (cameraFacingUser)
            {
                utility.LoggingFromOtherScripts("Camera is Facing User - Inside IF Statement");
                arSessionOrigin.GetComponent<ARFaceManager>().enabled = true;
            }
            else
            {
                utility.LoggingFromOtherScripts("Camera is Facing World - Inside ELSE Statement");
                arSessionOrigin.GetComponent<ARPlaneManager>().enabled = true;
            }
        }
    }

    public void RegisterBtn()
    {
        Application.OpenURL("http://hoth-app.s3-website-us-east-1.amazonaws.com/register");
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("User clicked on the Register Button -- Sending user to Internet Browser(Register)...");
        }
    }
}
