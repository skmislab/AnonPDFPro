---
title: AnonPDF Pro - Benutzerhandbuch
lang: de
---

*Deutsche Version der Benutzeranleitung*

# 1. Einführung

AnonPDF Pro ist ein Werkzeug zur teilweisen oder vollständigen Entfernung (Anonymisierung) ausgewählter Inhalte in PDF-Dateien. Sie markieren Bereiche, die entfernt werden sollen, und erzeugen danach eine neue PDF-Datei mit angewendeten Ausschlüssen. So lassen sich sensible Daten wie personenbezogene Angaben oder vertrauliche Inhalte vor der Weitergabe sicher entfernen.

# 2. Installation und Start

> **1.** Installieren Sie AnonPDF Pro (je nach Bereitstellung als Installer oder als bereitgestellte Programmdateien).

> **2.** Die Anwendung benötigt Windows und .NET Framework 4.8.

> **3.** Starten Sie die Anwendung über eine Desktop-Verknüpfung, das Startmenü oder direkt über AnonPDFPro.exe.

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

> \- Rechtes Panel mit den Tabs `Seitenliste`, `Miniaturen`, `Ebenen` und `Gefunden`. Dort können Seiten, Miniaturen und Ebenenattribute schnell umgeschaltet sowie Suchergebnisse und erkannte Personendaten eingesehen werden.

> \- Hauptvorschau in der Mitte zur Bearbeitung der aktuellen Seite.

> \- Bedienbereich mit Navigation, Zoom, Suche und Bearbeitungswerkzeugen.

# 4. Erste Schritte

## 4.1 PDF öffnen

Wählen Sie Datei -\> PDF öffnen und wählen Sie das Ziel-Dokument.

## 4.2 Seiten durchsuchen

Verwenden Sie `Seitenliste` oder `Miniaturen` im rechten Panel oder die Navigationsschaltflächen (Vorherige/Nächste), um zwischen Seiten zu wechseln.

## 4.3 Suche

So suchen Sie im Dokument:

> **1.** Geben Sie den Suchtext im linken Bereich ein und bestätigen Sie mit Enter.

> **2.** Wechseln Sie mit den Such-Navigationstasten zwischen Treffern.

> **3.** Verwenden Sie die Schaltfläche `PESEL, KW usw.`, um zusätzliche Identifikatoren wie PESEL, Grundbuchnummer, Ausweisnummer, NIP, REGON, KRS, Bankkonto, E-Mail-Adresse, Postleitzahl, Internetadresse oder VIN zu erkennen.

> **4.** Mit `Suchergebnisse markieren...` können gefundene Treffer in Anonymisierungsmarkierungen umgewandelt werden.

> **5.** Über die Schaltfläche X kann ein Suchtreffer aus der Liste entfernt werden.

Die Suche funktioniert auch in gescannten Dokumenten und auf Bildern, die in einer PDF-Datei eingebettet sind. Bei solchen Dokumenten kann die Anwendung zuerst den Text für die Suche vorbereiten; dabei kann kurz eine Indizierungsinformation angezeigt werden.

Wenn eine gescannte Seite leicht schief ist, versucht die Anwendung, die Positionen der Suchtreffer an die tatsächliche Textposition auf der Seite anzupassen.

Die Schaltfläche `Personendaten` startet die Erkennung personenbezogener Daten im gesamten Dokument. Suchergebnisse und erkannte Personendaten - auch aus Alternativtexten (ALT) und aus Bereichen außerhalb der Seite - erscheinen im Tab `Gefunden` (siehe Abschnitt 6.14).

## 4.4 Bereiche zur Anonymisierung markieren

Im Modus `Cursor` erstellt ein Klick auf ein Wort eine Anonymisierungsmarkierung, auch bei per OCR erkannten Wörtern. Die Markierung lässt sich mit Griffen anpassen. Für `Marker` oder `Box` gehen Sie wie folgt vor:

> **1.** Wählen Sie Marker oder Box.

> **2.** Halten Sie die linke Maustaste gedrückt und ziehen Sie den Bereich auf.

> **3.** Im Marker-Modus zeichnen Sie zeilenorientiert; im Box-Modus frei rechteckig.

