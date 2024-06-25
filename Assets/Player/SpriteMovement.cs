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
    

    private float Interp(float time)
    {
        return Math.Min(_coeff * time + 2, velocity);
    }

    private bool KeyPressed()
    {
        return Input.GetAxisRaw("Horizontal") != 0.0 || Input.GetAxisRaw("Vertical") != 0.0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
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
        _dir.x = Input.GetAxisRaw("Horizontal");
        _dir.y = Input.GetAxisRaw("Vertical");
        _dir.Normalize();
    }

    private void FixedUpdate()
    {
        float interpVel;
        if (!_keyHeld && KeyPressed())
        {
            _keyHeld = true;
            _startTime = Time.time;
            interpVel = 0;
        } else if (_keyHeld && !KeyPressed())
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
