using TMPro;
using UnityEngine;

public class DialogRuntimeContainer : MonoBehaviour
{
    [field:SerializeField] public DialogData data { get; private set; }
    [field:SerializeField] public Animator pnjBubbleAnimator { get; private set; }
    [field:SerializeField] public TMP_Text pnjBubbleText { get; private set; }
}
