using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public static PlayerController Instance;

    [SerializeField] public int c_Emeralds;
    [SerializeField] public int c_Coins;
    [SerializeField] public int c_Diamonds;
    [SerializeField] public bool hasSeasonPass;
    [SerializeField] public int c_SeasonPass;
    [SerializeField] public bool EnableAds = true;
    [SerializeField] public int c_EnableAds;
    [SerializeField] public int kittployees;
    [SerializeField] public int kittyCD;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    bool HasSeasonPass()
    { 
        bool value;
        if (c_SeasonPass == 1)
        {
            value = true;
        }
        else
        {
            value = false;
            c_SeasonPass = 0;
        }
        
        return value;
    }
    
    bool EnablePlayerAds()
    {
        bool value;
        if (c_EnableAds == 1)
        {
            value = true;
        }
        else
        {
            value = false;
            c_EnableAds = 0;
        }

        return value;
    }

    void GetSavedData()
    {
        c_Emeralds = PlayerPrefs.GetInt("Emeralds");
        c_Coins = PlayerPrefs.GetInt("Coins");
        c_Diamonds = PlayerPrefs.GetInt("Diamonds");
        c_SeasonPass = PlayerPrefs.GetInt("SeasonPass");
        c_EnableAds = PlayerPrefs.GetInt("EnableAds");

        EnableAds = EnablePlayerAds();
        hasSeasonPass = HasSeasonPass();
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetInt("Emeralds", c_Emeralds);
        PlayerPrefs.SetInt("Coins", c_Coins);
        PlayerPrefs.SetInt("Diamonds", c_Diamonds);
        PlayerPrefs.SetInt("SeasonPass", c_SeasonPass);
        PlayerPrefs.SetInt("EnableAds", c_EnableAds);

    }

    public void PurchaseCurrencyFromShop()
    {
        IAPButton button = GameObject.Find("").GetComponent<IAPButton>();

        if (button.productId == "001")
        {
            c_SeasonPass = 1;
            hasSeasonPass = true;
            SaveGameData();
        }
        else if (button.productId == "002")
        {
            //disableAds
            EnableAds = false;
            c_EnableAds = 0;
            SaveGameData();
        }
        else if (button.productId == "100")
        {
            c_Coins = c_Coins + 100;
            SaveGameData();
        }
        else if (button.productId == "101")
        {
            c_Coins = c_Coins + 550;
            SaveGameData();
        }
        else if (button.productId == "102")
        {
            c_Coins = c_Coins + 1275;
            SaveGameData();
        }
    }
}
