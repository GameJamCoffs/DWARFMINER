using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour {

    public GameObject Lantern;
    public GameObject InitialBlock;

    const string UP = "UP";
    const string DOWN = "DOWN";
    const string LEFT = "LEFT";
    const string RIGHT = "RIGHT";

    const string STONE = "STONE";
    const string GOLD = "GOLD";
    const string MITHRIL = "MITHRIL";

    double MINESTONETIME = 1;
    double MINEGOLDTIME = 2;
    double MINEMITHRILTIME = 3;

    double LIGHTPLACETIME = 3;

    int LANTERNSALLOWED = 10;

    const int MINMOVEMENTMODIFIER = 50;
    const int MAXMOVEMENTMODIFIER = 250;

    const int GOLDMODIFIER = 8;

    public Text StoneText;
    public Text GoldText;
    public Text MithrilText;

    //level specific variables
    int TheLevel = 1;
    int LanternsPlaced = 0;
    int MithrilRequired = 20;

    List<GameObject> AllBlockSprites;

    GameObject CurrentlyMining;
    GameObject CurrentlyLighting;

    int MovementModifier = 150;
    double MiningModifier = 20;

    bool IsMining = false;
    double MiningTimer = 0;

    bool IsLighting = false;
    double LightingTimer = 0;

    string CurrentDirection = UP;

    //variables for scoring
    int StoneCounter = 0;
    int GoldCounter = 0;
    int MithrilCounter = 0;

    double DarknessSpawnTimer = 0;
    public GameObject Darkness;

    int MapSizeModifier = 15;

    // Use this for initialization
    void Start()
    {
        GenerateMap();
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
            DarknessSpawnTimer += Time.deltaTime;
            if(DarknessSpawnTimer >= (25 - TheLevel))
            {
                //spawn a new darkness
                GameObject NewDarkness = Instantiate(Darkness, new Vector3(1300, 1300, -100), Quaternion.identity);
                NewDarkness.SetActive(true);
                DarknessSpawnTimer = 0;
            }
        }
        else if(IsMining)
        {
            MiningTimer += Time.deltaTime;
            ContinueMining(CurrentlyMining);
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
                    if ((sp.transform.position.y + thebounds.y / 2) >= gameObject.transform.position.y && (sp.transform.position.y - thebounds.y / 2) <= gameObject.transform.position.y && (sp.transform.position.x + thebounds.x / 2) <= gameObject.transform.position.x && (Mathf.Abs(Mathf.Abs(gameObject.transform.position.x) - Mathf.Abs(sp.transform.position.x + thebounds.x / 2)) < MiningModifier))
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
            if (sp != null)
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
                        if ((sp.transform.position.y + thebounds.y / 2) >= gameObject.transform.position.y && (sp.transform.position.y - thebounds.y / 2) <= gameObject.transform.position.y && (sp.transform.position.x - thebounds.x / 2) >= gameObject.transform.position.x && (Mathf.Abs(Mathf.Abs(sp.transform.position.x - thebounds.x / 2) - Mathf.Abs(gameObject.transform.position.x)) < MiningModifier))
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
    }

    //function to handle mining
    void startMining(GameObject BlockBeingMined)
    {
        IsMining = true;
        CurrentlyMining = BlockBeingMined;
    }

    void ContinueMining(GameObject BlockBeingMined)
    {
        double TimeNeeded = MINESTONETIME;
        switch (BlockBeingMined.GetComponent<BlockScript>().blocktype)
        {
            case STONE:
                TimeNeeded = MINESTONETIME;
                break;
            case GOLD:
                TimeNeeded = MINEGOLDTIME;
                break;
            case MITHRIL:
                TimeNeeded = MINEMITHRILTIME;
                break;
            default:
                TimeNeeded = MINESTONETIME;
                break;
        }

        if(MiningTimer >= TimeNeeded)
        {
            //mining is finished
            //add one to the appropriate counter
            BlockScript bs = CurrentlyMining.GetComponent<BlockScript>();
            switch (bs.blocktype)
            {
                case STONE:
                    StoneCounter++;
                    StoneText.text = "Stone: " + StoneCounter;
                    break;
                case GOLD:
                    GoldCounter++;
                    GoldText.text = "Gold: " + GoldCounter;
                    break;
                case MITHRIL:
                    MithrilCounter++;
                    MithrilText.text = "Mithril: " + MithrilCounter;
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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Lantern(Clone)")
        {
            if(MovementModifier < MAXMOVEMENTMODIFIER)
            {
                MovementModifier += 20;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Lantern(Clone)")
        {
            if (MovementModifier > MINMOVEMENTMODIFIER)
            {
                MovementModifier -= 20;
            }
        }
    }

    public void LevelWin()
    {
        //YAY! The level was won
        Debug.Log("Congrats! You won the level!");
        //respawn player
        Vector3 position = new Vector3(-50, -50, 0);
        gameObject.transform.position = position;

        //increase the level
        TheLevel++;

        //increase mining time
        MINEGOLDTIME = MINEGOLDTIME + 0.2;
        MINESTONETIME = MINESTONETIME + 0.2;
        MINEMITHRILTIME = MINEMITHRILTIME + 0.2;

        //increase lantern place time
        LIGHTPLACETIME = LIGHTPLACETIME + 0.2;
 
        //increase lanterns allowed
        LANTERNSALLOWED += 2;

        //increase Mithril required
        MithrilRequired += 2;

        if (TheLevel > 20)
        {
            //They have won the game
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        //reset lanterns just in case
        LanternsPlaced = 0;

        //need to load the next level
        MithrilCounter = 0;
        MithrilText.text = "Mithril: " + MithrilCounter;

        //remove all sprites
        AllBlockSprites = GetAllSpritesInScene();

        foreach (GameObject sp in AllBlockSprites)
        {
            if (sp.name == "Block(Clone)" || sp.name == "Darkness(Clone)" || sp.name == "Lantern(Clone)")
            {
                Destroy(sp);
            }
        }
        //increase the map size
        MapSizeModifier += 2;
        GenerateMap();
        AllBlockSprites = GetAllSpritesInScene();
    }

    public void LoseGame()
    {
        //AW NO! The darkness got you!
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void GenerateMap()
    {
        //get the height and width of the map
        float MapRandom = Random.value;
        int MapDimension = (int)Mathf.Round(MapRandom * 5) + MapSizeModifier;

        int WidthCounter = 0;
        int HeightCounter = 0;
        double xCo = 0;
        double yCo = 0;

        int TotalBlocks = 0;

        int StoneCounter = 0;
        int GoldCounter = 0;
        int MithrilCounter = 0;


        double InitialBlockSize = InitialBlock.GetComponent<Renderer>().bounds.size.x - 10;

        while (HeightCounter < MapDimension)
        {
            while(WidthCounter < MapDimension)
            {
                //set the new block
                GameObject newBlock = Instantiate(InitialBlock, new Vector3((float)xCo, (float)yCo, 0), Quaternion.identity);
                //choose whether it is stone, gold or mithril
                int TypeSelector = (int)Mathf.Round(Random.value * 10);
                switch (TypeSelector)
                {
                    case 0:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    case 1:
                        if (GoldCounter < MithrilRequired * GOLDMODIFIER)
                        {
                            newBlock.GetComponent<BlockScript>().blocktype = GOLD;
                            GoldCounter++;
                        }
                        else
                        {
                            newBlock.GetComponent<BlockScript>().blocktype = STONE;
                            StoneCounter++;
                        }
                        break;
                    case 2:
                        //stop the mithril being in the first two rows
                        if (MithrilCounter <= MithrilRequired && HeightCounter > 2)
                        {
                            //more even spread
                            if (MithrilCounter > MithrilRequired / 2 && HeightCounter < 10)
                            {
                                newBlock.GetComponent<BlockScript>().blocktype = STONE;
                                StoneCounter++;
                            }
                            else
                            {
                                newBlock.GetComponent<BlockScript>().blocktype = MITHRIL;
                                MithrilCounter++;
                            }
                        }
                        else
                        {
                            newBlock.GetComponent<BlockScript>().blocktype = STONE;
                            StoneCounter++;
                        }
                        break;
                    case 3:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    case 4:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    case 5:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    case 6:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    case 7:
                        if (GoldCounter < MithrilRequired * GOLDMODIFIER)
                        {
                            newBlock.GetComponent<BlockScript>().blocktype = GOLD;
                            GoldCounter++;
                        }
                        else
                        {
                            newBlock.GetComponent<BlockScript>().blocktype = STONE;
                            StoneCounter++;
                        }
                        break;
                    case 8:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    case 9:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    case 10:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;
                    default:
                        newBlock.GetComponent<BlockScript>().blocktype = STONE;
                        StoneCounter++;
                        break;

                }
                newBlock.SetActive(true);
                TotalBlocks++;
                WidthCounter++;

                //get new coordinates
                xCo += InitialBlockSize;
            }
            WidthCounter = 0;
            yCo += InitialBlockSize;
            xCo = 0;
            HeightCounter++;
        }

        if(MithrilCounter < MithrilRequired)
        {
            //there is not enough randomly generate mithril, we need to add some more
            AllBlockSprites = GetAllSpritesInScene();
            bool StopChecking = false;

            foreach (GameObject sp in AllBlockSprites)
            {
                if (sp.name == "Block(Clone)")
                {
                    if (sp.GetComponent<BlockScript>().blocktype != MITHRIL && !StopChecking)
                    {
                        sp.GetComponent<BlockScript>().blocktype = MITHRIL;
                        MithrilCounter++;
                        if (MithrilCounter >= MithrilRequired)
                        {
                            StopChecking = true;
                        }
                    }
                }
            }
        }
    }

    public int GetTheLevel()
    {
        return TheLevel;
    }
}
