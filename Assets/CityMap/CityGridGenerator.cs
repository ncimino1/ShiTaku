using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CityMap.WaveFunctionCollapse;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Grid = CityMap.WaveFunctionCollapse.Grid;
using Random = UnityEngine.Random;

public class CityGridGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private float size;

    [SerializeField] private Tile tile;

    [SerializeField] private bool readFile = false;

    //Make it so all the tiles are generated as doors based on the DoorPrefab variable
    [SerializeField] public GameObject NPC_Character;

    [SerializeField] public GameObject Door;

    [SerializeField] public GameObject TavernInterior;

    [SerializeField] public GameObject Player;

    [SerializeField] public GameObject GameMenuCanvas;

    [SerializeField] public NPCMenu NPCMenu;

    private static Grid grid;

    private static Thread genThread;

    public static bool genDone = false;

    private static bool startGen = false;

    private Dictionary<TileTypes, List<Sprite>> _spriteAtlas;

    private Dictionary<TileTypes, Sprite> _interiorAtlas;

    private Dictionary<TileTypes, List<Sprite>> _npcAtlas;

    private HashSet<Dialouge> _buildingLines;

    private Dialouge _fireLines;

    private Dialouge _policeLines;

    private Dialouge _shrineLines;

    private Dialouge _hardwareLines;

    private Dialouge _cityLines;

    private string[] _names;

    private Sprite _grass;

    private Thread _thread;

    private HashSet<TileTypes> _needsGrass;

    private int _houseRebuild = 0;
    private int _houseEvac = 0;
    private int _skyBuild = 0;
    private int _skyEvac = 0;
    private int _shrineRebuild = 0;
    private int _shrineEvac = 0;
    private int _hardwareRebuild = 0;
    private int _hardwareEvac = 0;
    private int _cityhallRebuild = 0;
    private int _cityhallEvac = 0;
    private int _fireRebuild = 0;
    private int _fireEvac = 0;
    private int _policeRebuild = 0;
    private int _policeEvac = 0;

    private class TileParameters
    {
        public Color? Color;
        public TileTypes? Type;
        public bool Interior;
        public String DialogueFile;
        public bool Grass = false;
        public bool Destroyed = false;
        public int Index = -1;
    }

    private void threadWorker()
    {
        startGen = true;
        var config = TileConfiguration.Generate();
        grid = new Grid(width, height, config);
        while (!grid.Collapse())
        {
        }

        grid.FixDuplicates();
        grid.FixRoads();
        grid.FixSkyscrapers();
        grid.DestroyBuildings();

        startGen = false;
        genDone = true;
        
        Debug.Log("background thread done");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene" && !startGen)
        {
            Debug.Log("generating tiles");
            genDone = false;
            genThread = new Thread(threadWorker);
            genThread.Start();
        }
        
        else if (SceneManager.GetActiveScene().name == "Cityscape")
        {
            LoadSprites();
            if (readFile)
            {
                LoadTiles();
            }
            else
            {
                GenerateTiles();
            }
        }
    }

    void parseDialogue()
    {
        var empty = new string[]
        {
            "Hello!",
        };

        var rebuildStart = new string[]
        {
            "Do you want to repair this building?",
        };

        var rebuildDone = new string[]
        {
            "This building is already in stable condition.",
        };

        var file = Resources.Load<TextAsset>("Dialogue").text;
        string splitChar = "\n";

        if (file.Contains('\r'))
        {
            splitChar = "\r\n";
        }

        var dialogueArray = file.Split(splitChar).SkipLast(1).ToArray();

        Dialouge dialouge = new Dialouge();

        List<string> interact = new List<string>();
        List<string> exit = new List<string>();

        TileTypes? state = null;
        bool interactState = false;

        _buildingLines = new HashSet<Dialouge>();

        for (int i = 0; i < dialogueArray.Length + 1; i++)
        {
            var line = "";
            if (i < dialogueArray.Length)
                line = dialogueArray[i];
            if (TileTypes.TryParse(line, out TileTypes result) || i == dialogueArray.Length)
            {
                if (state.HasValue)
                {
                    if (exit.Count != 0)
                    {
                        dialouge.exitSentences = exit.ToArray();
                        exit.Clear();
                    }

                    if (state == TileTypes.House)
                    {
                        _buildingLines.Add(dialouge);
                    }
                    else if (state == TileTypes.Park)
                    {
                        _shrineLines = dialouge;
                    }
                    else if (state == TileTypes.CityHall)
                    {
                        _cityLines = dialouge;
                    }
                    else if (state == TileTypes.HardwareStore)
                    {
                        _hardwareLines = dialouge;
                    }
                    else if (state == TileTypes.FireStation)
                    {
                        _fireLines = dialouge;
                    }
                    else if (state == TileTypes.PoliceStation)
                    {
                        _policeLines = dialouge;
                    }
                }

                dialouge = new Dialouge();
                dialouge.emptySentences = empty;
                dialouge.rebuildSentences = rebuildStart;
                dialouge.rebuildDoneSetences = rebuildDone;
                state = result;
            }
            else if (line == "Interact:")
            {
                interactState = true;
            }
            else if (line == "Exit:")
            {
                dialouge.sentences = interact.ToArray();
                interact.Clear();
                interactState = false;
            }
            else
            {
                if (interactState)
                {
                    interact.Add(line);
                }
                else
                {
                    exit.Add(line);
                }
            }
        }
    }

    void LoadSprites()
    {
        var spritesArray = Resources.Load<SpriteAtlas>("Tiles/TileAtlas");
        _grass = Resources.LoadAll<Sprite>("Tiles/grass")[0];

        _spriteAtlas = new Dictionary<TileTypes, List<Sprite>>();

        _npcAtlas = new Dictionary<TileTypes, List<Sprite>>();

        foreach (var type in Enum.GetValues(typeof(TileTypes)).Cast<TileTypes>())
        {
            var sprite = spritesArray.GetSprite(type.ToString());

            if (sprite)
            {
                if (!_spriteAtlas.ContainsKey(type))
                {
                    _spriteAtlas[type] = new List<Sprite>();
                }

                _spriteAtlas[type].Add(sprite);
                if (type >= TileTypes.SkyscraperCornerBL && type <= TileTypes.SkyscraperUpperTR)
                {
                    _spriteAtlas[type].Add(spritesArray.GetSprite("Reskinned" + type.ToString()));
                }
            }
        }

        _spriteAtlas[TileTypes.House].Add(spritesArray.GetSprite("HouseReskinned"));
        _spriteAtlas[TileTypes.HouseDestroyed].Add(spritesArray.GetSprite("DestroyedHouseReskinned"));

        _needsGrass = new HashSet<TileTypes>();
        _needsGrass.Add(TileTypes.FireStation);
        _needsGrass.Add(TileTypes.FireStationDestroyed);
        _needsGrass.Add(TileTypes.PoliceStation);
        _needsGrass.Add(TileTypes.PoliceStationDestroyed);
        _needsGrass.Add(TileTypes.Park);
        _needsGrass.Add(TileTypes.ParkDestroyed);
        _needsGrass.Add(TileTypes.HardwareStore);
        _needsGrass.Add(TileTypes.HardwareStoreDestroyed);
        _needsGrass.Add(TileTypes.CityHall);
        _needsGrass.Add(TileTypes.CityHallDestroyed);

        _interiorAtlas = new Dictionary<TileTypes, Sprite>();

        var parkInterior = Resources.Load<Sprite>("shrine");
        var houseInterior = Resources.Load<Sprite>("HouseInterior");
        var skyscraperInterior = Resources.Load<Sprite>("SkyscraperInterior");
        var cityHallInterior = Resources.Load<Sprite>("CityHallInterior");
        var hardwareStoreInterior = Resources.Load<Sprite>("HardwareStoreInterior");
        var firehouseInterior = Resources.Load<Sprite>("firehouseinterior");
        var policeInterior = Resources.Load<Sprite>("policestationinterior");

        _interiorAtlas.Add(TileTypes.Park, parkInterior);
        _interiorAtlas.Add(TileTypes.ParkDestroyed, parkInterior);
        _interiorAtlas.Add(TileTypes.House, houseInterior);
        _interiorAtlas.Add(TileTypes.HouseDestroyed, houseInterior);
        _interiorAtlas.Add(TileTypes.SkyscraperCornerBL, skyscraperInterior);
        _interiorAtlas.Add(TileTypes.SkyscraperCornerBLDestroyed, skyscraperInterior);
        _interiorAtlas.Add(TileTypes.CityHall, cityHallInterior);
        _interiorAtlas.Add(TileTypes.CityHallDestroyed, cityHallInterior);
        _interiorAtlas.Add(TileTypes.HardwareStore, hardwareStoreInterior);
        _interiorAtlas.Add(TileTypes.HardwareStoreDestroyed, hardwareStoreInterior);
        _interiorAtlas.Add(TileTypes.FireStation, firehouseInterior);
        _interiorAtlas.Add(TileTypes.FireStationDestroyed, firehouseInterior);
        _interiorAtlas.Add(TileTypes.PoliceStation, policeInterior);
        _interiorAtlas.Add(TileTypes.PoliceStationDestroyed, policeInterior);

        var shrineWorker = Resources.Load<Sprite>("shrineWorker");
        var fireFighter = Resources.Load<Sprite>("fire_fighter");
        var grandma = Resources.Load<Sprite>("grandma");
        var girl = Resources.Load<Sprite>("little_girl");
        var hardwareWorker = Resources.Load<Sprite>("hardware_store_worker");
        var receptionist = Resources.Load<Sprite>("receptionist");
        var mayor = Resources.Load<Sprite>("mayor");
        var cop = Resources.Load<Sprite>("cop");

        _npcAtlas.Add(TileTypes.Park, new List<Sprite>() { shrineWorker });
        _npcAtlas.Add(TileTypes.ParkDestroyed, new List<Sprite>() { shrineWorker });
        _npcAtlas.Add(TileTypes.FireStation, new List<Sprite>() { fireFighter });
        _npcAtlas.Add(TileTypes.FireStationDestroyed, new List<Sprite>() { fireFighter });
        _npcAtlas.Add(TileTypes.House, new List<Sprite>() { grandma, girl });
        _npcAtlas.Add(TileTypes.HouseDestroyed, new List<Sprite>() { grandma, girl });
        _npcAtlas.Add(TileTypes.HardwareStore, new List<Sprite>() { hardwareWorker });
        _npcAtlas.Add(TileTypes.HardwareStoreDestroyed, new List<Sprite>() { hardwareWorker });
        _npcAtlas.Add(TileTypes.SkyscraperCornerBL, new List<Sprite>() { receptionist });
        _npcAtlas.Add(TileTypes.SkyscraperCornerBLDestroyed, new List<Sprite>() { receptionist });
        _npcAtlas.Add(TileTypes.CityHall, new List<Sprite>() { mayor });
        _npcAtlas.Add(TileTypes.CityHallDestroyed, new List<Sprite>() { mayor });
        _npcAtlas.Add(TileTypes.PoliceStation, new List<Sprite>() { cop });
        _npcAtlas.Add(TileTypes.PoliceStationDestroyed, new List<Sprite>() { cop });

        var file = Resources.Load<TextAsset>("Names");
        _names = file.text.Split('\n').SkipLast(1).ToArray();

        parseDialogue();
    }

    void LoadTiles()
    {
        CityReader reader = new CityReader();
        var tiles = reader.ReadCity();
        if (tiles is null)
        {
            readFile = false;
            GenerateTiles();
            return;
        }


        for (int x = 0; x < tiles.Width; x++)
        {
            for (int y = 0; y < tiles.Height; y++)
            {
                TileParameters parameters = new TileParameters();
                parameters.Color = tiles.Types[x + y * tiles.Width].GetColor();
                // parameters.Interior = tiles.Doors[x + y * tiles.Width];
                parameters.DialogueFile = tiles.Dialogue[x + y * tiles.Width];
                GenerateTile(x, y, parameters);
            }
        }
    }

    void LoadNpcs(string fileName, Transform parent)
    {
        var readText = Resources.Load<TextAsset>(fileName);
        var iterator = readText.text.Split('\n');
        for (int i = 0; i < iterator.Length; i++)
        {
            var quoteLoc = iterator[i].IndexOf('"', 1);
            var charName = iterator[i].Substring(1, quoteLoc - 1);

            var split = iterator[i].Substring(quoteLoc + 2).Split(" ");

            var textureName = split[0];
            var success = float.TryParse(split[1], out float x);
            success = float.TryParse(split[2], out float y);
            success = float.TryParse(split[3], out float xScale);
            success = float.TryParse(split[4], out float yScale);

            i++;

            success = int.TryParse(iterator[i++], out int numLines);
            string[] dialogue = new string[numLines];
            for (int j = 0; j < numLines; j++)
            {
                dialogue[j] = iterator[i];
                i++;
            }

            LoadNpc(parent, x, y, xScale, yScale, charName, textureName, dialogue);
        }
    }

    GameObject LoadNpc(Transform parent, float x, float y, float xScale, float yScale, string npcName, string texture,
        string[] dialogue)
    {
        var npc = Instantiate(NPC_Character, parent);

        var textureObj = Resources.Load<Sprite>(texture);
        var spriteRenderer = npc.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = textureObj;
        spriteRenderer.sortingLayerName = "NPC";

        var localPos = new Vector3(x, y);
        npc.transform.localPosition = localPos;
        npc.name = npcName;

        var scale = new Vector3(xScale, yScale);
        npc.transform.localScale = scale;

        var manager = npc.GetComponent<NpcManager>();
        manager.dialouge.sentences = dialogue;
        manager.dialouge.name = npcName;

        return npc;
    }

    Tile GenerateTile(int x, int y, TileParameters parameters)
    {
        var xPos = x * size;
        var yPos = y * size;
        var t = Instantiate(tile, transform);
        t.transform.localPosition = new Vector3(xPos, yPos);


        t.transform.localScale = new Vector3(size, size, size);

        var sprite = t.GetComponent<SpriteRenderer>();
        sprite.sortingLayerID = SortingLayer.NameToID("Building");
        if (parameters is null)
        {
            sprite.color = Random.ColorHSV(0, 1, 0, 1, 0.5f, 1);
            return t;
        }

        if (!parameters.Color.HasValue)
        {
            // sprite.color = Random.ColorHSV(0, 1, 0, 1, 0.5f, 1);
        }
        else
        {
            sprite.color = parameters.Color.Value;
        }

        var buildingIndex = 0;

        if (parameters.Type.HasValue && _spriteAtlas.TryGetValue(parameters.Type.Value, out var value))
        {
            buildingIndex = parameters.Index != -1 ? parameters.Index : Random.Range(0, value.Count);
            sprite.sprite = value[buildingIndex];
        }

        if (parameters.Grass)
        {
            var grass = Instantiate(tile, t.transform).GetComponent<SpriteRenderer>();
            grass.sortingLayerID = SortingLayer.NameToID("Background");
            grass.sprite = _grass;
        }

        if (parameters.Destroyed)
        {
            t.Destroyed = parameters.Destroyed;
            var fixedName = parameters.Type.Value.ToString().Replace("Destroyed", "");
            if (!TileTypes.TryParse(fixedName, out TileTypes result))
            {
                t.FixedAsset = _spriteAtlas[TileTypes.HouseDestroyed][0];
            }
            else
            {
                t.FixedAsset = _spriteAtlas[result][buildingIndex];
            }
        }

        //Make each tile have a interactable prefab as a child; For testing its a door
        if (parameters.Interior)
        {
            var door = Instantiate(Door, t.transform);


            // var door = Instantiate(Door, t.transform);
            // door.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            var parentRenderer = t.GetComponent<SpriteRenderer>();
            var childRenderer = door.GetComponent<SpriteRenderer>();

            var offset = (parentRenderer.bounds.size.y / 2) - (childRenderer.bounds.size.y / 2);
            offset /= parentRenderer.transform.localScale.y;

            var vec = new Vector3(0, -offset, 0);
            if (parameters.Type == TileTypes.SkyscraperCornerBL ||
                parameters.Type == TileTypes.SkyscraperCornerBLDestroyed)
            {
                vec.x = parentRenderer.bounds.size.x / 2;
                vec.x /= parentRenderer.transform.localScale.x;
            }

            door.transform.localPosition = vec;

            var interactivityController = door.transform.GetComponentInChildren<InteractivityController>();

            interactivityController.Menu = NPCMenu;

            interactivityController.ParentTile = t;

            var details = new RoomDetails();

            Sprite interior;
            if (_interiorAtlas.ContainsKey(parameters.Type.Value))
            {
                interior = _interiorAtlas[parameters.Type.Value];
            }
            else
            {
                interior = _interiorAtlas[TileTypes.House];
            }

            details.RoomImage = interior;

            var npcArr = _npcAtlas[parameters.Type.Value];

            var charSelection = Random.Range(0, npcArr.Count);

            details.NPCImage = npcArr[charSelection];

            Dialouge dialogue;

            var type = parameters.Type.Value;
            if (type == TileTypes.CityHall || type == TileTypes.CityHallDestroyed)
            {
                dialogue = _cityLines;
            }
            else if (type == TileTypes.FireStation || type == TileTypes.FireStationDestroyed)
            {
                dialogue = _fireLines;
            }
            else if (type == TileTypes.Park || type == TileTypes.ParkDestroyed)
            {
                dialogue = _shrineLines;
            }
            else if (type == TileTypes.PoliceStation || type == TileTypes.PoliceStationDestroyed)
            {
                dialogue = _policeLines;
            }
            else if (type == TileTypes.HardwareStore || type == TileTypes.HardwareStoreDestroyed)
            {
                dialogue = _hardwareLines;
            }
            else
            {
                dialogue = new Dialouge();
                var randDialogue = Random.Range(0, _buildingLines.Count);
                var chosen = _buildingLines.Skip(randDialogue).Take(1).First();

                dialogue.rebuildDoneSetences = chosen.rebuildDoneSetences;
                dialogue.rebuildSentences = chosen.rebuildSentences;
                dialogue.sentences = chosen.sentences;
                dialogue.emptySentences = chosen.emptySentences;
                dialogue.exitSentences = chosen.exitSentences;
            }

            var randNameIndex = Random.Range(0, _names.Length);
            dialogue.name = _names[randNameIndex];

            details.NPCDialouge = dialogue;
            details.NPCResolved = false;
            details.RebuildResolved = false;
            details.HasInteracted = false;
            details.HasDecideInteracted = false;
            details.HasRebuildInteracted = false;
            details.type = type;

            interactivityController.Details = details;

            interactivityController.interactAction = new UnityEvent();
            interactivityController.interactAction.AddListener(NPCMenu.Interact);

            // var interactController = door.GetComponent<InteractController>();
            // interactController.player = Player;
            //
            // //game menu canvas, npcmenubackground 
            // interactController.GameMenuCanvas = GameMenuCanvas;
            // interactController.NPCMenu = NPCMenu;
            //
            // //children
            // //room canvas,
            // //room image, room npc
            //
            // var RoomCanvasChild = door.transform.GetChild(0).gameObject;
            // interactController.Room = RoomCanvasChild;
            //
            // var RoomImageChild = RoomCanvasChild.transform.GetChild(0).gameObject;
            // var RoomNPCChild = RoomCanvasChild.transform.GetChild(1).gameObject;
            //
            // interactController.RoomCanvasGroup = RoomImageChild.GetComponent<CanvasGroup>();
            // interactController.NPCCanvasGroup = RoomNPCChild.GetComponent<CanvasGroup>();
            // interactController.RoomNPC = RoomNPCChild;
            //
            // var roomSprite = RoomNPCChild.GetComponent<RoomSprite>();
            //
            // roomSprite.interact = door;
            //
            // interactController.RoomSprite = roomSprite;
            //
            //
            // var interior = Instantiate(TavernInterior,
            //     new Vector3(door.transform.position.x, door.transform.position.y), Quaternion.identity);
            //
            // interior.SetActive(false);
            //
            // var exteriorDoor = door.GetComponent<InteractController>();
            // exteriorDoor.exterior = gameObject;
            // exteriorDoor.interior = interior;
            // exteriorDoor.player = Player;

            // var interactController = interior.GetComponent<InteractController>();
            // interactController.exterior = gameObject;
            // interactController.interior = interior;

            // if (parameters.DialogueFile != null)
            // {
            //     LoadNpcs(parameters.DialogueFile, interior.transform);
            // }
        }

        return t;
    }

    void GenerateTiles()
    {
        if (genThread is null)
        {
            threadWorker();
        }
        else
        {
            Debug.Log("waiting");
            genThread.Join();
            Debug.Log("gen done");
        }

        var (startX, startY) = grid.GetRandomRoad();

        var posX = startX * size;
        var posY = (height - startY - 1) * size;

        Player.transform.position = new Vector3(posX, posY);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileParameters parameters = new TileParameters();
                var type = grid.TileGrid[y, x].TileOptions[0].Type;

                switch (type)
                {
                    case TileTypes.House:
                        _houseEvac++;
                        break;
                    case TileTypes.HouseDestroyed:
                        _houseRebuild++;
                        _houseEvac++;
                        break;
                    case TileTypes.SkyscraperCornerBL:
                        _skyEvac++;
                        break;
                    case TileTypes.SkyscraperCornerBLDestroyed:
                        _skyEvac++;
                        _skyBuild++;
                        break;
                    case TileTypes.Park:
                        _shrineEvac++;
                        break;
                    case TileTypes.ParkDestroyed:
                        _shrineEvac++;
                        _shrineRebuild++;
                        break;
                    case TileTypes.HardwareStore:
                        _hardwareEvac++;
                        break;
                    case TileTypes.HardwareStoreDestroyed:
                        _hardwareEvac++;
                        _hardwareRebuild++;
                        break;
                    case TileTypes.CityHall:
                        _cityhallEvac++;
                        break;
                    case TileTypes.CityHallDestroyed:
                        _cityhallEvac++;
                        _cityhallRebuild++;
                        break;
                    case TileTypes.FireStation:
                        _fireEvac++;
                        break;
                    case TileTypes.FireStationDestroyed:
                        _fireEvac++;
                        _fireRebuild++;
                        break;
                    case TileTypes.PoliceStation:
                        _policeEvac++;
                        break;
                    case TileTypes.PoliceStationDestroyed:
                        _policeEvac++;
                        _policeRebuild++;
                        break;
                }

                if (_spriteAtlas.TryGetValue(type, out var value))
                {
                    parameters.Type = type;
                }
                else
                {
                    parameters.Color = type.GetColor();
                }

                if (_needsGrass.Contains(type))
                {
                    parameters.Grass = true;
                }

                if ((type >= TileTypes.Park && type <= TileTypes.CityHallDestroyed) ||
                    type == TileTypes.SkyscraperCornerBL || type == TileTypes.SkyscraperCornerBLDestroyed)
                {
                    parameters.Interior = true;
                }

                if ((type >= TileTypes.HouseDestroyed && type <= TileTypes.CityHallDestroyed) ||
                    (type >= TileTypes.SkyscraperCornerBLDestroyed && type <= TileTypes.SkyscraperCornerTRDestroyed))
                {
                    parameters.Destroyed = true;
                }

                if (type < TileTypes.SkyscraperCornerBL || type > TileTypes.SkyscraperUpperTR)
                {
                    GenerateTile(x, height - y - 1, parameters).name =
                        grid.TileGrid[y, x].TileOptions[0].Type.ToString();
                }

                if (type == TileTypes.SkyscraperCornerBL || type == TileTypes.SkyscraperCornerBLDestroyed)
                {
                    parameters.Index = Random.Range(0, 2);
                    var blc = GenerateTile(x, height - y - 1, parameters);
                    parameters.Interior = false;
                    parameters.Type = grid.TileGrid[y, x + 1].TileOptions[0].Type;
                    var brc = GenerateTile(x + 1, height - y - 1, parameters);
                    parameters.Type = grid.TileGrid[y - 1, x].TileOptions[0].Type;
                    var tlc = GenerateTile(x, height - y, parameters);
                    parameters.Type = grid.TileGrid[y - 1, x + 1].TileOptions[0].Type;
                    var trc = GenerateTile(x + 1, height - y, parameters);

                    parameters.Color = null;
                    parameters.Type = parameters.Destroyed
                        ? TileTypes.SkyscraperUpperBLDestroyed
                        : TileTypes.SkyscraperUpperBL;
                    var bl = GenerateTile(x, height - y + 1, parameters);
                    bl.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Skyscraper");

                    parameters.Type = parameters.Destroyed
                        ? TileTypes.SkyscraperUpperBRDestroyed
                        : TileTypes.SkyscraperUpperBR;
                    var br = GenerateTile(x + 1, height - y + 1, parameters);
                    br.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Skyscraper");

                    parameters.Type = parameters.Destroyed
                        ? TileTypes.SkyscraperUpperTLDestroyed
                        : TileTypes.SkyscraperUpperTL;
                    var tl = GenerateTile(x, height - y + 2, parameters);
                    tl.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Skyscraper");

                    parameters.Type = parameters.Destroyed
                        ? TileTypes.SkyscraperUpperTRDestroyed
                        : TileTypes.SkyscraperUpperTR;
                    var tr = GenerateTile(x + 1, height - y + 2, parameters);
                    tr.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Skyscraper");

                    var collider = bl.AddComponent<BoxCollider2D>();
                    collider.isTrigger = true;
                    collider.offset = new Vector2(0.5f, 0.5f);
                    collider.size = new Vector2(2, 2);

                    var transparency = bl.AddComponent<TransparencyController>();
                    transparency.bl = bl.GetComponent<SpriteRenderer>();
                    transparency.br = br.GetComponent<SpriteRenderer>();
                    transparency.tl = tl.GetComponent<SpriteRenderer>();
                    transparency.tr = tr.GetComponent<SpriteRenderer>();

                    var otherTiles = new Tile[7]
                    {
                        brc, trc, tlc, bl, tl, br, tr,
                    };

                    blc.otherTiles = otherTiles;
                }
            }
        }

        genDone = true;
    }

    public int GetTalkNpcCount()
    {
        // For returning the total number of talk npcs, change it as needed 

        return 1;
    }

    public int GetHouseCount()
    {
        // For returning the total number of houses, change it as needed 

        return _houseEvac;
    }

    public int GetHouseRebuildCount()
    {
        return _houseRebuild;
    }

    public int GetSkyscraperCount()
    {
        // For returning the total number of skyscrapers, change it as needed

        return _skyEvac;
    }

    public int GetSkyscraperRebuildCount()
    {
        // For returning the total number of skyscrapers, change it as needed

        return _skyBuild;
    }

    public int GetShrineCount()
    {
        // For returning the total number of shrines, change it as needed 

        return _shrineEvac;
    }

    public int GetShrineRebuildCount()
    {
        // For returning the total number of shrines, change it as needed 

        return _shrineRebuild;
    }

    public int GetHardwareCount()
    {
        // For returning the total number of hardware stores, change it as needed

        return _hardwareEvac;
    }

    public int GetHardwareRebuildCount()
    {
        // For returning the total number of hardware stores, change it as needed

        return _hardwareRebuild;
    }

    public int GetCityHallCount()
    {
        // For returning the total number of city halls, change it as needed

        return _cityhallEvac;
    }

    public int GetCityHallRebuildCount()
    {
        // For returning the total number of city halls, change it as needed

        return _cityhallRebuild;
    }

    public int GetFireStationCount()
    {
        // For returning the total number of fire stations, change it as needed

        return _fireEvac;
    }

    public int GetFireStationRebuildCount()
    {
        // For returning the total number of fire stations, change it as needed

        return _fireRebuild;
    }

    public int GetPoliceStationCount()
    {
        // For returning the total number of police stations, change it as needed

        return _policeEvac;
    }

    public int GetPoliceStationRebuildCount()
    {
        // For returning the total number of police stations, change it as needed

        return _policeRebuild;
    }
}