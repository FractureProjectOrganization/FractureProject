using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogDisplayer : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private TMP_Text playerText;
    
    [SerializeField] private TMP_Text nameText;
    
    private static readonly int StartTrigger = Animator.StringToHash("Start");
    private static readonly int StopTrigger = Animator.StringToHash("End");
    private static readonly int NextTrigger = Animator.StringToHash("Next");

    private DialogData currentDialog;
    private DialogRuntimeContainer currentDialogContainer;

    private bool? previousLineIsPlayer;

    public void DisplayDialog(DialogRuntimeContainer datas)
    {
        ResetDisplay();
        
        currentDialog = LanguageManager.instance.isVf ? datas.vfData : datas.data;

        currentDialogContainer = datas;
        StartCoroutine(ReadDialog());
    }

    private IEnumerator ReadDialog()
    {
        for (int i = 0; i < currentDialog.dialog.Length; i++)
        {
            //Debug.Log(currentDialog.dialog[i].text);
            DisplayLine(currentDialog.dialog[i]);
            ManageBubble(currentDialog.dialog[i].isPlayer,true);

            previousLineIsPlayer = currentDialog.dialog[i].isPlayer;
            if (currentDialogContainer.dialogueEvents.ContainsKey(i))
            {
                currentDialogContainer.dialogueEvents[i].Invoke();
                yield return new WaitForSeconds(2f);
            }
            
            yield return new WaitUntil(NewInput.GetInteractDown);
            yield return null;
        }
        ManageBubble(currentDialog.dialog[currentDialog.dialog.Length-1].isPlayer, false);
        ResetDisplay();
        currentDialogContainer.dialogueEndEvent.Invoke();
    }

    private void DisplayLine(Line line)
    {
        playerText.text = line.text;
        nameText.text = line.actorName;
    }
    
    private void ManageBubble(bool lineIsPlayer, bool isTalking)
    {
        playerAnimator.SetBool("PNJ",!lineIsPlayer);
        if (previousLineIsPlayer == null)
        {
            playerAnimator.SetTrigger(StartTrigger);
        }
        else if (isTalking)
        {
            playerAnimator.SetTrigger(NextTrigger);
        }
        else
        {
            playerAnimator.SetBool("Ended",true);
            playerAnimator.SetTrigger(StopTrigger);
        }
    }

    private void ResetDisplay()
    {
        StopAllCoroutines();
        //playerAnimator.SetBool("Ended",true);
        
        
        currentDialog = null;

        
        previousLineIsPlayer = null;
    }

}
