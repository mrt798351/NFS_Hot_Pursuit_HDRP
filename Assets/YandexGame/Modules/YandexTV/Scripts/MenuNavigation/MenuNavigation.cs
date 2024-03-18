using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

namespace YG.MenuNav
{
    [DefaultExecutionOrder(-99)]
    public class MenuNavigation : MonoBehaviour
    {
        [Tooltip("Укажите кнопку, на которую будет направлен фокус при старте сцены. (Необязательно)")]
        public Button usButton;
        [Tooltip("Укажите объект, который будет становиться активным, при нажатии на кнопку back. Используйте это, для открытия меню выхода из игры. Меню выхода откроется, если не открыты другие слои, в ином случае, последний открытый слой закроется.")]
        public GameObject exitGameObj;
        [Tooltip("Использовать ли навигацию на ПК. На ПК визуальное выделение кнопки будет только в момент наведения мыши на кнопку. Но, если на клавиатуре нажать кнопки навигации, то визуальное выделение останется.")]
        public bool navigationOnDesktop;
        [Tooltip("Использовать ли навигацию на мобильных устройствах. Визуального выделения не будет, но сохранятся звуки, которые вы возможно повесили на события нажатия кнопки.")]
        public bool navigationOnMobile;
        public UnityEvent OnBackButton;
        public List<LayerUI> layers = new List<LayerUI>();

        public static MenuNavigation Instance;
        public static Action<bool> onIsActiveNavigation;
        public static Action<Button> onSelectButton;
        public static Action<GameObject> onOpenWindow;
        public static Action onCloseWindow;
        public static Action onEnterButton;
        public static Action<Button> onAddButton;
        public static Action<Button> onRemoveButton;
        public static Action onUnselectButton;
            
        private void OnEnable()
        {
            YandexGame.onTVKeyDown += OnKeyDown;
            YandexGame.onTVKeyBack += OnKeyBack;
        }

        private void OnDisable()
        {
            YandexGame.onTVKeyDown -= OnKeyDown;
            YandexGame.onTVKeyBack -= OnKeyBack;
        }

        private void Start()
        {
            Instance = this;
            bool isActiveNavigation = true;

            if (!YandexGame.EnvironmentData.isTV)
            {
                if (navigationOnDesktop && YandexGame.EnvironmentData.isDesktop)
                    TVKeyController.Create();
                else if (navigationOnMobile && (YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet))
                    TVKeyController.Create();
                else
                    isActiveNavigation = false;
            }

            if (isActiveNavigation)
            {
                AddLayer(null);

                if (usButton == null)
                    usButton = FindFirstObjectByType<Button>();

                if (usButton != null)
                    SelectButton(usButton);
            }

            onIsActiveNavigation?.Invoke(isActiveNavigation);
        }

        private bool NavigationAllow()
        {
            if (YandexGame.EnvironmentData.isDesktop && !navigationOnDesktop)
                return false;
            else if ((YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet) && !navigationOnMobile)
                return false; 
            else 
                return true;
        }

        private void OnKeyDown(string key)
        {
            Button b;
            switch (key)
            {
                case "Up":
                    b = usButton.FindSelectableOnUp()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Left":
                    b = usButton.FindSelectableOnLeft()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Down":
                    b = usButton.FindSelectableOnDown()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Right":
                    b = usButton.FindSelectableOnRight()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Enter":
                    if (usButton)
                    {
                        usButton.onClick?.Invoke();
                        onEnterButton?.Invoke();
                    }
                    break;
            }
        }

        private void OnKeyBack()
        {
            if (layers.Count > 1)
            {
                CloseWindow();
            }
            else if (exitGameObj && YandexGame.EnvironmentData.isTV)
            {
                OpenWindow(exitGameObj);
            }
            else
            {
                OnBackButton.Invoke();
            }
        }

        public void SelectButton(Button button)
        {
            if (button == null || !NavigationAllow())
                return;

            usButton = button;
            onSelectButton?.Invoke(button);
        }

        public void UnselectButton(bool considerDeviceParams = false)
        {
            if (considerDeviceParams)
            {
                if (!YandexGame.EnvironmentData.isDesktop || (YandexGame.EnvironmentData.isDesktop && !navigationOnDesktop))
                    return;
            }

            onUnselectButton?.Invoke();
        }

        public void SelectFirstButton()
        {
            Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
            usButton = buttons.FirstOrDefault(button => button.enabled);
            SelectButton(usButton);
        }

        private void AddLayer(GameObject openLayerObj)
        {
            if (openLayerObj)
                openLayerObj.SetActive(true);

            LayerUI newLayer = new LayerUI
            {
                layer = openLayerObj,
                buttons = new List<Button>()
            };

            Button[] allButtonsInScene = FindObjectsByType<Button>(FindObjectsSortMode.None);

            foreach (Button button in allButtonsInScene)
            {
                if (button.enabled && button.navigation.mode != Navigation.Mode.None)
                {
                    newLayer.buttons.Add(button);
                }
            }

            layers.Add(newLayer);
        }

        public void OpenWindow(GameObject openLayerObj)
        {
            if (layers.Count > 0)
            {
                foreach (Button button in layers[layers.Count - 1].buttons)
                {
                    button.enabled = false;
                }

                AddLayer(openLayerObj);
                SelectFirstButton();

                onOpenWindow?.Invoke(openLayerObj);
            }
        }

        public void CloseWindow()
        {
            onCloseWindow?.Invoke();
            
            if (layers.Count <= 1)
                return;

            int usLayer = layers.Count - 1;
            layers[usLayer].layer.SetActive(false);

            usLayer--;

            for (int i = 0; i < layers[usLayer].buttons.Count; i++)
            {
                if (layers[usLayer].buttons[i])
                    layers[usLayer].buttons[i].enabled = true;
            }

            layers.RemoveAt(usLayer + 1);
            SelectFirstButton();
        }

        public void AddButtonList(Button button, int layer)
        {
            if (layer < layers.Count && button && !layers[layer].buttons.Contains(button))
                layers[layer].buttons.Add(button);
            onAddButton?.Invoke(button);
        }

        public void AddButtonList(Button button)
        {
            AddButtonList(button, 0);
        }

        public void RemoveButtonList(Button button, int layer)
        {
            onRemoveButton?.Invoke(button);
            if (layer < layers.Count && layers[layer].buttons.Contains(button))
                layers[layer].buttons.Remove(button);
        }

        public void RemoveButtonList(Button button)
        {
            RemoveButtonList(button, 0);
        }

        [Serializable]
        public struct LayerUI
        {
            public GameObject layer;
            public List<Button> buttons;
        }
    }
}