using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 _direction = new Vector2(0, 0);
    private float _timeAlive = 10;
    private float _power = 0;

    public void Init(Vector2 point, float time, float speed, float power)
    {
        _direction = point.normalized * speed;
        _timeAlive = time;
        _power = power;
    }

    private void FixedUpdate()
    {
        _timeAlive -= 1;
        if (_timeAlive < 0)
        {
            Destroy(gameObject);
        }

        transform.position = _direction + (Vector2)transform.position;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Entity")
        {
            collision.gameObject.GetComponent<Entity>().Hurt(_power);
            Destroy(gameObject);
            
        }
    }

    private void OnDestroy()
    {
        
    }
}
