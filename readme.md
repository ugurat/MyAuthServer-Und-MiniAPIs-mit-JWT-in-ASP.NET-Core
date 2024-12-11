
# MyAuthServer und MiniAPIs

Die verschiedenen API-Projekte bieten spezialisierte Authentifizierungs- und Autorisierungslösungen, die auf JWT-Token basieren und in ASP.NET Core implementiert sind. AuthServer.API stellt eine umfassende Lösung für Benutzer-Authentifizierung und -Autorisierung bereit, während die MiniApi-Projekte spezifische Zugriffskontrollen wie rollenbasierte, altersbasierte, standortbasierte und gerätebasierte Authentifizierung bieten. Diese APIs nutzen moderne Sicherheitsmechanismen wie RSA-Verschlüsselung und bieten interaktive Dokumentation über Swagger. Die Konfiguration erfolgt hauptsächlich über `appsettings.json` und `launchSettings.json`, und die Integration von Entity Framework Core ermöglicht eine effiziente Datenbankinteraktion.

----

## AuthServer.API

Die `AuthServer.API`-Anwendung bietet eine umfassende Lösung für die Authentifizierung und Autorisierung von Benutzern, indem sie sichere Token-basierte Authentifizierungsmechanismen bereitstellt. Die Anwendung nutzt verschiedene NuGet-Pakete, um Funktionalitäten wie Objektzuordnung, JWT-Authentifizierung, Benutzerverwaltung und API-Dokumentation zu integrieren. Die Konfiguration erfolgt über Dateien wie `launchSettings.json` und `appsettings.json`, die die Startparameter und Token-Optionen definieren. Die Anwendung umfasst verschiedene DTOs zur Datenübertragung, Datenmodelle für die Benutzerverwaltung und eine Datenbankkontextklasse zur Verwaltung der Datenbankoperationen. Die Hauptdatei `Program.cs` konfiguriert die Webanwendung und die Middleware-Pipeline. Controller wie `AuthController`, `PublicKeyController` und `UserController` bieten Endpunkte für Authentifizierungsprozesse, Schlüsselbereitstellung und Benutzerverwaltung. Die Anwendung kann mit Postman getestet werden, um Funktionen wie Registrierung, Login, Token-Erstellung und -Widerruf zu überprüfen.


[AuthServer.API](./AuthServer.API/Readme.md)

----

## MiniApi1.API

Die MiniApi1.API ist eine ASP.NET Core Web-API, die als Client-API für einen zentralen Authentifizierungsserver fungiert. Sie verwendet JWT-Authentifizierung und RSA-Verschlüsselung, um eine sichere Token-basierte Authentifizierung zu gewährleisten. Die API bietet eine interaktive Dokumentation über Swagger und ermöglicht flexible Konfigurationen über die `appsettings.json`. Der Zugriff auf die API ist nur für authentifizierte Benutzer mit einem gültigen JWT-Token möglich, wobei der Endpunkt für Community-Informationen nur für Benutzer mit der Manager-Rolle zugänglich ist. Die Anwendung wird mit verschiedenen NuGet-Paketen ausgestattet, um Funktionalitäten wie Objektzuordnung, JWT-Authentifizierung und Datenbankinteraktion zu unterstützen. Die `launchSettings.json` definiert die Startkonfiguration der Anwendung, während `appsettings.json` die Token-Validierungseinstellungen enthält. Der `PublicKeyService` sorgt dafür, dass der Public Key entweder lokal oder vom AuthServer abgerufen wird. Der `Program.cs`-Code initialisiert die Webanwendung und konfiguriert die JWT-Authentifizierung. Der `CommunityController` bietet einen geschützten Endpunkt, der Benutzerinformationen aus dem Token extrahiert und zurückgibt.

[MiniApi1.API](./MiniApi1.API/Readme.md)


----

## MiniApi2.API

MiniApi2.API ist ein sicheres Web-API-Projekt, das JWT-basierte Authentifizierung und rollenbasierte Autorisierung verwendet. Es bietet geschützte Endpunkte, die nur für Benutzer mit Administrator-Rolle zugänglich sind. Die API integriert Funktionen wie RSA-signierte Token-Validierung, Swagger API-Dokumentation und Entity Framework Core für die Datenbankintegration. Die Konfiguration erfolgt über `appsettings.json`, wo Parameter wie `Audience` und `Issuer` für die Token-Validierung festgelegt sind. Der `PublicKeyService` sorgt dafür, dass der Public Key entweder lokal oder vom AuthServer abgerufen wird. Die Anwendung wird in `Program.cs` initialisiert, wobei die JWT-Authentifizierung konfiguriert und grundlegende Dienste wie Controller und Swagger registriert werden. Der `ManagerController` bietet einen Endpunkt, der Benutzerinformationen aus dem JWT-Token extrahiert und zurückgibt.

[MiniApi2.API](./MiniApi2.API/Readme.md)

----

## MiniApi3.API

