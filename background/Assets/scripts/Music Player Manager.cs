using UnityEngine;
using UnityEngine.UI;

public class MusicPlayerManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] tracks;

    public Button playPauseButton;
    public Sprite playSprite;
    public Sprite pauseSprite;

    private int currentTrackIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        UpdatePlayPauseButtonSprite();
    }

    public void TogglePlayPause()
    {
        if (tracks.Length == 0) return;

        if (!audioSource.isPlaying)
        {
            audioSource.clip = tracks[currentTrackIndex];
            audioSource.loop = true;
            audioSource.Play();
            isPlaying = true;
        }
        else
        {
            audioSource.Pause();
            isPlaying = false;
        }

        UpdatePlayPauseButtonSprite();
    }

    public void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % tracks.Length;
        audioSource.clip = tracks[currentTrackIndex];
        audioSource.Play();
        isPlaying = true;
        UpdatePlayPauseButtonSprite();
    }

    public void PreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + tracks.Length) % tracks.Length;
        audioSource.clip = tracks[currentTrackIndex];
        audioSource.Play();
        isPlaying = true;
        UpdatePlayPauseButtonSprite();
    }

    private void UpdatePlayPauseButtonSprite()
    {
        if (playPauseButton != null)
        {
            Image btnImage = playPauseButton.GetComponent<Image>();
            btnImage.sprite = isPlaying ? pauseSprite : playSprite;
        }
    }
}
