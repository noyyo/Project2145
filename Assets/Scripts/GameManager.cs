using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _i;
    public static GameManager Instance
    {
        get
        {
            if (_i == null)
            {
                _i = FindObjectOfType<GameManager>();
                if (_i == null)
                    Debug.Log("게임 매니저가 없습니다..");
            }
            return _i;
        }
    }
    public GameObject player;
    public GameObject bomb;
    public GameObject bombEffect;

    public bool IsPlaying = false;

    public GameObject PlayerUI;

    public List<GameObject> EnemyPrefabs;
    public List<GameObject> Enemies = new List<GameObject>();

    [Header("# Game Control")]
    public float gameTime;

    [Header("# Player Info")]
    public float playerExp;
    public int playerLevel;
    public int playerKill; 
    public int playerLife = 3;
    public int playerShield = 0;
    public int playerMaxShield = 200;
    public int playerBomb = 3;
    public int playerItem = 5;



    void Awake()
    {
        if (_i == null)
            _i = this;
        else if (_i != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        gameTime = 0;
        Time.timeScale = 1;
        IsPlaying = true;
        StartCoroutine(StartStage());
    }

    void Update()
    {
        // Test Code
        GetExp();
        
    }
    void GameLogic()
    {        
        //while (IsPlaying)
        //{
        //    gameTime += Time.deltaTime;
        //    if (playerHealth == 0)
        //    {
        //        playerLife--;
        //        if (playerLife <= 0)
        //        {
        //            IsPlaying = false;
        //            Time.timeScale = 0;
        //            break;
        //            // GameEnd() 결과창 띄우기
        //        }
        //        playerHealth = playerMaxHealth;
        //    }
        //}
    }
    public void GetExp()
    {
        playerExp++;
        if (playerExp == 100f)
        {
            playerLevel++;
            playerExp = 0;
        }
    }
    public void CreateBomb()
    {
        playerBomb--;
        GameObject _bomb = Instantiate(bomb);
        Bomb newBomb = _bomb.GetComponent<Bomb>();
        newBomb.Move(transform.up);        
    }
    public void Explosion()
    {
        // 화면 중앙 애니메이션
        bombEffect.SetActive(true);
        
    }
    public IEnumerator StartStage()
    {
        yield return StartCoroutine(RightToUpWave(5, 0.8f));
        yield return StartCoroutine(LeftToUpWave(5, 0.8f));
    }
    public IEnumerator RightToUpWave(int enemyCount, float spawnDelay)
    {
        GameObject enemyPrefab = EnemyPrefabs[0];
        int count = 0;
        while (count < enemyCount)
        {
            count++;
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector2(10, -2);
            TopDownEnemyController controller = enemy.GetComponent<TopDownEnemyController>();
            controller.AddMovePattern(MovePatternFactory.CircleMoveXDegree(controller, 6, 120, MovePatternDirection.UpperRight, MovePatternRotation.Clockwise));
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    public IEnumerator LeftToUpWave(int enemyCount, float spawnDelay)
    {
        GameObject enemyPrefab = EnemyPrefabs[0];
        int count = 0;
        while (count < enemyCount)
        {
            count++;
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector2(-10, -2);
            TopDownEnemyController controller = enemy.GetComponent<TopDownEnemyController>();
            controller.AddMovePattern(MovePatternFactory.CircleMoveXDegree(controller, 6, 120, MovePatternDirection.UpperLeft, MovePatternRotation.CounterClockwise));
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
