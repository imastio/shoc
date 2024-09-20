Param(
	[Parameter(Mandatory)]
    [string] $Service
)


function GetCsharpProject ($serviceName) {

	if($serviceName -eq "shoc-database-migrator"){
		return "$PSScriptRoot/apps/shoc-database-migrator/Shoc.Database.Migrator/Shoc.Database.Migrator.csproj"
	}

	if($serviceName -eq "shoc-identity"){
		return "$PSScriptRoot/apps/shoc-identity/Shoc.Identity/Shoc.Identity.csproj"
	}

	if($serviceName -eq "shoc-settings"){
		return "$PSScriptRoot/apps/shoc-settings/Shoc.Settings/Shoc.Settings.csproj"
	}

	if($serviceName -eq "shoc-workspace"){
		return "$PSScriptRoot/apps/shoc-workspace/Shoc.Workspace/Shoc.Workspace.csproj"
	}

	if($serviceName -eq "shoc-registry"){
		return "$PSScriptRoot/apps/shoc-registry/Shoc.Registry/Shoc.Registry.csproj"
	}

	if($serviceName -eq "shoc-cluster"){
		return "$PSScriptRoot/apps/shoc-cluster/Shoc.Cluster/Shoc.Cluster.csproj"
	}

	if($serviceName -eq "shoc-secret"){
		return "$PSScriptRoot/apps/shoc-secret/Shoc.Secret/Shoc.Secret.csproj"
	}

	if($serviceName -eq "shoc-webgtw"){
		return "$PSScriptRoot/apps/shoc-webgtw/Shoc.Webgtw/Shoc.Webgtw.csproj"
	}

	return ""
}

function GetNodeProject ($serviceName) {

	if($serviceName -eq "shoc-admin"){
		return "$PSScriptRoot/ui/shoc-admin/Shoc.Admin/"
	}
	
	if($serviceName -eq "shoc-ui"){
		return "$PSScriptRoot/ui/shoc-ui/Shoc.Ui/"
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
    pnpm --dir $NodeProj build  
    Exit
}
else {
    Write-Host "Not a dotnet or node project. Skipping..."
}

