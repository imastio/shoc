param (
	[string[]]
    $Services,
	
    [string] 
    $Version="latest",
	
    [string] 
    $Env="dev",
		
	[switch] 
    $SkipPublish,

	[switch] 
    $SkipBuild,
	
	[switch] 
    $SkipPush
)

$AllServices = @(
    "shoc-database-sql", 
    "shoc-database-migrator", 
    "shoc-webgtw", 
    "shoc-identity", 
    "shoc-builder", 
	"shoc-executor", 
    "shoc-ingress",
    "shoc-ui"
)

function IsValidService($service, $all){

	return $all -contains $service
}

$AllRequested = $AllServices

if($Services.Count -gt 0)
{
    $AllRequested = $Services | Where-Object -FilterScript { IsValidService $_  $AllServices }
}

Write-Host "Services selected to be released: $($AllRequested.Count)"
Write-Host "Environment: $Env"
Write-Host "Version: $Version"

$env:SHOC_VERSION=$Version
$env:SHOC_REGISTRY_REPO="shoc/$Env"

foreach($service in $AllRequested){

    if($SkipPublish){
        Write-Host "Skipping publish step for $service";
        continue
    }

    & "$PSScriptRoot/Publish.ps1" "$service"
}

$ComposePath = Get-Command docker-compose | Select-Object -First 1 -ExpandProperty Definition

if($SkipBuild){
    Write-Host "Skipping docker build step for $($AllRequested.Count) services"
}
else {    
   & "$ComposePath" "build" $AllRequested 
}

if($SkipPush){
    Write-Host "Skipping docker push step for $($AllRequested.Count) services"
}
else {    
   & "$ComposePath" "push" $AllRequested 
}

