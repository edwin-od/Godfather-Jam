using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class BonbonManager : MonoBehaviour
{
    [SerializeField] Bonbon bonbonPrefab;
    [SerializeField] InputObject inputPrefab;

    [SerializeField] Transform posPlayer1;
    [SerializeField] Transform posPlayer2;

    [SerializeField] Transform posPlayerInput1;
    [SerializeField] Transform posPlayerInput2;
    [SerializeField] float height = 10;

    [SerializeField] float fallDuration = 0.5f;

    private Bonbon currentBBJ1;
    private bool isEmballageP1 = true;
    private Bonbon currentBBJ2;
    private bool isEmballageP2 = true;

    private List<InputObject> inputPlayer1 = new List<InputObject>();
    private List<InputObject> inputPlayer2 = new List<InputObject>();

    [SerializeField] List<Sprite> inputSprites = new List<Sprite>();
    [SerializeField] BonbonScriptableObject pimentSprites;
    [SerializeField] List<BonbonScriptableObject> bonbonSprites = new List<BonbonScriptableObject>();
    [SerializeField] List<BonbonScriptableObject> bonbonSpritesGold = new List<BonbonScriptableObject>();
    [SerializeField] List<BonbonScriptableObject> emballageSprites = new List<BonbonScriptableObject>();

    [SerializeField] int nbEasyInput = 3;
    [SerializeField] int nbMediumInput = 4;
    [SerializeField] int nbHardInput = 6;

    public static BonbonManager Get;
    private void Awake()
    {
        if (Get == null)
        {
            Get = this;
        }
        else Destroy(gameObject);
    }

    public void Spawns()
    {
        currentBBJ1 = Spawnbonbon(posPlayer1, 1, posPlayerInput1);
        currentBBJ2 = Spawnbonbon(posPlayer2, 2, posPlayerInput2);
    }

    Bonbon Spawnbonbon(Transform pos, int playerId, Transform posInput)
    {
        Bonbon bb = Instantiate(bonbonPrefab, pos);
        ArrayList inputs = new ArrayList();
        BonbonType bonbonType;
        int nbInput = 2;
        int score = 50;
        int bonbonSpriteIndex = Random.Range(0, bonbonSprites.Count);
        int emballageSpriteIndex = Random.Range(0, emballageSprites.Count);

        //Creation du bonbon
        if (playerId == 1)
        {
            if (isEmballageP1)
            {
                bonbonType = (BonbonType)Random.Range(0, 3);
                switch (bonbonType)
                {
                    case BonbonType.easy:
                        nbInput = nbEasyInput;
                        score = ScoreManager.Get.lowScore;
                        break;
                    case BonbonType.medium:
                        nbInput = nbMediumInput;
                        score = ScoreManager.Get.mediumScore;
                        break;
                    case BonbonType.hard:
                        nbInput = nbHardInput;
                        score = ScoreManager.Get.highScore;
                        break;
                }
                bb.Init(emballageSprites[emballageSpriteIndex], score, bonbonType);
            }
            else
            {
                bonbonType = currentBBJ1.bonbonType;
                switch (bonbonType)
                {
                    case BonbonType.easy:
                        nbInput = nbEasyInput;
                        score = ScoreManager.Get.lowScore;
                        break;
                    case BonbonType.medium:
                        nbInput = nbMediumInput;
                        score = ScoreManager.Get.mediumScore;
                        break;
                    case BonbonType.hard:
                        nbInput = nbHardInput;
                        score = ScoreManager.Get.highScore;
                        break;
                }
                float rand = Random.Range(0, 100);
                if (rand > 90)
                {
                    bb.Init(bonbonSpritesGold[bonbonSpriteIndex], score * ScoreManager.Get.goldMultiplier, bonbonType);
                    SoundManager.Get.Play(Sound.soundNames.GoldBonbon);
                }
                else if (rand > 80)
                {
                    bb.Init(pimentSprites, score, bonbonType, true);
                }
                else
                {
                    bb.Init(bonbonSprites[bonbonSpriteIndex], score, bonbonType);
                }
            }
        }
        else
        {
            if (isEmballageP2)
            {
                bonbonType = (BonbonType)Random.Range(0, 3);
                switch (bonbonType)
                {
                    case BonbonType.easy:
                        nbInput = nbEasyInput;
                        score = ScoreManager.Get.lowScore;
                        break;
                    case BonbonType.medium:
                        nbInput = nbMediumInput;
                        score = ScoreManager.Get.mediumScore;
                        break;
                    case BonbonType.hard:
                        nbInput = nbHardInput;
                        score = ScoreManager.Get.highScore;
                        break;
                }
                bb.Init(emballageSprites[emballageSpriteIndex], score, bonbonType);
            }
            else
            {
                bonbonType = currentBBJ2.bonbonType;
                switch (bonbonType)
                {
                    case BonbonType.easy:
                        nbInput = nbEasyInput;
                        score = ScoreManager.Get.lowScore;
                        break;
                    case BonbonType.medium:
                        nbInput = nbMediumInput;
                        score = ScoreManager.Get.mediumScore;
                        break;
                    case BonbonType.hard:
                        nbInput = nbHardInput;
                        score = ScoreManager.Get.highScore;
                        break;
                }
                float rand = Random.Range(0, 100);
                if (rand > 90)
                {
                    bb.Init(bonbonSpritesGold[bonbonSpriteIndex], score * ScoreManager.Get.goldMultiplier, bonbonType);
                    SoundManager.Get.Play(Sound.soundNames.GoldBonbon);
                }
                else if (rand > 80)
                {
                    bb.Init(pimentSprites, score, bonbonType, true);
                    Debug.LogWarning("Piment");
                }
                else
                {
                    bb.Init(bonbonSprites[bonbonSpriteIndex], score, bonbonType);
                }
            }
        }

        // Creation des input associes
        for (int i = 0; i < nbInput; ++i)
        {
            int rand = Random.Range(0, 4);
            inputs.Add(rand);

            InputObject obj = Instantiate(inputPrefab, posInput);
            obj.transform.position += i * height * Vector3.up;
            obj.Init(rand, inputSprites[rand]);

            if (playerId == 1)
                inputPlayer1.Add(obj);
            else if (playerId == 2)
                inputPlayer2.Add(obj);
            else Destroy(obj.gameObject);
        }
        
        PlayerInput.Get.SetListInput(playerId == 1, inputs);
        return bb;
    }

    public void DestroyBonbon(Bonbon bb, bool forReset = false)
    {
        if (bb == currentBBJ1)
        {
            // TO DO : add animation

            //Sounds
            if (isEmballageP1)
                SoundManager.Get.Play(Sound.soundNames.Emballage);
            else SoundManager.Get.Play(Sound.soundNames.EatBonbon);

            Destroy(bb.gameObject);
            if (!forReset && !isEmballageP1)
            {
                ScoreManager.Get.AddScrore(currentBBJ1.score, 1);
                CheckPiment(bb, 1);
            }
            if (forReset) ResetEmballageStatus();
            else
            {
                SwapEmballageStatus(1);
                currentBBJ1 = Spawnbonbon(posPlayer1, 1, posPlayerInput1);
            }
        }
        else if (bb == currentBBJ2)
        {
            // TO DO : add animation

            //Sounds
            if (isEmballageP2)
                SoundManager.Get.Play(Sound.soundNames.Emballage);
            else SoundManager.Get.Play(Sound.soundNames.EatBonbon);

            Destroy(bb.gameObject);
            if (!forReset && !isEmballageP2)
            {
                ScoreManager.Get.AddScrore(currentBBJ2.score, 2);
                CheckPiment(bb, 2);
            }
            if (forReset) ResetEmballageStatus();
            else
            {
                SwapEmballageStatus(2);
                currentBBJ2 = Spawnbonbon(posPlayer2, 2, posPlayerInput2);
            } 
        }
        else
        {
            Destroy(bb.gameObject);
        }
    }

    private void CheckPiment(Bonbon bb, int playerId)
    {
        if (bb.isPiment)
        {
            SoundManager.Get.Play(Sound.soundNames.PimentMange);
            CanvasManager canvas = CanvasManager.Get;
            ScoreManager.Get.SetBlockMultiplier(playerId == 1, true);
            PlayerInput.Get.SetBlockMultiplier(playerId == 1, true);
            if (playerId == 1)
            {
                canvas.piment1.SetActive(true);
            }
            else
            {
                canvas.piment2.SetActive(true);
            }
        }
    }

    public void DestroyInput(int playerId)
    {
        if (playerId == 1)
        {
            foreach(InputObject obj in inputPlayer1)
            {
                obj.StartFall(height, fallDuration);
            }
            Destroy(inputPlayer1[0].gameObject);
            inputPlayer1.RemoveAt(0);

            if (inputPlayer1.Count == 0)
                DestroyBonbon(currentBBJ1);
            else CheckSwapSprite(currentBBJ1, inputPlayer1.Count);
        }
        else if (playerId == 2)
        {
            foreach (InputObject obj in inputPlayer2)
            {
                obj.StartFall(height, fallDuration);
            }
            Destroy(inputPlayer2[0].gameObject);
            inputPlayer2.RemoveAt(0);

            if (inputPlayer2.Count == 0)
                DestroyBonbon(currentBBJ2);
            else CheckSwapSprite(currentBBJ2, inputPlayer2.Count);
        }
        else Debug.LogError("Wrong playerId");
    }

    public void DestroyAllInput()
    {
        foreach (InputObject obj in inputPlayer1)
        {
            Destroy(obj.gameObject);
        }
        inputPlayer1.Clear();
        DestroyBonbon(currentBBJ1, true);

        foreach (InputObject obj in inputPlayer2)
        {
            Destroy(obj.gameObject);
        }
        inputPlayer2.Clear();
        DestroyBonbon(currentBBJ2, true);
    }

    private void SwapEmballageStatus(int playerId)
    {
        if (playerId == 1)
        {
            isEmballageP1 = !isEmballageP1;
        }
        else if (playerId == 2)
        {
            isEmballageP2 = !isEmballageP2;
        }
        else Debug.LogError("Wrong playerId");
    }

    private void ResetEmballageStatus()
    {
        isEmballageP1 = true;
        isEmballageP2 = true;
    }

    void CheckSwapSprite(Bonbon bonbon, int nbInputRestants)
    {
        switch (bonbon.bonbonType)
        {
            case BonbonType.easy:
                bonbon.NextSprite();
                break;
            case BonbonType.medium:
                if (nbInputRestants == 3 || nbInputRestants == 1)
                    bonbon.NextSprite();
                break;
            case BonbonType.hard:
                if (nbInputRestants == 4 || nbInputRestants == 2)
                    bonbon.NextSprite();
                break;
        }
    }

    public InputObject GetCurrentInputObject(bool isP1) {
        if(isP1)
            return inputPlayer1[0];
        else
            return inputPlayer2[0];
    }
}

public enum BonbonType
{
    easy = 0,
    medium = 1,
    hard = 2
}
