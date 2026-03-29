## AnonPDF Pro - Instrukcja użytkownika

# 1. Wprowadzenie

AnonPDF Pro to narzędzie służące do częściowego lub całkowitego usuwania (tzw. redakcji) wybranych treści w plikach PDF. Umożliwia oznaczanie obszarów w dokumencie, które mają zostać usunięte, a następnie generuje nową kopię pliku PDF z zastosowanymi redakcjami. Dzięki temu możesz łatwo usunąć wrażliwe dane, takie jak dane osobowe czy informacje poufne, przed przekazaniem plików PDF osobom trzecim.

# 2. Instalacja i uruchomienie

> 1\. Pobierz i zainstaluj aplikację AnonPDF Pro (w zależności od sposobu dystrybucji może to być instalator MSI/EXE lub pliki binarne).

> 2\. Upewnij się, że na komputerze jest zainstalowany .NET Framework / .NET Runtime (wersja zależna od projektu).

> 3\. Uruchom aplikację, np. poprzez skrót na pulpicie, w menu Start lub bezpośrednio przez plik wykonywalny AnonPDFPro.exe

> 4\. Po uruchomieniu powinno otworzyć się główne okno aplikacji.

## 2.5 Aktywacja wersji PRO (z wersji DEMO)

Sposób aktywacji zależy od trybu pracy aplikacji.

### Wersja jednostanowiskowa (standalone)

W tej wersji aktywację wykonuje użytkownik w aplikacji:

1. Uruchom AnonPDF Pro.
2. Wejdź w menu `?` i wybierz opcję `Aktywacja`.
3. Wskaż otrzymaną paczkę plików licencyjnych.
4. Potwierdź operację i uruchom aplikację ponownie.
5. Wejdź w `? -> O aplikacji` i sprawdź status licencji (PRO).

### Wersja sieciowa (central)

W tej wersji użytkownik końcowy nie aktywuje aplikacji z menu.
Aktywację wykonuje administrator:

1. Zamknij aplikację na stanowiskach użytkowników.
2. Rozpakuj paczkę licencyjną.
3. Skopiuj pliki licencyjne do katalogu aplikacji (tam, gdzie znajduje się `AnonPDFPro.exe`).
4. Potwierdź podmianę plików, jeśli system o to zapyta.
5. Uruchom aplikację ponownie i sprawdź `? -> O aplikacji`.

Uwagi:
- Zachowaj oryginalną strukturę i nazwy plików z paczki licencyjnej.
- Jeśli aplikacja nadal działa jako DEMO, sprawdź poprawność lokalizacji plików i uprawnienia do katalogu.

# 3. Interfejs aplikacji

Główne okno aplikacji składa się z następujących elementów:

- Górny pasek menu – zawiera opcje do zarządzania projektem i plikami, m.in.:

> \- Plik: umożliwia otwarcie nowego PDF, zapis/otwarcie projektu zaznaczeń redakcyjnych, wyjście z aplikacji.

> \- ?: wyświetla instrukcję użytkownika oraz informacje o aplikacji.

- Prawy panel – zawiera zakładki `Lista stron`, `Miniatury` i `Warstwy`. Możesz tam szybko przełączać strony, miniatury oraz atrybuty warstw.
- Główny obszar podglądu – prezentuje aktualnie wybraną stronę PDF, umożliwia rysowanie/wybór obszarów do redakcji.
- Pasek narzędzi – zawiera przyciski do nawigacji po stronach, przybliżania/oddalania, zaznaczania obszarów do zaciemniania:

> \- Nawigacja stron: „Pierwsza”, „Poprzednia”, „Następna”, „Ostatnia”.

> \- Nawigacja zaznaczeń: „Pierwsze zaznaczenie”, „Poprzednie zaznaczenie”, „Następne zaznaczenie”, „Ostatnie zaznaczenie”.

> \- Zoom: „Przybliżenie”, „Oddalenie”, „Min (najmniejsze)”, „Max (największe)”.

- Pole tekstowe zmiany strony – służy do szybkiego przejścia do wybranej strony, wprowadzając jej numer.
- Grupa narzędzi redakcji:

> \- Tryb markera – pozwala zaznaczać poziome obszary (np. wąskie pasy do usuwania pojedynczych linii tekstu).

> \- Tryb prostokąta – pozwala zaznaczać dowolny obszar w formie prostokąta.

