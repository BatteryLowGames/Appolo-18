using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Zombie : MonoBehaviour {

	private int health;
	private int fullhealth=50;

	public float navigationUpdate;
	private float navigationTime;
	private float force = 1000f;
	public Image healthImg; 
	private bool notDead;
	private bool gotSHoot;
    public Animator anim;
	public Animator destroyAnim;
    public Vector3 playerPos;

	//audio source
	public AudioSource audioSc;
	public AudioClip shootAudio;
	public AudioClip deathAudio;
    public ParticleSystem ps;

	// Use this for initialization
	void Start () {
    }

	public void ZombieInitialize(Vector3 pos)
	{
        playerPos = pos;
        ps.Stop();
        //setting values
        health = 50;
		navigationTime=0;
		navigationUpdate=.05f;
		notDead = true;
        gotSHoot = false;
        anim.Play("Walk");
        ps.Stop();
        healthImg.fillAmount = 1;
	}

	// Update is called once per frame
	void Update () {
		
			navigationTime += Time.deltaTime;
			if (navigationTime > navigationUpdate) {
			if (notDead && !gotSHoot) {
                transform.position = Vector3.MoveTowards (transform.position, playerPos, .2f);
			}
			navigationTime = 0;
			}
	}


	public void GulliKha(int damage)
	{
        if (gotSHoot)
            return;

		if (notDead) {
            gotSHoot = true;
            anim.Play("Puke");
            health -= damage;

			healthImg.fillAmount = (float)health / fullhealth;
			if (health <= 0) {
				MorBeta ();
				return;
			}
			audioSc.PlayOneShot (shootAudio);
            Invoke("StartWalking",.6f);
		}
	}

    private void StartWalking()
    {
        gotSHoot = false;
        anim.Play("Walk");
    }

    private void MorBeta()
	{
		GameManager.instance.IncreaseScore ();
		anim.Play ("Death");
		notDead = false;
		audioSc.PlayOneShot (deathAudio);
        ps.Play(true);
		Invoke ("ScaleDownZombie", 3f);

	}

    public void ScaleDownZombie()
    {
        destroyAnim.Play("allienDestroy");
        Invoke("DestroyZombbie", 1f);
    }

    public void DestroyZombbie()
	{
		Destroy (gameObject);
	}

	public void OnCollisionEnter(Collision col)
	{

	}

	public void OnTriggerEnter(Collider col)
	{
        /*
        if (col.transform.name == "Bullet") {
			GulliKha (10);
            Destroy(col.gameObject);
		}
        */
	}

}
