using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessScript : MonoBehaviour {

    public GameObject ThePlayer;

    int TheLevel = 1;
    int SpeedModifier = 100;

    const int RANDOMMOVEMENT = 1;
    const int BASICMOVEMENT = 2;
    const int DIRECTMOVEMENT = 3;

    const string UP = "UP";
    const string DOWN = "DOWN";
    const string LEFT = "LEFT";
    const string RIGHT = "RIGHT";

    int MOVEMENTMODE = 1;

    int RandomLevel = 50;
    int BasicLevel = 80;
    int DirectLevel = 100;

    double RandomMovementTimer = 0;
    string CurrentDirection;

	// Use this for initialization
	void Start () {
        //get the level
        TheLevel = ThePlayer.GetComponent<MovementScript>().GetTheLevel();

        //choose movement mode
        int ModeSelector = (int)Mathf.Round(Random.value * 100);

        //weighting
        RandomLevel = 50 - 2 * TheLevel;
        BasicLevel = 80 - 2 * TheLevel;

        if (ModeSelector < RandomLevel)
        {
            MOVEMENTMODE = RANDOMMOVEMENT;
        }
        if(ModeSelector >= RandomLevel && ModeSelector <= BasicLevel)
        {
            MOVEMENTMODE = BASICMOVEMENT;
        }
        if (ModeSelector > BasicLevel && ModeSelector <= DirectLevel)
        {
            MOVEMENTMODE = DIRECTMOVEMENT;
        }


        //Change the speed modifier
        SpeedModifier = SpeedModifier * ((1 / (25 - TheLevel)) + 1);

        //set initial direction
        CurrentDirection = LEFT;
    }
	
	// Update is called once per frame
	void Update () {
        //move the darkness according to which movement mode was selected
        Vector3 direction;
        Vector3 BasicPosition;
        switch (MOVEMENTMODE)
        {
            case RANDOMMOVEMENT:
                RandomMovementTimer += Time.deltaTime;
                if (RandomMovementTimer >= 3)
                {
                    //choose a new direction
                    int NewDirection = (int)Mathf.Round(Random.value * 3);
                    switch (NewDirection)
                    {
                        case 0:
                            CurrentDirection = UP;
                            break;
                        case 1:
                            CurrentDirection = LEFT;
                            break;
                        case 2:
                            CurrentDirection = RIGHT;
                            break;
                        case 3:
                            CurrentDirection = DOWN;
                            break;
                    }
                    RandomMovementTimer = 0;
                }
                    
                switch (CurrentDirection)
                {
                    case UP:
                        gameObject.transform.Translate(Vector3.up * Time.deltaTime * SpeedModifier);
                        break;
                    case LEFT:
                        gameObject.transform.Translate(Vector3.left * Time.deltaTime * SpeedModifier);
                        break;
                    case RIGHT:
                        gameObject.transform.Translate(Vector3.right * Time.deltaTime * SpeedModifier);
                        break;
                    case DOWN:
                        gameObject.transform.Translate(Vector3.down * Time.deltaTime * SpeedModifier);
                        break;
                }
                break;
            case BASICMOVEMENT:
                BasicPosition = new Vector3(ThePlayer.transform.position.x + 100, ThePlayer.transform.position.y + 100, -100);
                direction = BasicPosition - transform.position;
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, transform.position + direction, SpeedModifier * Time.deltaTime);
                break;
            case DIRECTMOVEMENT:
                direction = ThePlayer.transform.position - transform.position;
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, transform.position + direction, SpeedModifier * Time.deltaTime);
                break;
            default:
                break;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ThePlayer.GetComponent<MovementScript>().LoseGame();
        }
        if (collision.gameObject.name == "Lantern(Clone)")
        {
            //it ran into a lantern, we should respawn somewhere else
            Vector3 respawn = new Vector3(-50,-50,-100);
            gameObject.transform.position = respawn;
        }
    }
}
