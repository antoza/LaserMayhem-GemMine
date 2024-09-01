using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
#nullable enable

public class UIManagerGame : UIManager
{
#if !DEDICATED_SERVER
    public static new UIManagerGame Instance => (UIManagerGame)UIManager.Instance;

    private int[] playerIndexes;

    // TODO : Rendre UIManager abstraite, avec des sous-classes comme GameUIManager
    [SerializeField]
    private GameObject victoryPopUp;
    [SerializeField]
    private GameObject defeatPopUp;
    [SerializeField]
    private GameObject drawPopUp;

    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI gameOverScore;
    [SerializeField]
    private TextMeshProUGUI gameOverBestScore;
    [SerializeField]
    private TextMeshProUGUI turnCount;

    [SerializeField]
    private GameObject healthLoss;
    private float healthLossDisplayTime = 3f;
    [SerializeField]
    private GameObject coinToss;
    private float coinTossDisplayTime = 3f;

    [SerializeField]
    private Animator conveyorAnimator;
    [SerializeField]
    private SpriteRenderer _dividerCooldownSpriteRenderer;
    [SerializeField]
    private Sprite[] _dividerCooldownSprites;

    [SerializeField]
    private Image BackgroundImage;
    [SerializeField]
    public SkinData SkinData;

    protected override void Start()
    {
        base.Start();
        AssociatePlayersInAscendingOrder(GameInitialParameters.localPlayerID, PlayersManager.Instance.NumberOfPlayers);
        SetBackground();
    }

    private void AssociatePlayersInAscendingOrder(int localPlayerID, int numberOfPlayers)
    {
        playerIndexes = new int[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerIndexes[i] = (i + localPlayerID) % numberOfPlayers;
        }
    }


    // Gameover popups

    public async void TriggerDraw()
    {
        await WaitForReadiness();
        drawPopUp.SetActive(true);
    }

    public async void TriggerVictory()
    {
        await WaitForReadiness();
        victoryPopUp.SetActive(true);
    }

    public async void TriggerDefeat()
    {
        await WaitForReadiness();
        defeatPopUp.SetActive(true);
    }

    public async void TriggerGameOverShredder(int score, int bestScore)
    {
        await WaitForReadiness();
        defeatPopUp.SetActive(true);
        gameOverScore.text = "Score: " + score;
        gameOverBestScore.text = "Best score: " + bestScore;
    }

    public async void HideGameOverPopUps()
    {
        await WaitForReadiness();
        if (drawPopUp != null) drawPopUp.SetActive(false);
        if (victoryPopUp != null) victoryPopUp.SetActive(false);
        if (defeatPopUp != null) defeatPopUp.SetActive(false);
    }


    // Values

    public async void UpdateChallengeTitle(int value)
    {
        await WaitForReadiness();
        title.text = $"Challenge {value}";
    }

    public async void UpdateScoreInt(int value)
    {
        await WaitForReadiness();
        score.text = $"{value}";
    }

    public async void UpdateScoreFraction(int numerator, int denominator)
    {
        await WaitForReadiness();
        score.text = $"{numerator} / {denominator}";
        if (numerator == denominator) { score.color = Color.green; }
        else { score.color = Color.yellow; }
    }

    public async void UpdateTurnCount(int value)
    {
        await WaitForReadiness();
        turnCount.text = $"{value}";
    }

    // Volatile indicators

    public async void DisplayHealthLoss(int amount, Vector2 position)
    {
        await WaitForReadiness();
        GameObject healthLossGameObject = Instantiate(healthLoss, canvas.transform);
        healthLossGameObject.transform.position = position;
        healthLossGameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"-{amount}";
        StartCoroutine(DestroyCoroutine(healthLossGameObject, healthLossDisplayTime));
    }

    public async void DisplayCoinToss(int amount, Vector2 position)
    {
        await WaitForReadiness();
        Vector2[] coinPositions = new Vector2[amount];

        switch (amount)
        {
            default:
            case 1:
                coinPositions[0] = position;
                break;
            case 3:
                coinPositions[0] = position + new Vector2(0, -.24f);
                coinPositions[1] = position + new Vector2(-.32f, .24f);
                coinPositions[2] = position + new Vector2(.32f, .24f);
                break;
            case 5:
                coinPositions[0] = position;
                coinPositions[1] = position + new Vector2(-.32f, -.32f);
                coinPositions[2] = position + new Vector2(-.32f, .32f);
                coinPositions[3] = position + new Vector2(.32f, -.32f);
                coinPositions[4] = position + new Vector2(.32f, .32f);
                break;
        }

        foreach (Vector2 coinPosition in coinPositions)
        {
            GameObject coinTossGameObject = Instantiate(coinToss, canvas.transform);
            coinTossGameObject.transform.position = coinPosition;
            StartCoroutine(DestroyCoroutine(coinTossGameObject, coinTossDisplayTime));
        }
    }

    public async void OperateConveyor()
    {
        await WaitForReadiness();
        conveyorAnimator.SetTrigger("Operate");
    }

    public async void UpdateDividerCooldownIndicator(int value)
    {
        await WaitForReadiness();
        _dividerCooldownSpriteRenderer.sprite = _dividerCooldownSprites[value];
    }



    public void SetBackground()
    {/* TODO : décommenter (j'ai commenté car ça me créait des erreurs)
        if(PlayerPrefs.HasKey("Background Skin") && SkinData)
        {
            string backgroundSpriteName = PlayerPrefs.GetString("Background Skin");
            if(SkinData.BackgroundSkin.ContainsKey(backgroundSpriteName))
            {
                BackgroundImage.sprite = SkinData.BackgroundSkin[backgroundSpriteName];
            }
        }
        else
        {
            string backgroundSpriteName = "Default";
            if (SkinData.BackgroundSkin.ContainsKey(backgroundSpriteName))
            {
                BackgroundImage.sprite = SkinData.BackgroundSkin[backgroundSpriteName];
            }
        }*/
    }
#endif
}
