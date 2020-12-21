using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Databox;

public class Room : MonoBehaviour
{
    Vector2Int size;
    Vector2Int dungeonPosition;
    RoomTileset tileset;

    GameObject topWall;
    GameObject topDoorSection;

    GameObject leftWall;
    GameObject leftDoorSection;

    GameObject bottomWall;
    GameObject bottomDoorSection;

    GameObject rightWall;
    GameObject rightDoorSection;

    List<RoomConnectionInfo> connections;
    List<Door> theDoors;

    GameObject roomLoot;
 
    public UnityEvent OnEnter {
        get;
        set;
    }

    public UnityEvent OnExit
    {
        get;
        set;
    }

    public List<GameObject> monsters;
    private void Awake()
    {
        Type = RoomTypes.Unassigned;
        OnEnter = new UnityEvent();
        OnExit = new UnityEvent();
        theDoors = new List<Door>();
        connections = new List<RoomConnectionInfo>();
    }

    public void AddConnection(RoomConnectionInfo direction)
    {
        if (!HasConnectionOn(direction))
        {
            connections.Add(direction);
        }
    }

    bool HasConnectionOn(RoomConnectionInfo direction)
    {
        return connections.Contains(direction);
    }

    bool HasDoorOn(Directions direction)
    {
        foreach(RoomConnectionInfo info in connections)
        {
            if (info.direction == direction)
            {
                return true;
            }
        }

        return false;
    }

