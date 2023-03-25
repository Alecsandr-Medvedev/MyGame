using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Vector2 _step = new Vector2(0, 0);
    private float _health = 0;
    private float _maxHealth = 0;
    private float _power = 0;
    private int _lvl = 0;
    private GameManager _gameManager;
    [SerializeField] private GameObject _barHP;

    public void Init(Vector2 point, float speed, float health, float power, int lvl, GameManager gameManager)
    {
        _step = (point - (Vector2)transform.position).normalized * speed;
        _health = health;
        _maxHealth = health;
        _power = power;
        _gameManager = gameManager;
        _lvl = lvl;
        _barHP = Instantiate(_barHP, new Vector2(transform.position.x, transform.position.y + 0.7f), Quaternion.identity);
        _barHP.GetComponent<barHP>().ChangeHP(_health / _maxHealth);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_gameManager.isPause())
        {
            transform.position = (Vector2)transform.position + _step;
            _barHP.transform.position = (Vector2)_barHP.transform.position + _step;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
        {
            _gameManager.killEntity(gameObject.GetComponent<Transform>(), _lvl, _power);
            Destroy(gameObject);
        }
    }

    public void Hurt(float power)
    {
        _health -= power;
        if (_health <= 0)
        {
            _gameManager.killEntity(gameObject.GetComponent<Transform>(), _lvl, -1);
            Destroy(gameObject);
        }
        else
        {
            _barHP.GetComponent<barHP>().ChangeHP(_health / _maxHealth);
        }
        
    }

    private void OnDestroy()
    {
        Destroy(_barHP);
    }

    public float getMaxHealth()
    {
        return _maxHealth;
    }
}
