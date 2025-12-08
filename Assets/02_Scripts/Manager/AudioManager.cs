using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Volume Setting")]
    [Range(0f, 1f)] public float bgmVolume = 0.6f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //씬전환시 파괴되지 않게 직접 추가
            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            bgmSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
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

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void FadeBGM(AudioClip newClip, float duration = 1f)
    {
        StartCoroutine(FadeRoutine(newClip, duration));
    }

    private IEnumerator FadeRoutine(AudioClip newClip, float duration)
    {
        float startVol = bgmVolume;
        float time = 0f;

        //페이드 아웃
        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVol, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        //페이드 인
        time = 0f;
        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(0, startVol, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = startVol;
    }

    /// <summary>
    /// 효과음
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) 
            return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
