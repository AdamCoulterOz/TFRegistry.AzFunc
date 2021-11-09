variable "instance_name" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "cosmos_location" {
  type = string
}

variable "url" {
#   example = "https://terraform.mycompany.com"
}

# GUID of app owner (defaults to creator if not set)
variable "owner_id" {
    type = string
    default = null
}