resource "azurerm_cosmosdb_account" "db" {
  name                = var.instance_name
  location            = var.cosmos_location
  resource_group_name = azurerm_resource_group.instance.name
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"

  enable_automatic_failover = true

  geo_location {
    location          = var.cosmos_location
    failover_priority = 0
  }

  capabilities {
    name = "EnableServerless"
  }

  consistency_policy {
    consistency_level       = "Session"
    max_interval_in_seconds = 5
    max_staleness_prefix    = 100
  }
}
