using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {

    [SerializeField]
    private Text keyboardLayoutText;

    private ArrayList inputListP1, inputListP2;

    private InputsNormalized inputsNormalized;

    private bool atLeast1BonbonCorrectP1, startInterruptedP1; // pour commencer a utiliser le multiplicateur
    private bool atLeast1BonbonCorrectP2, startInterruptedP2;

    private bool needSmashP1, needsmashP2 = false;
    [SerializeField] int nbSmash = 20;
    private int currentSmashP1, currentSmashP2 = 0;

    public static PlayerInput Get;
    private void Awake()
    {
        if (Get == null)
        {
            Get = this;
        }
        else Destroy(this.gameObject);
    }

    void Start() {

        inputsNormalized = new InputsNormalized();

        // set text and button for detected keyboard layout and option to change
        if(inputsNormalized.getIsQwerty())
            keyboardLayoutText.text = "The keyboard layout is currently set to \'QWERTY\'";
        else 
            keyboardLayoutText.text = "La disposition du clavier est actuellement définie sur \'AZERTY\'";

        atLeast1BonbonCorrectP1 = startInterruptedP1 = false;
        atLeast1BonbonCorrectP2 = startInterruptedP2 = false;
    }

    void Update() {
        if(Timer.Get.getGameStarted() && Input.anyKeyDown) {
            StartCoroutine("InputCheckThreadPlayer1");
            StartCoroutine("InputCheckThreadPlayer2");
        }
    }

    private IEnumerator InputCheckThreadPlayer1() {

        if(inputListP1.Count != 0 && Input.GetKeyDown(inputsNormalized.realInput(true, (int)inputListP1[0]))) {
            if(inputListP1.Count == 1)
                p1Success();
            inputListP1.RemoveAt(0);
            p1InputPressed();
        } else if((inputsNormalized.getIsQwerty() && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))) ||
                (!inputsNormalized.getIsQwerty() && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))) {
                p1Fail();
        }

        if (needSmashP1 && Input.GetKeyDown(KeyCode.V))
        {
            SoundManager.Get.Play(Sound.soundNames.PimentSpam);
            ++currentSmashP1;
            if (currentSmashP1 >= nbSmash)
            {
                SoundManager.Get.Play(Sound.soundNames.PimentDebarasse);
                CanvasManager canvas = CanvasManager.Get;
                currentSmashP1 = 0;
                SetBlockMultiplier(true, false);
                ScoreManager.Get.SetBlockMultiplier(true, false);
                canvas.piment1.SetActive(false);
            }
        }

        yield return null;
    }

    private IEnumerator InputCheckThreadPlayer2() {
        
        if(inputListP2.Count != 0 && Input.GetKeyDown(inputsNormalized.realInput(false, (int)inputListP2[0]))) {
            if(inputListP2.Count == 1)
                p2Success();
            inputListP2.RemoveAt(0);
            p2InputPressed();
        } else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
            p2Fail();
        }

        if (needsmashP2 && Input.GetKeyDown(KeyCode.P))
        {
            SoundManager.Get.Play(Sound.soundNames.PimentSpam);
            ++currentSmashP2;
            if (currentSmashP2 >= nbSmash)
            {
                SoundManager.Get.Play(Sound.soundNames.PimentDebarasse);
                CanvasManager canvas = CanvasManager.Get;
                currentSmashP2 = 0;
                SetBlockMultiplier(false, false);
                ScoreManager.Get.SetBlockMultiplier(false, false);
                canvas.piment2.SetActive(false);
            }
        }

        yield return null;
    }

    // correct input
    private void p1InputPressed() {
        SoundManager.Get.Play(Sound.soundNames.CorrectSound);

        FeedbackManager.Get.correctInputFeedback(BonbonManager.Get.GetCurrentInputObject(true));
        BonbonManager.Get.DestroyInput(1);

        if(atLeast1BonbonCorrectP1) 
            ScoreManager.Get.IncrementMultiplier(true);
    }
    private void p2InputPressed() {
        SoundManager.Get.Play(Sound.soundNames.CorrectSound);

        FeedbackManager.Get.correctInputFeedback(BonbonManager.Get.GetCurrentInputObject(false));
        BonbonManager.Get.DestroyInput(2);

        if(atLeast1BonbonCorrectP2) 
            ScoreManager.Get.IncrementMultiplier(false);
    }

    // mauvais input
    private void p1Fail()
    {     // animation fail  pour p1
        SoundManager.Get.Play(Sound.soundNames.IncorrectSound);

        ScoreManager.Get.ResetMultiplier(true);

        FeedbackManager.Get.IncorrectInputFeedback(BonbonManager.Get.GetCurrentInputObject(true));

        atLeast1BonbonCorrectP1 = false;
        startInterruptedP1 = true;
    }
    private void p2Fail()
    {     // animation fail  pour p2
        SoundManager.Get.Play(Sound.soundNames.IncorrectSound);

        ScoreManager.Get.ResetMultiplier(false);

        FeedbackManager.Get.IncorrectInputFeedback(BonbonManager.Get.GetCurrentInputObject(false));

        atLeast1BonbonCorrectP2 = false;
        startInterruptedP2 = true;
    }

    // fin de tous input d'un bonbon
    private void p1Success() {        
        if(!startInterruptedP1) 
            atLeast1BonbonCorrectP1 = true;

        startInterruptedP1 = false;
    }
    private void p2Success() {        
        if(!startInterruptedP2) 
            atLeast1BonbonCorrectP2 = true;

        startInterruptedP2 = false;
    }



    public void ChangeKeyboardLayoutButton() {
        inputsNormalized.setIsQwerty(!inputsNormalized.getIsQwerty());
        if(inputsNormalized.getIsQwerty())
            keyboardLayoutText.text = "The keyboard layout is currently set to \'QWERTY\'";
        else 
            keyboardLayoutText.text = "La disposition du clavier est actuellement définie sur \'AZERTY\'";

        SoundManager.Get.Play(Sound.soundNames.MenuClick);
    }

    public void SetListInput(bool isP1, ArrayList list) {
        if (isP1)
            inputListP1 = list;
        else
            inputListP2 = list;
    }

    public void SetBlockMultiplier(bool isP1, bool state)
    {
        if (isP1)
            needSmashP1 = state;
        else needsmashP2 = state;
    }
}
