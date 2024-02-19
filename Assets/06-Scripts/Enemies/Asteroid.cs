using UnityEngine;
using System.Collections;

public class Asteroid : Enemy
{
    [Header("Parameters")]
    [SerializeField] float _tumble;
    [SerializeField] float _mixScale;
    [SerializeField] float _maxScale;

    protected override void OnEnable()
    {
        base.OnEnable();

        transform.localScale = Vector3.one * Random.Range(_mixScale, _maxScale);

        transform.rotation = Random.rotation;
    }

    void Update()
    {
        if(_isBehaviourActivated)
            transform.rotation *= Quaternion.AngleAxis(_tumble * Time.deltaTime, new Vector3(1, 1, 0));
    }
}