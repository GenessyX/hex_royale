using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Movement : MonoBehaviour
{
    public float speed = 0.5f;


    public GameObject player_prefab;
    private GameObject map;
    public Camera camera;
    public List<Vector2> path;
    public GameObject path_prefab;
    private GameObject player_go;
    private GameObject movement;
    private PathFinder pathFinder;
    
    public float timer = 0;

    public void Start()
    {
        map = GameObject.Find("Map");
        path = new List<Vector2>();
        pathFinder = GameObject.Find("Player").GetComponent<PathFinder>();

        int grid_mid = (map.GetComponent<Map>().grid_width - 1) / 2;
        player_go = Instantiate(player_prefab, get_world_pos(map, new Vector2(grid_mid,grid_mid)), Quaternion.identity);
        player_go.AddComponent<Player>();
        player_go.AddComponent<Inventory>();
        player_go.AddComponent<Squad>();

        player_go.GetComponent<Player>().init(new Vector2(grid_mid, grid_mid), player_go.GetComponent<Inventory>(), player_go.GetComponent<Squad>(), 10);

        Debug.Log(player_go.GetComponent<Player>().get_inventory().get_inventory());

        player_go.transform.SetParent(this.transform);
    }


    public Vector3 get_world_pos(GameObject map, Vector2 grid_pos)
    {
        Transform move_hex = map.transform.Find(string.Format("Hexagon ({0}|{1})", grid_pos.x, grid_pos.y));
        return move_hex.GetComponent<Hex>().world_position;
    }


    public void update_pos(GameObject player_go)
    {
        Transform move_hex = map.transform.Find(string.Format("Hexagon ({0}|{1})", player_go.GetComponent<Player>().get_position().x, player_go.GetComponent<Player>().get_position().y));
        if (move_hex != null)
            if (player_go.transform.position == move_hex.GetComponent<Hex>().world_position)
                player_go.transform.position = move_hex.GetComponent<Hex>().world_position;
            else
            {
                player_go.transform.Translate((move_hex.GetComponent<Hex>().world_position - player_go.transform.position), Space.World);
            }
    }

    public void build_path(List<Vector2> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 path_pos = GameObject.Find("Map").GetComponent<Map>().grid_to_world(path[i]);
            GameObject path_hex = Instantiate(path_prefab, path_pos + new Vector3(0,0.215f,0), Quaternion.identity, GameObject.Find("Map").GetComponent<Map>().find_hex_by_grid(path[i]));
            //path_hex.transform.SetParent(GameObject.Find("Map").GetComponent<Map>().find_hex_by_grid(path[i]));
            path_hex.name = "path";
        }
    }

    public void remove_path(Vector2 path)
    {
        Destroy(GameObject.Find("Map").GetComponent<Map>().find_hex_by_grid(path).Find("path").gameObject);
    }

    public void Update()
    {
        //Debug.Log(player_go.GetComponent<Player>().output_moves().Count);
        timer += Time.deltaTime;
        if (timer >= 0.2)
        {
            timer = 0;
            if (player_go.GetComponent<Player>().moves_count() > 0 && path.Count > 0)
            {
                if (player_go.GetComponent<Player>().get_is_moving())
                {
                    Vector2 next_move = player_go.GetComponent<Player>().make_move();
                    player_go.GetComponent<Player>().move_to(next_move);
                    //Debug.Log(player_go.GetComponent<Player>().get_position());
                    remove_path(next_move);
                    path.RemoveAt(0);
                    update_pos(player_go);
                }
            }
            else if (path.Count == 0 && player_go.GetComponent<Player>().get_is_moving())
            {
                player_go.GetComponent<Player>().invert_move();
            }
        }
        
        
        if (Input.GetKeyDown("right"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().get_position() + new Vector2(1,0));
            update_pos(player_go);
        }
        
        if (Input.GetKeyDown("left"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().get_position() + new Vector2(-1, 0));
            update_pos(player_go);
        }
        if (Input.GetKeyDown("up"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().get_position() + new Vector2(0, 1));
            update_pos(player_go);
        }
        if (Input.GetKeyDown("down"))
        {
            player_go.GetComponent<Player>().move_to(player_go.GetComponent<Player>().get_position() + new Vector2(0, -1));
            update_pos(player_go);
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(player_go.GetComponent<Player>().get_is_moving());
            Transform where_to = get_mouse_hit();
            if (where_to != null)
            {
                if (!player_go.GetComponent<Player>().get_is_moving() && path.Count == 0)
                {
                    Vector2 hex_pos = get_position_object(where_to);
                    //Vector2 pos = player_go.GetComponent<Player>().get_position();
                    path = pathFinder.GetComponent<PathFinder>().find_path(player_go.GetComponent<Player>().get_position(), hex_pos);
                    build_path(path);
                    //player_go.GetComponent<Player>().add_moves(path);               
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log(player_go.GetComponent<Player>().get_is_moving());
            if (!player_go.GetComponent<Player>().get_is_moving())
            {
                player_go.GetComponent<Player>().invert_move();
                player_go.GetComponent<Player>().add_moves(path);
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