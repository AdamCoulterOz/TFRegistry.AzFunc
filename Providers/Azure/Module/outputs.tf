output "url" {
	value = "https://${var.instance_name}.azurewebsites.net"
}

output "auth_url" {
	value = "api://${azuread_application.terraform.application_id}"
}

output "api_reader_role_id" {
	value = random_uuid.app_role_reader_id.result
}

output "api_contributor_role_id" {
	value = random_uuid.app_role_contributor_id.result
}

output "terraform_app_object_id" {
	value = azuread_application.terraform.object_id
}

output "terraform_app_application_id" {
	value = azuread_application.terraform.application_id
}