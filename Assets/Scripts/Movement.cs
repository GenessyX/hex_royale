using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class Player : MonoBehaviour
{
    public Vector2 position;

    public void init(Vector2 position)
    {
        this.position = position;
    }
    public void move_to(Vector2 where)
    {
        this.position = where;
    }

}

public class Movement : MonoBehaviour
{
    public float speed = 0.005f;

    public GameObject player_prefab;
    public GameObject map;
    public Camera camera;
    private GameObject player_go;
    private GameObject movement;
    private PathFinder pathFinder;

    public void Start()
    {

        List<Vector2> test = new List<Vector2>();
        test.Add(new Vector2(0, 0));

        map = GameObject.Find("Map");

        pathFinder = GameObject.Find("Player").GetComponent<PathFinder>();
        //.GetComponent<PathFinder>();
        //pathFinder = this.GetComponent<PathFinder>();

        int grid_mid = (map.GetComponent<Map>().grid_width - 1) / 2;
        player_go = Instantiate(player_prefab, get_world_pos(map, new Vector2(grid_mid,grid_mid)), Quaternion.identity);
        player_go.AddComponent<Player>();
        player_go.GetComponent<Player>().init(new Vector2(grid_mid, grid_mid));
        player_go.transform.SetParent(this.transform);
    }

    public Vector3 get_world_pos(GameObject map, Vector2 grid_pos)
    {
        Transform move_hex = map.transform.Find(string.Format("Hexagon ({0}|{1})", grid_pos.x, grid_pos.y));
        return move_hex.GetComponent<Hex>().world_position;
    }

    public void update_pos(GameObject player_go)
    {
        
        Transform move_hex = map.transform.Find(string.Format("Hexagon ({0}|{1})", player_go.GetComponent<Player>().position.x , player_go.GetComponent<Player>().position.y));

        if (move_hex != null)
        
            //player_go.transform.Translate(move_hex.GetComponent<Hex>().world_position * Time.deltaTime * speed);       часть Вани

            player_go.transform.position = move_hex.GetComponent<Hex>().world_position;
        

    }

    public void Update()
    {
        update_pos(player_go);
        if (Input.GetKeyDown("right"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().position + new Vector2(1,0));
            update_pos(player_go);
        }
        
        if (Input.GetKeyDown("left"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().position + new Vector2(-1, 0));
            update_pos(player_go);
        }
        if (Input.GetKeyDown("up"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().position + new Vector2(0, 1));
            update_pos(player_go);
        }
        if (Input.GetKeyDown("down"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().position + new Vector2(0, -1));
            update_pos(player_go);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Transform where_to = get_mouse_hit();
            if (where_to != null)
            {
                Vector2 hex_pos = get_position_object(where_to);
                Vector2 pos = player_go.GetComponent<Player>().position;
                List<Vector2> path = pathFinder.GetComponent<PathFinder>().find_path(player_go.GetComponent<Player>().position,hex_pos);
                foreach (Vector2 hex in path)
                {

                    //movement = Instantiate(player_prefab, get_world_pos(map, new Vector2(hex.x, hex.y)), Quaternion.identity);
                }

                player_go.GetComponent<Player>().move_to(hex_pos);
                update_pos(player_go);
            }
        }
        
    }
    
    public Vector2 get_position_object(Transform hex)
    {
        return hex.GetComponent<Hex>().grid_position;
    }
    
    public Transform get_mouse_hit()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            return objectHit;
        }
        return null;
    }

}