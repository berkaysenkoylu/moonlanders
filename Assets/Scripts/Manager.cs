using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    const int maxLevel = 6; // Add to this as you add new levels
    public int level = 1;
    public GameObject[] levels;
    public GameObject ship;
    private Transform startingPoint;
    public ShipMovement shipMovement;
    public ShipFuel shipFuel;
    public Animator transAnimator;
    public Animator menuAnimator;

    public GameObject MenuObject; // TODO: Remove/Change this later!
    public GameObject GameOverMenu;

    public static Manager instance = null;

    float time;
    bool levelStarted = false;

    //*
    float fuelTimerGameOverDelay = 2f;

    // UI Stuff
    public Text go_timeElapsed;  // Gameover menu
    public Text go_fuelConsumed; // Gameover menu
    public Text tm_timeElapsed; // Transition menu
    public Text tm_fuelConsumed; // Transition menu
    public Text levelIndicator;
    public Text healthText;
    public Text fuelText;
    public Slider shipHealth;
    public GameObject nextLevel;
    public GameObject mainMenu;

    void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }

        ShipMovement.LevelFinished += TransitionToHigherLevel;
        ShipMovement.ShipDamagedBeyondRepair += GameOver;
    }

    private void OnDestroy()
    {
        ShipMovement.LevelFinished -= TransitionToHigherLevel;
    }

    void Update ()
    {
        if (levelStarted)
        {
            levelIndicator.text = string.Format("Level: {0}", level);
            time += Time.deltaTime;

            if(shipFuel.GetFuelAmount() <= 0f)
            {
                fuelTimerGameOverDelay -= Time.deltaTime;
            }

            if (fuelTimerGameOverDelay <= 0f)
            {
                GameOver();

                fuelTimerGameOverDelay = 2.0f;
            }
        }

        if(shipFuel.GetFuelAmount() <= 0)
        {
            fuelText.enabled = true;
        }
        else
        {
            fuelText.enabled = false;
        }

        shipHealth.value = (shipMovement.GetShipHealth()) / shipMovement.maxShipHealth;
        healthText.text = string.Format("Health: {0:#.0} %", ((shipMovement.GetShipHealth()) / shipMovement.maxShipHealth) * 100);
    }

    public bool MaxLevelReached()
    {
        return level == maxLevel;
    }

    public void StartLevel()
    {
        //*
        time = 0f;
        levelStarted = true;

        // If level is not 1, we should also deactivate the previous level
        if(level == 1)
        {
            levels[level - 1].SetActive(true);
        }
        else
        {
            levels[level - 2].SetActive(false);
            levels[level - 1].SetActive(true);
        }

        if (!ship.activeSelf)
            ship.SetActive(true);

        // Reset Parameters
        shipFuel.ResetFuel();

        ship.GetComponent<ShipMovement>().RestartShip();

        startingPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;

        // Spawn ship at the spawn point
        ship.transform.position = startingPoint.position + new Vector3(0f, 2.2f, 0f);
        ship.transform.rotation = Quaternion.identity;
        ship.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Give ship controll back
        ship.GetComponent<ShipMovement>().enabled = true;

        transAnimator.SetBool("levelFinished", false);
        transAnimator.SetBool("newLevelStarted", true); 
    }

    void TransitionToHigherLevel()
    {
        levelStarted = false;

        // Make ship uncontrollable
        ship.GetComponent<ShipMovement>().enabled = false;

        transAnimator.SetBool("newLevelStarted", false);
        transAnimator.SetBool("levelFinished", true);

        if (MaxLevelReached())
        {
            nextLevel.SetActive(false);
            mainMenu.SetActive(true);
        }
        else
        {
            nextLevel.SetActive(true);
            mainMenu.SetActive(false);
        }

        tm_timeElapsed.text = string.Format("Time: {0:#.00} s", time);
        tm_fuelConsumed.text = string.Format("Fuel used: {0:#.0} L", shipFuel.initialFuel - shipFuel.GetFuelAmount());

        // Set current level false
        //levels[level - 1].SetActive(false);

        // Increment level
        if(!MaxLevelReached())
            level++;

        levelIndicator.text = "";

        // Starting a new level is implemented on GUI (via a button)
        // Start a new level...
    }

    // ====================== Just for the Main Menu ========================== //
    public void StartAtLevel(int levelNumber)
    {
        level = levelNumber;

        StartLevel();

        StartCoroutine(StartLevelWithAnim());

        //MenuObject.SetActive(false);
    }
    // ======================================================================== //

    IEnumerator StartLevelWithAnim()
    {
        menuAnimator.SetBool("sceneSelected", true);

        yield return new WaitForSeconds(1.5f);

        menuAnimator.SetBool("sceneSelected", false);

        yield return new WaitForSeconds(0.5f);

        MenuObject.SetActive(false);
    }

    void GameOver()
    {
        levelStarted = false;
        // Bring up the game over screen
        GameOverMenu.SetActive(true);

        // Populate UI elements
        go_timeElapsed.text = string.Format("Time: {0:#.00} s", time);
        go_fuelConsumed.text = string.Format("Fuel used: {0:#.0} L", shipFuel.initialFuel - shipFuel.GetFuelAmount());

        // Deactivate ship
        ship.SetActive(false);
    }

    public void RestartLevel()
    {
        ship.GetComponent<ShipMovement>().RestartShip();

        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        GameOverMenu.GetComponent<Animator>().SetBool("restartButtonPressed", true);

        StartLevel();

        yield return new WaitForSeconds(1.0f);
        
        GameOverMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        if (MaxLevelReached())
        {
            transAnimator.SetBool("levelFinished", false);
            transAnimator.SetBool("newLevelStarted", true);
        }

        levelStarted = false;

        levels[level - 1].SetActive(false);

        ship.SetActive(false);

        GameOverMenu.SetActive(false);

        MenuObject.GetComponent<RectTransform>().localScale = Vector3.one;
        MenuObject.SetActive(true);
    }
}
