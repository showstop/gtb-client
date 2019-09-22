using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using BestHTTP;

namespace Network
{
    public class LobbyRequest : Singleton<LobbyRequest>
    {
        //private static string _baseUrl = "http://127.0.0.1:8061/v1";
        private static string _baseUrl = "http://dev-gtb-api.yippeegames.net/v1";

        private string _accessToken;

        public void SetDevMode(bool value)
        {
            if(true == value)
                _baseUrl = "http://127.0.0.1:8061/v1";
            else
                _baseUrl = "http://dev-gtb-api.yippeegames.net/v1";
        }

        private HTTPRequest PrepareRequest(HTTPMethods method, string funcionUrl, OnRequestFinishedDelegate callBack)
        {
            var go = GameObject.FindGameObjectWithTag(Constants.WAIT_SERVER_RESPONSE_OBJECT);
            if (go)
            {
                go.SetActive(true);
            }
            
            HTTPRequest req = new HTTPRequest(
                new Uri(_baseUrl + funcionUrl), 
                method,
                callBack);
            req.SetHeader("Access-Token", _accessToken);

            return req;
        }

        public void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
        }
        
        public void LoginReq(Constants.LoginType loginType, string playerUuid, string deviceType, string pushToken)
        {
            
            Debug.LogFormat("LoginType:{0}, playerUuid:{1}, deviceType:{2}", loginType, playerUuid, deviceType);
            HTTPRequest req = new HTTPRequest(
                new Uri(_baseUrl + "/player/login"), 
                HTTPMethods.Post,
                LobbyResponse.OnLobbyAns);
            
            req.AddField("login_type", System.Convert.ToString((int)loginType));
            req.AddField("player_uuid", playerUuid);
            req.AddField("device_type", deviceType);
            req.AddField("push_token", pushToken);
            req.Send();
        }

        public void CheckIfExistPlayerNickReq(string nick)
        {
            var req = PrepareRequest(HTTPMethods.Get, "/player/nick/checkif",
                LobbyResponse.OnCheckIfExistPlayerNickAns);
            req.AddField("nick", nick);
            req.Send();
        }

        public void UpdatePlayerNickReq(string nick)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/player/nick/update", LobbyResponse.OnUpdatePlayerNickAns);
            req.AddField("nick", nick);
            req.Send();
        }

        public void TutorialProgressUpdateReq(Constants.TutorialProgress progress)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/player/tutorial", LobbyResponse.OnTutorialProgressUpdateAns);
            req.AddField("progress", System.Convert.ToString((int)progress));
            req.Send();
        }

        public void PlayerAssetReq()
        {
            var req = PrepareRequest(HTTPMethods.Get, "/player/asset", LobbyResponse.OnPlayerAssetAns);
            req.Send();
        }

        public void MatchInfoReq(Constants.GameMode mode)
        {
            var req = PrepareRequest(HTTPMethods.Get, "/game/match/info", LobbyResponse.OnMatchInfoAns);
            req.AddField("game_type", System.Convert.ToString((int)mode));
            req.Send();
        }

        public void MatchStartReq()
        {
            var req = PrepareRequest(HTTPMethods.Post, "/game/match/request", LobbyResponse.OnMatchStartAns);
            req.AddField("game_type", "0");
            req.Send();
        }

        public void PlayerIntegratedInfoReq()
        {
            YPLog.Log("PlayerIntegratedInfoReq");
            
            var req = PrepareRequest(HTTPMethods.Get, "/player/integrated", LobbyResponse.OnPlayerIntegratedInfoAns);
            req.Send();
        }

        public void GameConfigInfoReq()
        {
            var req = PrepareRequest(HTTPMethods.Get, "/game/config", LobbyResponse.OnGameConfigInfoAns);
            req.Send();
        }

        public void FirstCarChoiceReq(int choice)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/car/first", LobbyResponse.OnFirstCarChoiceAns);
            req.AddField("choice", System.Convert.ToString(choice));
            req.Send();
        }

        public void ChangeSelectCarReq(int vehicleNo)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/car/change", LobbyResponse.OnChangeSelectCarAns);
            req.AddField("vehicle_no", System.Convert.ToString(vehicleNo));
            req.Send();
        }

        public void UpgradeCarReq(int vehicleNo)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/car/upgrade", LobbyResponse.OnUpgradeCarAns);
            req.AddField("vehicle_no", System.Convert.ToString(vehicleNo));
            req.Send();
        }

        public void CarPartsTuningRegInfoReq()
        {
            var req = PrepareRequest(HTTPMethods.Get, "/car/parts/tuning/reg_info",
                LobbyResponse.OnCarPartsTuningRegInfoAns);
            req.Send();
        }

        public void CarPartsTuningReq(int vehicleNo, Constants.PARTS_CATEGORY partsId)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/car/upgrade", LobbyResponse.OnCarPartsTuningAns);
            req.AddField("vehicle_no", System.Convert.ToString(vehicleNo));
            req.AddField("parts_id", System.Convert.ToString((int)partsId));
            req.Send();
        }

        public void AchievementListReq()
        {
            var req = PrepareRequest(HTTPMethods.Get, "/achievement/list", LobbyResponse.OnAchievementListAns);
            req.Send();
        }

        public void AbilitySlotOpenReq(int slotNo)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/ability/slot", LobbyResponse.OnAbilitySlotOpenAns);
            req.AddField("op_type", "0");    // open slot operation
            req.AddField("slot_no", System.Convert.ToString(slotNo));
            req.Send();
        }

        public void AbilityEquipReq(int slotNo, int abilityId)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/ability/slot", LobbyResponse.OnAbilityEquipAns);
            req.AddField("op_type", "1");
            req.AddField("slot_no", System.Convert.ToString(slotNo));
            req.AddField("ability_id", abilityId.ToString());
            req.Send();
        }

        public void AbilityAcquireReq(int abilityId)
        {
            var req = PrepareRequest(HTTPMethods.Post, "/ability/upgrade", LobbyResponse.OnAbilityAcquireAns);
            req.AddField("ability_id", abilityId.ToString());
            req.Send();

        }

    }
}