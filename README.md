# QuizGame: 3D Real-Time Strategy & Trivia

[![Watch the Gameplay on YouTube](https://img.shields.io/badge/YouTube-Watch_Full_Gameplay-red?style=for-the-badge&logo=youtube)](https://youtu.be/VZfORJZIKtY)

QuizGame is an interactive, real-time multiplayer trivia and strategy game. Inspired by territory-conquest games, it combines fast-paced quiz mechanics with a rich, fully animated 3D simulation. The project relies on a robust multi-tier architecture, featuring a central Node.js server, a 3D desktop "Host" application, and lightweight 2D mobile clients for the players.

![Full Map Overview](Docs/Map.png)  

![Terrain](Docs/Terrain.png)  

## Game Mechanics

The core loop revolves around strategic planning and real-time trivia battles:

* **Lobby & Room Management:** The "Host" (Game Master) creates a room using the desktop client and selects a custom or public "Question Bank". Players join the waiting room via their mobile devices using a unique 6-digit Room ID.
* **Territory Conquest:** The Host navigates a 3D map and selects one of the 15 attackable villages. The villages are connected via a directed graph; successfully conquering a village unlocks adjacent territories for future attacks.
* **The Quiz Phase:** Upon attacking, the server broadcasts the current question to all connected mobile clients. Players must select the correct answer before the timer runs out.
* **Dynamic Battle Simulation:** Once answers are collected, the 3D battle simulation begins. The outcome is entirely dependent on the percentage of correct answers submitted by the players. 
    * If enough players answer correctly, the attackers win.
    * If the players fail, the defenders win.
    * The fight ends when the losing side's soldiers are fully eliminated, followed by a victory dance from the surviving troops.

![Battle Screen](Docs/Battle.png)  

![Victory Screen](Docs/Victory.png)  

![Defeat Screen](Docs/Defeat.png)  

---

## Server & Communication Architecture

The backend is engineered for high performance, utilizing a hybrid communication model on a single HTTP server instance:

* **Database & ORM:** Persistent data (user accounts, question banks, rooms) is stored in a MySQL database running inside a fully isolated Docker container. Database interactions are safely managed using the Prisma ORM.
* **REST API:** Handles traditional, one-way client requests such as JWT-based authentication, user registration, and CRUD operations for the Question Banks.
* **WebSocket (Socket.IO):** Powers the real-time, bi-directional engine of the game. It handles room synchronization, instant question broadcasting to mobile clients, and the millisecond-accurate collection of answers.

![Host UI - Login](Docs/Login.png)  
![Host UI - Question Banks](Docs/QuestionBanks.png)  
---

## Graphics, UI, and Simulation

Great care was taken to separate the simulation logic from the visual representation, strictly adhering to SOLID principles and the MVP (Model-View-Presenter) architecture.

### Desktop Host Client (Unity 3D)
* **Rich Environment:** The 3D map is divided into distinct biomes (e.g., pine forests, deserts, winter landscapes) featuring dynamic water simulation and 15 custom-designed settlements.
* **Cinematic Camera & VFX:** Utilizes Unity's Cinemachine for smooth camera interpolation between the global map and village-specific orbits. Global Post-Processing is used to simulate Depth-of-Field (blur) effects when UI panels are active.
* **Advanced Character Controllers:** The battle simulation features rigged characters (knights, plague doctors, zealots) with state-machine-driven animations for running, attacking, dying, and dancing. 

![Desert Village](Docs/Desert.png)  

![Forest Village](Docs/Forest.png)  

![Snow Villages](Docs/Snow.png)  

### Mobile Client (Unity 2D)
* **Network & Room Connection:** The mobile application connects to the active game room over the network using the unique Room ID. It maintains a continuous connection to synchronize with the current state of the match.
* **Real-Time Quiz Interactions:** During the game, the client receives the active question directly from the server, displays the available options, and instantly transmits the player's selected answer back to the server for evaluation.
* **UI Toolkit:** Just like the desktop Host, the mobile interface was built entirely using Unity's modern UI Toolkit. It provides a clean, responsive, and state-independent 2D interface tailored for fast and clear interactions.

![Mobil](Docs/Mobil.jpg)  

---

## Getting Started

The system consists of three main components that need to be set up: the Node.js Server, the Windows Host application, and the Android Mobile client.

### 1. Server (Node.js + Docker)
The server handles authentication, question banks, game logic, and real-time WebSocket communication. The MySQL database runs in an isolated Docker container.

**Prerequisites:**
* Node.js & npm
* Docker Desktop

**Installation & Execution:**
1. Extract the server folder.
2. Open a terminal in the directory and run `npm install` to install all dependencies.
3. Start Docker Desktop.
4. Launch the MySQL database container (using Docker Compose).
5. Start the server (preferably from an administrator command prompt) by running `npm run dev`.

**Public Access (Optional):**
If the Host or Mobile clients need to connect from outside your local network, you can expose the server using a tool like ngrok:
`ngrok http 3000`

### 2. Desktop Host Client (Windows)
This is the main application for the Game Master, containing the admin dashboard and the heavy 3D simulation.

**System Requirements:**
* Windows 10/11
* Dedicated GPU (NVIDIA RTX or equivalent is highly recommended; integrated GPUs may struggle with the 3D rendering)
* At least 4 GB RAM and 1 GB of free disk space

**Installation:**
1. Extract the provided Host application folder.
2. Run `QuizGameHost.exe` to launch the application.

### 3. Mobile Client (Android)
This lightweight 2D application is used by the players to join the lobby and submit answers.

**System Requirements:**
* Android 8.0 or newer
* A stable Wi-Fi or mobile internet connection
* ~200 MB of free storage

**Installation:**
1. Transfer the provided `.apk` file to your Android device.
2. Allow installation from "Unknown Sources" in your device's security settings.
3. Install the APK and launch the app to join a room via the Room ID.
