using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieces : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _ambil, _lempar;

    private bool _angkat, _placed;
    private Vector2 _offset, _originalPosition;
    private PuzzleSlot puzzleSlot;

    public void Init(PuzzleSlot slot)
    {
        //_renderer.sprite = slot.Renderer.sprite;
        puzzleSlot = slot;
    }

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_placed) return;
        if (!_angkat) return;

        var mousePosition = GetMousePos();
        transform.position = mousePosition - _offset;
    }

    void OnMouseDown()
    {
        _angkat = true;
        _source.PlayOneShot(_ambil);

        _offset = GetMousePos() - (Vector2)transform.position;
    }
    private void OnMouseUp()
    {
        if (Vector2.Distance(transform.position, puzzleSlot.transform.position) < 1)
        {
            transform.position = puzzleSlot.transform.position;
            puzzleSlot.Placed();
            _placed = true;
        }
        else
        {
            transform.position = _originalPosition;
            _angkat = false;
        }
        _source.PlayOneShot(_lempar);
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
