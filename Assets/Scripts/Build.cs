using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{

    private GameObject _select;
    private bool _isBuild = false;

    private void Update()
    {
        if (_isBuild)
        {

            _select.transform.position = getMousePos();
            if (Input.GetMouseButtonDown(0))
            {
                BuildObject();
            }
        }

    }

    private void BuildObject()
    {
        _isBuild = false;
        Instantiate(_select, getMousePos(), Quaternion.identity).GetComponent<Building>().setActive();
        Destroy(_select);
    }

    public void SelectObject(GameObject building)
    {
        _select = Instantiate(building, getMousePos(), Quaternion.identity);
        _isBuild = true;
    }

    private Vector3 getMousePos()
    {
        Vector3 mousePose = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePose.x, mousePose.y, 0);
    }
}