> **4.** Im Marker-Modus schaltet gedrücktes `CTRL` auf das rechteckige Zeichnen (Box) um.

> **5.** Lassen Sie die Maustaste los, um die Markierung zu Übernehmen.

> **6.** Eine vorhandene Markierung entfernen Sie per Rechtsklick im markierten Bereich.

Eine ausgewählte `Box`- oder `Marker`-Markierung kann auch mit der Taste `DELETE` entfernt werden. Im Cursor-Modus wählt ein Klick eine vorhandene Markierung aus, ohne eine neue Markierung zu zeichnen.

Das Kontextmenü einer `Box` enthält `Ganze Wörter`. Ist die Option ausgeschaltet, wird in Scans nur das Rechteck entfernt, ohne es auf ganze berührte OCR-Wörter zu erweitern. Die Einstellung gilt auch für weitere Boxen in diesem Dokument und wird im Projekt gespeichert. Der `Marker` erfasst weiterhin berührte Wörter. Erfasst er zu viel Text, löschen Sie die Markierung und zeichnen sie an der richtigen Stelle neu.

## 4.5 Vorschau der markierten Ausschlüsse

Zur Entfernung vorgesehener Text wird in der Vorschau grau dargestellt. Bei OCR-Scans hilft dies, versehentlich erfasste Nachbarzeilen zu erkennen. Bei einer Box ohne `Ganze Wörter` bestimmt das Rechteck den Entfernungsbereich. Prüfen Sie das Ergebnis vor der Weitergabe in der eingebauten Vorschau.

## 4.6 Projekt speichern

Speichern Sie den Bearbeitungsstand Über Projekt speichern oder Projekt speichern unter. Das Projekt enthält alle Markierungen und Objekte.

## 4.7 PDF exportieren

Klicken Sie auf PDF speichern, wählen Sie den Zielpfad und speichern Sie die finale Datei mit angewendeten Ausschlüssen.

Der vorgeschlagene Dateiname endet auf `_anon`, kann aber geändert werden. Bei weiteren Speichervorgängen für das finale PDF in derselben Dokumentsitzung wird der zuletzt gewählte Name vorgeschlagen.

Vor dem Speichern können Sie `Datei -> Vorschau` (`CTRL+F2`) verwenden. Diese Funktion öffnet eine eingebaute Vorschau des Dokuments nach der Anonymisierung, ohne einen externen PDF-Viewer zu starten. Im Vorschaufenster können Sie zwischen Seiten wechseln, eine Seitenzahl eingeben und das Dokument in der Form drucken, in der es gespeichert wird.

Die Vorschau öffnet sich maximiert auf der aktuellen Seite. Sie berücksichtigt auch Objekte, Seitenanordnung und Ränder aus den Dokumenteinstellungen. Umriss und Schwärzung beeinflussen das Aussehen der fertigen Ausgabe.

Wenn Sie Seitenbilder statt einer PDF-Datei benötigen, wählen Sie `Datei -> Seitenbereich als JPEG speichern`. Nach Auswahl eines Ordners erstellt die Anwendung darin einen Unterordner mit dem Namen der aktuellen PDF-Datei und speichert die ausgewählten Seiten als einzelne, nummerierte `.jpg`-Dateien. Der Fortschritt wird während des Speicherns oben in der Ansicht angezeigt.

Bei vollständig gescannten Dokumenten ohne eigene Textebene kann im Speichern-Dialog zusätzlich der Typ `PDF/A-Dateien (*.pdf)` verfügbar sein. Dadurch wird eine PDF/A-Datei für Archivierung und digitale Veröffentlichung erstellt, mit dem Seitenbild und einer unsichtbaren Textebene aus der OCR-Erkennung. Das Ergebnis lässt sich leichter durchsuchen und eignet sich besser für die digitale Bereitstellung.

Wenn Sie für ein gescanntes Dokument PDF/A auswählen, merkt sich die Anwendung diese Auswahl und verwendet PDF/A beim nächsten Speichern eines ähnlichen gescannten Dokuments als Standardtyp. Im Feld `Dateityp` können Sie jederzeit wieder auf eine normale PDF-Datei umstellen.

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

