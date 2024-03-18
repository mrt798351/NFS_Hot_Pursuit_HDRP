using UnityEngine;
using System.IO;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuildManager
    {
        public static void YandexMusic()
        {
            if (infoYG.yandexMusic.nameBanner != null && infoYG.yandexMusic.nameBanner != "")
            {
                string donorPatch = Application.dataPath + "/YandexGame/Modules/YandexMusic/Scripts/Editor/YandexMusic_js.js";
                string donorText = File.ReadAllText(donorPatch);
                donorText = donorText.Replace("__NameRTB__", infoYG.yandexMusic.nameBanner);
                donorText = donorText.Replace("__URL__", infoYG.yandexMusic.copyLink);

                AddIndexCode(donorText, CodeType.body);

                string word = "canvas.focus();";
                string insertText = $"\ndocument.getElementById('{infoYG.yandexMusic.nameBanner}').focus();";

                int index = indexFile.IndexOf(word);

                if (index != -1)
                    indexFile = indexFile.Insert(index + word.Length, insertText);
            }
        }
    }
}
