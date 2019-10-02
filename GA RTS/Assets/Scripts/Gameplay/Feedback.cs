using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    [SerializeField] PlayerSkillManager playerSkillManager;
    [SerializeField] Database database;
    [SerializeField] AIManager aiManager;

    [SerializeField] Slider slider;
    [SerializeField] GameObject ui;

    [SerializeField] float feedbackIntervalTime = 60.0f;
    private float feedbackTimer = 0.0f;

    private int skillCounter = 1;
    private float skillTotal = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ui.activeInHierarchy)
            feedbackTimer += Time.deltaTime;

        if (feedbackTimer > 0)
        {            
            if ((Mathf.RoundToInt(feedbackTimer) % 10 == 0) && skillCounter == (Mathf.RoundToInt(feedbackTimer) / 10))
            {
                skillTotal += playerSkillManager.GetPlayerSkill();
                skillCounter++;
            }
        }

        if (feedbackTimer > feedbackIntervalTime)
        {
            if (!GameplayManager.instance.GetGameOver())
                ShowUI();

            feedbackTimer = 0.0f;            

            Time.timeScale = 0.0f;            
        }
    }

    public void ShowUI()
    {
        ui.SetActive(true);
        slider.value = 0.5f;
    }

    public void NewFeedback()
    {
        float flow = slider.value;
        float skill = skillTotal / skillCounter;
        float difficulty = aiManager.GetDifficulty();
        //float skill = Mathf.Lerp(0, 500, (skillTotal / skillCounter));
        //float difficulty = Mathf.Lerp(10, 200, aiManager.GetDifficulty());

        skillTotal = 0;

        database.NewFeedbackData(difficulty, skill, flow);

        Time.timeScale = 1.0f;

        skillCounter = 0;
        skillTotal = 0;

        ui.SetActive(false);
    }
}
