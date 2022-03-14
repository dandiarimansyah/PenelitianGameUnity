using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> SpriteHuruf;
    [SerializeField] private List<Sprite> SpriteSlotHuruf;
    [SerializeField] private List<PuzzlePieces> piecePrefabs;
    [SerializeField] private List<PuzzleSlot> slotPrefabs;
    
    // [SerializeField] private TextAsset txtKata3Huruf;
    [SerializeField] private TextAsset txtKata4Huruf;
    // [SerializeField] private TextAsset txtKata5Huruf;
    
    private List<PuzzlePieces> listPieces = new List<PuzzlePieces>();
    private List<PuzzleSlot> listSlots = new List<PuzzleSlot>();
    private int sumPuzzle = 0;

    private char tempChar;
    private bool isSpawned = false;

    //[SerializeField] private PuzzlePieces piecePrefabs;
    [SerializeField] private Transform slotParent, pieceParent;

    void Start()
    {
        Spawn();
    }

    string getRandomWord(List<string> listWords)
    {
        int Length = listWords.Count;
        int randomNumber = Random.Range(0, Length);
        return listWords[randomNumber];
    }

    char[] Shuffler(string word)
    {
        char[] array = word.ToCharArray();
        int n = array.Length-1;
        for (int i = 0; i < n - 1; i++) 
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

    void Spawn()
    {

        //Randoming Jumlah Huruf Kata 3 sampai 5
        int randomSumHuruf = Random.Range(3,6);
        // sumPuzzle = randomSumHuruf;
        
        //Ini tinggal diganti sesuaiin nilai randomSumHuruf tinggal pake if else;
        var content = txtKata4Huruf.text;

        sumPuzzle = 4;
        

        //Randoming Kata
        var AllWords = content.Split('\n');
        List<string> listOfWords = new List<string>(AllWords);
        string word = getRandomWord(listOfWords);

        //Shuffling Susunan Kata
        char[] arraysShuffledWord = Shuffler(word);
        string shuffledWord = new string(arraysShuffledWord);
        Debug.Log(shuffledWord);

        //Convert string to array biar bisa di akses di looping bawah
        char[] arrayWord = word.ToCharArray();
        
        //kalau mau satu fungsi ini berarti buat variable prefabs buat 3,4,5 huruf terus var ini masukin if else yang atas
        var listSlot = slotPrefabs.Take(4).ToList();
        var listPiece = piecePrefabs.Take(4).ToList();

        

        //spawning huruf
        for (int i = 0; i < arrayWord.Length-1; i++)
        {
            Debug.Log(arraysShuffledWord[i]);
            var spawnSlot = Instantiate(listSlot[i], slotParent.GetChild(i).position, Quaternion.identity);
            
            var spawnPiece = Instantiate(listPiece[i], pieceParent.GetChild(i).position, Quaternion.identity);

            //Ganti Sprite sesuai dengan kata yang digenerate;
            spawnSlot.setSpriteNValue(arrayWord[i],SpriteSlotHuruf[getIndexForSprite(arrayWord[i])-1]);
            spawnPiece.setSpriteNValue(arraysShuffledWord[i],SpriteHuruf[getIndexForSprite(arraysShuffledWord[i])-1]);

            //Buat nanti Cek udah placed semua apa belum
            
            listPieces.Add(spawnPiece);
            listSlots.Add(spawnSlot);
            Debug.Log("adding piece"+i);
        }
        foreach(PuzzlePieces piece in listPieces)
        {
            piece.setListSlot(listSlots);
        }
        isSpawned = true;

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

    // Update is called once per frame
    void Update()
    {
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
                DestroyListSlots();
                DestroyListPieces();
                listPieces.Clear();
                listSlots.Clear();
                Spawn();
            }
        }
        //Buat jaga2 tadi bug gak ke spawn
        else if (!isSpawned || ((listPieces == null) && (listPieces.Any()) ))
        {
            Spawn();
        }
    }
}
