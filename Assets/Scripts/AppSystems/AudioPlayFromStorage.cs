using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class AudioPlayFromStorage : MonoBehaviour {

    [SerializeField]
    private Text fileNameText;
    private AudioSource audioSource;
    private bool hasLoadingTried;

	private void Start () {
        audioSource = GetComponent<AudioSource>();

        //string header = "file:///sdcard/Music/AudioLoadingTest/";
        string header = "file:///sdcard/Music/";
        //string fileName = "Ocean_Waves.mp3";
        string fileName = "Totally_Happy_Ending.mp3";
        StartCoroutine(LoadAudio(header + fileName));
        
    }

    private void Update()
    {
        /*
        if (hasLoadingTried) { return; }
        string header = "file:///sdcard/Music/AudioLoadingTest/";
        string fileName = "Ocean_Waves.mp3";
        StartCoroutine(LoadAudio(header + fileName));
        hasLoadingTried = true;
        */
    }

    private IEnumerator LoadAudio(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            Debug.Log("File does NOT exist!! file path = " + path);
            fileNameText.text = "File does NOT exist!! file path = " + path;
            yield break;
        }

        // Load a file
        WWW request = new WWW(path);

        // wait until complete
        while (!request.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        // Get audio clip from the file
        AudioClip audio = request.GetAudioClip(false, true);

        // wait until complete loading
        while (audio.loadState == AudioDataLoadState.Loading)
        {
            yield return new WaitForEndOfFrame();
        }

        if(audio.loadState != AudioDataLoadState.Loaded)
        {
            // Fail to load
            Debug.Log("Failed to Load!");
            fileNameText.text = fileNameText.text + "Failed to Load!";
            yield break;
        }

        // set the audio clip on the audio source
        audioSource.clip = audio;
        fileNameText.text = path + "is Loaded!";

        PlayAudio();
        yield break;
    }

    private void PlayAudio()
    {
        if (audioSource.isPlaying) { return; }
        audioSource.Play();
    }
}
