using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameStatus { Menu, Game ,HighScore};
public class MenuScripts : MonoBehaviour {
    Animator anim;
    enum Buttons { Play, High ,Back};
    
    Buttons buttons;
    public GameStatus gameStatus;
    // Use this for initialization
    short buttonNumber;
    public static MenuScripts instance;

    public Text highestScore;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        anim = GetComponent<Animator>();
        anim.Play("allHide");
        PlayButton();
    }

    private void PlayButton()
    {
        gameStatus = GameStatus.Menu;
        buttons = Buttons.Play;
        MakeAllFalse();
        anim.SetBool("play", true);
        buttonNumber = 0;
    }


    private void HighButton()
    {
        //if (gameStatus != GameStatus.Menu)
           // return;

        gameStatus = GameStatus.Menu;
        buttons = Buttons.High;
        MakeAllFalse();
        anim.SetBool("high", true);
        buttonNumber = 1;
    }

    private void BackButton()
    {
        Debug.Log("BackButton()");
        MakeAllFalse();
        gameStatus = GameStatus.HighScore;
        anim.SetBool("back", true);
        buttonNumber = 2;
    }

    private void HighScoreText()
    {
        gameStatus = GameStatus.HighScore;
        MakeAllFalse();
        highestScore.text = PlayerPrefs.GetInt("HIGHSCORE") + " Killed";
        anim.SetBool("text", true);
        Debug.Log("text "+anim.GetBool("text"));
    }

    public void MakeAllFalse()
    {
        if (gameStatus != GameStatus.HighScore)
            anim.SetBool("text", false);

        anim.SetBool("play", false);
        anim.SetBool("high", false); 
        anim.SetBool("allHide", false);
        anim.SetBool("back", false);
        buttonNumber = -1;
    }

    public void UpDown(short num)
    {
        //Debug.Log("UpDown "+num);

        if (num==0 )
        {
            PlayButton();
        }
        else if (num == 1 && gameStatus == GameStatus.Menu)
        {
            HighButton();
        }
        else if (num == 2 && gameStatus == GameStatus.HighScore)
        {
            BackButton();
        }
    }

    
    public void ButtonPressed()
    {
        Debug.Log("ButtonPressed() "+ buttonNumber);
        if (buttonNumber == 0)
        {
            gameStatus = GameStatus.Game;
            MakeAllFalse();
            GameManager.instance.MakeAllReady();
            anim.SetBool("allHide", true);
        }
        else if(buttonNumber == 1)
        {
            HighScoreText();
        }
        else if (buttonNumber == 2)
        {
            Debug.Log("Back btn");
            HighButton();
        }
    }
    
    public void ActiveUICanvas()
    {
        gameObject.SetActive(true);
        anim.Play("allHide");
        PlayButton();
    }
}
