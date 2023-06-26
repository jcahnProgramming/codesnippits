using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class PlayerSaveData : MonoBehaviour
{
    [SerializeField] public static PlayerSaveData instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public string playerName { get; set; }
    public string playerStartDateTime { get; set; }
    
    public int playerCard { get; set; }

    public int playerWeapon { get; set; }

    public Dictionary<int, Sprite> playerCards { get; set; }

    public Dictionary<int, GameObject> playerWeapons { get; set; }

    public PlayerSaveData(string m_playerName, int m_playerCard, int m_playerWeapon)
    {
        playerName = m_playerName;
        playerStartDateTime = System.DateTime.Now.ToString();
        playerCard = m_playerCard;
        playerWeapon = m_playerWeapon;
    }

    public PlayerSaveData(Sprite m_newPlayerCard)
    {
        playerCards.Add(playerCards.Count + 1, m_newPlayerCard);
    }

    public PlayerSaveData(GameObject m_newPlayerWeapon)
    {
        playerWeapons.Add(playerWeapons.Count + 1, m_newPlayerWeapon);
    }

    public void NewPlayerWeapon(GameObject weapon)
    {
        playerWeapons.Add(playerWeapons.Count + 1, weapon);
    }

    public void NewPlayerCard(Sprite playerCard)
    {
        playerCards.Add(playerCards.Count + 1, playerCard);
    }

    public void SaveCurrentPlayerInformation()
    {
        PlayerPrefs.SetString("PlayerData", JsonConvert.SerializeObject(GameManager.instance.GetCurrentUser()));
    }

    public void LoadPlayerSaveData()
    {
        string playerDataRaw = PlayerPrefs.GetString("PlayerData");

        PlayerSaveData loadedUser = (PlayerSaveData)JsonConvert.DeserializeObject(playerDataRaw);

        GameManager.instance.UpdateCurrentUser(loadedUser);
    }
}
