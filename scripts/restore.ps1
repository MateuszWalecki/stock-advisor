$source_projects=@("StockAdvisor.Core","StockAdvisor.Infrastructure","StockAdvisor.Api")
$test_projects=@("StockAdvisor.UnitTests","StockAdvisor.EndToEndTests")

foreach($project in $source_projects) {
  Write-Output "`nRestore: $project"
  dotnet restore src/$project/$project.csproj
}

foreach($project in $test_projects) {
  Write-Output "`nRestore: $project"
  dotnet restore tests/$project/$project.csproj
}

"Restoration finished"