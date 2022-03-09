using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DD_Apel_Keranjang : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _ambil, _lempar;

    private bool _angkat;
    private Vector2 _offset, _originalPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        _originalPosition = transform.position;
    }

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
        transform.position = _originalPosition;
        _angkat = false;
        _source.PlayOneShot(_lempar);
    }


    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
