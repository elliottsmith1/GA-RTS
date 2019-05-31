using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private enum BUILDSTATE
    {
        NOT_BUILT,
        BUILDING,
        FINISHED
    }

    [SerializeField] Image buildtimerBackground;
    [SerializeField] Image buildtimerForeground;

    [SerializeField] Mesh foundationMesh;
    [SerializeField] Mesh constructionMesh;
    [SerializeField] Mesh finishedMesh;

    [SerializeField] float buildTime = 10.0f;
    public float buildTimer = 0.0f;

    private bool built = false;

    private MeshFilter meshFilter;

    [SerializeField] BUILDSTATE buildState = BUILDSTATE.NOT_BUILT;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!built)
        {
            Construction();
        }
    }

    private void Construction()
    {
        switch(buildState)
        {
            case BUILDSTATE.NOT_BUILT:

                if (meshFilter.mesh != foundationMesh)
                {
                    meshFilter.mesh = foundationMesh;
                }

                buildTimer += Time.deltaTime;
                buildtimerForeground.fillAmount = buildTimer / buildTime;

                if (buildTimer > (buildTime / 2))
                {
                    buildState = BUILDSTATE.BUILDING;
                    if (meshFilter)
                    {
                        meshFilter.mesh = constructionMesh;
                    }

                }
                break;
            case BUILDSTATE.BUILDING:

                if (meshFilter.mesh != constructionMesh)
                {
                    meshFilter.mesh = constructionMesh;
                }

                buildTimer += Time.deltaTime;
                buildtimerForeground.fillAmount = buildTimer / buildTime;

                if (buildTimer > buildTime)
                {
                    buildState = BUILDSTATE.FINISHED;
                    if (meshFilter)
                    {
                        meshFilter.mesh = finishedMesh;
                    }
                }
                break;
            case BUILDSTATE.FINISHED:
                if (meshFilter.mesh != finishedMesh)
                {
                    meshFilter.mesh = finishedMesh;

                    buildtimerBackground.enabled = false;
                    buildtimerForeground.enabled = false;

                    built = true;
                }
                return;
        }
    }

    //private void OnMouseOver()
    //{
    //    if (buildState != BUILDSTATE.FINISHED)
    //    {
    //        buildtimerBackground.enabled = true;
    //        buildtimerForeground.enabled = true;
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    buildtimerBackground.enabled = false;
    //    buildtimerForeground.enabled = false;
    //}
}
