using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBot : MonoBehaviour
{
    private float sinceLastCheck;

    [SerializeField] float checkInterval = 1;

    [SerializeField] float minMusicPlay = 5;

    private GameManager gameManager;

    [SerializeField] AudioClip[] musics;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManager = gameObject.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sinceLastCheck < checkInterval)
        {
            sinceLastCheck += Time.deltaTime;
            return;
        }
        sinceLastCheck = 0;
        // Close to Minotaure : Priority 1 
        if(PlayerCloseToMinotaure())
        {   
            ChangeMusic(musics[0]);
            return;
        }
        // Enemy with 2nd orb : Priority 2 
        if(EnemyHasOrb(2))
        {   
            ChangeMusic(musics[1]);
            return;
        }
        bool allyOrb = AllyHasOrb(1) || AllyHasOrb(2);
        // Enemies close to ally with orb : Priority 3
        if(allyOrb && OrbHolderCloseToEnemy())
        {
            ChangeMusic(musics[2]);
            return;
        }
        // Enemies has orb : Priority 4
        if(EnemyHasOrb(1))
        {
            ChangeMusic(musics[3]);
            return;
        }
        // Ally has orb : Priority 5
        if(allyOrb)
        {
            ChangeMusic(musics[4]);
            return;
        }
        // Exploration : By default
        bool result = audioManager.ReturnToDefault();
        if(result)
        {
            sinceLastCheck = -minMusicPlay + checkInterval;
        }
        return;
    }

    void ChangeMusic(AudioClip new_clip)
    {
        bool result = audioManager.SwapTrack(new_clip);
        if(result)
        {
            sinceLastCheck = -minMusicPlay + checkInterval;
        }
    }

    bool PlayerCloseToMinotaure()
    {
        return gameManager.PlayerCloseToMinotaure();
    }

    bool OrbHolderCloseToEnemy()
    {
        return gameManager.OrbHolderCloseToEnemy();
    }

    bool AllyHasOrb(int numOrb)
    {
        return gameManager.AllyHasOrb(numOrb);
    }

    bool EnemyHasOrb(int numOrb)
    {
        return gameManager.EnemyHasOrb(numOrb);
    }
}
