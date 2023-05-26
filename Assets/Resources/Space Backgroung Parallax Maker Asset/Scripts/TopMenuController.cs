using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mkey
{
    public enum Mode { IOS, STANDALONE, ANDROID, WEBGL }
    public class TopMenuController : MonoBehaviour
    {
        public Mode mode;
        public Toggle touch;
        public Toggle keyboard;
        public Toggle gyro;
        public Toggle mouse;
        public Button nextButton;

        void Awake()
        {
#if UNITY_STANDALONE_WIN
            mode = Mode.STANDALONE;
#elif UNITY_STANDALONE_OSX
        mode = Mode.STANDALONE;
#elif UNITY_STANDALONE_LINUX
        mode = Mode.STANDALONE;
#elif UNITY_IOS
    mode = Mode.IOS;
#elif UNITY_ANDROID
    mode = Mode.ANDROID;
#elif UNITY_WEBGL
    mode = Mode.WEBGL;
#endif
        }

        void Start()
        {
            HideUnusedToggle();
            touch.onValueChanged.AddListener((on) => { if (on) CameraFollow.Instance.track = TrackMode.Touch; });
            keyboard.onValueChanged.AddListener((on) => { if (on) CameraFollow.Instance.track = TrackMode.Keyboard; });
            gyro.onValueChanged.AddListener((on) => { if (on) CameraFollow.Instance.track = TrackMode.Gyroscope; });
            mouse.onValueChanged.AddListener((on) => { if (on) CameraFollow.Instance.track = TrackMode.Mouse; });
            nextButton.onClick.AddListener(NextButtonClick);
        }

        private void HideUnusedToggle()
        {
            switch (mode)
            {
                case Mode.IOS:
                    keyboard.gameObject.SetActive(false);
                    gyro.gameObject.SetActive(true);
                    mouse.gameObject.SetActive(false);
                    break;
                case Mode.STANDALONE:
                    keyboard.gameObject.SetActive(true);
                    gyro.gameObject.SetActive(false);
                    mouse.gameObject.SetActive(true);
                    break;
                case Mode.ANDROID:
                    keyboard.gameObject.SetActive(false);
                    gyro.gameObject.SetActive(true);
                    mouse.gameObject.SetActive(false);
                    break;
                case Mode.WEBGL:
                    gyro.gameObject.SetActive(false);
                    keyboard.gameObject.SetActive(!IsMobileDevice());
                    mouse.gameObject.SetActive(!IsMobileDevice());
                    break;
            }
            touch.gameObject.SetActive(true);
            touch.isOn = true;
        }

        private void NextButtonClick()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            int activeScene = SceneManager.GetActiveScene().buildIndex;
            int nextScene = (int)Mathf.Repeat(++activeScene, sceneCount);
            SceneManager.LoadScene(nextScene);
        }

        /// <summary>
        /// Return true touch pad run on mobile device
        /// </summary>
        public static bool IsMobileDevice()
        {
            //check if our current system info equals a desktop
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                //we are on a desktop device, so don't use touch
                return false;
            }
            //if it isn't a desktop, lets see if our device is a handheld device aka a mobile device
            else if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                //we are on a mobile device, so lets use touch input
                return true;
            }
            return false;
        }
    }
}