using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Databox;

public class DungeonGenerator
{ 
    Vector2Int dungeonSize = new Vector2Int(9, 5);
    Room[,] rooms;
    GameObject dungeon = new GameObject("Dungeon");
    RoomTileset tileset;

    public static DungeonGenerator Instance
    {
        get;
        private set;
    }

    public List<ItemInfo> StoreItems
    {
        get;
        private set;
    }

    public List<ItemInfo> LootItems
    {
        get;
        private set;
    }

    public List<ItemInfo> RoomLoot
    {
        get;
        private set;
    }

    public List<Treasure> Treasures
    {
        get;
        private set;
    }
    public DungeonGenerator()
    {
        Instance = this;
        LoadTilesetFromDatabase();
        LoadItemsFromDatabase();
    }

    RoomTileset LoadTilesetFromDatabase()
    {
        tileset = new RoomTileset();

        tileset.botLeftCorner = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Bottom Left Corner");
        tileset.botRightCorner = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Bottom Right Corner");
        tileset.topLeftCorner = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Top Left Corner");
        tileset.topRightCorner = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Top Right Corner");

        tileset.topDoor = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Top Door");
        tileset.leftDoor = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Left Door");
        tileset.bottomDoor = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Bottom Door");
        tileset.rightDoor = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Right Door");

        tileset.topWall = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Top Wall");
        tileset.leftWall = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Left Wall");
        tileset.bottomWall = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Bottom Wall");
        tileset.rightWall = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Right Wall");
        tileset.floorTile = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Floor");
        tileset.waterTile = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Water");


        tileset.storeCarpet = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Shop", "Carpet");
        tileset.storeCounter = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Shop", "Table");
        tileset.shopkeep = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Shop", "Shopkeeper");
        tileset.heartContainer = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Epic", "Heart", "GameObject");
        tileset.treasureChest = DatabaseManager.Instance.GetGameObjectFromDataBase("Tilesets", "Basic", "Dungeon", "Treasure Chest");
        tileset.monsters = new List<GameObject>();
        foreach(KeyValuePair<string, DataboxObject.DatabaseEntry> kvp in DatabaseManager.Instance.GetEntriesFromTable("Monsters", "Basic"))
        {
            tileset.monsters.Add(DatabaseManager.Instance.GetGameObjectFromDataBase("Monsters", "Basic", kvp.Key, "Monster"));
        }

        tileset.minibosses = new List<GameObject>();
        foreach (KeyValuePair<string, DataboxObject.DatabaseEntry> kvp in DatabaseManager.Instance.GetEntriesFromTable("Monsters", "Minibosses"))
        {
            tileset.minibosses.Add(DatabaseManager.Instance.GetGameObjectFromDataBase("Monsters", "Minibosses", kvp.Key, "Monster"));
        }


        tileset.bosses = new List<GameObject>();
        foreach (KeyValuePair<string, DataboxObject.DatabaseEntry> kvp in DatabaseManager.Instance.GetEntriesFromTable("Monsters", "Bosses"))
        {
            tileset.bosses.Add(DatabaseManager.Instance.GetGameObjectFromDataBase("Monsters", "Bosses", kvp.Key, "Monster"));
        }

        return tileset;
    }