Die Anwendung erkennt elektronische Signaturen im Quelldokument.

- Unter `Optionen` wählen Sie die Signaturen aus, deren Darstellung entfernt werden soll. Anfangs ist keine Signatur zum Entfernen ausgewählt.
- Im Bereich `Signaturen` belässt `Beibehalten` die übrigen Signaturdarstellungen. `Bericht` ersetzt sie durch eine Seite mit Signaturinformationen.
- Unter `Darstellung` können Sie das Original oder `AnonPDFPro` mit Prüfstatus in der aktuellen Anwendungssprache wählen.
- Der Prüfstatus bezieht sich auf die Signatur im Quell-PDF. Das Bearbeiten erzeugt keine neue elektronische Signatur; die gespeicherte Kopie ist kein neu signiertes Dokument.

## 6.3 Seiten entfernen

> \- Mit den Lösch-Symbolen können einzelne Seiten oder Bereiche zum Entfernen markiert werden.

Dieselben Markierungen lassen sich unter `Datei -> Seiten organisieren` für einzelne oder mehrere Seiten setzen und aufheben. Quellseiten bleiben im Projekt erhalten und werden erst beim Speichern aus der fertigen PDF-Datei entfernt.

## 6.4 Schwärzung

Standardmäßig wird Inhalt im markierten Bereich entfernt. Bei Bild-/Scan-Inhalten wird der entfernte Bereich passend gefüllt.

Wenn die Option `Schwärzung` aktiviert ist, werden markierte Bereiche in der gespeicherten PDF-Datei schwarz dargestellt.

## 6.5 Vorschau nach dem Speichern

Ist diese Option aktiv, wird die gespeicherte PDF-Datei nach dem Export automatisch geöffnet.

## 6.6 Projekt laden

Ein gespeichertes Projekt kann jederzeit erneut geladen werden, um die Bearbeitung fortzusetzen.

Wenn die im Projekt gespeicherte PDF-Datei unter dem ursprünglichen Pfad nicht verfügbar ist, sucht die Anwendung zusätzlich nach einer PDF-Datei mit demselben Namen im Ordner der `.app`-Projektdatei.

Projektdateien mit der Erweiterung `.app` können in Windows mit AnonPDF Pro verknüpft werden, sodass sie direkt aus dem Datei-Explorer geöffnet werden können.

## 6.7 Anwendung schließen

Beim Beenden fragt die Anwendung bei ungespeicherten Änderungen nach einer Bestätigung.

## 6.8 Hilfe

> \- Benutzeranleitung: öffnet die Anleitung.

> \- `Wie fange ich an?`: öffnet das kurze Start-Tutorial.

> \- Über: zeigt Versions- und Herstellerinformationen.

## 6.9 Objekte hinzufügen und bearbeiten

`ESC` hebt die Objektauswahl auf, ohne Objekte zu löschen. Der integrierte Pfeil eines Textobjekts besitzt eigene Füll- und Randfarben, Randstärke, Liniendicke und Spitzenabmessungen. Randstärke null bedeutet keinen Pfeilrand, unabhängig vom Textrahmen. Bei der Bildbearbeitung lässt sich die Tintenfarbe ändern; geglättete Kanten behalten zur gewählten Farbe passende Abstufungen.

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
- Die Arbeitsebene liegt immer ganz oben und ist standardmäßig vom Export ausgeschlossen. Prüfen Sie vor dem Speichern die Exporteinstellungen: Der Export nur sichtbarer Ebenen verwendet die Sichtbarkeit statt der Spalte `Export`.
- Andere Ebenen lassen sich im Ebenendialog durch Ziehen umsortieren.
- Mit ausgeblendeten Ebenen kann in der Vorschau nicht interagiert werden.
- Gesperrte Ebenen blockieren Verschieben, Bearbeiten, Skalieren und Löschen von Objekten.
- Beim Projektimport bleiben die Zuordnungen von Objekten zu Ebenen erhalten.
- Im Ebenendialog überschreibt die Option `Nur sichtbare Ebenen exportieren und drucken` die Einstellungen aus der Spalte `Export`.

