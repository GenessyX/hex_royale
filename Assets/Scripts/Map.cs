using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public Vector2 grid_position;
    public Vector3 world_position;
    public string name;
    public string type;

    public void init(Vector3 world_position, Vector2 grid_position, string name, string type)
    {
        this.world_position = world_position;
        this.grid_position = grid_position;
        this.name = name;
        this.type = type;
    }

    void OnMouseEnter()
    {
        if (gameObject.transform.childCount == 0)
            gameObject.transform.localScale += new Vector3(0, 1, 0);
        if (gameObject.transform.childCount > 0)
            if (gameObject.transform.GetChild(0).gameObject.name == "path")
                gameObject.transform.localScale += new Vector3(0, 1, 0);
        Debug.Log(GameObject.Find("Map").GetComponent<Map>().grid_to_world(this.grid_position));
    }

    void OnMouseExit()
    {
        if (gameObject.transform.childCount == 0)
            gameObject.transform.localScale -= new Vector3(0, 1, 0);
        if (gameObject.transform.childCount > 0)
            if (gameObject.transform.GetChild(0).gameObject.name == "path")
                gameObject.transform.localScale -= new Vector3(0, 1, 0);
    }

}


public class Map : MonoBehaviour
{
    public GameObject land_prefab;
    public GameObject water_prefab;

    public GameObject tree_prefab1;
    public GameObject tree_prefab2;
    public GameObject tree_prefab3;
    public GameObject tree_prefab4;

    public List<string> water;
    public List<Transform> water_transform;
    public List<Transform> trees_transform;
    public float seed;
    public int childCount;

    public int worldScale;
    public int grid_width = 31;
    public int grid_height = 31;

    double xOffset = 1.02f;// + 0.05);
    double zOffset = 0.882f;// + 0.05*(0.75));

    public float xGenOffset = 0;
    public float yGenOffset = 0;

    public float xGenOffsetSpeed = 0.001f;
    public float yGenOffsetSpeed = 0.001f;

    public Transform find_hex_by_grid(Vector2 grid_pos)
    {
        Transform hex = GameObject.Find("Map").transform.Find(string.Format("Hexagon ({0}|{1})", grid_pos.x, grid_pos.y));
        if (hex == null)
            hex = GameObject.Find("Map").transform.Find(string.Format("Water ({0}|{1})", grid_pos.x, grid_pos.y));
        return hex;
    }

    public List<Transform> get_water(List<string> water, Transform map)
    {
        List<Transform> water_transform = new List<Transform>();
        for (int i = 0; i < water.Count; i++)
        {
            water_transform.Add(map.Find(water[i]));
        }
        return water_transform;
    }

    public Vector3 grid_to_world(Vector2 grid_pos)
    {
        Transform hex = find_hex_by_grid(grid_pos);
        return hex.GetComponent<Hex>().world_position;
    }

    // Start is called before the first frame update

    private float generate_sample(int x, int y, float seed, float xOffset, float yOffset ,float scale)
    {
        float iCoord = (xOffset + (float)seed + (float)x / grid_width) * scale;
        float jCoord = (yOffset + (float)seed + (float)y / grid_height) * scale;
        return Mathf.PerlinNoise(iCoord, jCoord);
    }

