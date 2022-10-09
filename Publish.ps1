Param(
	[Parameter(Mandatory)]
    [string] $Service
)


function GetCsharpProject ($serviceName) {

	if($serviceName -eq "shoc-identity"){
		return "$PSScriptRoot/Shoc.Identity/Shoc.Identity.csproj"
	}
	
	if($serviceName -eq "shoc-builder"){
		return "$PSScriptRoot/Shoc.Builder/Shoc.Builder.csproj"
	}
	
	if($serviceName -eq "shoc-database-migrator"){
		return "$PSScriptRoot/Shoc.Database.Migrator/Shoc.Database.Migrator.csproj"
	}
	
	if($serviceName -eq "shoc-webgtw"){
		return "$PSScriptRoot/Shoc.Webgtw/Shoc.Webgtw.csproj"
	}
	
	return ""
}


Write-Host "Detecting service type..."

$CsharpProj = GetCsharpProject $Service

Write-Host "Trying to detect csproj for the service... "

if(-not [string]::IsNullOrWhiteSpace($CsharpProj)){
    Write-Host "Detected csproj at $CsharpProj to publish..."
    dotnet publish -c Release "$CsharpProj" 
    Exit
} 
else {
    Write-Host "Not a dotnet project. Skipping..."
}

