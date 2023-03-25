using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _timeWave;
    [SerializeField] private int _timeRest;

    [SerializeField] private GameObject _entityLvl1;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _dimondPrefab;

    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _dimondCounter;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Player _player;
    [SerializeField] private GameState _gameState;

    [SerializeField] private Image _waveBar;
    [SerializeField] private TextMeshProUGUI _waveCounter;

    [SerializeField] private int _timerEvent;

    [SerializeField] private GameObject _gamePlace;
    [SerializeField] private GameObject _loseMenu;
    [SerializeField] private GameObject _winMenu;

    [SerializeField] private Build _build;
    [SerializeField] private GameObject _pit;
    [SerializeField] private GameObject _mine;

    [SerializeField] private TextMeshProUGUI _warningTownText;
    [SerializeField] private TextMeshProUGUI _warningGroundText;

    private float _timerSpawn;
    private float _typeEvent;
    private int _countWave;

    private bool _pause = false;

    private void Start()
    {
        _timerEvent = _timeRest;
        _typeEvent = 0;
        _timeWave = _timeWave / 2;
        _countWave = 0;
        _player.Init(_gameState.playerHealth, _gameState.countBullets, _gameState.timeReload, _gameState.timeHealing);
    }

    private void FixedUpdate()
    {
        if (!_pause)
        {
            _timerEvent--;
            if (_timerEvent < 1 && _gameState.countEntity == 0)
            {
                ChangeEvent();
            }

            if (_typeEvent == 1 && _timerEvent > 0)
            {
                _timerSpawn -= Random.Range(0, (_timeWave - _timerEvent) / _gameState.dificulty);

                if (_timerSpawn < 0)
                {

                    CreateEntity();
                    _timerSpawn = _timeWave;
                }

                _waveBar.fillAmount = 1 - (_timerEvent / (float)_timeWave);
            }
        }
        
    }

    private void CreateEntity()
    {
        _gameState.countEntity++;
        Vector2 startPosition = Random.insideUnitCircle.normalized * _gameState.radiusSpawn;
        Vector2 relativePosition = startPosition - (Vector2)_playerTransform.position;
        float angel = Mathf.Atan2(relativePosition.x, relativePosition.y) * _gameState.MC_180_PI;

        Vector3 rotationV = transform.eulerAngles;
        rotationV.z = -angel + 180;
        Quaternion rotationQ = Quaternion.Euler(rotationV);

        Entity entity = Instantiate(_entityLvl1, startPosition, rotationQ).GetComponent<Entity>();

        entity.Init(_playerTransform.position, _gameState.entitySpeed, _gameState.entityHealth, _gameState.entityPower, 1, gameObject.GetComponent<GameManager>());

    }

    private void ChangeEvent()
    {
        _typeEvent = (_typeEvent + 1) % 2;
        switch (_typeEvent)
        {
            case 0:
                setRest();
                break;
            case 1:
                setWave();
                break;
        }
    }

    private void setWave()
    {
        CreateEntity();
        if (_countWave != 3)
        {
            _countWave++;
            _waveCounter.SetText($"Номер волны: {_countWave}/3");
            _timeWave *= 2;
            _timerEvent = _timeWave;
            _timerSpawn = _timeWave;
            _gameState.dificulty -= _gameState.dificulty * 0.1f;
            
        }
        else
        {
            _waveCounter.SetText($"Конец");
            Win();
        }

    }

    private void setRest()
    {
        _waveBar.fillAmount = 0;
        _timerEvent = _timeRest;
    }

    public void killEntity(Transform tansformEntity, int lvl, float power) {
        _gameState.countEntity--;
        if (power == -1)
        {
            moneyDistributor(tansformEntity.position, lvl);
        }
        else
        {
            
            _player.Hurt(power);
        }

    }

    private void moneyDistributor(Vector2 position, int lvl)
    {
        int countCoins = Random.Range(1, lvl * (int)(25 - _gameState.dificulty));
        int countDimonds = Random.Range(0, countCoins);


        for (int i = 0; i < countCoins; i++)
        {
            Vector2 randomizePos = new Vector2(Random.Range(-10, 10) / 10f + position.x, Random.Range(-10, 10) / 10f + position.y);
            Money money = Instantiate(_coinPrefab, randomizePos, Quaternion.identity).GetComponent<Money>();

            money.Init((Vector2)_playerTransform.transform.position, _gameState.animationSpeed, 'c', gameObject.GetComponent<GameManager>());
        }

        for (int i = 0; i < countDimonds; i++)
        {
            Vector2 randomizePos = new Vector2(Random.Range(-10, 10) / 10f + position.x, Random.Range(-10, 10) / 10f + position.y);
            Money money = Instantiate(_dimondPrefab, randomizePos, Quaternion.identity).GetComponent<Money>();

            money.Init((Vector2)_playerTransform.transform.position, _gameState.animationSpeed, 'd', gameObject.GetComponent<GameManager>());
        }
    }


    public void addMoney(char type)
    {
        switch (type)
        {
            case 'c':
                _gameState.coinCount++;
                _coinCounter.SetText(_gameState.coinCount.ToString());
                break;
            case 'd':
                _gameState.dimondCount++;
                _dimondCounter.SetText(_gameState.dimondCount.ToString());
                break;
        }
    }

    public void Lose()
    {
        _pause = true;
        _loseMenu.SetActive(true);
        _gamePlace.SetActive(false);
    }

    public void Win()
    {
        _pause = true;
        _winMenu.SetActive(true);
        _gamePlace.SetActive(false);
    }

    public void ChangeSpeedRegen(int cost)
    {
        if (cost <= _gameState.dimondCount)
        {
            _gameState.timeHealing -= _gameState.timeHealing * 0.1f;
            _gameState.dimondCount -= cost;
            _warningTownText.SetText("");
            _player.ChangeSpeedRegen(_gameState.timeHealing);
            _dimondCounter.SetText(_gameState.dimondCount.ToString());
        }
        else
        {
            _warningTownText.SetText("Не хватает средств");
        }
    }
    public void ChangeSpeedReload(int cost)
    {
        if (cost <= _gameState.dimondCount)
        {
            _gameState.timeReload -= _gameState.timeReload * 0.1f;
            _gameState.dimondCount -= cost;
            _warningTownText.SetText("");
            _player.ChangeSpeedReload(_gameState.timeReload);
            _dimondCounter.SetText(_gameState.dimondCount.ToString());
        }
        else
        {
            _warningTownText.SetText("Не хватает средств");
        }
    }

    public void ChangePowerBullets(int cost)
    {
        if (cost <= _gameState.dimondCount)
        {
            _gameState.bulletPower += _gameState.bulletPower * 0.1f;
            _gameState.dimondCount -= cost;
            _warningTownText.SetText("");
            _dimondCounter.SetText(_gameState.dimondCount.ToString());
        }
        else
        {
            _warningTownText.SetText("Не хватает средств");
        }
    }

    public void ChangeMaxHealth(int cost)
    {
        if (cost <= _gameState.dimondCount)
        {
            _gameState.playerHealth += _gameState.playerHealth * 0.1f;
            _gameState.dimondCount -= cost;
            _warningTownText.SetText("");
            _player.ChangeMaxHealth(_gameState.playerHealth);
            _dimondCounter.SetText(_gameState.dimondCount.ToString());
        }
        else
        {
            _warningTownText.SetText("Не хватает средств");
        }
    }

    public void ChangeMaxBullets(int cost)
    {
        if (cost <= _gameState.dimondCount)
        {
            _gameState.countBullets += (int)(_gameState.countBullets * 0.1f);
            _gameState.dimondCount -= cost;
            _warningTownText.SetText("");
            _player.ChangeMaxBullets(_gameState.countBullets);
            _dimondCounter.SetText(_gameState.dimondCount.ToString());
        }
        else
        {
            _warningTownText.SetText("Не хватает средств");
        }
    }

    public void setPause(bool pause)
    {
        _pause = pause;
    }

    public bool isPause()
    {
        return _pause;
    }

    public void buyPit(int cost)
    {
        if (cost <= _gameState.coinCount)
        {
            _gameState.coinCount -= cost;
            _coinCounter.SetText(_gameState.coinCount.ToString());
            _build.SelectObject(_pit);
        }
        else
        {
            _warningGroundText.SetText("Не хватает средств");
        }
    }

    public void buyMine(int cost)
    {
        ;
        if (cost <= _gameState.coinCount)
        {
            _gameState.coinCount -= cost;
            _coinCounter.SetText(_gameState.coinCount.ToString());
            _build.SelectObject(_mine);
        }
        else
        {
            _warningGroundText.SetText("Не хватает средств");
        }
    }
}
