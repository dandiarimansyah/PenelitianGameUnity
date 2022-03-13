using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Soal : MonoBehaviour
{
  public GameObject originalGameObject;
    //Tempat soal angka
    public GameObject angkaDummy1;  
    public GameObject angkaDummy2;
    public GameObject result;
    public GameObject operand;  

    //Tempat soal gambar buah
    public GameObject BoxApel1;
    public GameObject BoxApel2; 
    public GameObject BoxApel3;

    //Uncomment bawah buat nanti kalau ada papan skor
    /*public GameObject papanSkor;*/

    //Permainan Selesai
    [SerializeField] private GameObject GameOver;
    public Text textNilaiAkhir;
    //private int nilaiAkhir = 0;

    //Audio Benar, Salah, New High Score
    public AudioSource m_Benar,m_Salah,m_SkorTinggiBaru;

    //Angka Pertama
    private int lowFirstValue = 1;
    private int highFirstValue = 9;
    private int firstValue;

    //Angka Kedua
    private int lowSecondValue = 1;
    private int highSecondValue = 9;
    private int SecondValue;

    //Jawaban soal
    public int finalValue;
    public int answerValue;
    public int jumlahSoal = 10;
    private int countSoal = 0;

    //List sprite angka & buah
    public GameObject[] m_Angka;
    public GameObject[] m_PapanBuah;
    public GameObject[] m_PapanBuah2;
    public Sprite[] m_Operand;

    //Attribute buat operand ama skor
    public bool isAdd;
    public bool isSub;

    // Poin
    public int poinBenar = 0;
    public int poinSalah = 0;
    public int highscoreBenar = 0;
    public bool isNewHScore = false;
    
    // Start is called before the first frame update
    void Start()
    {
        mulaiGame();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mulaiGame()
    {
        //Mulai Awal & Ganti Soal
        if (countSoal < jumlahSoal)
        {
            answerValue = 0;
            isAdd = false;
            isSub = false;

            generateSoal();
            isiAngkaSoal();
            isiImageSoal();

            countSoal++;
        }

        //Game Over
        else
        {
            float totalPoin = poinBenar + poinSalah;
            float nilaiAkhirF = (poinBenar / totalPoin) * 100;
            int nilaiAkhir = (int)nilaiAkhirF;

            //Atur NilaiAkhir
            textNilaiAkhir.text = nilaiAkhir.ToString();

            GameOver.SetActive(true);
        }

    }

    public void generateSoal()
    {
        var i = Random.Range(0, 2);

        //Penjumlahan
        if (i == 0)
        {
            firstValue = Random.Range(lowFirstValue, highFirstValue);
            highSecondValue = 9 - firstValue;
            SecondValue = Random.Range(lowSecondValue, highSecondValue);
            finalValue = firstValue + SecondValue;
            isAdd = true;
        }

        //Pengurangan
        if (i == 1)
        {
            firstValue = Random.Range(lowFirstValue+1, highFirstValue);
            highSecondValue = firstValue;
            SecondValue = Random.Range(lowSecondValue, highSecondValue);
            finalValue = firstValue - SecondValue;
            isSub = true;
        }
    }

    public void isiAngkaSoal()
    {
        //Ambil posisi angka
        Vector3 PositionAngka1 = angkaDummy1.transform.position;
        Vector3 PositionAngka2 = angkaDummy2.transform.position;
        Vector3 PositionAngka3 = result.transform.position;

        //Destroy Dummy, Initiate Prefab
        Destroy(angkaDummy1.gameObject);
        GameObject angka1 = Instantiate(m_Angka[firstValue], PositionAngka1, Quaternion.identity);
        angkaDummy1 = angka1;

        Destroy(angkaDummy2.gameObject);
        GameObject angka2 = Instantiate(m_Angka[SecondValue], PositionAngka2, Quaternion.identity);
        angkaDummy2 = angka2;

        Destroy(result.gameObject);
        GameObject angka3 = Instantiate(m_Angka[answerValue], PositionAngka3, Quaternion.identity);
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
        Vector3 PositionBuah1 = BoxApel1.transform.position;
        Vector3 PositionBuah2 = BoxApel2.transform.position;
        Vector3 PositionBuah3 = BoxApel3.transform.position;

        //Destroy Dummy, Initiate Prefab
        Destroy(BoxApel1.gameObject);
        GameObject buah1 = Instantiate(m_PapanBuah[firstValue], PositionBuah1, Quaternion.identity);
        BoxApel1 = buah1;

        Destroy(BoxApel2.gameObject);
        GameObject buah2 = Instantiate(m_PapanBuah[SecondValue], PositionBuah2, Quaternion.identity);
        BoxApel2 = buah2;

        Destroy(BoxApel3.gameObject);
        GameObject buah3 = Instantiate(m_PapanBuah2[answerValue], PositionBuah3, Quaternion.identity);
        BoxApel3 = buah3;
    }

    public void tambahApel()
    {        
        Vector3 PositionAngka3 = result.transform.position;
        Vector3 PositionBuah3 = BoxApel3.transform.position;

        //Maksimal 9 Apel
        if(answerValue!=9)
        {
            answerValue ++;
        }
        
        Destroy(result.gameObject);
        GameObject angka3 = Instantiate(m_Angka[answerValue], PositionAngka3, Quaternion.identity);
        result = angka3;

        Destroy(BoxApel3.gameObject);
        GameObject buah3 = Instantiate(m_PapanBuah2[answerValue], PositionBuah3, Quaternion.identity);
        BoxApel3 = buah3;

    }

    public void kurangApel()
    {        
        Vector3 PositionAngka3 = result.transform.position;
        Vector3 PositionBuah3 = BoxApel3.transform.position;

        //Minimal 1 Apel
        if(answerValue!=0)
        {
            answerValue --;
        }
        
        Destroy(result.gameObject);
        GameObject angka3 = Instantiate(m_Angka[answerValue], PositionAngka3, Quaternion.identity);
        result = angka3;

        Destroy(BoxApel3.gameObject);
        GameObject buah3 = Instantiate(m_PapanBuah2[answerValue], PositionBuah3, Quaternion.identity);
        BoxApel3 = buah3;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
