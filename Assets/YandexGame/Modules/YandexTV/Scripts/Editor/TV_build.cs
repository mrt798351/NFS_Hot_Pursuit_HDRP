using UnityEngine;
using System.IO;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuildManager
    {
        public static void TVModul()
        {
            string donorPatch = Application.dataPath + "/YandexGame/Modules/YandexTV/Scripts/Editor/TV_js.js";
            string donorText = File.ReadAllText(donorPatch);

            AddIndexCode(donorText, CodeType.js);
            AddIndexCode("TVInit();", CodeType.init);
        }
    }
}
