#!/bin/bash
set -xe

# Read configfile
. ../$1

STORAGEACCOUNT_DATA_CONNECTIONSTRING=$(az storage account show-connection-string -n $STORAGEACCOUNT_DATA_NAME -g $RESOURCEGROUP_DATA --query connectionString -o tsv)

az functionapp config appsettings set -n $FUNCTIONAPP_NAME  -g $RESOURCEGROUP_FUNCTIONAPP --settings \
    "tenantId=$tenantId" \
    "subscriptionId=$subscriptionId" \
    "applicationId=$applicationId" \
    "servicePrincipalPassword=$servicePrincipalPassword" \
    "tableStorageName=$DATA_TABLENAME" \
    "tableStorageConnectionString=$STORAGEACCOUNT_DATA_CONNECTIONSTRING" \
    "twitterApiKey=$twitterApiKey" \
    "twitterApiSecretKey=$twitterApiSecretKey" \
    "twitterAccessToken=$twitterAccessToken" \
    "twitterAccessTokenSecret=$twitterAccessTokenSecret" \
    "toggle_SendNotification=$TOGGLE_SENDNOTIFICATIONS" &>/dev/null

