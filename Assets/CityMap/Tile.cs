using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool Destroyed;
    public Sprite FixedAsset;

    public Tile[] otherTiles;

    public void FixTile()
    {
        if (otherTiles.Length != 0)
        {
            foreach (var tile in otherTiles)
            {
                tile.FixTile();
            }
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = FixedAsset;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
