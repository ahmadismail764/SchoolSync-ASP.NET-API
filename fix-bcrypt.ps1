# PowerShell script to replace BCrypt with Microsoft's secure password hasher
Write-Host "Starting BCrypt replacement process..." -ForegroundColor Green

# Navigate to the solution directory
Set-Location "g:\Uni\SchoolSync-ASP.NET-API"

Write-Host "Removing old BCrypt packages..." -ForegroundColor Yellow

# Remove old BCrypt packages
try {
    dotnet remove SchoolSync.App/SchoolSync.App.csproj package BCrypt
    Write-Host "Removed BCrypt from SchoolSync.App" -ForegroundColor Green
} catch {
    Write-Host "BCrypt not found in SchoolSync.App (might already be removed)" -ForegroundColor Yellow
}

try {
    dotnet remove SchoolSync.App/SchoolSync.App.csproj package BCrypt.Net-Next
    Write-Host "Removed BCrypt.Net-Next from SchoolSync.App" -ForegroundColor Green
} catch {
    Write-Host "BCrypt.Net-Next not found in SchoolSync.App (might already be removed)" -ForegroundColor Yellow
}

try {
    dotnet remove SchoolSync.Infra/SchoolSync.Infra.csproj package BCrypt.Net-Next
    Write-Host "Removed BCrypt.Net-Next from SchoolSync.Infra" -ForegroundColor Green
} catch {
    Write-Host "BCrypt.Net-Next not found in SchoolSync.Infra (might already be removed)" -ForegroundColor Yellow
}

Write-Host "Adding Microsoft's secure password hasher..." -ForegroundColor Yellow

# Add Microsoft's Identity package
try {
    dotnet add SchoolSync.App/SchoolSync.App.csproj package Microsoft.AspNetCore.Identity
    Write-Host "Added Microsoft.AspNetCore.Identity to SchoolSync.App" -ForegroundColor Green
} catch {
    Write-Host "Failed to add Microsoft.AspNetCore.Identity" -ForegroundColor Red
    exit 1
}

Write-Host "Restoring packages..." -ForegroundColor Yellow
dotnet restore

Write-Host "BCrypt replacement completed successfully!" -ForegroundColor Green
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Update your UserService to use Microsoft IPasswordHasher" -ForegroundColor White
Write-Host "2. Register the password hasher in your DI container" -ForegroundColor White
Write-Host "3. Test password hashing and verification" -ForegroundColor White

pause
