using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieces : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _ambil, _lempar;

    private bool _angkat;
    private Vector2 _offset, _originalPosition;
    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
        /*if (Vector3.Distance(transform.position, soal.BoxApel3.transform.position) < 3)
        {
            soal.tambahApel();
        }*/
        transform.position = _originalPosition;
        _angkat = false;
        _source.PlayOneShot(_lempar);
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
