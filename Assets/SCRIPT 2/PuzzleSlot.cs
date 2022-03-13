using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    public SpriteRenderer Renderer;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _pasangSound;

    public void Placed()
    {
        _source.PlayOneShot(_pasangSound);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
