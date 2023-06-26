using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [Header("C# References")]
    public GameManager gm;
    public Utility utility;
    public AWSConnectionVariables aws;
    public ChangeView changeView;

    [Header("User Profile UI Elements")]
    public Sprite DownloadActive;
    public Sprite DownloadInactive;
    public Dropdown dropdownForDownloadList;
    public Button DownloadObjButton;
    public Text selectedDropdownItemLabel;
    bool isDownloadBtnActive = false;
    bool isOBJDownloaded = false;
    public string username;
    public Text usernameText;
    public GameObject UpperUserProfileNavigation;
    public GameObject userProfilePanel;
    public string lastScreen = "";

    [Header("Upper Navigation Pane")]
    public Text panelTitle;
    public Image userImage;

    [Header("Main Panels")]
    public GameObject loginPanel;
    public GameObject faceTrackingUIPanel;
    public GameObject swapObjectsPanelPane;
    public GameObject objectSwapTrackingPanel;
    public GameObject uiScreen;

    [Header("Secondary Panels")]
    public GameObject launchPanel;
    public GameObject useCasesPanel;
    public GameObject toolsPanel;

    [Header("Navigation Panels")]
    public GameObject UpperPanelPane;
    public GameObject NavigationPane;
    public GameObject swapObjectsUpperPane;

    [Header("Navigation Pane")]
    public Button toolsBtn;
    public Button launchBtn;
    public Button useCasesBtn;
    public Color activeBtn = new Color32(64, 155, 224, 255);
    public Color standardBtn = new Color32(255, 255, 255, 255);

    [Header("Live Tracking UI Buttons")]
    public Toggle brainToggle;
    public Toggle ventriclesToggle;
    public Toggle skullToggle;
    public Button faceMaskToggleBtn;

    [Header("Launch Panel Info")]
    public Button faceTrackingBtn;
    public Button bodyTrackingBtn;
    public Button objUploadTrackingBtn;
    public Button swapObjectTrackingBtn;
    public bool isLoggedIn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch()
    {
        LaunchSystem();
    }

    void UIScreenToggle(bool value)
    {
        uiScreen.SetActive(value);
    }

    public void AllPanelsOff()
    {
        loginPanel.SetActive(false);
        launchPanel.SetActive(false);
        useCasesPanel.SetActive(false);
        toolsPanel.SetActive(false);
        userProfilePanel.SetActive(false);
        UpperUserProfileNavigation.SetActive(false);
        faceTrackingUIPanel.SetActive(false);
        UpperPanelPane.SetActive(false);
        NavigationPane.SetActive(false);
        swapObjectsPanelPane.SetActive(false);
        swapObjectsUpperPane.SetActive(false);
        changeView.SwapObjTrackingLowerPanel.SetActive(false);
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("All Panels have been turned off.");
        }
    }

    public void UIToggleReset(bool value)
    {
        brainToggle.gameObject.SetActive(value);
        ventriclesToggle.gameObject.SetActive(value);
        skullToggle.gameObject.SetActive(value);
        brainToggle.isOn = value;
        ventriclesToggle.isOn = value;
        skullToggle.isOn = value;

    }

    void UpperPanelPaneActivated()
    {
        UpperPanelPane.SetActive(true);
        NavigationPane.SetActive(true);
    }

    void LaunchSystem()
    {
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("Launch Method Initialized");
        }
        UIScreenToggle(true);
        AllPanelsOff();
        UpperPanelPaneActivated();
        launchPanel.SetActive(true);

        if (!isLoggedIn)
        {
            if (utility.isLoggingEnabled)
            {
                utility.LoggingFromOtherScripts("User has not been fully logged in, Initializing Data Now...");
            }

            bodyTrackingBtn.onClick.AddListener(gm.BodyTrackingActive);
            objUploadTrackingBtn.onClick.AddListener(gm.CustomOBJTrackingActive);
            faceTrackingBtn.onClick.AddListener(gm.FaceTrackingActive);
            swapObjectTrackingBtn.onClick.AddListener(SwapObjectsPane);
            isLoggedIn = true;
            gm.TrackingBtnEnableDisable(true);

            if (utility.isLoggingEnabled)
            {
                utility.LoggingFromOtherScripts("User is now fully logged in!");
            }
        }

        if (isOBJDownloaded)
        {
            gm.ObjUploadTrackingBtnInteraction(true);
            if (utility.isLoggingEnabled)
            {
                utility.LoggingFromOtherScripts("Object Tracking Button Interaction Enabled");
            }
        }
        ResetNavigationPane();
        launchBtn.GetComponent<Image>().color = activeBtn;
        panelTitle.text = "Launch";
        lastScreen = "launch";
    }

    void SwapObjectsPane()
    {
        AllPanelsOff();
        ResetNavigationPane();
        swapObjectsPanelPane.SetActive(true);
        NavigationPane.SetActive(true);
        swapObjectsUpperPane.SetActive(true);
    }

    public void ResetNavigationPane()
    {
        toolsBtn.GetComponent<Image>().color = standardBtn;
        launchBtn.GetComponent<Image>().color = standardBtn;
        useCasesBtn.GetComponent<Image>().color = standardBtn;
        if (utility.isLoggingEnabled)
        {
            utility.LoggingFromOtherScripts("Navigation Button Colors - Reset");
        }
    }
}
