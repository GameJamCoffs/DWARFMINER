using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    public GameObject Lantern;

    const string UP = "UP";
    const string DOWN = "DOWN";
    const string LEFT = "LEFT";
    const string RIGHT = "RIGHT";

    const string STONE = "STONE";
    const string GOLD = "GOLD";
    const string MITHRIL = "MITHRIL";

    const int MINESTONETIME = 3;
    const int MINEGOLDTIME = 5;
    const int MINEMITHRILTIME = 10;

    const int LIGHTPLACETIME = 3;

    const int LANTERNSALLOWED = 10;
    const int MINMOVEMENTMODIFIER = 5;
    const int MAXMOVEMENTMODIFIER = 10;

    //level specific variables
    int TheLevel = 1;
    int LanternsPlaced = 0;
    int MithrilRequired = 10;

    List<GameObject> AllBlockSprites;

    GameObject CurrentlyMining;
    GameObject CurrentlyLighting;

    int MovementModifier = 7;
    double MiningModifier = .1;

    bool IsMining = false;
    double MiningTimer = 0;

    bool IsLighting = false;
    double LightingTimer = 0;

    string CurrentDirection = UP;

    //variables for scoring
    int StoneCounter = 0;
    int GoldCounter = 0;
    int MithrilCounter = 0;

    // Use this for initialization
    void Start()
    {
        AllBlockSprites = GetAllSpritesInScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMining && !IsLighting)
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
                //do they have lanterns to spare?
                if (LanternsPlaced <= LANTERNSALLOWED) { 
                    checkLight();
                }
                else
                {
                    //PLAY A NOPE SOUND
                }
            }
        }
        else if(IsMining)
        {
            MiningTimer += Time.deltaTime;
            ContinueMining();
        }
        else if (IsLighting)
        {
            LightingTimer += Time.deltaTime;
            ContinueLighting();
        }

    }

    //check if the player is able to place a light i.e. facing a block or wall
    void checkLight()
    {
        foreach (GameObject sp in AllBlockSprites)
        {

            Vector3 thebounds = sp.GetComponent<Collider2D>().bounds.size;
            //check for location
            switch (CurrentDirection)
            {
                case UP:
                    if ((sp.transform.position.x - thebounds.x / 2) <= gameObject.transform.position.x && (sp.transform.position.x + thebounds.x / 2) >= gameObject.transform.position.x && (sp.transform.position.y - thebounds.y / 2) >= gameObject.transform.position.y && (Mathf.Abs(Mathf.Abs(sp.transform.position.y - thebounds.y / 2) - Mathf.Abs(gameObject.transform.position.y)) < MiningModifier))
                    {
                        placeLight(sp);
                    }
                    break;
                case DOWN:
                    if ((sp.transform.position.x - thebounds.x / 2) <= gameObject.transform.position.x && (sp.transform.position.x + thebounds.x / 2) >= gameObject.transform.position.x && (sp.transform.position.y + thebounds.y / 2) <= gameObject.transform.position.y && (Mathf.Abs(Mathf.Abs(gameObject.transform.position.y) - Mathf.Abs(sp.transform.position.y + thebounds.y / 2)) < MiningModifier))
                    {
                        placeLight(sp);
                    }
                    break;
                case LEFT:
                    if ((sp.transform.position.y + thebounds.y / 2) >= gameObject.transform.position.y && (sp.transform.position.y - thebounds.y / 2) <= gameObject.transform.position.y && (sp.transform.position.x + thebounds.x / 2) <= gameObject.transform.position.y && (Mathf.Abs(Mathf.Abs(gameObject.transform.position.x) - Mathf.Abs(sp.transform.position.x + thebounds.x / 2)) < MiningModifier))
                    {
                        placeLight(sp);
                    }
                    break;
                case RIGHT:
                    if ((sp.transform.position.y + thebounds.y / 2) >= gameObject.transform.position.y && (sp.transform.position.y - thebounds.y / 2) <= gameObject.transform.position.y && (sp.transform.position.x - thebounds.x / 2) >= gameObject.transform.position.x && (Mathf.Abs(Mathf.Abs(sp.transform.position.x - thebounds.x / 2) - Mathf.Abs(gameObject.transform.position.x)) < MiningModifier))
                    {
                        placeLight(sp);
                    }
                    break;
                default:
                    //default to nothing
                    break;
            }
        }
    }

    void placeLight(GameObject sp)
    {
        IsLighting = true;
        CurrentlyLighting = sp;
    }

    void ContinueLighting()
    {
        if(LightingTimer >= LIGHTPLACETIME)
        {
            LightingTimer = 0;
            IsLighting = false;
            //create the new light
            float xCo = 0;
            float yCo = 0;
            Vector3 thebounds = CurrentlyLighting.GetComponent<Collider2D>().bounds.size;
            switch (CurrentDirection)
            {
                case UP:
                    yCo = CurrentlyLighting.transform.position.y - thebounds.y / 2;
                    xCo = CurrentlyLighting.transform.position.x;
                    break;
                case DOWN:
                    yCo = CurrentlyLighting.transform.position.y + thebounds.y / 2;
                    xCo = CurrentlyLighting.transform.position.x;
                    break;
                case LEFT:
                    yCo = CurrentlyLighting.transform.position.y;
                    xCo = CurrentlyLighting.transform.position.x + thebounds.x / 2;
                    break;
                case RIGHT:
                    yCo = CurrentlyLighting.transform.position.y;
                    xCo = CurrentlyLighting.transform.position.x - thebounds.x / 2;
                    break;
            }
            GameObject newLantern = Instantiate(Lantern, new Vector3(xCo, yCo, 0), Quaternion.identity);
            newLantern.transform.parent = CurrentlyLighting.transform;
            newLantern.SetActive(true);
            LanternsPlaced++;
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
                    if ((sp.transform.position.y + thebounds.y / 2) >= gameObject.transform.position.y && (sp.transform.position.y - thebounds.y / 2) <= gameObject.transform.position.y && (sp.transform.position.x + thebounds.x / 2) <= gameObject.transform.position.x && (Mathf.Abs(Mathf.Abs(gameObject.transform.position.x) - Mathf.Abs(sp.transform.position.x + thebounds.x / 2)) < MiningModifier))
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
            //add one to the appropriate counter
            BlockScript bs = CurrentlyMining.GetComponent<BlockScript>();
            switch (bs.blocktype)
            {
                case STONE:
                    StoneCounter++;
                    break;
                case GOLD:
                    GoldCounter++;
                    break;
                case MITHRIL:
                    MithrilCounter++;
                    if(MithrilCounter == MithrilRequired)
                    {
                        LevelWin();
                    }
                    break;
            }
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
                objectsInScene.Add(sp);
            }
        }

        return objectsInScene;
    }

    public void removeLantern()
    {
        LanternsPlaced--;
        Debug.Log(LanternsPlaced);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.name == "Lantern(Clone)")
        {
            if(MovementModifier < MAXMOVEMENTMODIFIER)
            {
                MovementModifier++;
                Debug.Log(MovementModifier);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Lantern(Clone)")
        {
            if (MovementModifier > MINMOVEMENTMODIFIER)
            {
                MovementModifier--;
                Debug.Log(MovementModifier);
            }
        }
    }

    public void LevelWin()
    {
        //YAY! The level was won
        Debug.Log("Congrats! You won the level!");
    }
}
