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

    private Sprite ShrineWorker;

    private Dictionary<TileTypes, Sprite> _spriteAtlas;

    private Dictionary<TileTypes, Sprite> _interiorAtlas;

    private Sprite _grass;

    private Thread _thread;

    private HashSet<TileTypes> _needsGrass;

    private class TileParameters
    {
        public Color? Color;
        public TileTypes? Type;
        public bool Interior;
        public String DialogueFile;
        public bool Grass = false;
        public bool Destroyed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadSprites();
        if (readFile)
        {
            LoadTiles();
        }
        else
        {
            //_thread = new Thread(GenerateTiles);
            //_thread.Start();
            GenerateTiles();
        }
    }

    void LoadSprites()
    {
        var spritesArray = Resources.Load<SpriteAtlas>("Tiles/TileAtlas");
        _grass = Resources.LoadAll<Sprite>("Tiles/grass")[0];

        _spriteAtlas = new Dictionary<TileTypes, Sprite>();

        foreach (var type in Enum.GetValues(typeof(TileTypes)).Cast<TileTypes>())
        {
            var sprite = spritesArray.GetSprite(type.ToString());

            if (sprite)
            {
                _spriteAtlas[type] = sprite;
            }
        }

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

        _interiorAtlas.Add(TileTypes.Park, parkInterior);
        _interiorAtlas.Add(TileTypes.ParkDestroyed, parkInterior);
        _interiorAtlas.Add(TileTypes.House, houseInterior);
        _interiorAtlas.Add(TileTypes.HouseDestroyed, houseInterior);
        _interiorAtlas.Add(TileTypes.SkyscraperCornerBL, skyscraperInterior);
        _interiorAtlas.Add(TileTypes.SkyscraperCornerBLDestroyed, skyscraperInterior);

        ShrineWorker = Resources.Load<Sprite>("shrineWorker");
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

        if (parameters.Type.HasValue && _spriteAtlas.TryGetValue(parameters.Type.Value, out var value))
        {
            sprite.sprite = value;
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
            Debug.Log(fixedName);
            if (!TileTypes.TryParse(fixedName, out TileTypes result))
            {
                t.FixedAsset = _spriteAtlas[TileTypes.HouseDestroyed];
            }
            else
            {
                t.FixedAsset = _spriteAtlas[result];
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
            details.NPCImage = ShrineWorker;

            var dialogue = new Dialouge();
            dialogue.name = "Worker" + x + " " + y;
            dialogue.sentences = new[]
            {
                "Hello!", "Are you here to help us!",
            };
            dialogue.exitSentences = new[]
            {
                "Thank you for your initiative!", "We will evacuate now!", "Thank you!"
            };
            dialogue.emptySentences = new[]
            {
                "Hello!",
            };
            dialogue.rebuildSentences = new[]
            {
                "Do you want to repair this building?"
            };
            dialogue.rebuildDoneSetences = new[]
            {
                "This building is already in stable condition."
            };

            details.NPCDialouge = dialogue;
            details.NPCResolved = false;
            details.HasInteracted = false;
            details.HasDecideInteracted = false;
            details.HasRebuildInteracted = false;

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
        var config = TileConfiguration.Generate();


        Grid grid = new Grid(width, height, config);
        while (!grid.Collapse())
        {
        }

        grid.FixDuplicates();
        grid.FixRoads();
        grid.FixSkyscrapers();
        grid.DestroyBuildings();

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
    }

    public int GetEvacNpcCount()
    {
        // For returning the total number of evacuation npcs, change it as needed 

        return 1;
    }

    public int GetWornBldCount()
    {
        // For returning the total number of worn buildings, change it as needed 

        return 1;
    }

    public int GetTalkNpcCount()
    {
        // For returning the total number of talk npcs, change it as needed 

        return 1;
    }
}