using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TV : MonoBehaviour, IInteractable
{
    private VideoPlayer _videoPlayer;
    private GameObject _videoContainer;
    private Outline _outline;

    private void Start()
    {
        _outline = GetComponentInChildren<Outline>();
        _outline.enabled = false;
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoContainer = transform.GetChild(0).gameObject;
    }

    public void HideOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = false;
        }
    }

    public void Interact()
    {
        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Stop();
            _videoContainer.SetActive(false);
        }
        else
        {
            _videoContainer.SetActive(true);
            _videoPlayer.Play();
        }
    }

    public void ShowOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = true;
        }
    }
}