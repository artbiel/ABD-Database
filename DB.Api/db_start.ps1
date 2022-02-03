$config = (Get-Content appsettings.json | ConvertFrom-Json).ClusterConfig;
$ids = $config.Nodes | SELECT Id;
foreach($id in $ids)
{
    Start-Process -FilePath "./bin/Release/net6.0/ABDDB.Api.exe" -ArgumentList "$($id.Id)";
}
Write-Host "DB is running...";