using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> SpriteHuruf;
    [SerializeField] private List<Sprite> SpriteSlotHuruf;
    [SerializeField] private List<PuzzlePieces> piecePrefabs;
    [SerializeField] private List<PuzzleSlot> slotPrefabs;

    [SerializeField] private TextAsset txtKataHuruf;


    //Variabel GameOver
    [SerializeField] private GameObject PapanGameOver;
    [SerializeField] private GameObject[] DeletedObject;
    private bool isGameOver = false;

    [SerializeField] private GameObject AlertBenar;
    [SerializeField] private GameObject AlertSalah;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _benar, _salah, _gameover;


    private List<PuzzlePieces> listPieces = new List<PuzzlePieces>();
    private List<PuzzleSlot> listSlots = new List<PuzzleSlot>();
    private int sumPuzzle = 0;
    private int urutanTerpilih = 0;
    private int[] sudahMuncul;
    HashSet<int> listMuncul = new HashSet<int>();


    private char tempChar;
    private bool isSpawned = false;
    public int jumlahSoal = 10;
    private int countSoal = 0;
    private float waktuNext = 1f;


    [SerializeField] private Transform slotParent, pieceParent, posHewan;
    public GameObject hewan;
    public GameObject[] semuaHewan;

    void Start()
    {
        PapanGameOver.SetActive(false);
        AlertBenar.SetActive(false);
        AlertSalah.SetActive(false);
        Spawn();
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    char[] Shuffler(string word)
    {
        char[] array = word.ToCharArray();
        int n = array.Length-1; //n=4
        for (int i = 0; i < n - 1; i++) //0-3 
        {
            int rnd = Random.Range(i, n);
            tempChar = array[rnd];
            array[rnd] = array[i];
            array[i] = tempChar;
        }
        return array;
    }

    int getIndexForSprite(char letter)
    {
        int index = char.ToUpper(letter) - 64;
        return index;
    }

    void PermainanSelesai()
    {
        //Hapus Object
        /*for (int i = 0; i < DeletedObject.Length; i++)
        {
            Destroy(DeletedObject[i].gameObject);
        }*/

        //Muncul Papan Game Over
        PapanGameOver.SetActive(true);
        _source.PlayOneShot(_gameover);

        isGameOver = true;
    }

    private int RandomAngka(List<string> listOfWords)
    {
        var range = Enumerable.Range(0, listOfWords.Count).Where(i => !listMuncul.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(0, listOfWords.Count - listMuncul.Count);
        return range.ElementAt(index);
    }

    void Spawn()
    {
        //Permainan Selesai
        if (countSoal >= jumlahSoal)
        {
            PermainanSelesai();
            return;
        }


        //Ini tinggal diganti sesuaiin nilai randomSumHuruf tinggal pake if else;
        var content = txtKataHuruf.text;

        //Randoming Kata
        var AllWords = content.Split('\n');
        List<string> listOfWords = new List<string>(AllWords);
        //string word = getRandomWord(listOfWords); //Dapat urutan kata random

        urutanTerpilih = RandomAngka(listOfWords);

        string word = listOfWords[urutanTerpilih];

        //sudahMuncul[countSoal] = urutanTerpilih;
        listMuncul.Add(urutanTerpilih);

        //Jumlah Kata
        sumPuzzle = word.Length - 1;

        //Shuffling Susunan Kata
        char[] arraysShuffledWord = Shuffler(word); //U,D,U,K
        string shuffledWord = new string(arraysShuffledWord); //"UDUK"
        Debug.Log(shuffledWord);


        // Munculin gambar hewan
        Vector3 posHewan2 = hewan.transform.position;

        Destroy(hewan.gameObject);
        GameObject gambarHewan = Instantiate(semuaHewan[urutanTerpilih], posHewan2, Quaternion.identity);
        hewan = gambarHewan;
        

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
            spawnSlot.setSpriteNValue(arrayWord[i],SpriteSlotHuruf[getIndexForSprite(arrayWord[i])-1]);
            spawnPiece.setSpriteNValue(arraysShuffledWord[i],SpriteHuruf[getIndexForSprite(arraysShuffledWord[i])-1]);

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
        AlertBenar.SetActive(true);

        yield return new WaitForSeconds(waktuNext);

        AlertBenar.SetActive(false);

        DestroyListSlots();
        DestroyListPieces();
        listPieces.Clear();
        listSlots.Clear();
        Spawn();

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if(isSpawned)
        {
            int sum = 0;
            foreach(PuzzleSlot slot in listSlots)
            {
                if(slot._placed)
                {
                    sum++;
                }
            }
            // Debug.Log(sum);
            if(sum == sumPuzzle)
            {
                isSpawned = false;
                _source.PlayOneShot(_benar);

                StartCoroutine(NextGame());

            }
        }
        //Buat jaga2 tadi bug gak ke spawn
        /*else if (!isSpawned || ((listPieces == null) && (listPieces.Any()) ))
        {
            StartCoroutine(NextGame());
        }*/
    }
}
