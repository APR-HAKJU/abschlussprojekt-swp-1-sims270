using UnityEngine;
using TMPro; // Wichtig für TextMeshPro
using System.Collections.Generic;

public class ZahlenManager : MonoBehaviour
{
    // Die Text-Felder aus der UI
    public TextMeshProUGUI[] spaltenTexte; 

    // Liste der erlaubten Zahlen (0-9 ohne die 8)
    private List<int> erlaubteZahlen = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 9 };

    // Diese Methode wird aufgerufen, wenn der Button gedrückt wird
    public void ZahlenNeuGenerieren()
    {
        foreach (TextMeshProUGUI textFeld in spaltenTexte)
        {
            // Wähle einen zufälligen Index aus der Liste der erlaubten Zahlen
            int zufallsIndex = Random.Range(0, erlaubteZahlen.Count);
            int gewaehlteZahl = erlaubteZahlen[zufallsIndex];

            // Zahl im Textfeld anzeigen
            textFeld.text = gewaehlteZahl.ToString();
        }
    }
}