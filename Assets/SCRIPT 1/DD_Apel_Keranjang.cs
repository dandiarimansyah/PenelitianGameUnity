using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DD_Apel_Keranjang : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _ambil, _lempar;

    [SerializeField] private Soal soal;

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

        Cursor.SetCursor(soal.dragCursor, new Vector2(40, 40), CursorMode.ForceSoftware);
        var mousePosition = GetMousePos();

        transform.position = mousePosition - _offset;
    }

    void OnMouseDown()
    {
        Cursor.SetCursor(soal.dragCursor, new Vector2(40, 40), CursorMode.ForceSoftware);

        _angkat = true;
        _source.PlayOneShot(_ambil);

        _offset = GetMousePos() - (Vector2)transform.position;
    }
    private void OnMouseUp()
    {
        if (Vector3.Distance(transform.position,soal.BoxApel3.transform.position) < 3)
        {
            soal.tambahApel();
        }
        Cursor.SetCursor(soal.handCursor, new Vector2(50, 50), CursorMode.ForceSoftware);

        transform.position = _originalPosition;
        _angkat = false;
        _source.PlayOneShot(_lempar);
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnMouseEnter()
    {
        Cursor.SetCursor(soal.handCursor, new Vector2(50, 50), CursorMode.ForceSoftware);
    }
    public void OnMouseExit()
    {
        Cursor.SetCursor(soal.cursorImage, new Vector2(30, 10), CursorMode.ForceSoftware);
    }
}
