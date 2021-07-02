resource "azurerm_storage_account" "module_repo" {
  name                     = "${var.instance_name}repo"
  resource_group_name      = azurerm_resource_group.instance.name
  location                 = azurerm_resource_group.instance.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  allow_blob_public_access = true
}

resource "azurerm_storage_container" "module_repo" {
  name                  = "modules"
  storage_account_name  = azurerm_storage_account.module_repo.name
  container_access_type = "blob"
}

resource "azurerm_role_assignment" "controller_storage_access_modules" {
  scope                = azurerm_storage_container.module_repo.resource_manager_id
  role_definition_name = "Storage Blob Data Owner"
  principal_id         = azurerm_function_app.app.identity[0].principal_id
}

resource "azurerm_storage_container" "provider_repo" {
  name                  = "provider"
  storage_account_name  = azurerm_storage_account.module_repo.name
  container_access_type = "blob"
}

resource "azurerm_role_assignment" "controller_storage_access_providers" {
  scope                = azurerm_storage_container.provider_repo.resource_manager_id
  role_definition_name = "Storage Blob Data Owner"
  principal_id         = azurerm_function_app.app.identity[0].principal_id
}
