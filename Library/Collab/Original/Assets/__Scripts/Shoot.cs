﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Shoot : MonoBehaviour
{
    public GameObject ball; //reference to the ball prefab, set in editor
    private Vector3 throwSpeed = new Vector3(0, 26, 40); //guarenteed basket value
    public Vector3 ballPos; //starting ball position
    private bool thrown = false; //prevents there from being two balls in play
    private GameObject ballClone; //So we dont edit the orignal prefab
    public TextMeshProUGUI ballText;
    public GameObject gameOverPanel;
    public GameObject shooting;

    public GameObject availableShotsGO; //ScoreText game object reference
    private int _availableShots;

    public GameObject meter; //force meter
    public GameObject arrow;
    public float arrowSpeed; //Arrow speed, faster = more difficult
    private bool right = true; //used to revers arrow movement

    
    public GameObject gameOver; //game over text

    private QuizManager quizManager;
    private playerVariables variables;

    private static int game = 0;

   void Awake() {
       quizManager = GameObject.FindObjectOfType<QuizManager>();
   }

    private void Start()
    {
        Physics.gravity = new Vector3(0, -20, 0);
        _availableShots = 10;
        SetSpeed();
        BallText();
    }
   
    private void FixedUpdate() //This moves the arrow along the power bar
    {
        if (arrow.transform.position.x < 10.25f && right)
        {
            arrow.transform.position += new Vector3(arrowSpeed, 0, 0);
        }
        if (arrow.transform.position.x >= 10.25f)
        {
            right = false;
        }
        if (right == false)
        {
            arrow.transform.position -= new Vector3(arrowSpeed, 0, 0);
        }
        if (arrow.transform.position.x <= -8.75f)
        {
            right = true;
        }

        if (Input.GetKey(KeyCode.Space) && !thrown && _availableShots > 0) //Shoots the ball
        {
            thrown = true;
            _availableShots--;
            BallText(); //changes the number under the mini basketball, bottom right corner

            ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
            throwSpeed.y = throwSpeed.y + arrow.transform.position.x;
            throwSpeed.z = throwSpeed.z + arrow.transform.position.x;

            ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
        }

        if (ballClone != null && ballClone.transform.position.y < -12) //Removes the ball when it hits the floor
        {
            Destroy(ballClone);
            thrown = false;
            throwSpeed = new Vector3(0, 26, 40);//Reset perfect shot variable
        }

        if (_availableShots == 0) //If the player is out of shots
        {
            Invoke("Delay", 4);   
        }
    }
    public void updateArrowSpeed(float speedChange) {
        arrowSpeed -= speedChange;
    }
    void BallText() //Display number of available balls
    {
        ballText.text = "Shots: " + _availableShots.ToString();
    }

    void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void Delay() //to delay the game over panel
    {
       
        SceneManager.LoadScene("Gym");
    }

    public void RetryButton() //The retry button at the end of the game, (Not currently in use)
    {
        SceneManager.LoadScene("Gym");
    }

    public void SetSpeed() //Sets the speed of the ball
    {
        if (playerVariables.PV.triviaScore >= 4)
        {
            arrowSpeed = 0.3f;
        }
        else
        {
            arrowSpeed = 0.6f;
        }
    }
}
