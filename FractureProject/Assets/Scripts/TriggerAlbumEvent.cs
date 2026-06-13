using System;
using UnityEngine;

public class TriggerAlbumEvent : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = AlbumPhoto.instance.gameObject.GetComponent<Animator>();
    }

    public void AlbumShow()
    {
        anim.SetTrigger("Open");
    }

    public void AlbumHide()
    {
        anim.SetTrigger("Close");

    }
}
