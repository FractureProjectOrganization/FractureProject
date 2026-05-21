using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class DialogRuntimeContainer : MonoBehaviour
{
    [field:SerializeField] public DialogData data { get; private set; }
    [field:SerializeField] public Animator pnjBubbleAnimator { get; private set; }
    [field:SerializeField] public TMP_Text pnjBubbleText { get; private set; }
    
    [field:SerializeField] public UnityEvent dialogueEndEvent { get; private set; }
    
    [SerializedDictionary("LineIndex","Event")]
    [field:SerializeField] public SerializedDictionary<int,UnityEvent> dialogueEvents = new SerializedDictionary<int, UnityEvent>(); 

}
