## AnonPDF Pro - Benutzerhandbuch

*Deutsche Version der Benutzeranleitung*

# 1. Einführung

AnonPDF Pro ist ein Werkzeug zur teilweisen oder vollständigen Entfernung (Anonymisierung) ausgewählter Inhalte in PDF-Dateien. Sie markieren Bereiche, die entfernt werden sollen, und erzeugen danach eine neue PDF-Datei mit angewendeten Ausschlüssen. So lassen sich sensible Daten wie personenbezogene Angaben oder vertrauliche Inhalte vor der Weitergabe sicher entfernen.

# 2. Installation und Start

> **1.** Installieren Sie AnonPDF Pro (je nach Bereitstellung als Installer oder als bereitgestellte Programmdateien).

> **2.** Stellen Sie sicher, dass die benötigte .NET-Laufzeit installiert ist.

> **3.** Starten Sie die Anwendung Über Desktop-Verknüpfung, Startmenü oder direkt Über AnonPDFPro.exe.

> **4.** Nach dem Start wird das Hauptfenster der Anwendung geöffnet.

## 2.5 PRO-Aktivierung (Upgrade von DEMO)

Die Aktivierung hängt vom Betriebsmodus der Anwendung ab.

### Einzelplatzversion (Standalone)

In diesem Modus führt der Benutzer die Aktivierung in der Anwendung aus:

1. Starten Sie AnonPDF Pro.
2. Öffnen Sie das Menü `?` und wählen Sie `Aktivierung`.
3. Wählen Sie das erhaltene Lizenzpaket aus.
4. Bestätigen Sie den Vorgang und starten Sie die Anwendung neu.
5. Öffnen Sie `? -> Über` und prüfen Sie den Lizenzstatus (PRO).

### Netzwerkversion (Central)

In diesem Modus aktivieren Endbenutzer die Anwendung nicht über das Menü.
Die Aktivierung erfolgt durch einen Administrator:

1. Schließen Sie die Anwendung auf den Benutzerarbeitsplätzen.
2. Entpacken Sie das Lizenzpaket.
3. Kopieren Sie die Lizenzdateien in den Anwendungsordner (dort, wo `AnonPDFPro.exe` liegt).
4. Bestätigen Sie ggf. das Ersetzen vorhandener Dateien.
5. Starten Sie die Anwendung neu und prüfen Sie `? -> Über`.

Hinweise:
- Behalten Sie die ursprünglichen Dateinamen und die Struktur des Lizenzpakets bei.
- Falls die Anwendung weiterhin im DEMO-Modus läuft, prüfen Sie Dateipfad und Ordnerberechtigungen.

# 3. Programmoberfläche

Das Hauptfenster besteht aus folgenden Bereichen:

> \- Obere Menüleiste mit den Bereichen Datei, Werkzeuge, Optionen und ?.

> \- Rechtes Panel mit den Tabs `Seitenliste`, `Miniaturen` und `Ebenen`. Dort können Seiten, Miniaturen und Ebenenattribute schnell umgeschaltet werden.

> \- Hauptvorschau in der Mitte zur Bearbeitung der aktuellen Seite.

> \- Bedienbereich mit Navigation, Zoom, Suche und Bearbeitungswerkzeugen.

# 4. Erste Schritte

## 4.1 PDF öffnen

Wählen Sie Datei -\> PDF öffnen und wählen Sie das Ziel-Dokument.

## 4.2 Seiten durchsuchen

Verwenden Sie die Seitenliste oder die Navigationsschaltflächen (Vorherige/Nächste), um zwischen Seiten zu wechseln.

## 4.3 Suche

So suchen Sie im Dokument:

> **1.** Geben Sie den Suchtext im linken Bereich ein und bestätigen Sie mit Enter.

> **1.** Geben Sie den Suchtext im linken Bereich ein und best?tigen Sie mit Enter.

> **2.** Wechseln Sie mit den Such-Navigationstasten zwischen Treffern.

> **3.** Verwenden Sie die Schaltfl?che ?PESEL, KW usw.?, um zus?tzlich typische Identifikatoren zu erkennen.

> **4.** ?ber die Schaltfl?che X kann ein Suchtreffer aus der Liste entfernt werden.

Zum Markieren eines Bereichs:

> **1.** Wählen Sie Marker oder Box.

> **2.** Halten Sie die linke Maustaste gedrückt und ziehen Sie den Bereich auf.

> **3.** Im Marker-Modus zeichnen Sie zeilenorientiert; im Box-Modus frei rechteckig.

