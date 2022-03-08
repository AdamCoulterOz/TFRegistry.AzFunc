Install-Module -Name Az.Accounts -Scope CurrentUser -Repository PSGallery -Force

$secureClientSecret = $env:test_clientSecret | ConvertTo-SecureString -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential ($env:test_clientId, $secureClientSecret)

Connect-AzAccount -ServicePrincipal -Credential $credential -Tenant $env:test_tenantId

$token = (Get-AzAccessToken -ResourceUrl $env:test_registryAuth -TenantId $tenantId).Token

$secureToken = $token | ConvertTo-SecureString -AsPlainText -Force
Invoke-RestMethod -Uri "$env:test_registryUrl/v1/modules/adam/server/azure" -Method GET -Authentication OAuth -Token $secureToken
