using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public Vector2 grid_position;
    public Vector3 world_position;
    public string name;

    public void init(Vector3 world_position, Vector2 grid_position, string name)
    {
        this.world_position = world_position;
        this.grid_position = grid_position;
        this.name = name;
    }

}


public class Map : MonoBehaviour
{
    public GameObject hex_prefab;
    public int worldScale;
    public int grid_width = 31;
    public int grid_height = 31; 

    double xOffset = 1.02f;// + 0.05);
    double zOffset = 0.882f;// + 0.05*(0.75));

    float xGenOffset = 0;
    float yGenOffset = 0;

    public float xGenOffsetSpeed = 0.001f;
    public float yGenOffsetSpeed = 0.001f;

    public Transform find_hex_by_grid(Vector2 grid_pos, Transform map)
    {
        Transform hex = map.Find(string.Format("Hexagon ({0}|{1})", grid_pos.x, grid_pos.y));
        return hex;
    }

    public Vector3 grid_to_world(Vector2 grid_pos, Transform map)
    {
        Transform hex = find_hex_by_grid(grid_pos, map);
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
        float seed = UnityEngine.Random.Range(0f, (float)Math.Pow(2, 15));
        float min_height = 10000;
        float world_scale = 50;
        Debug.Log(seed);
        Vector3 hex_scale = hex_prefab.transform.localScale;
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

        for (int i = 0; i < grid_height; i++)
        {
            if (start_col > 0)
                cols = grid_width - start_col;
            else if (start_col == 0)
                cols = grid_width - (i - grid_mid);
            for (int j = start_col; j < (start_col + cols); j++)
            {
                xPos = (grid_mid - i) * (xOffset) + (grid_mid - j) * (0.5 * xOffset);
                zPos = (grid_mid - j) * zOffset;
                GameObject hex_on_screen = Instantiate(hex_prefab, new Vector3((float)xPos, 0, (float)zPos), Quaternion.identity);
                float sample = generate_sample(i, j, seed, 0, 0, worldScale);

                float height = sample * world_scale;
                hex_on_screen.transform.localScale = new Vector3(1 ,height, 1);
                if (height < min_height)
                    min_height = height;
                hex_on_screen.transform.SetParent(this.transform);
                hex_on_screen.AddComponent<Hex>();
                hex_on_screen.GetComponent<Hex>().init(new Vector3((float)xPos, 0, (float)zPos), new Vector2(i, j), String.Format("Hexagon ({0}|{1})", i, j));
                hex_on_screen.name = hex_on_screen.GetComponent<Hex>().name;
            }
            if (start_col > 0)
                start_col--;
        }
    }

    void Awake()
    {

        //Application.targetFrameRate = 10;
    }
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
                float sample = generate_sample(i, j, xGenOffset, yGenOffset, 2);
                float world_scale = 50;
                hex_on_screen.localScale = new Vector3(1, sample * world_scale, 1);
            }
            if (start_col > 0)
                start_col--;

        }
        xGenOffset += xGenOffsetSpeed;
        yGenOffset += yGenOffsetSpeed;
    }
    */
}
