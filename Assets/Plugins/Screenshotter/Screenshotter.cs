using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_HDRP
using UnityEngine.Rendering.HighDefinition;
#elif UNITY_URP
using UnityEngine.Rendering.Universal;
#elif UNITY_BUILT_IN
using UnityEngine.Rendering.PostProcessing;
#endif
namespace SkatanicStudios
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(PlayerInput))]
    public class Screenshotter : MonoBehaviour
    {

        PlayerInput input;
        Camera camera;
#if UNITY_HDRP || UNITY_URP
        Volume volume;
        DepthOfField depthOfField;
#elif UNITY_BUILT_IN
    PostProcessLayer layer;
    PostProcessVolume volume;
    DepthOfField depthOfField;
#endif

        public bool invertLook;
        public Vector2Int screenShotResolution = new Vector2Int(1920, 1080);
        public Color debugTextColor = Color.yellow;
        public int debugFontSize = 30;


        float speed = 1f;
        float horizontal;
        float vertical;
        float height;
        float lookVertical;
        float lookHorizontal;
        float zoomIn;
        float zoomOut;
        bool showDebug = true;
        bool isDOFControl;
        float timeScale = 1;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            input = GetComponent<PlayerInput>();


            //Make this the camera in focus
            camera.depth = float.MaxValue;
#if UNITY_HDRP || UNITY_URP
            volume = gameObject.AddComponent<Volume>();
            volume.priority = 100f;
            volume.profile = new VolumeProfile();

            depthOfField = volume.profile.Add<DepthOfField>(true);
#if UNITY_HDRP
        //HDRP// depthOfField.focusMode.Override(DepthOfFieldMode.Manual);
#elif UNITY_URP
            depthOfField.mode.Override(DepthOfFieldMode.Bokeh);
#endif
#elif UNITY_BUILT_IN
        gameObject.layer = LayerMask.NameToLayer("PostProcessing");

        layer = gameObject.AddComponent<PostProcessLayer>();
        layer.volumeLayer.value = LayerMask.GetMask("PostProcessing");

        volume = gameObject.AddComponent<PostProcessVolume>();
        volume.priority = 100;
        volume.profile = new PostProcessProfile();
        volume.isGlobal = true;

        camera.allowHDR = true;
        
        depthOfField = volume.profile.AddSettings<DepthOfField>();
#endif
        }

        public void OnMove(InputValue value)
        {
            horizontal = value.Get<Vector2>().x;
            vertical = value.Get<Vector2>().y;
        }

        public void OnLook(InputValue value)
        {
            lookHorizontal = value.Get<Vector2>().x;
            lookVertical = value.Get<Vector2>().y;
        }

        public void OnZoomIn(InputValue value)
        {
            zoomIn = value.Get<float>();
        }

        public void OnZoomOut(InputValue value)
        {
            zoomOut = value.Get<float>();
        }

        public void OnCameraUp(InputValue value)
        {
            height = value.Get<float>() * speed;
        }

        public void OnCameraDown(InputValue value)
        {
            height = value.Get<float>() * -speed;
        }

        public void OnToggleDebug(InputValue value)
        {
            if (value.Get<float>() == 1)
            {
                showDebug = !showDebug;
            }
        }

        public void OnToggleControls(InputValue value)
        {
            if (value.Get<float>() == 1)
            {
                isDOFControl = !isDOFControl;
            }
        }

        public void OnToggleSpeed(InputValue value)
        {
            if (value.Get<float>() == 1)
            {
                if (speed == 0.1f)
                {
                    speed = 2;
                }
                else if (speed == 2)
                {
                    speed = 1;
                }
                else if (speed == 1)
                {
                    speed = 0.5f;
                }
                else if (speed == 0.5f)
                {
                    speed = 0.1f;
                }
            }
        }

        public void OnScreenshot(InputValue value)
        {
            if (value.Get<float>() == 1)
            {
                TakeScreenshot();
            }
        }



        public void OnPause(InputValue value)
        {
            if (value.Get<float>() == 1)
            {
                if (timeScale == 0)
                {
                    timeScale = 1;
                }
                else
                {
                    timeScale = 0;
                }

                Time.timeScale = timeScale;

            }
        }

        public void OnNext(InputValue value)
        {
            //TODO Profile Switching
        }

        public void OnPrevious(InputValue value)
        {
            //TODO Profile Switching
        }

#if UNITY_HDRP
    float nearStart;
    float nearEnd;
    float farStart;
    float farEnd;
#elif UNITY_URP || UNITY_BUILT_IN
        float focusDistance;
        float focalLength;
        float aperture;
