using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropApel : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _ambil, _lempar;

    private bool _angkat;
    private Vector2 _offset, _originalPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        _angkat = false;
        _source.PlayOneShot(_lempar);
        rb.gravityScale = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Buah")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject.tag == "ResetBuah" || collision.gameObject.name != "WadahApel")
        {
            Debug.Log("out");
            transform.position = _originalPosition;
            rb.gravityScale = 0f;
        }
        else
        {
            Debug.Log("masuk");
        }
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
