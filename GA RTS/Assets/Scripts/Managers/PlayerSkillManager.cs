using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public enum ACTION_TYPES
    {
        NEW_BUILDING,
        NEW_UNIT,
        SELECT_UNITS,
        SELECT_BUILDING,
        MOVE_UNITS,
        WAIT
    }

    public static PlayerSkillManager instance;

    private ACTION_TYPES lastAction;
    private float actionTimer = 0.0f;
    private float actionDelay = 1.0f;

    private List<ACTION_TYPES> actions = new List<ACTION_TYPES>();

    private List<float> APMs = new List<float>();

    private float APM = 0.0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        actionTimer += Time.deltaTime;

        if (actionTimer > actionDelay)
        {
            actionTimer = 0.0f;
            CalculateAPM();
        }
    }

    private void CalculateAPM()
    {
        int secondsCounter = 15;

        float action = actions.Count * 60;
        APMs.Add(action);
        actions.Clear();

        lastAction = ACTION_TYPES.WAIT;

        if (APMs.Count > secondsCounter)
        {
            float sum = 0;

            for (var i = 0; i < APMs.Count; i++)
            {
                sum += APMs[i];
            }

            APM = ((sum / APMs.Count) + APM) / 2; 
            APMs.Clear();
        }               
    }

    public void NewAction(ACTION_TYPES _action)
    {
        if (_action == lastAction)
            return;

        lastAction = _action;
        actions.Add(lastAction);
    }

    public float GetAPM()
    {
        return APM;
    }
}
