using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum EaseType
{
    Linear,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic
}

public class CombatTransition : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float _duration = .6f;
    [SerializeField] private EaseType _easeType = EaseType.EaseInCubic;

    [Header("End Values")]
    [SerializeField] private float _lensDistortionIntensity = -0.8f;
    [SerializeField] private float _depthOfFieldFocus = 0;
    [SerializeField] private float _chromaticAberrationIntensity = 1;
    [SerializeField] private float _vignetteIntensity = .5f;

    [Header("Default Values")]
    [SerializeField] private float _defaultLensDistortionIntensity = 0;
    [SerializeField] private float _defaultDepthOfFieldFocusDistance = 10;
    [SerializeField] private float _defaultChromaticAberrationIntensity = 0;
    [SerializeField] private float _defaultVignetteIntensity = 0;

    [Header("Debug: Refs")]
    [SerializeField, ReadOnly] private Volume _volume;
    [SerializeField, ReadOnly] private LensDistortion _lensDistortion;
    [SerializeField, ReadOnly] private DepthOfField _depthOfField;
    [SerializeField, ReadOnly] private ChromaticAberration _chromaticAberration;
    [SerializeField, ReadOnly] private Vignette _vignette;

    private void Awake()
    {
        _volume = GetComponent<Volume>();
        if (_volume.profile.TryGet(out _lensDistortion) &&
            _volume.profile.TryGet(out _depthOfField) &&
            _volume.profile.TryGet(out _chromaticAberration) &&
            _volume.profile.TryGet(out _vignette))
        {
            _lensDistortion.intensity.value = _defaultLensDistortionIntensity;
            _depthOfField.focusDistance.value = _defaultDepthOfFieldFocusDistance;
            _chromaticAberration.intensity.value = _defaultChromaticAberrationIntensity;
            _vignette.intensity.value = _defaultVignetteIntensity;
        }
    }

    public void StartCombatTransition(Action onFinished = null)
    {
        StartCoroutine(AnimatePostProcessingEffects(onFinished));
    }

    private IEnumerator AnimatePostProcessingEffects(Action onFinished = null)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _duration;

            // apply selected easing function
            switch (_easeType)
            {
                case EaseType.Linear:
                    // no change needed for linear
                    break;
                case EaseType.EaseInCubic:
                    t = t * t * t;
                    break;
                case EaseType.EaseOutCubic:
                    t = 1 - Mathf.Pow(1 - t, 3);
                    break;
                case EaseType.EaseInOutCubic:
                    t = t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
                    break;
            }

            _lensDistortion.intensity.value = Mathf.Lerp(_defaultLensDistortionIntensity, _lensDistortionIntensity, t);
            _depthOfField.focusDistance.value = Mathf.Lerp(_defaultDepthOfFieldFocusDistance, _depthOfFieldFocus, t);
            _chromaticAberration.intensity.value = Mathf.Lerp(_defaultChromaticAberrationIntensity, _chromaticAberrationIntensity, t);
            _vignette.intensity.value = Mathf.Lerp(_defaultVignetteIntensity, _vignetteIntensity, t);

            yield return null;
        }

        _lensDistortion.intensity.value = _lensDistortionIntensity;
        _depthOfField.focusDistance.value = _depthOfFieldFocus;
        _chromaticAberration.intensity.value = _chromaticAberrationIntensity;
        _vignette.intensity.value = _vignetteIntensity;

        onFinished?.Invoke();
    }
}
