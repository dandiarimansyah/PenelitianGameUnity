using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public char value;
    public bool _placed=false;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _pasangSound;

    public void Placed()
    {
        _placed = true;
        _source.PlayOneShot(_pasangSound);
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

    void Start()
    {

    }

    void Update()
    {
        
    }
}