    public void Generate(Vector2Int roomSize, RoomTileset tiles)
    {
        size = roomSize;
        tileset = tiles;

        float halfWidth = (size.x - 1) * 1.5f;
        float halfHeight = (size.y - 1) * 1.5f;

        topWall = new GameObject("Top Wall");
        topWall.transform.SetParent(transform);
        topWall.transform.localPosition = new Vector3(0, halfHeight + 2.5f);

        bottomWall = new GameObject("Bottom Wall");
        bottomWall.transform.SetParent(transform);
        bottomWall.transform.localPosition = new Vector3(0, -(halfHeight + 2.5f));

        for (int i = 0; i < size.x; i++)
        {
            GameObject wally = Instantiate(tileset.topWall);
            wally.transform.SetParent(topWall.transform);
            wally.transform.localPosition = Vector3.right * ((i * 3) - halfWidth);
            if (i == size.x / 2)
            {
                topDoorSection = wally;
            }

            wally = Instantiate(tileset.bottomWall);
            wally.transform.SetParent(bottomWall.transform);
            wally.transform.localPosition = Vector3.right * ((i * 3) - halfWidth);

            if (i == size.x / 2)
            {
                bottomDoorSection = wally;
            }

        }

        leftWall = new GameObject("Left Wall");
        leftWall.transform.SetParent(transform);
        leftWall.transform.localPosition = new Vector3(-halfWidth - 2.5f, 0);

        rightWall = new GameObject("Right Wall");
        rightWall.transform.SetParent(transform);
        rightWall.transform.localPosition = new Vector3(halfWidth + 2.5f, 0); ;

        for (int i = 0; i < size.y; i++)
        {
            GameObject wally = Instantiate(tileset.leftWall);
            wally.transform.SetParent(leftWall.transform);
            wally.transform.localPosition = Vector3.up * ((i * 3) - halfHeight);

            if (i == size.y / 2)
            {
                leftDoorSection = wally;
            }


            wally = Instantiate(tileset.rightWall);
            wally.transform.SetParent(rightWall.transform);
            wally.transform.localPosition = Vector3.up * ((i * 3) - halfHeight);

            if (i == size.y / 2)
            {
                rightDoorSection = wally;
            }
        }

        GameObject corner = Instantiate(tileset.topLeftCorner);
        corner.transform.SetParent(transform);
        corner.transform.localPosition = new Vector3(-halfWidth - 2.5f, halfHeight + 2.5f);

        corner = Instantiate(tileset.topRightCorner);
        corner.transform.SetParent(transform);
        corner.transform.localPosition = new Vector3(halfWidth + 2.5f, halfHeight + 2.5f);

        corner = Instantiate(tileset.botLeftCorner);
        corner.transform.SetParent(transform);
        corner.transform.localPosition = new Vector3(-halfWidth - 2.5f, -(halfHeight + 2.5f));

        corner = Instantiate(tileset.botRightCorner);
        corner.transform.SetParent(transform);
        corner.transform.localPosition = new Vector3((halfWidth + 2.5f), -(halfHeight + 2.5f));

        GameObject floor = new GameObject("Floor");
        floor.transform.SetParent(transform);
        floor.transform.localPosition = Vector3.zero;

        GameObject[,] myTiles = new GameObject[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameObject tile = Instantiate(tileset.floorTile);
                tile.transform.SetParent(floor.transform);
                tile.transform.localPosition = new Vector3(x * 3 - halfWidth, y * 3 - halfHeight);
                myTiles[x, y] = tile;
            }
        }



        
        if (Type == RoomTypes.Boss)
        {
            name = "Boss Room";

            roomLoot = tileset.heartContainer;
            GameObject boss = Instantiate(tileset.bosses.RandomElement());
            boss.transform.SetParent(transform);
            boss.transform.localPosition = new Vector3(0, 0, -2);

            boss.GetComponent<DamageTaker>().OnHealthChanged.AddListener((int hp) =>
            {
                if (hp <= 0)
                {
                    OpenDoors();
                    monsters.Remove(boss);
                }
            });

            monsters = new List<GameObject>() { boss };
            OnEnter.AddListener(RevealMonsters);
            OnExit.AddListener(HideMonsters);
            HideMonsters();
            
        }
        else if (Type == RoomTypes.Healing)
        {
            name = "Healing Spring";
            GameObject spring = null;//GameManager.Instance.ThePool.GetPooledObject(tileset.spring.name);
            spring.transform.SetParent(floor.transform);
            spring.transform.localPosition = Vector3.zero.SetZ(-1);
        } else if (Type == RoomTypes.Shop)
        {
            name = "Shop";
            GameObject carpet = Instantiate(tileset.storeCarpet);
            carpet.transform.SetParent(floor.transform);
            carpet.transform.localPosition = Vector3.zero.SetZ(-1);

            
            carpet = Instantiate(tileset.shopkeep);
            carpet.transform.SetParent(floor.transform);
            carpet.transform.localPosition = Vector3.zero.SetZ(-2);
            List<ItemInfo> items = DungeonGenerator.Instance.StoreItems;

            for (int i = 0; i < 4; i++)
            {
                ShopCounter counter = Instantiate(tileset.storeCounter).GetComponent<ShopCounter>();
                counter.transform.SetParent(floor.transform);
                counter.transform.localPosition = (DirectionHandler.ConvertDirectionToVector3((Directions)i) * 2.5f).SetZ(-1);
                if (i % 2 == 1)
                {
                    counter.transform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
                }

                counter.SetItem(items.RandomElement());
              
            }

        } else if (Type == RoomTypes.Miniboss)
        {
            name = "Miniboss";

            GameObject boss = Instantiate(tileset.minibosses.RandomElement());
            boss.transform.SetParent(transform);
            boss.transform.localPosition = new Vector3(0, 0, -2);

            TreasureChest chest = Instantiate(tileset.treasureChest).GetComponent<TreasureChest>();
            chest.transform.SetParent(transform);
            chest.transform.localPosition = -Vector3.forward;
            chest.contents = DungeonGenerator.Instance.Treasures.RandomElement();
            chest.gameObject.SetActive(false);
            boss.GetComponent<DamageTaker>().OnHealthChanged.AddListener((int hp) =>
            {
                if (hp <= 0)
                {
                    //Create treasure chest and put treasure inside of it
                    monsters.Remove(boss);
                    chest.gameObject.SetActive(true);

                    foreach(ShortLivedObject slo in GetComponentsInChildren<ShortLivedObject>())
                    {
                        Destroy(slo.gameObject);
                    }

                    OpenDoors();
                    OnEnter.RemoveListener(RevealMonsters);
                }
            });

            monsters = new List<GameObject>() { boss };
            OnEnter.AddListener(RevealMonsters);
            OnExit.AddListener(HideMonsters);
            HideMonsters();
        }
        else if (Type == RoomTypes.Basic)
        {
            

            int randy = Random.Range(0, 5);
            
            if (randy == 4)
            {
                Type = RoomTypes.Goddess;
                string color = "Red";
                randy = Random.Range(0, 3);
                if (randy == 1)
                {
                    color = "Green";
                } else
                {
                    color = "Blue";
                }
                GameObject coinPrefab = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Epic", "Goddess Coins", color);
                GameObject coin = Instantiate(coinPrefab);
                coin.transform.SetParent(transform);
                coin.transform.localPosition = -Vector3.forward;
                name = color + "Goddess Room";
                OnEnter.AddListener(CloseDoors);
            }
            else if (randy >= 2)
            {
                //Spawn Enemies
                Type = RoomTypes.Encounter;
                SpawnMonsters();
                roomLoot = (Random.value >= 0.5f) ? DungeonGenerator.Instance.RoomLoot.RandomElement().item : null;
                //HideMonsters();

               
            } else
            {
                OnEnter.AddListener(OpenDoors);

                int wetness = Random.Range(0, 5);
                if (wetness == 3)
                {
                    for (int x = size.x / 2 - 1; x <= size.x / 2 + 1; x++)
                    {
                        for (int y = size.y / 2 - 1; y <= size.y / 2 + 1; y++)
                        {
                            Destroy(myTiles[x, y]);
                            GameObject tile = Instantiate(tileset.waterTile);
                            tile.transform.SetParent(floor.transform);
                            tile.transform.localPosition = new Vector3(x * 3 - halfWidth, y * 3 - halfHeight);
                            myTiles[x, y] = tile;
                        }
                    }
                }
                else if (wetness > 3)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        if (y != size.y / 2 || !HasDoorOn(Directions.Left))
                        {
                            Destroy(myTiles[0, y]);
                            GameObject tile = Instantiate(tileset.waterTile);
                            tile.transform.SetParent(floor.transform);
                            tile.transform.localPosition = new Vector3(-halfWidth, y * 3 - halfHeight);
                            myTiles[0, y] = tile;
                        }
                        if (y != size.y / 2 || !HasDoorOn(Directions.Right))
                        {
                            Destroy(myTiles[size.x - 1, y]);
                            GameObject tile = Instantiate(tileset.waterTile);
                            tile.transform.SetParent(floor.transform);
                            tile.transform.localPosition = new Vector3((size.x - 1) * 3 - halfWidth, y * 3 - halfHeight);
                            myTiles[size.x - 1, y] = tile;
                        }

                    }

                    for (int x = 0; x < size.x; x++)
                    {
                        if (x != size.x / 2 || !HasDoorOn(Directions.Up))
                        {
                            Destroy(myTiles[x, 0]);
                            GameObject tile = Instantiate(tileset.waterTile);
                            tile.transform.SetParent(floor.transform);
                            tile.transform.localPosition = new Vector3(x * 3 - halfWidth, -halfHeight);
                            myTiles[x, 0] = tile;
                        }
                        if (x != size.x / 2 || !HasDoorOn(Directions.Down))
                        {
                            Destroy(myTiles[x, size.y - 1]);
                            GameObject tile = Instantiate(tileset.waterTile);
                            tile.transform.SetParent(floor.transform);
                            tile.transform.localPosition = new Vector3(x * 3 - halfWidth, (size.y - 1) * 3 - halfHeight);
                            myTiles[x, size.y - 1] = tile;
                        }

                    }
                }
            }
           
        } else
        {
            OnEnter.AddListener(OpenDoors);
        }

        foreach (RoomConnectionInfo direction in connections)
        {
            AddDoor(direction);
        }
    }

    private void OnDestroy()
    {
        OnEnter.RemoveAllListeners();
        OnExit.RemoveAllListeners();
    }

    public void SpawnMonsters()
    {
        monsters = new List<GameObject>();
        int numEnemies = Random.Range(3, 6);
        GameObject enemy = tileset.monsters.RandomElement();
        for (int i = 0; i < numEnemies; i++)
        {
            GameObject mon = Instantiate(enemy);
            mon.transform.SetParent(transform);
            mon.transform.localPosition = new Vector3(Random.Range(-10, 10), Random.Range(-5, 5), -2);
            mon.GetComponent<DamageTaker>().OnHealthChanged.AddListener((int hp) =>
            {
                if (hp <= 0)
                {
                    monsters.Remove(mon);

                    if (monsters.Count == 0)
                    {
                        OnEnter.RemoveAllListeners();
                        OpenDoors();
                    }
                }
            });
            monsters.Add(mon);
        }

        if (!IsFight())
        {
            CloseDoors();
        } else
        {
            HideMonsters();
            OnEnter.AddListener(RevealMonsters);
            OnExit.AddListener(HideMonsters);
        }
    }

    bool IsFight()
    {
        return Type == RoomTypes.Encounter || Type == RoomTypes.Boss || Type == RoomTypes.Miniboss;
    }
    void RevealMonsters()
    { 
        CloseDoors();
           
        foreach (GameObject mon in monsters)
        {
            mon.SetActive(true);
        }
        
    }

    void OpenDoors()
    {
        AudioManager.Instance.PlayClip("Door", "Open");
        foreach (Door door in theDoors)
        {
            door.IsOpen = true;
        }

        if (roomLoot)
        {
            GameObject heart = Instantiate(roomLoot);
            heart.transform.parent = transform;
            heart.transform.localPosition = new Vector3(0, 0, -2);
        }
        
    }
    
    void CloseDoors()
    {
        AudioManager.Instance.PlayClip("Door", "Close");
        foreach (Door door in theDoors)
        {
            door.IsOpen = false;
        }
        
    }
    void HideMonsters()
    {
        foreach (GameObject mon in monsters)
        {
            mon.SetActive(false);
        }
    }
    public void AddDoor(RoomConnectionInfo rci)
    {
        switch (rci.direction)
        {
            case Directions.Up:
                topDoorSection = CreateDoor(topWall, topDoorSection, tileset.topDoor, rci.isMainPath);
                break;
            case Directions.Left:
                leftDoorSection = CreateDoor(leftWall, leftDoorSection, tileset.leftDoor, rci.isMainPath);
                break;
            case Directions.Down:
                bottomDoorSection = CreateDoor(bottomWall, bottomDoorSection, tileset.bottomDoor, rci.isMainPath);
                break;
            case Directions.Right:
                rightDoorSection = CreateDoor(rightWall, rightDoorSection, tileset.rightDoor, rci.isMainPath);
                break;
        }
    }

    public void SetRoomLoot(GameObject loot)
    {
        if (IsFight())
        {
            roomLoot = loot;
        }
    }

    GameObject CreateDoor(GameObject wall, GameObject oldSection, GameObject door, bool isMainPath)
    {
        Door newDoor = Instantiate(door).GetComponent<Door>();
        newDoor.transform.SetParent(wall.transform);
        newDoor.transform.position = oldSection.transform.position;
        newDoor.room = this;
        newDoor.IsLocked = !isMainPath && Random.value < 0.66f;
        oldSection.SetActive(false);
        theDoors.Add(newDoor);
        return newDoor.gameObject;
    }

    public void ResetRoom()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public Vector2Int DungeonPosition
    {
        get {
            return dungeonPosition;
        }
        set
        {
            dungeonPosition = value;
            transform.localPosition = new Vector3(value.x * 30.5f, value.y * 19);
        }
    }

    public RoomTypes Type {
        get;
        set;
    }

}

public enum RoomTypes
{
    Unassigned,
    Basic,
    Entrance,
    Boss,
    Miniboss,
    Secret,
    Healing,
    Shop,
    Goddess,
    Encounter
}
