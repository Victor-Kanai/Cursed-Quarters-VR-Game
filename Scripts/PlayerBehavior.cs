using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public int monstersKilled;
    [SerializeField] int hordesPassed;
    [SerializeField] int health;
    [SerializeField] TextMeshPro hordeTxt;
    [SerializeField] TextMeshPro monstersKilledTxt;
    [SerializeField] Slider healthSlider;
    [SerializeField] GameObject cartAnimation;
    [SerializeField] GameObject cartSound;
    [SerializeField] GameObject menuPanel;
    [SerializeField] TextMeshPro endTxt;
    int pastHorde;
    public bool canBeSpawned;
    bool hasStartedGame;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        cartAnimation.GetComponent<Animator>().SetFloat("CartSpeed", 0);
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartGame(!hasStartedGame);
            Shoot();
        }

        UpdateScoreAndHealth();

        if(health <= 0)
        {
            endTxt.text = "Você matou " + monstersKilled + " monstros, mas não conseguiu escapar do Cursed Quarters";
            cartAnimation.GetComponent<Animator>().SetFloat("CartSpeed", 0);
            menuPanel.SetActive(true);
            cartSound.GetComponent<AudioSource>().Stop();
        }

        if(Input.GetButtonDown("Fire1") && health <= 0)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void StartGame(bool start)
    {
        if (start)
        {
            menuPanel.SetActive(false);
            cartSound.GetComponent<AudioSource>().Play();
            cartAnimation.GetComponent<Animator>().SetFloat("CartSpeed", 1);
            hasStartedGame = true;
        }
    }

    private void UpdateScoreAndHealth()
    {
        hordeTxt.text = hordesPassed.ToString();
        monstersKilledTxt.text = monstersKilled.ToString();
        healthSlider.value = health;
    }

    private void Shoot()
    {
        Vector2 cameraScreenSpace = new Vector2(Screen.width/2, Screen.height/2);
        ray = Camera.main.ScreenPointToRay(cameraScreenSpace);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Enemy")
            {
                if (hit.collider.gameObject != null)
                {
                    hit.collider.gameObject.GetComponent<EnemyBehavior>().PlayerHitten();
                }
                else
                {
                    return;
                }
            }
            //Debug.DrawLine(transform.position, cameraScreenSpace * hit.distance, Color.yellow, 100, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "EntryDoor")
        {
            canBeSpawned = true;
            PassHorde();
        }

        if (other.gameObject.tag == "ExitDoor")
        {
            canBeSpawned = false;
            cartAnimation.GetComponent<Animator>().SetFloat("CartSpeed", cartAnimation.GetComponent<Animator>().GetFloat("CartSpeed") * .9f);
        }    
    }

    private void PassHorde()
    {
        hordesPassed++;
    }

    public void EnemyHit(int damage)
    {
        GetComponent<AudioSource>().Play();
        health = health - damage;
    }
}
