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
    $Push
)

$AllServices = @(
    "shoc-ui", 
    "shoc-admin"
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


if($SkipBuild){
    Write-Host "Skipping docker build step for $($AllRequested.Count) services"
}
else {    
   & docker "compose" "--profile" "ui" "build" $AllRequested 
}

if($Push){
   & docker "compose" "--profile" "ui" "push" $AllRequested 
}
else {    
    Write-Host "Skipping docker push step for $($AllRequested.Count) services"
}

