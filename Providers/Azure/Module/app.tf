resource "random_uuid" "app_api_oauth2_id" { }
resource "random_uuid" "app_role_reader_id" { }
resource "random_uuid" "app_role_contributor_id" { }

resource "azuread_application" "terraform" {
  device_only_auth_enabled       = false
  display_name                   = "Terraform"
  fallback_public_client_enabled = false
  group_membership_claims        = []
  identifier_uris                = [var.url]
#   logo_image                     = ""
  marketing_url                  = ""
  oauth2_post_response_required  = false
  owners                         = [var.owner_id]
  prevent_duplicate_names        = false
  privacy_statement_url          = ""
  sign_in_audience               = "AzureADMyOrg"
  support_url                    = ""
  terms_of_service_url           = ""
  api {
    known_client_applications      = []
    mapped_claims_enabled          = false
    requested_access_token_version = 1
    oauth2_permission_scope {
      admin_consent_description  = "Allow the application to access terraform on behalf of the signed-in user."
      admin_consent_display_name = "Access Terraform"
      enabled                    = true
      id                         = random_uuid.app_api_oauth2_id.result
      type                       = "User"
      user_consent_description   = "Allow the application to access terraform on your behalf."
      user_consent_display_name  = "Access Terraform"
      value                      = "user_impersonation"
    }
  }
  app_role {
    allowed_member_types = ["Application", "User"]
    description          = "Read and download terraform modules"
    display_name         = "Terraform Module Reader"
    enabled              = true
    id                   = random_uuid.app_role_reader_id.result
    value                = "Terraform.Module.Reader"
  }
  app_role {
    allowed_member_types = ["Application", "User"]
    description          = "Read and write terraform modules in the registry"
    display_name         = "Terraform Module Contributor"
    enabled              = true
    id                   = random_uuid.app_role_contributor_id.result
    value                = "Terraform.Module.Contributor"
  }
  feature_tags {
    custom_single_sign_on = false
    enterprise            = false
    gallery               = false
    hide                  = false
  }
  public_client {
    redirect_uris = []
  }
  required_resource_access {
    resource_app_id = "00000003-0000-0000-c000-000000000000" # microsoft graph
    resource_access {
      id   = "e1fe6dd8-ba31-4d61-89e7-88639da4683d" # powershell
      type = "Scope"
    }
  }
  single_page_application {
    redirect_uris = []
  }
  timeouts {
    create = null
    delete = null
    read   = null
    update = null
  }
  web {
    homepage_url  = var.url
    logout_url    = ""
    redirect_uris = ["${var.url}/.auth/login/aad/callback"]
    implicit_grant {
      access_token_issuance_enabled = true
      id_token_issuance_enabled     = true
    }
  }
}
