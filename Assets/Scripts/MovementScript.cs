using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    const string UP = "UP";
    const string DOWN = "DOWN";
    const string LEFT = "LEFT";
    const string RIGHT = "RIGHT";

    const int MINESTONETIME = 3;
    const int MINEGOLDTIME = 5;
    const int MINEMITHRILTIME = 10;

    List<GameObject> AllBlockSprites;

    GameObject CurrentlyMining;

    int MovementModifier = 10;
    double MiningModifier = 1;
    float MapXScale = 5;
    float MapYScale = 5;

    bool IsMining = false;
    double MiningTimer = 0;

    string CurrentDirection = UP;

    public Sprite TheMapVar;

    // Use this for initialization
    void Start()
    {
        TheMapVar = GameObject.Find("TheMap").GetComponent<SpriteRenderer>().sprite;
        AllBlockSprites = GetAllSpritesInScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMining)
        {
            //Down Movement
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S)))
            {
                CurrentDirection = DOWN;
                gameObject.transform.Translate(Vector3.down * Time.deltaTime * MovementModifier);
            }
            //Right Movement
            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D)))
            {
                CurrentDirection = RIGHT;
                gameObject.transform.Translate(Vector3.right * Time.deltaTime * MovementModifier);
            }
            //Left Movement
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A)))
            {
                CurrentDirection = LEFT;
                gameObject.transform.Translate(Vector3.left * Time.deltaTime * MovementModifier);
            }
            //Up Movement
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W)))
            {
                CurrentDirection = UP;
                gameObject.transform.Translate(Vector3.up * Time.deltaTime * MovementModifier);
            }
            //Mining
            if (Input.GetKeyDown(KeyCode.M))
            {
                checkMining();
            }
            //Torch
            if (Input.GetKeyDown(KeyCode.L))
            {
                
            }
        }
        else
        {
            MiningTimer += Time.deltaTime;
            ContinueMining();
        }

    }

    //check that the player is able to start mining i.e. facing a block
    void checkMining()
    {
        foreach(GameObject sp in AllBlockSprites)
        {
            
            Vector3 thebounds = sp.GetComponent<Collider2D>().bounds.size;
            //check for location
            switch (CurrentDirection)
            {
                case UP:
                    if ((sp.transform.position.x - thebounds.x / 2) <= gameObject.transform.position.x && (sp.transform.position.x + thebounds.x / 2) >= gameObject.transform.position.x && (sp.transform.position.y - thebounds.y / 2) >= gameObject.transform.position.y && (Mathf.Abs(Mathf.Abs(sp.transform.position.y - thebounds.y / 2) - Mathf.Abs(gameObject.transform.position.y)) < MiningModifier))
                    {
                        startMining(sp);
                    }
                    break;
                case DOWN:
                    if ((sp.transform.position.x - thebounds.x / 2) <= gameObject.transform.position.x && (sp.transform.position.x + thebounds.x / 2) >= gameObject.transform.position.x && (sp.transform.position.y + thebounds.y / 2) <= gameObject.transform.position.y && (Mathf.Abs(Mathf.Abs(gameObject.transform.position.y) - Mathf.Abs(sp.transform.position.y + thebounds.y / 2)) < MiningModifier))
                    {
                        startMining(sp);
                    }
                    break;
                case LEFT:
                    if ((sp.transform.position.y + thebounds.y / 2) >= gameObject.transform.position.y && (sp.transform.position.y - thebounds.y / 2) <= gameObject.transform.position.y && (sp.transform.position.x + thebounds.x / 2) <= gameObject.transform.position.x && (Mathf.Abs(Mathf.Abs(gameObject.transform.position.y) - Mathf.Abs(sp.transform.position.x + thebounds.x / 2)) < MiningModifier))
                    {
                        startMining(sp);
                    }
                    break;
                case RIGHT:
                    if ((sp.transform.position.y + thebounds.y / 2) >= gameObject.transform.position.y && (sp.transform.position.y - thebounds.y / 2) <= gameObject.transform.position.y && (sp.transform.position.x - thebounds.x / 2) >= gameObject.transform.position.x && (Mathf.Abs(Mathf.Abs(sp.transform.position.x - thebounds.x / 2) - Mathf.Abs(gameObject.transform.position.x))  < MiningModifier))
                    {
                        startMining(sp);
                    }
                    break;
                default:
                    //default to nothing
                    break;
            }
            if (sp.transform.position.x <= gameObject.transform.position.x && (sp.transform.position.x + thebounds.x) >= gameObject.transform.position.x && sp.transform.position.y >= gameObject.transform.position.y && (sp.transform.position.y - gameObject.transform.position.y < MiningModifier)){
                startMining(sp);
            }
            
        }
    }

    //function to handle mining
    void startMining(GameObject BlockBeingMined)
    {
        IsMining = true;
        CurrentlyMining = BlockBeingMined;
    }

    void ContinueMining()
    {
        double TimeNeeded = MINESTONETIME;

        //TODO: Add a switch for the type of block

        if(MiningTimer >= TimeNeeded)
        {
            //mining is finished
            IsMining = false;
            AllBlockSprites.Remove(CurrentlyMining);
            Destroy(CurrentlyMining);
            //reset timer
            MiningTimer = 0;
        }
    }

    List<GameObject> GetAllSpritesInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject sp in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (sp.GetComponent<Collider2D>() != null && !sp.ToString().Equals(gameObject.ToString()) && sp != null)
            {
                Debug.Log(sp.ToString());
                objectsInScene.Add(sp);
            }
        }

        return objectsInScene;
    }
}
