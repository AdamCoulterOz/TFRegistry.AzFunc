Install-Module -Name Az.Accounts -Scope CurrentUser -Repository PSGallery -Force

$secureClientSecret = $env:clientSecret | ConvertTo-SecureString -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential ($env:clientId, $secureClientSecret)

Connect-AzAccount -ServicePrincipal -Credential $credential -Tenant $env:tenantId

$token = (Get-AzAccessToken -ResourceUrl $env:registryAuth -TenantId $tenantId).Token

$secureToken = $token | ConvertTo-SecureString -AsPlainText -Force
Invoke-RestMethod -Uri "$env:registryUrl/v1/modules/adam/server/azure" -Method GET -Authentication OAuth -Token $secureToken