#endif

        private void Update()
        {

#if UNITY_HDRP
        nearStart = depthOfField.nearFocusStart.GetValue<float>();
        nearEnd = depthOfField.nearFocusEnd.GetValue<float>();
        farStart = depthOfField.farFocusStart.GetValue<float>();
        farEnd = depthOfField.farFocusEnd.GetValue<float>();
#elif UNITY_URP || UNITY_BUILT_IN

            focusDistance = depthOfField.focusDistance.GetValue<float>();
            focalLength = depthOfField.focalLength.GetValue<float>();
            aperture = depthOfField.aperture.GetValue<float>();
#endif

            if (isDOFControl)
            {
                if (Mathf.Abs(vertical) < 0.25f)
                {
                    vertical = 0;
                }

                if (Mathf.Abs(horizontal) < 0.25f)
                {
                    horizontal = 0;
                }

                if (Mathf.Abs(lookHorizontal) < 0.25f)
                {
                    lookHorizontal = 0;
                }

                if (Mathf.Abs(lookVertical) < 0.25f)
                {
                    lookVertical = 0;
                }
#if UNITY_HDRP
            float nearFocusStart = vertical * speed;
            float nearFocusEnd = horizontal * speed;

            float farFocusStart = lookVertical * speed;
            float farFocusEnd = lookHorizontal * speed;

            nearStart += nearFocusStart;
            nearEnd += nearFocusEnd;
            farStart += farFocusStart;
            farEnd += farFocusEnd;
            
            if (nearStart > nearEnd)
            {
                nearEnd = nearStart;
            }
            if (farStart > farEnd)
            {
                farEnd = farStart;
            }


            if (farStart < 0)
            {
                farStart = 0;
            }
            if (nearStart < 0)
            {
                nearStart = 0;
            }
            if (farEnd < 0)
            {
                farEnd = 0;
            }
            if(nearEnd < 0)
            {
                nearEnd = 0;
            }

            depthOfField.nearFocusStart.Override(nearStart);
            depthOfField.nearFocusEnd.Override(nearEnd);
            depthOfField.farFocusStart.Override(farStart);
            depthOfField.farFocusEnd.Override(farEnd);
            
#elif UNITY_URP || UNITY_BUILT_IN
                focusDistance += vertical * speed;
                focalLength += horizontal * speed;
                aperture += lookVertical * speed;

                if (focalLength < 0)
                {
                    focalLength = 0;
                }
                if (focusDistance < 0)
                {
                    focusDistance = 0;
                }
                if (aperture < 0.1f)
                {
                    aperture = 0.1f;
                }


                depthOfField.focalLength.Override(focalLength);
                depthOfField.focusDistance.Override(focusDistance);
                depthOfField.aperture.Override(aperture);
#endif

            }
            else
            {


                transform.position = Vector3.Lerp(transform.position, transform.position + (transform.forward * vertical) + (transform.right * horizontal) + (Vector3.up * height), speed);

                transform.Rotate(Vector3.up, lookHorizontal * speed);

                if (invertLook)
                {
                    transform.Rotate(Vector3.right, lookVertical * speed);
                }
                else
                {
                    transform.Rotate(Vector3.right, -lookVertical * speed);
                }

                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            }

            if (camera.fieldOfView > 0)
            {
                camera.fieldOfView -= zoomIn;
            }
            if (camera.fieldOfView < 100)
            {
                camera.fieldOfView += zoomOut;
            }

        }

        public int screenshotCount;
        public void TakeScreenshot()
        {
#if UNITY_EDITOR
            string time = string.Format("{0}_{1}_{2}", System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
            string name = string.Format("{0}_{1}_shot_{2}", time, Application.productName, screenshotCount).ToLower();
            string fullpathname = EditorUtility.SaveFilePanel("Save Screenshot", "", name, "png");
            TakeNewScreenshot(fullpathname);
            screenshotCount++;
#endif
        }

        public void TakeNewScreenShot(string path, string filename)
        {
            string fname = string.Format("{0}/{1}.png", path, filename);
            TakeNewScreenshot(fname);
        }

        public void TakeNewScreenshot(string fullPath)
        {
            if (camera == null)
            {
                camera = GetComponent<Camera>();
            }

            RenderTexture rt = new RenderTexture(screenShotResolution.x, screenShotResolution.y, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(screenShotResolution.x, screenShotResolution.y, TextureFormat.RGBA32, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, screenShotResolution.x, screenShotResolution.y), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; //added to avoid errors

            if (Application.isEditor)
            {
                DestroyImmediate(rt);
            }
            else
            {
                Destroy(rt);
            }

            byte[] bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(fullPath, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", fullPath));
        }

        private void OnGUI()
        {
            if (showDebug)
            {
                string label = "";
#if UNITY_HDRP
            label = string.Format("SCREENSHOTTER DEBUG (Y) \nMode {0} (A) \nSpeed {1} (L3)\n\nDEPTH OF FIELD Near = LS, Far = RS)\nNear Start:{2}\nNear End: {3}\nFar Start {4}\nFar End {5}\n\nTime Scale:{6}", (isDOFControl)? "DoF" : "Look", speed, nearStart, nearEnd, farStart, farEnd, Time.timeScale);
#elif UNITY_URP || UNITY_BUILT_IN
                label = string.Format("SCREENSHOTTER DEBUG (Y) \nMode {0} (A) \nSpeed {1} (L3)\n\nDEPTH OF FIELD Focus Length/Distance = LS, Aperture = RS)\nFocal Distance: {3}\nFocal Narrowness:{2}\nApature {4}f\n\nTime Scale:{5}", (isDOFControl) ? "DoF" : "Look", speed, focalLength, focusDistance, aperture, Time.timeScale);
#endif
                GUI.skin.label.fontSize = debugFontSize;
                GUI.contentColor = debugTextColor;

                GUI.Label(new Rect(20, 20, Screen.width, Screen.height), label);
            }
        }

    }
}
