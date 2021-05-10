resource "azurerm_storage_account" "module_repo" {
  name                     = "${var.instance_name}repo"
  resource_group_name      = azurerm_resource_group.instance.name
  location                 = azurerm_resource_group.instance.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "module_repo" {
  name                  = "content"
  storage_account_name  = azurerm_storage_account.module_repo.name
  container_access_type = "private"
}

resource "azurerm_role_assignment" "controller_storage_access" {
  scope                = azurerm_storage_container.module_repo.resource_manager_id
  role_definition_name = "Storage Blob Data Owner"
  principal_id         = azurerm_function_app.app.identity[0].principal_id
}