## 6.12 Darstellung und Barrierefreiheit

- Im Menü `Optionen` kann das Anwendungsthema gewählt werden.
- Es stehen auch kontrastreiche Themen zur Verfügung: Schwarz-Gelb und Schwarz-Weiß.
- Die Anwendung passt die Größe der Oberfläche besser an die Windows-Anzeigeskalierung an.

## 6.13 Aktuelle Ansicht speichern

`Datei -> Aktuelle Ansicht speichern...` speichert die aktuell sichtbare Seitenansicht als PNG- oder BMP-Bilddatei. Gespeichert wird der sichtbare Vorschauzustand, zum Beispiel für Dokumentationszwecke oder zur schnellen Weitergabe eines Seitenbildes.

## 6.14 Tab „Gefunden“

Der Tab `Gefunden` im rechten Panel sammelt Suchergebnisse und erkannte Personendaten an einem Ort.

- Die Einträge sind nach Seiten gruppiert und haben Kontrollkästchen.
- Jeder Treffer zeigt den gefundenen Inhalt und seinen Typ. Im Filter können mehrere verfügbare Kategorien gleichzeitig gewählt werden, etwa nur NIP und PESEL.
- Änderungen der Filterkästchen aktualisieren sofort die Liste und die gelben Trefferhervorhebungen auf der Seite. Das Ausblenden einer Kategorie löscht keine bereits erstellten Anonymisierungsmarkierungen.
- Ein Klick auf einen Eintrag springt zur betreffenden Seite und hebt die Stelle im Dokument hervor.
- Das Aktivieren eines Eintrags erstellt eine Anonymisierungsmarkierung, das Deaktivieren entfernt sie. Die Kontrollkästchen sind in beide Richtungen mit den Markierungen auf der Seite synchronisiert.
- Wählen Sie einzelne Treffer, alle Treffer einer Seite oder alle beim aktuellen Filter sichtbaren Treffer. Ein Klick auf ein hervorgehobenes Wort im Modus `Cursor` wird mit dem zugehörigen Listeneintrag synchronisiert.

## 6.15 Erkennung personenbezogener Daten

Die Schaltfläche `Personendaten` analysiert das gesamte Dokument und zeigt die Ergebnisse im Tab `Gefunden` an.