> **4.** Im Marker-Modus schaltet gedrücktes `CTRL` auf das rechteckige Zeichnen (Box) um.

> **5.** Lassen Sie die Maustaste los, um die Markierung zu Übernehmen.

> **6.** Eine vorhandene Markierung entfernen Sie per Rechtsklick im markierten Bereich.

## 4.5 Vorschau der markierten Ausschlüsse

Nach dem Markieren wird der betroffene Text in der Vorschau sofort grau dargestellt.

## 4.6 Projekt speichern

Speichern Sie den Bearbeitungsstand Über Projekt speichern oder Projekt speichern unter. Das Projekt enthält alle Markierungen und Objekte.

## 4.7 PDF exportieren

Klicken Sie auf PDF speichern, wählen Sie den Zielpfad und speichern Sie die finale Datei mit angewendeten Ausschlüssen.

# 5. Navigation und Zoom

## 5.1 Seitennavigation

Verwenden Sie Erste, Vorherige, Nächste, Letzte oder geben Sie eine Seitenzahl direkt ein. Die aktuelle Seite kann mit `CTRL + R` gedreht werden.

## 5.2 Navigation zwischen Markierungen

Mit den Funktionen Erste/Vorherige/Nächste/Letzte Markierung springen Sie schnell zu Seiten mit Ausschlüssen.

## 5.3 Zoom und Scrollen

> \- Zoomen Über Plus/Minus sowie Min/Max.

> \- Mausrad: vertikales Scrollen.

> \- CTRL + Mausrad: Zoomen.

> \- F11 schaltet den Vollbildmodus um, ESC beendet den Vollbildmodus.

# 6. Zusätzliche Funktionen

## 6.1 Markierungen löschen

- `Aktuelle Seite löschen` entfernt alle Markierungen auf der aktiven Seite und verlangt eine Bestätigung.

- `Alle Seiten löschen` entfernt alle Markierungen im gesamten Dokument und verlangt ebenfalls eine Bestätigung.

## 6.2 Signaturen

Für qualifizierte Signaturen stehen mehrere Optionen zur Verfügung:

> \- Entfernt: entfernt die Signaturen aus dem Dokument.

> \- Original: lässt Signaturen sichtbar, sie sind nach Änderungen jedoch nicht mehr gültig.

> \- Bericht: fügt statt der Signaturdarstellung eine zusätzliche Berichtsseite hinzu.

## 6.3 Seiten entfernen

> \- Mit den Lösch-Symbolen können einzelne Seiten oder Bereiche zum Entfernen markiert werden.

## 6.4 Schwärzung

Standardmäßig wird Inhalt im markierten Bereich entfernt. Bei Bild-/Scan-Inhalten wird der entfernte Bereich passend gefüllt.

Wenn die Option `Schwärzung` aktiviert ist, werden markierte Bereiche in der gespeicherten PDF-Datei schwarz dargestellt.

## 6.5 Vorschau nach dem Speichern

Ist diese Option aktiv, wird die gespeicherte PDF-Datei nach dem Export automatisch geöffnet.

## 6.6 Projekt laden

Ein gespeichertes Projekt kann jederzeit erneut geladen werden, um die Bearbeitung fortzusetzen.

Wenn die im Projekt gespeicherte PDF-Datei unter dem ursprünglichen Pfad nicht verfügbar ist, sucht die Anwendung zusätzlich nach einer PDF-Datei mit demselben Namen im Ordner der `.app`-Projektdatei.

## 6.7 Anwendung schließen

Beim Beenden fragt die Anwendung bei ungespeicherten Änderungen nach einer Bestätigung.

## 6.8 Hilfe

> \- Benutzeranleitung: öffnet die Anleitung.

> \- `Wie fange ich an?`: öffnet das kurze Start-Tutorial.

> \- Über: zeigt Versions- und Herstellerinformationen.

## 6.9 Objekte hinzufügen und bearbeiten

Objekte können über das Menü `Werkzeuge` oder mit Tastenkürzeln hinzugefügt werden:

> \- `CTRL + SHIFT + A` - Pfeil hinzufügen.

> \- `CTRL + F` - Form hinzufügen.

> \- `CTRL + G` - Bild hinzufügen.

> \- `CTRL + K` - Kommentar hinzufügen.

> \- `CTRL + A` - alle sichtbaren Objekte auf der aktiven Ebene der aktuellen Seite auswählen.

Für die Arbeit mit Objekten stehen außerdem zur Verfügung:

> \- `CTRL + C` - aktives Objekt oder aktive Objektgruppe kopieren.

