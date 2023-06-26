using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterface : MonoBehaviour
{
    [SerializeField]public static UserInterface instance;

    [SerializeField] public static GameObject m_Settings;
    [SerializeField] public static GameObject m_Customize;
    [SerializeField] public static GameObject m_Popup;
    [SerializeField] public static GameObject m_MainMenu;


    public PlayerSaveData tmpData;
    public string tmpuserName;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DefineVariables();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DefineVariables()
    {
        m_Popup = GameObject.Find("MasterCanvas/PopupWindow");
        m_Settings = GameObject.Find("MasterCanvas/Settings");
        m_Customize = GameObject.Find("MasterCanvas/Customize");
        m_MainMenu = GameObject.Find("MasterCanvas/MainMenu");

    }

    public void SettingsBtn()
    {
        m_Settings.SetActive(true);





        //SaveData.instance.SaveGame();
    }

    public void CustomizeBtn()
    {
        m_Customize.SetActive(true);
        //SaveData.instance.SaveGame();
    }

    public void PlayerCardBtn()
    {
        if (m_Customize.activeInHierarchy)
        {
            LocalDB.instance.GetPlayerCards();


        }
    }

    public void PlayerWeaponsBtn()
    {
        if (m_Customize.activeInHierarchy)
        {
            LocalDB.instance.GetPlayerWeapons();


        }
    }

    public void PlayBtn()
    {
        if (GameManager.instance.currentUser == null)
        {
            GameManager.instance.NewPlayer();
        }
        else
        {
            GameManager.instance.LaunchGame();
        }
    }

    public void PopupWindow(string value)
    {
        m_Popup.SetActive(true);
        var titleText = GameObject.Find("popupTitle");
        var popupInformationText = GameObject.Find("popupInfoText");

        var bottomLeft = GameObject.Find("BottomPanel/LeftPanel");
        var bottomMiddle = GameObject.Find("BottomPanel/MiddlePanel");
        var bottomRight = GameObject.Find("BottomPanel/RightPanel");

        GameObject SaveBtn = GameObject.Find("MiddlePanel/RightPanel/SaveBtn");

        var bottomLeftText = GameObject.Find("BottomPanel/LeftPanel/Top");
        var bottomLeftUserInput = GameObject.Find("BottomPanel/LeftPanel/Bottom");

        var bottomMiddleText = GameObject.Find("BottomPanel/MiddlePanel/Top");
        var bottomMiddleUserInput = GameObject.Find("BottomPanel/MiddlePanel/Bottom");

        var bottomRightText = GameObject.Find("BottomPanel/RightPanel/Top");
        var bottomRightUserInput = GameObject.Find("BottomPanel/RightPanel/Bottom");

        switch (value)
        {
            case "newUser":
                {
                    SaveBtn.GetComponent<Button>().onClick.AddListener(SaveNewUser);

                    titleText.GetComponent<TMP_Text>().text = "New User";
                    popupInformationText.GetComponent<TMP_Text>().text = $"Welcome to {GameInformation.instance.GameName}! Please fill out the information below to get started!";
                    bottomMiddle.gameObject.SetActive(true);
                    bottomMiddleText.GetComponent<TMP_Text>().text = "Username:";
                    tmpuserName = bottomMiddleUserInput.GetComponent<TMP_InputField>().text;
                }
                break;

            default:
                break;
        }
    }

    public void SaveNewUser()
    {
        tmpData = new PlayerSaveData(tmpuserName, 0, 0);
        GameManager.instance.UpdateCurrentUser(tmpData);

        tmpuserName = null;
        tmpData = null;

        m_Popup.SetActive(false);
    }

    public void BackBtn()
    {
        m_Customize.SetActive(false);
        m_Popup.SetActive(false);
        m_Settings.SetActive(false);
        m_MainMenu.SetActive(true);

    }
}
