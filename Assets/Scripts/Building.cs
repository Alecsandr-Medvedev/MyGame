using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private bool _isActive = false;

    public void setActive()
    {
        _isActive = true;
    }

    public bool isActive()
    {
        return _isActive;
    }
}
