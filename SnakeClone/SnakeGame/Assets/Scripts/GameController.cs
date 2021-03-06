﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject snakePrefab;
    public Snake tail;
    public Snake head;
    public int NESW;
    public Vector2 nextPos;
    public int maxSize;
    public int currSize;
    public int xBound;
    public int yBound;
    public GameObject foodPrefab;
    public GameObject currFood;
    public int score;
    private int highScore;
    public float speed = 4.0f;

    private Vector2 origin;

    void TailFunction()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }
	// Use this for initialization
	void Start () {
        InvokeRepeating("TimerInvoke",0, 0.2f);
        FoodFunctions();
        highScore = PlayerPrefs.GetInt("HighScore");
	}

    private void OnEnable()
    {
        Snake.hit += hit;
    }

    private void OnDisable()
    {
        Snake.hit -= hit;
    }

    void TimerInvoke()
    {
        Movement();
        if (currSize >= maxSize)
        {
            TailFunction();
        }
        else
        {
            currSize++;
        }
    }
	
	// Update is called once per frame
	void Update () {
        ChangeDirection();
	}

    void Movement()
    {
        GameObject temp;
        nextPos = head.transform.position;
        switch (NESW)
        {
            case 0://NORTH
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1://EAST
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2://DOWN
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3://LEFT
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;
        }
        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
        head.SetNext(temp.GetComponent<Snake>());
        head = temp.GetComponent<Snake>();

        return;
    }

    void FoodFunctions()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos),transform.rotation);
        StartCoroutine(CheckRender(currFood));
    }

    IEnumerator CheckRender(GameObject g)
    {
        yield return new WaitForEndOfFrame();
        if(g.GetComponent<Renderer>().isVisible == false)
        {
            if (g.tag == "Food")
            {
                Destroy(g);
                FoodFunctions();
            }
        }
    }

    void ChangeDirection()
    {
        if(NESW != 2 && Input.GetKeyDown(KeyCode.W))
        {
            NESW = 0;
        }
        if(NESW != 3 && Input.GetKeyDown(KeyCode.D))
        {
            NESW = 1;
        }
        if(NESW !=0 && Input.GetKeyDown(KeyCode.S))
        {
            NESW = 2;
        }
        if(NESW!=1 && Input.GetKeyDown(KeyCode.A))
        {
            NESW = 3;
        }
        if(Input.touchCount > 0)
        {
            Touch t = Input.touches[0];
            if(t.phase == TouchPhase.Began)
            {
                origin = t.position;
            }else if (t.phase == TouchPhase.Ended)
            {
                float dx = t.position.x - origin.x;
                float dy = t.position.y - origin.y;
                if (dx < 1f && NESW!=1) //left movement or A code
                {
                    NESW = 3;
                }
                else if(dx>1f && NESW != 3) //right movement or D code
                {
                    NESW = 1;
                }
                else if (dy > 1f && NESW!=2) { //up
                    NESW = 0;
                }
                else if(dy<1f && NESW != 0) //down
                {
                    NESW = 2;
                }
            }
        }
    }

    void hit(string sent)
    {
        if(sent == "Food")
        {
            FoodFunctions();
            maxSize++;
            IncreaseScore();
            //score++;
        }
        if (sent=="Snake" || sent=="Border")
        {
            CancelInvoke("TimerInvoke");
            Debug.Log("GAME OVER");
            GameOver();
        }
    }

    void IncreaseScore()
    {
        var textScore = GameObject.Find("Score").GetComponent<Text>();
        score++;
        textScore.text = score.ToString();
    }

    void GameOver()
    {
        Destroy(gameObject);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        SceneManager.LoadScene("GameOver");
    }
}
