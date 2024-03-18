using UnityEngine;

namespace YG
{
    public class TVKeyController : MonoBehaviour
    {
        public static TVKeyController Instance;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (YandexGame.EnvironmentData.isMobile)
                {
                    Destroy(gameObject);
                    return;
                }

                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public static void Create()
        {
            GameObject tvTestObj = new GameObject { name = "TV Testing" };
            tvTestObj.AddComponent<TVKeyController>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                YandexGame.onTVKeyDown?.Invoke("Up");
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                YandexGame.onTVKeyUp?.Invoke("Up");
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                YandexGame.onTVKeyDown?.Invoke("Left");
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                YandexGame.onTVKeyUp?.Invoke("Left");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                YandexGame.onTVKeyDown?.Invoke("Down");
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                YandexGame.onTVKeyUp?.Invoke("Down");
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                YandexGame.onTVKeyDown?.Invoke("Right");
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                YandexGame.onTVKeyUp?.Invoke("Right");
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                YandexGame.onTVKeyDown?.Invoke("Enter");
            }
            if (Input.GetKeyUp(KeyCode.Return))
            {
                YandexGame.onTVKeyUp?.Invoke("Enter");
            }

            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                YandexGame.onTVKeyBack?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                YandexGame.onTVKeyDown?.Invoke("MediaRewind");
            }
            if (Input.GetKeyUp(KeyCode.F6))
            {
                YandexGame.onTVKeyUp?.Invoke("MediaRewind");
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                YandexGame.onTVKeyDown?.Invoke("MediaPlayPause");
            }
            if (Input.GetKeyUp(KeyCode.F7))
            {
                YandexGame.onTVKeyUp?.Invoke("MediaPlayPause");
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                YandexGame.onTVKeyDown?.Invoke("MediaFastForward");
            }
            if (Input.GetKeyUp(KeyCode.F8))
            {
                YandexGame.onTVKeyUp?.Invoke("MediaFastForward");
            }
        }
    }
}