using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryModifiedEvent : UnityEvent
{

}

public class ItemEquippedEvent : UnityEvent<Item>
{

}

[System.Serializable]
public class PlayerInventoryData
{
    public List<ItemData> equippedItems;
    public List<ItemData> heldItems;

}

public class PlayerInventory : MonoBehaviour {

    public const int MaxItems = 20;

    public InventoryModifiedEvent inventoryModifiedEvent;
    public ItemEquippedEvent itemEquippedEvent;

    public Weapon equippedWeapon { get; private set; }
    public Armor equippedArmor { get; private set; }
    public Armor equippedGloves { get; private set; }
    public Armor equippedBoots { get; private set; }
    public Armor equippedHelmet { get; private set; }

    List<Item> items;

    //Using int instead of ItemQuality enum to allow for -1 in case user selects None
    public int autoSellQuality = -1;



    public void Awake()
    {
        items = new List<Item>();
        inventoryModifiedEvent = new InventoryModifiedEvent();
        itemEquippedEvent = new ItemEquippedEvent();
    }

    public Weapon GetWeapon()
    {
        return equippedWeapon;
    }

    public Armor GetArmor()
    {
        return equippedArmor;
    }


   public Armor GetGloves()
    {
        return equippedGloves;
    }

    public Armor GetBoots()
    {
        return equippedBoots;
    }

    public Armor GetHelmet()
    {
        return equippedHelmet;
    }

    public List<Item> GetAllEquipped()
    {
        List<Item> l = new List<Item>();
        l.Add(equippedWeapon);
        l.Add(equippedArmor);
        l.Add(equippedHelmet);
        l.Add(equippedGloves);
        l.Add(equippedBoots);
        //l.Add(equippedRing);

        return l;
    }

    public List<Item> getInventoryItems()
    {
        return items;
    }

    //Subject to auto-sell
    public bool PickUpItem(Item item)
    {
        if (addItem(item))
        {
            if ((int)item.GetQuality() <= autoSellQuality)
            {
                SellItem(item);
            }
            return true;
        }else
        {
            return false;
        }


    }

    public bool addItem(Item item)
    {
        if(items.Count >= MaxItems || item == null)
        {
            return false;
        }
        items.Add(item);
        inventoryModifiedEvent.Invoke();

        return true;
    }

    public bool RemoveItem(Item item)
    {
        var temp = items.Remove(item); 
        inventoryModifiedEvent.Invoke();
        return temp;
    }

    public bool SellItem(Item item)
    {
        if (items.Contains(item))
        {
            // Debug.Log("Selling for x gold");
            var craft = GetComponent<PlayerCrafting>();
            craft.AddMaterials(craft.GetMiningMaterialAmount() * item.GetSaleValue());
        }else
        {
            return false;
        }

        RemoveItem(item);

        return true;
    }

    public bool EquipItem(Item item)
    {
        //Check if usable

        RemoveItem(item);


        Item temp = null;

        if (item.GetComponent<Weapon>())
        {
            if(equippedWeapon != null)
            {
                equippedWeapon.Unequip();
            }

            temp = equippedWeapon;
            equippedWeapon = item.GetComponent<Weapon>();
            equippedWeapon.Equip();

        }

        if (item.GetComponent<Armor>())
        {

            switch (item.GetComponent<Armor>().slot)
            {
                case ArmorSlot.Armor:
                    if(equippedArmor != null)
                    {
                        equippedArmor.Unequip();
                    }

                    temp = equippedArmor;
                    equippedArmor = item.GetComponent<Armor>();
                    equippedArmor.Equip();
                    break;

                case ArmorSlot.Boots:
                    if(equippedBoots != null)
                    {
                        equippedBoots.Unequip();
                    }

                    temp = equippedBoots;
                    equippedBoots = item.GetComponent<Armor>();
                    equippedBoots.Equip();
                    break;

                case ArmorSlot.Gloves:
                    if (equippedGloves != null)
                    {
                        equippedGloves.Unequip();
                    }

                    temp = equippedGloves;
                    equippedGloves = item.GetComponent<Armor>();
                    equippedGloves.Equip();
                    break;

                case ArmorSlot.Helmet:
                    if (equippedHelmet != null)
                    {
                        equippedHelmet.Unequip();
                    }

                    temp = equippedHelmet;
                    equippedHelmet = item.GetComponent<Armor>();
                    equippedHelmet.Equip();
                    break;
            }




        }

        if (temp != null)
        {
            addItem(temp);
        }

        itemEquippedEvent.Invoke(item);
        inventoryModifiedEvent.Invoke();

        //itemEquippedEvent.Invoke(item);
        //inventoryModifiedEvent.Invoke();

        return true;
    }

    public PlayerInventoryData SaveInventoryData()
    {
        PlayerInventoryData pid = new PlayerInventoryData();
        pid.equippedItems = new List<ItemData>();

        //Forgot I made a function for this until after typing it all.. damn.

        var equippedItems = GetAllEquipped();
        foreach(Item item in equippedItems)
        {
            if(item == null)
            {
                //Still add null items to make sure everything stays in order. Order is critical to the save/load process
                pid.equippedItems.Add(null);
            }else
            {
                pid.equippedItems.Add(item.SaveItemData());
            }
        }
        /*
        pid.equippedItems.Add(equippedWeapon.SaveItemData());
        pid.equippedItems.Add(equippedArmor.SaveItemData());
        pid.equippedItems.Add(equippedHelmet.SaveItemData());
        pid.equippedItems.Add(equippedGloves.SaveItemData());
        pid.equippedItems.Add(equippedBoots.SaveItemData());
        */

        pid.heldItems = new List<ItemData>();
        foreach(Item i in items)
        {
            pid.heldItems.Add(i.SaveItemData());
        }

        return pid;
    }



    public void LoadInventoryData(PlayerInventoryData pid)
    {
        ItemFactory itemFactory = FindObjectOfType<ItemFactory>();

        foreach(ItemData id in pid.equippedItems)
        {
            if(id != null)
            {
                Item newItem = itemFactory.SpawnItemOfType(id.itemType);
                newItem.LoadItemData(id);
                EquipItem(newItem);
            }
        }

        foreach(ItemData id in pid.heldItems)
        {
            if(id != null)
            {
                Item newItem = itemFactory.SpawnItemOfType(id.itemType);
                newItem.LoadItemData(id);
                addItem(newItem);
            }
        }
    }
}
