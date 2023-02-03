using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private TMP_Text _coinText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCoinText(int coins)
    {
        _coinText.text = coins.ToString();
    }
}
