using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    Image SettingPage;

    AudioSource bgmAudio;
    AudioSource effectAudio;

    Slider bgmSlider;
    Slider effectSlider;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        SettingPage = canvas.transform.Find("SettingPage").GetComponent<Image>();
        
        bgmAudio = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        effectAudio = GameObject.Find("Effect Audio Source").GetComponent<AudioSource>();
        
        bgmSlider = SettingPage.transform.Find("BGMSlider").GetComponent<Slider>();
        bgmSlider.value = bgmAudio.volume;
        bgmSlider.onValueChanged.AddListener(MusicSoundSlider);

        effectSlider = SettingPage.transform.Find("EffectSlider").GetComponent<Slider>();
        effectSlider.value = effectAudio.volume;
        effectSlider.onValueChanged.AddListener(EffectSoundSlider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        SettingPage.transform.SetAsLastSibling();
    }

    public void SettingButtonClicked()
    {
        SettingPage.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void SettingPageExitClicked()
    {
        SettingPage.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void MusicSoundSlider(float volume)
    {
        bgmAudio.volume = volume;
    }

    public void EffectSoundSlider(float volume)
    {
        effectAudio.volume = volume;
    }

    public void restartButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void settingFinishClicked()
    {
        Time.timeScale = 1;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TeacherButtonClicked()
    {
        Time.timeScale = 1;
        Debug.Log("Teacher Button Clicked");
    }
}
