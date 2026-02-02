using UnityEngine;
using TMPro;
using System.Collections; // WICHTIG: Wird für Coroutinen benötigt (IEnumerator)
using System.Collections.Generic;

public class ZahlenManager : MonoBehaviour
{
    [Header("UI Elemente")]
    public TextMeshProUGUI[] spaltenTexte;

    [Header("Einstellungen Slot-Maschine")]
    // Wie lange soll sich EINE Spalte insgesamt drehen?
    public float spinDauer = 3.0f;
    // Wie schnell wechseln die Zahlen während des Drehens (das "Rattern")?
    public float ratterGeschwindigkeit = 0.10f;
    // Wie viel Verzögerung gibt es zwischen dem Start von Spalte 1, 2 und 3?
    public float verzögerungZwischenSpalten = 0.5f;

    // Liste der erlaubten Zahlen (0-9 ohne die 8)
    private List<int> erlaubteZahlen = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 9 };

    // Eine Sicherung, damit man den Button nicht spammen kann, während es sich dreht
    private bool drehtSichGerade = false;

    // Diese Methode wird aufgerufen, wenn der Button gedrückt wird
    public void ZahlenNeuGenerieren()
    {
        // Wenn es sich schon dreht, brich ab. Verhindert Chaos bei Mehrfachklicks.
        if (drehtSichGerade) return;

        // Starte die Haupt-Routine, die den Ablauf steuert
        StartCoroutine(StarteSlotSequenz());
    }

    // --- NEUE LOGIK ---

    // 1. Die Haupt-Routine: Steuert den zeitlichen Ablauf der Spalten nacheinander
    private IEnumerator StarteSlotSequenz()
    {
        drehtSichGerade = true;

        // Gehe durch jede Spalte in deinem Array
        foreach (TextMeshProUGUI textFeld in spaltenTexte)
        {
            // a) Bestimme JETZT schon, was die endgültige Zahl sein wird
            int zufallsIndex = Random.Range(0, erlaubteZahlen.Count);
            int finaleZahl = erlaubteZahlen[zufallsIndex];

            // b) Starte das Rattern für DIESE EINE Spalte parallel
            // Wir übergeben das Textfeld und die Zahl, bei der es anhalten soll
            StartCoroutine(RatterSpalte(textFeld, finaleZahl));

            // c) Warte kurz, bevor die nächste Spalte im Loop gestartet wird
            // Das erzeugt den Effekt "nacheinander"
            yield return new WaitForSeconds(verzögerungZwischenSpalten);
        }

        // Optional: Warten bis alles fertig ist, um den Button wieder freizugeben.
        // Wir warten so lange wie ein Spin dauert, nachdem die letzte Spalte gestartet wurde.
        yield return new WaitForSeconds(spinDauer);

        drehtSichGerade = false;
        Debug.Log("Slot-Spin beendet!");
    }

    // 2. Die Ratter-Routine: Kümmert sich um die Animation einer einzigen Spalte
    private IEnumerator RatterSpalte(TextMeshProUGUI targetFeld, int endZahl)
    {
        float abgelaufeneZeit = 0f;

        // Solange die definierte Spin-Dauer noch nicht erreicht ist...
        while (abgelaufeneZeit < spinDauer)
        {
            // ...zeige irgendeine zufällige "Fake"-Zahl aus der Liste an
            int fakeIndex = Random.Range(0, erlaubteZahlen.Count);
            targetFeld.text = erlaubteZahlen[fakeIndex].ToString();

            // Warte ganz kurz (das Rattern)
            yield return new WaitForSeconds(ratterGeschwindigkeit);

            // Addiere die Wartezeit zur abgelaufenen Zeit
            abgelaufeneZeit += ratterGeschwindigkeit;
        }

        // WICHTIG: Am Ende der Zeit die echte, vorher bestimmte Endzahl setzen.
        // Damit stellen wir sicher, dass es exakt auf der richtigen Zahl stoppt.
        targetFeld.text = endZahl.ToString();
    }
}