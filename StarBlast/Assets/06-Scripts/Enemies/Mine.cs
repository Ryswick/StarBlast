using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Enemy
{
    [Header("References")]
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] GameObject _prefabExplosion;

    [Header("Parameters")]
    [SerializeField] float _timeBeforeExplosion = 5.0f;

    Vector3 _startScale;
    Color _startColor;
    Color _startEmissiveColor;

    AudioSource _mineSound;

    Coroutine _blinkCoroutine;

    bool _hasDetonated = false;

    protected override void Awake()
    {
        base.Awake();

        _startScale = transform.localScale;
        _startColor = _meshRenderer.materials[0].color;
        _startEmissiveColor = _meshRenderer.materials[1].GetColor("_EmissionColor");
        _mineSound = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();

        _hasDetonated = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        transform.localScale = _startScale;
        _meshRenderer.materials[0].color = _startColor;
        _meshRenderer.materials[1].SetColor("_EmissionColor", _startEmissiveColor);

        StopAllCoroutines();

        _blinkCoroutine = null;
    }

    public override void ActivateBehaviour()
    {
        base.ActivateBehaviour();

        StartCoroutine(Blink());
        StartCoroutine(Detonate());
    }

    // Update is called once per frame
    IEnumerator Blink()
    {
        float duration = 0.5f;

        while (!_hasDetonated)
        {
             transform.DOScale(_startScale * 1.05f, duration).SetLoops(2, LoopType.Yoyo);
            _meshRenderer.materials[0].DOColor(Color.red * 15.0f, duration).SetLoops(2, LoopType.Yoyo);
            _meshRenderer.materials[1].DOColor(Color.yellow * 15.0f, Shader.PropertyToID("_EmissionColor"), duration).SetLoops(2, LoopType.Yoyo);

            _mineSound.Play();

            yield return new WaitForSeconds(duration * 2.0f);

            duration = duration / 1.25f;

            if (duration < 0.02f)
                duration = 0.02f;
        }
    }

    IEnumerator Detonate()
    {
        yield return new WaitForSeconds(_timeBeforeExplosion);

        if(_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);

        _mineSound.Stop();

        LeanPool.Spawn(_prefabExplosion, transform.position, _prefabExplosion.transform.rotation, StageLoop.Instance.StageTransform);
        SoundManager.Instance.PlaySound(SoundType.MineExplosion);

        yield return new WaitForSeconds(0.5f);

        if(gameObject.activeInHierarchy)
            LeanPool.Despawn(gameObject);
    }
}
