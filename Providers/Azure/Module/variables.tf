variable "app_name" {
  type        = string
  description = "The name of the application, you may want to call it '{Organisation Name} Terraform'"
  default     = "Terraform"
}

variable "instance_name" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "url" {
  type = string
  #   example = "https://terraform.mycompany.com"
  default     = null
  description = "The URL of the service, if not specified it default to https://{instance_name}.azurewebsites.net"
}

# GUID of app owner (defaults to creator if not set)
variable "owner_id" {
  type    = string
  default = null
}
