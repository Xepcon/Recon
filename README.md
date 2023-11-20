# Diploma Munka

## Project leirása 

Munkaidő nyilvántartó web alkalmazás, egy mágnes kártyás megoldással.
A Program az alábbiakat tudja : 
  - Szerepkörök kezelése
  - Munkaidő nyilvántartása
  - Munkaidő módosítása
  - Kimutatások
  - Szabadságok engedélyezése
  - Mágnes kártya integráció
  - Felhasználók kezelés
  - Jelenléti ív Excelbe való exportálása

#A Program felépítése : 
/
│
├── Tests/ --Az összes teszt itt található meg 
├── Recon/ -- fő mappa 
│ ├── Controllers/ 
│ │ ├── api/ ** api kontrollerek
│ │ ├── ... ** Kontrollerek
│
├── Models/
│ ├── Model/ ** Osztályok
│ │ ├── Account **Felhasználóval kapcsolatos osztályok
│ │ ├── Card **Mágneskártya osztály
│ │ ├── Group **Munkacsoportokkal kapcsolatos osztályok
│ │ ├── Roles **Szerepkörökkel kapcsolatos osztályok
│ │ ├── Ticket **Szabadságolással kapcsolatos osztályok
│ │ ├── TimeManager **Jelenléti ívvel kapcsolatos osztályok
│ │ ├── UsersInRole **Szerepkörök és felhasználó összekapcsoló osztály
│ ├── Interface/ ** Interface-ek
│ │ ├── Account **Felhasználóval kapcsolatos Interfészek
│ │ ├── Card **Mágneskártya Interfészek
│ │ ├── Group **Munkacsoportokkal kapcsolatos Interfészek
│ │ ├── Roles **Szerepkörökkel kapcsolatos Interfészek
│ │ ├── Ticket **Szabadságolással kapcsolatos Interfészek
│ │ ├── UsersInRole **Szerepkörök és felhasználó összekapcsoló Interfészek│
├── Attributes/
│ ├── AuthenticatedAttribute **Authentikációért felelős attríbutum
│ └── RoleAttribute **Authorizációért felelős attríbutom
├── Views/ **Az összes elérhető Viewok




Hosszabb dokumentáció a Documentation.pdf-ben található

