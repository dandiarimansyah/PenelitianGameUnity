using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KuisManager : MonoBehaviour
{
    public Animator A1, A2, A3, GameOverAnim;

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
    [SerializeField] private AudioClip _benar, _salah, _gameover, _nextSoal;
    [SerializeField] private AudioClip[] m_audioAngka;

    List<int> listMuncul = new List<int>();
    List<int> JenisTerpilih = new List<int>();
    List<int> TanganTerpilih = new List<int>();

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
    private int soalSebelumnya = -1;

    int[] urutanAcak = new int[3];
    private float waktuNext = 1.8f;

    //Variabel GameOver
    [SerializeField] private GameObject PapanGameOver;
    [SerializeField] private GameObject PauseMenu;

    void Start()
    {
        Cursor.SetCursor(cursorImage, new Vector2(30,10), CursorMode.Auto);
        
        listMuncul.Clear();
        
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator PermainanSelesai()
    {
        //Muncul Papan Game Over
        PapanGameOver.SetActive(true);
        _source.PlayOneShot(_gameover);
        GameOverAnim.SetTrigger("Trigger");
        yield return new WaitForSeconds(1.2f);
        AudioListener.pause = true;
    }

    public void MulaiGame()
    {   
        countSoal++;
        jawabanAngka = 0;
        isWin = false;
        JenisTerpilih.Clear();
        TanganTerpilih.Clear();

        //Menentukan Random Kendaraan

        if (countSoal - 1 == jumlah)
        {
            soalSebelumnya = kendaraanTerpilih;
            listMuncul.Clear();
            kendaraanTerpilih = RandomAll(jumlah, listMuncul, soalSebelumnya);
        }
        else
        {
            kendaraanTerpilih = RandomAll(jumlah, listMuncul);
        }

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

            switch (jawabanDitekan)
            {
                case 0:
                    A1.SetTrigger("Trig_1");
                    break;
                case 1:
                    A2.SetTrigger("Trig_2");
                    break;
                case 2:
                    A3.SetTrigger("Trig_3");
                    break;
            }

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

            //Permainan Selesai
            if (countSoal >= jumlahSoal)
            {
                StartCoroutine(PermainanSelesai());
            }
            else
            {
                _source.PlayOneShot(_nextSoal);
                yield return new WaitForSeconds(0.3f);
                MulaiGame();
            }
        }
        else
        {
            m_AlertSalah[jawaban].SetActive(true);
            yield return new WaitForSeconds(waktuNext);
            m_AlertSalah[jawaban].SetActive(false);
        }
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
