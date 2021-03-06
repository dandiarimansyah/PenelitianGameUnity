using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public Animator A1, GameOverAnim;
    [SerializeField] Texture2D cursorImage;

    [SerializeField] private List<Sprite> SpriteHuruf;
    [SerializeField] private List<Sprite> SpriteSlotHuruf;
    [SerializeField] private List<PuzzlePieces> piecePrefabs;
    [SerializeField] private List<PuzzleSlot> slotPrefabs;

    [SerializeField] private TextAsset txtKataHuruf;

    //Variabel GameOver
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject PapanGameOver;
    [SerializeField] private GameObject[] DeletedObject;
    [SerializeField] private GameObject AlertBenar;
    [SerializeField] private GameObject AlertSalah;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah, _gameover;

    private List<PuzzlePieces> listPieces = new List<PuzzlePieces>();
    private List<PuzzleSlot> listSlots = new List<PuzzleSlot>();
    private int sumPuzzle = 0;
    private int urutanTerpilih = 0;
    private int[] sudahMuncul;
    List<int> listMuncul = new List<int>();


    private char tempChar;
    private bool isSpawned = false;
    public int jumlahSoal = 10;
    private int countSoal = 0;
    private float waktuNext = 0.6f;
    public int antrian;

    [SerializeField] private Transform slotParent, pieceParent, posHewan;
    public GameObject hewan;
    public Sprite[] m_semuaHewan;
    [SerializeField] private AudioClip[] m_audioHewan;

    void Start()
    {
        Cursor.SetCursor(cursorImage, new Vector2(30, 10), CursorMode.Auto);

        PapanGameOver.SetActive(false);
        PauseMenu.SetActive(false);
        AlertBenar.SetActive(false);
        AlertSalah.SetActive(false);
        Spawn();
    }
    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    char[] Shuffler(string word)
    {
        char[] array = word.ToCharArray();
        int n = array.Length-1; //n=4
        int index1 = array[0];

        for (int i = 0; i < n - 1; i++) //0-3 
        {
            int rnd = Random.Range(i, n);
            tempChar = array[rnd];
            array[rnd] = array[i];
            array[i] = tempChar;
        }

        if (index1 == array[0])
        {
            tempChar = array[0];
            array[0] = array[3];
            array[3] = tempChar;
        }

        return array;
    }

    int getIndexForSprite(char letter)
    {
        int index = char.ToUpper(letter) - 64;
        return index;
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

    private int RandomAngka(List<string> listOfWords)
    {
        var range = Enumerable.Range(0, listOfWords.Count).Where(i => !listMuncul.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(0, listOfWords.Count - listMuncul.Count);
        return range.ElementAt(index);
    }

    public void setAntrian(int angka)
    {
        antrian = angka;
    }

    void Spawn()
    {
        antrian = 0;

        //Ini tinggal diganti sesuaiin nilai randomSumHuruf tinggal pake if else;
        var content = txtKataHuruf.text;

        //Randoming Kata
        var AllWords = content.Split('\n');
        List<string> listOfWords = new List<string>(AllWords);

        urutanTerpilih = RandomAngka(listOfWords);
        string word = listOfWords[urutanTerpilih];

        listMuncul.Add(urutanTerpilih);

        //Jumlah Kata
        sumPuzzle = word.Length - 1;

        //Shuffling Susunan Kata
        char[] arraysShuffledWord = Shuffler(word); //U,D,U,K
        string shuffledWord = new string(arraysShuffledWord); //"UDUK"

        // Munculin gambar hewan
        hewan.gameObject.GetComponent<SpriteRenderer>().sprite = m_semuaHewan[urutanTerpilih];

        //Convert string to array biar bisa di akses di looping bawah
        char[] arrayWord = word.ToCharArray(); //K,U,D,A

        var listSlot = slotPrefabs.Take(sumPuzzle).ToList();
        var listPiece = piecePrefabs.Take(sumPuzzle).ToList();


        //spawning huruf
        for (int i = 0; i < arrayWord.Length-1; i++)
        {
            var spawnSlot = Instantiate(listSlot[i], slotParent.GetChild(i).position, Quaternion.identity);
            var spawnPiece = Instantiate(listPiece[i], pieceParent.GetChild(i).position, Quaternion.identity);

            //Ganti Sprite sesuai dengan kata yang digenerate;
            spawnSlot.setSpriteNValue(i, arrayWord[i],SpriteSlotHuruf[getIndexForSprite(arrayWord[i])-1]);
            spawnPiece.setSpriteNValue(i, arraysShuffledWord[i],SpriteHuruf[getIndexForSprite(arraysShuffledWord[i])-1]);

            //Buat nanti Cek udah placed semua apa belum
            
            listPieces.Add(spawnPiece);
            listSlots.Add(spawnSlot);
        }

        foreach (PuzzlePieces piece in listPieces)
        {
            piece.setListSlot(listSlots);
        }
        isSpawned = true;

        //Tambah Soal
        countSoal++;

    }
    void DestroyListSlots()
    {
        foreach(PuzzleSlot slot in listSlots)
        {
            Destroy(slot.gameObject);
        }
    }

    void DestroyListPieces()
    {
        foreach(PuzzlePieces piece in listPieces)
        {
            Destroy(piece.gameObject);
        }
    }

    IEnumerator NextGame()
    {
        yield return new WaitForSeconds(0.7f);

        AlertBenar.SetActive(true);
        
        yield return new WaitForSeconds(0.4f);
        _source.PlayOneShot(m_audioHewan[urutanTerpilih]);
        A1.SetTrigger("Trigger");

        yield return new WaitForSeconds(1.7f);
        
        //Permainan Selesai
        if (countSoal >= jumlahSoal)
        {
            StartCoroutine(PermainanSelesai());
        }
        else
        {
            _source.PlayOneShot(_benar);
            yield return new WaitForSeconds(waktuNext);
            AlertBenar.SetActive(false);

            DestroyListSlots();
            DestroyListPieces();
            listPieces.Clear();
            listSlots.Clear();
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpawned)
        {
            int sum = 0;
            foreach(PuzzleSlot slot in listSlots)
            {
                if (slot.getPosisi() == antrian)
                {
                    slot.setColor();
                }

                if(slot._placed)
                {
                    sum++;
                }
            }
            // Debug.Log(sum);
            if(sum == sumPuzzle)
            {
                isSpawned = false;

                StartCoroutine(NextGame());

            }
        }
        //Buat jaga2 tadi bug gak ke spawn
        //else if (!isSpawned || ((listPieces == null) && (listPieces.Any())))
        //{
        //    StartCoroutine(NextGame());
        //}
    }
}
