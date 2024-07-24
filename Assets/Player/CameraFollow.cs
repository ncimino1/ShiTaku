using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    public bool followCamera = true;

    public void zoomOut()
    {
        followCamera = false;
        transform.position = new Vector3(35, 35, -10);
        GetComponent<Camera>().orthographicSize = 43;
    }

    public void zoomIn()
    {
        followCamera = true;
        GetComponent<Camera>().orthographicSize = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (followCamera)
        {
            transform.position = player.position + new Vector3(0, 0, -10);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            zoomOut();
        } else if (Input.GetKeyDown(KeyCode.H))
        {
            zoomIn();
        }
    }
}