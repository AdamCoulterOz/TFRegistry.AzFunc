resource "azurerm_resource_group" "instance" {
  name     = var.resource_group_name
  location = var.location
}