> \- Przycisk „Wyczyść bieżącą stronę” – usuwa wszystkie zaznaczenia z aktualnie wyświetlanej strony.

> \- Przycisk „Wyczyść wszystkie strony” – usuwa wszystkie zaznaczenia we wszystkich stronach dokumentu.

- Przyciski zapisu/odczytu projektu i generowania PDF:

> \- Otwórz projekt – wczytuje plik projektu .app z zapisanymi obszarami redakcji.

> \- Otwórz ostatni PDF i projekt – wczytuje plik ostatnio opracowywany plik PDF i plik projektu .app

> \- Zapisz projekt – zapisuje bieżące zaznaczenia do aktualnego projektu .app

> \- Zapisz projekt jako – umożliwia wybranie nazwy i lokalizacji dla nowego pliku projektu .app

> \- Zapisz plik PDF (anonimizacja) – generuje nowy plik PDF z usuniętymi obszarami.

# 4. Pierwsze kroki

## 4.1 Otwórz plik PDF

Wybierz z paska menu pozycję Plik → Otwórz plik PDF (lub przycisk „Wybierz plik PDF”), a następnie wskaż docelowy dokument w formacie PDF.

## 4.2 Przeglądanie stron

Użyj listy stron z lewej strony okna (lub przycisków „Następna”, „Poprzednia” itp.), aby wybrać stronę do edycji.

## 4.3 Wyszukiwanie

Aby wyszukać w dokumencie:

> 1\. W lewym panelu wpisz wyszukiwany tekst i naciśnij ENTER, następnie użyj przycisków nawigacyjnych aby poruszać się po wynikach wyszukiwania

> 2\. Użyj przycisków nawigacyjnych, aby poruszać się po wynikach wyszukiwania.

> 3\. Użyj przycisku „PESEL, KW itd.” aby sprawdzić dokument czy zawiera identyfikatory PESEL, numer Księgi Wieczystej lub numer dowodu osobistego

> 4\. Aby usunąć wynik wyszukiwania kliknij przycisk „X”

## 4.4 Zaznaczanie obszarów do redakcji

Aby utworzyć obszar redakcji:

> 1\. Wybierz tryb markera lub prostokąta

> 2\. Naciśnij i przytrzymaj lewy przycisk myszy na podglądzie strony w miejscu, gdzie chcesz zaznaczyć zawartość.

> 3\. W przypadku „markera” rysujesz poziomy pas, a w przypadku „box” (prostokąt) – pełen obszar o dowolnym kształcie prostokątnym.

> 4\. Gdy w trybie „markera” przytrzymasz `CTRL`, przełączysz rysowanie na tryb prostokąta (box).

> 5\. Zwolnij przycisk myszy, aby zatwierdzić zaznaczenie.

> 6\. Aby usunąć istniejące zaznaczenie (np. przypadkowo dodane), kliknij prawym przyciskiem myszy w jego obszarze.

## 4.5 Podgląd po zmianach

Po wprowadzeniu zaznaczenia tekst, który będzie usunięty, jest od razu wyszarzany na podglądzie.

## 4.6 Zapisywanie projektu

Jeśli chcesz zapisać stan zaznaczeń do późniejszej edycji, wybierz Zapisz projekt lub Zapisz projekt jako i wskaż lokalizację pliku .app. Dzięki temu po ponownym otwarciu dokumentu PDF i projektu .app możesz kontynuować nanoszenie kolejnych poprawek.

## 4.7 Anonimizacja PDF

Po zaznaczeniu odpowiednich obszarów wybierz przycisk Zapisz plik PDF (lub z paska menu Plik → Zapisz plik PDF). Wskaż docelowy plik wynikowy, w którym uwzględnione zostaną zaznaczone obszary. Po zakończeniu procesu aplikacja wygeneruje nowy, zredagowany plik PDF.

# 5. Anonimizacja i nawigacja

## 5.1 Nawigacja stron

Użyj przycisków „Pierwsza”, „Poprzednia”, „Następna”, „Ostatnia” lub wpisz numer strony w polu tekstowym i wciśnij Enter. Bieżącą stronę możesz obrócić skrótem `CTRL + R`.

## 5.2 Nawigacja zaznaczeń

