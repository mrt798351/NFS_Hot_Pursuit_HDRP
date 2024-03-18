using UnityEngine;

namespace YG.Example
{
    public class HideIfnotTV : MonoBehaviour
    {
        private void OnEnable() => YandexGame.GetDataEvent += GetData;
        private void OnDisable() => YandexGame.GetDataEvent -= GetData;

        private void Awake()
        {
            if (YandexGame.SDKEnabled)
                GetData();
        }

        private void GetData()
        {
            if (!YandexGame.EnvironmentData.isTV)
                gameObject.SetActive(false);
        }
    }
}