using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SaveSystem {

    private const string SAVE_EXTENSION = "json";

    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    private static readonly string LOG_FOLDER = Application.dataPath + "/Logs/";
    private static bool isInit = false;

    public static void Init() {
        if (!isInit) {
            isInit = true;
            // Test if Save Folder exists
            if (!Directory.Exists(SAVE_FOLDER)) {
                // Create Save Folder
                Directory.CreateDirectory(SAVE_FOLDER);
            }
            // Test if Log Folder exists
            if (!Directory.Exists(LOG_FOLDER))
            {
                // Create Log Folder
                Directory.CreateDirectory(LOG_FOLDER);
            }
        }
    }

    public static bool FileExists(string fileName)
    {
        string filePath = SAVE_FOLDER + fileName + "." + SAVE_EXTENSION;

        if (!File.Exists(filePath))
        {
            return false;
        }

        // Comprobar que el nombre del archivo sea exactamente igual a fileName
        string[] files = Directory.GetFiles(SAVE_FOLDER);
        foreach (string file in files)
        {
            if (file == filePath)
            {
                return true;
            }
        }

        return false;
    }

    public static void Save(string fileName, string saveString, bool overwrite) {
        Init();
        string saveFileName = fileName;
        if (!overwrite) {
            // Make sure the Save Number is unique so it doesnt overwrite a previous save file
            int saveNumber = 1;
            while (File.Exists(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION)) {
                saveNumber++;
                saveFileName = fileName + "_" + saveNumber;
            }
            // saveFileName is unique
        }
        File.WriteAllText(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION, saveString);
    }

    public static string Load(string fileName) {
        Init();

        string filePath = SAVE_FOLDER + fileName + "." + SAVE_EXTENSION;

        if (File.Exists(filePath))
        {
            // Realiza la comparación con sensibilidad a mayúsculas y minúsculas
            string[] files = Directory.GetFiles(SAVE_FOLDER);
            foreach (string file in files)
            {
                if (string.Equals(file, filePath, StringComparison.Ordinal))
                {
                    string saveString = File.ReadAllText(filePath);
                    return saveString;
                }
            }
        }

        return null;
    }

    public static string LoadMostRecentFile() {
        Init();
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        // Get all save files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        // Cycle through all save files and identify the most recent one
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles) {
            if (mostRecentFile == null) {
                mostRecentFile = fileInfo;
            } else {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) {
                    mostRecentFile = fileInfo;
                }
            }
        }

        // If theres a save file, load it, if not return null
        if (mostRecentFile != null) {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        } else {
            return null;
        }
    }

    public static string ObjectToString(object saveObject)
    {
        return JsonUtility.ToJson(saveObject);
    }

    public static SaveDataInventory StringToInventory(string inventory)
    {
        return JsonUtility.FromJson<SaveDataInventory>(inventory);
    }

    public static void SaveObject(object saveObject) {
        SaveObject("save", saveObject, false);
    }

    public static void SaveObject(string fileName, object saveObject, bool overwrite) {
        Init();
        string json = JsonUtility.ToJson(saveObject);
        Save(fileName, json, overwrite);
    }

    public static TSaveObject LoadMostRecentObject<TSaveObject>() {
        Init();
        string saveString = LoadMostRecentFile();
        if (saveString != null) {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        } else {
            return default(TSaveObject);
        }
    }

    public static TSaveObject LoadObject<TSaveObject>(string fileName) {
        Init();
        string saveString = Load(fileName);
        if (saveString != null) {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        } else {
            return default(TSaveObject);
        }
    }

    // Escribir una nueva línea en el archivo
    public static void WriteNewLine(string fileName, string line)
    {
        Init();

        string filePath = LOG_FOLDER + fileName;

        // Añadir una nueva línea al archivo
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(line);
        }
    }
}
