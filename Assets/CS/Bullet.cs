﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public Transform from;
    public Transform to;
    public float value = 10.0F;
    private float startTime;
    public GameControler gameController;

    //turn
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;

    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
       //탄환이 포물선을 그리며 이동
        if (!transform.position.Equals(to))
        {
            Vector3 center = (from.position + to.position) * 0.5F;
            center -= new Vector3(0, 1, 0);
            Vector3 riseRelCenter = from.position - center;
            Vector3 setRelCenter = to.position - center;
            float fracComplete = (Time.time - startTime) / value;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
         }
    }

    //destroy bullet
    void OnTriggerEnter(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "Arrow":
                Destroy(other.gameObject);
                break;

            case "Tile":
                //remove bullet
                Destroy(this.gameObject);
                //test : remove fog
                FogControler fg = other.GetComponent<FogControler>();
                fg.fogOff();

                //no hit - change turn
                ChangeTurn();
                destroyBullet();
                break;

            case "Ship":
                //hit - not change turn, attack again
                AttackAgain();
                destroyBullet();
                break;
        }
    }

    void destroyBullet()
    {
        //destroy bullet game object
        Destroy(this.gameObject);
    }

    //no hit - change turn
    void ChangeTurn()
    {
        //get game controller's turn
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        int whoseTurn = gameController.GetTurn();

        //change turn each case
        switch (whoseTurn)
        {
            case USER_BLOCK:
                gameController.turn = AI_TURN;
                break;
            case AI_BLOCK:
                gameController.turn = USER_TURN;
                break;
        }
    }

    //hit - not change turn, attack again
    void AttackAgain()
    {
        //get game controller's turn
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        int whoseTurn = gameController.GetTurn();

        //change turn each case
        switch (whoseTurn)
        {
            case USER_BLOCK:
                gameController.turn = USER_TURN;
                break;
            case AI_BLOCK:
                gameController.turn = AI_TURN;
                break;
        }
    }


}
