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

- Panel boczny / Lista stron – wyświetla listę wszystkich stron załadowanego pliku PDF w formie tekstowej („Strona 1”, „Strona 2”, itd.).
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

> 3\. Użyj przycisku „PESEL, KW ?” aby sprawdzić dokument czy zawiera identyfikatory PESEL, numer Księgi Wieczystej lub numer dowodu osobistego

> 4\. Aby usunąć wynik wyszukiwania kliknij przycisk „X”

## 4.4 Zaznaczanie obszarów do redakcji

Aby utworzyć obszar redakcji:

> 1\. Wybierz tryb markera lub prostokąta

> 2\. Naciśnij i przytrzymaj lewy przycisk myszy na podglądzie strony w miejscu, gdzie chcesz zaznaczyć zawartość.

> 3\. W przypadku „markera” rysujesz poziomy pas, a w przypadku „box” (prostokąt) – pełen obszar o dowolnym kształcie prostokątnym.

> 4\. Gdy w trybie „markera naciśniesz przycisk CTRL to również narysujesz prostokąt o dowolnym kształcie.

> 5\. Zwolnij przycisk myszy, aby zatwierdzić zaznaczenie.

> 6\. Aby usunąć istniejące zaznaczenie (np. przypadkowo dodane), kliknij prawym przyciskiem myszy w jego obszarze.

## 4.5 Podgląd po zmianach

Po wprowadzeniu zaznaczenia po 2 sekundach tekst który będzie usunięty zostanie zaznaczony w kolorze szarym.

## 4.6 Zapisywanie projektu

Jeśli chcesz zapisać stan zaznaczeń do późniejszej edycji, wybierz Zapisz projekt lub Zapisz projekt jako i wskaż lokalizację pliku .app. Dzięki temu po ponownym otwarciu dokumentu PDF i projektu .app możesz kontynuować nanoszenie kolejnych poprawek.

## 4.7 Anonimizacja PDF

Po zaznaczeniu odpowiednich obszarów wybierz przycisk Zapisz plik PDF (lub z paska menu Plik → Zapisz plik PDF). Wskaż docelowy plik wynikowy, w którym uwzględnione zostaną zaznaczone obszary. Po zakończeniu procesu aplikacja wygeneruje nowy, zredagowany plik PDF.

# 5. Zaznaczenia i nawigacja

## 5.1 Nawigacja stron

Użyj przycisków „Pierwsza”, „Poprzednia”, „Następna”, „Ostatnia” lub wpisz numer strony w polu tekstowym i wciśnij Enter.

## 5.2 Nawigacja zaznaczeń

Jeśli w dokumencie jest dużo zaznaczonych obszarów, możesz użyć przycisków „Pierwsze zaznaczenie”, „Poprzednie zaznaczenie”, „Następne zaznaczenie”, „Ostatnie zaznaczenie”, aby szybko przechodzić między stronami zawierającymi obszary do redakcji.

## 5.3 Zoom i przewijanie

- Przybliżanie / Oddalanie: użyj przycisków „+” i „−” lub przycisków „Min / Max”, aby automatycznie ustawić minimalne/maksymalne powiększenie.
- Kółko myszy: przewijanie kółkiem myszy powoduje przesuwanie strony w górę i w dół. Jeśli dojedziesz do górnej/dolnej krawędzi, aplikacja automatycznie przełączy się na poprzednią/następną stronę.
- CTRL + kółko myszy: przytrzymanie CTRL powoduje pomniejszanie i powiększanie strony.

# 6. Funkcje dodatkowe

## 6.1 Czyszczenie zaznaczeń

- Wyczyść bieżącą stronę – usuwa wszystkie obszary redakcji z aktualnej strony.
- Wyczyść wszystkie strony – usuwa wszystkie obszary redakcji ze wszystkich stron wczytanego pliku PDF.

## 6.2 Podpisy

Aplikacja udostępnia opcje związane z kwalifikowanymi podpisami elektronicznymi:

