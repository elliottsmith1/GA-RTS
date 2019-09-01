﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] PlayerManager playerManager;

    [SerializeField] Text populationText;
    [SerializeField] Text goldText;
    [SerializeField] Text woodText;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] Text gameOverText;
    [SerializeField] Text timeElapsedText;

    [SerializeField] BuildingUI barracksUI;
    [SerializeField] BuildingUI archerRangeUI;

    [SerializeField] GameObject buildingsPane;
    [SerializeField] GameObject barracksPane;
    [SerializeField] GameObject archerRangePane;
    [SerializeField] GameObject mageTowerPane;
    [SerializeField] GameObject stablesPane;
    [SerializeField] GameObject techPane;

    private GameObject activePane;

    // Start is called before the first frame update
    void Start()
    {
        activePane = buildingsPane;
    }

    // Update is called once per frame
    void Update()
    {
        populationText.text = playerManager.GetPopulationString();
        goldText.text = playerManager.GetGold().ToString();
        woodText.text = playerManager.GetWood().ToString();

        if (activePane == barracksPane)
        {
            barracksUI.UpdateSpawnQueue(buildingManager.GetActiveBuilding().GetSpawnQueue(), buildingManager.GetActiveBuilding().GetSpawnTimerP());
        }
        else if (activePane == archerRangePane)
        {
            archerRangeUI.UpdateSpawnQueue(buildingManager.GetActiveBuilding().GetSpawnQueue(), buildingManager.GetActiveBuilding().GetSpawnTimerP());
        }
    }

    public void ActivatePane(string _pane)
    {
        activePane.SetActive(false);

        switch(_pane)
        {
            case "barracks":
                activePane = barracksPane;
                break;
            case "archery":
                activePane = archerRangePane;
                break;
            case "magetower":
                activePane = mageTowerPane;
                break;
            case "stables":
                activePane = stablesPane;
                break;
            case "buildings":
                activePane = buildingsPane;
                break;
            case "research":
                activePane = techPane;
                break;
        }

        activePane.SetActive(true);

        PlayerSkillManager.instance.NewAction(PlayerSkillManager.ACTION_TYPES.SELECT_BUILDING);
    }

    public void CancelUnitSpawn(int _id)
    {
        buildingManager.GetActiveBuilding().CancelSpawnUnit(_id);
    }

    public void GameOver(bool _won, float _time)
    {
        gameOverUI.SetActive(true);

        string minutes = Mathf.Floor(_time / 60).ToString("00");
        string seconds = (_time % 60).ToString("00");

        timeElapsedText.text = "Time: " + string.Format("{0}:{1}", minutes, seconds);

        if (_won)
        {
            gameOverText.text = "You win!";
        }
        else
        {
            gameOverText.text = "Game Over!";
        }
    }
}
