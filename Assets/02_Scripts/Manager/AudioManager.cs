using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource _bgmSource;
    public AudioSource _sfxSource;

    [Header("Volume Setting")]
    [Range(0f, 1f)] public float _bgmVolume = 0.6f;
    [Range(0f, 1f)] public float _sfxVolume = 0.8f;

    public AudioClip _gameStartSFX;
    public AudioClip _ZombieHitSFX;
    public AudioClip _ZombieAttackSFX;
    public AudioClip[] _ZombieChaseSFX;

    private int _zombieChaseIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            

            GameManager.OnGameSceneStarted += PlayGameStartSound;
            ZombieController.OnZombieHit += PlayZombieHit;
            ZombieController.OnZombieAttack += PlayZombieAttack;
            ZombieController.OnZombieChase += PlayZombieChase;

            _bgmSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _bgmSource.volume = _bgmVolume;
        _sfxSource.volume = _sfxVolume;
    }

    private void OnDestroy()
    {
        GameManager.OnGameSceneStarted -= PlayGameStartSound;
        ZombieController.OnZombieHit -= PlayZombieHit;
        ZombieController.OnZombieAttack -= PlayZombieAttack;
        ZombieController.OnZombieChase -= PlayZombieChase;
    }

    /// <summary>
    /// 배경음악
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="loop"></param>
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null)
            return;

        _bgmSource.clip = clip;
        _bgmSource.loop = loop;
        _bgmSource.Play();
    }

    public void FadeBGM(AudioClip newClip, float duration = 1f)
    {
        StartCoroutine(FadeRoutine(newClip, duration));
    }

    private IEnumerator FadeRoutine(AudioClip newClip, float duration)
    {
        float startVol = _bgmVolume;
        float time = 0f;

        //페이드 아웃
        while (time < duration)
        {
            _bgmSource.volume = Mathf.Lerp(startVol, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _bgmSource.Stop();
        _bgmSource.clip = newClip;
        _bgmSource.Play();

        //페이드 인
        time = 0f;
        while (time < duration)
        {
            _bgmSource.volume = Mathf.Lerp(0, startVol, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _bgmSource.volume = startVol;
    }

    /// <summary>
    /// 효과음
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) 
            return;
        _sfxSource.PlayOneShot(clip, _sfxVolume);
    }

    private void PlayGameStartSound()
    {
        if (_gameStartSFX != null)
            PlaySFX(_gameStartSFX);
    }

    private void PlayZombieHit()
    {
        if (_ZombieHitSFX != null)
            PlaySFX(_ZombieHitSFX);
    }

    private void PlayZombieAttack()
    {
        if (_ZombieAttackSFX != null)
            PlaySFX(_ZombieAttackSFX);
    }

    private void PlayZombieChase()
    {
        if (_ZombieChaseSFX == null || _ZombieChaseSFX.Length == 0)
            return;

        PlaySFX(_ZombieChaseSFX[_zombieChaseIndex]);

        _zombieChaseIndex++;

        if (_zombieChaseIndex >= _ZombieChaseSFX.Length)
            _zombieChaseIndex = 0;
    }
}
