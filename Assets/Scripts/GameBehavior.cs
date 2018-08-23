using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameBehavior : MonoBehaviour {

    private PlayerBehavior player;
    private BackgroundBehavior background;

    private GameObject spawner;

    private Canvas canvas;
    private Button respawnButton;
    private Button abilityButton;
    private RectTransform gameOverPanel;

    public const int ABILITY_TIME_TEXT_DIGITS = 1;
    public const int GLOBAL_TIME_TEXT_DIGITS = 4;

    private TimeClass GlobalClock;
    private TimeClass AbilityClock;

    public const float abilityCooldown = 10f;

    public float startTime;
    /*
    private Text timeText;
    private float[] time = new float[4];
    private float startTime;

    private Text coolDownTimeText;
    private float[] cdTime = new float[2];
    private float cdStartTime;
    */

    public GameObject floorPrefab;
    public GameObject obstaclePrefab;
    public GameObject enemyPrefab;

    public bool gameOver = false;

    private int floorCount = 2;

    private const int greyColorHex = 0x808080;
    private const int defaultColorHex = 0xFFFFFF;

    private Color greyColor = new Color(128f, 128f, 128f, 255f);
    private Color defaultColor = new Color(255f, 255f, 255f, 255f);

    public class TimeClass
    {
        private Text timeText;
        private GameBehavior m_game;
        private float m_startTime;
        private float m_coolDown;
        private float[] time;

        private int type = 0;

        public TimeClass(string canvasTextName, Canvas canvas, GameBehavior game)
        {
            switch (canvasTextName)
            {
                case "TimeText":
                    type = GLOBAL_TIME_TEXT_DIGITS;
                    time = new float[type];
                    timeText = canvas.gameObject.transform.Find(canvasTextName).GetComponent<Text>();
                    break;

                case "AbilityTimeText":
                    type = ABILITY_TIME_TEXT_DIGITS;
                    time = new float[type];
                    timeText = canvas.gameObject.transform.Find("AbilityButton").Find(canvasTextName).GetComponent<Text>();
                    break;

                default:
                    time = new float[4];
                    Debug.Log("Invalid canvasTextName");
                    break;
            }

            m_game = game;
            
            m_startTime = game.startTime;
        }

        public void AbilityTimeSet(float time, float cooldown)
        {
            m_startTime = time;
            m_coolDown = cooldown;
            timeText.gameObject.SetActive(true);
        }

        public void PrintTimeAbility()
        {

            time[0] = m_coolDown - (Time.time - m_startTime);

            if(Time.time - m_startTime >= m_coolDown)
            {
                m_game.AbilityOffCoolDown();
                timeText.gameObject.SetActive(false);
            }

            timeText.text = Mathf.Floor(time[0] + 1).ToString();

        }

        public void PrintTimeGlobal()
        {

            time[0] = Time.time - m_startTime;

            if (time[0] > 10)
            {
                m_startTime = Time.time;
                time[0] = 0;
                time[1]++;
                if (time[1] > 5)
                {
                    time[1] = 0;
                    time[2]++;
                    if (time[2] > 10)
                    {
                        time[2] = 0;
                        time[3]++;
                    }
                }
            }

            timeText.text = Mathf.Floor(time[3]).ToString() + Mathf.Floor(time[2]).ToString() + ":" + Mathf.Floor(time[1]).ToString() + Mathf.Floor(time[0]).ToString();

        }

        public void PrintTime()
        {
            if (type == GLOBAL_TIME_TEXT_DIGITS)
            {
                PrintTimeGlobal();
            }
            else if (type == ABILITY_TIME_TEXT_DIGITS)
            {
                PrintTimeAbility();
            }
        }

    }

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        background = GameObject.Find("Background").GetComponent<BackgroundBehavior>();
        spawner = GameObject.Find("Spawner");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        gameOverPanel = canvas.transform.Find("Panel").GetComponent<RectTransform>();
        respawnButton = gameOverPanel.transform.Find("RespawnButton").GetComponent<Button>();
        abilityButton = canvas.transform.Find("AbilityButton").GetComponent<Button>();

        abilityButton.onClick.AddListener(delegate { player.CastOrb(); });
        respawnButton.onClick.AddListener(delegate { RestartScene(); });

        startTime = Time.time;

        GlobalClock = new TimeClass("TimeText", canvas, this);
        AbilityClock = new TimeClass("AbilityTimeText", canvas, this);
    }
	
	// Update is called once per frame
	void Update () {
        background.SetSpeed(player.GetSpeed());
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Instantiate(obstaclePrefab, spawner.transform);
        }
        /*
        //if(floorCount < 3)
        {
            Instantiate(floorPrefab, spawner.transform).GetComponent<FloorBehavior>().Init(150, 20);
            floorCount++;
        }
        */

        if (!gameOver)
        {
            GlobalClock.PrintTime();
            if (player.abilityCooldown)
            {
                AbilityClock.PrintTime();
            }
        }
	}

    public float GetSpeed()
    {
        return (player.GetSpeed());
    }

    public void DecFloor()
    {
        //floorCount--;
        Instantiate(floorPrefab, spawner.transform).GetComponent<FloorBehavior>().Init(150, 20);
    }

    public void EndGame()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOver = true;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawner.transform);
    }

    public void AbilityOffCoolDown()
    {
        player.abilityCooldown = false;
        abilityButton.interactable = true;
    }

    public void AbilityOnCoolDown()
    {
        player.abilityCooldown = true;
        AbilityClock.AbilityTimeSet(Time.time, abilityCooldown);
        abilityButton.interactable = false;
    }
}
