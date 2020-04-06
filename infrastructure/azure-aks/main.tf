# Azure as a provider
provider "azurerm" {
  version = "~>2.0"
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
  tags = {
    ProvisionedBy = "Terraform"
    Application   = "Current-IP"
  }
}

resource "azurerm_kubernetes_cluster" "aks_k8s" {
  name                = var.cluster_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  dns_prefix          = var.dns_prefix
  kubernetes_version  = var.kubernetes_version

  default_node_pool {
    name                = "default"
    node_count          = 1
    vm_size             = var.vm_size
    enable_auto_scaling = false
  }

  service_principal {
    client_id     = var.client_id
    client_secret = var.client_secret
  }

  addon_profile {
    kube_dashboard {
      enabled = true
    }

  }

  tags = {
    ProvisionedBy = "Terraform"
    Application   = "Current-IP"
  }
}