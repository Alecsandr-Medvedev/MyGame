using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barHP : MonoBehaviour
{
    private float _countHp;
    [SerializeField] private Transform _lineHP;


    void Start()
    {
     _countHp = 0.1f;
    }
   

    public void ChangeHP(float _changeCount)
    {
        _countHp = _changeCount / 10f;
        _lineHP.localScale = new Vector3(_countHp, _lineHP.localScale.y, _lineHP.localScale.z);
    }
}
