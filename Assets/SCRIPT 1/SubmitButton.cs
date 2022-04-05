using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    private float waktuNext = 1f;
    private bool isWin, isClick = false;

    [SerializeField] private Soal soal;
    [SerializeField] private GameObject AlertBenar;
    [SerializeField] private GameObject AlertSalah;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah;

    void Start()
    {
        AlertBenar.SetActive(false);
        AlertSalah.SetActive(false);
    }

    public void JawabSoal()
    {
        if (isClick) return;

        // Kondisi Benar
        if (soal.answerValue == soal.finalValue) 
        {
            _source.PlayOneShot(_benar);
            //AlertBenar.SetActive(true);
            isWin = true;
            isClick = true;
            soal.poinBenar++;
        }
        //Kondisi Salah
        else
        { 
            _source.PlayOneShot(_salah);
            soal.poinSalah++;
            isWin = false;

            //AlertSalah.SetActive(true);
        }

        //Next Spawn Game
        StartCoroutine(NextGame(isWin));

        PoinManager.instance.UpdateText();
    }

    IEnumerator NextGame(bool isWin)
    {
        if (isWin)
        {
            AlertBenar.SetActive(true);
            yield return new WaitForSeconds(waktuNext);
            soal.mulaiGame();
            isClick = false;
        }
        else
        {
            AlertSalah.SetActive(true);
            yield return new WaitForSeconds(waktuNext);
        }

        AlertBenar.SetActive(false);
        AlertSalah.SetActive(false);

    }
}
