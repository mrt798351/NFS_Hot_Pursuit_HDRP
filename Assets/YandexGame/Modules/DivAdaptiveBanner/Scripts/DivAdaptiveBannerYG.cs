using UnityEngine;
using UnityEngine.UI;
using System;

namespace YG.DivRBT
{
    [HelpURL("https://www.notion.so/PluginYG-d457b23eee604b7aa6076116aab647ed#e53839c6ec8842ef98cedcc2344cbd85")]
    public class DivAdaptiveBannerYG : MonoBehaviour
    {
        [Tooltip("имя баннера, оно должно соответствовать тому, что вы указывали в InfoYG")]
        public string nameBanner;
        [Tooltip("Минимальный размер блока. Баннер не будет меньше установленного значения.\n X - минимальная ширина.\n Y - минимальная высота.")]
        public Vector2 minSize = new Vector2(20, 20);
        public enum Device { desktopAndMobile, onlyDesktop, onlyMobile };
        [Tooltip("Desktop And Mobile - Отображение баннера на всех устройствах.\n Only Desktop - Отображение баннера только на компьютере.\n Only Mobile - Отображение баннера только на мобильных устройствах (телефонах и планшетах).")]
        public Device device;

        [HideInInspector]
        public bool focus;

        private RectTransform renderBlock;
        private CanvasScaler scaler;

#if !UNITY_EDITOR
        [Obsolete]
        private void OnApplicationFocus(bool hasFocus)
        {
            focus = hasFocus;
            if (YandexGame.SDKEnabled)
            {
                ActivateRTB();
            }
        }

        private void OnApplicationPause(bool isPaused) => focus = !isPaused;

        private void Awake()
        {
            renderBlock = (RectTransform)transform.GetChild(0);
            renderBlock.GetComponent<RawImage>().color = Color.clear;
            renderBlock.pivot = new Vector2(0, 1);
        }

        [Obsolete]
        private void OnEnable()
        {
            YandexGame.GetDataEvent += ActivateRTB;
            YandexGame.OpenFullAdEvent += DeactivateRTB;
            YandexGame.CloseFullAdEvent += ActivateRTB;
            YandexGame.OpenVideoEvent += DeactivateRTB;
            YandexGame.CloseVideoEvent += ActivateRTB;

            DebuggingModeYG.onRBTActivity += ActivityRTB;
            DebuggingModeYG.onRBTRecalculate += RecalculateRect;
            DebuggingModeYG.onRBTExecuteCode += ExecuteCode;

            focus = true;

            if (YandexGame.SDKEnabled)
                ActivateRTB();
        }

        [Obsolete]
        private void OnDisable()
        {
            YandexGame.GetDataEvent -= ActivateRTB;
            YandexGame.OpenFullAdEvent -= DeactivateRTB;
            YandexGame.CloseFullAdEvent -= ActivateRTB;
            YandexGame.OpenVideoEvent -= DeactivateRTB;
            YandexGame.CloseVideoEvent -= ActivateRTB;

            DebuggingModeYG.onRBTActivity -= ActivityRTB;
            DebuggingModeYG.onRBTRecalculate -= RecalculateRect;
            DebuggingModeYG.onRBTExecuteCode -= ExecuteCode;

            DeactivateRTB();
        }

        [Obsolete]
        private void OnRectTransformDimensionsChange()
        {
            if (CheckDevice())
            {
                RecalculateRect();
                CancelInvoke("RecalculateRect");
                Invoke("RecalculateRect", 0.3f);
            }
        }
#endif

        [Obsolete]
        public void RecalculateRect()
        {
            if (CheckDevice())
            {
                if (!renderBlock)
                    renderBlock = transform.GetChild(0).GetComponent<RectTransform>();

                if (!scaler)
                    scaler = GetComponent<CanvasScaler>();

                float width = renderBlock.rect.width;
                float height = renderBlock.rect.height;

                float left = renderBlock.localPosition.x;
                float top = -renderBlock.localPosition.y;

                if (scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    Vector2 multResolution = new Vector2(Screen.width / scaler.referenceResolution.x, Screen.height / scaler.referenceResolution.y);

                    if (scaler.matchWidthOrHeight == 0)
                    {
                        width *= multResolution.x;
                        height *= multResolution.x;
                        left *= multResolution.x;
                        top *= multResolution.x;
                    }
                    else if (scaler.matchWidthOrHeight == 1)
                    {
                        width *= multResolution.y;
                        height *= multResolution.y;
                        left *= multResolution.y;
                        top *= multResolution.y;
                    }
                }

                if (width < minSize.x) width = minSize.x;
                if (height < minSize.y) height = minSize.y;

                width = 100 * width / Screen.width;
                height = 100 * height / Screen.height;
                left = 100 * (Screen.width / 2 + left) / Screen.width;
                top = 100 * (Screen.height / 2 + top) / Screen.height;

                left = Mathf.Clamp(left, 0, 100);
                top = Mathf.Clamp(top, 0, 100);

                string _width = width.ToString().Replace(",", ".") + "%";
                string _height = height.ToString().Replace(",", ".") + "%";
                string _left = left.ToString().Replace(",", ".") + "%";
                string _top = top.ToString().Replace(",", ".") + "%";

                RecalculateRBT(_width, _height, _left, _top);
            }
        }

        private bool PaintBlock()
        {
            if (YandexGame.EnvironmentData.payload != "" &&
                YandexGame.EnvironmentData.payload != null &&
                YandexGame.EnvironmentData.payload == DebuggingModeYG.Instance.payloadPassword)
            {
                return true;
            }
            else return false;
        }

        [Obsolete]
        private void ActivateRTB()
        {
            if (NoAds()) 
                ActivityRTB(true);
        }

        [Obsolete]
        private void ActivateRTB(int id) => ActivateRTB();

        [Obsolete]
        private void DeactivateRTB() => ActivityRTB(false);

        [Obsolete]
        private void DeactivateRTB(int id) => DeactivateRTB();


        [Obsolete]
        public void ActivityRTB(bool state)
        {
            if (CheckDevice())
            {
                if (state)
                    RecalculateRect();

                string jsCode = $"{nameBanner}Activity('{state}');";
                Application.ExternalEval(jsCode);

                if (PaintBlock())
                    PaintRBT();
            }
        }

        [Obsolete]
        public void RecalculateRBT(string width, string height, string left, string top)
        {
            string jsCode = $"{nameBanner}Recalculate('{width}', '{height}', '{left}', '{top}');";
            Application.ExternalEval(jsCode);
        }

        [Obsolete]
        public void ExecuteCode()
        {
            string jsCode = $"{nameBanner}ExecuteCode();";
            Application.ExternalEval(jsCode);
        }

        [Obsolete]
        private void PaintRBT()
        {
            renderBlock.GetComponent<RawImage>().color = Color.blue;
            string jsCode = $"PaintRBT('{nameBanner}');";
            Application.ExternalEval(jsCode);
        }

        static bool? allowDevice;
        bool CheckDevice()
        {
            if (allowDevice == null)
            {
                bool result = true;

                if (device == Device.onlyDesktop)
                {
                    if (YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet)
                        result = false;
                }
                else if (device == Device.onlyMobile)
                {
                    if (!YandexGame.EnvironmentData.isMobile && !YandexGame.EnvironmentData.isTablet)
                        result = false;
                }

                allowDevice = result;
                return result;
            }
            else
            {
                return allowDevice.Value;
            }
        }

        bool NoAds()
        {
            if (!YandexGame.nowFullAd && !YandexGame.nowVideoAd)
                return true;
            else return false;
        }
    }
}
