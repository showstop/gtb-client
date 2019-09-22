using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace AudienceNetwork
{
    public class AdHandler : MonoBehaviour
    {
        private readonly static Queue<Action> executeOnMainThreadQueue = new Queue<Action>();

        public void executeOnMainThread (Action action)
        {
            executeOnMainThreadQueue.Enqueue(action);
        }

        void Update () {
            // dispatch stuff on main thread
            while (executeOnMainThreadQueue.Count > 0)
            {
                executeOnMainThreadQueue.Dequeue().Invoke();
            }
        }

        public void removeFromParent () {
            #if UNITY_EDITOR
//          UnityEngine.Object.DestroyImmediate (this);
            #else
            UnityEngine.Object.Destroy (this);
            #endif
        }
    }

    public delegate void FBNativeAdHandlerValidationCallback(bool success);

    [RequireComponent (typeof (RectTransform))]
    public class NativeAdHandler : AdHandler
    {
        public int minViewabilityPercentage;
        public float minAlpha;
        public int maxRotation;
        public int checkViewabilityInterval;
        #pragma warning disable 109
        public new Camera camera;
        #pragma warning restore 109

        public FBNativeAdHandlerValidationCallback validationCallback;

        private float lastImpressionCheckTime;
        private bool impressionLogged;
        private bool shouldCheckImpression;

        public void startImpressionValidation ()
        {
            if (!this.enabled) {
                this.enabled = true;
            }
            this.shouldCheckImpression = true;
        }

        public void stopImpressionValidation ()
        {
            this.shouldCheckImpression = false;
        }

        void OnGUI ()
        {
            this.checkImpression ();
        }

        private bool checkImpression ()
        {
            float currentTime = Time.time;
            float secondsSinceLastCheck = currentTime - this.lastImpressionCheckTime;

            if (this.shouldCheckImpression && !this.impressionLogged && (secondsSinceLastCheck > checkViewabilityInterval)) {
                this.lastImpressionCheckTime = currentTime;

                GameObject currentObject = this.gameObject;
                Camera camera = this.camera;
                if (camera == null) {
                    camera = this.GetComponent<Camera>();
                }
                if (camera == null) {
                    camera = Camera.main;
                }

                while (currentObject != null) {
                    Canvas canvas = currentObject.GetComponent<Canvas>();
                    if (canvas != null) {
                        // Break if the current object is a nested world canvas
                        if (canvas.renderMode == RenderMode.WorldSpace) {
                            break;
                        }
                    }

                    bool currentObjectViewable = this.checkGameObjectViewability (camera, currentObject);
                    if (!currentObjectViewable) {
                        if (this.validationCallback != null) {
                            this.validationCallback(false);
                        }
                        return false;
                    }
                    currentObject = null;
                };

                if (this.validationCallback != null) {
                    this.validationCallback(true);
                }
                this.impressionLogged = true;
            }
            return this.impressionLogged;
        }

        private bool logViewability (bool success, string message)
        {
            if (!success) {
                Debug.Log ("Viewability validation failed: " + message);
            } else {
                Debug.Log ("Viewability validation success! " + message);
            }
            return success;
        }

        private bool checkGameObjectViewability (Camera camera, GameObject gameObject)
        {
            if (gameObject == null) {
                return this.logViewability (false, "GameObject is null.");
            }

            if (camera == null) {
                return this.logViewability (false, "Camera is null.");
            }

            if (!gameObject.activeInHierarchy) {
                return this.logViewability (false, "GameObject is not active in hierarchy.");
            }

            // Cull items that do not pass the alpha test
            CanvasGroup[] canvasGroups = gameObject.GetComponents<CanvasGroup>();
            foreach (CanvasGroup group in canvasGroups) {
                if (group.alpha < this.minAlpha) {
                    return this.logViewability(false, "GameObject has a CanvasGroup with less than the minimum alpha required.");
                }
            }

            RectTransform transform = gameObject.transform as RectTransform;
            Vector3 position = transform.position;

            // Get object's width/height from the rect transform
            float width = transform.rect.width;
            float height = transform.rect.height;

            // TODO: t15811089 - non-centered anchor points
            // TODO: t15811196 - apply rotation to corner points

            // Compute the screenSpace size by taking the 2 corners composing the rect in world space and projecting them in the camera space.
            Vector3 bottomLeft = position;
            bottomLeft.x -= width / 2.0f;
            bottomLeft.y -= height / 2.0f;

            Vector3 topRight = position;
            topRight.x += width / 2.0f;
            topRight.y += height / 2.0f;

            // Projected values
            Vector3 projectedBottomLeft = camera.WorldToScreenPoint(bottomLeft);
            Vector3 projectedTopRight = camera.WorldToScreenPoint(topRight);

            // Get the projected size
            float projectedWidth = projectedTopRight.x - projectedBottomLeft.x;
            float projectedHeight = projectedTopRight.y - projectedBottomLeft.y;

            // Get the camera size
            Rect pixelRect = camera.pixelRect;
            Rect screenSize = new Rect(pixelRect.x * Screen.dpi, pixelRect.y * Screen.dpi, pixelRect.width * Screen.dpi, pixelRect.height * Screen.dpi);

            // Check if the width / height are valid
            if (projectedWidth <= 0 && projectedHeight <= 0) {
                return this.logViewability (false, "GameObject's height/width is less than or equal to zero.");
            }

            // Check that the ad is in the camera rect
            if (!CheckScreenPosition (projectedBottomLeft, projectedTopRight, screenSize)) {
                return this.logViewability (false, "Not enough of the GameObject is inside the viewport.");
            }

            // Check that the item is not too small
            if (projectedWidth / width < this.minViewabilityPercentage || projectedHeight / height < this.minViewabilityPercentage) {
                return this.logViewability (false, "The GameObject is too small to count as an impression.");
            }

            // Check that item is not rotated too much
            Vector3 rotation = transform.eulerAngles;
            int xRotation = Mathf.FloorToInt (rotation.x);
            int yRotation = Mathf.FloorToInt (rotation.y);
            int zRotation = Mathf.FloorToInt (rotation.z);

            int minRotation = 360 - this.maxRotation;
            int maxRotation = this.maxRotation;

            if (!(xRotation >= minRotation || xRotation <= maxRotation)) {
                return this.logViewability (false, "GameObject is rotated too much. (x axis)");
            } else if (!(yRotation >= minRotation || yRotation <= maxRotation)) {
                return this.logViewability (false, "GameObject is rotated too much. (y axis)");
            } else if (!(zRotation >= minRotation || zRotation <= maxRotation)) {
                return this.logViewability (false, "GameObject is rotated too much. (z axis)");
            }

            return this.logViewability (true, "--------------- VALID IMPRESSION REGISTERED! ----------------------");
        }

        private bool CheckScreenPosition(Vector3 lowerLeft, Vector3 upperRight, Rect screen)
        {
            float exceedingWidth = 0.0f;
            float exceedingHeight = 0.0f;

            // Check that ad width does not exceed screen width
            if (lowerLeft.x < screen.xMin) {
                exceedingWidth += Mathf.Abs(lowerLeft.x - screen.xMin);
            }

            if (upperRight.x > screen.xMax) {
                exceedingWidth += Mathf.Abs(upperRight.x - screen.xMax);
            }

            float widthViewablePercentage = 1.0f - exceedingWidth / (upperRight.x - lowerLeft.x);
            if (widthViewablePercentage < this.minViewabilityPercentage) {
                return false;
            }

            // Check that ad height does not exceed screen height
            if (lowerLeft.y < screen.yMin) {
                exceedingHeight += Mathf.Abs(lowerLeft.y - screen.yMin);
            }

            if (upperRight.y > screen.yMax) {
                exceedingHeight += Mathf.Abs(upperRight.y - screen.yMax);
            }

            float heightViewablePercentage = 1.0f - exceedingHeight / (upperRight.y - lowerLeft.y);
            if (heightViewablePercentage < this.minViewabilityPercentage) {
                return false;
            }

            return true;
        }
    }
}
