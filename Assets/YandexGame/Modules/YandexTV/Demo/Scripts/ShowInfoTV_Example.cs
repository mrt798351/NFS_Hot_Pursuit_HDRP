using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class ShowInfoTV_Example : MonoBehaviour
    {
        public Text keyDownText, keyUpText;

        // Пример. Подписываемся на ивенты ввода кнопок для телевизора
        private void OnEnable()
        {
            YandexGame.onTVKeyDown += OnTVKeyDown; // Ивент нажатия на кнопку
            YandexGame.onTVKeyUp += OnTVKeyUp; // Ивент отжатия кнопки
            YandexGame.onTVKeyBack += OnTVKeyBack;  // Ивент кнопки "назад" (Back)
        }

        // Пример. Отписываемся от ивентов ввода кнопок для телевизора
        private void OnDisable()
        {
            YandexGame.onTVKeyDown -= OnTVKeyDown;
            YandexGame.onTVKeyUp -= OnTVKeyUp;
            YandexGame.onTVKeyBack -= OnTVKeyBack;
        }

        // Пример. Метод закрытия игры на телевизре
        public void ExitTVGame() // Дублирующий метод
        {
            YandexGame.ExitTVGame(); // Метод из YandexGame
        }
        
        private void OnTVKeyDown(string value)
        {
            keyDownText.text = KeyCounterCalculate(keyDownText.text, value);
        }

        private void OnTVKeyUp(string value)
        {
            keyUpText.text = KeyCounterCalculate(keyUpText.text, value);
        }

        private void OnTVKeyBack()
        {
            string value = "Back";
            keyDownText.text = KeyCounterCalculate(keyDownText.text, value);
            keyUpText.text = KeyCounterCalculate(keyUpText.text, value);
        }
        
        private string KeyCounterCalculate(string oldText, string newText)
        {
            if (oldText == null || oldText == "")
            {
                return newText;
            }

            string[] oldSplit = oldText.Split();

            if (newText == oldSplit[0])
            {
                if (oldSplit.Length == 1)
                { 
                    return newText + " +1";
                }
                else
                {
                    int indexOfPlus = oldText.IndexOf('+');
                    int number = int.Parse(oldText.Substring(indexOfPlus + 1));
                    
                    return newText + " +" + (number + 1).ToString();
                }
            }
            else
            {
                return newText;
            }
        }
    }
}