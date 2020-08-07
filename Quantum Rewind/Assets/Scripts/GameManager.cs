﻿using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    public int GameIteration { private set; get; }
    public bool IsFinalIteration { get { return GameIteration == spawnManager.spawnpoints.Length - 1; } }
    public bool isPlaying = false;

    public Portal portal;

    UIManager uiManager;
    SpawnManager spawnManager;
    EnergyManager energyManager;

    void Awake()
    {
        Instance = this;
        GameIteration = -1;
    }

    void Start()
    {
        uiManager = UIManager.Instance;
        spawnManager = SpawnManager.Instance;
        energyManager = EnergyManager.Instance;

        PostProcessingController.Instance.TriggerDepthOfField(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        if (!isPlaying)
            if (Input.anyKeyDown)
                StartGame();
    }

    void StartGame()
    {
        isPlaying = true;

        PostProcessingController.Instance.TriggerDepthOfField(false);

        AudioManager.Instance.PlaySFX("Start");

        ReincarnateInNext();

        uiManager.OnStartGame();
        uiManager.RequiredEnergyTextPosition(energyManager.NowBattery.transform.position);
    }

    public void Win()
    {
        Debug.Log("Win!");
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        Debug.Log("Lose!");
        SceneManager.LoadScene(0);
    }

    #region Reincarnations
    public void ReincarnateInNext()
    {
        NextIteration();
        if (IsOutOfIterations())
            return;

        uiManager.RequiredEnergyTextPosition(energyManager.NowBattery.transform.position);
        uiManager.SetRequiredEnergyTextValue(energyManager.EnergyPerBattery);
        uiManager.SetRequiredEnergyTextState(true);

        energyManager.InitEnergy();
        spawnManager.SpawnAnomalies(true);
    }

    public void ReincarnateInCurrent()
    {
        energyManager.InitEnergy();
        spawnManager.SpawnAnomalies(false);
    }
    #endregion

    #region Iterations
    public void NextIteration()
    {
        GameIteration++;
        //Debug.Log(GameIteration);
    }
    public bool IsOutOfIterations()
    {
        return GameIteration >= spawnManager.spawnpoints.Length;
    }
    #endregion
}
