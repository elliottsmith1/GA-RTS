using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnQueueUI;
    [SerializeField] Image spawnIcon; 

    private List<Unit> spawnQueue = new List<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        spawnIcon = spawnQueueUI[0].GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpawnQueue(List<Unit> _spawnQueue, float _spawn)
    {
        spawnQueue = _spawnQueue;
        AnimateQueueIcon(1 - _spawn);

        for (int i = 0; i < spawnQueueUI.Count; i++)
        {
            if (spawnQueue.Count >= i+1)
            {
                if (!spawnQueueUI[i].activeSelf)
                {
                    spawnQueueUI[i].SetActive(true);
                }
            }
            else
            {
                if (spawnQueueUI[i].activeSelf)
                {
                    spawnQueueUI[i].SetActive(false);
                }
            }
        }
    }

    public void AnimateQueueIcon(float _val)
    {
        spawnIcon.fillAmount = _val;
    }
}
