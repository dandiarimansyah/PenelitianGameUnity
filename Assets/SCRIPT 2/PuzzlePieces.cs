using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzlePieces : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _ambil, _lempar;

    public char value;
    private bool _angkat, _placed;
    private Vector2 _offset, _originalPosition;
    private List<PuzzleSlot> puzzleSlots = new List<PuzzleSlot>();

    public void setListSlot(List<PuzzleSlot> listSlot)
    {
        puzzleSlots = listSlot;
    }

    public bool isPlaced(){
        return _placed;
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
        IEnumerable<PuzzleSlot> slots = from pSlot in puzzleSlots
                                where pSlot.getValue() == value
                                select pSlot;
                                
        foreach (PuzzleSlot puzzleSlot in slots)
        {
            if (Vector2.Distance(transform.position, puzzleSlot.transform.position) < 1 && !puzzleSlot._placed)
            {
                transform.position = puzzleSlot.transform.position;
                puzzleSlot.Placed();
                _placed = true;
            }
        }
        if(!_placed)
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
