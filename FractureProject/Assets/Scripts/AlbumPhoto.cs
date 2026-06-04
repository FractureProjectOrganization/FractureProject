using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AlbumPhoto : MonoBehaviour
{
    bool open,close = true, turned;
    Animator anim;
    public Player player;
    public GameObject[] images = new  GameObject[3];
    public RectTransform[] marquePages;
    private int index =0;
    private int pageUnlocked = 1;
    public float timeBforeTurn, marquePageDeformation;
    public GameObject leftArow, rightArrow;

    public static AlbumPhoto instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Start()
    {
        anim = GetComponent<Animator>();
        player = Player.instance;
        marquePages[0].localScale = new Vector3(1+marquePageDeformation,1+marquePageDeformation,1+marquePageDeformation);
    }
    void Update()
    {
        if(Input.GetKeyDown("e") || Input.GetButtonDown("Fire3"))
        {
            if (player && player.locked) return;
            if (!open)
            {
                open = true;
                close = false;
                anim.SetTrigger("Open");
                if (player)
                {
                    player.LockPlayer(true);
                }
            }
        }

        if (!Input.GetKey("e") && !Input.GetButton("Fire3"))
        {
            if (!close)
            {
                open = false;
                close = true;
                anim.SetTrigger("Close");
                if (player)
                {
                    player.LockPlayer(false);
                }
            }
        }

        if (open)
        {
            if (Mathf.RoundToInt(Input.GetAxis("Horizontal")) != 0)
            {
                if (!turned)
                {
                    
                    StartCoroutine(TurningPageCoroutine(Mathf.RoundToInt(Input.GetAxis("Horizontal"))));
                    turned = true;
                }
            }
            else
            {
                turned = false;
            }
        }
        
        
        
    }

    void TurnPage(int movement)
    {
        if (close) return;

        switch (index)
        {
            case 0:
                leftArow.SetActive(false);
                rightArrow.SetActive(true);
                break;
            case 1:
                leftArow.SetActive(true);
                if (pageUnlocked>1)
                rightArrow.SetActive(true);
                else rightArrow.SetActive(false);
                break;
            case 2:
                leftArow.SetActive(true);
                rightArrow.SetActive(false);
                break;
            default:
                leftArow.SetActive(true);
                rightArrow.SetActive(true);
                break;
        }
        for (int i=0; i<3; i++ )
        {
            GameObject image = images[i];
            if(i != index) image.SetActive(false);
            else image.SetActive(true);
        }
    }
    
    private IEnumerator TurningPageCoroutine(int movement)
    {
        index += movement;
        index = Mathf.Clamp(index ,0, pageUnlocked);
        float timer = 0f;
        while (timer < timeBforeTurn)
        {
            timer += Time.deltaTime;
            for (int i=0; i<pageUnlocked+1; i++ )
            {
                RectTransform mp = marquePages[i];
                float zoom = 0;
                if(i==index)  zoom = 1 + (marquePageDeformation * (timer/timeBforeTurn));
                else  zoom = Mathf.Lerp(mp.localScale.x, 1, (timer/timeBforeTurn));
                mp.localScale = new Vector3(zoom,zoom,zoom);
            }
            yield return null;
        }
        TurnPage(movement);
        
    }

    public static void AddPage()
    {
        instance.pageUnlocked++;
    }
}
