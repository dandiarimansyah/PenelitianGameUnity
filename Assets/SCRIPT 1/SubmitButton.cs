using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    private float waktuNext = 2f;
    private bool isWin, isClick = false;

    public Animator A1,A2,A3,A4,A5;

    [SerializeField] private Soal soal;
    [SerializeField] private GameObject AlertBenar;
    [SerializeField] private GameObject AlertSalah;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah, _nextSoal;
    [SerializeField] private AudioClip[] m_audioAngka;
    [SerializeField] private AudioClip[] m_suaraOperand;

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
            //AlertBenar.SetActive(true);
            //yield return new WaitForSeconds(waktuNext);
            //soal.mulaiGame();
            //isClick = false;

            AlertBenar.SetActive(true);
            
            yield return new WaitForSeconds(0.7f);
            _source.PlayOneShot(m_audioAngka[soal.firstValue - 1]);
            A1.SetTrigger("Trig_1");
            
            yield return new WaitForSeconds(1.5f);
            if (soal.isAdd)
            {
                _source.PlayOneShot(m_suaraOperand[0]);
            }
            else
            {
                _source.PlayOneShot(m_suaraOperand[1]);
            }
            A2.SetTrigger("Trig_2");
            
            yield return new WaitForSeconds(1.5f);
            _source.PlayOneShot(m_audioAngka[soal.SecondValue - 1]);
            A3.SetTrigger("Trig_3");
            
            yield return new WaitForSeconds(1.5f);
            _source.PlayOneShot(m_suaraOperand[2]);
            A4.SetTrigger("Trig_4");

            yield return new WaitForSeconds(1.5f);
            _source.PlayOneShot(m_audioAngka[soal.finalValue - 1]);
            A5.SetTrigger("Trig_5");
            
            yield return new WaitForSeconds(2f);
            _source.PlayOneShot(_nextSoal);
            soal.mulaiGame();
            isClick = false;

            //IlustrasiJawaban.SetTrigger("Trig_1");

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
