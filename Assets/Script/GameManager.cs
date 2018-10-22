using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState{
    play,
    notplaying
 }

public class GameManager : MonoBehaviour {
    public int level =1;
    public int countEnemy;
    public int minLength;
    public int maxLength;
    public int ammo;
    public static int HighScore = 0;
    public GameState gstate;
    public GameObject player;
    public GameObject bullet;
    public GameObject winMenu;
    public TextMeshProUGUI accuracytxtW;
    public TextMeshProUGUI scoretxtW;
    public GameObject losMenu;
    public TextMeshProUGUI scoretxtL;
    public GameObject bomb;
    public EnemySpawn Espawner;
    public ScoreManager Smanager;
    public List<Word> words;

    private float totalketikanMust = 0;
    private float totaldiketik = 0;
    private static GameManager instance;
    private bool WordActivated;
    private Word activeWord;
    private int WordFinished = 0;
    private int score = 0;
    private int currentEnemy1 = 0;
    private int currentEnemy2 = 0;
    private int maxEnemy1 = 2;
    private int maxEnemy2 = 1;
   
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gstate = GameState.notplaying;
       resume();
        Play();
    }

    private void Play()
    {
        gstate = GameState.play;
        StartCoroutine(spawnWord());
    }

    IEnumerator spawnWord() {
        for (int i = 0; i < countEnemy; i++) {
            yield return new WaitForSeconds(3f);
            if (gstate ==GameState.play){
                WordAdding();
            }
        }
    }

    public void Press(string letter) {
        TypeLetter(letter);
    }

    public void WordAdding()
    {
        string _word;
        bool same;
        do
        {
            same = false;
            _word = WordGenerator.RandomizeWord();
            foreach (Word Xword in words) {
                if ((_word == Xword.word))
                {
                    same = true;
                }
                if ((_word.Length >= 9 && currentEnemy2 == maxEnemy2) || ((_word.Length >= 6 && _word.Length < 9) && currentEnemy1 == maxEnemy1)) {
                    same = true;
                }
            }
        } while ((_word.Length < minLength || _word.Length > maxLength) || same);

        if (_word.Length >= 9) { currentEnemy2++; } else if (_word.Length >= 6 && _word.Length < 9) { currentEnemy1++; }

        Word word = new Word(_word, Espawner.SpawnEnemy(), 0);
        totalketikanMust += _word.Length;
        totaldiketik += _word.Length;
        words.Add(word);
    }

    public static void updateTotalKetikan(int tambahan) {
        instance.totalketikanMust += tambahan;
        instance.totaldiketik += tambahan;
    }

    public static List<Word> GetList() {
        return instance.words;
    }

    public static void UpdateWordList(Word word) {
        instance.words.Add(word);
    }

    public static void RemoveWord(GameObject collided) {
        string _word = collided.GetComponent<EnemyStats>().words;
        Word temp= null;
        foreach (Word xword in GetList()) {
            if (_word == xword.word) {
                temp = xword;
                break;
            }
        }
       
        if (temp.stat.type == 0) {
            temp.stat.basescore = 0;
            instance.WordFinished++;
            if (temp.stat.eindx == 1) { instance.currentEnemy1--; }
            else if (temp.stat.eindx == 2) { instance.currentEnemy2--; }
        }
        if (instance.activeWord == temp)
        {
            instance.WordActivated = false;
        }
        GetList().Remove(temp);
        Destroy(collided);
        if (instance.WordFinished == instance.countEnemy && GetList().Count == 0) { instance.Win(); }
    }

    public void TypeLetter(string letter) {
        if (WordActivated)
        {
            if (activeWord.NextLetter() == letter) {
                if (activeWord.stat.type == 0)
                {
                    Smanager.UpdateComboBar(activeWord.NextLetter() == letter);
                }
                activeWord.LetterTyped();
            }
            else {
                totaldiketik++;
                Smanager.UpdateComboBar(false);
            }
        }
        else {
            bool foundmatch = false;
            foreach (Word word in words) {
                if (word.NextLetter() == letter)
                {
                    if (word.stat.type == 0)
                    {
                        Smanager.UpdateComboBar(word.NextLetter() == letter);
                    }
                    WordActivated = true;
                    activeWord = word;
                    word.LetterTyped();
                    foundmatch = true;
                    break;
                }
            }
            if (!foundmatch) {
                totaldiketik++;
                Smanager.UpdateComboBar(false);

            }
        }
        if (WordActivated && activeWord.WordComplete()) {
            score = Smanager.calculate(score, activeWord.stat.CalculateScore());
            if (activeWord.stat.type == 0) {
                if (activeWord.stat.eindx == 1) {
                    currentEnemy1--;
                } else if (activeWord.stat.eindx == 2)
                {
                    currentEnemy2--;
                }
                WordFinished++;
            }
            WordActivated = false;
            words.Remove(activeWord);
            if (WordFinished == countEnemy && words.Count == 0) { Win(); }
        }
    }

    public static void shoot(GameObject target){
        Vector2 dir = target.transform.position - instance.player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        instance.player.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        Vector3 bulletspawn = instance.player.transform.position + new Vector3(0, 0.4f, 0);
        GameObject bullets = Instantiate(instance.bullet, bulletspawn, instance.player.transform.rotation);
        bullets.GetComponent<Bullet>().getTarget(target);
    }

    public void Win(){
        winMenu.SetActive(true);
        gstate = GameState.notplaying;
        float accuracy = ( totalketikanMust / totaldiketik) * 100f;
        scoretxtW.text = "Score      : " + score;
        accuracytxtW.text = "Accuracy : " + accuracy + "%";
        if (score > HighScore) {
            HighScore = score;
        }
    }

    public static void Lose(){
        instance.gstate = GameState.notplaying;
        instance.losMenu.SetActive(true);
        instance.scoretxtL.text = "Score      : " + instance.score;
        if (instance.score > HighScore)
        {
            HighScore = instance.score;
        }
    }

    public void NextLevel(){
        Smanager.UpdateComboBar(false);
        level++;
        Smanager.Lvltxt(level); 
        countEnemy += 3;
        WordFinished = 0;
        totaldiketik = 0;
        totalketikanMust = 0;
        maxLength++;
        gstate = GameState.play;
        StartCoroutine(spawnWord());
    }

    public void Bomb(){
        if (ammo > 0)
        {
            ammo--;
            Smanager.updateAmmo(ammo);
            StartCoroutine(BombAway());
            
        }
    }

    IEnumerator BombAway() {
        GameObject _bomb = Instantiate(bomb, player.transform);
        _bomb.transform.parent = null;
        Animator anima= _bomb.GetComponent<Animator>();
        anima.SetBool("Expanding", true);
        yield return new WaitForSeconds(1.2f);
        Destroy(_bomb);
    }

    public void Retry() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void pause() {
        Time.timeScale = 0f;
        gstate = GameState.notplaying;
    }

    public void resume() {
        Time.timeScale = 1f;
        gstate = GameState.play;
    }

    public static void SetHighScore(int _HS)
    {
        HighScore = _HS;
    }


}