- Usunięte – usuwa wszystkie podpisy kwalifikowane z dokumentu.
- Oryginalne – pozostawia oryginalne podpisy, jednak z uwagi na zmiany w dokumencie nie będzie można ich zweryfikować.
- Raport – generuje dodatkową stronę w formie listy z informacjami o złożonych podpisach zamiast oryginalnej wizualizacji podpisów.

## 6.3 Usuwanie stron

- Kliknięcie na ikonce „Kosz 1..n” w lewym panelu umożliwia oznaczenie wybranego zakresu stron do usunięcia lub rezygnacji z usunięcia.
- Kliknięcie na ikonce „Kosz” w lewym panelu umożliwia oznaczenie strony do usunięcia. Ikonka zmienia kolor tła na czarny.

## 6.4 Wyróżnienie kolorem

Domyślnie w zapisanym pliku PDF usuwane są treści, które znajdują się w obrębie zaznaczenia. W przypadku obrazów, grafik lub skanów obszar usuwany jest wypełniany kolorem białym i jest tej samej wielkości co zaznaczenie.

Użycie opcji „Wyróżnienie kolorem” zmienia kolor zaznaczeń w pliku PDF na czarny.

## 6.5 Podgląd po zapisaniu

Włączona opcja „Podgląd po zapisaniu” – umożliwi automatyczne otwieranie zapisanego pliku PDF.

## 6.6 Wczytywanie projektu

Możesz w dowolnym momencie załadować gotowy plik .app z opisem redakcji (np. dostarczony przez inną osobę lub utworzony wcześniej), co zaktualizuje zaznaczenia i przypisane dane w aplikacji.

## 6.7 Zamykanie aplikacji

Podczas zamykania aplikacja może zapytać, czy zapisać zmiany w projekcie ("Zapisz", "Nie zapisuj", "Anuluj").

## 6.8 Menu \[?\] - pomoc 

- Menu \[?\] wyświetla instrukcję użytkownika oraz okno "O aplikacji".
- O aplikacji - wyświetla informacje o wersji, autorze i prawach autorskich.

## 6.9 Skróty klawiszowe i modyfikatory

CTRL + A - dodaj strzałkę.

CTRL + F - dodaj kształt.

CTRL + G - dodaj obraz.

CTRL + K - dodaj komentarz.

CTRL + R - obróć bieżącą stronę.

CTRL + C - kopiuj (najpierw aktywny obiekt lub grupę obiektów; gdy brak aktywnego obiektu, kopiowany jest tekst z zaznaczeń).

CTRL + V - wklej obiekt lub dane ze schowka.

DELETE - usuń aktywny obiekt (lub grupę aktywnych obiektów).

F11 - przełącz tryb pełnoekranowy, ESC - wyjście z trybu pełnoekranowego.

CTRL + kółko myszy - zmiana powiększenia podglądu.

ALT podczas tworzenia kształtów wymusza proporcje: prostokąt -\> kwadrat, elipsa -\> okrąg, trójkąt -\> równoboczny.

ALT podczas zmiany rozmiaru obrazu działa symetrycznie względem środka obiektu.

W trybie markera przytrzymanie CTRL przełącza rysowanie na tryb prostokąta (box).

## 6.10 Powielanie zaznaczeń i obiektów

W menu kontekstowym (prawy przycisk myszy) dostępne są funkcje:

- `Powiel zaznaczenie` - dla zaznaczeń anonimizacji.
- `Powiel obiekt` - dla obiektów (np. napisy, obrazy, strzałki, kształty, komentarze).

Po wybraniu opcji pojawia się dialog zakresu stron:

- `Od` / `Do` - zakres stron, na których ma zostać utworzona kopia.
- Bieżąca strona źródłowa jest pomijana automatycznie.

Dla obiektów powielonych dostępna jest także opcja:

- `Usuń powielone obiekty` - usuwa kopie z wybranego zakresu stron.

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
