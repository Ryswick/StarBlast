using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float _rotationSpeed = 5.0f;

    SpriteRenderer _spriteRenderer;

    bool _isEnemyTargeted = false;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        _isEnemyTargeted = false;
        _spriteRenderer.color = Color.white;

        StartCoroutine(CheckForEnemy());
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -9f;

        transform.position = mousePosition;
        transform.Rotate(Vector3.back, _rotationSpeed * Time.deltaTime);
    }

    IEnumerator CheckForEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            if(Physics.Raycast(transform.position, Vector3.forward, 10.0f, 1 << 9))
            {
                if (!_isEnemyTargeted)
                {
                    _isEnemyTargeted = true;
                    _spriteRenderer.color = Color.red;

                    if (DOTween.IsTweening(transform))
                    {
                        DOTween.Kill(transform);
                    }

                    transform.DOScale(0.8f, 0.2f);
                }
            }
            else
            {
                if(_isEnemyTargeted)
                {
                    _isEnemyTargeted = false;
                    _spriteRenderer.color = Color.white;
                    
                    if (DOTween.IsTweening(transform))
                    {
                        DOTween.Kill(transform);
                    }

                    transform.DOScale(1.0f, 0.2f);
                }
            }
        }
    }
}
