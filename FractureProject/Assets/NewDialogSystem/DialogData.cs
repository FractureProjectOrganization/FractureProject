using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogData", menuName = "Dialog System/Dialog Data")]
public class DialogData : ScriptableObject
{
    [field:SerializeField] public Line[] dialog { get; private set; }
}
