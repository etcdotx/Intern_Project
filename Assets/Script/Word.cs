using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Word {
    public string word;
    public EnemyStats stat;
    private int index=0;

    public Word(string _word, EnemyStats _stat,int type)
    {
        word = _word;
        index = 0;
        stat = _stat;
        stat.SetWord(word,type);
    }

    public string NextLetter()
    {
        return word[index].ToString();
    }

    public void LetterTyped() {
        index++;
        stat.RemoveLetter();
    }

    public bool WordComplete() {
        bool complete = (index >= word.Length-1);
        if (complete)
        {
            stat.RemoveWord();
        }
        return complete;
    }

}
