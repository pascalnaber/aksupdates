#!/bin/bash
set -xe

# Read configfile
. ./$1

az storage account create  -g $RESOURCEGROUP_DATA -n $STORAGEACCOUNT_DATA_NAME --sku Standard_LRS
az storage account create  -g $RESOURCEGROUP_DATA -n $STORAGEACCOUNT_FUNCTIONAPP_NAME --sku Standard_LRS

STORAGEACCOUNT_FUNCTIONAPP_RESOURCEID=$(az storage account show -g $RESOURCEGROUP_DATA -n $STORAGEACCOUNT_FUNCTIONAPP_NAME --query id -o tsv)

# provisions a hostingplan, application insights and the function app
az functionapp create  --name $FUNCTIONAPP_NAME --consumption-plan-location $LOCATION --os-type Linux --functions-version 3 --resource-group $RESOURCEGROUP_FUNCTIONAPP --runtime dotnet --storage-account $STORAGEACCOUNT_FUNCTIONAPP_RESOURCEID
