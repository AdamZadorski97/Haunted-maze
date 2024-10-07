using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

// Simple example of stripping of a debug build configuration
class ShaderDebugBuildProcessor : IPreprocessShaders
{
    public ShaderDebugBuildProcessor()
    {

    }

    // Multiple callback may be implemented.
    // The first one executed is the one where callbackOrder is returning the smallest number.
    public int callbackOrder { get { return 0; } }

    public void OnProcessShader(
        Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData)
    {
        //UnityEngine.Debug.Log($"Shader {shader.name}");
        if (shader.name.Contains("UberPost"))
        {
            //UnityEngine.Debug.Log($"- Shader cleared");
            shaderCompilerData.Clear();
        }
    }
}
