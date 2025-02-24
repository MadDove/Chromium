using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealth : Health
{
    [SerializeField] BossController boss;
    public float timeToDeactivate = 5f;

    public bool shouldChangeColor;

    public float vulnerableTime = 2f; // how long the object is vulnerable in seconds
    public float invulnerableTime = 5f; // how long the object is invulnerable in seconds

    private bool isVulnerable = true; // current vulnerability status
    private float lastChangeTime; // time of last status change

    void Start()
    {
        lastChangeTime = Time.time; // set last change time to current time
    }

    void Update()
    {
        float timeSinceLastChange = Time.time - lastChangeTime; // calculate time since last status change

        // check if it's time to change the vulnerability status
        if (timeSinceLastChange >= (isVulnerable ? vulnerableTime : invulnerableTime))
        {
            isVulnerable = !isVulnerable; // change vulnerability status
            lastChangeTime = Time.time; // update last change time
        }

        if (!IsVulnerable())
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    public bool IsVulnerable()
    {
        return isVulnerable; // return current vulnerability status
    }

    public override void Die()
    {
        boss.advanceGameStage();
        //invincibility phase
        Instantiate(corpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    //invincibility phase 

    public override IEnumerator DamageWithInvincible(float Iframes, float damage)
    {
        if (IsVulnerable())
        {
            Damage(damage);
            StartCoroutine(DamageFlash(Iframes));
            yield return null;
        }


    }

}