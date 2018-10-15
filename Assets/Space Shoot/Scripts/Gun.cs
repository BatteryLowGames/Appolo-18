using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gun : MonoBehaviour {

	//public ParticleSystem mzlFlsh;

	float magn = 1, rough = 1, fadeIn = 0.1f, fadeOut = 2f;

	//----------------------------for bullet shoot--------------------
	//Drag in the Bullet Emitter from the Component Inspector.
	public GameObject Bullet_Emitter;

	//Drag in the Bullet Prefab from the Component Inspector.
	public GameObject Bullet;

	//Enter the Speed of the Bullet from the Component Inspector.
	//public float Bullet_Forward_Force;

	//for audio
	private AudioSource audioSc;
	private Animator anim;
    public ParticleSystem ps;

    public LineRenderer laserLine;
    public Ray ray;
    private bool allowShooting;

    public bool rightGun;

    void Start () {
		audioSc = GetComponent<AudioSource> ();
		//Bullet_Forward_Force=250;
        anim= GetComponent<Animator>();
        laserLine.enabled = false;
        allowShooting = true;
    }

    // Update is called once per frame
    void Update () {
        if (!rightGun)
            return;
        if (MenuScripts.instance.gameStatus == GameStatus.Menu || MenuScripts.instance.gameStatus == GameStatus.HighScore)
            GameMenu();
	}

    public void Shoot()
	{
        if (GameManager.instance.isGameOver || MenuScripts.instance.gameStatus!=GameStatus.Game)
            return;

        if (!allowShooting)
            return;

        allowShooting = false;
        ps.Play();
		anim.Play ("Shoot");

        /*
		//The Bullet instantiation happens here.
		GameObject Temporary_Bullet_Handler;
        Temporary_Bullet_Handler = Instantiate(Bullet,Bullet_Emitter.transform.position,Bullet_Emitter.transform.rotation) as GameObject;
		Temporary_Bullet_Handler.transform.name="Bullet";
		//Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
		//This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.

		//Retrieve the Rigidbody component from the instantiated Bullet and control it.
		Rigidbody Temporary_RigidBody;
		Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

		//Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
		Temporary_RigidBody.AddForce(transform.right*-1 * Bullet_Forward_Force);

		//Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
		Destroy(Temporary_Bullet_Handler, 5.0f);
        */

        laserLine.enabled = true;
        laserLine.SetPosition(0,Bullet_Emitter.transform.position);

        ray.origin = Bullet_Emitter.transform.position;
        ray.direction = Bullet_Emitter.transform.right*-1;

        laserLine.SetPosition(1, ray.origin + ray.direction * 500);
        audioSc.Play();

        Invoke("DeactivateLaser",.1f);
        RaycastHit hittedObj;
        Debug.DrawRay(Bullet_Emitter.transform.position, Bullet_Emitter.transform.right * -500);
        if (Physics.Raycast(Bullet_Emitter.transform.position, Bullet_Emitter.transform.right * -1000, out hittedObj, 1000))
        {
            Zombie jibitoLash = hittedObj.transform.GetComponent<Zombie>();
            if (jibitoLash != null)
            {
                jibitoLash.GulliKha(10);
            }
        }
    }

    private void GameMenu()
    {
        laserLine.enabled = true;
        laserLine.SetPosition(0, Bullet_Emitter.transform.position);

        ray.origin = Bullet_Emitter.transform.position;
        ray.direction = Bullet_Emitter.transform.right * -1;

        laserLine.SetPosition(1, ray.origin + ray.direction * 500);

        RaycastHit hittedObj;
        if (Physics.Raycast(Bullet_Emitter.transform.position, Bullet_Emitter.transform.right * -1000, out hittedObj, 1000))
        {
            string btnName = hittedObj.transform.gameObject.name;
            if (btnName != null)
            {
                if (btnName == "PlayBtn")
                {
                    MenuScripts.instance.UpDown(0);
                }
                else if (btnName == "HighScore")
                {
                    MenuScripts.instance.UpDown(1);
                }
                else if (btnName == "Back")
                {
                    MenuScripts.instance.UpDown(2);
                }
                else
                {
                    MenuScripts.instance.MakeAllFalse();
                }
            }
        }

    }

    public void GameMenuShoot()
    {
        if (!rightGun)
            return;
        MenuScripts.instance.ButtonPressed();
        DeactivateLaser();
    }

    void DeactivateLaser()
    {
        laserLine.enabled = false;
        allowShooting = true;
    }

    public void OnCollisionEnter(Collision col)
	{

    }

	public void RotateGun()
	{
		//anim.Play ("rotation");
	}

	public void ReverseRotateGun()
	{
		//anim.Play ("reverseRotate");
	}
}
