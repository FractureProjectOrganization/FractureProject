using System.Collections;
using TMPro;
using UnityEngine;

public class DialogDisplayer : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    private Animator pnjAnimator;

    [SerializeField] private TMP_Text playerText;
    private TMP_Text pnjText;
    
    private static readonly int StartTrigger = Animator.StringToHash("Start");
    private static readonly int StopTrigger = Animator.StringToHash("End");
    private static readonly int NextTrigger = Animator.StringToHash("Next");

    private DialogData currentDialog;

    private bool? previousLineIsPlayer;

    public void DisplayDialog(DialogRuntimeContainer datas)
    {
        ResetDisplay();
        
        currentDialog = datas.data;
        pnjAnimator = datas.pnjBubbleAnimator;
        pnjText = datas.pnjBubbleText;
        
        StartCoroutine(ReadDialog());
    }

    private IEnumerator ReadDialog()
    {
        for (int i = 0; i < currentDialog.dialog.Length; i++)
        {
            DisplayLine(currentDialog.dialog[i]);
            ManageBubble(currentDialog.dialog[i].isPlayer);

            previousLineIsPlayer = currentDialog.dialog[i].isPlayer;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
        }
        
        ResetDisplay();
    }

    private void DisplayLine(Line line)
    {
        if (line.isPlayer)
            playerText.text = line.text;
        else if (pnjText != null)
            pnjText.text = line.text;
    }
    
    private void ManageBubble(bool isPlayerTalking)
    {
        switch (previousLineIsPlayer, isPlayerTalking)
        {
            case (null, true):
                playerAnimator.SetTrigger(StartTrigger);
                break;
            case (null, false):
                pnjAnimator?.SetTrigger(StartTrigger);
                break;

            case (true, true):
                playerAnimator.SetTrigger(NextTrigger);
                break;
            case (false, true):
                pnjAnimator?.SetTrigger(StopTrigger);
                playerAnimator.SetTrigger(StartTrigger);
                break;

            case (false, false):
                pnjAnimator?.SetTrigger(NextTrigger);
                break;
            case (true, false):
                playerAnimator.SetTrigger(StopTrigger);
                pnjAnimator?.SetTrigger(StartTrigger);
                break;
        }
    }

    private void ResetDisplay()
    {
        StopAllCoroutines();
        playerAnimator.SetTrigger(StopTrigger);
        pnjAnimator?.SetTrigger(StopTrigger);
        
        currentDialog = null;
        pnjAnimator = null;
        pnjText = null;
        
        previousLineIsPlayer = null;
    }
}
