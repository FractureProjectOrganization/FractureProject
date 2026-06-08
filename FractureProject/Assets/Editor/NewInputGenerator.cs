#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputGenerator
{
    [MenuItem("Tools/Générer la classe NewInput")]
    public static void GenerateClass()
    {
        Type playerActionsType = typeof(PlayerControls.PlayerActions);
        PropertyInfo[] properties = playerActionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        StringBuilder code = new StringBuilder();
        code.AppendLine("using UnityEngine;");
        code.AppendLine();
        code.AppendLine("public static class NewInput");
        code.AppendLine("{");
        code.AppendLine("    private static PlayerControls _controls;");
        code.AppendLine("    private static PlayerControls Controls");
        code.AppendLine("    {");
        code.AppendLine("        get");
        code.AppendLine("        {");
        code.AppendLine("            if (_controls == null)");
        code.AppendLine("            {");
        code.AppendLine("                _controls = new PlayerControls();");
        code.AppendLine("                _controls.Enable();");
        code.AppendLine("            }");
        code.AppendLine("            return _controls;");
        code.AppendLine("        }");
        code.AppendLine("    }");
        code.AppendLine();
        code.AppendLine("    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]");
        code.AppendLine("    private static void ResetState()");
        code.AppendLine("    {");
        code.AppendLine("        if (_controls != null)");
        code.AppendLine("        {");
        code.AppendLine("            _controls.Disable();");
        code.AppendLine("            _controls = null;");
        code.AppendLine("        }");
        code.AppendLine("    }");
        code.AppendLine();
        code.AppendLine("    public static Vector2 GetMovement() => Controls.Player.Move.ReadValue<Vector2>();");
        code.AppendLine("    public static float GetAxisHorizontal() => Controls.Player.Move.ReadValue<Vector2>().x;");
        code.AppendLine("    public static float GetAxisVertical() => Controls.Player.Move.ReadValue<Vector2>().y;");
        code.AppendLine();

        foreach (PropertyInfo prop in properties)
        {
            if (prop.PropertyType == typeof(InputAction))
            {
                string actionName = prop.Name;
                if (actionName == "Move") continue;

                code.AppendLine($"    public static bool Get{actionName}Down() => Controls.Player.{actionName}.WasPressedThisFrame();");
                code.AppendLine($"    public static bool Get{actionName}() => Controls.Player.{actionName}.IsPressed();");
                code.AppendLine($"    public static bool Get{actionName}Up() => Controls.Player.{actionName}.WasReleasedThisFrame();");
                code.AppendLine();
            }
        }

        code.AppendLine("}");
        
        string folderPath = Path.Combine(Application.dataPath, "Input");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string path = Path.Combine(folderPath, "NewInput.cs");
        File.WriteAllText(path, code.ToString());
        
        AssetDatabase.Refresh();
        
        Debug.Log("<color=green><b>[NewInput]</b> Classe générée et mise à jour avec succès !</color>");
    }
}
#endif