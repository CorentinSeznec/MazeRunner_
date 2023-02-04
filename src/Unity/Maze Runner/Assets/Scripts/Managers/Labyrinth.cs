using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using UnityEngine.AI;

public enum structType{
    Wall,
    OptionalWall,
    Ground,
    Center,
    None
}

public class Node
{
    public Node()
    {
        Tags = new List<string>();
    }
    
    public string Name { get; set; }
    
    public void add_tag(string tag)
    {
        Tags.Add(tag);
    }
    
    public void remove_tag(string tag)
    {
        Tags.Remove(tag);
    }
    
    public bool has_tag(string tag)
    {
        return Tags.Contains(tag);
    }
    
    public List<string> Tags { get; }
}


public class Graph
{
    public Graph(List<Node> nodes)
    {
        Nodes = nodes;
        Edges = new Dictionary<Tuple<Node, Node>, int>();
    }
    
    public Graph()
    {
        Nodes = new List<Node>();
        Edges = new Dictionary<Tuple<Node, Node>, int>();
    }
    
    public void add_node(Node node)
    {
        Nodes.Add(node);
    }
    
    public void remove_node(Node node)
    {
        Nodes.Remove(node);
    }
    
    public void add_edge(Node node1, Node node2, int weight)
    {
        Edges.Add(new Tuple<Node, Node>(node1, node2), weight);
    }
    
    public void remove_edge(Node node1, Node node2)
    {
        if (Edges.ContainsKey(new Tuple<Node, Node>(node1, node2)))
        {
            Edges.Remove(new Tuple<Node, Node>(node1, node2));
        } else if (Edges.ContainsKey(new Tuple<Node, Node>(node2, node1)))
        {
            Edges.Remove(new Tuple<Node, Node>(node2, node1));
        } else
        {
            throw new Exception("Edge does not exist");
        }
    }
    
    public int edge_weight(Node node1, Node node2)
    {
        if (Edges.ContainsKey(new Tuple<Node, Node>(node1, node2)))
        {
            return Edges[new Tuple<Node, Node>(node1, node2)];
        } if (Edges.ContainsKey(new Tuple<Node, Node>(node2, node1)))
        {
            return Edges[new Tuple<Node, Node>(node2, node1)];
        }

        throw new Exception("Edge does not exist");
    }
    
    public bool is_edge(Node node1, Node node2)
    {
        return Edges.ContainsKey(new Tuple<Node, Node>(node1, node2)) || Edges.ContainsKey(new Tuple<Node, Node>(node2, node1));
    }

    public void set_weight(Node node1, Node node2, int weight)
    {
        if (Edges.ContainsKey(new Tuple<Node, Node>(node1, node2)))
        {
            Edges[new Tuple<Node, Node>(node1, node2)] = weight;
        } else if (Edges.ContainsKey(new Tuple<Node, Node>(node2, node1)))
        {
            Edges[new Tuple<Node, Node>(node2, node1)] = weight;
        } else
        {
            throw new Exception("Edge does not exist");
        }
    }

    public List<Node> Adjacent(Node node)
    {
        var result = new List<Node>();
        foreach (var edge in Edges)
        {
            if (edge.Key.Item1 == node)
            {
                result.Add(edge.Key.Item2);
            }
            if (edge.Key.Item2 == node)
            {
                result.Add(edge.Key.Item1);
            }
        }
        return result;
    }
    
    public List<Node> Nodes { get; }
    public Dictionary<Tuple<Node, Node>, int> Edges { get; }
    
    public virtual Graph Clone()
    {
        var result = new Graph();
        foreach (var node in Nodes)
        {
            result.add_node(node);
        }
        foreach (var edge in Edges)
        {
            result.add_edge(edge.Key.Item1, edge.Key.Item2, edge.Value);
        }
        return result;
    }
    
    public void clear_edges()
    {
        Edges.Clear();
    }
    
    public void clear_nodes()
    {
        Nodes.Clear();
    }

