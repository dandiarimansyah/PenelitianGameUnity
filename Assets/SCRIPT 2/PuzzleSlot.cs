using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public char value;
    public bool _placed = false;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _suara;

    public void Suara(AudioClip _suaraHuruf)
    {
        _source.PlayOneShot(_suaraHuruf);
    }

    public void Placed()
    {
        _placed = true;
        //_source.PlayOneShot(_suara);
    }

    public void setSpriteNValue(char newValue, Sprite newSprite)
    {
        value = newValue;
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public char getValue()
    {
        return value;
    }
}
