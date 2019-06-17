using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> inventory = new List<Item>() { null, null, null };
    public Dictionary<string, int> slots = new Dictionary<string, int>()
    {
        {"weapon", 0 },
        {"armor", 1 },
        {"artifact", 2 }
    };

    public void add_item(Item item)
    {
        if (inventory[slots[item.get_type()]] == null)
        {
            this.inventory[slots[item.get_type()]] = item;
        }
    }

    public void remove_item(Item item)
    {
        if (inventory[slots[item.get_type()]] != null)
        {
            this.inventory[slots[item.get_type()]] = null;
        }
    }

    public List<Item> get_inventory()
    {
        return this.inventory;
    }
}

public class Item : MonoBehaviour
{
    public int ID;
    public string item_name;
    private string type;
    private int damage;
    private int armor;

    public void init(int ID, string item_name, string type, int damage, int armor)
    {
        this.ID = ID;
        this.item_name = item_name;
        this.type = type;
        this.damage = damage;
        this.armor = armor;
    }

    public string get_type()
    {
        return this.type;
    }

    public int get_damage()
    {
        return this.damage;
    }

    public int get_armor()
    {
        return this.armor;
    }

}