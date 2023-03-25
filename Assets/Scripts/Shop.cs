using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _townShop;
    [SerializeField] private GameObject _groundShop;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _townWarningText;
    [SerializeField] private TextMeshProUGUI _groundWarningText;
    private bool _townIsOpen = false;
    private bool _groundIsOpen = false;

    public void OpenCloseTownShop()
    {
        if (_groundIsOpen)
        {
            OpenCloseGroundShop();
        }
        if (_townIsOpen)
        {
            _townWarningText.SetText("");
            _gameManager.setPause(false);
            _townShop.SetActive(false);
            _townIsOpen = false;
        }
        else
        {
            _gameManager.setPause(true);
            _townShop.SetActive(true);
            _townIsOpen = true;
        }
    }

    public void OpenCloseGroundShop()
    {
        if (_townIsOpen)
        {
            OpenCloseTownShop();
        }
        if (_groundIsOpen)
        {
            _groundWarningText.SetText("");
            _gameManager.setPause(false);
            _groundShop.SetActive(false);
            _groundIsOpen = false;
        }
        else
        {
            _gameManager.setPause(true);
            _groundShop.SetActive(true);
            _groundIsOpen = true;
        }
    }
}
