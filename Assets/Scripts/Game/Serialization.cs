/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TK.Util
{
    public class Serialization
    {
        public static void Save(object saveObj, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            bf.Serialize(stream, saveObj);
            stream.Close();
            Debug.Log($"<color=#00FFFF>File {path} = Saved</color>");
        }

        public static object Load(string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            object result = bf.Deserialize(stream);
            stream.Close();
            Debug.Log($"<color=#00FFFF>File {path} = Loaded</color>");

            return result;
        }

        public static bool FileExists(string path)
        {
            bool fileExists = File.Exists(path);
            if (!fileExists) Debug.LogError($"FILE \"{path}\" DOES NOT EXIST");
            return fileExists;
        }

        public static bool DirectoryExists(string path)
        {
            bool exist = Directory.Exists(path);
            if(!exist) Debug.LogError($"DIRECTORY \"{path}\" DOES NOT EXIST");
            return exist;
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }

        public static void SaveText(string text, string path)
        {
            File.WriteAllText(path, text);
            Debug.Log($"<color=#00FFFF>File {path} = Saved</color>");
        }

        public static string LoadText(string path)
        {
            var content = File.ReadAllText(path);
            Debug.Log($"<color=#00FFFF>File {path} = Loaded</color>");
            return content;
        }

        public static List<string> LoadTextByLine(string path)
        {
            var content = new List<string>();

            foreach (var line in File.ReadAllLines(path))
            {
                content.Add(line);
            }

            Debug.Log($"<color=#00FFFF>File {path} = Loaded</color>");
            return content;
        }

        public static void SaveToJson(object obj, string path)
        {
            string jsonData = JsonUtility.ToJson(obj, true);
            SaveText(jsonData, path);
        }
    }
}