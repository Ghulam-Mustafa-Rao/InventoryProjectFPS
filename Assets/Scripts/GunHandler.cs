using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    //Has To inherit MonoBehaviour to attach it to GameObject

    public int damage;
    public int magSize;
    public float range;

    [SerializeField]
    int bulletsInMag;
    [SerializeField]
    float reloadTimeWait;
    [SerializeField]
    float waitForNextBulletToBeFired;
    [SerializeField]
    bool isReadyToFire = true;
    [SerializeField]
    ParticleSystem hitEffect;
    [SerializeField]
    GameObject shootEffect;

    private void Awake()
    {
        shootEffect = transform.Find("ShootEffect").gameObject;
    }

    public void ShootBullet(GameObject whoShootTheBullet)
    {
        if (isReadyToFire)
        {
            //Add particle effect (A Muzzel Flash)
            if (!shootEffect.active)
                StartCoroutine(ShootEffect());

            RaycastHit hit;
            if (Physics.Raycast(whoShootTheBullet.transform.position, whoShootTheBullet.transform.forward, out hit, range))
            {
                //Debug.LogError(hit.transform.gameObject.tag);
                //Instantiat hit effect
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));

                if (hit.transform.gameObject.CompareTag("Chracter"))
                {
                    hit.transform.gameObject.GetComponent<Character>().TakeDamage(whoShootTheBullet.GetComponent<Character>());
                }
            }

            bulletsInMag--;
            isReadyToFire = false;
            if (bulletsInMag <= 0)
            {
                StartCoroutine(ReloadGun());
            }
            else
            {
                StartCoroutine(WaitForNextBulletToBeFired());
            }
        }
    }

    IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(reloadTimeWait);
        bulletsInMag = magSize;
        isReadyToFire = true;
    }
    IEnumerator WaitForNextBulletToBeFired()
    {
        yield return new WaitForSeconds(waitForNextBulletToBeFired);
        isReadyToFire = true;
    }

    IEnumerator ShootEffect()
    {
        shootEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        shootEffect.SetActive(false);
    }
}
