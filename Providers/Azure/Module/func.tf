resource "azurerm_storage_account" "app_storage" {
  name                     = "${var.instance_name}func"
  resource_group_name      = azurerm_resource_group.instance.name
  location                 = azurerm_resource_group.instance.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_role_assignment" "func_app_access_to_storage" {
  scope                = azurerm_storage_account.app_storage.id
  role_definition_name = "Storage Blob Data Owner"
  principal_id         = azurerm_linux_function_app.app.identity[0].principal_id
}

resource "azurerm_service_plan" "app_plan" {
  name                = var.instance_name
  location            = azurerm_resource_group.instance.location
  resource_group_name = azurerm_resource_group.instance.name
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_application_insights" "monitor" {
  name                = var.instance_name
  location            = azurerm_resource_group.instance.location
  resource_group_name = azurerm_resource_group.instance.name
  application_type    = "web"
}

resource "azurerm_linux_function_app" "app" {
  name                          = var.instance_name
  location                      = azurerm_resource_group.instance.location
  resource_group_name           = azurerm_resource_group.instance.name
  storage_account_name          = azurerm_storage_account.app_storage.name
  service_plan_id               = azurerm_service_plan.app_plan.id
  storage_uses_managed_identity = true
  https_only                    = true
  app_settings = {
    FUNCTIONS_WORKER_RUNTIME          = "dotnet-isolated"
    PurpleDepot__Storage__Account     = azurerm_storage_account.repo.name
    PurpleDepot__Storage__Container   = azurerm_storage_container.registry.name
    PurpleDepot__Database__Connection = azurerm_cosmosdb_account.db.connection_strings[0]
    PurpleDepot__Database__Name       = "PurpleDepot"
    AzureWebJobsStorage               = azurerm_storage_account.app_storage.primary_connection_string
  }

  site_config {
    application_insights_key               = azurerm_application_insights.monitor.instrumentation_key
    application_insights_connection_string = azurerm_application_insights.monitor.connection_string
    # application_stack {
    #   dotnet_version = "6"
    # }
    app_service_logs {
      disk_quota_mb         = 25
      retention_period_days = 3
    }
  }

  identity {
    type = "SystemAssigned"
  }

  auth_settings {
    enabled                       = true
    token_store_enabled           = true
    issuer                        = "https://sts.windows.net/${data.azuread_client_config.current.tenant_id}/"
    unauthenticated_client_action = "RedirectToLoginPage"
    active_directory {
      client_id         = azuread_application.terraform.application_id
      allowed_audiences = var.url != null ? [var.url] : [local.token_audience]
    }
  }
}
