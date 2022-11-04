# JOURNAL DE DEVELOPPEMENT

## J1 03/11/2022

- Je commence par créer la solution "PKMania" dans mon repo Github local
- Cette solution comprend : 1 projet Web API .NET Core pour le back nommé "PM-Backend", 1 bibliothèque de classe pour la logique business nommée "PM-BLL", 1 bibliothèque de classe pour la gestion de la base de données nommée "PM-DAL" et 1 projet Standalone TypeScript Angular Project pour la partie front nommé "PM-FrontEnd"
- Après avoir fait en sorte que les projets "PM-Backend" et "PM-Frontend" puissent être accessibles en même temps sur le même hébergement ("Définir les projets de démarrage" + proxy.conf.js), j'ai organisé mes projets en créant des sous-répertoires appropriés et en nettoyant un peu les fichiers par défaut
- Pour le front, j'ai créé un module "home" avec la page d'accueil et créé le routing adéquat pour y accéder par défaut
- Suppression des répertoires /bin et /obj du back et "first commit arbo" sur main Github
- Création d'une nouvelle branch 'dev' pour la suite et push de la branch
- maj Trello & Ganttproject
- Ajout d'un nouveau projet à la solution afin de créer la base de données ("Projet de base de données SQL Server") et création des sous-répertoires
- Finalisation du Schéma des Entités & Associations sur base du diagramme de classes
- Mise à disposition du Schéma dans "project-doc" -> "database" -> "schema_entites_associations"
- Rédaction du script de création des tables en T-SQL
- Ajout des contraintes sur les champs
- Publication de la base de données en local ("Pokermania")
- git commit/push de la branch dev
- maj Trello & Ganttproject
  > COMMENTAIRES : bon début de projet, journée bien remplie, pas de réelle difficulté rencontrée, il m'a juste manqué un peu de temps pour le script de post-déploiement mais je vais le faire à mon aise ce soir.

## J2 04/11/2022

- quelques minutes de réflexion afin de voir dans quel ordre je vais attaquer le code
- ajout de 2 champs supplémentaires dans la table 'Members' : 'password' et 'role'
- 'password' car je l'avais tout simplement oublié... ;o)
- 'role' car même si a priori le projet ne tient compte que des joueurs, il est préférable que je prévoie 'admin' pour l'administration que je ferai du site
- adaptation du script de post-déploiement en conséquence
- destruction et reconstruction de la base de données locale avec le nouveau script
- nettoyage /bin & /obj
- commit & push de la branch dev
- merge de dev sur main et commit/push de main
