using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSteps : MonoBehaviour
{
    [SerializeField] private IntVariable _comboStepsCount;
    [SerializeField] private TrailRenderer _trailRenderer;

    [SerializeField] private Color _normalStartColor = new Color(0.9150f, 0.540f, 0.0561f);
    [SerializeField] private Color _normalEndColor = new Color(1f, 0.2915f, 0.1367f);

    [SerializeField] private Color _comboStartColor = Color.green;
    [SerializeField] private Color _comboEndColor = Color.yellow;
    private void OnEnable()
    {
        _comboStepsCount.onValueChanged += ChangeBallTrail;
    }

    private void OnDisable()
    {
        _comboStepsCount.onValueChanged -= ChangeBallTrail;
    }

    private void ChangeBallTrail()
    {
        if (_comboStepsCount.Value > 3)
        {
            SetComboColors();
        }
        else
        {
            SetNormalColors();
        }
        
    }

    private void SetNormalColors()
    {
        _trailRenderer.startColor = _normalStartColor;
        _trailRenderer.endColor = _normalEndColor;
    }

    private void SetComboColors()
    {
        _trailRenderer.startColor = _comboStartColor;
        _trailRenderer.endColor = _comboEndColor;
    }
}
