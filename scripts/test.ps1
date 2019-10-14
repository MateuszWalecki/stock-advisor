$projects=@("StockAdvisor.UnitTests","StockAdvisor.EndToEndTests")

foreach($project in $projects) {
  Write-Output "Running tests for: $project"
  dotnet test tests/$project/$project.csproj
}