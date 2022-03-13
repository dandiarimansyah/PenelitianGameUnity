using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<PuzzlePieces> piecePrefabs;
    [SerializeField] private List<PuzzleSlot> slotPrefabs;

    //[SerializeField] private PuzzlePieces piecePrefabs;
    [SerializeField] private Transform slotParent, pieceParent;

    void Start()
    {
        //Spawn();
    }

    void Spawn()
    {
        var listSlot = slotPrefabs.Take(4).ToList();
        var listPiece = piecePrefabs.Take(4).ToList();

        for (int i = 0; i < 4; i++)
        {
            var spawnSlot = Instantiate(listSlot[i], slotParent.GetChild(i).position, Quaternion.identity);
            
            var spawnPiece = Instantiate(listPiece[i], pieceParent.GetChild(i).position, Quaternion.identity);

            //spawnPiece.Init(spawnSlot);
        }

    }

        // Update is called once per frame
    void Update()
    {
        
    }
}
