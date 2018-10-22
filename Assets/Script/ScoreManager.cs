using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {
    public Slider combobar;
    public TextMeshProUGUI combotxt;
    public TextMeshProUGUI ammotxt;
    public TextMeshProUGUI lvltxt;
    private int comboValue = 0;
    private int comboX = 1;

    public void updateAmmo(int ammo) {
        ammotxt.text = "Bomb : " + ammo;
    }

    public int calculate(int _score,int _total) {
        int score = _score + _total*comboX;
        return score;
    }

    public void UpdateComboBar(bool _same) {
        if (_same)
        {
            comboValue += 10;
            if (comboValue == 100)
            {
                comboX++;
                comboValue = 0;
            }
            combobar.value = comboValue;
            combotxt.text = "Combo : " + comboX + "X";
        }
        else {
            comboValue = 0;
            comboX = 1;
            combobar.value = comboValue;
            combotxt.text = "Combo : " + comboX + "X";
        }
    }

    public void Lvltxt(int level)
    {
        lvltxt.text = "Level " + level;
    }
}
