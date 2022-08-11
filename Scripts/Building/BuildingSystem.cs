using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    Grid grid;
    public static BuildingSystem instance; // Singleton for building System
    void Awake()
    {
        instance = this;
        grid = gridLayout.GetComponent<Grid>();
    }
    [SerializeField] PoolManager poolManager; // Instance of Pool Manager
    public GridLayout gridLayout;

    [SerializeField]
    GameObject buildingUi;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] Tilemap mainTimeMap;
    [SerializeField] TileBase tile;

    public GameObject prefab1; // Tavern
    public GameObject prefab2; // Bakery
    [SerializeField] GameManager gameManager;// Instance of Game Manager
    [SerializeField] List<GameObject> buildings = new List<GameObject>(); // List of Buildings
    PlacableObject objectToPlace; 
    /* Building can only be placed if there are no other buildings in 3 unit radius of where you want to place te building 
     Maximum number of buidings is set 5.*/
    [SerializeField] int placableDistance = 3, maxBuildings=5, wood=0, woodForTavern=100, woodForBakery=150;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        buildingUi.SetActive(false);
        poolManager = PoolManager.instance;
    }
    private void Update()
    {
        buildings = GetAllBuildings();
        if (Input.GetKeyDown(KeyCode.C))
        {
            /*Used to Add Tavern. Wood Count required = 100. Displays a message if 
             you don't have enough wood*/
            if (buildings.Count >= maxBuildings)
            {
                message.text = "You have maxed out buildings";
                StartCoroutine(ResetMessage());
                return;
            }
            if (objectToPlace == null && HasWoodForTavern())
                InitializeWithObject(prefab1);
            else
            {
                message.text = "You Don't have enough wood";
                StartCoroutine(ResetMessage());
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            /* Used to Add Bakery. Wood Count required = 150. Displays a message if 
            you don't have enough wood*/
            if (buildings.Count >= maxBuildings)
            {
                message.text = "You have maxed out buildings";
                StartCoroutine(ResetMessage());
                return;
            }
            if (objectToPlace == null && HasWoodForBakery())
                InitializeWithObject(prefab2);
            else
            {
                message.text = "You Don't have enough wood";
                StartCoroutine(ResetMessage());
            }
        }
        if (!objectToPlace)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Used to Place a building
            if(CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                objectToPlace.GetComponentInChildren<DestroyAfter>().DestroyGameObject();
                Building building = objectToPlace.GetComponent<Building>();
                building.IsPlaced(true);
                building.SetReady();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.size);
                if(building.buildingType.Equals(BuildingType.Tavern))
                {
                    wood -= woodForTavern;
                    gameManager.SetPlayerWood(wood);
                }
                if (building.buildingType.Equals(BuildingType.Bakery))
                {
                    wood -= woodForBakery;
                    gameManager.SetPlayerWood(wood);
                }
                objectToPlace = null;
            }
            else
            {
                Destroy(objectToPlace.gameObject);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            objectToPlace.GetComponent<Building>().ResetPlacedandReady();
            Destroy(objectToPlace.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            objectToPlace.Rotate();
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlacableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    public void Construction()
    {
        buildingUi.SetActive(true);
    }
    static TileBase[] GetTileBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }
    bool CanBePlaced(PlacableObject placableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placableObject.size;
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);
        TileBase[] baseArray = GetTileBlock(area, mainTimeMap);

        foreach(var b in baseArray)
        {
            if(b==tile)
            {
                return false;
            }
        }

        /* return true if there are no other buildings within 
        range =(default=3) otherwise displays an error message and returns false */
        GameObject nearestBuilding=FindNearestBuilding();
        if (nearestBuilding!=null && FindDistance(nearestBuilding) <= placableDistance)
        {
            message.text="Can't place here!";
            StartCoroutine(ResetMessage());
            return false;
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        mainTimeMap.BoxFill(start, tile, start.x, start.y, start.x + size.x, start.y + size.y);
    }
    public List<GameObject> GetAllBuildings()
    {
        // Return all Player Buildings
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject building in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (building.GetComponent<Building>() && building.GetComponent<Building>().IsPlaced() && !building.GetComponent<Health>().IsDead())
                temp.Add(building);
        }
        return temp;
    }
    private GameObject FindNearestBuilding()
    {
        /* Returns the nearest Building to where you are trying to place the new building 
         If this is > a default range. You can place the building. Otherwise you cannot.*/
        float minDist = Mathf.Infinity;
        GameObject nearestBuilding = null;
        foreach (GameObject building in buildings)
        {
            float dist = FindDistance(building);
            if (minDist > dist)
            {
                nearestBuilding = building;
                minDist = dist;
            }
        }
        return nearestBuilding;
    }
    private float FindDistance(GameObject targt)
    {
        // Returns distance between 2 points
        return Vector3.Distance(targt.transform.position, GetMouseWorldPosition());
    }
    bool HasWoodForTavern()
    {
        // returns true if wood count>100
        return wood >= woodForTavern;
    }
    bool HasWoodForBakery()
    {
        // returns true if wood count>150
        return wood >= woodForBakery;
    }
    public void UpdateWoodCount(int wood)
    {
        this.wood = wood;
    }
    IEnumerator ResetMessage()
    {
        yield return new WaitForSeconds(5f);
        message.text = "";
    }
}
