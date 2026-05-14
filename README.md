# 🏥 Cabinet Infirmier

Un application de gestion complète pour un cabinet infirmier, permettant de gérer les infirmiers, patients, visites et actes médicaux.

## 👥 À propos

Ce projet est un travail de groupe réalisé par **4 étudiants**. Il démontre nos compétences en développement backend, gestion de données et collaboration en équipe.

## 🎯 Fonctionnalités

- ✅ **Gestion des infirmiers** - Enregistrement et suivi des infirmiers du cabinet
- ✅ **Gestion des patients** - Création et historique des patients
- ✅ **Gestion des visites** - Suivi des visites aux patients
- ✅ **Gestion des actes** - Enregistrement des actes médicaux réalisés
- ✅ **Gestion des adresses** - Stockage et gestion des coordonnées
- ✅ **Génération de documents** - Export en HTML/XML avec transformation XSLT
- ✅ **Génération de factures** - Calcul et génération des factures patients

## 🛠️ Technologies utilisées

| Technologie | Utilisation |
|-------------|------------|
| **C# .NET 9** | Langage et framework principal |
| **XML** | Stockage et échange de données |
| **XSD** | Validation des schémas XML |
| **XSLT** | Transformation des documents XML |
| **HTML/CSS** | Génération des pages web |
| **JavaScript** | Logique client (factures) |

## 🚀 Installation

### Prérequis
- **.NET 9 SDK** ou supérieur
- Un éditeur compatible (.NET, Visual Studio, VS Code)

### Étapes

```bash
# 1. Cloner le repository
git clone https://github.com/kikflash21/CabinetInfirmier.git
cd CabinetInfirmier

# 2. Restaurer les dépendances
dotnet restore

# 3. Compiler le projet
dotnet build

# 4. Lancer l'application
dotnet run
```

## 📁 Structure du projet

```
CabinetInfirmier/
├── *.cs                    # Classes métier (Patient, Infirmier, Visite, etc.)
├── CabinetDOM.cs          # Gestion DOM XML
├── XMLManager.cs          # Gestionnaire XML
├── XMLUtils.cs            # Utilitaires XML
├── Program.cs             # Point d'entrée
├── data/
│   ├── xml/               # Fichiers XML de données
│   ├── xsd/               # Schémas de validation
│   ├── xslt/              # Feuilles de transformation
│   ├── html/              # Templates HTML
│   ├── css/               # Styles CSS
│   └── js/                # Scripts JavaScript
└── CabinetInfirmier.csproj
```

## 💡 Compétences démontrées

- Programmation orientée objet en C#
- Manipulation et validation de données XML
- Transformation de documents avec XSLT
- Gestion de données métier complexes
- Collaboration et travail en équipe
- Architecture logicielle multi-couches

## 📝 Licence

Ce projet est fourni à titre éducatif.

---

**Contributeurs** : Projet réalisé par 4 étudiants

**Dernière mise à jour** : Mai 2026
