using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string savePath = "Assets/SaveFiles/"; // Ruta donde se guardar�n los archivos

    // Comprobar si el archivo existe
    public static bool DoesFileExist(string fileName)
    {
        string filePath = savePath + fileName;
        return File.Exists(filePath);
    }

    // Abrir y reescribir el archivo completo
    public static void RewriteFile(string fileName, string[] lines)
    {
        string filePath = savePath + fileName;

        // Escribir todas las l�neas en el archivo (reescribir)
        File.WriteAllLines(filePath, lines);
    }

    // Escribir una nueva l�nea en el archivo
    public static void WriteNewLine(string fileName, string line)
    {
        string filePath = savePath + fileName;

        // A�adir una nueva l�nea al archivo
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(line);
        }
    }

    // Crear un nuevo archivo si no existe
    public static void CreateFileIfNotExists(string fileName)
    {
        string filePath = savePath + fileName;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        if (!DoesFileExist(fileName))
        {
            File.Create(filePath).Close();
        }
    }
}
