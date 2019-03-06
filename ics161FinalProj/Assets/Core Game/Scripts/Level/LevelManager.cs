using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject collectiblesPanel; //the panel to add the key sprites to
    [SerializeField] Sprite keySpriteFilled; //key sprite filled
    [SerializeField] Sprite keySpriteEmpty; //key sprite outline
    [SerializeField] float distanceBetween; //distance between the sprites in the upper left hand corner

    //public static LevelManager instance;
    GameObject[] collectibles; //used only for setting the index of the collectibles 
    Image[] collectiblesSprites; //Stores the images of the sprites in the upper left in order to easily update them 
    public bool[] collected; //The array that stores whether or not a Collectible has been collected

    public UnityEvent FinishedLevel;
    //private bool levelOver = false;

    int totalCollectibles;
    int itemsCollected = 0;

    bool gameOver = false;

    PlayerLevelMovement playerMovement;
    PlayerLevelInteraction playerInteraction;

    void Awake()
    {
        FinishedLevel = new UnityEvent();
        StartCoroutine(TransitionManager.instance.FadeToLevel(1.5f));
    }

    private void Start()
    {
        SetDeathBlockListener();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevelMovement>();
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevelInteraction>();
        collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        collectiblesSprites = new Image[collectibles.Length];
        collected = new bool[collectibles.Length];
        totalCollectibles = collectibles.Length;
        for(int i = 0; i < collectibles.Length; ++i)
        {
            Collectible collComponent = collectibles[i].GetComponent<Collectible>();
            collComponent.setIndex(i);
            collComponent.Collected.AddListener(CollectedListener);
        }
        CreateCollectiblesPanel();
    }

    //Adds the listener to the GameOver event of the DeathBlock
    void SetDeathBlockListener()
    {
        GameObject deathBlock = GameObject.FindGameObjectWithTag("DeathBlock");
        if(deathBlock != null)
        {
            deathBlock.GetComponent<KillPlayerScript>().GameOver.AddListener(GameOverListener);
        }
    }

    //Tells the ExitDoorScript that the player can exit - ExitDoorScript is kind of unnecessary, I can merge it with this script later if you want)
    void checkExit()
    {
        if (itemsCollected == totalCollectibles)
            FinishedLevel.Invoke();
    }

    //Updates the collectibles panel, and sets the collectible to true in the array. Checks if the player can exit (if all collectibles have been obtained)
    void CollectedListener(int index)
    {
        collected[index] = true;
        UpdateCollectiblesPanel(index);
        itemsCollected += 1;
        checkExit();
    }

    //Passes the array of bools representing the collectibles obtained to SaveFileManager (to pass to the StageHubScript to update the StagePanel in the test map) and then loads the Test Map scene
    public void PassDataToSaveManager()
    {
        Time.timeScale = 1;    //allows the fade coroutine to run when player wants to quit back to map
        playerInteraction.enabled = false;
        playerMovement.enabled = false;
        FindObjectOfType<AudioManager>().Stop("SPRING"); //this needs to be replaced with a variable holding the current song being played
        StartCoroutine(BackToMapCR()); 
    }

    //Creates the collectible icons in the top left corner
    void CreateCollectiblesPanel()
    {
        RectTransform r = collectiblesPanel.GetComponent<RectTransform>();
        Vector3 leftBorder = new Vector3(r.rect.xMin, r.transform.position.y);
        for (int i = 0; i < totalCollectibles; i++)
        {
            GameObject newImage = new GameObject();
            newImage.transform.SetParent(collectiblesPanel.transform, false);
            newImage.transform.localScale = newImage.transform.localScale * 0.7f;
            RectTransform rect = newImage.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(leftBorder.x + distanceBetween/2 + distanceBetween * i, newImage.transform.localPosition.y);

            Image sr = newImage.AddComponent<Image>();
            sr.sprite = keySpriteEmpty;
            collectiblesSprites[i] = sr;
        }
    }

    //Updates the empty sprite to the filled sprite when a collectible is collecfted
    void UpdateCollectiblesPanel(int index)
    {
        collectiblesSprites[index].sprite = keySpriteFilled;
    }

    //Listens for the GameOver event given by deathblocks
    void GameOverListener()
    {
        FreezePlayer();
        GameOverPanel.SetActive(true);
    }

    //Constrains the player so animator doesn't update nor can the player move
    public void FreezePlayer()
    {
        playerMovement.frozen = true;
        playerMovement.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    //Restarts the level
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Checks if the game is over
    public bool isGameOver()
    {
        return gameOver;
    }

    public IEnumerator BackToMapCR()
    {

        while(AudioManager.instance.CR_running)
        {
            yield return null;
        }
        playerMovement.enabled = true;
        playerInteraction.enabled = true;
        SaveFileManager.instance.AddCollectablesCountToCurrentState(collected);
        SceneManager.LoadScene("TestMap");


    }
}
