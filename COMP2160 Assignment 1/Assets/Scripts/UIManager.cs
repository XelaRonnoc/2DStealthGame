using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Text score;
    [SerializeField] private Text highestScore;
    [SerializeField] private GameObject gameOverPanel;

    private PlayerMove scoreNum;
    static private UIManager instance; // is a singleton
    static public UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogError("There is not UIManager in the scene.");
            }
            return instance;
        }
    }

    static private int highScore;

    public void Awake()
    {
        if(instance != null) // keep newest instance
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {   

        scoreNum = player.GetComponent<PlayerMove>();
        highestScore.text = "High Score: " + (highScore);
        score.text = "Score: " + (scoreNum.getScore());
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {  
        highestScore.text = "High Score: " + (highScore);
        score.text = "Score: " + (scoreNum.getScore());
    }

    public void reloadScene(){ // on scene reload after loss (restart button pressed)
        if (scoreNum.getScore() > highScore)
        {
            highScore = scoreNum.getScore();
        }
        gameOverPanel.SetActive(false);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        Time.timeScale = 1.0f;
        
    }

    public int getHighScore(){
        return highScore;
    }

    public void gameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
