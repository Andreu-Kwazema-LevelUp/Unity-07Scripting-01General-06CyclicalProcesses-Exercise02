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


    private bool _scale;

    private int _steps;

    private float _transitionStep;

    private float _direction = 1;

    private Vector3 _currentValue;

    private PointsMovement _pointsMovement;

    #endregion


    #region Unity Lifecycle

    private void Awake()
    {
        _pointsMovement = GetComponent<PointsMovement>();
    }

    private void Start()
    {
        _currentValue = transform.localScale;

        _transitionStep = 0;

        _steps = 0;

        _scale = false;

        StartCoroutine(CheckStartScalerLoop());
    }

    #endregion


    #region Corroutines

    IEnumerator ScalerLoop()
    {
        while (_scale)
        {
            if (_scale && _steps < 1)
            {
                _transitionStep += _direction * Time.deltaTime;

                float step = Math.Min(_transitionStep / _transition, 1);

                transform.localScale = Vector3.Lerp(_currentValue, _currentValue * _scaleMultiplier, step);

                if (step >= 1)
                {
                    _direction = -_direction;
                }
                else if (step <= 0)
                {
                    _direction = -_direction;

                    _scale = false;

                    _transitionStep = 0;

                    _steps = 0;

                    StartCoroutine(CheckStartScalerLoop());
                }
            }

            yield return null;
        }
    }


    IEnumerator CheckStartScalerLoop()
    {
        while (!_scale)
        {
            if (_pointsMovement.Loops % _loops == 0)
            {
                _scale = true;
                StartCoroutine(ScalerLoop());
            }

            yield return null;
        }
    }

    #endregion
}
