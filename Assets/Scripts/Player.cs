using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 position;
    private Inventory inventory;
    private Squad squad;
    private int move_limit;
    private int damage;
    private int armor;

    
    public void init(Vector2 position, Inventory inventory, Squad squad, int move_limit)
    {
        this.position = position;
        this.inventory = inventory;
        this.squad = squad;
        this.move_limit = move_limit;
    }

    public void move_to(Vector2 position)
    {
        this.position = position;
    }

    public Vector2 get_position()
    {
        return position;
    }

    public void update_stats()
    {
        this.damage = inventory.get_inventory()[GameObject.Find("Player").GetComponent<Inventory>().slots["weapon"]].get_damage();
        this.armor = inventory.get_inventory()[GameObject.Find("Player").GetComponent<Inventory>().slots["armor"]].get_armor();
    }

    public Inventory get_inventory()
    {
        return this.inventory;
    }

}
