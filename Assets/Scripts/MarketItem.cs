using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public int itemId, wearId;
    public int price;
    public Text priceText;
    int money;

    MarketItem marketItem;
    Item equippedItem;

    public GameObject itemPrefab;

    public Button buyButton, equipButton, unequipButton;
    public bool HasItem()
    {
        //0: Not Buy
        //1: Buy But Don't Wear
        //2: Buy And Wear
        bool hasItem = PlayerPrefs.GetInt("item" + itemId.ToString()) != 0;
        return hasItem;
    }
    public bool IsEquipped()
    {
        bool equippedItem = PlayerPrefs.GetInt("item" + itemId.ToString()) != 2;
        return equippedItem;
    }

    public void InitializeItem()
    {
        priceText.text = price.ToString();
        if (HasItem())
        {
            buyButton.gameObject.SetActive(false);
            if (IsEquipped())
            {
                EquipItem();
            }
            else
            {
                equipButton.gameObject.SetActive(true);
            }
        }
        else
        {
            buyButton.gameObject.SetActive(true);
        }
    }
    public void BuyItem()
    {
        if (!HasItem())
        {
            money = PlayerPrefs.GetInt("money");
            if (money >= price)
            {
                PlayerController.current.itemAudioSource.PlayOneShot(PlayerController.current.buyAudioCllip, 0.1f);
                LevelController.Current.GiveMoneyToPlayer(-price);
                PlayerPrefs.SetInt("item" + itemId.ToString(), 1);
                buyButton.gameObject.SetActive(false);
                equipButton.gameObject.SetActive(true);
            }
        }
    }

    public void EquipItem()
    {
        UnequipItem();
        MarketController.Current.equippedItems[wearId]= Instantiate(itemPrefab,PlayerController.current.wearSpots[wearId].transform).GetComponent<Item>();
        MarketController.Current.equippedItems[wearId].itemId = itemId;
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true);
        PlayerPrefs.SetInt("item" + itemId.ToString(), 2);
    }
    public void UnequipItem()
    {
        equippedItem = MarketController.Current.equippedItems[wearId];
        if (equippedItem != null)
        {
            marketItem = MarketController.Current.items[equippedItem.itemId];
            PlayerPrefs.SetInt("item" + marketItem.itemId, 1);
            marketItem.equipButton.gameObject.SetActive(true);
            marketItem.unequipButton.gameObject.SetActive(false);
            Destroy(equippedItem.gameObject);
        }
    }


    public void EquipItemButton()
    {
        PlayerController.current.itemAudioSource.PlayOneShot(PlayerController.current.equipAudioClip,0.1f);
    }

    public void UnequipItemButton()
    {
        PlayerController.current.itemAudioSource.PlayOneShot(PlayerController.current.unequipAudioClip, 0.1f);
    }
}
