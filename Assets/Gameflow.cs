using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gameflow : MonoBehaviour {

    public PlayerHealth playerHealth;
    public EnemyHealth enemyHealth;
    public Image gameOver;
    public Image gameOverText;
    public Image keyBoard;
    public Image keyBoardText;
    public Image paused;
    public Image win;
    public bool inPause = false;
    public bool inGameOver = false;
    static bool inRetry = false;
    float time_to_fade;
    int counter;

	// Use this for initialization
	void Start () {
        gameOver.enabled = false;
        gameOverText.enabled = false;
        keyBoard.enabled = false;
        keyBoardText.enabled = false;
        paused.enabled = false;
        win.enabled = false;
        Cursor.visible = false;
        time_to_fade = 0.5f;
        counter = 0;
    }

    // Update is called once per frame
    void Update () {

        //Keyboard Menu
        if (!inRetry)
        {
            if (counter == 10)
            {
                GameStart();
            }
            if (keyBoard.enabled)
            {
                //If "space" pressed, disable keyboard menu and text 
                if (Input.GetKeyDown("space") || (Input.touchCount > 0))
                {
                    keyBoard.enabled = false;
                    keyBoardText.enabled = false;
                }
            }
        }
        
        //Pause Menu
        if ((!inGameOver) && (Input.GetKeyDown("space") || (Input.touchCount > 0)))
        {
            //Enable or disable pause menu
            if (inPause)
            {
                Time.timeScale = 1;
                paused.enabled = false;
                inPause = false;
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0;
                paused.enabled = true;
                inPause = true;
                Cursor.visible = true;
            }
        }

        //Gameover Menu
        if (playerHealth.isDead)
        {
            GameOver();
        }

        //Win
        if (enemyHealth.currentHealth <= 0)
        {
            Win();
        }

        //Exit Game
        if (Input.GetKey("escape"))
            Application.Quit();

        counter++;
    }

    //Enable keyboard menu and text 
    void GameStart()
    {
        Time.timeScale = 0;
        inPause = true;
        keyBoard.enabled = true;
        keyBoardText.enabled = true;
    }

    void GameOver()
    {
        Time.timeScale = 0;
        gameOver.enabled = true;
        gameOverText.enabled = true;
        inGameOver = true;
        
        //press "space" to retry level
        if (Input.GetKeyDown("space"))
        {
            inRetry = true;
            Application.LoadLevel(0);
            Time.timeScale = 1;
        }
    }

    void Win()
    {
        Time.timeScale = 0;
        win.enabled = true;
        inPause = true;
        inGameOver = true;

        //press "space" to reset level
        if (Input.GetKeyDown("space"))
        {
            inRetry = false;
            Application.LoadLevel(0);
            Time.timeScale = 1;
        }
    }
}