Jeśli w dokumencie jest dużo zaznaczonych obszarów, możesz użyć przycisków „Pierwsze zaznaczenie”, „Poprzednie zaznaczenie”, „Następne zaznaczenie”, „Ostatnie zaznaczenie”, aby szybko przechodzić między stronami zawierającymi obszary do redakcji.

## 5.3 Zoom i przewijanie

- Przybliżanie / Oddalanie: użyj przycisków „+” i „−” lub przycisków „Min / Max”, aby automatycznie ustawić minimalne/maksymalne powiększenie.
- Kółko myszy: przewijanie kółkiem myszy powoduje przesuwanie strony w górę i w dół. Jeśli dojedziesz do górnej/dolnej krawędzi, aplikacja automatycznie przełączy się na poprzednią/następną stronę.
- CTRL + kółko myszy: przytrzymanie CTRL powoduje pomniejszanie i powiększanie strony.
- F11 przełącza tryb pełnoekranowy, a ESC zamyka tryb pełnoekranowy.

# 6. Funkcje dodatkowe

## 6.1 Czyszczenie zaznaczeń

- `Wyczyść bieżącą stronę` usuwa wszystkie obszary redakcji z aktualnej strony i wymaga potwierdzenia.
- `Wyczyść wszystko` usuwa wszystkie obszary redakcji z całego dokumentu i również wymaga potwierdzenia.

## 6.2 Podpisy

Aplikacja udostępnia opcje związane z kwalifikowanymi podpisami elektronicznymi:

- Usunięte – usuwa wszystkie podpisy kwalifikowane z dokumentu.
- Oryginalne – pozostawia oryginalne podpisy, jednak z uwagi na zmiany w dokumencie nie będzie można ich zweryfikować.
- Raport – generuje dodatkową stronę w formie listy z informacjami o złożonych podpisach zamiast oryginalnej wizualizacji podpisów.

## 6.3 Usuwanie stron

- Kliknięcie na ikonce „Kosz 1..n” w lewym panelu umożliwia oznaczenie wybranego zakresu stron do usunięcia lub rezygnacji z usunięcia.
- Kliknięcie na ikonce „Kosz” w lewym panelu umożliwia oznaczenie strony do usunięcia. Ikonka zmienia kolor tła na czarny.

## 6.4 Zaczernienie

Domyślnie w zapisanym pliku PDF usuwane są treści, które znajdują się w obrębie zaznaczenia. W przypadku obrazów, grafik lub skanów obszar usuwany jest wypełniany kolorem białym i jest tej samej wielkości co zaznaczenie.

Użycie opcji `Zaczernienie` powoduje, że w zapisanym pliku PDF zaznaczenia będą zaczernione.

## 6.5 Podgląd po zapisaniu

Włączona opcja „Podgląd po zapisaniu” – umożliwi automatyczne otwieranie zapisanego pliku PDF.

## 6.6 Wczytywanie projektu

Możesz w dowolnym momencie załadować gotowy plik .app z opisem redakcji (np. dostarczony przez inną osobę lub utworzony wcześniej), co zaktualizuje zaznaczenia i przypisane dane w aplikacji.

Jeżeli zapisany w projekcie plik PDF nie jest dostępny pod oryginalną ścieżką, aplikacja spróbuje odnaleźć plik PDF o tej samej nazwie w katalogu projektu `.app`.

## 6.7 Zamykanie aplikacji

Podczas zamykania aplikacja może zapytać, czy zapisać zmiany w projekcie ("Zapisz", "Nie zapisuj", "Anuluj").

## 6.8 Menu \[?\] - pomoc 

- Menu \[?\] wyświetla instrukcję użytkownika oraz okno "O aplikacji".
- Opcja `Jak zacząć?` uruchamia krótki samouczek startowy.
- O aplikacji - wyświetla informacje o wersji, autorze i prawach autorskich.

## 6.9 Dodawanie i obsługa obiektów

Obiekty możesz dodawać z menu `Narzędzia` lub skrótami:

- `CTRL + SHIFT + A` - dodaj strzałkę.
- `CTRL + F` - dodaj kształt.
- `CTRL + G` - dodaj obraz.
- `CTRL + K` - dodaj komentarz.
- `CTRL + A` - zaznacz wszystkie widoczne obiekty na aktywnej warstwie bieżącej strony.

Podczas pracy z obiektami dostępne są także:

