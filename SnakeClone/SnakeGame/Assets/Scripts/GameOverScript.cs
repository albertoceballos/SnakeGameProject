using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {
    private int highscore;
	// Use this for initialization
	void Start () {
        highscore = PlayerPrefs.GetInt("HighScore");
        var score = GameObject.Find("HighScore").GetComponent<Text>();
        score.text = highscore.ToString() ;
	}
}
