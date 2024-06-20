using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CityGridGenerator : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    
    [SerializeField] private float size;

    [SerializeField] private Tile tile;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles();
    }

    void GenerateTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var xPos = x * size;
                var yPos = y * size;
                var t =Instantiate(tile, new Vector3(xPos, yPos), Quaternion.identity);
                t.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tile");
                t.transform.localScale = new Vector3(size, size, size);
            }
        }
    }
}
