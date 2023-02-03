using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _fillImage;
    [SerializeField]
    private float _lerpSpeed;
    
    private Character _target;

    public void OnInit(Character character)
    {
        _target = character;

        if (character as Player)
        {
            _fillImage.color = Color.green;
        }
        else
        {
            _fillImage.color = Color.red;
        }
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_target.transform.position + Vector3.up * 1.6f);

        _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _target.CurrentHealth / _target.MaxHealth, _lerpSpeed * Time.deltaTime);
    }
}
