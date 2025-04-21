$Path = "$PSScriptRoot/Shoc.Shared.proj"
$XPath = "//Version"
$VersionNode = Select-Xml -Path $Path -XPath $XPath 
Write-Host $VersionNode.Node.InnerText