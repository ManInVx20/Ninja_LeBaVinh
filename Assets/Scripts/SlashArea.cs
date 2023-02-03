using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashArea : MonoBehaviour
{
    private List<Character> _targetCharacters = new List<Character>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (!_targetCharacters.Contains(other.GetComponent<Character>()))
            {
                _targetCharacters.Add(other.GetComponent<Character>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (_targetCharacters.Contains(other.GetComponent<Character>()))
            {
                _targetCharacters.Remove(other.GetComponent<Character>());
            }
        }
    }

    public void Activate()
    {
        foreach (Character character in _targetCharacters)
        {
            character.OnHit(35.0f);
        }
    }
}
