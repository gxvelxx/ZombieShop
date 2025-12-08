using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    [Header("Setting")]
    public Image _flashImage;
    [Range(0f, 1f)] public float _maxAlpha = 0.2f;
    public float _fadeDuration = 0.2f;

    private Coroutine _flashRoutine;

    private void Awake()
    {
        if (_flashImage != null)
        {
            Color color = _flashImage.color;
            color.a = 0f; // 알파값 투명하게
            _flashImage.color = color;
        }
    }

    public void PlayFlash()
    {
        if (_flashImage == null)
            return;
        
        if (_flashRoutine != null)
            StopCoroutine( _flashRoutine);

        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Color color = _flashImage.color;
        color.a = _maxAlpha;
        _flashImage.color = color;

        float time = 0f;
        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            float normalized = time / _fadeDuration;
            color.a = Mathf.Lerp(_maxAlpha, 0f, normalized);
            _flashImage.color = color;
            yield return null;
        }

        //다시 투명하게
        color.a = 0f;
        _flashImage.color = color;
        _flashRoutine = null;
    }
}
