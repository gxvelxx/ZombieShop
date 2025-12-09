using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class ShieldLightController : MonoBehaviour
{
    [Header("Light Settings")]
    public Light _shieldLight;          // 방패의 빛
    public float _maxIntensity = 500f;    // 최대 밝기
    public float _fadeSpeed = 2f;       // 밝기 전환 속도

    private XRGrabInteractable _grab;
    private bool _isHolding = false;

    private void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();

        _grab.selectEntered.AddListener(OnGrab);
        _grab.selectExited.AddListener(OnRelease);

        //초기엔 꺼진 상태 유지
        if (_shieldLight != null)
        {
            _shieldLight.intensity = 0f;
            _shieldLight.enabled = false;   // 빛자체 비활성화
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _isHolding = true;
        if (_shieldLight != null)
        {
            _shieldLight.enabled = true; // 빛활성화
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        _isHolding = false;
        if (_shieldLight != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        while (_shieldLight.intensity < _maxIntensity)
        {
            _shieldLight.intensity += Time.deltaTime * _fadeSpeed;
            yield return null;
        }
        _shieldLight.intensity = _maxIntensity;
    }

    private IEnumerator FadeOut()
    {
        while (_shieldLight.intensity > 0f)
        {
            _shieldLight.intensity -= Time.deltaTime * _fadeSpeed;
            yield return null;
        }

        _shieldLight.intensity = 0f;
        _shieldLight.enabled = false;   // 완전히 꺼진 후 비활성화
    }
}
