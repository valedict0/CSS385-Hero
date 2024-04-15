using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{

    [SerializeField]
    private Game game = null;

    private Text text = null;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = "";
        text.text += "Hero Status: " + game.heroStatus + "\n";
        text.text += "Touched Enemy: " + game.enemyTouched + "\n";
        text.text += "Egg Count: " + game.eggCount + "\n";
        text.text += "Enemy Count: " + game.enemyCount + "\n";
        text.text += "Enemy Destroyed: " + game.enemyDestroyed + "\n";
    }
}
