Mini Projet de Gestion Commerciale

Application de gestion commerciale permettant de gérer les clients, les produits et les commandes, avec calcul automatique des totaux et gestion du stock.

Stack technique


Back-end : .NET 8 (ASP.NET Core Web API)
Front-end : Angular 19
Base de données : SQL Server (Express)
ORM : Entity Framework Core


Prérequis d'installation

Avant de lancer le projet, assurez-vous d'avoir installé :


.NET 8 SDK
Node.js (version LTS recommandée) et npm
Angular CLI : npm install -g @angular/cli
SQL Server Express (ou une autre instance SQL Server accessible)
Un IDE tel que VS Code


Vérifiez vos installations avec :

bashdotnet --version
node --version
npm --version
ng version

Étapes pour lancer le back-end


Ouvrir un terminal dans le dossier backend/GestionCommerciale.Api
Configurer la chaîne de connexion dans appsettings.json si nécessaire (par défaut, elle pointe vers une instance locale SQLEXPRESS) :


json"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GestionCommercialeDb;Trusted_Connection=True;TrustServerCertificate=True"
}


Restaurer les dépendances :


bashdotnet restore


Appliquer les migrations pour créer la base de données :


bashdotnet ef database update


Si dotnet ef n'est pas reconnu, installez l'outil globalement :

bashdotnet tool install --global dotnet-ef




Lancer l'API :


bashdotnet run


L'API sera disponible sur http://localhost:5177 (le port exact s'affiche dans le terminal). La documentation Swagger est accessible sur :


http://localhost:5177/swagger

Étapes pour lancer le front-end


Ouvrir un terminal dans le dossier frontend/gestion-commerciale (ou le nom de votre dossier Angular)
Installer les dépendances :


bashnpm install


Vérifier que l'URL de l'API dans les fichiers services (src/app/services/*.service.ts) correspond bien au port du back-end (par défaut http://localhost:5177/api/...)
Lancer l'application :


bashng serve


Ouvrir un navigateur sur :


http://localhost:4200

Informations de connexion

L'authentification n'a pas été implémentée dans cette version du projet — l'accès aux pages et à l'API est libre, conformément au cahier des charges (l'authentification étant optionnelle/bonus).

Structure du projet

GestionCommerciale/
├── backend/
│   └── GestionCommerciale.Api/
│       ├── Controllers/     # Endpoints REST (Clients, Products, Orders)
│       ├── Models/          # Entités (Client, Product, Order, OrderLine)
│       ├── DTOs/             # Objets de transfert de données
│       ├── Services/        # Logique métier
│       ├── Data/             # DbContext EF Core
│       └── Migrations/      # Migrations de base de données
├── frontend/
│   └── gestion-commerciale/
│       └── src/app/
│           ├── pages/        # Clients, Produits, Commandes
│           ├── services/    # Services HTTP Angular
│           └── models/      # Interfaces TypeScript
└── README.md

Fonctionnalités principales


Clients : liste, création, modification, suppression, consultation du détail
Produits : liste, création, modification, suppression, gestion du stock
Commandes : création avec plusieurs lignes de produits, calcul automatique du total HT/TTC (TVA 19%), validation avec mise à jour automatique du stock, consultation du détail


Règles de gestion implémentées


Une commande ne peut pas être créée sans client
Une ligne de commande ne peut pas avoir une quantité ≤ 0
Impossible de commander une quantité supérieure au stock disponible
Le stock est automatiquement décrémenté lors de la validation d'une commande
Le total TTC est calculé avec une TVA fixe de 19%
