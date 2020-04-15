using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace SkatanicStudios
{
    [InitializeOnLoad]
    public class ScreenshotterPipelineType
    {
        const string DEFINE_HDRP = "UNITY_HDRP";
        const string DEFINE_URP = "UNITY_URP";
        const string DEFINE_BUILTIN = "UNITY_BUILT_IN";

        public enum PipelineType
        {
            HighDefinition,
            Universal,
            BuiltIn,
            None
        }

        public static PipelineType GetPipelineType()
        {
#if UNITY_2019_1_OR_NEWER
            if (GraphicsSettings.renderPipelineAsset != null)
            {
                // SRP
                var srpType = GraphicsSettings.renderPipelineAsset.GetType().ToString();
                if (srpType.Contains("HDRenderPipelineAsset"))
                {
                    return PipelineType.HighDefinition;
                }
                else if (srpType.Contains("UniversalRenderPipelineAsset") || srpType.Contains("LightweightRenderPipelineAsset"))
                {
                    return PipelineType.Universal;
                }
                else return PipelineType.None;
            }
#elif UNITY_2017_1_OR_NEWER
            if (GraphicsSettings.renderPipelineAsset != null) {
                // SRP not supported before 2019
                return PipelineType.None;
            }
#endif
            // no SRP
            return PipelineType.BuiltIn;

        }

        //Set the build defines
        static ScreenshotterPipelineType()
        {

            EditorApplication.update += SetPipelineType;


        }

        static void SetPipelineType()
        {
            PipelineType type = GetPipelineType();

            var targetPlatform = EditorUserBuildSettings.selectedBuildTargetGroup;

            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatform);
            bool isEmpty = false;
            if (string.IsNullOrEmpty(defines))
            {
                isEmpty = true;
            }
            switch (GetPipelineType())
            {
                case PipelineType.BuiltIn:
                    Debug.Log("Screenshotter Set to Built In");
                    if (!defines.Contains(DEFINE_BUILTIN))
                    {

                        defines += (isEmpty) ? DEFINE_BUILTIN : ";" + DEFINE_BUILTIN;
                    }
                    break;
                case PipelineType.HighDefinition:
                    Debug.Log("Screenshotter Set to High Definition");
                    if (!defines.Contains(DEFINE_HDRP))
                    {
                        defines += (isEmpty) ? DEFINE_HDRP : ";" + DEFINE_HDRP;
                    }
                    break;
                case PipelineType.Universal:
                    Debug.Log("Screenshotter Set to Universal");
                    if (!defines.Contains(DEFINE_URP))
                    {
                        defines += (isEmpty) ? DEFINE_URP : ";" + DEFINE_URP;
                    }
                    break;
                case PipelineType.None:
                    Debug.LogError("No Pipeline Type is available! Screenshotter not supported in this version of Unity!");
                    break;
            }



            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, defines);

            EditorApplication.update -= SetPipelineType;
        }

    }
}

