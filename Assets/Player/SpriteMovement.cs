using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteMovement : MonoBehaviour
{
    private Rigidbody2D _phys;
    private Vector2 _dir;
    public float velocity = 5;

    public Scene mainScene;

    private bool _keyHeld;
    private float _startTime;

    private float _coeff;
    private readonly float _accelTime = 0.5f;

    private static Dictionary<string, Vector3> _savedLocs = new Dictionary<string, Vector3>();

    public bool lockMovement;


    private float Interp(float time)
    {
        return Math.Min(_coeff * time + 2, velocity);
    }

    private bool KeyPressed()
    {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.S);
    }

    private Vector2 MovementDirection()
    {
        Vector2 dir = new Vector2();
        dir.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        dir.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        return dir;
    }

    // Start is called before the first frame update
    void Start()
    {
        lockMovement = false;
        _phys = GetComponent<Rigidbody2D>();
        _dir = new Vector2();
        _keyHeld = false;

        _coeff = (velocity - 2) / (_accelTime);

        if (_savedLocs.TryGetValue(SceneManager.GetActiveScene().name, out Vector3 pos))
        {
            transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lockMovement == false){
            _dir = MovementDirection().normalized;
        }
    }

    private void FixedUpdate()
    {
        float interpVel;
        if (!_keyHeld && KeyPressed())
        {
            _keyHeld = true;
            _startTime = Time.time;
            interpVel = 0;
        }
        else if (_keyHeld && !KeyPressed())
        {
            interpVel = 0;
            _keyHeld = false;
        }
        else
        {
            interpVel = Time.time - _startTime > 0.2 ? velocity : Interp(Time.time - _startTime);
        }

        _phys.velocity = _dir * interpVel;
    }

    private void OnDestroy()
    {
        _savedLocs[SceneManager.GetActiveScene().name] = transform.position;
    }
}