using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour {
    public AudioSource bgm;
    public Slider musicbar;
    public TextMeshProUGUI scoretxt;
    public int HS;

    private void Start()
    {
        load();
        musicbar.value = bgm.volume;
    }

    private void load() {
        if (File.Exists(Application.persistentDataPath + "/info.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/info.dat", FileMode.Open);
            settingVariable sets = (settingVariable)bf.Deserialize(file);
            file.Close();

            bgm.volume = sets.slidervalue;
            if (GameManager.FindObjectOfType<GameManager>() != null)
            {
                GameManager.SetHighScore(sets.highscore);
            }
            else {
                scoretxt.text = "HighScore : " + sets.highscore;
                HS = sets.highscore;
            }
        }
    }

    public void save(){

        settingVariable sets = new settingVariable();
        sets.slidervalue = musicbar.value;
        if (GameManager.FindObjectOfType<GameManager>() != null)
        {
            sets.highscore = GameManager.HighScore;
        }
        else {
            sets.highscore = HS;
        }
       

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +"/info.dat");

        bf.Serialize(file, sets);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit() {
        Application.Quit();
    }
}

[System.Serializable]
class settingVariable {
    public float slidervalue;
    public int highscore;
}
