using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEditor;
using System.IO;

public class WordGenerator : MonoBehaviour
{
    public TextAsset txt;
    public TextAsset txta;

    private static string[] word;
    private static string[] wordAlphabet;

    private void Awake()
    {
        char[] n= { '\n' };
        word = txt.text.Split(n,System.StringSplitOptions.RemoveEmptyEntries);
        wordAlphabet = txta.text.Split(n, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public static string RandomizeWord() {
        int randomindex = Random.Range(0, word.Length);
        string randomWord = word[randomindex];
        return randomWord;
    }
    public static string RandomizeWordAlphabet()
    {
        int randomindex = Random.Range(0, wordAlphabet.Length);
        string randomWord = wordAlphabet[randomindex];
        return randomWord;
    }
}
