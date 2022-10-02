using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Private")]
    [SerializeField]
    public int health = 100;
    [SerializeField]
    List<string> abilityNames;
    [SerializeField]
    List<int> abilityQuantity;

    [Header("Public")]
    public string cName;
    public float speed = 10;
    public float maxSpeed = 9;
    public float rotateSpeed = 5;
    public int kills = 0;
    //                Name  , Quantity   
    public Dictionary<string, int> abilities;
    //sspublic List<GameObject> availableGuns;
    public GameObject currentGun;
    public Rigidbody rigidbody;
    public GameObject GunHolder;

    private void Awake()
    {
        
        abilities = new Dictionary<string, int>();
        for (int i = 0; i < abilityNames.Count; i++)
        {
            abilities.Add(abilityNames[i], abilityQuantity[i]);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        currentGun.GetComponent<GunHandler>().ShootBullet(this.gameObject);
    }

    public void TakeDamage(Character character)
    {
        if (GameManager.gameManager.specialAmmoEnabled)
        {
            health -= character.currentGun.GetComponent<GunHandler>().damage * 2;
        }
        else
        {
            health -= character.currentGun.GetComponent<GunHandler>().damage;
        }
           
        if (health <= 0)
        {
            
            //give Kill to Character
            character.kills++;
            
            //Update Kills in Leaderboard
            GameManager.gameManager.charactersNameKills[character.cName] = character.kills;
            //Update Leaderboard
            GameManager.gameManager.LeaderBoard();

            if (character.cName == "Player")
            {
                //Show Gun Name
                GameManager.gameManager.ShowGunName(character.currentGun.name);

            }
            //If player's health is zero then its a GameOver
            if (cName == "Player")
            {
                GameManager.gameManager.gameOver = true;
                return;
            }
            //Kill Character
            Destroy(this.gameObject);
        }
    }


    public void SetCurrentGun(int gunNo)
    {
        if (currentGun != null)
            Destroy(currentGun);

        currentGun = Instantiate(GameManager.gameManager.guns[gunNo], GunHolder.transform.position, GameManager.gameManager.guns[gunNo].transform.rotation);
        currentGun.transform.SetParent(GunHolder.transform);
        currentGun.transform.eulerAngles = new Vector3(currentGun.transform.eulerAngles.x, 0, 0);
        currentGun.transform.localScale = Vector3.one;
    }

    public void AddAbility(string abilityName)
    {
        abilities[abilityName]++;
    }

    public void SubTractAbility(string abilityName)
    {
        abilities[abilityName]--;
    }

    public void InitialisePlayer(int initialGunIndex, int playerTypeIndex)
    {
        SetCurrentGun(initialGunIndex);
    }

}
