using UnityEngine;
using UnityEngine.UI;

namespace YG.MenuNav
{
    public class VisualSelectButtton : MonoBehaviour
    {
        public RectTransform visualSelector;
        public float sizeFrame = 10.0f;
        public bool visualOnDesktop = true, visualOnMobile;
            
        private GameObject visualSelObject;
        private bool active;

        private void Awake()
        {
            visualSelObject = visualSelector.gameObject;
            visualSelObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (!CheckDevice())
                return;

            MenuNavigation.onIsActiveNavigation += Setup;
            MenuNavigation.onSelectButton += OnSelectButton;
            MenuNavigation.onUnselectButton += HideVusualSlector;
        }

        private void OnDisable()
        {
            if (!CheckDevice())
                return;

            MenuNavigation.onIsActiveNavigation -= Setup;
            MenuNavigation.onSelectButton -= OnSelectButton;
            MenuNavigation.onUnselectButton -= HideVusualSlector;
        }

        private void Setup(bool active)
        {
            this.active = active;
        }
        
        private void OnSelectButton(Button button)
        {
            if (active || YandexGame.EnvironmentData.isTV)
            {
                visualSelObject.SetActive(true);
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                CopyTransform(visualSelector, buttonRect);
                IncreaseSize(visualSelector);
            }
        }

        private void CopyTransform(RectTransform target, RectTransform source)
        {
            target.SetParent(source);
            target.anchorMin = new Vector2(0, 0);
            target.anchorMax = new Vector2(1, 1);
            target.position = Vector3.zero;
            target.sizeDelta = Vector2.zero;
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
            target.localScale = Vector2.one;
        }

        private void IncreaseSize(RectTransform target)
        {
            Vector2 newSize = target.sizeDelta;
            newSize.x += sizeFrame;
            newSize.y += sizeFrame;
            target.sizeDelta = newSize;
        }

        public void HideVusualSlector()
        {
            visualSelObject.SetActive(false);
            visualSelector.SetParent(null);
        }

        private bool CheckDevice()
        {
            if (!visualOnDesktop && YandexGame.EnvironmentData.isDesktop)
                return false;
            else if (!visualOnMobile && (YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet))
                return false;
            return true;
        }
    }
}