using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    public Dictionary<int, string> currentLanguage;

    void Awake()
    {
        Instance = this;
    }

    // Funci�n para cambiar el idioma
    public void ChangeLanguage(string newLanguage)
    {
        // L�gica para cambiar las cadenas de texto al idioma seleccionado
         currentLanguage = LoadLanguage(newLanguage);
    }

    public string GetLocalizedString(int key)
    {
        string result = "";
        if (currentLanguage.TryGetValue(key, out result))
        {
            return result; // Devuelve la cadena traducida seg�n la clave
        }
        return "KEY NOT FOUND"; // Si la clave no existe en el diccionario
    }

    Dictionary<int, string> LoadLanguage(string selectedLanguage)
    {
        Dictionary<int, string> loadedLanguage = new Dictionary<int, string>();

        // Verifica si el idioma seleccionado est� presente en los datos de idioma
        if (WordsClass.languageData.ContainsKey(selectedLanguage))
        {
            // Carga las traducciones para el idioma seleccionado
            loadedLanguage = WordsClass.languageData[selectedLanguage];
        }
        else
        {
            // Si el idioma seleccionado no est� disponible, carga un idioma predeterminado (por ejemplo, ingl�s)
            loadedLanguage = WordsClass.languageData["Spanish"];
        }

        return loadedLanguage;
    }
}
