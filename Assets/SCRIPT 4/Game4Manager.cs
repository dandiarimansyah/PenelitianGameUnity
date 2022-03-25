using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game4Manager : MonoBehaviour
{
    public GameObject KategoriSoal;
    public GameObject TulisanSoal;
    public GameObject[] m_Gambar;
    public GameObject[] m_Border;
    public GameObject[] m_Button;

    public Sprite[] m_KategoriSoal;
    public Sprite[] m_TulisanSoal;
    public Sprite[] m_SemuaHewan;
    public Sprite[] m_Hewan_Darat;
    public Sprite[] m_Hewan_Laut;
    public Sprite[] m_Hewan_Tanduk;
    public Sprite[] m_Hewan_Kaki2;
    public Sprite[] m_Hewan_Kaki4;
    public Sprite BorderBenar;
    public Sprite BorderSalah;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah, _gameover;

    HashSet<int> listMuncul = new HashSet<int>();
    HashSet<int> HewanTerpilih = new HashSet<int>();
    HashSet<int> Ditempati = new HashSet<int>();
    private Sprite[] ArrayTerpilih;


    public int jumlahSoal = 5;
    private int countSoal;
    private int jumlahJawaban;
    private int jumlahHewan;
    //private int countJumlahJawaban;
    private int kategoriTerpilih;

    //Variabel GameOver
    [SerializeField] private GameObject PapanGameOver;

    void Start()
    {
        countSoal = 0;
        jumlahHewan = m_SemuaHewan.Count();
        MulaiGame();
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void PermainanSelesai()
    {
        //Muncul Papan Game Over
        PapanGameOver.SetActive(true);
        _source.PlayOneShot(_gameover);
    }

    public void MulaiGame()
    {
        //Permainan Selesai
        if (countSoal >= jumlahSoal)
        {
            PermainanSelesai();
            return;
        }

        //---------------------------------
        //countSoal++;

        jumlahJawaban = 0;
        //countJumlahJawaban = 0;
        listMuncul.Clear();
        listMuncul.TrimExcess();
        HewanTerpilih.Clear();
        HewanTerpilih.TrimExcess();
        Ditempati.Clear();
        Ditempati.TrimExcess();

        //Menentukan Soal
        kategoriTerpilih = Random.Range(1, 6);
        KategoriSoal.gameObject.GetComponent<SpriteRenderer>().sprite = m_KategoriSoal[kategoriTerpilih-1];
        TulisanSoal.gameObject.GetComponent<SpriteRenderer>().sprite = m_TulisanSoal[kategoriTerpilih-1];
        
        //Menentukan Array dari kategori yang terpilih
        switch (kategoriTerpilih)
        {
            case 1:
                ArrayTerpilih = m_Hewan_Darat;
                break;
            case 2:
                ArrayTerpilih = m_Hewan_Laut;
                break;
            case 3:
                ArrayTerpilih = m_Hewan_Tanduk;
                break;
            case 4:
                ArrayTerpilih = m_Hewan_Kaki2;
                break;
            case 5:
                ArrayTerpilih = m_Hewan_Kaki4;
                break;
        }

        //Jumlah hewan yang benar
        jumlahJawaban = Random.Range(3, 6);
        Debug.Log("jumlah = "+ jumlahJawaban);

        SusunGambarJawaban(jumlahHewan, kategoriTerpilih, jumlahJawaban, ArrayTerpilih);
    }

    public int RandomAll(int jumlah, HashSet<int> Array)
    {
        var angka = 0;
        do
        {
            angka = Random.Range(0, jumlah);
        } while (Array.Contains(angka));

        Array.Add(angka);

        return angka;
    }

    public void SusunGambarJawaban(int jumlahHewan, int kategoriTerpilih, int jumlahJawaban, Sprite[] ArrayTerpilih)
    {
        //var angkaSebelum = -1;
        var randomTempat = 0;
        var randomHewan = 0;
        var tempatTersisa = 10 - jumlahJawaban;
        var jmlHewanKategori = ArrayTerpilih.Count();
        Debug.Log("JumlahArray = " + jmlHewanKategori);

        for (int i = 0; i < jumlahJawaban; i++)
        {
            randomTempat = RandomAll(10, Ditempati);            
            randomHewan = RandomAll(jmlHewanKategori, HewanTerpilih);

            Debug.Log("Tempat = "+randomTempat);
            Debug.Log("Hewan = "+ randomHewan);

            m_Gambar[randomTempat].GetComponent<Image>().sprite = ArrayTerpilih[randomHewan];

        }
        
    }

}
