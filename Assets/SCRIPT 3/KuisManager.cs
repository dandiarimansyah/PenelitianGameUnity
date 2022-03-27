using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KuisManager : MonoBehaviour
{
    [SerializeField] private Transform posKumpulan, posJawaban, posAlert;

    public GameObject GambarSoal;
    public GameObject[] m_Kumpulan;
    public GameObject[] m_Kendaraan;
    public Sprite[] m_Jari;
    public Sprite[] m_GambarSoal;
    public GameObject tanganKiri;
    public GameObject tanganTengah;
    public GameObject tanganKanan;

    [SerializeField] private GameObject[] Alert;
    [SerializeField] private GameObject[] m_AlertBenar;
    [SerializeField] private GameObject[] m_AlertSalah;
    [SerializeField] private GameObject AlertBenar;
    [SerializeField] private GameObject AlertSalah;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah, _gameover;

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

    void Start()
    {
        PapanGameOver.SetActive(false);
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

    private int RandomAngka(List<GameObject> listOfWords)
    {
        var range = Enumerable.Range(0, listOfWords.Count).Where(i => !listMuncul.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(0, listOfWords.Count - listMuncul.Count);
        return range.ElementAt(index);
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
        //var all_kendaraan = m_Kendaraan.ToList();
        //var kendaraanTerpilih = RandomAngka(all_kendaraan);
        
        kendaraanTerpilih = Random.Range(0, 7);

        //listMuncul.Add(kendaraanTerpilih);
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
            randomAngka = Random.Range(0, jumlah);
            while (JenisTerpilih.Contains(randomAngka))
            {
                randomAngka = Random.Range(0, jumlah);
            }

            JenisTerpilih.Add(randomAngka);
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

            //Instantiate(m_Kendaraan[randomAngka], posKumpulan.GetChild(i).position, Quaternion.identity);
            
            // Kiri kanan & Baris tengah tidak sama dengan atas
            /*if (i > 5 && i < 11)
            {
                while (randomAngka == simpanUrutan.ElementAt(i-6) || !JenisTerpilih.Contains(randomAngka) || randomAngka == angkaSebelum)
                {
                    randomAngka = Random.Range(0, jumlah);
                }
            }
            else
            {
                while (!JenisTerpilih.Contains(randomAngka) || randomAngka == angkaSebelum)
                {
                    randomAngka = Random.Range(0, jumlah);
                }
            }*/
            //simpanUrutan.Add(randomAngka);
        }
        Debug.Log("Jawaban = " + jawabanAngka);
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

        Debug.Log("Jari 1 = "+ urutanAcak[0]);
        Debug.Log("Jari 2 = "+ urutanAcak[1]);
        Debug.Log("Jari 3 = "+ urutanAcak[2]);

        tanganKiri.GetComponent<Image>().sprite = m_Jari[urutanAcak[0]-1];
        tanganTengah.GetComponent<Image>().sprite = m_Jari[urutanAcak[1]-1];
        tanganKanan.GetComponent<Image>().sprite = m_Jari[urutanAcak[2]-1];
        
    }
    public void KlikTombol(int jawabanDitekan)
    {
        if (urutanAcak[jawabanDitekan] == jawabanAngka)
        {
            _source.PlayOneShot(_benar);
            isWin = true;
        }
        else
        {
            _source.PlayOneShot(_salah);
            isWin = false;
        }

        //StartCoroutine(NextGame(isWin));
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

    IEnumerator NextGame(bool isWin)
    {
        if (isWin)
        {
            //AlertBenar.SetActive(true);
            GameObject AlertB = Instantiate(AlertBenar, posAlert.GetChild(jawabanDitekan).position, Quaternion.identity);
            Alert[jawabanDitekan] = AlertB;
            Alert[jawabanDitekan].SetActive(true);

            yield return new WaitForSeconds(waktuNext);

            //Ati ati komputer meleduk
            MulaiGame();
        }
        else
        {
            //AlertSalah.SetActive(true);
            GameObject AlertS = Instantiate(AlertSalah, posAlert.GetChild(jawabanDitekan).position, Quaternion.identity);
            Alert[jawabanDitekan] = AlertS;
            Alert[jawabanDitekan].SetActive(true);

            yield return new WaitForSeconds(waktuNext);
        }

        for (int i = 0; i < 3; i++)
        {
            Alert[i].SetActive(false);
        }

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
