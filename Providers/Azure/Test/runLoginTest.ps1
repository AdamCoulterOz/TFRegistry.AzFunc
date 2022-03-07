$env:ARM_THREEPOINTZERO_BETA_RESOURCES="true"

terraform init
terraform apply -auto-approve

$tenantId = (terraform -chdir=Module output -raw tenantId)
$clientId = (terraform -chdir=Module output -raw clientId)
$clientSecret = (terraform -chdir=Module output -raw clientSecret)
$registryAuth = (terraform -chdir=Module output -raw registryAuth)
$registryUrl = (terraform -chdir=Module output -raw registryUrl)

$secureClientSecret = $clientSecret | ConvertTo-SecureString -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential ($clientId, $secureClientSecret)

Connect-AzAccount -ServicePrincipal -Credential $credential -Tenant $tenantId

$token = (Get-AzAccessToken -ResourceUrl $registryAuth -TenantId $tenantId).Token

$secureToken = $token | ConvertTo-SecureString -AsPlainText -Force
Invoke-RestMethod -Uri "$registryUrl/v1/modules/adam/server/azure" -Method GET -Authentication OAuth -Token $secureToken