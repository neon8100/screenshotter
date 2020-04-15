using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

namespace SkatanicStudios
{
    public class ScreenshotterMenu
    {
        [MenuItem("GameObject/Screenshotter Camera", false, 10)]
        public static void CreateCamera()
        {
            GameObject screenshotterCamera = new GameObject("Screenshotter Camera", typeof(Screenshotter));
            string[] path = AssetDatabase.FindAssets("ScreenshotterActions", null);

            Camera camera = screenshotterCamera.GetComponent<Camera>(); 

            PlayerInput input = screenshotterCamera.GetComponent<PlayerInput>();
            input.actions = (InputActionAsset)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(path[0]), typeof(InputActionAsset));
            input.defaultControlScheme = "Gamepad";
            input.neverAutoSwitchControlSchemes = true;
            input.camera = camera;

#if UNITY_URP
            UniversalAdditionalCameraData universalCameraData = camera.GetComponent<UniversalAdditionalCameraData>();
            universalCameraData.renderPostProcessing = true;
#endif

        }
       
    }
}
