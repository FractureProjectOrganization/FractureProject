using System;
using JetBrains.Annotations;
using UnityEngine;

public class ActSplashscreensPlayer : MonoBehaviour
{
    private Animator anim;
    private string save;
    public GameObject secretButton;
    public bool secretIsOn = false;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

   private void FixedUpdate()
   {
       if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != save)
       {
           save = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
           TestTrigger();
       }
   }

   private void TestTrigger()
   {
       switch (save)
       {
           case "Present I" : anim.SetTrigger("Tr_Acte1"); break;
           case "Present II" : anim.SetTrigger("Tr_Acte2"); break;
           case "Present III" : anim.SetTrigger("Tr_Acte3"); break;
           case "Credits" : secretButton.SetActive(true);
               if (secretIsOn && secretButton.GetComponent<SecretButton>().active)
               {
                   AchievementManager.instance.TriggerAchievement("SquidGame");
               }
               else
               {
                   secretIsOn = true;
               }
               break;
           default: break;

       }
   }
}
