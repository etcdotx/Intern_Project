using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum state {
    alive,
    dead
}

public class EnemyStats : MonoBehaviour
{
    public string words;
    public TextMeshPro text;
    public GameObject player;
    public float basescore;
    public int eindx;
    public int type;//0=spawn, 1=child spawner, 2 = spliterrspawner spawn have score while child dont 
    public GameObject _emptyWord;
    public GameObject[] enemy; //0= mob, 1= spawner, 2=splitter, 3 = spawner child, 4 = splitter child
    public float[] speed;
    public float[] eMultipier;//
    private state states;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player != null)
        {
            if (type != 2)
            {
                Vector2 dir = player.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            }
            transform.Translate(0, -speed[eindx] * Time.deltaTime, 0f);
        }

        if (basescore > 0)
        {
            basescore = basescore - 0.3f * Time.deltaTime;
        }
    }

    public void SetWord(string word, int _type)
    {
        eindx = 0;
        type = _type;
        text = GetComponent<TextMeshPro>();
        words = word;
        text.text = word;
        text.color = Color.white;

        if (type == 0)
        {
            if (word.Length >= 9)
            {
                eindx = 2;
            }
            else if (word.Length >= 6 && word.Length < 9)
            {
                eindx = 1;
            }
            else
            {
                eindx = 0;
            }
           
        }
        else
        {
            basescore = 0;
            if (type == 1) {
                enemy[1].SetActive(false);
                eindx = 3;
            } else if (type == 2) {
                enemy[2].SetActive(false);
                eindx = 4;
            }

        }
        states = state.alive;
        enemy[eindx].SetActive(true);
        if (eindx == 1)
        {
            StartCoroutine(spawnningSpawner());
        }
        else if (eindx == 2) {
            StartCoroutine(spawningShotgun());
        }
    }

    public void RemoveLetter()
    {
        text.text = text.text.Remove(0, 1);
        text.color = Color.red;
    }

    public void RemoveWord()
    {
        enemy[eindx].tag = "target";
        GameManager.shoot(this.gameObject);
    }

    public int CalculateScore()
    {
        int total = Mathf.RoundToInt(basescore * eMultipier[eindx]);
        return total;
    }

    public EnemyStats spawnChild()
    {
        GameObject obj = Instantiate(_emptyWord);
        obj.transform.SetParent(null);
        EnemyStats stat = obj.GetComponent<EnemyStats>();
        return stat;
    }

    IEnumerator spawnningSpawner()
    {
        while (this.states == state.alive)
        {
            yield return new WaitForSeconds(10f);
            string _word;
            bool same;
            do
            {
                
                same = false;
                _word = WordGenerator.RandomizeWord();
                foreach (Word Xword in GameManager.GetList())
                {
                    if ((_word == Xword.word))
                    {
                        same = true;
                    }
                }
            } while (_word.Length > 4 || same);

            Word word = new Word(_word, spawnChild(), 1);
            GameManager.UpdateWordList(word);
        }
    }

    public EnemyStats spawnChildShotgun(float deg)
    {
        
        GameObject obj = Instantiate(_emptyWord, new Vector3(transform.position.x,transform.position.y - 0.3f,transform.position.z),transform.rotation);
        obj.transform.SetParent(null);
        obj.transform.rotation = Quaternion.Euler(0, 0, deg);
        EnemyStats stat = obj.GetComponent<EnemyStats>();
        return stat;
    }

    IEnumerator spawningShotgun() {
        while (this.states == state.alive)
        {
            string _word;
            bool same;
            float _deg = 85f;
            for (int i = 0; i < 5; i++)
            {
                do
                {
                    same = false;
                    _word = WordGenerator.RandomizeWordAlphabet();
                    foreach (Word Xword in GameManager.GetList())
                    {
                        if ((_word == Xword.word))
                        {
                            same = true;
                        }
                    }
                } while (same);
                Word word = new Word(_word, spawnChildShotgun(_deg), 2);
                _deg -= 42.5f;
                GameManager.UpdateWordList(word);
            }
            yield return new WaitForSeconds(30f);
        }
    }

}
