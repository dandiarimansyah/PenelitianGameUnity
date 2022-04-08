using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzlePieces : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _ambil, _lempar;

    public AudioClip[] SuaraAbjad;

    public GameObject PM;

    public char value;
    public int posisi;
    private int antrian2;
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

    public void setSpriteNValue(int newPos, char newValue, Sprite newSprite)
    {
        value = newValue;
        posisi = newPos;
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public char getValue()
    {
        return value;
    }

    public int getPosisi()
    {
        return posisi;
    }

    private void Awake()
    {
        _originalPosition = transform.position;

        PM = GameObject.Find("PuzzleManager");
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
            antrian2 = PM.GetComponent<PuzzleManager>().antrian;

            if (Vector2.Distance(transform.position, puzzleSlot.transform.position) < 1 && !puzzleSlot._placed && puzzleSlot.getPosisi() == antrian2)
            {
                int index = ((int)puzzleSlot.getValue() % 32) - 1;
                _source.PlayOneShot(SuaraAbjad[index]); 

                transform.position = puzzleSlot.transform.position;
                puzzleSlot.Placed();
                _placed = true;

                var script = PM.GetComponent<PuzzleManager>();
                script.setAntrian(antrian2 + 1);
            }
        }
        if(!_placed)
        {
            transform.position = _originalPosition;
            _angkat = false;
            _source.PlayOneShot(_lempar);
        }
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