> \- `CTRL + V` - Objekt aus der Zwischenablage einfügen. Das eingefügte Objekt wird auf der aktiven Ebene erstellt, behält die ursprüngliche Stapelreihenfolge und wird bei Bedarf automatisch verkleinert, wenn die Zielseite kleiner ist.

> \- Das Kopieren und Einfügen von Objekten funktioniert auch zwischen zwei gleichzeitig gestarteten Instanzen der Anwendung.

> \- `DELETE` - aktives Objekt oder aktive Objektgruppe löschen.
> \- Im Textdialog kann die Option `Pfeil` für den Textrahmen aktiviert werden.
> \- Für den Textpfeil lassen sich `Liniendicke des Pfeils`, `Spitzenlänge` und `Spitzenbreite` festlegen.
> \- Das Ende des Textpfeils kann mit der Maus verschoben werden; der Anheftungspunkt am Rahmen wird automatisch auf Ecken und Seitenmitten aktualisiert.
> \- Beim Drehen des Textes wird das Pfeilende nicht mitgedreht; nur der Anheftungspunkt am Rahmen wird aktualisiert.
> \- Beim Skalieren des Textes werden Rahmen und angehefteter Pfeil als ein Objekt mit skaliert.
- Im Formdialog steht für Rechteck, Ellipse, Dreieck und Region die Option `Rasteransicht begrenzen` zur Verfügung.
- Diese Option dient nicht der Anonymisierung. Sie begrenzt nur den sichtbaren Bereich von Rasterobjekten, die darunter auf derselben Ebene liegen.
- In der Vorschau bleibt der Rasterbereich innerhalb der Form klar sichtbar, während der Bereich außerhalb abgeblendet wird. Im finalen PDF bleibt nur der durch die Form begrenzte Bereich sichtbar.
- Beim Erstellen von Mehrpunktformen zeigt die Anwendung oben im Arbeitsbereich eine kurze Anleitung an. Die linke Maustaste fügt den nächsten Punkt hinzu, die rechte Maustaste beendet das Zeichnen.

Modifikatoren bei der Objektbearbeitung:

> \- `ALT` beim Erstellen von Formen erzwingt feste Proportionen: Rechteck -> Quadrat, Ellipse -> Kreis, Dreieck -> gleichseitig.

> \- `ALT` beim Ändern der Bildgröße arbeitet symmetrisch zum Mittelpunkt des Objekts.

> \- Texte, Pfeile und Vektorformen können mit Eckgriffen skaliert werden.

> \- `SHIFT` beim Skalieren erhält die Proportionen des Objekts.

> \- `ALT` beim Skalieren skaliert relativ zum Mittelpunkt des Objekts.

> \- `SHIFT + ALT` beim Skalieren erhält die Proportionen und hält den Mittelpunkt des Objekts an derselben Position.

## 6.10 Markierungen und Objekte duplizieren

Im Kontextmenü (rechte Maustaste) stehen folgende Funktionen zur Verfügung:

> \- `Markierung duplizieren` - für Anonymisierungsmarkierungen.

> \- `Objekt duplizieren` - für Objekte (z. B. Text, Bild, Pfeil, Vektorform, Kommentar).

Nach Auswahl wird ein Dialog mit Seitenbereich geöffnet:

> \- `Von` / `Bis` - Seitenbereich, in dem Kopien erstellt werden.

> \- Die aktuelle Quellseite wird automatisch ausgelassen.

Für duplizierte Objekte gibt es zusätzlich:

> \- `Duplizierte Objekte löschen` - entfernt Kopien im gewählten Seitenbereich.

## 6.11 Ebenen

Die Anwendung unterstützt Arbeitsebenen für Markierungen und Objekte.

