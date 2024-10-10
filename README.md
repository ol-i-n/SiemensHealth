# Directory Processor

## Popis projektu

Tento projekt je jednoduchá C# aplikácia, ktorá načíta informácie o adresári (súbory a podadresáre), serializuje ich do JSON formátu, následne deserializuje a vypíše štruktúru adresára vrátane unikátnych prípon súborov.

## Inštalácia a spustenie

### Požiadavky

- [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) (alebo novšia verzia)
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) (nainštaluje sa cez NuGet)
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) (nainštaluje sa cez NuGet)

### Inštalácia závislostí

Ak používaš .NET CLI, spusti nasledujúce príkazy na inštaláciu potrebných balíkov:

```bash
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Newtonsoft.Json
```
### Spustenie projektu

1. Naklonuj si repozitár alebo skopíruj súbory do lokálneho adresára.
2. Pridaj súbor `appsettings.json` do koreňového adresára projektu. Tento súbor by mal obsahovať základnú cestu k adresáru, ktorý chceš spracovať:

    ```json
    {
      "DirectorySettings": {
        "BaseDirectory": "EXAMPLEFOLDER"
      }
    }
    ```

3. Prejdi do adresára projektu a spusti aplikáciu cez príkaz:

    ```bash
    dotnet run
    ```

### Výstup

- Aplikácia načíta informácie o adresári špecifikovanom v `appsettings.json`.
- Vypíše unikátne prípony súborov nachádzajúcich sa v danom adresári a jeho podadresároch.
- Serializuje informácie o adresári do súboru `directoryInfo.json`.
- Následne deserializuje tento súbor a vypíše štruktúru adresára v konzole.

### Zložky a súbory

- **`Program.cs`**: Hlavný zdrojový súbor aplikácie.
- **`appsettings.json`**: Konfiguračný súbor, ktorý definuje základný adresár pre spracovanie.

## Chyby a problémy

Ak adresár špecifikovaný v `appsettings.json` neexistuje, aplikácia vypíše chybové hlásenie do konzoly.

