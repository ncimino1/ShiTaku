using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CityMap.WaveFunctionCollapse;
using Unity.VisualScripting;
using UnityEngine;
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
    
    private Thread _thread;

    private class TileParameters
    {
        public Color? Color;
        public String Interior;
        public String DialogueFile;
    }

    // Start is called before the first frame update
    void Start()
    {
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
                parameters.Interior = tiles.Doors[x + y * tiles.Width];
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
        sprite.sortingLayerID = SortingLayer.NameToID("Tile");
        if (parameters is null)
        {
            sprite.color = Random.ColorHSV(0, 1, 0, 1, 0.5f, 1);
            return t;
        }
        else
        {
            sprite.color = parameters.Color ?? Random.ColorHSV(0, 1, 0, 1, 0.5f, 1);
        }

        //Make each tile have a interactable prefab as a child; For testing its a door
        if (parameters.Interior != null)
        {
            var door = Instantiate(Door, t.transform);
            door.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            var parentRenderer = t.GetComponent<SpriteRenderer>();
            var childRenderer = door.GetComponent<SpriteRenderer>();

            var offset = (parentRenderer.bounds.size.y / 2) - (childRenderer.bounds.size.y / 2);
            offset /= parentRenderer.transform.localScale.y;

            door.transform.localPosition = new Vector3(0, -offset, 0);


            var interior = Instantiate(TavernInterior,
                new Vector3(door.transform.position.x, door.transform.position.y), Quaternion.identity);

            interior.SetActive(false);

            var exteriorDoor = door.GetComponent<InteractController>();
            exteriorDoor.exterior = gameObject;
            exteriorDoor.interior = interior;
            exteriorDoor.player = Player;

            var interactController = interior.GetComponent<InteractController>();
            interactController.exterior = gameObject;
            interactController.interior = interior;

            if (parameters.DialogueFile != null)
            {
                LoadNpcs(parameters.DialogueFile, interior.transform);
            }
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
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var color = grid.TileGrid[y,x].TileOptions[0].Type.GetColor();
                TileParameters parameters = new TileParameters();
                parameters.Color = color;
                // Debug.Log(grid.TileGrid[x,y].TileOptions[0].Type);
                GenerateTile(x, height - y - 1, parameters).name = grid.TileGrid[y,x].TileOptions[0].Type.ToString();
            }
        }
    }
}
