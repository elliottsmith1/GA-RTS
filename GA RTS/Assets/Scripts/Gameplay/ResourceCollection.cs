﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCollection : MonoBehaviour
{
    // Start is called before the first frame update

    enum RESOURCETYPE
    {
        GOLD,
        WOOD
    }

    [SerializeField] RESOURCETYPE resource = RESOURCETYPE.GOLD;
    [SerializeField] float collectionSpeed = 5.0f;

    [SerializeField] bool enemyBuilding = false;

    [SerializeField] GameObject popup;
    private Text popupText;

    private int resourceValue = 1;

    private float collectionTimer = 0.0f;

    private float yPos = 0.0f;
    private Color alphaVal = Color.yellow;

    private PlayerManager playerManager;
    private AIManager aIManager;

    

    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        aIManager = GameObject.Find("AI Manager").GetComponent<AIManager>();

        popupText = popup.GetComponent<Text>();

        switch (resource)
        {
            case RESOURCETYPE.GOLD:
                alphaVal = Color.yellow;
                break;
            case RESOURCETYPE.WOOD:
                alphaVal = new Color32(244, 164, 96, 255);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        collectionTimer += Time.deltaTime;

        if (collectionTimer > collectionSpeed)
        {
            collectionTimer = 0.0f;

            if (!enemyBuilding)
                TextPop();

            switch(resource)
            {
                case RESOURCETYPE.WOOD:
                    if (!enemyBuilding)
                    {
                        playerManager.AddWood(resourceValue);
                        PlayerSkillManager.instance.Income(resourceValue);
                    }
                    else
                    {
                        aIManager.AddWood(resourceValue);
                    }
                    break;
                case RESOURCETYPE.GOLD:
                    if (!enemyBuilding)
                    {
                        playerManager.AddGold(resourceValue);
                        PlayerSkillManager.instance.Income(resourceValue);
                    }
                    else
                    {
                        aIManager.AddGold(resourceValue);
                    }
                    break;
            }
        }

        if (!enemyBuilding)
            AnimateText();
    }

    private void AnimateText()
    {
        yPos += 0.05f;
        Vector3 pos = transform.position;
        pos.y += yPos;

        alphaVal.a -= 0.01f;
        popupText.color = alphaVal;

        popup.transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    private void TextPop()
    {
        alphaVal.a = 1.0f;
        yPos = 0.0f;

        string text = "+" + resourceValue + " " + resource;
        popupText.text = text;
        
        popup.SetActive(true);
    }

    public void SetEnemyBuilding()
    {
        enemyBuilding = true;
    }
}
