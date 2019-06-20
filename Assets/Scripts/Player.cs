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
    private List<Vector2> moves;
    private bool is_moving = false;

    public void invert_move()
    {
        this.is_moving = !this.is_moving;
    }

    public bool get_is_moving()
    {
        return this.is_moving;
    }

    public void init(Vector2 position, Inventory inventory, Squad squad, int move_limit)
    {
        this.position = position;
        this.inventory = inventory;
        this.squad = squad;
        this.move_limit = move_limit;
        this.moves = new List<Vector2>();
    }

    public void add_moves(List<Vector2> _moves)
    {
        for (int i = 0; i < _moves.Count; i++)
        {
            this.moves.Add(_moves[i]);
        }
    }

    public List<Vector2> output_moves()
    {
        return this.moves;
    }

    public Vector2 make_move()
    {
        Vector2 temp = this.moves[0];
        this.moves.RemoveAt(0);
        return temp;
    }

    public int moves_count()
    {
        return this.moves.Count;
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