MiniApi3.API ist eine Web-API mit altersbasierter Zugriffskontrolle, die JWT-Authentifizierung verwendet. Sie stellt sicher, dass nur Benutzer über 18 Jahre auf bestimmte Endpunkte zugreifen können. Die API bietet Funktionen wie altersbasierte Zugriffskontrolle, JWT-Authentifizierung, automatische Altersberechnung, policy-basierte Autorisierung, RSA-verschlüsselte Token-Validierung, Swagger-Dokumentation und Entity Framework Core Integration. Sie arbeitet eng mit AuthServer.API zusammen, um JWT-Tokens zu beziehen, die das Geburtsdatum des Benutzers als Claim enthalten.

[MiniApi3.API](./MiniApi3.API/Readme.md)

----

## MiniApi4.API

MiniApi4.API ist eine Web-API, die standortbasierte Zugriffskontrolle mit JWT-Authentifizierung bietet. Sie stellt sicher, dass nur Benutzer aus Wien auf bestimmte Endpunkte zugreifen können. Die API nutzt Policy-basierte Autorisierung und RSA-verschlüsselte Token-Validierung. Sie arbeitet eng mit AuthServer.API zusammen, um JWT-Tokens zu beziehen, die den Wohnort des Benutzers enthalten. Die API ist mit Swagger dokumentiert und integriert Entity Framework Core für Datenbankoperationen.

[MiniApi4.API](./MiniApi4.API/Readme.md)

----

## MiniApi5.API

MiniApi5.API ist eine Web-API, die für die Geräteauthentifizierung entwickelt wurde und JWT-basierte Token-Validierung verwendet. Sie ermöglicht es Geräten, sich über spezielle Device-Tokens zu authentifizieren und auf geschützte Ressourcen zuzugreifen. Die API ist für Machine-to-Machine (M2M) Kommunikation optimiert und arbeitet mit einem AuthServer zusammen, der die Device-Tokens ausstellt. Sie bietet eine vereinfachte Autorisierung für Gerätezugriffe und nutzt RSA-verschlüsselte Token-Sicherheit. Die Integration von Swagger ermöglicht eine umfassende API-Dokumentation, während Entity Framework Core die Datenbankinteraktion unterstützt.

[MiniApi5.API](./MiniApi5.API/Readme.md)

----

## MiniApi6.API

Tabellarisch werden die verschiedenen Abschnitte der MiniApi6.API-Dokumentation beschrieben. Die Einführung erklärt, dass es sich um eine moderne Web-API auf Basis von ASP.NET Core handelt, die eine robuste Authentifizierungs- und Autorisierungsinfrastruktur bietet. Der Zugriff erfolgt über User- und Client-Token, die als JWT implementiert sind. Die Installation von NuGet-Paketen ermöglicht die Integration verschiedener Funktionalitäten wie JWT-Authentifizierung, Datenbankzugriff und API-Dokumentation. Die `launchSettings.json`-Datei definiert die Startkonfiguration der Anwendung, während `appsettings.json` die Token-Konfigurationen festlegt. Der `PublicKeyService` ist für das Abrufen und Speichern eines öffentlichen Schlüssels verantwortlich. In `Program.cs` wird die Anwendung konfiguriert, um JWT-Token zu validieren und die Authentifizierung sowie Autorisierung zu aktivieren. Der `InfoController` bietet Endpunkte, die nur für authentifizierte Benutzer oder Clients zugänglich sind.


[MiniApi6.API](./MiniApi6.API/Readme.md)

----




## Git Konto wechseln und commiten

```` bash
git init

touch .gitignore

git add .
git commit -m "Initial commit"

git branch -M main

git remote add origin https://github.com/ugurat/MyAuthServer-Und-MiniAPIs-mit-JWT-in-ASP.NET-Core.git

git push -u origin main
````

Falls Fehler:

```` bash
PASSWORT LÖSCHEN und ERNEUT ANMELDEN

Gehe zu "Windows-Anmeldeinformationen":
Unter Windows-Anmeldeinformationen "gespeicherte Zugangsdaten für Webseiten und Anwendungen" finden.

Suche nach gespeicherten GitHub-Einträgen:
git:https://github.com oder Ähnliches.

Eintrag löschen und erneut versuchen:

git push -u origin main
````

Aktualisiert

```` bash
git add .
git commit -m "aktualisiert"
git push -u origin main
````

Überschreiben

```` bash

git init

git add .
git commit -m "Initial commit"

git branch -M main

git remote add origin https://github.com/ugurat/MyAuthServer-Und-MiniAPIs-mit-JWT-in-ASP.NET-Core.git


git push -u origin main --force

````

Mit dem Parameter `--force` wird Git-Repo überschrieben.

----


## Entwickler
- **Name**: Ugur CIGDEM
- **E-Mail**: [ugurat@gmail.com](mailto:ugurat@gmail.com)

---

## Markdown-Datei (.md)

---


