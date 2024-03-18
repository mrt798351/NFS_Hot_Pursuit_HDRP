using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YG
{
    public partial class YandexGame
    {
        public static Action<string> onTVKeyDown, onTVKeyUp;
        public static Action onTVKeyBack;

        public void TVKeyDown(string key)
        {
            if (key != null || key != "")
                onTVKeyDown?.Invoke(key);
        }

        public void TVKeyUp(string key)
        {
            if (key != null || key != "")
                onTVKeyUp?.Invoke(key);
        }

        public void TVKeyBack()
        {
            onTVKeyBack?.Invoke();
        }

        [DllImport("__Internal")]
        private static extern void ExitTVGame_js();

        public static void ExitTVGame()
        {
#if !UNITY_EDITOR
            ExitTVGame_js();
#else 
            Message("Exit TV Game");
#endif
        }

        public void _ExitTVGame() => ExitTVGame();

        [InitYG]
        public static void CreateTVTestObj()
        {
#if UNITY_EDITOR
            if (Instance.infoYG.TVSettings.TVTestInEditor == false)
                return;
#else
            UnityEngine.Debug.Log("Init TV inGame");
            if (EnvironmentData.payload != "tvtest")
                return;
#endif
            EnvironmentData.deviceType = "tv";
            EnvironmentData.isTV = true;
            EnvironmentData.isMobile = false;
            EnvironmentData.isTablet = false;
            EnvironmentData.isDesktop = false;

            TVKeyController.Create();
        }
    }
}