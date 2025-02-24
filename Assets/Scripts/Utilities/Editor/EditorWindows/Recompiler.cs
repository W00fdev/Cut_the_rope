using UnityEditor;
using UnityEditor.Compilation;

namespace _Project.Scripts.Utilities.Editor.EditorWindows
{
    public class Recompiler : EditorWindow
    {
        [MenuItem("Tools/Recompile")]
        private static void Init()
        {
            Recompile();
        }

        private static void Recompile()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}