# Lernperiode 11

22.8 bis 26.9.2024

## Grob-Planung

1. Fitness-Timer App - einfacher Timer für Training mit Start/Stop und Zeitanzeige.

2. Timer programmieren, Daten auf Handy speichern, einfache Buttons erstellen.

3. Benutzerfreundlichkeit und saubere Dokumentation für Bewerbung.

4. LP11: lokale Timer-App, 335: noch nicht entschieden.

## 22.8

- [x] Projekt erstellen und ersten Code schreiben
- [x] Timer-Funktion programmieren
- [x] Layout skizzieren, wie die App aussehen soll

✍️ Heute habe ich die Projektplanung gemacht und MAUI-Workloads in Visual Studio installiert. Das Setup war herausfordernd da MAUI anfangs nicht verfügbar war. Ich habe mich über Timer-Funktionen informiert, erste Skizzen für das Layout erstellt und das Projekt angelegt. Die grundlegende Projektstruktur mit MainPage.xaml steht, aber der eigentliche Timer-Code ist noch nicht implementiert.

## 29.8

- [x] Als Benutzer möchte ich einen Timer starten und stoppen können, damit ich mein Training kontrolliert durchführen kann.
- [x] Als Benutzer möchte ich die Zeit deutlich sehen während sie läuft, damit ich weiss wie viel Zeit noch bleibt.
- [x] Als Benutzer möchte ich verschiedene Timer-Zeiten einstellen können, damit ich verschiedene Übungen unterschiedlich lang timen kann.
- [X] Als Benutzer möchte ich dass die App meine letzten Timer-Zeiten speichert, damit ich meine gewohnten Zeiten schnell wieder verwenden kann.

✍️ Heute habe ich die komplette Timer-App programmiert und zum Laufen gebracht. Mit MainPage.xaml für die Benutzeroberfläche und MainPage.xaml.cs für die Timer-Logik konnte ich alle grundlegenden Funktionen implementieren. Die App hat Start/Pause/Stop/Reset-Buttons, Eingabefelder für Minuten und Sekunden sowie eine Countdown-Anzeige mit Farbwechsel. Der Timer läuft zuverlässig und alle Buttons funktionieren. Nur das Speichern von Timer-Zeiten konnte ich noch nicht umsetzen - das verschiebe ich auf nächste Woche.


## 5.9

- [x] Als Benutzer möchte ich dass alle Timer-Funktionen zuverlässig und fehlerfrei funktionieren, damit ich mich während des Trainings voll auf die Übungen konzentrieren kann.
- [x] Als Sportler möchte ich dass die App meine häufig genutzten Timer-Zeiten automatisch speichert und beim nächsten Start wieder lädt, damit ich Zeit bei der Eingabe spare.
- [x] Als Benutzer möchte ich die App auf meinem Smartphone testen können, damit ich sie auch unterwegs beim Training verwenden kann.
- [x] Als Entwickler möchte ich auftretende Bugs finden und beheben, damit die App stabil und benutzerfreundlich wird.

✍️ Heute habe ich die Timer-Funktionalität weiter optimiert und alle grundlegenden Bugs behoben. Besonders wichtig war die Implementierung der Speicher-Funktion, wodurch die App jetzt automatisch die zuletzt verwendeten Timer-Zeiten speichert und beim Neustart wieder lädt. Das Testen auf einem echten Android-Gerät war sehr aufschlussreich - einige Features funktionierten anders als im Emulator. Ich musste mehrere kleine Anpassungen vornehmen, besonders bei der Touch-Bedienung und der Bildschirmauflösung. Die App läuft jetzt deutlich stabiler und ist bereit für den produktiven Einsatz beim Training.


## 12.9

- [x] Als Benutzer möchte ich eine schöne und übersichtliche Benutzeroberfläche haben, damit die App professionell aussieht und angenehm zu bedienen ist.
- [x] Als Sportler möchte ich akustische Signale hören wenn der Timer abgelaufen ist, damit ich auch ohne Blick auf das Display merke wann die Zeit um ist.
- [x] Als Trainierender möchte ich meine vergangenen Timer-Zeiten in einer Liste sehen können, damit ich nachvollziehen kann welche Übungen ich wie lange gemacht habe.
- [x] Als Entwickler möchte ich meinen Code aufräumen und kommentieren, damit der Code verständlich und wartbar bleibt.

