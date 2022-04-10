using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreP1;
    [SerializeField] Text scoreP2;

    [SerializeField] Text multiplierTextP1;
    [SerializeField] Text multiplierTextP2;

    int currentScoreP1;
    int currentScoreP2;

    int multiplierP1;
    int multiplierP2;

    bool blockMultiplierP1 = false;
    bool blockMultiplierP2 = false;

    public int lowScore = 50;
    public int mediumScore = 75;
    public int highScore = 100;
    public int goldMultiplier = 3;

    [SerializeField] int milestoneForSound = 10;

    [Header("Bounce Score")]
    [SerializeField, Min(1)] float maxScale = 1.3f;
    [SerializeField] float bounceDuration = 0.3f;

    [Header("Bounce Score Multiplier")]
    [SerializeField, Min(1)] float maxScaleMultiplier = 1.1f;
    [SerializeField] float bounceDurationMultiplier = 0.1f;

    void Start() {
        ResetScore();
    }

    public static ScoreManager Get;
    private void Awake()
    {
        if (Get == null)
        {
            Get = this;
        }
        else Destroy(this.gameObject);
    }

    public void AddScrore(int scoreToAdd, int playerId)
    {
        if (playerId == 1)
        {
            currentScoreP1 += multiplierP1 * scoreToAdd;
            scoreP1.text = currentScoreP1.ToString();
            Bounce(true);
        }
        else
        {
            currentScoreP2 += multiplierP2 * scoreToAdd;
            scoreP2.text = currentScoreP2.ToString();
            Bounce(false);
        }
    }

    public void ResetScore()
    {
        ResetMultiplier();

        currentScoreP1 = 0;
        scoreP1.text = currentScoreP1.ToString();

        currentScoreP2 = 0;
        scoreP2.text = currentScoreP2.ToString();
    }

    private void ResetMultiplier() {
        multiplierP1 = 1;
        multiplierP2 = 1;
    }

    public void ResetMultiplier(bool isP1) {    // Overload used to be called from PlayerInput Script when a wrong input happens
        if(isP1) {
            multiplierP1 = 1;
            multiplierTextP1.text = "x" + multiplierP1;
        } else {
            multiplierP2 = 1;
            multiplierTextP2.text = "x" + multiplierP2;
        }
    }
    
    public void IncrementMultiplier(bool isP1) {
        if(isP1)
        {
            if (!blockMultiplierP1)
            {
                multiplierP1++;
                multiplierTextP1.text = "x" + multiplierP1;

                StartCoroutine(BounceRoutine(multiplierTextP1.gameObject, maxScaleMultiplier, bounceDurationMultiplier));

                if (multiplierP1 % milestoneForSound == 0)
                {
                    SoundManager.Get.Play(Sound.soundNames.MultiplierMileStone);
                }
            }
        } 
        else
        {
            if (!blockMultiplierP2)
            {
                multiplierP2++;
                multiplierTextP2.text = "x" + multiplierP2;

                StartCoroutine(BounceRoutine(multiplierTextP2.gameObject, maxScaleMultiplier, bounceDurationMultiplier));

                if (multiplierP2 % milestoneForSound == 0)
                {
                    SoundManager.Get.Play(Sound.soundNames.MultiplierMileStone);
                }
            }
        }
    }

    public void SetBlockMultiplier(bool isP1, bool state)
    {
        if (isP1)
        {
            blockMultiplierP1 = state;
        }
        else
        {
            blockMultiplierP2 = state;
        }
    }

    private void Bounce(bool isP1)
    {
        if (isP1)
            StartCoroutine(BounceRoutine(scoreP1.gameObject, maxScale, bounceDuration));
        else StartCoroutine(BounceRoutine(scoreP2.gameObject, maxScale, bounceDuration));
    }

    IEnumerator BounceRoutine(GameObject obj, float scale, float duration)
    {
        float timer = 0;
        while (timer < bounceDuration)
        {
            timer += Time.deltaTime;
            obj.transform.localScale = Mathf.Lerp(1, maxScale, timer / bounceDuration) * Vector3.one;
            yield return null;
        }
        timer = 0f;
        while (timer < bounceDuration)
        {
            timer += Time.deltaTime;
            obj.transform.localScale = Mathf.Lerp(maxScale, 1, timer / bounceDuration) * Vector3.one;
            yield return null;
        }
        obj.transform.localScale = Vector3.one;
    }

    public int GetScore(bool isP1)
    {
        return isP1 ? currentScoreP1 : currentScoreP2;
    }
}
