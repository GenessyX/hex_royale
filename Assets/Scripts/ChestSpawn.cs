using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject primary;
    public GameObject shield;
    public GameObject spell;


    public void Init()
    {
       
    }
}

public class ChestSpawn : MonoBehaviour
{
    public GameObject chest_prefab;
    // Start is called before the first frame update
    public void Chest_Spawn()
    {

        int child_count = GameObject.Find("Map").GetComponent<Map>().childCount;
        Debug.Log(child_count);
        for (int i = 0; i < 30; i++)
        {
            //Vector2 position = new Vector2((int)Random.Range(-25.5f, 25.5f), (int)Random.Range(-44.1f, 44.1f));
            Transform hex = GameObject.Find("Map").transform.GetChild((int)Random.Range(0, child_count - 1)); 
            
            //GameObject.GetComponent<Map>().grid_to_world(position)
            GameObject chest = Instantiate(chest_prefab,hex.GetComponent<Hex>().world_position + new Vector3(0,0.5f,0), Quaternion.identity);
            chest.AddComponent<Chest>();
        }
    }

    public void Loot_Spawn()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

