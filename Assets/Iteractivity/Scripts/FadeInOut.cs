using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool fadein  = false;
    public bool fadeout = false;

    public float TimeToFade = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(fadein){
            if(canvasGroup.alpha < 1){
                canvasGroup.alpha += Time.deltaTime * TimeToFade;

                if(canvasGroup.alpha >= 1){
                    fadein = false;
                }
            }
        }

        if(fadeout){
            if(canvasGroup.alpha >= 0){
                canvasGroup.alpha -= Time.deltaTime * TimeToFade;

                if(canvasGroup.alpha == 0){
                    fadeout = false;
                }
            }
        }
    }

    public void FadeIn(){
        fadein = true;
        fadeout = false;
    }

    public void FadeOut(){
        fadeout = true;
        fadein = false;
    }

    public void Despawn(){
        Debug.Log("Despawning NPC");
        StartCoroutine(StartDespawn());
    }

    public IEnumerator StartDespawn(){
        FadeIn();
        Debug.Log("Despawning NPC");
        yield return new WaitForSeconds(1);
        Debug.Log("NPC Despawned");
        FadeOut();
        Debug.Log("Fading back out");
    }
}