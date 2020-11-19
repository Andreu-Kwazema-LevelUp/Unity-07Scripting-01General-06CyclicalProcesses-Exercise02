using System;
using System.Collections;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private int _loops;

    [SerializeField]
    private float _scaleMultiplier;

    [SerializeField]
    private float _transition;

    private int _steps;

    private int _currentLoop;

    private PointsMovement _pointsMovement;

    #endregion


    #region Unity Lifecycle

    private void Awake()
    {
        _pointsMovement = GetComponent<PointsMovement>();
    }

    private void Start()
    {
        _steps = 0;

        StartCoroutine(DoCheckLoops());
    }

    #endregion


    #region Corroutines

    private IEnumerator DoCheckLoops()
    {
        yield return new WaitUntil(() => _pointsMovement.Loops - _currentLoop >= _loops);

        yield return StartCoroutine(DoPingPong());

        _currentLoop = _pointsMovement.Loops;

        StartCoroutine(DoCheckLoops());
    }

    private IEnumerator DoPingPong()
    {
        Vector3 current = transform.localScale;
        Vector3 next = current * _scaleMultiplier;

        yield return StartCoroutine(DoTransition(current, next));
        yield return StartCoroutine(DoTransition(next, current));
    }

    private IEnumerator DoTransition(Vector3 current, Vector3 next)
    {
        float transitionStep = 0;

        while (_transition > transitionStep)
        {
            transitionStep += Time.deltaTime;

            float step = transitionStep / _transition;

            transform.localScale = Vector3.Lerp(current, next, step);

            yield return null;
        }
    }


    #endregion
}
