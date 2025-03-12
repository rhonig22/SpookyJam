using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private const float _border = .2f;
    private float _width = 1;
    public float Width
    {
        get { return _width; }
        set
        {
            _width = value;
            UpdateSize();
        }
    }

    private float _height = 1;
    public float Height
    {
        get { return _height; }
        set
        {
            _height = value;
            UpdateSize();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSize()
    {
        transform.localScale = new Vector3(_width - _border*2, _height - _border*2, 1);
        var shape = _particleSystem.shape;
        shape.enabled = true;
        shape.radius = _width / 2 - _border*2;
        shape.position = new Vector3(0, -_height / 2 + _border, 0);
    }
}
