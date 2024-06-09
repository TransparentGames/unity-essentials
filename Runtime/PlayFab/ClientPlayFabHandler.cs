using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace TransparentGames.Essentials.PlayFab
{
    public static class ClientPlayFabHandler
    {
        public static string SessionTicket { get; private set; }
        public static string PlayFabId { get; private set; }

        #region Register
        public static void Register(string email, string username, string password,
        Action<RegisterPlayFabUserResult> successCallback)
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
            {
                Email = email,
                Username = username,
                Password = password,
                RequireBothUsernameAndEmail = false
            }, successResult =>
            {
                // TODO: Move this to a separate UI
                CreateCharacter(username, (result) =>
                {
                    Debug.Log($"Successfully registered [{username}] user");
                    successCallback(successResult);
                });
            }, PlayFabFailure);
        }
        #endregion

        #region Login

        public static void Login(string username, string password,
        Action<LoginResult> successCallback)
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = username,
                Password = password,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                }
            }, successResult =>
            {
                SessionTicket = successResult.SessionTicket;
                Debug.Log($"Successfully logged with [{SessionTicket}] user");
                successCallback(successResult);
            }, PlayFabFailure);
        }

        public static void GuestLogin(Action<LoginResult> successCallback, Action<PlayFabError> errorCallback = null, string customId = null)
        {
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
            {
                CustomId = customId ?? SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetUserData = true,
                    GetUserInventory = true,
                    GetUserVirtualCurrency = true,
                }

            }, successResult =>
            {
                SessionTicket = successResult.SessionTicket;
                PlayFabId = successResult.PlayFabId;
                Debug.Log($"Successfully logged with [{SessionTicket}] user");
                successCallback(successResult);
            }, errorCallback ?? PlayFabFailure);
        }

        #endregion

        public static void CreateCharacter(string displayName,
        Action<UpdateUserDataResult> successCallback)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { "display_name", displayName }
            }
            }, successResult =>
            {
                Debug.Log($"Successfully created [{displayName}] character");
                successCallback(successResult);
            }, PlayFabFailure);
        }

        public static void GetPlayerProfile(Action<GetPlayerProfileResult> successCallback)
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
            {
                ProfileConstraints = new PlayerProfileViewConstraints
                {
                    ShowDisplayName = true
                }
            }, successResult =>
            {
                Debug.Log($"Successfully retrieved [{successResult.PlayerProfile.DisplayName}] player profile");
                successCallback(successResult);
            }, PlayFabFailure);
        }

        public static void GetUserInventory(Action<GetUserInventoryResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), successResult =>
            {
                Debug.Log($"Successfully retrieved inventory");
                successCallback(successResult);
            }, errorCallback ?? PlayFabFailure);
        }

        /// <summary>
        /// Purchase Item by Item ID in catalog using specific currency.
        /// </summary>
        /// <param name="itemId">Item ID you want to buy</param>
        /// <param name="currencyCode">Currency Code (for example: AB/CD/etc.)</param>
        /// <param name="successCallback">On Purchase Success. Contains PurchaseItemResult</param>
        /// <param name="errorCallback">On Error. Contains PlayFabError</param>
        public static void PurchaseItem(string itemId, string currencyCode, int price, Action<PurchaseItemResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = itemId,
                VirtualCurrency = currencyCode,
                Price = price
            }, successCallback,
            errorCallback ?? PlayFabFailure);
        }

        public static void ConsumeItem(string itemInstanceId, int count, Action<ConsumeItemResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
            {
                ItemInstanceId = itemInstanceId,
                ConsumeCount = count
            }, successCallback,
            errorCallback ?? PlayFabFailure);
        }

        public static void GetCatalogItems(Action<GetCatalogItemsResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
            {
            }, successCallback,
            errorCallback ?? PlayFabFailure);
        }

        public static void SubtractUserVirtualCurrency(string currencyCode, int amount, Action<ModifyUserVirtualCurrencyResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest
            {
                VirtualCurrency = currencyCode,
                Amount = amount
            }, successCallback,
            errorCallback ?? PlayFabFailure);
        }

        public static void AddUserVirtualCurrency(string currencyCode, int amount, Action<ModifyUserVirtualCurrencyResult> successCallback, Action<PlayFabError> errorCallback = null)
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
            {
                VirtualCurrency = currencyCode,
                Amount = amount
            }, successCallback,
            errorCallback ?? PlayFabFailure);
        }

        private static void PlayFabFailure(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }
    }
}