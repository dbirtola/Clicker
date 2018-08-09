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
    public Button equipButton;

    public Text[] propertyTexts;

    public Color[] qualityColors = { Color.white, Color.blue, Color.yellow, Color.cyan, Color.green };


    public void UpdateWithItem(Item item)
    {
        targetItem = item;

        itemNameText.text = item.GetDisplayName(); ;
        itemNameText.color = qualityColors[(int)item.GetQuality()];

        if (item.GetComponent<Armor>())
        {
            Armor armor = item.GetComponent<Armor>();

            mainStatText.text = "Armor: " + armor.GetArmorValue().ToString();
            //healthText.text = "Health: " + armor.GetHealthValue().ToString();
        }

        if (item.GetComponent<Weapon>())
        {
            Weapon weapon = item.GetComponent<Weapon>();
            mainStatText.text = "Damage: " + weapon.GetDamageValue().ToString();
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
}
