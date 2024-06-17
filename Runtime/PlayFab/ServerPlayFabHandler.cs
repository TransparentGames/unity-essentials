#if ENABLE_PLAYFABSERVER_API && !DISABLE_PLAYFAB_STATIC_API

using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ServerModels;
using UnityEngine;

namespace TransparentGames.Essentials.PlayFab
{
    public static class ServerPlayFabHandler
    {
        public static void Authenticate(string sessionTicket,
        Action<AuthenticateSessionTicketResult> successCallback = null)
        {
            PlayFabServerAPI.AuthenticateSessionTicket(new AuthenticateSessionTicketRequest
            { SessionTicket = sessionTicket }, successResult =>
            {
                successCallback(successResult);
            }, PlayFabFailure);
        }

        public static void GetUserData(string playFabId,
        Action<GetUserDataResult> successCallback = null)
        {
            PlayFabServerAPI.GetUserData(new GetUserDataRequest
            { PlayFabId = playFabId }, successResult =>
            {
                successCallback(successResult);
            }, PlayFabFailure);
        }

        public static void UpdateUserData(string playFabId, Dictionary<string, string> data, Action<UpdateUserDataResult> successCallback = null, Action<PlayFabError> errorCallback = null)
        {
            PlayFabServerAPI.UpdateUserData(new UpdateUserDataRequest
            {
                PlayFabId = playFabId,
                Data = data
            }, successResult =>
            {
                Debug.Log("Successfully updated user data");
                successCallback(successResult);
            }, errorCallback ?? PlayFabFailure);
        }

        public static void GetDropTableData(string tableId, Action<RandomResultTableListing> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabServerAPI.GetRandomResultTables(new GetRandomResultTablesRequest()
            {
                TableIDs = new List<string> { tableId }
            }, successResult =>
        {
            Debug.Log("Completed getting drop tables");
            successCallback(successResult.Tables[tableId]);
        }, errorCallback ?? PlayFabFailure);
        }

        public static void GrantItemsToUser(string playFabId, List<string> itemIds, Action<GrantItemsToUserResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabServerAPI.GrantItemsToUser(new GrantItemsToUserRequest
            {
                PlayFabId = playFabId,
                ItemIds = itemIds
            }, successResult =>
            {
                Debug.Log("Successfully granted items to user");
                successCallback(successResult);
            }, errorCallback ?? PlayFabFailure);
        }

        public static void EvaluateRandomResultTable(string playFabId, string tableId, Action<GrantItemsToUserResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabServerAPI.EvaluateRandomResultTable(new EvaluateRandomResultTableRequest
            {
                TableId = tableId,
            },
            successResult =>
            {
                Debug.Log("Successfully got random result tables");
                OnRandomResultTableResponse(successResult, playFabId, successCallback, errorCallback);
            }, errorCallback ?? PlayFabFailure);
        }

        public static void RevokeInventoryItems(string playFabId, List<string> itemInstanceIds, Action<RevokeInventoryItemsResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            List<RevokeInventoryItem> revokeInventoryItems = new();
            foreach (var itemInstanceId in itemInstanceIds)
            {
                revokeInventoryItems.Add(new RevokeInventoryItem
                {
                    ItemInstanceId = itemInstanceId,
                    PlayFabId = playFabId
                });
            }

            PlayFabServerAPI.RevokeInventoryItems(new RevokeInventoryItemsRequest
            {
                Items = revokeInventoryItems,
            }, successResult =>
            {
                Debug.Log("Successfully revoked inventory items");
                successCallback?.Invoke(successResult);
            }, errorCallback ?? PlayFabFailure);
        }

        private static void OnRandomResultTableResponse(EvaluateRandomResultTableResult tableResult, string playFabId, Action<GrantItemsToUserResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            // First, check if the result is valid
            if (tableResult.ResultItemId == null)
            {
                Debug.LogError("Invalid result item id");
                return;
            }

            // Second, take the result and grant it to the player
            PlayFabServerAPI.GrantItemsToUser(new GrantItemsToUserRequest
            {
                PlayFabId = playFabId,
                ItemIds = new List<string> { tableResult.ResultItemId }
            }, successResult =>
            {
                successCallback(successResult);
                // Handle Result
            }, errorCallback ?? PlayFabFailure);
        }

        private static void PlayFabFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}
#endif