- `CTRL + C` - kopiuj aktywny obiekt lub grupę aktywnych obiektów.
- `CTRL + V` - wklej obiekt ze schowka. Wklejony obiekt trafia na aktywną warstwę, zachowuje kolejność nakładania ze źródła i może zostać automatycznie przeskalowany, jeśli strona docelowa jest mniejsza.
- Kopiowanie i wklejanie obiektów działa również między dwoma uruchomionymi instancjami aplikacji.
- `DELETE` - usuń aktywny obiekt lub grupę aktywnych obiektów.
- W dialogu napisów można włączyć opcję `Strzałka` dla ramki tekstu.
- Opcję `Ogranicz widok rastra` w dialogu kształtów dla prostokąta, elipsy, trójkąta i regionu.
- Dla strzałki napisu można ustawić `Grubość linii strzałki`, `Długość grotu` i `Szerokość grotu`.
- Koniec strzałki napisu można przeciągać myszą, a punkt przypięcia do ramki aktualizuje się automatycznie do narożników i środków boków.
- Obrót napisu nie obraca końca strzałki; aktualizuje się tylko miejsce przypięcia do ramki.
- Skalowanie napisu skaluje także ramkę i przypiętą strzałkę jako jeden obiekt.

- W dialogu kształtów dla prostokąta, elipsy, trójkąta i regionu dostępna jest opcja `Ogranicz widok rastra`.
- Opcja nie służy do anonimizacji. Ogranicza tylko widoczny obszar rastrów znajdujących się pod kształtem na tej samej warstwie.
- W podglądzie fragment rastra wewnątrz kształtu pozostaje wyraźny, a obszar poza nim jest przygaszony. W finalnym PDF pozostaje tylko obszar ograniczony kształtem.
- Podczas rysowania kształtów wielopunktowych aplikacja pokazuje w overlayu krótką instrukcję. Lewy przycisk myszy dodaje kolejny punkt, a prawy kończy rysowanie.

Modyfikatory podczas pracy z obiektami:

- `ALT` podczas tworzenia kształtów wymusza proporcje: prostokąt -> kwadrat, elipsa -> okrąg, trójkąt -> równoboczny.
- `ALT` podczas zmiany rozmiaru obrazu działa symetrycznie względem środka obiektu.
- Napisy, strzałki i kształty można skalować narożnymi uchwytami.
- `SHIFT` podczas skalowania zachowuje proporcje obiektu.
- `ALT` podczas skalowania skaluje obiekt względem środka.
- `SHIFT + ALT` podczas skalowania zachowuje proporcje i pozostawia środek obiektu w tym samym miejscu.

## 6.10 Powielanie zaznaczeń i obiektów

W menu kontekstowym (prawy przycisk myszy) dostępne są funkcje:

- `Powiel zaznaczenie` - dla zaznaczeń anonimizacji.
- `Powiel obiekt` - dla obiektów (np. napisy, obrazy, strzałki, kształty, komentarze).

Po wybraniu opcji pojawia się dialog zakresu stron:

- `Od` / `Do` - zakres stron, na których ma zostać utworzona kopia.
- Bieżąca strona źródłowa jest pomijana automatycznie.

Dla obiektów powielonych dostępna jest także opcja:

- `Usuń powielone obiekty` - usuwa kopie z wybranego zakresu stron.

## 6.11 Warstwy

Aplikacja obsługuje warstwy robocze dla anonimizacji i obiektów.

- Zakładka `Warstwy` w prawym panelu pozwala szybko zmieniać aktywną warstwę, widoczność i blokadę.
- W panelu można też łączyć warstwy w grupy po wspólnej nazwie.
- Checkbox grupy pozwala jednym kliknięciem zmienić widoczność albo blokadę wszystkich warstw należących do tej grupy.
- Gdy warstwy w grupie mają różne stany, checkbox grupy pokazuje stan mieszany.
- Dwuklik na nazwie grupy w panelu `Warstwy` pozwala zmienić nazwę grupy.
- `Narzędzia -> Warstwy` lub skrót `CTRL + L` otwierają pełne okno zarządzania warstwami.
- W oknie zarządzania warstwami można przypisać warstwę do grupy przez kolumnę `Grupa`.
- Nowe zaznaczenia i nowe obiekty powstają na aktualnie aktywnej warstwie.
- Wklejane obiekty trafiają na aktywną warstwę, z zachowaniem kolejności nakładania ze źródła.
- Warstwa `Robocza` jest zawsze najwyżej i nie trafia do finalnego PDF.
- Ukryte warstwy nie biorą udziału w interakcji na podglądzie.
- Zablokowane warstwy blokują przesuwanie, edycję, skalowanie i usuwanie obiektów.
- Import projektu zachowuje przypisanie obiektów do warstw.

