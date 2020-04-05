variable "client_id" {}
variable "client_secret" {}

variable "kubernetes_version" {
  default = "1.16.7"
}

variable "agent_count" {
  default = 3
}

variable "ssh_public_key" {
  default = "~/.ssh/id_rsa.pub"
}

variable "dns_prefix" {
  default = "aks-current-ip-dns"
}

variable cluster_name {
  default = "aks-current-ip"
}

variable resource_group_name {
  default = "rg-current-ip-k8s"
}

variable vm_size {
  default = "Standard_F2s_v2"
}


variable location {
  default = "West US 2"
}
