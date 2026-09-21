using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class MyBuildPostprocessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        string path = pathToBuiltProject.Replace(".exe", "");

        path += "_Data/ScenesSettings";
        Directory.CreateDirectory(path);

        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/ScenesSettings/");

        for (int i = 0; i < dir.GetFiles().Length; i++)
        {
            Debug.Log(dir.GetFiles()[i].Name);

            File.Copy(Application.dataPath + "/ScenesSettings/" + dir.GetFiles()[i].Name, path + "/" + dir.GetFiles()[i].Name);
        }
    }
}