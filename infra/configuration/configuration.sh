#!/bin/bash
set -xe

# Read configfile
. ../$1

STORAGEACCOUNT_DATA_CONNECTIONSTRING=$(az storage account show-connection-string -n $STORAGEACCOUNT_DATA_NAME -g $RESOURCEGROUP_DATA --query connectionString -o tsv)

az functionapp config appsettings set -n $FUNCTIONAPP_NAME  -g $RESOURCEGROUP_FUNCTIONAPP --settings \
"tenantId=${{ secrets.tenantId }}" \
"subscriptionId=${{ secrets.subscriptionId }}" \
"applicationId=${{ secrets.applicationId }}" \
"servicePrincipalPassword=${{ secrets.servicePrincipalPassword }}" \
"tableStorageName=$DATA_TABLENAME" \
"tableStorageConnectionString=$STORAGEACCOUNT_DATA_CONNECTIONSTRING" \
"twitterApiKey=${{ secrets.twitterApiKey }}" \
"twitterApiSecretKey=${{ secrets.twitterApiSecretKey }}" \
"twitterAccessToken=${{ secrets.twitterAccessToken }}" \
"twitterAccessTokenSecret=${{ secrets.twitterAccessTokenSecret }}" \
"toggle_SendNotification=$TOGGLE_SENDNOTIFICATIONS"

