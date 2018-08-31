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

public class ItemPickedUpEvent : UnityEvent<Item, bool>
{

}

[System.Serializable]
public class PlayerInventoryData
{
    public List<EquipmentData> equippedItems;
    public List<EquipmentData> heldItems;

}

public class PlayerInventory : MonoBehaviour {

    public const int MaxItems = 20;
    public const int MAX_EQUIPPED_CHARMS = 3;

    public InventoryModifiedEvent inventoryModifiedEvent;
    public ItemEquippedEvent itemEquippedEvent;
    public ItemPickedUpEvent itemPickedUpEvent;


    public Weapon equippedWeapon { get; private set; }
    public Armor equippedArmor { get; private set; }
    public Armor equippedGloves { get; private set; }
    public Armor equippedBoots { get; private set; }
    public Armor equippedHelmet { get; private set; }
    public Ring equippedRing { get; private set; }

    List<Equipment> items;

    List<Charm> charms;
    List<Charm> equippedCharms;

    //Using int instead of ItemQuality enum to allow for -1 in case user selects None
    public int autoSellQuality = -1;



    public void Awake()
    {
        items = new List<Equipment>();
        charms = new List<Charm>();
        equippedCharms = new List<Charm>();
        
        inventoryModifiedEvent = new InventoryModifiedEvent();
        itemEquippedEvent = new ItemEquippedEvent();
        itemPickedUpEvent = new ItemPickedUpEvent();
    }

    public Weapon GetWeapon()
    {
        return equippedWeapon;
    }

    public Ring GetRing()
    {
        return equippedRing;
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

    public List<Equipment> GetAllEquipped()
    {
        List<Equipment> l = new List<Equipment>();
        l.Add(equippedWeapon);
        l.Add(equippedArmor);
        l.Add(equippedHelmet);
        l.Add(equippedGloves);
        l.Add(equippedBoots);
        l.Add(equippedRing);

        return l;
    }

    public List<Equipment> getInventoryItems()
    {
        return items;
    }

    //Subject to auto-sell
    public bool PickUpItem(Item item)
    {
        if (addItem(item))
        {
            bool autoSell = (int)item.GetQuality() <= autoSellQuality;
            if (autoSell && item.GetComponent<Equipment>())
            {
                SellItem(item.GetComponent<Equipment>());
            }

            Debug.Log("Invoked");
            itemPickedUpEvent.Invoke(item, autoSell);
            return true;
        }else
        {
            return false;
        }


    }

    public bool addItem(Item item)
    {
        if (item == null)
        {
            return false;
        }

        if (item.GetComponent<Equipment>())
        {

            if (items.Count >= MaxItems)
            {
                return false;
            }
            items.Add(item.GetComponent<Equipment>());
            inventoryModifiedEvent.Invoke();
            return true;

        }
        if (item.GetComponent<Charm>()){
            if (charms.Count >= MaxItems)
            {
                return false;
            }

            Debug.Log("Added Charm");
            charms.Add(item.GetComponent<Charm>());
            inventoryModifiedEvent.Invoke();
            return true;
        }

        return false;
    }

    public bool RemoveItem(Item item)
    {
        if (item.GetComponent<Equipment>())
        {

            var temp = items.Remove(item.GetComponent<Equipment>());
            inventoryModifiedEvent.Invoke();
            return temp;
        }
        if (item.GetComponent<Charm>())
        {
            var temp = charms.Remove(item.GetComponent<Charm>());
            inventoryModifiedEvent.Invoke();
            return temp;
        }
        return false;
    }

    public bool SellItem(Equipment item)
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

    public bool EquipCharm(Charm charm)
    {
        if(equippedCharms.Contains(charm)){
            return false;
        }

        return true;
        //RemoveCharm(charm);
        

    }

    public bool EquipItem(Equipment item)
    {
        //Check if usable
        if (GetAllEquipped().Contains(item))
        {
            return false;

        }
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

        if (item.GetComponent<Ring>())
        {
            if(equippedRing != null)
            {
                equippedRing.Unequip();
            }

            temp = equippedRing;
            equippedRing = item.GetComponent<Ring>();
            equippedRing.Equip();
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
        pid.equippedItems = new List<EquipmentData>();

        //Forgot I made a function for this until after typing it all.. damn.

        var equippedItems = GetAllEquipped();
        foreach(Equipment item in equippedItems)
        {
            if(item == null)
            {
                //Still add null items to make sure everything stays in order. Order is critical to the save/load process
                pid.equippedItems.Add(null);
            }else
            {
                pid.equippedItems.Add(item.SaveEquipmentData());
            }
        }
        /*
        pid.equippedItems.Add(equippedWeapon.SaveItemData());
        pid.equippedItems.Add(equippedArmor.SaveItemData());
        pid.equippedItems.Add(equippedHelmet.SaveItemData());
        pid.equippedItems.Add(equippedGloves.SaveItemData());
        pid.equippedItems.Add(equippedBoots.SaveItemData());
        */

        pid.heldItems = new List<EquipmentData>();
        foreach(Equipment i in items)
        {
            pid.heldItems.Add(i.SaveEquipmentData());
        }

        return pid;
    }



    public void LoadInventoryData(PlayerInventoryData pid)
    {
        ItemFactory itemFactory = FindObjectOfType<ItemFactory>();

        foreach(EquipmentData id in pid.equippedItems)
        {
            if(id != null)
            {
                Equipment newItem = itemFactory.SpawnEquipmentOfType(id.itemType);
                newItem.LoadEquipmentData(id);
                EquipItem(newItem);
            }
        }

        foreach(EquipmentData id in pid.heldItems)
        {
            if(id != null)
            {
                Equipment newItem = itemFactory.SpawnEquipmentOfType(id.itemType);
                newItem.LoadEquipmentData(id);
                addItem(newItem);
            }
        }
    }
}
