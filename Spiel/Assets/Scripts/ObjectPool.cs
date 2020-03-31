using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static string WALL = "Wall";
    public static string OBSTACLE = "Obstacle";
    public static string GROUND = "Ground";
   
    private Dictionary<string, List<GameObject>> prefabs;
    private Dictionary<string, List<GameObject>> lists;


    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject wallObstaclePrefab;


    [SerializeField]
    GameObject groundPrefab;
    [SerializeField]
    GameObject groundWithTreePrefab;


    public static ObjectPool current;
    private  List<GameObject> walls;
    private  List<GameObject> ground;
    private  List<GameObject> wallsObstacle;

    

    private void Awake()
    {
        ObjectPool.current = this;
        walls = new List<GameObject>();
        ground = new List<GameObject>();
        wallsObstacle = new List<GameObject>();

        prefabs = new Dictionary<string, List<GameObject>>();
        lists = new Dictionary<string, List<GameObject>>();

        List<GameObject> wallPrefabs = new List<GameObject>();
        List<GameObject> groundPrefabs = new List<GameObject>();
        List<GameObject> wallObstaclePrefabs = new List<GameObject>();

        wallPrefabs.Add(wallPrefab);
        groundPrefabs.Add(groundPrefab);
        groundPrefabs.Add(groundWithTreePrefab);
        wallObstaclePrefabs.Add(wallObstaclePrefab);

        prefabs.Add(ObjectPool.WALL, wallPrefabs);
        prefabs.Add(ObjectPool.GROUND, groundPrefabs);
        prefabs.Add(ObjectPool.OBSTACLE, wallObstaclePrefabs);

        lists.Add(ObjectPool.WALL, walls);
        lists.Add(ObjectPool.GROUND, ground);
        lists.Add(ObjectPool.OBSTACLE, wallsObstacle);
    }

    public GameObject CreateGameObject(string type, Vector3 position, Quaternion rotation, Transform parent)
    {
        
        if(lists[type].Count > 0)
        {
            GameObject newGameObject = lists[type][0];
            
            lists[type].Remove(newGameObject);
            newGameObject.transform.position = position;
            newGameObject.transform.rotation = rotation;
            newGameObject.transform.parent = parent;
            newGameObject.SetActive(true);

            return newGameObject;
        }
        
        int index = Random.Range(0 , prefabs[type].Count);
        return GameObject.Instantiate(prefabs[type][index] , position, rotation, parent);
    }

    public void RemoveGameobject(GameObject go)
    {
        go.transform.parent = transform;

        if(go.tag == ObjectPool.WALL)
        {
            go.SetActive(false);
            walls.Add(go);
        }else if(go.tag == ObjectPool.GROUND)
        {
            go.SetActive(false);
            ground.Add(go);
        }else if(go.tag == ObjectPool.OBSTACLE)
        {
            go.SetActive(false);
            wallsObstacle.Add(go);
        }        
        else     
        {
            Debug.Log("Unknown Object!");
        }
    }




}
