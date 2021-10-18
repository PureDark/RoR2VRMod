﻿using RoR2;
using RoR2.UI;
using UnityEngine;

namespace VRMod
{
    internal class SmoothHUD : MonoBehaviour
    {
        internal static SmoothHUD instance;

        private Quaternion smoothHUDRotation;

        private CameraRigController cameraRig;

        internal void Init(CameraRigController cameraRig)
        {
            instance = this;
            this.cameraRig = cameraRig;

            smoothHUDRotation = cameraRig.uiCam.transform.rotation;
        }

        private void OnDisable()
        {
            if (cameraRig && cameraRig.uiCam)
            {
                smoothHUDRotation = cameraRig.uiCam.transform.rotation;

                TransformRect();
            }
        }

        private void LateUpdate()
        {
            //This slerp code block was taken from idbrii in Unity answers and from an article by Rory
            float delta = Quaternion.Angle(smoothHUDRotation, cameraRig.uiCam.transform.rotation);
            if (delta > 0f)
            {
                float t = Mathf.Lerp(delta, 0f, 1f - Mathf.Pow(0.03f, Time.unscaledDeltaTime));
                t = 1.0f - (t / delta);
                smoothHUDRotation = Quaternion.Slerp(smoothHUDRotation, cameraRig.uiCam.transform.rotation, t);
            }

            TransformRect();

            if (!ModConfig.InitialMotionControlsValue)
            {
                CrosshairManager crosshairManager = cameraRig.hud.GetComponent<CrosshairManager>();

                if (crosshairManager)
                {
                    crosshairManager.container.position = cameraRig.uiCam.transform.position + (cameraRig.uiCam.transform.forward * 12.35f);
                    crosshairManager.container.rotation = cameraRig.uiCam.transform.rotation;
                    crosshairManager.hitmarker.transform.position = crosshairManager.container.position;
                    crosshairManager.hitmarker.transform.rotation = cameraRig.uiCam.transform.rotation;
                }
            }
        }

        private void TransformRect()
        {
            transform.rotation = smoothHUDRotation;
            transform.rotation = Quaternion.LookRotation(transform.forward, cameraRig.uiCam.transform.up);
            transform.position = cameraRig.uiCam.transform.position + (transform.forward * 12.35f);
        }
    }
}
