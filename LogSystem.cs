using System;
using System.IO;
using UnityEngine;

public static class LogSystem
{ 
    // Método para guardar registros de log en un archivo de texto
    public static void Log(string player, string description, string logFilePath)
    {
        // Obtiene la fecha y hora actual
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Formatea el registro de log
        string logEntry = $"{dateTime} | Player: {player} | Description: {description}";

        // Escribe el log en un archivo de texto
        WriteToLogFile(logEntry, logFilePath);
        Debug.Log(logEntry);
    }

    // Método para guardar registros de log en un archivo de texto
    public static void LogMessage(string player, string message, string logFilePath)
    {
        // Obtiene la fecha y hora actual
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Formatea el registro de log
        string logEntry = $"{dateTime} | Player: {player} | Mensaje: {message}";

        // Escribe el log en un archivo de texto
        WriteToLogFile(logEntry, logFilePath);
        Debug.Log(logEntry);
    }

    // Método para escribir en el archivo de log
    private static void WriteToLogFile(string logEntry, string logFilePath)
    {
        try
        {
            SaveSystem.CreateFileIfNotExists(logFilePath);
            SaveSystem.WriteNewLine(logFilePath, logEntry);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error al escribir en el archivo de log: " + e.Message);
        }
    }
}

