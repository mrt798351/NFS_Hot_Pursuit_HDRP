using System;

namespace YG
{
    public partial class InfoYG
    {
        [Serializable]
        public struct DivRBT
        {
            public DivRBTExecuteCode[] banners;
        }

        [Serializable]
        public struct DivRBTExecuteCode
        {
            public string name;
            public string executeCode;
        }

        public DivRBT divAdaptiveBanners;
    }
}
