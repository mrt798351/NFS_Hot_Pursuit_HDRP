using UnityEngine;
using UnityEngine.UI;

namespace YG.MenuNav
{
    public class ButtonDynamicSelect : MonoBehaviour
    {
        [Tooltip("Вы можете заполнить поле. Если ButtonCash будет пустым, то при старте компонент Button будет найден с помощью метода GetComponent.")]
        public Button buttonCash;

        public virtual void Start()
        {
            if (!buttonCash)
                buttonCash.GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (MenuNavigation.Instance) 
            {
                int layer = MenuNavigation.Instance.layers.Count - 1;
                if (layer < 0) layer = 0;
                MenuNavigation.Instance.AddButtonList(buttonCash, layer);

            }
        }

        public virtual void OnDisable()
        {
            if (MenuNavigation.Instance)
            {
                int layer = MenuNavigation.Instance.layers.Count - 1;
                if (layer < 0) layer = 0;
                MenuNavigation.Instance.RemoveButtonList(buttonCash, layer);
            }
        }
    }
}