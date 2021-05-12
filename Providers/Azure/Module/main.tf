provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "instance" {
  name     = var.instance_name
  location = var.location
}