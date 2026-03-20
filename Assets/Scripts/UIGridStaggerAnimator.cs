using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class UIGridStaggerAnimator : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] private List<RectTransform> cards = new List<RectTransform>();

    [Header("Animation")]
    [SerializeField] private float startScale = 0.8f;
    [SerializeField] private float endScale = 1f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float delayBetweenCards = 0.04f;

    private void Awake()
    {
        PrepareCards();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(PlayStaggerAnimation());
    }

    private void PrepareCards()
    {
        foreach (RectTransform card in cards)
        {
            if (card == null)
                continue;

            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = card.gameObject.AddComponent<CanvasGroup>();

            card.localScale = Vector3.one * startScale;
            cg.alpha = 0f;
        }
    }

    private IEnumerator PlayStaggerAnimation()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform card = cards[i];
            if (card != null)
                StartCoroutine(AnimateCard(card));

            yield return new WaitForSeconds(delayBetweenCards);
        }
    }

    private IEnumerator AnimateCard(RectTransform card)
    {
        CanvasGroup cg = card.GetComponent<CanvasGroup>();

        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            // Плавность
            float eased = EaseOutBack(t);

            float scale = Mathf.Lerp(startScale, endScale, eased);
            float alpha = Mathf.Lerp(0f, 1f, t);

            card.localScale = Vector3.one * scale;
            cg.alpha = alpha;

            yield return null;
        }

        card.localScale = Vector3.one * endScale;
        cg.alpha = 1f;
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    public void PlayAgain()
    {
        StopAllCoroutines();
        PrepareCards();
        StartCoroutine(PlayStaggerAnimation());
    }
}
