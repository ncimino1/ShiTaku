using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    public bool followCamera = true;

    public void zoomOut()
    {
        transform.position = new Vector3(35, 35, -10);
        GetComponent<Camera>().orthographicSize = 43;
    }

    // Update is called once per frame
    void Update()
    {
        if (followCamera)
        {
            transform.position = player.position + new Vector3(0, 0, -10);
        }
    }
}