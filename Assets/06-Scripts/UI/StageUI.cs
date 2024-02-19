using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUI : Singleton<StageUI>
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] RectTransform _readyTextTransform;
    [SerializeField] RectTransform _goTextTransform;

    RectTransform _scoreTextRectTransform;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeSingleton();
        _scoreTextRectTransform = _scoreText.GetComponent<RectTransform>();
    }

    public void Initialize()
    {
        UpdateScore(0, false);
        _readyTextTransform.localScale = Vector3.zero;
        _goTextTransform.localScale = Vector3.zero;
    }

    public void UpdateScore(int scoreValue, bool shouldAnimate = true)
    {
        if (_scoreText)
        {
            _scoreText.text = $"{scoreValue:00000}";
        }
        if (shouldAnimate)
        {
            if (!DOTween.IsTweening(_scoreTextRectTransform))
            {
                _scoreTextRectTransform.DOJump(_scoreTextRectTransform.position, 0.05f, 1, 0.2f);
                _scoreText.DOColor(Color.green, 0.2f).SetLoops(2, LoopType.Yoyo);
            }
        }
    }

    internal void PlayReadyGo()
    {
        StartCoroutine(ReadyGoCoroutine());
    }

    IEnumerator ReadyGoCoroutine()
    {
        _readyTextTransform.DOScale(Vector3.one, 0.5f).From(Vector3.zero);

        yield return new WaitForSeconds(0.75f);

        _readyTextTransform.DOScale(Vector3.zero, 0.2f);
        _goTextTransform.DOScale(Vector3.one, 1.0f).From(Vector3.zero).SetEase(Ease.OutBack);
        _goTextTransform.DORotate(new Vector3(0.0f, 0.0f, 360.0f), 1.0f).From(Vector3.zero);

        yield return new WaitForSeconds(1.0f);

        _goTextTransform.DOScale(Vector3.zero, 0.2f);
    }
}
