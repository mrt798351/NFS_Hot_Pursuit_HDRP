using UnityEngine;
using System.IO;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuildManager
    {
        public static void DivRBT()
        {
            if (infoYG.divAdaptiveBanners.banners.Length > 0)
            {
                string donorPatch = Application.dataPath + "/YandexGame/Modules/DivAdaptiveBanner/Scripts/Editor/DivRBT_Paint_js.js";
                string donorText = File.ReadAllText(donorPatch);

                AddIndexCode(donorText, CodeType.js);

                for (int i = 0; i < infoYG.divAdaptiveBanners.banners.Length; i++)
                {
                    Head();
                    Body();

                    void Head()
                    {
                        string donorPatch = Application.dataPath + "/YandexGame/Modules/DivAdaptiveBanner/Scripts/Editor/DivRBT_Head_js.html";
                        string donorText = File.ReadAllText(donorPatch);
                        donorText = donorText.Replace("__NameRTB__", infoYG.divAdaptiveBanners.banners[i].name);

                        AddIndexCode(donorText, CodeType.head);
                    }

                    void Body()
                    {
                        string donorPatch = Application.dataPath + "/YandexGame/Modules/DivAdaptiveBanner/Scripts/Editor/DivRBT_Body_js.html";
                        string donorText = File.ReadAllText(donorPatch);
                        donorText = donorText.Replace("__NameRTB__", infoYG.divAdaptiveBanners.banners[i].name);
                        donorText = donorText.Replace("__FunctionExecuteCode__", infoYG.divAdaptiveBanners.banners[i].executeCode);

                        AddIndexCode(donorText, CodeType.body);
                    }
                }
            }
        }
    }
}
