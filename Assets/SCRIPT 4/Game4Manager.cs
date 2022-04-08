using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game4Manager : MonoBehaviour
{
    [SerializeField] Texture2D cursorImage;

    [SerializeField] private GameObject KategoriSoal;
    [SerializeField] private GameObject TulisanSoal;
    [SerializeField] private GameObject[] m_Gambar;
    [SerializeField] private GameObject[] m_Border;
    [SerializeField] private GameObject[] m_Button;

    [SerializeField] private Sprite[] m_KategoriSoal;
    [SerializeField] private Sprite[] m_TulisanSoal;
    [SerializeField] private Sprite[] m_SemuaHewan;
    [SerializeField] private Sprite[] m_Hewan_Darat;
    [SerializeField] private Sprite[] m_Hewan_Laut;
    [SerializeField] private Sprite[] m_Hewan_Tanduk;
    [SerializeField] private Sprite[] m_Hewan_Kaki2;
    [SerializeField] private Sprite[] m_Hewan_Kaki4;
    [SerializeField] private Sprite[] m_Serangga;

    [SerializeField] private Sprite BorderNormal;
    [SerializeField] private Sprite BorderBenar;
    [SerializeField] private Sprite BorderSalah;
    [SerializeField] private GameObject AlertBenar;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah, _nextGame, _gameover;

    List<int> listMuncul = new List<int>();
    List<int> HewanTerpilih = new List<int>();
    List<int> Ditempati = new List<int>();
    List<int> TempatBenar = new List<int>();
    List<string> NamaHewanTerpilih = new List<string>();

    private Sprite[] ArrayTerpilih;

    public int jumlahSoal = 5;
    private int countSoal;
    private int jumlahJawaban;
    private int jumlahHewan;
    private int soalTersedia;
    private int soalSebelumnya = -1;
    private int countJumlahJawaban;
    private int kategoriTerpilih;

    private float waktuNext = 1f;
    private bool[] isClick = new bool[10];

    //Variabel GameOver
    [SerializeField] private GameObject PapanGameOver;
    [SerializeField] private GameObject PauseMenu;

    void Start()
    {
        Cursor.SetCursor(cursorImage, new Vector2(30, 10), CursorMode.ForceSoftware);

        listMuncul.Clear();

        countSoal = 0;
        jumlahHewan = m_SemuaHewan.Count();
        soalTersedia = m_KategoriSoal.Count();

        PapanGameOver.SetActive(false);
        PauseMenu.SetActive(false);

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
        countSoal++;

        jumlahJawaban = 0;
        countJumlahJawaban = 0;

        NamaHewanTerpilih.Clear();
        HewanTerpilih.Clear();
        Ditempati.Clear();
        TempatBenar.Clear();

        for (int i = 0; i < 10; i++)
        {
            m_Border[i].GetComponent<Image>().sprite = BorderNormal;
        }

        for (int i = 0; i < isClick.Length; i++)
        {
            isClick[i] = false;
        }

        //Menentukan Soal

        if (countSoal - 1 == soalTersedia)
        {
            soalSebelumnya = kategoriTerpilih;
            listMuncul.Clear();
            kategoriTerpilih = RandomAll(soalTersedia, listMuncul, soalSebelumnya);
        }
        else
        {
            kategoriTerpilih = RandomAll(soalTersedia, listMuncul);
        }

        KategoriSoal.gameObject.GetComponent<SpriteRenderer>().sprite = m_KategoriSoal[kategoriTerpilih];
        TulisanSoal.gameObject.GetComponent<SpriteRenderer>().sprite = m_TulisanSoal[kategoriTerpilih];

        //Menentukan Array dari kategori yang terpilih
        switch (kategoriTerpilih)
        {
            case 0:
                ArrayTerpilih = m_Hewan_Darat;
                break;
            case 1:
                ArrayTerpilih = m_Hewan_Laut;
                break;
            case 2:
                ArrayTerpilih = m_Hewan_Tanduk;
                break;
            case 3:
                ArrayTerpilih = m_Hewan_Kaki2;
                break;
            case 4:
                ArrayTerpilih = m_Hewan_Kaki4;
                break;
            case 5:
                ArrayTerpilih = m_Serangga;
                break;
        }

        for (int i = 0; i < ArrayTerpilih.Count(); i++)
        {
            NamaHewanTerpilih.Add(ArrayTerpilih[i].name);
        }

        //Jumlah hewan yang benar
        jumlahJawaban = Random.Range(3, 6);

        SusunGambarJawaban(jumlahJawaban, ArrayTerpilih);
    }

    public int RandomAll(int jumlah, List<int> Array, int sebelumnya = -1)
    {
        var angka = 0;
        do
        {
            angka = Random.Range(0, jumlah);
        } while (Array.Contains(angka) || Array.Contains(sebelumnya));

        Array.Add(angka);

        return angka;
    }

    public void SusunGambarJawaban(int jumlahJawaban, Sprite[] ArrayTerpilih)
    {
        //var angkaSebelum = -1;
        var randomTempat = 0;
        var randomHewan = 0;
        string namaRandomHewan;
        var tempatTersisa = 10 - jumlahJawaban;
        var jmlHewanKategori = ArrayTerpilih.Count();

        for (int i = 0; i < jumlahJawaban; i++)
        {
            randomTempat = RandomAll(10, Ditempati);
            TempatBenar.Add(Ditempati.ElementAt(i));

            randomHewan = RandomAll(jmlHewanKategori, HewanTerpilih);

            m_Gambar[randomTempat].GetComponent<Image>().sprite = ArrayTerpilih[randomHewan];
        }

        for (int i = 0; i < tempatTersisa; i++)
        {
            randomTempat = RandomAll(10, Ditempati);

            do
            {
                randomHewan = Random.Range(0, jumlahHewan);
                namaRandomHewan = m_SemuaHewan[randomHewan].name;
            } while (NamaHewanTerpilih.Contains(namaRandomHewan));

            NamaHewanTerpilih.Add(namaRandomHewan);

            m_Gambar[randomTempat].GetComponent<Image>().sprite = m_SemuaHewan[randomHewan];
        }
    }

    public void KlikTombol(int urutan)
    {
        if (isClick[urutan]) return;

        if (TempatBenar.Contains(urutan))
        {
            _source.PlayOneShot(_benar);
            m_Border[urutan].GetComponent<Image>().sprite = BorderBenar;
            countJumlahJawaban++;

            isClick[urutan] = true;

            if (countJumlahJawaban == jumlahJawaban)
            {
                _source.PlayOneShot(_nextGame);
                StartCoroutine(DelayNextGame());
            }
        }
        else
        {
            _source.PlayOneShot(_salah);
            StartCoroutine(ShowAlertSalah(urutan));
        }
    }

    IEnumerator ShowAlertSalah(int urutan)
    {
        m_Border[urutan].GetComponent<Image>().sprite = BorderSalah;
        yield return new WaitForSeconds(0.5f);
        m_Border[urutan].GetComponent<Image>().sprite = BorderNormal;
    }

    IEnumerator DelayNextGame()
    {
        AlertBenar.SetActive(true);
        yield return new WaitForSeconds(waktuNext);
        AlertBenar.SetActive(false);
        MulaiGame();
    }

}
