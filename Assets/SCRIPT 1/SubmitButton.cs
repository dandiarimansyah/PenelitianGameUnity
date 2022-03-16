using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    public float waktuAlert = 0.8f;
    private float waktu;
    private bool deleteAlert = false;

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

    // Update is called once per frame
    void Update()
    {
        if (deleteAlert)
        {
            HapusAlert(waktuAlert);
        }
    }

    public void JawabSoal()
    {
        // Kondisi Benar
        if (soal.answerValue == soal.finalValue) 
        {
            _source.PlayOneShot(_benar);
            AlertBenar.SetActive(true);

            soal.poinBenar++;

            soal.mulaiGame();

        }

        //Kondisi Salah
        else
        { 
            _source.PlayOneShot(_salah);
            soal.poinSalah++;

            AlertSalah.SetActive(true);
        }

        PoinManager.instance.UpdateText();

        // Hapus Alert
        deleteAlert = true;

    }

    private void HapusAlert(float waktuAlert)
    {
        waktu += Time.deltaTime;

        if (waktu >= waktuAlert)
        {
            AlertSalah.SetActive(false);
            AlertBenar.SetActive(false);
            deleteAlert = false;
            waktu = 0f;

        }
    }


}
