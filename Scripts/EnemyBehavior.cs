using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    GameObject player;
    [SerializeField] float speed;
    NavMeshAgent agent;
    [SerializeField] bool isBeholder;
    [SerializeField] GameObject vfx;
    [SerializeField] GameObject deathVfx;
    [SerializeField] GameObject playerScript;
    GameObject killSound;
    AudioSource spawnSound;
    void Start()
    {
        if(!isBeholder)
            agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = GameObject.FindGameObjectWithTag("MainCamera");
        killSound = GameObject.FindGameObjectWithTag("KillThemeSound");
        spawnSound = GetComponent<AudioSource>();
        spawnSound.Play();
        Instantiate(vfx, transform.position, transform.rotation);
    }
    void Update()
    {
        transform.LookAt(player.transform);
        
        if(!isBeholder)
            agent.SetDestination(player.transform.position);

        else
            transform.Translate(player.transform.position * Time.deltaTime * speed);

        if (!playerScript.GetComponent<PlayerBehavior>().canBeSpawned)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerHitten()
    {
        PlayAudio();
        playerScript.GetComponent<PlayerBehavior>().monstersKilled++;
        Instantiate(deathVfx, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void PlayAudio()
    {
        //killSound.GetComponent<AudioSource>().Stop();
        killSound.GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerScript.GetComponent<PlayerBehavior>().EnemyHit(1);
        Destroy(gameObject);
        print("Morri");
    }
}
