using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    private Vector2 _step = new Vector2(0, 0);
    private Vector2 _endPoint = new Vector2(0, 0);
    private GameManager _gameManger;
    private char _type = 'N';

    public void Init(Vector2 point, float speed, char type, GameManager gameManager)
    {
        _gameManger = gameManager;
        _endPoint = point;
        _step = (point - (Vector2)transform.position).normalized * speed;
        _type = type;
    }

    private void FixedUpdate()
    {
        transform.position = (Vector2)transform.position + _step;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
        {
            _gameManger.addMoney(_type);
            Destroy(gameObject);
        }
    }

}
