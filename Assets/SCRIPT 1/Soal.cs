using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soal : MonoBehaviour
{
  public GameObject originalGameObject;
    //Tempat soal angka
    public GameObject var1;  
    public GameObject operand;  
    public GameObject var2;
    public GameObject result;

    //Tempat soal gambar buah
    public GameObject imageSoal1;
    public GameObject imageSoal2; 
    public GameObject imageSoal3;
    
    //Uncomment bawah buat nanti kalau ada papan skor
    /*public GameObject papanSkor;*/

    //buat munculin feedback kalau salah atau benar atau game selesai
    public GameObject feedBenar,feedSalah,selesai;  
    //Audio kalau salah benar dan skor tinggi baru
    public AudioSource m_Benar,m_Salah,m_SkorTinggiBaru;

    //Nilai pertama soal
    private int lowFirstValue = 1;
    private int highFirstValue = 9;
    private int firstValue;

    //Nilai kedua soal
    private int lowSecondValue = 1;
    private int highSecondValue = 9;
    private int SecondValue;

    //Jawaban soal
    public int finalValue;
    public int answerValue=1;

    //List sprite angka ama buah
    public GameObject[] m_Angka;
    public GameObject[] m_PapanBuah;
    public GameObject[] m_PapanBuah2;
    public Sprite[] m_Operand;



    //Attribute buat operand ama skor
    public bool isAdd = false;
    public bool isSub = false;
    private int skor = 0;
    private int skorTinggi=0;
    private bool isNewHScore = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        // if(PlayerPrefs.HasKey("skorTinggi"))
        // {
        //     skorTinggi = PlayerPrefs.GetInt("skorTinggi");
        // }
        generateSoal();
        isiAngkaSoal();
        isiImageSoal();
    }

    public void generateSoal()
    {
        var i = Random.Range(0, 2);

        //Kalau +
        if (i == 0)
        {
            firstValue = Random.Range(lowFirstValue, highFirstValue);
            highSecondValue = 9 - firstValue;
            SecondValue = Random.Range(lowSecondValue, highSecondValue);
            finalValue = firstValue + SecondValue;
            isAdd = true;
        }

        //Kalau -
        if (i == 1)
        {
            firstValue = Random.Range(lowFirstValue+1, highFirstValue);
            highSecondValue = firstValue-1;
            SecondValue = Random.Range(lowSecondValue, highSecondValue);
            finalValue = firstValue - SecondValue;
            isSub = true;
        }
    }

    public void isiAngkaSoal()
    {
        //Ambil posisi angka
        Vector3 PositionAngka1 = var1.transform.position;
        Vector3 PositionAngka2 = var2.transform.position;
        Vector3 PositionAngka3 = result.transform.position;

        //Hapus prefab, inisiate prefab baru dengan posisi angka prefab dulu, jadikan prefab baru menjadi atribute var
        Destroy(var1.gameObject);
        GameObject angka1 = Instantiate(m_Angka[firstValue-1], PositionAngka1, Quaternion.identity);
        var1 = angka1;

        Destroy(var2.gameObject);
        GameObject angka2 = Instantiate(m_Angka[SecondValue-1], PositionAngka2, Quaternion.identity);
        var2 = angka2;

        Destroy(result.gameObject);
        GameObject angka3 = Instantiate(m_Angka[answerValue-1], PositionAngka3, Quaternion.identity);
        result = angka3;
        
        //Operand
        if(isAdd)
        {
            operand.gameObject.GetComponent<SpriteRenderer>().sprite = m_Operand[0];
        }
        if(isSub)
        {
            operand.gameObject.GetComponent<SpriteRenderer>().sprite = m_Operand[1];
        }

    }

    public void isiImageSoal()
    {
        //Ambil posisi buah
        Vector3 PositionBuah1 = imageSoal1.transform.position;
        Vector3 PositionBuah2 = imageSoal2.transform.position;
        Vector3 PositionBuah3 = imageSoal3.transform.position;

        //Hapus prefab, inisiate prefab baru dengan posisi buah prefab dulu, jadikan prefab baru menjadi atribute var
        Destroy(imageSoal1.gameObject);
        GameObject buah1 = Instantiate(m_PapanBuah[firstValue-1], PositionBuah1, Quaternion.identity);
        imageSoal1 = buah1;

        Destroy(imageSoal2.gameObject);
        GameObject buah2 = Instantiate(m_PapanBuah[SecondValue-1], PositionBuah2, Quaternion.identity);
        imageSoal2 = buah2;

        Destroy(imageSoal3.gameObject);
        GameObject buah3 = Instantiate(m_PapanBuah2[answerValue-1], PositionBuah3, Quaternion.identity);
        imageSoal3 = buah3;
    }

    public void updateAnswer()
    {        
        Vector3 PositionAngka3 = result.transform.position;
        Vector3 PositionBuah3 = imageSoal3.transform.position;
        if(answerValue!=9)
        {
            answerValue ++;
        }
        
        Destroy(result.gameObject);
        GameObject angka3 = Instantiate(m_Angka[answerValue-1], PositionAngka3, Quaternion.identity);
        result = angka3;

        Destroy(imageSoal3.gameObject);
        GameObject buah3 = Instantiate(m_PapanBuah2[answerValue-1], PositionBuah3, Quaternion.identity);
        imageSoal3 = buah3;

    }

    public void updateAnswer2()
    {        
        Vector3 PositionAngka3 = result.transform.position;
        Vector3 PositionBuah3 = imageSoal3.transform.position;
        if(answerValue!=1)
        {
            answerValue --;
        }
        
        Destroy(result.gameObject);
        GameObject angka3 = Instantiate(m_Angka[answerValue-1], PositionAngka3, Quaternion.identity);
        result = angka3;

        Destroy(imageSoal3.gameObject);
        GameObject buah3 = Instantiate(m_PapanBuah2[answerValue-1], PositionBuah3, Quaternion.identity);
        imageSoal3 = buah3;

    }

    // public void jawab(){
    //     int jawaban = int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
    //     Debug.Log(jawaban);
    //     if (jawaban == finalValue) 
    //     {   
    //         m_Benar.Play();
    //         feedBenar.SetActive(false);
    //         feedBenar.SetActive(true);
    //         skor+=5;
    //         if(skor>skorTinggi)
    //         {
    //             isNewHScore = true;
    //             skorTinggi=skor;
    //             PlayerPrefs.SetInt("skorTinggi",skorTinggi);
    //         }
    //         papanSkor.GetComponentInChildren<Text>().text = skor.ToString();
    //         isAdd = false;
    //         isSub = false;
    //         generateSoal();
    //         isiTextSoal();
    //         isiImageSoal();
    //         isiJawabButton();
    //     }
    //     else{
    //         m_Salah.Play();
    //         Text[] textSelesai;
    //         feedSalah.SetActive(false);
    //         feedSalah.SetActive(true);
    //         if(isNewHScore)
    //         {
    //             m_SkorTinggiBaru.Play();
    //         }
    //         selesai.SetActive(true);
    //         textSelesai = selesai.GetComponentsInChildren<Text>();
    //         string skorAkhir = "Skor : " + skor;
    //         string skorTinggiAkhir = "Skor Tertinggi : " + skorTinggi;
    //         textSelesai[1].text=skorAkhir;
    //         textSelesai[2].text=skorTinggiAkhir;
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