    public Graph prim_algorithm()
    {
        var result = new Graph();
        foreach(var node in Nodes)
        {
            result.add_node(node);
        }
        const int inf = int.MaxValue;
        var visited = new List<Node>();
        var father = new Dictionary<Node, Node>();
        var unseen = new Dictionary<Node, int>();
        var s = Nodes[0];
        foreach (var node in Nodes)
        {
            unseen.Add(node, inf);
            father.Add(node, node);
        }
        unseen[s] = 0;
        while (Nodes.Count > visited.Count)
        {
            var u = unseen.OrderBy(x => x.Value).First().Key;
            result.add_edge(u, father[u], 1);
            visited.Add(u);
            foreach (var v in Adjacent(u).Where(v => !visited.Contains(v) && unseen[v] > edge_weight(u, v)))
            {
                unseen[v] = edge_weight(u, v);
                father[v] = u;
            }
            unseen.Remove(u);
        }
        return result;
    }
}

public class Grid : Graph
{
    public Grid(int height, int width)
    {
        Height = height;
        Width = width;
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                add_node(new Node());
            }
        }

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                if (i>0)
                {
                    add_edge(get_node(i, j), get_node(i - 1, j), 1);
                }
                if (j>0)
                {
                    add_edge(get_node(i, j), get_node(i, j - 1), 1);
                }
            }
        }
    }
    
    public Node get_node(int i, int j)
    {
        return Nodes[i * Width + j];
    }
    
    public int Height { get; }
    
    public int Width { get; }

    public new Grid Clone()
    {
        var result = new Grid(Height, Width);
        foreach (var node in Nodes)
        {
            result.add_node(node);
        }
        foreach (var edge in Edges)
        {
            result.add_edge(edge.Key.Item1, edge.Key.Item2, edge.Value);
        }
        return result;
    }

    public List<List<Node>> StraightLines()
    {
        List<List<Node>> lines = new List<List<Node>>();
        var memory = new Dictionary<Node, List<List<Node>>>();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Node node = get_node(i, j);
                if (node.has_tag("center"))
                {
                    continue;
                }
                memory.Add(node, new List<List<Node>>());
                memory[node].Add(new List<Node> { node });
                memory[node].Add(new List<Node> { node });
                for (int k = 0; k < 4; k++)
                {
                    var di = 0;
                    var dj = 0;
                    switch (k)
                    {
                        case 0:
                            di = 1;
                            break;
                        case 1:
                            dj = 1;
                            break;
                        case 2:
                            di = -1;
                            break;
                        case 3:
                            dj = -1;
                            break;
                    }
                    if (!(0 <= i +di && i+di < Height && 0 <= j + dj && j + dj < Width))
                    {
                        continue;
                    }
                    Node node2 = get_node(i + di, j + dj);
                    if (node2.has_tag("center"))
                    {
                        continue;
                    }
                    if (!is_edge(node, node2))
                    {
                        continue;
                    }

                    if (memory.ContainsKey(node2))
                    {
                        if (di == 0)
                        {
                            memory[node2][1].Add(node);
                            memory[node][1] = memory[node2][1];
                        }
                        else
                        {
                            memory[node2][0].Add(node);
                            memory[node][0] = memory[node2][0];
                        }
                    }
                }

                if (memory[node][0].Count == 1)
                {
                    lines.Add(memory[node][0]);
                }
                if (memory[node][1].Count == 1)
                {
                    lines.Add(memory[node][1]);
                }
            }
        }
        return lines;
    }
    
    public int Distance(Node node1, Node node2)
    {
        if (node1 == node2)
        {
            return 0;
        }
        var openList = new List<Tuple<Node, Node>>();
        var closedList = new List<Tuple<Node, Node>>();
        openList.Add(new Tuple<Node, Node>(null, node1));
        while (openList.Count > 0)
        {
            var current = openList[0];
            openList.RemoveAt(0);
            closedList.Add(current);
            if (current.Item2 == node2)
            {
                break;
            }

            foreach (var neighboor in Adjacent(current.Item2))
            {
                if (closedList.Any(x => x.Item2 == neighboor))
                {
                    continue;
                }
                if (!openList.Any(x => x.Item2 == neighboor))
                {
                    openList.Add(new Tuple<Node, Node>(current.Item2, neighboor));
                }
            }
        }
        var path = new List<Node>();
        var current2 = closedList.Last();
        while (current2.Item1 != null)
        {
            path.Add(current2.Item2);
            current2 = closedList.First(x => x.Item2 == current2.Item1);
        }
        return path.Count;
    }

    public int[,] to_matrix()
    {
        var matrix = new int[Height*2+1, Width*2+1];
        for (var i = 0; i < Height*2+1; i++)
        {
            for (var j = 0; j < Width*2+1; j++)
            {
                matrix[i, j] = 0;
            }
        }
        for (var i = 0; i < Height*2-1; i++)
        {
            for (var j = 0; j < Width*2-1; j++)
            {
                if (i % 2 == 1 && !is_edge(get_node(i/2, j/2), get_node(i/2+1, j/2)))
                {
                    matrix[i+1, j+1] = 0;
                } 
                else if (j % 2 == 1 && !is_edge(get_node(i/2, j/2), get_node(i/2, j/2+1)))
                {
                    matrix[i+1, j+1] = 0;
                }
                else
                {
                    matrix[i + 1, j + 1] = 1;
                }
            }
        }
        for (var i = 0; i < Height*2-1; i++)
        {
            for (var j = 0; j < Width*2-1; j++)
            {
                if (i%2==1 && j%2==1 && check_adjacent_wall(matrix, i+1, j+1))
                {
                    matrix[i+1, j+1] = 0;
                }
                else if ((get_node(i/2, j/2).has_tag("center") || get_node((i+1)/2, (j+1)/2).has_tag("center")) && matrix[i+1, j+1] == 1)
                {
                    if (get_node((i+1)/2, (j+1)/2).has_tag("north door") && i%2 == 1)
                        matrix[i+1, j+1] = 4;
                    else if (get_node(i/2, j/2).has_tag("south door") && i%2 == 1)
                        matrix[i+1, j+1] = 4;
                    else if (get_node(i/2, j/2).has_tag("east door") && j%2 == 1)
                        matrix[i+1, j+1] = 4;
                    else if (get_node((i+1)/2, (j+1)/2).has_tag("west door") && j%2 == 1)
                        matrix[i+1, j+1] = 4;
                    else
                        matrix[i+1, j+1] = 2;
                    
                }
            }
        }
        return matrix;
    }

    public static bool check_adjacent_wall(int[,] matrix, int x, int y)
    {
        for (var i = -1; i < 2; i++)
        {
            for (var j = -1; j < 2; j++)
            {
                if (matrix[x+i, y+j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static int[,] make_labyrinth(int h, int w, int centerRadius, int optionalWalls)
    {
        var rnd = new System.Random();
        var grid = new Grid(h, w);
        var centerX = w / 2;
        var centerY = h / 2;
        if (centerRadius > 0)
        {
            if (h%2 == 0 || w%2 ==0)
            {
                throw new Exception("Center radius can only be used with odd height and width");
            }
            for (var i = centerY-centerRadius; i <= centerY+centerRadius; i++)
            {
                for (var j = centerX-centerRadius; j <= centerX+centerRadius; j++)
                {
                    grid.remove_edge(grid.get_node(i, j), grid.get_node(i+1, j));
                    grid.remove_edge(grid.get_node(i, j), grid.get_node(i, j+1));
                    grid.get_node(i, j).add_tag("center");
                    if (i==centerY-centerRadius)
                        grid.get_node(i,j).add_tag("north door");
                    else if (i==centerY+centerRadius)
                        grid.get_node(i,j).add_tag("south door");
                    else if (j==centerX-centerRadius)
                        grid.get_node(i,j).add_tag("west door");
                    else if (j==centerX+centerRadius)
                        grid.get_node(i,j).add_tag("east door");
                }
            }
        }
        Dictionary<Tuple<Node, Node>, int> temp = new Dictionary<Tuple<Node, Node>, int>(grid.Edges);
        foreach (var edge in temp)
        {
            grid.set_weight(edge.Key.Item1, edge.Key.Item2, rnd.Next());
        }

        var tree = grid.prim_algorithm();
        Dictionary<Tuple<Node, Node>, int> temp2 = new Dictionary<Tuple<Node, Node>, int>(grid.Edges);
        foreach (var edge in temp2)
        {
            if (!tree.is_edge(edge.Key.Item1, edge.Key.Item2))
            {
                grid.remove_edge(edge.Key.Item1, edge.Key.Item2);
            }
            else
            {
                grid.set_weight(edge.Key.Item1, edge.Key.Item2, 1);
            }
        }

        if (centerRadius > 0)
        {
            for (var i = centerY - centerRadius; i <= centerY + centerRadius; i++)
            {
                for (var j = centerX - centerRadius; j <= centerX + centerRadius; j++)
                {
                    grid.add_edge(grid.get_node(i, j), grid.get_node(i + 1, j), 1);
                    grid.add_edge(grid.get_node(i, j), grid.get_node(i, j + 1), 1);
                }
            }

            for (var i = centerY - centerRadius; i <= centerY + centerRadius; i++)
            {
                if (i == centerY) continue;
                if (grid.is_edge(grid.get_node(i, centerX - centerRadius),
                        grid.get_node(i, centerX - centerRadius - 1)))
                {
                    grid.remove_edge(grid.get_node(i, centerX - centerRadius), grid.get_node(i, centerX - centerRadius - 1));
                }
                if (grid.is_edge(grid.get_node(i, centerX + centerRadius),
                        grid.get_node(i, centerX + centerRadius + 1)))
                {
                    grid.remove_edge(grid.get_node(i, centerX + centerRadius), grid.get_node(i, centerX + centerRadius + 1));
                }
            }
            for (var j = centerX - centerRadius; j <= centerX + centerRadius; j++)
            {
                if (j == centerX) continue;
                if (grid.is_edge(grid.get_node(centerY - centerRadius, j),
                        grid.get_node(centerY - centerRadius - 1, j)))
                {
                    grid.remove_edge(grid.get_node(centerY - centerRadius, j), grid.get_node(centerY - centerRadius - 1, j));
                }
                if (grid.is_edge(grid.get_node(centerY + centerRadius, j),
                        grid.get_node(centerY + centerRadius + 1, j)))
                {
                    grid.remove_edge(grid.get_node(centerY + centerRadius, j), grid.get_node(centerY + centerRadius + 1, j));
                }
            }
        }
        var matrix = grid.to_matrix();
        if (optionalWalls <= 0) return matrix;
        {
            var cache = new List<Tuple<int, Tuple<int, int>, Tuple<int, int>>>();
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
                {
                    if (grid.get_node(i, j).has_tag("center")) continue;
                    if (i>0 && !grid.is_edge(grid.get_node(i, j), grid.get_node(i-1, j)) && !grid.get_node(i-1, j).has_tag("center"))
                    {
                        var distance = grid.Distance(grid.get_node(i, j), grid.get_node(i - 1, j));
                        Insort(cache, Tuple.Create(distance, Tuple.Create(i, j), Tuple.Create(i - 1, j)));
                    }
                    if (j>0 && !grid.is_edge(grid.get_node(i, j), grid.get_node(i, j-1)) && !grid.get_node(i, j-1).has_tag("center"))
                    {
                        var distance = grid.Distance(grid.get_node(i, j), grid.get_node(i, j - 1));
                        Insort(cache, Tuple.Create(distance, Tuple.Create(i, j), Tuple.Create(i, j - 1)));
                    }
                }
            }
            var count = 0;
            var first = true;
            while (count < optionalWalls)
            {
                var pop = cache.Last();
                cache.RemoveAt(cache.Count - 1);
                if (!first)
                {
                    var distance = grid.Distance(grid.get_node(pop.Item2.Item1, pop.Item2.Item2), grid.get_node(pop.Item3.Item1, pop.Item3.Item2));
                    if (distance < cache.Last().Item1)
                    {
                        Insort(cache, Tuple.Create(distance, Tuple.Create(pop.Item2.Item1, pop.Item2.Item2), Tuple.Create(pop.Item3.Item1, pop.Item3.Item2)));
                        continue;
                    }
                }
                else
                {
                    first = false;
                }
                grid.add_edge(grid.get_node(pop.Item2.Item1, pop.Item2.Item2), grid.get_node(pop.Item3.Item1, pop.Item3.Item2), 1);
                var x = pop.Item2.Item1 + pop.Item3.Item1;
                var y = pop.Item2.Item2 + pop.Item3.Item2;
                matrix[x+1, y+1] = 3;
                count += 1;
            }
        }

        return matrix;
    }

    private static void Insort(IList<Tuple<int, Tuple<int, int>, Tuple<int, int>>> list, Tuple<int, Tuple<int, int>, Tuple<int, int>> item)
    {
        var i = 0;
        while (i < list.Count && list[i].Item1 < item.Item1)
        {
            i++;
        }
        list.Insert(i, item);
    }
}

public class Labyrinth : MonoBehaviour
{

    [SerializeField] GameObject groundPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject centerPrefab;
    [SerializeField] GameObject optionalWall;

    [SerializeField] GameObject orbPiedestal;
    [SerializeField] GameObject orbStockers;

    GameObject[,] objects;
    
    GameObject grounds;
    GameObject walls;
    GameObject centers;
    GameObject optionalWalls;

    private List<GameObject> pedestals;

    private List<GameObject> itemBlocks;

    private float groundWidth;
    private float groundHeight;
    private float wallHeight;

    private int matrixWidth;
    private int matrixHeight;

    private int[,] labyrinth;
    // Start is called before the first frame update
    void Start()
    {
        itemBlocks = new List<GameObject>();
        labyrinth = Grid.make_labyrinth(19,19,2,19);

        grounds = new GameObject("Grounds");
        grounds.transform.parent = transform;
        walls = new GameObject("Walls");
        walls.transform.parent = transform;
        centers = new GameObject("Centers");
        centers.transform.parent = transform;
        optionalWalls= new GameObject("OptionalsWalls");
        optionalWalls.transform.parent = transform;
        pedestals = new List<GameObject>();
        
        CreateLabyrinth();
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public Vector3 TileToCoord(int x, int y)
    {
        float l = matrixWidth / 2 * groundWidth;
        float xx = x * groundWidth - l;
        float yy = y * groundWidth - l;
        return new Vector3(xx, -groundHeight, yy);
    }

    public Tuple<int,int> CoordToTile(Transform t)
    {
        Vector3 pos = t.position;
        return CoordToTile(pos);
    }

    public Tuple<int,int> CoordToTile(Vector3 pos)
    {
        float l = matrixWidth / 2 * groundWidth;

        int x = (int) ((l + pos.x) / groundWidth+.5);
        int y = (int) ((l + pos.z) / groundWidth+.5); 

        return new Tuple<int, int>(x, y);
    }

    public bool IsValidTile(int x, int y)
    {
        return x < matrixWidth && y < matrixHeight;
    }

    public bool IsCenterTile(int x, int y)
    {
        return labyrinth[x,y] == 2;
    }

    void Shuffle(int[] array)
    {
        Random random = new Random();
        int n = array.Count();
        while (n > 1)
        {
            n--;
            int i = random.Next(n + 1);
            int temp = array[i];
            array[i] = array[n];
            array[n] = temp;
        }
    }

    public bool SpawnRandomOrb()
    {
        int[] indexes = {0, 1, 2, 3};
        Shuffle(indexes);
        foreach(int i in indexes)
        {
            if(pedestals[i].GetComponent<InteractableOrbPiedestal>().SpawnOrb())
            {
                return true;
            }
        }
        return false;
    }

    List<GameObject> InitItemBlocks()
    {
        var ret = new List<GameObject>();
        for (int x = 0; x < matrixWidth; x++)
        {
            for (int y = 0; y < matrixHeight; y++)
            {
                if (IsEqualPrefab(objects[x, y], groundPrefab))
                {
                    ret.Add(objects[x, y]);
                }
            }
        }
        return ret;
    }
    
    void CreateLabyrinth()
    {
        groundWidth = groundPrefab.GetComponent<MeshRenderer>().bounds.size[0];
        wallHeight = wallPrefab.GetComponent<MeshRenderer>().bounds.size[1];
        groundHeight = groundPrefab.GetComponent<MeshRenderer>().bounds.size[1];

        matrixWidth = labyrinth.GetLength(0);
        matrixHeight = labyrinth.GetLength(1);

        objects = new GameObject[matrixWidth, matrixHeight];

        float l = matrixWidth / 2 * groundWidth;

        GameObject temp;
        for(int x = 0 ; x < matrixWidth ; x++)
        {
            for(int y = 0; y < matrixHeight ; y++)
            {
                switch (labyrinth[x, y])
                {
                    case 0:
                        temp = Instantiate(wallPrefab, new Vector3(x * groundWidth - l, wallHeight / 2 - 3 * groundHeight / 2, y * groundWidth -l), Quaternion.identity);
                        temp.transform.parent = walls.transform;
                        objects[x ,y] = temp;
                        break;
                    case 1:
                        temp = Instantiate(groundPrefab, TileToCoord(x, y), Quaternion.identity);
                        temp.transform.parent = grounds.transform;
                        objects[x ,y] = temp;
                        break;
                    case 2:
                        temp = Instantiate(centerPrefab, TileToCoord(x, y), Quaternion.identity);
                        temp.transform.parent = centers.transform;
                        objects[x ,y] = temp;
                        break;
                    case 3:
                        Quaternion rotation = labyrinth[x-1, y] == 0 ? Quaternion.identity : Quaternion.Euler(0,90,0);
                        temp = Instantiate(optionalWall, TileToCoord(x, y), rotation);
                        temp.transform.parent = optionalWalls.transform;
                        objects[x ,y] = temp;
                        break;
                    case 4: //doors
                        temp = Instantiate(centerPrefab, TileToCoord(x, y), Quaternion.identity);
                        temp.transform.parent = centers.transform;
                        objects[x ,y] = temp;
                        break;
                    default:
                        Debug.LogError("There is an incorrect integer in the matrix");
                        Application.Quit();
                        break;
                }
            }
        }

        // generate the center 
        int centerX = matrixWidth / 2;
        int centerY = matrixHeight / 2;
        Destroy(objects[centerX, centerY]);
        temp = Instantiate(orbStockers, TileToCoord(centerX, centerY), Quaternion.identity);
        temp.transform.parent = transform;
        objects[centerX, centerY] = temp;
        // generate the orb piedestals
        List<Tuple<int, int>> possiblePos = new List<Tuple<int, int>>();
        possiblePos.Add(new Tuple<int, int>(1, 1));
        possiblePos.Add(new Tuple<int, int>(1, matrixHeight - 2));
        possiblePos.Add(new Tuple<int, int>(matrixWidth - 2, 1));
        possiblePos.Add(new Tuple<int, int>(matrixWidth - 2, matrixHeight - 2));
        for(int i = 0 ; i < 4 ; i++)
        {
            int x = possiblePos[i].Item1;
            int y = possiblePos[i].Item2;
            Destroy(objects[x, y]);
            temp = Instantiate(orbPiedestal, TileToCoord(x, y), Quaternion.identity);
            temp.transform.parent = transform;
            pedestals.Add(temp.transform.GetChild(0).gameObject);
            objects[x, y] = temp;
        }
        for(int  i = 0 ; i < 3 ; i++)
        {
            SpawnRandomOrb();
        }

        itemBlocks = InitItemBlocks ();
    }

    public Tuple<int, int> GetCenterTile()
    {
        return new Tuple<int, int>(matrixWidth / 2, matrixHeight / 2);
    }

    bool IsEqualPrefab(GameObject obj, GameObject prefab)
    {
        return obj.name == string.Format("{0}(Clone)", prefab.name);
    }
    
    public structType WhatIs(int x, int y)
    {
        if(!IsValidTile(x, y))
        {
            return structType.None;
        }

        GameObject here = objects[x, y];
        if (IsEqualPrefab(here, groundPrefab))
            return structType.Ground;
        else if (IsEqualPrefab(here, wallPrefab))
            return structType.Wall;
        else if (IsEqualPrefab(here, centerPrefab))
            return structType.Center;
        else if (IsEqualPrefab(here, optionalWall))
            return structType.OptionalWall;
        else if (IsEqualPrefab(here, orbPiedestal))
            return structType.Ground;
        else if (IsEqualPrefab(here, orbStockers))
            return structType.Center;
        else
            return structType.None;
    }

    public GameObject GetObjectIn(int x, int y)
    {
        if(IsValidTile(x, y))
        {
            return objects[x, y];
        }
        else
        {
            return new GameObject("Dromadaire");
        }
    }

    public List<GameObject> GetItemBlocks()
    {
        return itemBlocks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
