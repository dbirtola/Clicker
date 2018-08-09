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

    public bool addItem(Item item)
    {
        if(items.Count >= MaxItems && item != null)
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

    
}