    void LoadItemsFromDatabase()
    {
        StoreItems = new List<ItemInfo>();
        LootItems = new List<ItemInfo>();
        RoomLoot = new List<ItemInfo>();
        Treasures = new List<Treasure>();

        Databox.Dictionary.OrderedDictionary<string, DataboxObject.DatabaseEntry> itemList = DatabaseManager.Instance.GetEntriesFromTable("Items", "Pickups");

        foreach (string key in itemList.Keys)
        {
            ItemInfo info = new ItemInfo();
            info.item = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Pickups", key, "GameObject");
            info.name = DatabaseManager.Instance.GetStringFromDatabase("Items", "Pickups", key, "Name");
            info.description = DatabaseManager.Instance.GetStringFromDatabase("Items", "Pickups", key, "Description");
            info.basePrice = DatabaseManager.Instance.GetIntFromDatabase("Items", "Pickups", key, "BasePrice");
            info.lootLevel = (EnemyLevel)DatabaseManager.Instance.GetIntFromDatabase("Items", "Pickups", key, "LootLevel");
            info.maxDropSize = DatabaseManager.Instance.GetIntFromDatabase("Items", "Pickups", key, "MaxDropSize");


            if (IsShopItem(key))
            {
                StoreItems.Add(info);
            }

            if (IsLootItem(key))
            {
                LootItems.Add(info);
            }

            if (IsRoomLoot(key))
            {
                RoomLoot.Add(info);
            }
        }

        itemList = DatabaseManager.Instance.GetEntriesFromTable("Items", "Treasures");

        foreach (string key in itemList.Keys)
        {
            Treasures.Add(DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Treasures", key, "GameObject").GetComponent<Treasure>());
        }
    }

    public List<ItemInfo> GetLootForLevel(EnemyLevel level)
    {
        List<ItemInfo> loot = new List<ItemInfo>();

        foreach(ItemInfo info in LootItems)
        {
            if (info.lootLevel <= level)
            {
                loot.Add(info);
            }
        }

        return loot;
    }

    public void ClearDungeon()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Room theRoom = rooms[x, y];
                if (theRoom)
                {
                    rooms[x, y] = null;
                    GameObject.Destroy(theRoom.gameObject);
                }
            }
        }

        GenerateDungeon();
    }
    public void GenerateDungeon()
    {
        RoomTileset current = tileset;

        rooms = new Room[5, 5];
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Room newRoom = new GameObject("Room").AddComponent<Room>();
                //newRoom.Generate(dungeonSize, current);
                newRoom.transform.SetParent(dungeon.transform);
                newRoom.name = string.Format("Room ({0}, {1})", x, y);
                newRoom.DungeonPosition = new Vector2Int(x, y);
                rooms[x, y] = newRoom;
            }
        }

        Vector2Int pos = new Vector2Int(Random.Range(0, 5), 0);
        Room entrance = GetRoomAt(pos);
        entrance.Type = RoomTypes.Entrance;
        //entrance.Generate(dungeonSize, current);
        int stepsTaken = 0;
        int minSteps = 5;
        int maxSteps = 12;
        List<Room> mainPath = new List<Room>();
        mainPath.Add(entrance);
        do
        {
            Room currentRoom = mainPath[mainPath.Count - 1];
            List<Room> possibleNeighbors = GetPossibleNeighbors(currentRoom);
            if (possibleNeighbors.Count == 0)
            {
                if (stepsTaken > minSteps)
                {
                    //Set the current room as the boss room and continue dungeon generation
                    break;
                }
                else
                {
                    //Recreate the basic path
                    break;
                }
            }

            
            Room nextRoom = possibleNeighbors.RandomElement();
            nextRoom.Type = RoomTypes.Basic;//nextRoom.Generate(dungeonSize, current);

            //pos = nextRoom.DungeonPosition;
            stepsTaken++;
            mainPath.Add(nextRoom);

            possibleNeighbors.Remove(nextRoom);
            if (possibleNeighbors.Count > 0 && Random.value > 0.5f)
            {
                for (int j = 0; j < Random.Range(1, possibleNeighbors.Count); j++)
                {
                    Room nextNeighbor = possibleNeighbors.RandomElement();
                    nextNeighbor.Type = RoomTypes.Basic;
                    possibleNeighbors.Remove(nextNeighbor);
                    ConnectRooms(currentRoom, nextNeighbor, false);
                }
            }
        } while (stepsTaken < maxSteps);

        mainPath[mainPath.Count - 1].Type = RoomTypes.Boss;
        mainPath[mainPath.Count - 2].Type = RoomTypes.Shop;
        int randi = Random.Range(2, mainPath.Count - 2);
        mainPath[randi].Type = RoomTypes.Miniboss;
        

        for (int i = 1; i < mainPath.Count; i++)
        {
            Room one = mainPath[i - 1];
            Room two = mainPath[i];
            ConnectRooms(one, two);
        }


        mainPath.Remove(entrance);
        mainPath.RemoveAt(mainPath.Count - 1);

        foreach (Room room in mainPath)
        {
            if (NumActiveRooms >= maxSteps)
            {
                break;
            }

            List<Room> neighbors = GetPossibleNeighbors(room);
            if (neighbors.Count > 0)
            {
                for (int i = 0; i < Random.Range(0, neighbors.Count + 1); i++)
                {
                    if (NumActiveRooms >= maxSteps)
                    {
                        break;
                    }
                    Room roger = neighbors.RandomElement();
                    roger.Type = RoomTypes.Basic;
                    ConnectRooms(room, roger);
                }
            }

        }

        GameManager.Instance.Player.transform.position = entrance.transform.position.SetZ(-2);
        CameraManager.Instance.CurrentRoom = null;
        CameraManager.Instance.CurrentRoom = entrance;

        foreach(Room theRoom in rooms)
        {
            if (theRoom.Type != RoomTypes.Unassigned)
            {
                theRoom.Generate(dungeonSize, current);
            }
        }
    }

    int NumActiveRooms
    {

        get
        {
            int activeRooms = 0;

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (rooms[x, y].Type != RoomTypes.Unassigned)
                    {
                        activeRooms += 1;
                    }
                }
            }

            return activeRooms;
        }
    }
    bool IsRoomAt(Vector2Int pos)
    {
        if (rooms != null && IsOnGrid(pos))
        {
            return rooms[pos.x, pos.y] != null;
        }

        return false;

    }

    Room GetRoomAt(Vector2Int pos)
    {
        if (rooms != null && IsOnGrid(pos))
        {
            return rooms[pos.x, pos.y];
        }

        return null;
    }

    bool IsOnGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 5 && pos.y >= 0 && pos.y < 5;
    }

    List<Room> GetPossibleNeighbors(Room room)
    {
        List<Room> possibleDirections = new List<Room>();
        for (int i = 0; i < 4; i++)
        {
            Vector2Int newPos = room.DungeonPosition + DirectionHandler.ConvertDirectionToVector2Int((Directions)i);

            if (IsRoomAt(newPos))
            {
                Room newRoom = GetRoomAt(newPos);
                if (newRoom.Type == RoomTypes.Unassigned)
                {
                    possibleDirections.Add(newRoom);
                }
            }


        }

        return possibleDirections;
    }

    void ConnectRooms(Room one, Room two, bool onMainPath = true)
    {
        Directions roomDirection = DirectionHandler.GetClosestDirection(two.DungeonPosition - one.DungeonPosition);
        RoomConnectionInfo rci = new RoomConnectionInfo(roomDirection, onMainPath);
        one.AddConnection(rci);
        rci = new RoomConnectionInfo(DirectionHandler.GetOppositeDirection(roomDirection), true);
        two.AddConnection(rci);
    }

    bool IsShopItem(string key)
    {
        return DatabaseManager.Instance.GetBoolFromDatabase("Items", "Pickups", key, "IsInShop");
    }

    bool IsLootItem(string key)
    {
        return DatabaseManager.Instance.GetBoolFromDatabase("Items", "Pickups", key, "IsLoot");
    }

    bool IsRoomLoot(string key)
    {
        return DatabaseManager.Instance.GetBoolFromDatabase("Items", "Pickups", key, "IsRoomLoot");
    }
}

public struct RoomTileset
{
    public GameObject topWall;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bottomWall;

    public GameObject topLeftCorner;
    public GameObject topRightCorner;
    public GameObject botLeftCorner;
    public GameObject botRightCorner;

    public GameObject topDoor;
    public GameObject leftDoor;
    public GameObject bottomDoor;
    public GameObject rightDoor;

    public GameObject floorTile;
    public GameObject waterTile;
    public GameObject spring;

    public GameObject storeCarpet;
    public GameObject storeCounter;
    public GameObject shopkeep;

    public List<GameObject> monsters;
    public List<GameObject> minibosses;
    public List<GameObject> bosses;

    public GameObject heartContainer;
    public GameObject treasureChest;
}

public struct ItemInfo
{
    public GameObject item;
    public string name;
    public string description;
    public int basePrice;
    public EnemyLevel lootLevel;
    public int maxDropSize;
}

public struct RoomConnectionInfo
{
    public Directions direction;
    public bool isMainPath;

    public RoomConnectionInfo(Directions d, bool b)
    {
        direction = d;
        isMainPath = b;
    }
}
