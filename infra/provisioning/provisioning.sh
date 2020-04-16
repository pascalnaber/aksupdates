#!/bin/bash
set -xe

# Read configfile
. ../$1

az account set --subscription $SUBSCRIPTION_ID

az storage account create  -g $RESOURCEGROUP_DATA -n $STORAGEACCOUNT_DATA_NAME --sku Standard_LRS
az storage account create  -g $RESOURCEGROUP_DATA -n $STORAGEACCOUNT_FUNCTIONAPP_NAME --sku Standard_LRS

STORAGEACCOUNT_FUNCTIONAPP_RESOURCEID=$(az storage account show -g $RESOURCEGROUP_DATA -n $STORAGEACCOUNT_FUNCTIONAPP_NAME --query id -o tsv)

# provisions a hostingplan, application insights and the function app
az functionapp create  --name $FUNCTIONAPP_NAME --consumption-plan-location $LOCATION --os-type Windows --functions-version 3 --resource-group $RESOURCEGROUP_FUNCTIONAPP --runtime dotnet --storage-account $STORAGEACCOUNT_FUNCTIONAPP_RESOURCEID

# az deployment group create --resource-group $RESOURCEGROUP_FUNCTIONAPP --verbose \
#    --template-file "./arm/Microsoft.Web/sites/functionapp/azuredeploy-linux.json" \
#    --parameters functionName=$FUNCTIONAPP_NAME \
#    storageAccountName=$STORAGEACCOUNT_FUNCTIONAPP_NAME \
#    storageAccountResourceGroup=$RESOURCEGROUP_DATA \