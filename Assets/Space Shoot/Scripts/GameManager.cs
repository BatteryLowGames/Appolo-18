using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//UI elemets
	public Text scoreText;
	public Animator canvasAnim;
	public GameObject instruction;

	//game elements
	public GameObject zombie;
	public GameObject portals;
	public GameObject leftGun;
	public GameObject rightGun;
    public GameObject playerPos;
    private GameObject  [] zombies;
	public Transform[] entryPoints;
	public ParticleSystem[] particleSystem;

    //logical variables
    private float time;
	private int count ;
	private int score ;
	private int zombieNumberInc;
	private int zombieNumberMinmum;
    private short zombieCount;
	public bool isGameOver;
    private AudioSource gameOverSound;

    public static GameManager instance;

	private List<int> zombieList;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}


	// Use this for initialization
	void Start () {
        if (!PlayerPrefs.HasKey("HIGHSCORE"))
        {
            PlayerPrefs.SetInt("HIGHSCORE", 0);
        }
        portals.SetActive(false);
        //MakeAllReady ();
    }

    public void MakeAllReady()
	{
        
        //init values
        portals.SetActive(true);
        leftGun.SetActive(true);
        rightGun.SetActive(true);
        time =10f;
		score = 0;
        zombieNumberInc = 2;
        zombieNumberMinmum = 1;
        scoreText.text=score+" Killed";
		isGameOver=false;
		zombieList = new List<int> ();
		canvasAnim.Play ("Default");
        gameOverSound = GetComponent<AudioSource>();
        for (int i = 0; i < particleSystem.Length; i++)
        {
            particleSystem[i].Play();
        }
        Invoke("StartAllCoRoutine",4f);
	}

	private void StartAllCoRoutine()
	{
		StartCoroutine(RandomZombieCreator());
		StartCoroutine(RandomZombiePosClear());
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
		else if (Input.GetKeyDown (KeyCode.Space))
			RestartGame ();
	}
	private void CreateZombie()
	{
		int point = Random.Range (0, entryPoints.Length);
		if (zombieList.Contains (point))
			return;
		zombieList.Add (point);
		GameObject zombieClone = Instantiate (zombie);
		zombieClone.transform.name = "Allien";
		zombieClone.transform.SetParent(entryPoints [point].transform);
		zombieClone.transform.position=entryPoints [point].transform.position;
		zombieClone.transform.rotation=entryPoints [point].transform.rotation;
        zombieClone.GetComponent<Zombie> ().ZombieInitialize(playerPos.transform.position);
	}

	IEnumerator RandomZombieCreator()
	{
		int zombieNum=Random.Range (zombieNumberMinmum, zombieNumberInc);
		for (short i = 0; i < zombieNum; i++) {
			CreateZombie ();
		}
			
		if (time >3) {
			time -= .5f;
        }

		if (time > 0) {
            if (zombieNumberInc <= 7)
            {
                Debug.Log("Baraw");
                zombieNumberInc++;
                zombieNumberMinmum= zombieNumberInc-1;
            }
            yield return new WaitForSeconds (time);
        }

		if(!isGameOver)
			StartCoroutine(RandomZombieCreator());
	}
		
	IEnumerator RandomZombiePosClear()
	{
		if(zombieList.Count>0)
			zombieList.RemoveAt (0);
	
		yield return new WaitForSeconds (4f);

		if(!isGameOver)
			StartCoroutine(RandomZombiePosClear());
	}


	public void GameOver(ParticleSystem gmps)
	{
        if (isGameOver)
            return;
        gmps.Play();
        Debug.Log ("Game Over");
        gameOverSound.Play();
        canvasAnim.Play ("RevealGameOver"); 
         isGameOver = true;

		StopCoroutine(RandomZombieCreator());
		StopCoroutine(RandomZombiePosClear());
        for (int i = 0; i < particleSystem.Length; i++)
        {
            particleSystem[i].Stop(); ;
        }
        leftGun.SetActive(false);
        rightGun.SetActive(false);
        Invoke("InActiveThePortals",5f);

        int highScore=PlayerPrefs.GetInt("HIGHSCORE");
        if(highScore<score)
        {
            PlayerPrefs.SetInt("HIGHSCORE",score);
            scoreText.text = "Congratulations you scored highest " + score + " E.T Killed";
        }

    }

    public void InActiveThePortals()
    {
        portals.SetActive(false);
        //delete all alliens
        foreach (Transform child in entryPoints)
        {
            foreach (Transform grandChild in child)
                GameObject.Destroy(grandChild.gameObject);
        }
        MenuScripts.instance.ActiveUICanvas();
        canvasAnim.Play("HideGameOver");
        leftGun.SetActive(true);
        rightGun.SetActive(true);
    }

    public void IncreaseScore()
	{
		Debug.Log ("IncreaseScore");
		score++;
		scoreText.text=score+" Killed";
	}

	public void RestartGame()
	{
		isGameOver = true;
        //SceneManager.LoadScene (0);
        /*
        for (int i = 0; i < entryPoints.Length; i++) {
			Zombie z=entryPoints [i].GetComponentInChildren<Zombie> ();
			if(z!=null)
				z.DestroyZombbie ();
		}
        */
       
        MakeAllReady ();
	}

}
