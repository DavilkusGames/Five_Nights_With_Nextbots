using System;
using System.IO;
using UnityEngine;

namespace Plugins.Audio.Editor
{
    public static class EditorAudioPluginUtils
    {
        public static void UpgradeAudioJS()
        {
            string filePath = UnityEditor.EditorApplication.applicationContentsPath + "/PlaybackEngines/WebGLSupport/BuildTools/lib/Audio.js";

            string searchText = "this.source.start(startTime, startOffset);";

            string jsCode = @"
                if(isNaN(startOffset)){
                    startOffset = 0;
                }
            ";
        
            try
            {
                string fileContent = File.ReadAllText(filePath);

                if (fileContent.Contains(jsCode))
                {
                    Debug.Log("File is upgradet");
                    return;
                }
            
                if (fileContent.Contains(searchText))
                {
                    int index = fileContent.IndexOf(searchText);

                    fileContent = fileContent.Insert(index - 1, jsCode);

                    File.WriteAllText(filePath, fileContent);


                    Debug.Log("Audio js upgrade completed.");
                }
                else
                {
                    Debug.Log("Audio js not upgrade.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Audio js upgrade error: " + e.Message);
            }
        }
    }
}