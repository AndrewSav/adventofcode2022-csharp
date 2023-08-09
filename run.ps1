Get-ChildItem -Directory -Filter "day*" | ForEach-Object {
  Push-Location
  Set-Location $_
  dotnet run
  Pop-Location
}
