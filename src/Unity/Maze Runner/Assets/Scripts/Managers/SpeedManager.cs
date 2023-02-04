using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] float playerWalkSpeed = 5;
    [SerializeField] float playerRunSpeed = 10;
    [SerializeField] float botSpeed = 8; 
    [SerializeField] float speedModifierTime = 15;

    private int invokeCount = 1;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerController;
    private UnityEngine.AI.NavMeshAgent botController;

    void Start()
    {
        playerController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        botController = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void SetPlayerSpeed(float speed_mult)
    {
        playerController.SetRunSpeed(playerRunSpeed*speed_mult);
        playerController.SetWalkSpeed(playerWalkSpeed*speed_mult);
        ++invokeCount;
        Invoke("resetSpeedPlayer", speedModifierTime);
    }

    public void SetAISpeed(float speed_mult)
    {
        botController.speed = botSpeed*speed_mult;
        ++invokeCount;
        Invoke("resetSpeedAI", speedModifierTime);
    }

    void resetSpeedPlayer()
    {
        if (invokeCount <3)
        {
            playerController.SetRunSpeed(playerRunSpeed);
            playerController.SetWalkSpeed(playerWalkSpeed);
        }
        --invokeCount;
    }

    void resetSpeedAI()
    {
        if (invokeCount <3)
        {
            botController.speed = botSpeed;
        }
        --invokeCount;
    }
}
