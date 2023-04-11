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
	
	if($serviceName -eq "shoc-executor"){
		return "$PSScriptRoot/Shoc.Executor/Shoc.Executor.csproj"
	}

	return ""
}

function GetNodeProject ($serviceName) {

	if($serviceName -eq "shoc-ui"){
		return "$PSScriptRoot/Shoc.Ui/"
	}
	
	return ""
}

Write-Host "Detecting service type..."

$CsharpProj = GetCsharpProject $Service
$NodeProj = GetNodeProject $Service

Write-Host "Trying to detect csproj for the service... "

if(-not [string]::IsNullOrWhiteSpace($CsharpProj)){
    Write-Host "Detected csproj at $CsharpProj to publish..."
    dotnet publish -c Release "$CsharpProj" 
    Exit
} 
elseif(-not [string]::IsNullOrWhiteSpace($NodeProj)) {
	Write-Host "Detected package.json at $NodeProj to publish..."
    yarn --cwd $NodeProj build  
    Exit
}
else {
    Write-Host "Not a dotnet or node project. Skipping..."
}