✍️ Heute habe ich die Benutzeroberfläche meiner Timer-App deutlich verbessert und professioneller gestaltet. Ich habe neue Farben und ein besseres Layout implementiert, wodurch die App viel ansprechender wirkt. Ausserdem konnte ich erfolgreich Sound-Benachrichtigungen hinzufügen, sodass Benutzer jetzt akustisch informiert werden wenn der Timer abläuft. Die grösste Herausforderung war die Implementierung der Timer-Historie, bei der gespeicherte Zeiten in einer scrollbaren Liste angezeigt werden. Zusätzlich habe ich meinen Code überarbeitet, sinnvolle Kommentare hinzugefügt und die Struktur verbessert für bessere Wartbarkeit.

## 31.10

- [x] Als Benutzer möchte ich einen vollständigen Timer mit Start/Stop/Pause/Reset haben, damit ich flexibel trainieren kann.
- [x] Als Sportler möchte ich vorgefertigte Workout-Programme nutzen können, damit ich verschiedene Trainingsarten durchführen kann.
- [x] Als Trainierender möchte ich automatische Trainings-/Pausen-Wechsel haben, damit ich mich voll auf das Training konzentrieren kann.
- [x] Als Benutzer möchte ich eine professionelle App mit Speicherfunktion und Historie haben, damit ich meine Fortschritte verfolgen kann.

✍️ Heute habe ich das komplette Projekt von Anfang bis Ende durchgearbeitet und fertiggestellt. Ich habe eine vollständige MAUI Timer-App entwickelt mit allen wichtigen Features: Timer-Funktionalität mit Start/Pause/Stop/Reset, 5 vorgefertigte Workout-Programme (HIIT, Tabata, Kraft, Cardio, Stretching), automatische Trainings- und Pausen-Phasen mit Rundenzählung, persistente Datenspeicherung mit JSON, Timer-Historie die alle Sessions speichert, und ein professionelles dunkles Design mit grünen Akzenten. Die grösste Herausforderung war die Implementierung des automatischen Workout-Systems, bei dem Training und Pause automatisch wechseln und die Runden gezählt werden. Das State-Management für die verschiedenen Timer-Zustände war komplex, aber ich konnte es mit sauberer Struktur lösen. Die App läuft stabil auf Windows und ist durch MAUI auch für Android/iOS bereit. Besonders stolz bin ich auf die benutzerfreundliche Oberfläche und den sauberen, kommentierten Code. Das Projekt ist perfekt für mein Bewerbungsportfolio und zeigt meine Fähigkeiten in C#, MAUI, XAML, JSON-Verarbeitung und Cross-Platform Entwicklung.

---

## Implementierte Features
✅ Timer mit Start/Pause/Stop/Reset  
✅ Manuelle Zeit-Eingabe (Minuten + Sekunden)  
✅ 5 vorgefertigte Workout-Programme (HIIT, Tabata, Kraft, Cardio, Stretching)  
✅ Automatische Trainings-/Pausen-Wechsel mit Rundenanzeige  
✅ Timer-Historie mit Datum/Zeit und Workout-Namen  
✅ Persistente Datenspeicherung (JSON + Preferences)  
✅ Professionelles dunkles Design (Grün/Schwarz/Grau)  
✅ Sound-Benachrichtigungen bei Timer-Ende  
✅ Cross-Platform (MAUI - Windows getestet, Android/iOS kompatibel)

## Technologie-Stack
- **Framework:** .NET MAUI
- **Sprache:** C#
- **UI:** XAML
- **Datenspeicherung:** Preferences API + JSON-Serialisierung
- **Plattformen:** Windows (getestet)

## Projektstruktur
```
FitnessTimer/
├── MainPage.xaml          (UI-Layout mit Workout-Auswahl)
├── MainPage.xaml.cs       (Timer-Logik + Workout-System)
├── WorkoutPresets.cs      (Workout-Vorlagen + JSON-Verwaltung)
├── App.xaml               (App-Konfiguration)
└── MauiProgram.cs         (Entry Point)
```

## Gelerntes
- MAUI Cross-Platform App-Entwicklung
- XAML UI-Design und responsive Layouts
- Timer-Programmierung mit System.Timers
- State-Management für komplexe Abläufe
- JSON-Serialisierung und Deserialisierung
- Persistente Datenspeicherung mit Preferences
- Asynchrone Programmierung und Threading
- Clean Code und Code-Dokumentation
