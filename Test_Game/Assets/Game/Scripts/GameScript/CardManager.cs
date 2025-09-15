using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Naninovel;
using System;
using UnityEngine.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform lastPoint;
    [SerializeField] private Sprite[] cardFaceSprites;
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 6;
    [SerializeField] private float spacingMultiplier = 1.2f;

    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float gameDuration = 60f;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private CardSoundManager soundManager;
    [SerializeField] private Image slider;
    [SerializeField] private Color fullColor = Color.green;
    [SerializeField] private Color emptyColor = Color.red;

    public event Action OnGameCompleted;

    private List<Card> spawnedCards = new List<Card>();
    private Card firstCard, secondCard;
    private bool isChecking = false;
    private bool isGameOver = false;
    private float timeRemaining;

    public void StartGame()
    {
        GenerateBoard();
        timeRemaining = gameDuration;
        isGameOver = false;
        StartTimer().Forget();
    }

    private void GenerateBoard()
    {
        List<Sprite> selectedFaces = new List<Sprite>();
        int pairCount = (rows * columns) / 2;

        selectedFaces.AddRange(cardFaceSprites.Take(pairCount));
        selectedFaces.AddRange(cardFaceSprites.Take(pairCount));
        selectedFaces = selectedFaces.OrderBy(x => UnityEngine.Random.value).ToList();

        Vector2 cardSize = GetCardWorldSize();
        float offsetX = cardSize.x * spacingMultiplier;
        float offsetY = cardSize.y * spacingMultiplier;

        float startX = -(columns - 1) * offsetX / 2;
        float startY = -(rows - 1) * offsetY / 2;

        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                var card = Instantiate(cardPrefab, cardContainer);
                card.transform.localPosition = new Vector3(startX + c * offsetX, startY + r * offsetY, 0);
                card.Init(selectedFaces[index], OnCardClicked);
                spawnedCards.Add(card);
                index++;
            }
        }
    }

    private Vector2 GetCardWorldSize()
    {
        Card tempCard = Instantiate(cardPrefab);
        SpriteRenderer sr = tempCard.GetComponentInChildren<SpriteRenderer>();
        Vector2 size = sr.bounds.size;
        Destroy(tempCard.gameObject);
        return size;
    }

    private async void OnCardClicked(Card clickedCard)
    {
        if (isChecking || clickedCard.IsMatched || clickedCard.IsRevealed || isGameOver) return;

        soundManager?.PlayFlip();
        await clickedCard.Reveal();

        if (firstCard == null)
        {
            firstCard = clickedCard;
        }
        else if (secondCard == null)
        {
            secondCard = clickedCard;
            isChecking = true;

            await UniTask.Delay(700);

            if (firstCard.CardSprite == secondCard.CardSprite)
            {
                soundManager?.PlayMatch();
                await MoveMatchedCards(firstCard, secondCard);
                firstCard.SetMatched();
                secondCard.SetMatched();
            }
            else
            {
                soundManager?.PlayMismatch();
                await UniTask.WhenAll(firstCard.Hide(), secondCard.Hide());
            }

            firstCard = null;
            secondCard = null;
            isChecking = false;

            if (spawnedCards.All(c => c.IsMatched))
                OnGameComplete();
        }
    }

    private async UniTask MoveMatchedCards(Card cardA, Card cardB)
    {
        await UniTask.WhenAll(
            MoveAndFlipCard(cardA),
            MoveAndFlipCard(cardB)
        );
    }

    private async UniTask MoveAndFlipCard(Card card)
    {
        await card.Hide();

        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;
        Quaternion endRot = startRot;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            card.transform.position = Vector3.Lerp(startPos, lastPoint.position, t);
            card.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            await UniTask.Yield();
            elapsed += Time.deltaTime;
        }

        card.transform.position = lastPoint.position;
        card.transform.rotation = endRot;
    }

    private void OnGameComplete()
    {
        if (isGameOver) return;

        Debug.Log("Все пары найдены!");
        isGameOver = true;
        OnGameCompleted?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnGameFail()
    {
        if (isGameOver) return;

        Debug.Log("Время вышло, игра проиграна!");
        isGameOver = true;
        OnGameCompleted?.Invoke();
        gameObject.SetActive(false);
    }

    private async UniTaskVoid StartTimer()
    {
        slider.fillAmount = 1f;

        while (timeRemaining > 0 && !isGameOver)
        {
            timeRemaining -= Time.deltaTime;

            timerText.text = $"{Mathf.CeilToInt(timeRemaining)}";

            slider.fillAmount = timeRemaining / gameDuration;
            slider.color = Color.Lerp(emptyColor, fullColor, timeRemaining / gameDuration);

            await UniTask.Yield();
        }

        if (!isGameOver)
            OnGameFail();
    }


}
