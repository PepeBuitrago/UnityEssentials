using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string savePath = "Assets/SaveFiles/"; // Ruta donde se guardarán los archivos

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

        // Escribir todas las líneas en el archivo (reescribir)
        File.WriteAllLines(filePath, lines);
    }

    // Escribir una nueva línea en el archivo
    public static void WriteNewLine(string fileName, string line)
    {
        string filePath = savePath + fileName;

        // Añadir una nueva línea al archivo
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
