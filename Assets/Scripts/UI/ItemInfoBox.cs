using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoBox : MonoBehaviour {

    Item targetItem;

    public Text itemNameText;
    public Text mainStatText;
    public Text healthText;
    public Text implicitPropertyText;
    public Text sellText;
    public Button equipButton;

    public Text[] propertyTexts;

    public Color[] qualityColors = { Color.white, Color.blue, Color.yellow, Color.cyan, Color.green };


    public void UpdateWithItem(Item item)
    {
        targetItem = item;

        itemNameText.text = item.GetDisplayName(); ;
        itemNameText.color = qualityColors[(int)item.GetQuality()];

        if (sellText != null)
        {

            sellText.text = "Sell (" + item.GetSaleValue().ToString() + " taps)";
        }

        if (item.GetComponent<Armor>())
        {
            Armor armor = item.GetComponent<Armor>();

            mainStatText.text = "Armor: " + armor.armorValue;
            if(armor.refineLevel > 0)
            {
                mainStatText.text += "(+" + (armor.GetTotalArmorValue() - armor.armorValue).ToString() + ")";
            }
            //healthText.text = "Health: " + armor.GetHealthValue().ToString();
        }

        if (item.GetComponent<Weapon>())
        {
            Weapon weapon = item.GetComponent<Weapon>();
            mainStatText.text = "Damage: " + weapon.damageValue;

            if (weapon.refineLevel > 0)
            {
                mainStatText.text += "(+" + (weapon.GetTotalDamageValue() - weapon.damageValue).ToString() + ")";
            }
        }

        if (item.GetComponent<Ring>())
        {
            Ring ring = item.GetComponent<Ring>();
            mainStatText.text = "Health: " + ring.healthValue;

            if (ring.refineLevel > 0)
            {
                mainStatText.text += "(+" + (ring.GetTotalHealthValue() - ring.healthValue).ToString() + ")";
            }
        }
       
        implicitPropertyText.text = item.GetImplicitProperty().GetDisplayString();

        for (int i = 0; i < propertyTexts.Length; i++)
        {
            if (i < item.GetItemProperties().Count)
            {
                propertyTexts[i].text = item.GetItemProperties()[i].GetDisplayString();
            }else
            {
                propertyTexts[i].text = "";
            }
        }

        if(equipButton != null)
        {
            equipButton.interactable = !item.GetEquipped();
        }
       
    }

    public void EquipItem()
    {
        FindObjectOfType<PlayerInventory>().EquipItem(targetItem);
        gameObject.SetActive(false);
    }

    public void SellItem()
    {
        FindObjectOfType<PlayerInventory>().SellItem(targetItem);
        gameObject.SetActive(false);
    }
}
