using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WordsClass 
{
    // Array de palabras censurables
    public static string[] forbiddenWords = {
            "puto",
            "marica",
            // Agrega más palabras censurables aquí
        };

    public static Dictionary<string, Dictionary<int, string>> languageData = new Dictionary<string, Dictionary<int, string>>
    {
        {
            "Spanish", new Dictionary<int, string>
            {
                { 1, "Nueva partida" },
                { 2, "Unirse a la partida" },
                { 3, "Abrir servidor" },
                { 4, "Salir" },
                { 5, "Soltar" },
                { 6, "No puedes hacer nada con esto" },
                { 7, "Comer" },
                { 8, "Beber" },
                { 9, "Equipar" },
                { 10, "Desequipar" },
                // Otras traducciones para el idioma español
            }
        },
        {
            "English", new Dictionary<int, string>
            {
                { 1, "New game" },
                { 2, "Join game" },
                { 3, "Open server" },
                { 4, "Quit" },
                { 5, "Drop" },
                { 6, "You can't do anything with this" },
                { 7, "Eat" },
                { 8, "Drink" },
                { 9, "Equip" },
                { 10, "Unequip" },
                // Otras traducciones para el idioma inglés
            }
        },
        // Otros idiomas con sus traducciones
    };
}
