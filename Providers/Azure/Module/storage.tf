resource "azurerm_storage_account" "repo" {
  name                     = var.instance_name
  resource_group_name      = azurerm_resource_group.instance.name
  location                 = azurerm_resource_group.instance.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  allow_blob_public_access = true
  min_tls_version          = "TLS1_2"
}

resource "azurerm_role_assignment" "controller_storage_access" {
  scope                = azurerm_storage_account.repo.id
  role_definition_name = "Storage Blob Data Owner"
  principal_id         = azurerm_linux_function_app.app.identity[0].principal_id
}

resource "azurerm_storage_container" "registry" {
  name                  = "registry"
  storage_account_name  = azurerm_storage_account.repo.name
  container_access_type = "blob"
}

resource "azurerm_storage_container" "state" {
  name                  = "state"
  storage_account_name  = azurerm_storage_account.repo.name
}
