--- Containers:

zawiera wszystkie moje natywne pojemniki. container'y Unity są zabezpieczone przez SafetySystem co powoduje, że są uciążliwe w używaniu. Są również nieoptymalne (np. zwracają wartości przez kopię zamiast przez referencję).

Containers/Raw/ zawiera proste container'y:
1. RawArray - zwykły array
2. RawSet - lista nie zachowująca kolejności (stała złożoność O(1) przy usuwaniu)

Containers/Raw/Queue/ zawiera implementacje kopca binarnego (heap) do pathfinder'ów. RawGeoQueue.cs do Dijkstry, RawGeoQueueHeuristic.cs do A*, zrobione konkretnie pod system pathfinder'a do tej gry.

Containers/Raw/Stackalloc/ zawiera pojemniki do alokacji na stosie (stack'u), tzn. nie alokują pamięci tylko przyjmują wskaźnik na stos i długość.

Containers/Memory/ zawiera container MemoryBlock, służący do wydzielania tymczasowej pamięci (podobny do Unity.Allocator.Temp)

Containers/Database/ zawiera duże container'y typu SoA (structure of arrays). strukturą przypominają bazy danych. pola obiektu są przetrzymywane w oddzielnych array'ach, obiekt jest ekwiwalentem rzędu (row) tabli DB, a pola obiektów są ekwiwalentem kolumny tabeli DB.

baz danych jest kilka rodzajów:
1. DatabaseStatic - najprostsza, do danych o stałej ilości (np. pola, rzeki), zawiera tylko tabelę o stałym rozmiarze
2. Database - ogólna, zawiera zarówno tabelę dynamicznego rozmiaru jak i DatabaseIdData. Id służą do referowania na obiekty w bazie danych. jest to moje rozwiązanie na problem referowania na obiekt przez unsafe wskaźnik, w sytuacji kiedy obiekty "poruszają się" po array'ach (np. przez usuwanie innych obiektów list, co powoduje przesunięcie wszystkich innych).
3. DatabaseLookup - modyfikacja Database, każda para obiektów ma unikalne miejsce do przechowywania danych wspólnych (np. relacje dwóch państw). jest to macierz trójkątna, podobna do macierzy kolizji Unity.
4. DatabaseMul - rozwinięcie Database, zamiast 1 tabeli jest wiele tabel, obiekty mogą zmieniać tabele bez utraty ważności referencji Id na nie. ważne gdy np. część obiektów jest wyłączona - obiekty włączone są przechowywane w konkretnej tabeli, nie trzeba przy iteracji po niej sprawdzać każdego obiektu (if obiekt.enabled)

DatabaseColumnsGenerator.cs

bardzo ważny skrypt edytora, pobiera ścieżkę do struktury obiektu i generuje z jego pól strukturę kolumn tego obiektu. robi to przez parse'ing string'a.
żeby zrozumieć lepiej należy spojrzeć na przykładowy obiekt i jego kolumny:

Sim/Field/Field.cs zawiera strukturę pola, zwykła struktura. jest to wiersz bazy danych.

Sim/Field/FieldColumns.cs jest wygenerowany przez skrypt. z każdego pola Field został zrobiony unsafe array. jest to ekwiwalent tabeli bazy danych, a pojedynczy array jest ekwiwalentem kolumny. dodatkowo zawiera funkcje do zbiorowego przetwarzania kolumn (poruszanie, usuwanie, zamiana itd.)

--- Sim

zawiera struktury do przetrzymywania danych gry w czasie wykonywania. Sim.cs to główna struktura, ma w sobie głównie bazy danych. podfoldery zawierają struktury obiektów gry i wygenerowane z nich struktury kolumn.

--- RawDataProcessor

skrypty edytora, służące do otworzenia danych binarnych wygenerowanych przez projekt z generatorami. przetwarza je (głównie kopiuje) do finalnej postaci, tworzy strukturę gry (Sim.cs) i dokonuje zapisu do pliku. zapis gry jest podzielony na 2 części:
1. persistent - stałe, niezmienne dane (const). istnieją tylko 1 na całą grę
2. dynamic - zmienne dane, save gracza. gracz zaczyna grę z pierwotnymi danymi dynamicznymi, później zapisując grę tworzy swoje dane binarne (gracz de facto startuje grę uruchamiając domyślny save startowy)

--- Utility

zawiera wszystkie funkcje do operacji binarnych, również funkcje do konwersji między przestrzeniami w projekcji Mercator'a. najważniejsze pliki:

1. BinaryReadUtility.cs
2. BinarySaveUtility.cs
3. CesMemoryUtility.cs
4. GeoUtilitiesDouble.cs

