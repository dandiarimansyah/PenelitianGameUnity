using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PoinManager : MonoBehaviour
{
    public static PoinManager instance;

    [SerializeField] private Soal soal;

    public Text textPoinBenar;
    public Text textPoinSalah;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Atur Poin Awal
        textPoinBenar.text = soal.poinBenar.ToString();
        textPoinSalah.text = soal.poinSalah.ToString();
    }

    public void UpdateText()
    {
        textPoinBenar.text = soal.poinBenar.ToString();
        textPoinSalah.text = soal.poinSalah.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
