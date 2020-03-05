#!/bin/bash
set -xe

. ../aksupdates-dev.config
SERVICEPRINCIPAL_NAME=spn-aksupdates-deployment

az account set --subscription "$SUBSCRIPTION_ID"

az group create -l $LOCATION -n $RESOURCEGROUP_FUNCTIONAPP
az group create -l $LOCATION -n $RESOURCEGROUP_DATA

RESOURCEGROUP_FUNCTIONAPP_DEVELOPMENT_RESOURCEID=$(az group show -n $RESOURCEGROUP_FUNCTIONAPP --query id -o tsv)
RESOURCEGROUP_DATA_DEVELOPMENT_RESOURCEID=$(az group show -n $RESOURCEGROUP_DATA --query id -o tsv)

. ../aksupdates-prod.config

az account set --subscription "$SUBSCRIPTION_ID"

az group create -l $LOCATION -n $RESOURCEGROUP_FUNCTIONAPP
az group create -l $LOCATION -n $RESOURCEGROUP_DATA

RESOURCEGROUP_FUNCTIONAPP_PRODUCTION_RESOURCEID=$(az group show -n $RESOURCEGROUP_FUNCTIONAPP --query id -o tsv)
RESOURCEGROUP_DATA_PRODUCTION_RESOURCEID=$(az group show -n $RESOURCEGROUP_DATA --query id -o tsv)

SPN_ID=$(az ad sp list --spn "http://$SERVICEPRINCIPAL_NAME" --query [].appId -o tsv)
if [ -z "$SPN_ID" ]; then
  az ad sp create-for-rbac --name $SERVICEPRINCIPAL_NAME --role contributor --sdk-auth --scopes \
    $RESOURCEGROUP_FUNCTIONAPP_DEVELOPMENT_RESOURCEID \
    $RESOURCEGROUP_DATA_DEVELOPMENT_RESOURCEID \
    $RESOURCEGROUP_FUNCTIONAPP_PRODUCTION_RESOURCEID \
    $RESOURCEGROUP_DATA_PRODUCTION_RESOURCEID
else
  az role assignment create --role Contributor --assignee $SPN_ID --scope $RESOURCEGROUP_FUNCTIONAPP_DEVELOPMENT_RESOURCEID
  az role assignment create --role Contributor --assignee $SPN_ID --scope $RESOURCEGROUP_DATA_DEVELOPMENT_RESOURCEID
  az role assignment create --role Contributor --assignee $SPN_ID --scope $RESOURCEGROUP_FUNCTIONAPP_PRODUCTION_RESOURCEID
  az role assignment create --role Contributor --assignee $SPN_ID --scope $RESOURCEGROUP_DATA_PRODUCTION_RESOURCEID
fi

echo "Create a secret in Github > Settings > Secrets > Add a new secret > with the name 'AZURE_CREDENTIALS' and add the json above as value"