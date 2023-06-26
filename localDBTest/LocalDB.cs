using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalDB : MonoBehaviour
{
    [SerializeField] public static LocalDB instance;

    [SerializeField] public static Dictionary<int, Sprite> playerCards;

    [SerializeField] public static Dictionary<int, GameObject> playerWeapons;

    [SerializeField] public static Dictionary<int, Sprite> playerWeaponImages;

    public GameObject playerCardPlaceholder;
    public Transform playerCardPlaceholderTransform;

    void Start()
    {
        instance = this;
    }

    public Sprite GetPlayerCard(int cardNumber)
    {
        Sprite requestedPlayerCard;

        playerCards.TryGetValue(cardNumber, out requestedPlayerCard);

        return requestedPlayerCard;
    }

    public GameObject GetPlayerWeapon(int weaponNumber)
    {
        GameObject requestedPlayerWeapon = playerWeapons[weaponNumber];

        return requestedPlayerWeapon;
    }

    public void GetPlayerCards()
    {
        Vector3 transformPos = playerCardPlaceholderTransform.position;

        for (int i = 0; i < playerCards.Count; i++)
        {
            var placeholder = Instantiate(playerCardPlaceholder, transformPos, Quaternion.identity);
            placeholder.gameObject.GetComponent<Image>().sprite = playerCards[i];
            
        }
    }
    public void GetPlayerWeapons()
    {
        Vector3 transformPos = playerCardPlaceholderTransform.position;

        for (int i = 0; i < playerWeapons.Count; i++)
        {
            var placeholder = Instantiate(playerCardPlaceholder, transformPos, Quaternion.identity);
            placeholder.gameObject.GetComponent<Image>().sprite = playerWeaponImages[i];
            placeholder.AddComponent<Button>();
            var btn = placeholder.GetComponent<Button>();
            btn.onClick.AddListener(PlayerChoosesWeapon);
        }
    }

    private void PlayerChoosesWeapon()
    {
        GameObject playerChoice = gameObject;
        PlayerSaveData currentPlayer = GameManager.instance.GetCurrentUser();
        currentPlayer.NewPlayerWeapon(playerChoice.gameObject);
    }

    private void PlayerChoosesCard()
    {
        GameObject playerChoice = gameObject;
        PlayerSaveData currentPlayer = GameManager.instance.GetCurrentUser();
        currentPlayer.NewPlayerCard(playerChoice.GetComponent<Sprite>());
    }
}