- Diese Funktion benötigt das passende PII-Plugin. Laden Sie es von der [Produktseite](https://misart.pl/anonpdfpro/?lang=de#pii), entpacken Sie es im Anwendungsordner `plugins` und starten Sie die Anwendung neu. Warten Sie auf die Initialisierung und die Dokumentanalyse.
- Unabhängig vom Modul bleibt die Suche nach Identifikatoren (PESEL, Grundbuchnummer, NIP, REGON, KRS, Bankkonto, Ausweis, E-Mail, Postleitzahl, Internetadresse, VIN) aus Abschnitt 4.3 verfügbar.
- Die Ergebnisse können im Tab `Gefunden` gesammelt in Anonymisierungsmarkierungen umgewandelt werden.

## 6.16 Alternativtext (ALT)

Die Anwendung erkennt personenbezogene Daten auch im Alternativtext (Beschreibungen) von Bildern und Dokumentelementen.

- Solche Ergebnisse erscheinen im Tab `Gefunden` in der Kategorie `ALT-Text`.
- Das Kontextmenü (Rechtsklick) bietet `Alternativtext bearbeiten` und `Alternativtext löschen`. Änderungen bleiben in der exportierten PDF erhalten.
- Bildbeschreibungen lassen sich auch über `Werkzeuge` bearbeiten. ALT-Beschreibungen oder ein PDF/A-Export allein machen das gesamte Dokument nicht automatisch WCAG-konform.

## 6.17 Text aus Markierungen kopieren

Über das Kontextmenü einer Anonymisierungsmarkierung können Sie `In Zwischenablage kopieren` wählen, um den von der Markierung erfassten Text zu kopieren.

- Das funktioniert sowohl für nativen Text als auch für reine Scans (Text aus OCR).
- Der Text wird in natürlicher Lesereihenfolge zusammengesetzt (von oben nach unten, von links nach rechts), auch bei gedrehten Seiten.

## 6.18 Landauswahl

Im Menü `Optionen -> Land` können Sie das Land des Dokuments festlegen: `Automatisch`, `Polen` oder `Deutschland`.

- Die Einstellung passt die zur Anonymisierung vorgeschlagenen Datenbereiche an das gewählte Land und die Sprache an.
- `Automatisch` verwendet die Regionseinstellungen des Systems und die erkannte Dokumentsprache.

## 6.19 JPEG-2000- und JBIG2-Bilder

Die Anonymisierung unterstützt JPEG 2000 (JP2/JPX) und Schwarz-Weiß-Scans im JBIG2-Format ohne vorherige Umwandlung. Komprimierung und Ausgabeoptimierung beeinflussen weiterhin die Größe der gespeicherten PDF-Datei.

## 6.20 Schnellere erneute Durchsicht

Die Analyseergebnisse des Dokuments (einschließlich OCR-Text) werden in einem Festplatten-Cache gespeichert. Dadurch ist das erneute Öffnen derselben Datei schneller und erfordert keine erneute vollständige Analyse.

## 6.21 Seiten organisieren

Mit `Datei -> Seiten organisieren` ändern Sie die Seitenanordnung des geöffneten Dokuments.

Die Miniaturen sind von links nach rechts und danach in weiteren Zeilen angeordnet. Beim Ziehen zeigt eine senkrechte Linie die Einfügeposition an. Die Miniaturen zeigen auch hinzugefügte Objekte, selbst auf ursprünglich leeren Seiten.

- `Seiten aus PDF hinzufügen` ermöglicht die Auswahl von Seiten aus einem anderen Dokument und das Einfügen vor oder nach der ausgewählten Seite.
- `Leere Seite hinzufügen` bietet Format, Ausrichtung und eigene Abmessungen in Millimetern. Standardmäßig wird das Format der ausgewählten Seite verwendet.
- Ändern Sie die Reihenfolge durch Ziehen oder mit den Schaltflächen zum Verschieben. Mehrere ausgewählte Seiten lassen sich gemeinsam verschieben.
- In langen Dokumenten wählen Sie die Seiten aus und klicken auf `Verschieben`. Geben Sie die Zielseitennummer ein und wählen Sie vor oder nach dieser Seite. Die Nummer darf die aktuelle Seitenzahl einschließlich hinzugefügter Seiten nicht überschreiten. Anschließend werden die verschobenen Seiten angezeigt. `Vorherige Ansicht` neben `Verschieben` wechselt die Ansicht zwischen der letzten Ausgangs- und Zielposition, ohne Reihenfolge oder Auswahl der Seiten zu ändern.
- Das Papierkorbsymbol markiert Seiten des aktuellen Dokuments zum Entfernen beim Speichern des fertigen PDFs, wie die bisherige Funktion zum Entfernen von Seiten. Markierte Miniaturen sind durchgestrichen. Ein erneuter Klick für markierte Seiten hebt die Markierung auf. In diesem Dialog neu hinzugefügte Seiten werden sofort aus der Anordnung entfernt.
- Nach `Übernehmen` bleiben Markierungen, Objekte, Drehungen und ALT-Bearbeitungen ihren Seiten zugeordnet. Die Änderungen erscheinen auch in der Vorschau, beim Drucken und im gespeicherten PDF.

Änderungen können rückgängig gemacht werden. Das Projekt und die Funktion zum Fortsetzen speichern das geänderte Dokument einschließlich der eingefügten Seiten. Die Original-PDF-Dateien werden beim Organisieren nicht geändert.

**Wichtig:** nach einer Neuordnung enthält die `.app`-Projektdatei auch das PDF vor der Anonymisierung. Geben Sie sie nicht anstelle des fertigen anonymisierten PDFs weiter. Eine andere Seitenanordnung macht digitale Signaturen im geänderten Dokument ungültig, nicht in den Originaldateien.

Wählen Sie einzelne Seiten mit `CTRL` und Klicks oder einen Bereich mit `SHIFT`. `Übernehmen` bestätigt die Änderungen am Dokument, `Abbrechen` verwirft sie. Miniaturen werden für den sichtbaren Bereich und benachbarte Zeilen vorbereitet, um den Speicherbedarf großer Dokumente zu begrenzen.

## 6.22 Dokumenteinstellungen

Unter `Datei -> Dokumenteinstellungen` geben Sie obere, untere, linke und rechte Ränder in ganzen Millimetern ein. `0` bedeutet keinen Rand. Der Inhalt jeder Seite wird proportional in die verbleibende Fläche eingepasst, etwa für einen freien Kopfbereich. Vorschau, Druck und die fertige PDF-/PDF/A-Datei zeigen das Ergebnis. Die Einstellungen werden im Projekt gespeichert und beim Fortsetzen wiederhergestellt.

## 6.23 Mehrere Dokumente automatisch bearbeiten

Ein gespeichertes Projekt kann als Vorlage für gleiche Markierungen, Überschriften, Stempel und andere Objekte in ähnlichen Dokumenten dienen. Eine `.cmd`-Datei ermöglicht die Ausführung ohne Anwendungsfenster:

```bat
start "" /wait AnonPDFPro.exe "dokument.pdf" ^
  --headless --template "vorlage.app" ^
  --pdfout "ergebnis.pdf" --lang de
```

- `--template` verwendet die Projektanordnung, ignoriert aber deren gespeicherten PDF-Pfad. Geben Sie das Eingabe-PDF getrennt an.
- `--overwrite` erlaubt das Überschreiben einer vorhandenen Ausgabedatei. Ohne diese Option bleibt sie geschützt.
- `--text` aktiviert die Indizierung nativen Texts; `--ocr` zusätzlich die Erkennung von Seiten ohne Text. Standardmäßig läuft diese Zusatzanalyse nicht. Das PII-Plugin wird in diesem Modus nicht geladen.
- `--format pdfa` wählt PDF/A und aktiviert die erforderliche OCR automatisch. Standard ist normales PDF.
- `--skip-template-validation` überspringt den normalen Vergleich von Seitenzahl, Seitengrößen und Drehungen mit der Vorlage, etwa bei unterschiedlich vielen Folgeseiten. Markierungen werden dabei nicht verschoben oder skaliert: Prüfen Sie selbst, ob sie die richtigen Bereiche erfassen.
- `--help --lang de` zeigt alle Parameter und Rückgabecodes. Verfügbare Meldungssprachen sind `pl`, `en` und `de`.

Prüfen Sie repräsentative Ergebnisse, bevor Sie die gesamte Dateigruppe bearbeiten. Verwenden Sie Vorlagen für Dokumente mit gleicher Seitenanordnung.

## 6.24 Fußnoten zu Ausschlüssen

Legen Sie unter `Optionen` die `Stelle oder Person, die Ausschlüsse vornimmt` fest und prüfen Sie das Wörterbuch der Rechtsgrundlagen. Wählen Sie dann im Kontextmenü einer Markierung den Ausschlussbereich und die Rechtsgrundlage. Die Option für automatische Fußnoten fügt diese der Ausgabe hinzu.

Das globale Wörterbuch wird vom Administrator gepflegt; Benutzer können lokale Einträge verwalten. Die mitgelieferten Rechtsgrundlagenauszüge sind Beispiele, die die Organisation ergänzen und aktuell halten muss. Prüfen Sie Fußnoten vor der Veröffentlichung immer in der Vorschau.

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
- Eingebaute `Vorschau` im Menü `Datei` (`CTRL+F2`), die das Dokument nach der Anonymisierung ohne externen PDF-Viewer anzeigt.
- `Seiten organisieren`: Seiten aus anderen PDFs und leere Seiten einfügen, Gruppen durch Ziehen oder über eine Zielseitennummer verschieben sowie `Vorherige Ansicht`.
- `Dokumenteinstellungen` mit im Projekt gespeicherten Rändern.
- `Ganze Wörter` für Box-Markierungen und automatische Bearbeitung mehrerer Dokumente anhand einer Projektvorlage.
- Option `Seitenbereich als PDF speichern` im Menü `Datei`, standardmäßig mit der aktuellen Seite vorbelegt.
- Option `Seitenbereich als JPEG speichern` im Menü `Datei`, die ausgewählte Seiten als nummerierte `.jpg`-Dateien in einem separaten Ordner speichert.
- Speichern vollständig gescannter Dokumente als PDF/A mit unsichtbarer OCR-Textebene sowie Merken des zuletzt gewählten Speichertyps für Scans.
- Vier Tabs auf der rechten Seite: `Seitenliste`, `Miniaturen`, `Ebenen` und `Gefunden`.
- Dynamische Erzeugung von Miniaturen sowie Miniatur-Cache für bessere Leistung bei großen Dokumenten.
- Verbesserte Aktualisierung der Miniaturen nach Änderung des Seitenfilters.
- Miniaturen, die zusätzlich Markierungen, Kommentare, Objekte und zum Löschen markierte Seiten anzeigen.
- Textsuche in gescannten Dokumenten und in Bildern, die in PDFs eingebettet sind.
- Erkennung personenbezogener Daten und Identifikatoren auch in gescannten Dokumenten.
- Umwandlung von Suchtreffern aus gescannten Seiten in Anonymisierungsmarkierungen.
- `Cursor`-Modus zum Anonymisieren von Wörtern per Klick und zum Auswählen vorhandener Markierungen und Objekte.
- Kontrastreiche Themen: Schwarz-Gelb und Schwarz-Weiß.
- `Aktuelle Ansicht speichern...` im Menü `Datei`.
- Speicherung des zuletzt gewählten rechten Tabs (`Seitenliste` / `Miniaturen` / `Ebenen`).
- Speicherung der Breite des rechten Panels zwischen Programmstarts.
- Speicherung und Wiederherstellung der Scroll-Positionen in den rechten Panels (`Seitenliste` und `Miniaturen`) beim Fortsetzen eines Projekts.
- Unterstützung für Arbeitsebenen mit eigener Sichtbarkeit, Sperre und aktiver Ebene.
- Erweiterte Kontextmenüs für Markierungen und Objekte (Kopieren, Ausschneiden, Duplizieren, duplizierte Kopien löschen).
- Skalierung einzelner Objekte und Objektgruppen über Eckgriffe (`SHIFT` - proportional, `ALT` - vom Mittelpunkt).
- Die Option `Pfeil` im Textdialog mit einstellbaren Abmessungen, Füll- und Randfarben sowie Randstärke.
- Kopieren und Einfügen von Objekten über die Systemzwischenablage, auch zwischen zwei laufenden Instanzen der Anwendung, mit erhaltener Stapelreihenfolge.
- `Rückgängig` (`CTRL+Z`) und `Wiederholen` (`CTRL+Y`).
- Kurzes Start-Tutorial über `? -> Wie fange ich an?`.
- Einen neuen Tab `Gefunden` mit Suchergebnissen und erkannten Personendaten, Kontrollkästchen, Kategoriefilter und Sprung zur Seite.
- Eine Schaltfläche `Personendaten` zur Erkennung personenbezogener Daten (die Erkennung von Namen und Orten erfordert ein optionales Modul für das gewählte Land).
- Erkennung und Bearbeitung personenbezogener Daten im Alternativtext (ALT) von Bildern und Elementen.
- Gruppierung von Text außerhalb der Seite in der Kategorie `Außerhalb`.
- Auswahl des Dokumentlands im Menü `Optionen -> Land`, die die Datenbereiche für die Anonymisierung anpasst.
- Kopieren von Text aus Markierungen in die Zwischenablage (`In Zwischenablage kopieren`) in korrekter Lesereihenfolge, auch bei gedrehten Seiten und reinen Scans.
- Graue Vorschau des betroffenen Textes auch bei reinen Scans (auf Basis von OCR).
- Speichern eines ausgewählten Seitenbereichs in nummerierte JPEG-Dateien.
- Unterstützung der Anonymisierung von JPEG-2000- und JBIG2-Bildern.
- Schnellere erneute Durchsicht des Dokuments dank Festplatten-Cache.
