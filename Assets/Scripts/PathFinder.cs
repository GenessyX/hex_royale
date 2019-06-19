using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map");
    }

    class PriorityVector
    {
        public Vector2 grid_pos;
        public float cost;
        public PriorityVector came_from;
        public PriorityVector(Vector2 grid_pos, float cost, PriorityVector came_from)
        {
            this.grid_pos = grid_pos;
            this.cost = cost;
            this.came_from = came_from;
        }
    }

    public List<Vector2> find_neighbours(Vector2 hex)
    {
        List<Vector2> neighbours = new List<Vector2>();
        neighbours.Add(new Vector2(hex.x + 1, hex.y    ));
        neighbours.Add(new Vector2(hex.x - 1, hex.y    ));
        neighbours.Add(new Vector2(hex.x + 1, hex.y - 1));
        neighbours.Add(new Vector2(hex.x - 1, hex.y + 1));
        neighbours.Add(new Vector2(hex.x    , hex.y + 1));
        neighbours.Add(new Vector2(hex.x    , hex.y - 1));
        return neighbours;
    }

    public float heuristic(Vector2 A, Vector2 B)
    {
        return (Math.Abs(A.x - B.x) + Math.Abs(A.x + A.y - B.x - B.y) + Math.Abs(A.y - B.y)) / 2;
    }

    public List<Vector2> find_path(Vector2 A, Vector2 B)
    {
        PriorityVector current = new PriorityVector(A, 0, null);
        List<PriorityVector> to_open = new List<PriorityVector>();
        List<Vector2> to_openV = new List<Vector2>();
        to_open.Add(current);
        List<Vector2> opened = new List<Vector2>();
        while (to_open.Count != 0)
        {
            to_open = to_open.OrderBy(to => to.cost + heuristic(to.grid_pos, B)).ToList();
            current = to_open[0];
            if (current.grid_pos == B)
            {
                break;
            }
            to_open.RemoveAt(0);
            opened.Add(current.grid_pos);
            List<Vector2> neighbours = find_neighbours(current.grid_pos);

            foreach (Vector2 next in neighbours)
            {
                if (!opened.Contains(next) && !to_openV.Contains(next))
                {
                    to_open.Add(new PriorityVector(next, current.cost + 1f, current));
                    to_openV.Add(next);
                }
            }
        }
        List<Vector2> path = new List<Vector2>();
        path.Add(B);
        while (current.came_from != null)
        {
            path.Add(current.came_from.grid_pos);
            current = current.came_from;
        }
        //path.Add(current.grid_pos);
        
        Debug.Log(111111111111111);
        Debug.Log(B);
        path.Reverse();
        current = null;
        to_open = null;
        opened = null;
        return path;
    }

    
}
