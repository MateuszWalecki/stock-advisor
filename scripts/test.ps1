$projects=@("StockAdvisor.Tests.Unit","StockAdvisor.Tests.EndToEnd")

foreach($project in $projects) {
  Write-Output "Running tests for: $project"
  dotnet test tests/$project/$project.csproj
}