using System;
using UnityEngine;
using Naninovel;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer frontFace;
    [SerializeField] private GameObject backFace;

    public Sprite CardSprite { get; private set; }
    public bool IsRevealed { get; private set; }
    public bool IsMatched { get; private set; }

    private Action<Card> onClick;
    private bool isAnimating = false;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Init(Sprite sprite, Action<Card> callback)
    {
        CardSprite = sprite;
        frontFace.sprite = sprite;
        onClick = callback;
        HideInstant();
    }

    public async UniTask Reveal()
    {
        if (isAnimating || IsRevealed) return;
        isAnimating = true;

        await Flip(true);
        IsRevealed = true;
        isAnimating = false;
    }

    public async UniTask Hide()
    {
        if (isAnimating || !IsRevealed) return;
        isAnimating = true;

        await Flip(false);
        IsRevealed = false;
        isAnimating = false;
    }

    public void HideInstant()
    {
        transform.localScale = originalScale;
        frontFace.gameObject.SetActive(false);
        backFace.SetActive(true);
        IsRevealed = false;
        IsMatched = false;
    }

    public void SetMatched()
    {
        IsMatched = true;
    }

    private async UniTask Flip(bool showFront)
    {
        float duration = 0.15f;
        float elapsed = 0f;

        Vector3 startScale = originalScale;
        Vector3 midScale = new Vector3(0f, originalScale.y, originalScale.z);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, midScale, t);
            await UniTask.Yield();
            elapsed += Time.deltaTime;
        }

        frontFace.gameObject.SetActive(showFront);
        backFace.SetActive(!showFront);

        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(midScale, startScale, t);
            await UniTask.Yield();
            elapsed += Time.deltaTime;
        }

        transform.localScale = startScale;
    }

    private void OnMouseDown()
    {
        if (!IsMatched && !isAnimating)
            onClick?.Invoke(this);
    }
}
