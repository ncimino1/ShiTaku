using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public SpriteRenderer bl;
    public SpriteRenderer br;
    public SpriteRenderer tl;
    public SpriteRenderer tr;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bl.color = new Color(1, 1, 1, 0.5f);
            br.color = new Color(1, 1, 1, 0.5f);
            tl.color = new Color(1, 1, 1, 0.5f);
            tr.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bl.color = new Color(1, 1, 1, 1);
            br.color = new Color(1, 1, 1, 1);
            tl.color = new Color(1, 1, 1, 1);
            tr.color = new Color(1, 1, 1, 1);
        }
    }
}