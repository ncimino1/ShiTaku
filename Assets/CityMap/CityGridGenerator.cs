using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        if (readFile)
        {
            LoadTiles();
        }
        else
        {
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
                GenerateTile(x, y, tiles.Types[x + y * tiles.Width].GetColor(), tiles.doors[x + y * tiles.Width]);
            }
        }
    }

    Tile GenerateTile(int x, int y, Color? color = null, String tp = null)
    {
        var xPos = x * size;
        var yPos = y * size;
        var t = Instantiate(tile, transform);
        t.transform.localPosition = new Vector3(xPos, yPos);


        t.transform.localScale = new Vector3(size, size, size);

        var sprite = t.GetComponent<SpriteRenderer>();
        sprite.sortingLayerID = SortingLayer.NameToID("Tile");
        sprite.color = color ?? Random.ColorHSV(0, 1, 0, 1, 0.5f, 1);

        //Make each tile have a interactable prefab as a child; For testing its a door
        if (tp != null)
        {
            Debug.Log(tp);
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
        }

        return t;
    }

    void GenerateTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var t = GenerateTile(x, y);
            }
        }
    }
}