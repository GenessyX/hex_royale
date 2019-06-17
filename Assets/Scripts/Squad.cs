using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    private int ID;
    private string name;
    private string info;

}

public class Minion
{
    private int health;
    private int mana;
    private string race;
    private string Class;
    private Skill skill;

    public void init(int health, int mana, string race, string Class, Skill skill)
    {
        this.health = health;
        this.mana = mana;
        this.race = race;
        this.Class = Class;
        this.skill = skill;
    }

    public void take_damage(int damage)
    {
        this.health -= damage;
    }

    public void use_skill()
    {

    }


}

public class Squad : MonoBehaviour
{
    
}
