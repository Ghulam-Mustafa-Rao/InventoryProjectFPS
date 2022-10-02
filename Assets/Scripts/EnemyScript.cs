using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyScript : Character
{
    NavMeshAgent agent;
    [SerializeField]
    Vector3[] targetLocations;
    [SerializeField]
    float botShootWait;
    public Vector3 curentTarget;
    public bool targetLocked = false;
    public GameObject player;
    bool botReadyToShot = true;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager.chracters.Add(this);
        GameManager.gameManager.charactersNameKills.Add(cName, kills);
        //Debug.LogError(GameManager.gameManager.chracters.Count);
        agent = GetComponent<NavMeshAgent>();     
        changeTarget();
        Invoke("SetPlayer", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.gameStarted && !GameManager.gameManager.gameOver)
        {
            if (curentTarget.x == transform.position.x
                       && curentTarget.z == transform.position.z
                       && !targetLocked)
            {
                changeTarget();
            }
            if (targetLocked)
            {
                curentTarget = player.transform.position;
            }
            agent.SetDestination(curentTarget);

            if (botReadyToShot && playerInRange())
            {
                if (agentLookingAtPlayer())
                {
                    Shoot();
                    StartCoroutine(ShootWait());
                }
            }
        }
       
    }

    private void FixedUpdate()
    {

    }
    bool playerInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentGun.GetComponent<GunHandler>().range);

        foreach (var item in colliders)
        {
            if (player == item.gameObject)
            {
                curentTarget = player.transform.position;
                targetLocked = true;
                return true;
            }
        }

        return false;
    }

    bool agentLookingAtPlayer()
    {
        Vector3 dirFromAtoB = (player.transform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        if(dotProd > 0.9)
        {
            return true;
        }

        return false;
    }

    void changeTarget()
    {
        curentTarget = targetLocations[Random.Range(0, targetLocations.Length)];
    }

    void SetPlayer()
    {
        foreach (var item in GameManager.gameManager.chracters)
        {
            if (item.cName == "Player")
            {
                player = item.gameObject;
            }
        }
    }

    IEnumerator ShootWait()
    {
        botReadyToShot = false;
        yield return new WaitForSeconds(botShootWait);
        botReadyToShot = true;
    }
}
