# QuizGame

A real-time strategic quiz game where players conquer territories by answering questions correctly.

## SharedAssets setup

1. Navigate to repo root folder
2. Open command line as admin
3. Create symbolic link to SharedAssets (Windows)
   ```
   mklink /D "C:\absoulte\path\to\QuizGame\QuizGameHost\Assets\SharedAssets" "C:\absolute\path\to\QuizGame\SharedAssets"
   mklink /D "C:\absoulte\path\to\QuizGame\QuizGameMobil\Assets\SharedAssets" "C:\absolute\path\to\QuizGame\SharedAssets"
   ```
## Server setup

1. Navigate to the root of the server project
2. Open the command line as admin
3. Start docker-engine (DockerDesktop)
4. Run the command in cmd:
```
npm run dev
```