- Im Tab `Ebenen` im rechten Panel können aktive Ebene, Sichtbarkeit und Sperre schnell geändert werden.
- Ebenen können zusätzlich über einen gemeinsamen Gruppennamen gruppiert werden.
- Mit der Gruppen-Checkbox lassen sich Sichtbarkeit oder Sperre für alle Ebenen der Gruppe mit einem Klick ändern.
- Haben Ebenen einer Gruppe unterschiedliche Zustände, zeigt die Gruppen-Checkbox einen gemischten Zustand an.
- Ein Doppelklick auf den Gruppennamen im Tab `Ebenen` ermöglicht das Umbenennen der Gruppe.
- `Werkzeuge -> Ebenen` öffnet das vollständige Ebenenfenster.
- Im Ebenendialog kann eine Ebene über die Spalte `Gruppe` einer Gruppe zugeordnet werden.
- Neue Markierungen und neue Objekte werden auf der aktuell aktiven Ebene erstellt.
- Ein neu erstellter `Box`-Ausschluss bleibt sofort ausgewählt. Seine Größe kann über die Griffe oben links und unten rechts korrigiert werden.
- Ein Klick auf eine vorhandene Markierung wählt sie aus und zeigt oben im Arbeitsbereich eine kurze Information an. Dabei wird keine neue Markierung innerhalb der bestehenden erzeugt.
- Eingefügte Objekte werden auf der aktiven Ebene erstellt und behalten die Stapelreihenfolge der Quelle.
- Die Ebene `Robocza` liegt immer ganz oben und wird nicht in das finale PDF exportiert.
- Mit ausgeblendeten Ebenen kann in der Vorschau nicht interagiert werden.
- Gesperrte Ebenen blockieren Verschieben, Bearbeiten, Skalieren und Löschen von Objekten.
- Beim Projektimport bleiben die Zuordnungen von Objekten zu Ebenen erhalten.
- Im Ebenendialog überschreibt die Option `Nur sichtbare Ebenen exportieren und drucken` die Einstellungen aus der Spalte `Export`.

# 7. Häufige Probleme und Hinweise

## 7.1 Ausgabe-PDF kann nicht gespeichert werden

> \- Prüfen Sie, ob die Zieldatei in einem anderen Programm geöffnet ist.

> \- Speichern Sie ggf. unter einem anderen Dateinamen.

## 7.2 Projektdatei passt nicht zum PDF

Projektdateien enthalten seitenbezogene Informationen. Laden Sie das Projekt nur mit der passenden PDF-Datei.

## 7.3 Keine Markierungen / PDF speichern ist deaktiviert

Prüfen Sie, ob mindestens eine Markierung vorhanden ist.

## 7.4 Langsame Verarbeitung

Sehr große Dokumente oder viele Objekte können die Verarbeitung verlangsamen. Speichern Sie Zwischenstände und arbeiten Sie abschnittsweise.

# 8. Zusammenfassung

AnonPDF Pro ermöglicht eine schnelle und nachvollziehbare Bearbeitung von PDF-Dokumenten für Veröffentlichung und Weitergabe. Durch Projekte, Objektwerkzeuge, Fußnoten und flexible Exportoptionen ist die Anwendung für den professionellen Einsatz in Verwaltung und Organisationen ausgelegt.

# 9. Neue Funktionen

In den letzten Versionen wurden u. a. ergänzt:

- Drucken über das Menü `Datei` (`CTRL+P`) mit Auswahl: aktuelle Seite, Seitenbereich oder gesamtes Dokument.
- Option `Seitenbereich als PDF speichern` im Menü `Datei`, standardmäßig mit der aktuellen Seite vorbelegt.
- Drei Tabs auf der rechten Seite: `Seitenliste`, `Miniaturen` und `Ebenen`.
- Dynamische Erzeugung von Miniaturen sowie Miniatur-Cache für bessere Leistung bei großen Dokumenten.
- Miniaturen, die zusätzlich Markierungen, Kommentare, Objekte und zum Löschen markierte Seiten anzeigen.
- Speicherung des zuletzt gewählten rechten Tabs (`Seitenliste` / `Miniaturen` / `Ebenen`).
- Speicherung der Breite des rechten Panels zwischen Programmstarts.
- Speicherung und Wiederherstellung der Scroll-Positionen in den rechten Panels (`Seitenliste` und `Miniaturen`) beim Fortsetzen eines Projekts.
- Unterstützung für Arbeitsebenen mit eigener Sichtbarkeit, Sperre und aktiver Ebene.
- Erweiterte Kontextmenüs für Markierungen und Objekte (Kopieren, Ausschneiden, Duplizieren, duplizierte Kopien löschen).
- Skalierung einzelner Objekte und Objektgruppen über Eckgriffe (`SHIFT` - proportional, `ALT` - vom Mittelpunkt).
- Die Option `Pfeil` im Textdialog mit einstellbarer Liniendicke des Pfeils, Spitzenlänge und Spitzenbreite.
- Kopieren und Einfügen von Objekten über die Systemzwischenablage, auch zwischen zwei laufenden Instanzen der Anwendung, mit erhaltener Stapelreihenfolge.
- `Rückgängig` (`CTRL+Z`) und `Wiederholen` (`CTRL+Y`).
- Kurzes Start-Tutorial über `? -> Wie fange ich an?`.
