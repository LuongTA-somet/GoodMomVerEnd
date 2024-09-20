using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private Animator momAnimator;
    [SerializeField] private Animator dadAnimator;
    [SerializeField] private GameObject goodDress;
    [SerializeField] private GameObject badDress;
    [SerializeField] private GameObject curDress;
    [SerializeField] private GameObject WinPos;
    [SerializeField] private GameObject GoodFxPrefab;
    [SerializeField] private GameObject GoodFxPrefab2;
    [SerializeField] private GameObject GoodFxPrefab3;
    [SerializeField] private GameObject BadFxPrefab;
    [SerializeField] private GameObject BadFxPrefab2;
    [SerializeField] private GameObject BadFxPrefab3;
   

    public GameObject mom;
    public GameObject dad;
    public CameraFollow cameraFollow;
    public Score score;
    public Timer timer;
    public bool isGood = false;
    public bool isGood2 = false;

    public bool isBad = false;
    public bool isBad2 = false;

    public bool isImpact = false;
    //    public int gate = 1;
    public float olDis;
    public float curDis;

    private Vector3 startMousePos;
    private Vector3 endMousePos;
    private bool fingerDown = false;
    private bool isWin = false;
    private bool tutorial = true;

    [Range(0, 10)]
    public float finalScore = 5;
    private void Start()
    {
        olDis = Vector3.Distance(transform.position, WinPos.transform.position);
    }

    void Update()
    {
        //if (cameraFollow.demoCamera) return;
        //if (tutorial)
        //{
        //    GameManager.instance.UIManager.Tutorial(true);
        //}
        StartCoroutine(Tutorial());

        if (finalScore >= 7)
        {
            score.UpdateScore("Good", Color.green);
        }
        else if (finalScore < 7 && finalScore >= 4)
        {
            score.UpdateScore("AVG", Color.yellow);
        }
        else
        {
            score.UpdateScore("Bad", Color.red);
        }

        if (isWin) return;
        finalScore = Mathf.Clamp(finalScore, 0, 10);

        Move();

    }
    public void Move()
    {
        if (tutorial == false)
        {
            GameManager.instance.soundManager.bg.mute = false;

            score.SetScore(finalScore);
            score.ActiveScore();
            timer.SetTimer(olDis - curDis, olDis);
            timer.ActiveTimer();

            curDis = Vector3.Distance(transform.position, WinPos.transform.position);
            gameObject.transform.position += new Vector3(0, 0, 0.115f);
            if (isWin == false)
            {
                momAnimator.SetBool("Walk", true);
                momAnimator.SetBool("idle", false);
                dadAnimator.SetBool("Idle", false);
                dadAnimator.SetBool("Walk", true);
            }
        }
        if (fingerDown == false && Input.GetMouseButtonDown(0))
        {
            tutorial = false;
            GameManager.instance.UIManager.Tutorial(false);
            startMousePos = Input.mousePosition;
            fingerDown = true;
        }
        if (fingerDown)
        {

            if (Input.mousePosition.x > startMousePos.x)
            {
                transform.position += new Vector3(0.1f, 0, 0);
                if (transform.position.x >= 1.6f)
                {
                    transform.position = new Vector3(1.6f, 0, transform.position.z);
                }
            }
            if (Input.mousePosition.x < startMousePos.x)
            {
                gameObject.transform.position += new Vector3(-0.1f, 0, 0);
                if (transform.position.x <= -1.6f)
                {
                    transform.position = new Vector3(-1.6f, 0, transform.position.z);
                }
            }
        }
        if (fingerDown && Input.GetMouseButtonUp(0))
        {
            fingerDown = false;
        }

    }
    private IEnumerator DelayEnd()
    {
        yield return new WaitForSeconds(3f);
        if (finalScore > 5.5) { GameManager.instance.UIManager.WinGame(); }
        else { GameManager.instance.UIManager.LoseGame(); }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Evolution"))
        {
            GameManager.instance.soundManager.PlaySoundFX(TypeSound.GoodImpact);
            isGood = true;
          GoodFxPrefab.SetActive(true);
            GoodWalk();
        }
        if (other.CompareTag("Hurdle"))
        {
            GameManager.instance.soundManager.PlaySoundFX(TypeSound.BadImpact);
            isBad = true;
          BadFxPrefab.SetActive(true);
            BadWalk();

        }
        if (other.CompareTag("GateGood"))
        {
            GameManager.instance.soundManager.PlaySoundFX(TypeSound.GoodImpact);
            isGood2 = true;
           GoodFxPrefab3.SetActive(true);
            GoodWalk();
        }
        if (other.CompareTag("GateBad"))
        {
            GameManager.instance.soundManager.PlaySoundFX(TypeSound.BadImpact);
            isBad2 = true;
            BadFxPrefab3.SetActive(true);
            BadWalk();

        }
        if (other.CompareTag("Good"))
        {
          GoodFxPrefab2.SetActive(true);
            GameManager.instance.soundManager.PlaySoundFX(TypeSound.GoodImpact);
            goodDress.SetActive(true);
            curDress.SetActive(false);
            finalScore += 3;
            other.gameObject.SetActive(false);
            GoodWalk();
        }
        if (other.CompareTag("Bad"))
        {
            BadFxPrefab2.SetActive(true);   
            GameManager.instance.soundManager.PlaySoundFX(TypeSound.BadImpact);
            badDress.SetActive(true);
            curDress.SetActive(false);
            finalScore -= 4;
            other.gameObject.SetActive(false);
            BadWalk();

        }
        if (other.CompareTag("Finish"))
        {
            GoodWalk();
            score.DeActiveScore();
            timer.DeActiveTimer();

            isWin = true;
            GameManager.instance.soundManager.bg.Stop();
            if (finalScore > 5.5) { GameManager.instance.soundManager.PlaySoundFX(TypeSound.Win); }
            else { GameManager.instance.soundManager.PlaySoundFX(TypeSound.BadImpact); }
            cameraFollow.end = true;
            StartCoroutine(WalkEnd());
            StartCoroutine(cameraFollow.CamZoom());

            StartCoroutine(DelayEnd());


        }
        
    }
    public void GoodWalk()
    {
        dadAnimator.SetBool("SadWalk", false);
        dadAnimator.SetBool("Walk", true);
        momAnimator.SetBool("sadWalk", false);
        momAnimator.SetBool("Walk", true);
    }
    public void BadWalk()

    {
        dadAnimator.SetBool("SadWalk", true);
        dadAnimator.SetBool("Walk", false);
        momAnimator.SetBool("sadWalk", true);
        momAnimator.SetBool("Walk", false);
    }
    public IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(3.45f);
        if (tutorial)
            GameManager.instance.UIManager.Tutorial(true);
    }
    public IEnumerator WalkEnd()
    {

        gameObject.transform.DOMove(new Vector3(0f, 0.9f, 185.104f), 1f);
        yield return new WaitForSeconds(1f);
        gameObject.transform.DOMove(new Vector3(0f, 0.9f, 189.743f), 1.6f);
        yield return new WaitForSeconds(1.6f);
        mom.transform.DORotate(new Vector3(0, -90, 0), 0.7f);
        dad.transform.DORotate(new Vector3(0, 90, 0), 0.7f);
        momAnimator.SetBool("idle", true);
        dadAnimator.SetBool("Idle", true);
        momAnimator.SetBool("Walk", false);
        dadAnimator.SetBool("Walk", false);
    }
 
}
