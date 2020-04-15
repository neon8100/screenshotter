using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

namespace SkatanicStudios
{
    public class ScreenshotterMenu
    {
        static Camera camera;

        [MenuItem("GameObject/Screenshotter Camera", false, 10)]
        public static void CreateCamera()
        {
            GameObject screenshotterCamera = new GameObject("Screenshotter Camera", typeof(Screenshotter));
            string[] path = AssetDatabase.FindAssets("ScreenshotterActions", null);

            camera = screenshotterCamera.GetComponent<Camera>();

            PlayerInput input = screenshotterCamera.GetComponent<PlayerInput>();
            input.actions = (InputActionAsset)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(path[0]), typeof(InputActionAsset));
            input.defaultControlScheme = "Gamepad";
            input.neverAutoSwitchControlSchemes = true;
            input.camera = camera;

#if UNITY_URP
            //We need to wait for the object to be added so we can enable post processing on this camera automatically
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate()
        {

            if (camera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>() != null)
            {
                EditorApplication.update -= OnUpdate;

                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData universalCameraData = camera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
                universalCameraData.renderPostProcessing = true;

            }
#endif
        }
    }
}
