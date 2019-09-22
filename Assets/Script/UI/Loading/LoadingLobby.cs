using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Network;

public class LoadingLobby : GUIComponent
{
    [SerializeField]
    private string _lobbySceneName = "mainlobby";

    [SerializeField]
    private float _sceneChangeDelay = 1.2f;

    private int _protocolCount = 0;

    private void Start()
    {
        // TODO temp remove            

        if (0 == PlayerDataRepository.Instance.MyPlayerInfo.PlayerNo)        // enter via login.
        {
            Debug.Log("LoadingLobby - PlayerIntegratedInfoReq");

            LobbyRequest.Instance.PlayerIntegratedInfoReq();
            LobbyRequest.Instance.GameConfigInfoReq();
            LobbyRequest.Instance.CarPartsTuningRegInfoReq();
            LobbyRequest.Instance.AchievementListReq();
//            LSConnector.Instance.PlayerProfileInfoReq(myPlayerNo);
//            LSConnector.Instance.VehicleListReq(myPlayerNo);
//            LSConnector.Instance.AssetInfoReq(myPlayerNo);
//            LSConnector.Instance.AbilityListReq();
//            LSConnector.Instance.AchievementListReq();
//            LSConnector.Instance.AttendanceListReq();
//            LSConnector.Instance.GrandPrixInfoReq();
//            LSConnector.Instance.GrandPrixRankInfoReq(protocol.grandprix_rank_type.LEAGUE, 1, 50);
//            LSConnector.Instance.GrandPrixRankInfoReq(protocol.grandprix_rank_type.GLOBAL, 1, 50);

            StartCoroutine(WaitForInitialData());
        }
        else
        {
            // TODO temp remove
            // end game.
//            if (protocol.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
//            {
//                _protocolCount = 3;
//                LSConnector.Instance.PlayerProfileInfoReq(myPlayerNo);
//                LSConnector.Instance.AssetInfoReq(myPlayerNo);
//                LSConnector.Instance.AchievementListReq();
//            }
//            else
//            {
//                PlayerDataRepository.Instance.ClearGrandPrixLeagueInfo();
//
//                _protocolCount = 3;
//                LSConnector.Instance.GrandPrixInfoReq();
//                LSConnector.Instance.GrandPrixRankInfoReq(protocol.grandprix_rank_type.LEAGUE, 1, 50);
//                LSConnector.Instance.GrandPrixRankInfoReq(protocol.grandprix_rank_type.GLOBAL, 1, 50);
//            }

            StartCoroutine(WaitForUpdateData());
        }
    }

    private IEnumerator WaitForInitialData()
    {
        yield return new WaitForSeconds(_sceneChangeDelay);
        while (!PlayerDataRepository.Instance.IsSetInitialData)
        {
            yield return null;
        }
        SceneManager.LoadScene(_lobbySceneName);
    }

    private IEnumerator WaitForUpdateData()
    {
        yield return new WaitForSeconds(_sceneChangeDelay);
        while (0 != _protocolCount)
        {
            yield return null;
        }

        SceneManager.LoadScene(_lobbySceneName);
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.PlayerIntegratedInfoAnsOK:
                {
                    --_protocolCount;
                    break;
                }

            case GameEventType.VehicleListAnsOK:
                {
//                    protocol.vehicle_list_ans ans = (protocol.vehicle_list_ans)args[0];
//
//                    PlayerDataRepository.Instance.UpdateStuff(ans.GetStuffs().GetStuffInfo());
//                    foreach (protocol.vehicle v in ans.GetInfos().GetInfos())
//                    {
//                        PlayerDataRepository.Instance.AddVehicle(v);
//                    }
//                    PlayerDataRepository.Instance.SortVehicles();
//
//                    PlayerDataRepository.Instance.SelectCarNo = ans.GetSelectedVehicleNo();
//                    if (0 == PlayerDataRepository.Instance.SelectCarNo)
//                    {
//                        PlayerDataRepository.Instance.SelectCarNo = Constants.SUPARK_CAR_ID;
//                    }

                    break;
                }

            case GameEventType.AssetInfoAnsOK:
                {
                    --_protocolCount;
                    //PlayerDataRepository.Instance.AssetInfo = (protocol.asset_info)args[0];
                    break;
                }

            case GameEventType.AbilityListAnsOK:
                {
//                    PlayerDataRepository.Instance.UpdateSpecialAbilityList(((protocol.ability_list)args[0]).GetInfos());                    
//                    PlayerDataRepository.Instance.UpdateSpecialAbilitySlot(((protocol.ability_slot)args[1]).GetSlots());
                    break;
                }

            case GameEventType.AchievementListAnsOK:
                {
                    --_protocolCount;
//                    foreach (protocol.achievement_info info in ((protocol.achievement_list)args[0]).GetInfos())
//                    {
//                        PlayerDataRepository.Instance.UpdateAchievementInfo(info);
//                    }

                    break;
                }

            case GameEventType.AttendanceListAnsOK:
                {
                    //PlayerDataRepository.Instance.AttendanceInfo = (protocol.attendance_list)args[0];
                    break;
                }

            case GameEventType.GrandPrixInfoAnsOK:
                {
                    --_protocolCount;
                    //PlayerDataRepository.Instance.GrandPrixInfo = (protocol.grandprix_info)args[0];

                    break;
                }

            case GameEventType.GrandPrixRankInfoAnsOK:
                {
                    --_protocolCount;
//                    protocol.grandprix_rank_info_ans ans = (protocol.grandprix_rank_info_ans)args[0];
//                    foreach (protocol.grandprix_rank_unit unit in ans.GetInfos())
//                    {
//                        PlayerDataRepository.Instance.AddGrandPrixLeagueInfo(ans.GetRankType(), unit);
//                    }

                    break;
                }

            case GameEventType.LSConnectFailed:
            case GameEventType.PlayerIntegratedInfoAnsFailed:
            case GameEventType.AssetInfoAnsFailed:
            case GameEventType.VehicleListAnsFailed:
            case GameEventType.AbilityListAnsFailed:
            case GameEventType.AchievementListAnsFailed:
            case GameEventType.AttendanceListAnsFailed:
            case GameEventType.GrandPrixInfoAnsFailed:
            case GameEventType.GrandPrixRankInfoAnsFailed:
            case GameEventType.TutorialProgressUpdateFailed:
                {
                    short result = (short)args[0];
                    // TODO show error message via popup.
                    break;
                }
        }
    }
}