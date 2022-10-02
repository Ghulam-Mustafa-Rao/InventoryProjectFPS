using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public List<Character> chracters;
    public bool gameOver = false;
    public GameObject GameOverText;
    [SerializeField]
    GameObject killedByGunNameText;
    //For Leaderboard
    public Dictionary<string, int> charactersNameKills;
    public bool specialAmmoEnabled = false;
    [SerializeField]
    GameObject specialAmmoCounterText;

    public List<GameObject> guns;
    public List<GameObject> playerTypes;
    public InitialSelection initialSelection;

    [SerializeField]
    GameObject gunsHolderInventory;
    [SerializeField]
    GameObject gunPanel;
    [SerializeField]
    GameObject playerTypesHolderInventory;
    [SerializeField]
    GameObject playerTypePanel;
    [SerializeField]
    GameObject InventoryPanel;
    [SerializeField]
    GameObject leaderboardHolder;
    [SerializeField]
    GameObject leaderboardPanel;

    public bool gameStarted = false;

    List<GameObject> gunsInInventory;
    List<GameObject> playerTypesInInventory;
    [SerializeField]
    List<GameObject> leaderboardObjects;
    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        initialSelection = new InitialSelection();
        chracters = new List<Character>();
        leaderboardObjects = new List<GameObject>();
        charactersNameKills = new Dictionary<string, int>();
        InventoryPanel.SetActive(true);

    }
    // Start is called before the first frame update
    void Start()
    {
        CreateInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowGunName(string gunName)
    {
        StopCoroutine(ShowGunNameCo(gunName));
        StartCoroutine(ShowGunNameCo(gunName));
    }

    IEnumerator ShowGunNameCo(string gunName)
    {
        killedByGunNameText.GetComponent<TextMeshProUGUI>().text = "Killed By Gun : " + gunName;
        killedByGunNameText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        killedByGunNameText.SetActive(false);
    }

    public void LeaderBoard()
    {
        foreach (var item in leaderboardObjects)
        {
            leaderboardObjects.Remove(item);
            Destroy(item);
        }
        List<KeyValuePair<string, int>> myList = new List<KeyValuePair<string, int>>(charactersNameKills);
        myList.Sort(delegate (KeyValuePair<string, int> firstPair, KeyValuePair<string, int> nextPair)
    {
        return firstPair.Value.CompareTo(nextPair.Value);
    });
        int i = 0;
        foreach (var item in myList)
        {
            GameObject lbPanel = Instantiate(leaderboardPanel, leaderboardHolder.transform.position, Quaternion.identity);
            lbPanel.transform.SetParent(leaderboardHolder.transform);
            lbPanel.transform.Find("Position").gameObject.GetComponent<TextMeshProUGUI>().text = i.ToString();
            lbPanel.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = item.Key;
            lbPanel.transform.Find("kills").gameObject.GetComponent<TextMeshProUGUI>().text = item.Value.ToString();
            lbPanel.transform.localScale = Vector3.one;
            lbPanel.transform.eulerAngles = Vector3.zero;
            leaderboardObjects.Add(lbPanel);
            i++;
        }



    }

    IEnumerator SpecialAmmoCo()
    {
        specialAmmoCounterText.SetActive(false);
        yield return new WaitForSeconds(5);
        specialAmmoCounterText.SetActive(true);
        specialAmmoEnabled = true;
        for (int i = 10; i > 0; i--)
        {
            specialAmmoCounterText.GetComponent<TextMeshProUGUI>().text = "Special Ammo Counter : " + i;
            yield return new WaitForSeconds(1);
        }
        specialAmmoCounterText.SetActive(false);
        specialAmmoEnabled = false;
    }

    public void CreateInventory()
    {

        int i = 0;
        gunsInInventory = new List<GameObject>();
        playerTypesInInventory = new List<GameObject>();
        //Setup Guns in inventory
        foreach (var item in guns)
        {

            GameObject temp = Instantiate(gunPanel, gunsHolderInventory.transform.position, Quaternion.identity);
            temp.transform.SetParent(gunsHolderInventory.transform);

            Debug.LogError(i + " : " + temp);
            temp.transform.Find("Image").GetComponent<Image>().color = item.GetComponent<Renderer>().sharedMaterial.color;
            temp.transform.Find("Damage").GetComponent<TextMeshProUGUI>().text = "Damage : " + item.GetComponent<GunHandler>().damage.ToString();
            temp.transform.localScale = Vector3.one;

            //Select 1st gun by default
            if (i == 0)
                temp.GetComponent<Image>().color = Color.green;
            temp.name = i.ToString();
            gunsInInventory.Add(temp);
            temp.GetComponent<Button>().onClick.AddListener(delegate { GameManager.gameManager.SetPlayerGun(temp.gameObject); });
            i++;

        }
        i = 0;

        //Setup PlayerTypes in inventory
        foreach (var item in playerTypes)
        {

            GameObject temp = Instantiate(playerTypePanel, playerTypesHolderInventory.transform.position, Quaternion.identity);
            temp.transform.SetParent(playerTypesHolderInventory.transform);

            temp.transform.Find("Image").GetComponent<Image>().color = item.GetComponent<Image>().color;
            temp.transform.localScale = Vector3.one;

            //Select 1st playerType by default
            if (i == 0)
                temp.GetComponent<Image>().color = Color.green;

            temp.name = i.ToString();
            playerTypesInInventory.Add(temp);
            temp.GetComponent<Button>().onClick.AddListener(() => GameManager.gameManager.SetPlayerType(temp));
            i++;
        }
    }

    public void SetPlayerGun(GameObject button)
    {
        int GunIndex = int.Parse(button.name);
        foreach (var item in gunsInInventory)
        {
            item.GetComponent<Image>().color = Color.white;
        }

        initialSelection.initialGunIndex = GunIndex;
        button.GetComponent<Image>().color = Color.green;
        //Debug.LogError("GunIndex  : " + GunIndex + " : " + button);
    }

    public void SetPlayerType(GameObject button)
    {
        int PlayerIndex = int.Parse(button.name);
        foreach (var item in playerTypesInInventory)
        {
            item.GetComponent<Image>().color = Color.white;
        }

        initialSelection.playerTypeIndex = PlayerIndex;
        button.GetComponent<Image>().color = Color.green;

        //Debug.LogError("PlayerIndex  : " + PlayerIndex);
    }

    public void StartGame()
    {
        foreach (var item in chracters)
        {
            if (item.cName == "Player")
            {
                item.InitialisePlayer(initialSelection.initialGunIndex, initialSelection.playerTypeIndex);
            }
            else
            {
                item.InitialisePlayer(Random.Range(0, guns.Count), Random.Range(0, playerTypes.Count));
            }
        }

        InventoryPanel.SetActive(false);
        StartCoroutine(SpecialAmmoCo());
        gameStarted = true;
        LeaderBoard();
    }
}
