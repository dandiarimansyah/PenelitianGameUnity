using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KuisManager : MonoBehaviour
{
    [SerializeField] Texture2D cursorImage;

    [SerializeField] private Transform posKumpulan, posJawaban, posAlert;

    public GameObject GambarSoal;
    public GameObject[] m_Kumpulan;
    public GameObject[] m_Kendaraan;
    public Sprite[] m_Jari;
    public Sprite[] m_GambarSoal;
    public GameObject tanganKiri;
    public GameObject tanganTengah;
    public GameObject tanganKanan;

    [SerializeField] private GameObject[] m_AlertBenar;
    [SerializeField] private GameObject[] m_AlertSalah;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah, _gameover;
    [SerializeField] private AudioClip[] m_audioAngka;

    HashSet<int> listMuncul = new HashSet<int>();
    HashSet<int> JenisTerpilih = new HashSet<int>();
    HashSet<int> TanganTerpilih = new HashSet<int>();
    HashSet<int> simpanUrutan = new HashSet<int>();

    //private List<int> JenisTerpilih = new List<int>();
    public int jumlahSoal = 5;
    private int countSoal;
    private int kendaraanTerpilih;
    private int jumlah;
    private int jumlahKumpulan;
    private int randomAngka;
    private int temp;
    private int jawabanDitekan;
    private bool isWin = false;

    private int jawabanAngka;
    private int angkaSebelum;

    int[] urutanAcak = new int[3];
    private float waktuNext = 1.2f;

    //Variabel GameOver
    [SerializeField] private GameObject PapanGameOver;
    [SerializeField] private GameObject PauseMenu;

    void Start()
    {
        Cursor.SetCursor(cursorImage, new Vector2(30,10), CursorMode.ForceSoftware);
        
        PapanGameOver.SetActive(false);
        PauseMenu.SetActive(false);
        System.Array.Clear(urutanAcak, 0, urutanAcak.Length);
        jumlahKumpulan = posKumpulan.childCount;

        jumlah = m_Kendaraan.Count();
        countSoal = 0;
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

        countSoal++;
        jawabanAngka = 0;
        isWin = false;
        JenisTerpilih.Clear();
        JenisTerpilih.TrimExcess();
        TanganTerpilih.Clear();
        TanganTerpilih.TrimExcess();

        //Menentukan Random Kendaraan       
        kendaraanTerpilih = RandomAll(7, listMuncul);
        JenisTerpilih.Add(kendaraanTerpilih);

        //Ganti Gambar Soal
        GambarSoal.gameObject.GetComponent<SpriteRenderer>().sprite = m_GambarSoal[kendaraanTerpilih];

        SusunGambarAtas(jumlah, kendaraanTerpilih);
        TanganTerpilih.Add(jawabanAngka);

        AturPilihanTangan();
    }

    public void SusunGambarAtas(int jumlah, int kendaraanTerpilih)
    {
        //Menentukan 4 Jenis Kendaraan
        for (int i = 1; i < 4; i++)
        {
            randomAngka = RandomAll(jumlah, JenisTerpilih);
        }

        //Penempatan Random
        for (int i = 0; i < jumlahKumpulan; i++)
        {
            randomAngka = Random.Range(0, jumlah);

            // Kiri kanan tidak sama
            while (!JenisTerpilih.Contains(randomAngka) || (randomAngka == angkaSebelum && randomAngka == kendaraanTerpilih))
            {
                randomAngka = Random.Range(0, jumlah);
            }
            angkaSebelum = randomAngka;

            if (randomAngka == kendaraanTerpilih)
            {
                jawabanAngka++;
            }

            m_Kumpulan[i].gameObject.GetComponent<SpriteRenderer>().sprite = m_GambarSoal[randomAngka];

        }
    }

    public void AturPilihanTangan()
    {
        //Menentukan Tangan berapa saja
        for (int i = 0; i < 2; i++)
        {
            randomAngka = Random.Range(1, 11);
            while (TanganTerpilih.Contains(randomAngka))
            {
                randomAngka = Random.Range(1, 11);
            }

            TanganTerpilih.Add(randomAngka);
        }

        //Acak Urutan
        int[] array = new int[3];
        TanganTerpilih.CopyTo(array);
        urutanAcak = pengacakArray(array);

        tanganKiri.GetComponent<Image>().sprite = m_Jari[urutanAcak[0]-1];
        tanganTengah.GetComponent<Image>().sprite = m_Jari[urutanAcak[1]-1];
        tanganKanan.GetComponent<Image>().sprite = m_Jari[urutanAcak[2]-1];
        
    }
    public void KlikTombol(int jawabanDitekan)
    {
        if (urutanAcak[jawabanDitekan] == jawabanAngka)
        {
            _source.PlayOneShot(m_audioAngka[jawabanAngka-1]);
            isWin = true;
        }
        else
        {
            _source.PlayOneShot(_salah);
            isWin = false;
        }

        var jawaban = jawabanDitekan;
        StartCoroutine(AlertMuncul(jawaban, isWin));
    }

    IEnumerator AlertMuncul(int jawaban, bool isWin)
    {
        if (isWin)
        {
            m_AlertBenar[jawaban].SetActive(true);
            yield return new WaitForSeconds(waktuNext);
            m_AlertBenar[jawaban].SetActive(false);

            //Ati ati komputer meleduk
            MulaiGame();
        }
        else
        {
            m_AlertSalah[jawaban].SetActive(true);
            yield return new WaitForSeconds(waktuNext);
            m_AlertSalah[jawaban].SetActive(false);
        }
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

    int[] pengacakArray(int[] array)
    {
        for (int i = 0; i < TanganTerpilih.Count-1; i++)
        {
            int rnd = Random.Range(i, TanganTerpilih.Count);
            temp = array[rnd];
            array[rnd] = array[i];
            array[i] = temp;
        }
        return array;
    }
}
