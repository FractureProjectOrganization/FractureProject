using System;
using UnityEngine;

public class TriggerAlbumEvent : MonoBehaviour
{
    private Animator anim;
    private GameObject[] images;

    private void Start()
    {
        anim = AlbumPhoto.instance.gameObject.GetComponent<Animator>();
        images = AlbumPhoto.instance.images;
    }

    public void AlbumShow(int imageIndex = 0)
    {
        for(int i = 0; i < images.Length; i++)
        {
            if(i==imageIndex) images[i].SetActive(true);
            else images[i].SetActive(false);
        }
        anim.SetTrigger("Open");
    }

    public void AlbumHide()
    {
        anim.SetTrigger("Close");

    }
}
