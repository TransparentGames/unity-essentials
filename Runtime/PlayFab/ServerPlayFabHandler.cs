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

        public static void UpdateUserData(string playFabId, Dictionary<string, string> data, Action<UpdateUserDataResult> successCallback = null)
        {
            PlayFabServerAPI.UpdateUserData(new UpdateUserDataRequest
            {
                PlayFabId = playFabId,
                Data = data
            }, successResult =>
            {
                successCallback(successResult);
                Debug.Log("Successfully updated user data");
            }, PlayFabFailure);
        }

        private static void PlayFabFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}