# 7. Najczęstsze problemy i wskazówki

## 7.1 Nie można zapisać pliku PDF

- Upewnij się, że plik PDF nie jest otwarty w innej aplikacji. System może blokować zapis, jeśli plik jest aktualnie w użyciu.
- Wybierz inną nazwę docelową, aby zapobiec nadpisaniu oryginalnego pliku.

## 7.2 Niepoprawny format pliku projektu (.app)

Pliki .app zawierają wewnętrzny zapis zaznaczeń w postaci JSON. Upewnij się, że otwierasz właściwy plik projektu, który odpowiada wczytanemu plikowi PDF.

- Jeżeli pojawi się komunikat o tym, że projekt zawiera więcej stron, niż liczy PDF, wczytaj plik projektu do właściwego dokumentu PDF.

## 7.3 Brak zaznaczeń / „Zapisz plik PDF” jest nieaktywne

- Jeśli przycisk „Zapisz plik PDF” jest nieaktywny lub nie ma zaznaczeń, sprawdź, czy faktycznie dodałeś jakiś obszar do redakcji.

## 7.4 Wolne działanie

- Przy bardzo obszernych dokumentach lub wielu zaznaczeniach proces redagowania może potrwać dłużej, zwłaszcza na komputerach o niższej wydajności.
- Zapisanie projektu i praca na mniejszych częściach dokumentu może ułatwić zarządzanie pamięcią i przyspieszyć przetwarzanie.

# 8. Podsumowanie

Aplikacja AnonPDF Pro pozwala w szybki i intuicyjny sposób usuwać wybrane obszary w dokumentach PDF. Zapisywanie i ponowne wczytywanie projektów umożliwia etapowe przygotowanie wielu dokumentów. Dzięki temu narzędziu łatwiej zadbasz o ochronę danych wrażliwych w dystrybuowanych materiałach.

# 9. Nowe funkcjonalności

W ostatnich wersjach dodano m.in.:

- Drukowanie z menu `Plik` (`CTRL+P`) z wyborem: bieżąca strona, zakres stron lub cały dokument.
- Opcję `Zapisz zakres stron do PDF` (menu `Plik`) z domyślnym zakresem ustawionym na bieżącą stronę.
- Trzy zakładki po prawej stronie: `Lista stron`, `Miniatury` i `Warstwy`.
- Dynamiczne generowanie miniaturek stron oraz pamięć podręczną miniaturek dla szybszej pracy na dużych dokumentach.
- Miniatury pokazujące również zaznaczenia anonimizacji, komentarze, obiekty oraz strony oznaczone do usunięcia.
- Zapamiętywanie ostatnio wybranej zakładki po prawej stronie (`Lista stron` / `Miniatury` / `Warstwy`).
- Zapamiętywanie szerokości prawego panelu między uruchomieniami aplikacji.
- Zapamiętywanie i odtwarzanie pozycji przewijania w prawych panelach (`Lista stron` i `Miniatury`) po wznowieniu pracy z projektem.
- Obsługę warstw roboczych z własną widocznością, blokadą i aktywną warstwą.
- Rozszerzone menu kontekstowe dla zaznaczeń i obiektów (m.in. kopiowanie, wycinanie, powielanie, usuwanie kopii).
- Skalowanie pojedynczych obiektów i grup obiektów narożnymi uchwytami (`SHIFT` - proporcje, `ALT` - od środka).
- Opcję `Strzałka` w dialogu napisów z regulacją grubości linii strzałki, długości grotu i szerokości grotu.
- Kopiowanie i wklejanie obiektów przez schowek systemowy także między dwoma instancjami aplikacji, z zachowaniem kolejności nakładania.
- Funkcje `Cofnij` (`CTRL+Z`) i `Ponów` (`CTRL+Y`).
- Krótki samouczek startowy dostępny z menu `? -> Jak zacząć?`.
