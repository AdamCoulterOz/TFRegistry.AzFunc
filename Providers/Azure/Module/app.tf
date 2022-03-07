# resource "random_uuid" "app_auth_audience" {}
resource "random_uuid" "app_api_oauth2_id" {}
resource "random_uuid" "app_role_reader_id" {}
resource "random_uuid" "app_role_contributor_id" {}

locals {
  token_audience = "api://${azuread_application.terraform.application_id}"
}

data "azuread_client_config" "current" {}

resource "azuread_application" "terraform" {
  display_name    = var.app_name
  identifier_uris = var.url != null ? [var.url] : null
  logo_image      = filebase64("${path.module}/terraform.png")
  owners          = var.owner_id != null ? [data.azuread_client_config.current.object_id, var.owner_id] : [data.azuread_client_config.current.object_id]
  api {
    oauth2_permission_scope {
      id                         = random_uuid.app_api_oauth2_id.result
      type                       = "User"
      value                      = "user_impersonation"
      enabled                    = true
      admin_consent_description  = "Allow the application to access terraform on behalf of the signed-in user."
      user_consent_description   = "Allow the application to access terraform on your behalf."
      admin_consent_display_name = "Access Terraform"
      user_consent_display_name  = "Access Terraform"
    }
  }
  app_role {
    id                   = random_uuid.app_role_reader_id.result
    value                = "Terraform.Module.Reader"
    display_name         = "Terraform Module Reader"
    description          = "Read and download terraform modules"
    allowed_member_types = ["Application", "User"]
    enabled              = true
  }
  app_role {
    id                   = random_uuid.app_role_contributor_id.result
    value                = "Terraform.Module.Contributor"
    display_name         = "Terraform Module Contributor"
    description          = "Read and write terraform modules in the registry"
    allowed_member_types = ["Application", "User"]
    enabled              = true
  }
  required_resource_access {
    resource_app_id = "00000003-0000-0000-c000-000000000000" # microsoft graph

    resource_access {
      id   = "e1fe6dd8-ba31-4d61-89e7-88639da4683d" # User.Read
      type = "Scope"
    }
    # resource_access {
    #   id   = "df021288-bdef-4463-88db-98f22de89214" # User.Read.All
    #   type = "Role"
    # }
    # resource_access {
    #   id   = "b4e74841-8e56-480b-be8b-910348b18b4c" # User.ReadWrite
    #   type = "Scope"
    # }
  }
  web {
    homepage_url  = var.url != null ? var.url : null
    redirect_uris = var.url != null ? ["${var.url}/.auth/login/aad/callback"] : []
    implicit_grant {
      access_token_issuance_enabled = true
    }
  }
}

resource "azuread_application_pre_authorized" "powershell" {
  application_object_id = azuread_application.terraform.object_id
  authorized_app_id     = "1950a258-227b-4e31-a9cf-717495945fc2" # powershell
  permission_ids        = [random_uuid.app_api_oauth2_id.result]
}

resource "azuread_application_pre_authorized" "azurecli" {
  application_object_id = azuread_application.terraform.object_id
  authorized_app_id     = "04b07795-8ddb-461a-bbee-02f9e1bf7b46" # azure-cli
  permission_ids        = [random_uuid.app_api_oauth2_id.result]
}

resource "azuread_service_principal" "terraform" {
  application_id = azuread_application.terraform.application_id
}
