using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchBuilding : MonoBehaviour
{
    private TechnologyManager techManager;

    private float researchTimer = 0.0f;
    private float researchDelay = 100.0f;
    private float researchProgress = 0.0f;
    private bool researching = false;
    private string currentResearch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (researching)
        {
            researchTimer += Time.deltaTime;

            researchProgress = researchTimer / researchDelay;

            techManager.ResearchProgress(currentResearch, researchProgress);

            if (researchTimer > researchDelay)
            {
                researchTimer = 0.0f;
                researching = false;

                techManager.UnlockTech(currentResearch);
            }
        }
    }

    public bool GetIsResearching()
    {
        return researching;
    }

    public string GetCurrentResearch()
    {
        return currentResearch;
    }

    public void NewResearch(float _time, string _research)
    {
        if (researching)
            return;

        researchDelay = _time;
        currentResearch = _research;
        researching = true;
    }

    public TechnologyManager GetTechManager()
    {
        if (techManager)
            return techManager;
        else
            return null;
    }

    public void SetTechManager(TechnologyManager _mana)
    {
        techManager = _mana;
    }
}
