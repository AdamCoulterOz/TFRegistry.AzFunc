output "tenantId" {
	value = data.azuread_client_config.current.tenant_id
}

output "clientId" {
	value = azuread_application.tester.application_id
}

output "clientSecret" {
	value = azuread_application_password.tester_password.value
	sensitive = true
}

output "registryAuth" {
	value = module.infra.auth_url
}

output "registryUrl" {
	value = module.infra.url
}