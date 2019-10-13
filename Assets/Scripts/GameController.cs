using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;
    public Pet playerAttributes;
    [HideInInspector] public bool playerTurn = true;

    private int level = 1;
    BoardManager boardManager;

    List<Enemy> enemies;

    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;
    bool enemyMoving;

    bool doingSetup;
    GameObject levelImage;
    Text levelText;

    GameObject startButton;

    // 初始化
    private void Awake()
    {

        Debug.Log("Awake");
        if( instance == null)
        {
            instance = this;
            playerAttributes = new Pet("Player");
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        boardManager = GetComponent<BoardManager>();
        InitGame();
        Debug.Log("GameController Awake");
    }

    // 调用boardManager脚本的SetupScene方法来生成关卡
    public void InitGame()
    {
        Debug.Log("initGame");

        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        startButton = GameObject.Find("StartButton");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + GameController.instance.level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardManager.SetupScene(GameController.instance.level);

        startButton.SetActive(false);

        enabled = true;
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        startButton.SetActive(false);
        doingSetup = false;
    }

    void Update()
    {
        if(playerTurn || enemyMoving ||doingSetup)
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    IEnumerator MoveEnemies()
    {
        Debug.Log("enemy turn, enmies count:"+enemies.Count);
        enemyMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for( int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];
            if (enemy.isActiveAndEnabled)
            {
                enemies[i].MoveEnemy();
                yield return new WaitForSeconds(enemies[i].moveTime);
            }
        }
        enemyMoving = false;
        playerTurn = true;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void GameOver()
    {
        enabled = false;

        StartCoroutine(showMenu());
        
    }

    IEnumerator showMenu()
    {
        yield return new WaitForSeconds(1f);

        levelText.text = "After " + level + " days, you starved";
        levelImage.SetActive(true);
        startButton.SetActive(true);
    }

    private void OnLevelWasLoaded(int level1)
    {
        level++;
        Debug.Log("level++: " + level);
        Debug.Log("OnLevelWasLoaded level++, level:"+level);
        InitGame();
    }

}


