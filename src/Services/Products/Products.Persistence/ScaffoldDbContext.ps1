$projectSuffix = 'Products'
$projectBasePath = $args[0]
$projectFullPath = -join($projectBasePath, '\' + $projectSuffix + '.Persistence')
$contextFullPath = -join($projectFullPath, '\Database')
$entitiesSourceFullPath = -join($projectBasePath, '\' + $projectSuffix + '.Persistence\Entities')
$entitiesFolderFullPath = -join($projectBasePath, '\' + $projectSuffix + '.Domain\Entities')
$entitiesDestinationFullPath = -join($projectBasePath, '\' + $projectSuffix + '.Domain')
$dbContextName = 'DbContextRateIt'
$namespaceDBContext = $projectSuffix + '.Persistence.Database.RateIt'
$namespaceEntities = $projectSuffix + '.Domain.Entities'

Write-Host 
Write-Host 'Scaffolding started!'

try {
#dotnet ef dbcontext scaffold "Server=tcp:rateitserver.database.windows.net,1433;Initial Catalog=rateit;Persist Security Info=False;User ID=gmanfredonia;Password=Giangi1974_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer `    
dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Initial Catalog=RateIt;Trusted_Connection=true;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer `
                                 -p $projectFullPath `
                                 -c $dbContextName `
                                 --context-dir $contextFullPath `
                                 --context-namespace $namespaceDBContext `
                                 -o Entities `
                                 --namespace $namespaceEntities `
                                 --no-onconfiguring -f --no-build `
                                 -t Log -t Categories -t Products

    Write-Host 'Scaffolding finished!'
    Write-Host 'Move entities started!'
    if (Test-Path $entitiesFolderFullPath) {
        Remove-Item -Path $entitiesFolderFullPath -Force -Recurse
    }
    Move-Item -Path $entitiesSourceFullPath -Destination $entitiesDestinationFullPath
    Write-Host 'Move entities finished!'
}
catch {
    Write-Host "An error has been occurred: $_"
}


Write-Host
Read-Host 'Press ENTER to continue...'
