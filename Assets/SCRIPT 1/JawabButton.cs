using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JawabButton : MonoBehaviour
{
    [SerializeField] private Soal soal;
    // Start is called before the first frame update

    void Start()
    {
    }

    public void JawabSoal()
    {
        if (soal.answerValue == soal.finalValue) 
        {   
            //feedback benar
            // m_Benar.Play();
            // feedBenar.SetActive(false);
            // feedBenar.SetActive(true);
            
            //skoring
            // skor+=5;
            // if(skor>skorTinggi)
            // {
            //     isNewHScore = true;
            //     skorTinggi=skor;
            //     PlayerPrefs.SetInt("skorTinggi",skorTinggi);
            // }
            //papanSkor.GetComponentInChildren<Text>().text = skor.ToString();
            
            soal.isAdd = false;
            soal.isSub = false;
            soal.answerValue=1;
            soal.generateSoal();
            soal.isiAngkaSoal();
            soal.isiImageSoal();
        }
        else{
            Debug.Log("Salaaah");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
