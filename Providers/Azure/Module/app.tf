resource "random_uuid" "app_api_oauth2_id" {}
resource "random_uuid" "app_role_reader_id" {}
resource "random_uuid" "app_role_contributor_id" {}

resource "azuread_application" "terraform" {
  display_name     = "Terraform"
  identifier_uris  = [var.url]
  owners           = [var.owner_id]
  sign_in_audience = "AzureADMyOrg"
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
      id   = "e1fe6dd8-ba31-4d61-89e7-88639da4683d" # powershell
      type = "Scope"
    }
  }
  web {
    homepage_url  = var.url
    redirect_uris = ["${var.url}/.auth/login/aad/callback"]
    implicit_grant {
      access_token_issuance_enabled = true
      id_token_issuance_enabled     = true
    }
  }
}
