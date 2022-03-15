terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>2.99"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "~>2.18"
    }
    random = {
      source  = "hashicorp/random"
      version = "~>3.1"
    }
  }
}
