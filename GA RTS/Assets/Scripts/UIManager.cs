using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject buildingsPane;
    [SerializeField] GameObject barracksPane;
    [SerializeField] GameObject archerRangePane;
    [SerializeField] GameObject mageTowerPane;
    [SerializeField] GameObject stablesPane;

    private GameObject activePane;

    // Start is called before the first frame update
    void Start()
    {
        activePane = buildingsPane;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }

        activePane.SetActive(true);
    }
}
