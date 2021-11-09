param ($TenantId, $ClientId, $ClientSecret, $TFRegistryURL)

$authUrl = "https://login.microsoftonline.com/$TenantId/oauth2/token"
Write-Output $authUrl
$form = @{
  grant_type    = 'client_credentials'
  client_id     = $ClientId
  client_secret = $ClientSecret
  resource      = $TFRegistryURL
}
Write-Output $form
$authResult = Invoke-RestMethod -Uri $authUrl -Method Post -Form $form
Write-Output $authResult
$url=[System.Uri]$TFRegistryURL
$terraformrc = @"
credentials "$($url.Host)" {
  token = "$($authResult.access_token)"
}
"@
Write-Output $terraformrc
Set-Content -Path ~/.terraformrc -Value $terraformrc
Write-Output $(Get-Content ~/.terraformrc)