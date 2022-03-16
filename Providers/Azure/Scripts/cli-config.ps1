param ($Token, $TFRegistryURL)

$url=[System.Uri]$TFRegistryURL
$terraformrc = @"
credentials "$($url.Host)" {
  token = "$($Token)"
}
"@
Set-Content -Path ~/.terraformrc -Value $terraformrc