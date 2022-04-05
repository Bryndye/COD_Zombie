using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : ItemInteraction
{
    public bool Full, CanRebuild = true;
    [SerializeField] GameObject[] planks;
    [SerializeField] Vector3[] posPlanksInit;
    public int plankStill = 5;
    public Transform finalPosZombie;
    public float TimeMaxToRebuildAWindow = 1.5f;
    float timeToRebuild;
    bool isRebuild = false;
    AudioSource audioBarrier;

    private void Awake()
    {
        posPlanksInit = new Vector3[planks.Length];
        audioBarrier = GetComponent<AudioSource>();

        for (int i = 0; i < planks.Length; i++)
        {
            posPlanksInit[i] = planks[i].transform.localPosition;
        }
    }

    private void Start()
    {
        for (int i = 0; i < planks.Length; i++)
        {
            planks[i].SetActive(i < plankStill);
        }
    }

    private void Update()
    {
        Full = plankStill >= 5;
        if (isRebuild && !Full)
        {
            timeToRebuild += Time.deltaTime;
            if (timeToRebuild >= TimeMaxToRebuildAWindow)
            {
                timeToRebuild = 0;
                isRebuild = false;
            }
        }
        MessageCanAppear = !Full && CanRebuild;
    }

    public void TakeDamage()
    {
        if (plankStill <= 0)
        {
            plankStill = 0;
            return;
        }
        plankStill--;
        planks[plankStill].SetActive(false);
    }

    public override void Interact(PlayerActions _playerActions)
    {
        if (Full || !CanRebuild)
        {
            return;
        }
        Rebuild(_playerActions);
    }
    public void Rebuild(PlayerActions _playerActions)
    {
        if (isRebuild)
        {
            return;
        }
        isRebuild = true;
        audioBarrier.Play();
        plankStill++;
        planks[plankStill -1].SetActive(true);
        _playerActions.pPoints.GetPoints(10);
    }
}
