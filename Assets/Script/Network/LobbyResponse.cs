using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using BestHTTP;
//using NUnit.Framework.Internal;
using Random = System.Random;

namespace Network
{
    public class LobbyResponse
    {
        private static bool ResponseParser(string res, out bool result, out string resultCode, out Dictionary<string, object> data)
        {
            var go = GameObject.FindGameObjectWithTag(Constants.WAIT_SERVER_RESPONSE_OBJECT);
            if (go)
            {
                go.SetActive(false);
            }

            var decodedResult = BestHTTP.JSON.Json.Decode(res) as Dictionary<string, object>;
            if (null == decodedResult)
            {
                result = false;
                resultCode = "";
                data = null;
                return false;
            }
            result = (bool)decodedResult["result"];
            resultCode = (string) decodedResult["result_code"];
            data = decodedResult["data"] as Dictionary<string, object>;
            return true;
        }
        
        public static void OnLobbyAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);

            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);

            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.LobbyLoginAns, false, resultCode);
            }
            else
            {
                LobbyRequest.Instance.SetAccessToken((string)data["access_token"]);

                var info = data["player_info"] as Dictionary<string, object>;
                Debug.LogFormat("PlayerNo:{0}, Nick:{1}", 
                    System.Convert.ToInt32(info["player_no"]), 
                    System.Convert.ToString(info["nick"]));
                
                var tp = (Constants.TutorialProgress)System.Convert.ToInt32(info["tutorial_progress"]);
                PlayerDataRepository.Instance.TutorialProgress = tp;
                TutorialManager.Instance.Init();
                
                EventManager.Instance.SendGameEvent(GameEventType.LobbyLoginAns, true);
            }
        }
        
        public static void OnCheckIfExistPlayerNickAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.CheckIfExsitPlayerNickAnsFailed, resultCode);
            }
            else
            {
                EventManager.Instance.SendGameEvent(GameEventType.CheckIfExsitPlayerNickAnsOK);
            }
        }
        
        public static void OnUpdatePlayerNickAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.UpdatePlayerNickAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.MyPlayerInfo.Nick = System.Convert.ToString(data["nick"]);
                EventManager.Instance.SendGameEvent(GameEventType.UpdatePlayerNickAnsOK);
            }
        }
        
        public static void OnTutorialProgressUpdateAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);

            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.TutorialProgressUpdateFailed, resultCode);
            }
            else
            {
                var progress = System.Convert.ToInt32(data["tutorial_progress"]);
                PlayerDataRepository.Instance.TutorialProgress = (Constants.TutorialProgress) progress;
                EventManager.Instance.SendGameEvent(GameEventType.TutorialProgressUpdateAnsOK, progress);
            }
        }

        public static void OnPlayerAssetAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            
        }
        
        public static void OnMatchInfoAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);

            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.MatchInfoAnsFailed, resultCode);
            }
            else
            {
                EventManager.Instance.SendGameEvent(GameEventType.MatchInfoAnsOK);
            }
        }
        
        public static void OnMatchStartAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            
        }
        
        public static void OnGameConfigInfoAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.GameConfigInfoAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.SetGameConfig(data);
                EventManager.Instance.SendGameEvent(GameEventType.GameConfigInfoAnsOK);
            }
        }
        
        public static void OnPlayerIntegratedInfoAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.PlayerIntegratedInfoAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.SetPlayerIntegratedInfo(data);
                EventManager.Instance.SendGameEvent(GameEventType.PlayerIntegratedInfoAnsOK);
            }
        }

        public static void OnFirstCarChoiceAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.FirstVehicleSelectAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.SetCarList(data["car_info"] as List<object>);
                EventManager.Instance.SendGameEvent(GameEventType.FirstVehicleSelectAnsOK);
            }
        }

        public static void OnChangeSelectCarAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.VehicleSelectAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.SetCarList(data["car_info"] as List<object>);
                EventManager.Instance.SendGameEvent(GameEventType.VehicleSelectAnsOK);
            }
        }

        public static void OnUpgradeCarAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.VehicleUpgradeAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.UpdateCarInfo(data["info"] as Dictionary<string, object>);
                EventManager.Instance.SendGameEvent(GameEventType.VehicleUpgradeAnsOK);
            }
        }

        public static void OnCarPartsTuningAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.VehiclePartsTuningAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.UpdateCarInfo(data["info"] as Dictionary<string, object>);
                EventManager.Instance.SendGameEvent(GameEventType.VehiclePartsTuningAnsOK);
            }
        }

        public static void OnCarPartsTuningRegInfoAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.VehiclePartsTuningRegInfoAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.SetPartsTuningStuffInfo(data["info"] as List<object>);
                EventManager.Instance.SendGameEvent(GameEventType.VehiclePartsTuningRegInfoAnsOK);
            }
        }
        
        public static void OnAchievementListAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.AchievementListAnsFailed, resultCode);
            }
            else
            {
                PlayerDataRepository.Instance.SetAchievementInfos(data["info"] as List<object>);
                EventManager.Instance.SendGameEvent(GameEventType.AchievementListAnsOK);
            }
        }
        
        public static void OnAbilitySlotOpenAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.AbilitySlotOpenAnsFailed, resultCode);
            }
            else
            {
                var slotNo = System.Convert.ToInt32(data["slot_no"]);
                PlayerDataRepository.Instance.UpdateAbilitySlot(slotNo, 0);
                EventManager.Instance.SendGameEvent(GameEventType.AbilitySlotOpenAnsOK);
            }
        }
        
        public static void OnAbilityEquipAns(HTTPRequest request, HTTPResponse response)
        {
            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.AbilityEquipAnsFailed, resultCode);
            }
            else
            {
                EventManager.Instance.SendGameEvent(GameEventType.AbilityEquipAnsOK);
            }
        }
        
        public static void OnAbilityAcquireAns(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);

            bool result;
            string resultCode;
            Dictionary<string, object> data;
            ResponseParser(response.DataAsText, out result, out resultCode, out data);
            
            if (false == result)
            {
                EventManager.Instance.SendGameEvent(GameEventType.AbilityAcquireAnsFailed, resultCode);
            }
            else
            {
                var assetInfo = data["asset_info"] as Dictionary<string, object>;
                PlayerDataRepository.Instance.SetAssetInfo(assetInfo);
                int abilityId = System.Convert.ToInt32(data["ability_id"]);
                int abilityLv = System.Convert.ToInt32(data["ability_level"]);
                PlayerDataRepository.Instance.UpdateAbilityLevel(abilityId, abilityLv);
                EventManager.Instance.SendGameEvent(GameEventType.AbilityAcquireAnsOK, abilityId);
            }
        }
    }
}