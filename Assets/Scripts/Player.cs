using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameState _gameState;
    [SerializeField] private barHP _barHP;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _counterBullets;

    private float _health;
    private float _maxHealth;

    private int _countBullets;
    private int _maxCountBullets;

    private float _allTimeReload;
    private float _timeReload = 0;

    private float _allTimeHealing;
    private float _timeHealing = 0;

    private Vector2 _myPosition;

    private void Start()
    {
        _myPosition = transform.position;
        _health = 1;
        _maxHealth = 1;
    }

    private void Update()
    {
        if (!_gameManager.isPause())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RotatePlayer(mousePosition);
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !EventSystem.current.IsPointerOverGameObject())
            {
                Shot(mousePosition);
            }
        }
            
    }

    private void FixedUpdate()
    {
        if (!_gameManager.isPause())
        {
            if (_timeReload > 0 && _countBullets < _maxCountBullets)
            {
                _timeReload--;
            }
            else
            {
                if (_countBullets < _maxCountBullets)
                {
                    Reload();

                }
            }
            if (_timeHealing > 0 && _health < _maxHealth)
            {
                _timeHealing--;
            }
            else
            {
                if (_health < _maxHealth)
                {
                    Heal();
                }
            }
        }
        
    }

    private void RotatePlayer(Vector2 pos)
    {
        Vector2 relativePos = _myPosition - pos;
        float angel = Mathf.Atan2(relativePos.x, relativePos.y) * _gameState.MC_180_PI;
        Vector3 rotation = transform.eulerAngles;
        rotation.z = -angel;
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void Shot(Vector2 point)
    {
        if (_countBullets > 0)
        {
            _countBullets--;
            string line = $"{_countBullets}/{_maxCountBullets}";
            _counterBullets.SetText(line);

            Vector3 rot = transform.eulerAngles + new Vector3(0, 0, 180);

            GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(rot));

            bullet.GetComponent<Bullet>().Init(point, _gameState.bulletTimeAlive, _gameState.bulletSpeed, _gameState.bulletPower);
        }
    }

    public void Hurt(float power)
    {
        _health -= power;
        if (_health <= 0)
        {
            _gameManager.Lose();
        }
        else
        {
            _barHP.ChangeHP(_health / _maxHealth);
        }

    }

    public void Init(float health, int countBullets, float timeReload, float timeHealing)
    {
        _health = health;
        _maxHealth = health;

        _countBullets = countBullets;
        _maxCountBullets = countBullets;

        _allTimeReload = timeReload;
        _timeReload = timeReload;
        _timeHealing = timeHealing;
        _allTimeHealing = timeHealing;
        string line = $"{_countBullets}/{_maxCountBullets}";
        _counterBullets.SetText(line);

    }

    private void Reload()
    {
        
        _timeReload = _allTimeReload;
        _countBullets++;
        string line = $"{_countBullets}/{_maxCountBullets}";
        _counterBullets.SetText(line);
    }

    private void Heal()
    {
        _timeHealing = _allTimeHealing;
        _health++;
        _barHP.ChangeHP(_health / _maxHealth);
    }

    public void ChangeSpeedRegen(float num)
    {
        _allTimeHealing = num;
    }
    public void ChangeSpeedReload(float num)
    {
        _allTimeReload = num;
    }

    public void ChangeMaxHealth(float num)
    {
        _maxHealth = num;
        _barHP.ChangeHP(_health / _maxHealth);
    }

    public void ChangeMaxBullets(int num)
    {
        _maxCountBullets = num;
        string line = $"{_countBullets}/{_maxCountBullets}";
        _counterBullets.SetText(line);
    }
}