    void Start()
    {

        //UnityEngine.Random.InitState(System.Environment.TickCount);
        seed = UnityEngine.Random.Range(0f, (float)Math.Pow(2, 10));
        float min_height = 10000;
        float world_scale = 50;
        Debug.Log(seed);
        Vector3 hex_scale = land_prefab.transform.localScale;
        float hex_width = hex_scale.x;
        float hex_height = hex_scale.z;
        int grid_mid = (grid_width - 1) / 2;
        int start_row = 0;
        int start_col = grid_mid;
        int rows = grid_width - start_col;
        double xPos;
        double zPos;
        int this_col = start_col;
        int this_row = start_row;
        int cols = grid_width - start_col;
        water = new List<string>();

        for (int i = 0; i < grid_height; i++)
        {
            if (start_col > 0)
                cols = grid_width - start_col;
            else if (start_col == 0)
                cols = grid_width - (i - grid_mid);
            for (int j = start_col; j < (start_col + cols); j++)
            {
                //GameObject hex_on_screen;
                GameObject curr_prefab;
                string curr_name;
                string curr_type;
                xPos = (grid_mid - i) * (xOffset) + (grid_mid - j) * (0.5 * xOffset);
                zPos = (grid_mid - j) * zOffset;
                float sample = generate_sample(i, j, seed, 0, 0, worldScale);
                float height = sample * world_scale;
                if ((float)0.2 * (height - 1) <= 2)
                {
                    curr_prefab = water_prefab;
                    curr_name = String.Format("Water ({0}|{1})", i, j);
                    water.Add(curr_name);
                    curr_type = "Water";
                }
                else
                {
                    curr_prefab = land_prefab;
                    curr_name = String.Format("Hexagon ({0}|{1})", i, j);
                    curr_type = "Land";
                }
                GameObject hex_on_screen = Instantiate(curr_prefab, new Vector3((float)xPos, 0, (float)zPos), Quaternion.identity);
                hex_on_screen.transform.localScale = new Vector3(1 ,height, 1);
                hex_on_screen.transform.SetParent(this.transform);
                hex_on_screen.AddComponent<Hex>();
                hex_on_screen.GetComponent<Hex>().init(new Vector3((float)xPos, (float)0.2 * (sample * world_scale - 1), (float)zPos), new Vector2(i, j), curr_name, curr_type);
                hex_on_screen.name = hex_on_screen.GetComponent<Hex>().name;

                if (height < min_height)
                    min_height = height;

                float tree_gen = generate_sample(i, j, seed * 2, 0, 0, worldScale*5);

                if (tree_gen * 10 >= 5)
                {
                    if ((float)0.2 * (height - 1) > 2)
                    {
                        float tree = UnityEngine.Random.Range(0, 4);
                        GameObject tree_prefab = tree_prefab1;
                        float curr_height = 1.6f;
                        if (tree < 1)
                        {
                            tree_prefab = tree_prefab1;
                            curr_height = 1.6f;
                        }
                        else if (tree < 2)
                        {
                            tree_prefab = tree_prefab2;
                            curr_height = 1.4f;
                        }
                        else if (tree < 3)
                        {
                            tree_prefab = tree_prefab3;
                            curr_height = 1.6f;
                        }
                        else if (tree < 4)
                        {
                            tree_prefab = tree_prefab4;
                            curr_height = 1.4f;
                        }
                        GameObject tree_go = Instantiate(tree_prefab, new Vector3((float)xPos, hex_on_screen.GetComponent<Hex>().world_position.y + curr_height, (float)zPos), Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
                        tree_go.transform.SetParent(hex_on_screen.transform);
                        trees_transform.Add(tree_go.transform);
                    }
                }
            }
            if (start_col > 0)
                start_col--;
        }
        childCount = this.transform.childCount;
        GameObject.Find("Map").GetComponent<ChestSpawn>().Chest_Spawn();
        water_transform = get_water(water, GameObject.Find("Map").transform);

        //ChestSpawn.Chest();
    }

    void Awake()
    {

        //Application.targetFrameRate = 10;
    }
    /*
    void Update()
    {
        foreach (Transform tree_transform in trees_transform)
        {
            float sample = generate_sample((int)tree_transform.position.x, (int)tree_transform.position.y, seed, xGenOffset, yGenOffset, 1);
            tree_transform.rotation = Quaternion.Euler((sample - 0.5f) * 20, 0, (sample - 0.5f)* 50);
        }


        foreach (Transform water_hex in water_transform)
        {
            float sample = generate_sample((int)water_hex.GetComponent<Hex>().grid_position.x, (int)water_hex.GetComponent<Hex>().grid_position.y, seed, xGenOffset, yGenOffset, 5);
            float height = sample * worldScale * 4;
            //Debug.Log(heigth);
            if (height >= 11)
                height = 11;
            water_hex.localScale = new Vector3(water_hex.localScale.x, height, water_hex.localScale.z);
        }

        xGenOffset += xGenOffsetSpeed;
        yGenOffset += yGenOffsetSpeed;
    }
    */

    // Update is called once per frame
    /* Continuos generation
    void Update()
    {
        Vector3 hex_scale = hex_prefab.transform.localScale;
        float hex_width = hex_scale.x;
        float hex_height = hex_scale.z;
        int grid_mid = (grid_width - 1) / 2;
        int start_row = 0;
        int start_col = grid_mid;
        int rows = grid_width - start_col;
        int this_col = start_col;
        int this_row = start_row;
        int cols = grid_width - start_col;


        for (int i = 0; i < grid_height; i++)
        {
            if (start_col > 0)
                cols = grid_width - start_col;
            else if (start_col == 0)
                cols = grid_width - (i - grid_mid);
            for (int j = start_col; j < (start_col + cols); j++)
            {
                Transform hex_on_screen = find_hex_by_grid(new Vector2(i, j), this.transform);
                float sample = generate_sample(i, j, seed, xGenOffset, yGenOffset, 2);
                float world_scale = 50;
                hex_on_screen.localScale = new Vector3(1, sample * world_scale, 1);
                hex_on_screen.GetComponent<Hex>().world_position.y = (float)0.2 * (sample * world_scale - 1);
            }
            if (start_col > 0)
                start_col--;

        }
        xGenOffset += xGenOffsetSpeed;
        yGenOffset += yGenOffsetSpeed;
    }
    */
}
