using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float MusicVolume = 1f;
    [Range(0f, 1f)] public float SFXVolume = 1f;
    private float savedMusicVolume, savedSFXVolume;
    [Header("Mixer Reference")]
    [SerializeField] private AudioMixer _masterMixer;

    [Header("UI Settings")]
    public bool AllowPopups = true;

    [Header("External Links")]
    [SerializeField] private List<ExternalLink> externalLinks;

    private Dictionary<string, string> _linkLookup;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Cache links for fast lookup
        _linkLookup = new Dictionary<string, string>();
        foreach (var link in externalLinks)
        {
            if (!_linkLookup.ContainsKey(link.key))
                _linkLookup.Add(link.key, link.url);
        }
    }

    public void ShouldShowPopups(bool option)
    {
        AllowPopups = option;
    }

    public void MuteAll(bool option)
    {
        if (option)
        {
            MusicVolume = 0f;
            SFXVolume = 0f;
        }
        else
        {
            SetMusicVolume(savedMusicVolume);
            SetSFXVolume(savedSFXVolume);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        _masterMixer.SetFloat("MusicVolume", db);
        savedMusicVolume = db;
    }

    public void SetSFXVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        _masterMixer.SetFloat("SFXVolume", db);
        savedSFXVolume = db;
    }

    public void OpenExternalLink(string key)
    {
        if (_linkLookup.TryGetValue(key, out string url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning($"No link registered with key: {key}");
        }
    }

    [System.Serializable]
    public class ExternalLink
    {
        public string key;
        public string url;
    }
}
