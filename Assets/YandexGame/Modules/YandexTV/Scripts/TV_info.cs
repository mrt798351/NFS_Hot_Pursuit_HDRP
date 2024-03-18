using System;
using UnityEngine;

namespace YG
{
    public partial class InfoYG
    {
        [Serializable]
        public class TVInfo
        {
            [Tooltip("Параметр работает только в Unity Editor. Поставьте галочку, чтобы эмулировать запуск игры на телевизоре.")]
            public bool TVTestInEditor;
        }
        public TVInfo TVSettings;
    }
}
