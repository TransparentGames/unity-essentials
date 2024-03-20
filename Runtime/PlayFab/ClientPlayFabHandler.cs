using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace TransparentGames.Essentials.PlayFab
{
    public static class ClientPlayFabHandler
    {
        public static string SessionTicket { get; private set; }

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

        public static void GuestLogin(Action<LoginResult> successCallback, string customId = null)
        {
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
            {
                CustomId = customId ?? SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,

            }, successResult =>
            {
                SessionTicket = successResult.SessionTicket;
                Debug.Log($"Successfully logged with [{SessionTicket}] user");
                successCallback(successResult);
            }, PlayFabFailure);
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

        public static void GetVirtualCurrency(Action<GetUserInventoryResult> successCallback)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), successResult =>
            {
                Debug.Log($"Successfully retrieved [{successResult.VirtualCurrency}] virtual currency");
                successCallback(successResult);
            }, PlayFabFailure);
        }

        private static void PlayFabFailure(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }
    }
}