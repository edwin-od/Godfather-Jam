using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {
    
    [SerializeField, Range(1.0f, 300.0f)]
    private float timerValue = 60.0f;

    [SerializeField, Range(1.0f, 10.0f)]
    private float countdownValue = 3.0f;

    public bool restart = false;

    public Text timerText;
    [SerializeField] GameObject timerBackground, scoreP1Back, scoreP2Back, scoreP1Text, scoreP2Text, comboTextP1, comboTextP2;
    [SerializeField]
    private GameObject startUI, p1NotReadyText, p1ReadyText, p2NotReadyText, p2ReadyText, restartButton, leaveButton;

    private float curTime = 0;

    private bool gameStarted, gameCountdown, p1Ready, p2Ready;

    [SerializeField] GameObject keyBoardLayout;

    [SerializeField] GameObject winnerSpriteP1, winnerScoreTextP1, winnerSpriteP2, winnerScoreTextP2, loserSpriteP1, loserSpriteP2;

    public static Timer Get;
    private void Awake()
    {
        if (Get == null)
        {
            Get = this;
        }
        else Destroy(this.gameObject);
    }

    void Start() {
        winnerSpriteP1.SetActive(false);
        winnerScoreTextP1.SetActive(false);
        winnerSpriteP2.SetActive(false);
        winnerScoreTextP2.SetActive(false);
        loserSpriteP1.SetActive(false);
        loserSpriteP2.SetActive(false);

        scoreP1Back.SetActive(false);
        scoreP2Back.SetActive(false);
        scoreP1Text.SetActive(false);
        scoreP2Text.SetActive(false);
        comboTextP1.SetActive(false);
        comboTextP2.SetActive(false);

        timerBackground.SetActive(false);
        timerText.gameObject.SetActive(false);
        matchStatusChange(false, true);
        gameStarted = gameCountdown = p1Ready = p2Ready = false;
        keyBoardLayout.SetActive(true);
    }

    void Update() {
        if(gameStarted) {
            if(curTime > 0f)
                curTime -= Time.deltaTime;
            else {
                curTime = 0f;
                matchStatusChange(true, false);
                gameStarted = p1Ready = p2Ready = false;
                CanvasManager.Get.piment1.SetActive(false);
                CanvasManager.Get.piment2.SetActive(false);
                BonbonManager.Get.DestroyAllInput();
                timerBackground.SetActive(false);
                timerText.gameObject.SetActive(false);

                comboTextP1.GetComponent<Text>().text = "x1";
                comboTextP2.GetComponent<Text>().text = "x1";

                comboTextP1.SetActive(false);
                comboTextP2.SetActive(false);

                if (ScoreManager.Get.GetScore(true) > ScoreManager.Get.GetScore(false))
                { // P1 wins
                    winnerSpriteP1.SetActive(true);
                    winnerScoreTextP1.SetActive(true);
                    winnerScoreTextP1.GetComponent<Text>().text = ScoreManager.Get.GetScore(true) + "";
                    winnerSpriteP2.SetActive(false);
                    winnerScoreTextP2.SetActive(false);
                    loserSpriteP1.SetActive(false);
                    loserSpriteP2.SetActive(true);
                }
                else if (ScoreManager.Get.GetScore(true) < ScoreManager.Get.GetScore(false))
                { // P2 wins
                    winnerSpriteP1.SetActive(false);
                    winnerScoreTextP1.SetActive(false);
                    winnerSpriteP2.SetActive(true);
                    winnerScoreTextP2.SetActive(true);
                    winnerScoreTextP2.GetComponent<Text>().text = ScoreManager.Get.GetScore(false) + "";
                    loserSpriteP1.SetActive(true);
                    loserSpriteP2.SetActive(false);
                }
                else
                { // P1 and P2 win
                    winnerSpriteP1.SetActive(true);
                    winnerScoreTextP1.SetActive(true);
                    winnerScoreTextP1.GetComponent<Text>().text = ScoreManager.Get.GetScore(true) + "";
                    winnerSpriteP2.SetActive(true);
                    winnerScoreTextP2.SetActive(true);
                    winnerScoreTextP2.GetComponent<Text>().text = ScoreManager.Get.GetScore(false) + "";
                    loserSpriteP1.SetActive(false);
                    loserSpriteP2.SetActive(false);
                }
            }
        } else if(gameCountdown) {
            if(curTime > 0f)
                curTime -= Time.deltaTime;
            else {
                curTime = timerValue;
                matchStatusChange(false, false);
                gameStarted = true;
                gameCountdown = false;
                BonbonManager.Get.Spawns();
                FeedbackManager.Get.vsAnimationOut();
            }
        } else if(!p1Ready || !p2Ready) {
            if(Input.anyKeyDown) {
                StartCoroutine("InputCheckThreadPlayer1");
                StartCoroutine("InputCheckThreadPlayer2");
            }
        }
        
        if(curTime != 0)
            timerText.text = (int)(curTime + 1) + "";
        else
            timerText.text = curTime + "";
    }

    private IEnumerator InputCheckThreadPlayer1() {
        if(!p1Ready && Input.GetKeyDown(KeyCode.LeftControl)) {
            p1Ready = true;
            p1NotReadyText.SetActive(false);
            p1ReadyText.SetActive(true);
            if(p2Ready) 
                startGame();
        }

         yield return null;
    }

    private IEnumerator InputCheckThreadPlayer2() {
        if(!p2Ready && Input.GetKeyDown(KeyCode.RightControl)) {
            p2Ready = true;
            p2NotReadyText.SetActive(false);
            p2ReadyText.SetActive(true);
            if(p1Ready) 
                startGame();
        }

         yield return null;
    }

    public bool getGameStarted() {
        return gameStarted;
    }

    private void matchStatusChange(bool status, bool init) {    // true -> match just ended
        if(init) {
            resetPlayersReadyStatus();

            restartButton.SetActive(false);
            leaveButton.SetActive(true);
        } else {
            restartButton.SetActive(status);
            leaveButton.SetActive(status);
        }
        if(status) 
            resetPlayersReadyStatus();
    }

    private void resetPlayersReadyStatus() {
        p1NotReadyText.SetActive(true);
        p1ReadyText.SetActive(false);
        p2NotReadyText.SetActive(true);
        p2ReadyText.SetActive(false);
    }

    private void startGame() {
        timerBackground.SetActive(true);
        timerText.gameObject.SetActive(true);
        scoreP1Back.SetActive(true);
        scoreP2Back.SetActive(true);
        scoreP1Text.SetActive(true);
        scoreP2Text.SetActive(true);
        comboTextP1.SetActive(true);
        comboTextP2.SetActive(true);

        FeedbackManager.Get.vsAnimationIn();
        gameCountdown = true;
        startUI.SetActive(false);
        curTime = countdownValue;
        SoundManager.Get.Play(Sound.soundNames.MatchConfirm);
        keyBoardLayout.SetActive(false);
    }

    public void RestartGame() 
    {
        winnerSpriteP1.SetActive(false);
        winnerScoreTextP1.SetActive(false);
        winnerSpriteP2.SetActive(false);
        winnerScoreTextP2.SetActive(false);
        loserSpriteP1.SetActive(false);
        loserSpriteP2.SetActive(false);

        scoreP1Back.SetActive(false);
        scoreP2Back.SetActive(false);
        scoreP1Text.SetActive(false);
        scoreP2Text.SetActive(false);

        ScoreManager.Get.ResetScore();
        startUI.SetActive(true);
        matchStatusChange(false, true);
        SoundManager.Get.Play(Sound.soundNames.MenuClick);
        keyBoardLayout.SetActive(true);
    }

    public void LeaveGame() {
         SceneManager.LoadScene("Menu");
        SoundManager.Get.Play(Sound.soundNames.MenuClick);
    }
}
