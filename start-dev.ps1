# PowerShell script to start both .NET backend and React frontend
Start-Process powershell -ArgumentList '-NoExit', '-Command', 'cd SchoolSync.API; dotnet run'
Start-Process powershell -ArgumentList '-NoExit', '-Command', 'cd SchoolSync.API/clientapp; bun run dev'
