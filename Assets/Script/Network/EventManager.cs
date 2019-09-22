// #define DEBUG_EVENTMANAGER

using UnityEngine;
using System.Collections.Generic;

public enum GameEventType
{
	None = -1, // <- Null event (never sent)
	
	GameLogoPlayEnd = 5,

	LobbyLoginAns = 200,
	
    LSConnectOK = 200,
    LSConnectFailed = 201,
    LSSessionExpired = 202,
    TutorialProgressUpdateAnsOK,
    TutorialProgressUpdateFailed,
    FirstVehicleSelectAnsOK,
    FirstVehicleSelectAnsFailed,
    PlayerIntegratedInfoAnsOK,
	PlayerIntegratedInfoAnsFailed,
	CheckIfExsitPlayerNickAnsOK,
	CheckIfExsitPlayerNickAnsFailed,
	UpdatePlayerNickAnsOK,
	UpdatePlayerNickAnsFailed,
	GameConfigInfoAnsOK,
	GameConfigInfoAnsFailed,
    UpdatePlayerProfileInfoAnsOK,
    UpdatePlayerProfileInfoAnsFailed,
    MatchItemBuyAnsOK,
    MatchItemBuyAnsFailed,
    MatchInfoAnsOK,
    MatchInfoAnsFailed,
    MatchStartAnsOK,
    MatchStartAnsFailed,
    MatchStopAnsOK,
    MatchStopAnsFailed,
    MatchCompleteNotifyOK,
    MatchCompleteNotifyFailed,

    VehicleListAnsOK,
    VehicleListAnsFailed,
    VehicleSelectAnsOK,
    VehicleSelectAnsFailed,
    VehicleUpgradeAnsOK,
    VehicleUpgradeAnsFailed,
    VehiclePartsTuningAnsOK,
    VehiclePartsTuningAnsFailed,
	VehiclePartsTuningRegInfoAnsOK,
	VehiclePartsTuningRegInfoAnsFailed,
    AssetInfoAnsOK,
    AssetInfoAnsFailed,
    AbilityListAnsOK,
    AbilityListAnsFailed,
    AbilitySlotOpenAnsOK,
    AbilitySlotOpenAnsFailed,
    AbilityEquipAnsOK,
    AbilityEquipAnsFailed,
    AbilityAcquireAnsOK,
    AbilityAcquireAnsFailed,

    GrandPrixInfoAnsOK,
    GrandPrixInfoAnsFailed,
    GrandPrixRankInfoAnsOK,
    GrandPrixRankInfoAnsFailed,


    DevPutVehicleCardAnsOK,
    DevPutVehicleCardAnsFailed,
    DevPutVehicleStuffAnsOK,
    DevPutVehicleStuffAnsFailed,

    AchievementListAnsOK,
    AchievementListAnsFailed,
    AchievementReceiveRewardAnsOK,
    AchievementReceiveRewardAnsFailed,
    AchievementAccomplishedNotifyOK,

    DailyMissionListAnsOK,
    DailyMissionListAnsFailed,
    DailyMissionReceiveRewardAnsOK,
    DailyMissionReceiveRewardAnsFailed,
    DailyMissionAccomplishedNotifyOK,

    AttendanceListAnsOK,
    AttendanceListAnsFailed,
    AttendanceReceiveRewardAnsOK,
    AttendanceReceiveRewardAnsFailed,
    AttendanceMonthlyRewardAnsOK,
    AttendanceMonthlyRewardAnsFailed,

    // Client Side    
    DownloadUpdate,
    LoadingClientData,
    LoadingClientDataEnd,    
    
    ChangeShowRoomCar,
    ChangeSelectCar,
    UpgradeCarPresentationStart,
    UpgradeCarPresentationEnd,
    PartsTuningPresentationEnd,

    RequestAttendReward,
    
    ShowLanguageSelection,
    ChangeLanguage,
	ChangeGameMode,
	
	UnlockSpeicalAbilitySlot,
    BuySpecialAbility,
    LevelUpSpecialAbility,
    EquipSpecialAbility,
    
    ShowGrandPrixLeagueInfo,
    ShowGrandPrixRepeatRewardList,
    ShowGrandPrixSeasonRewardList,

    StartWaitServerResponse,
    EndWaitServerResponse,

    // Play Server
    ReceivePlayerInfo,
    KillProcess,
    InternalPlayerEndGameReportAns,
}

public interface IEventListener
{
	void OnHandleEvent(GameEventType gameEventType, params System.Object[] args);
}

public class EventManager
{
	
	private static EventManager instance = null;
	public static EventManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new EventManager();
			}
			return instance;
		}
	}
	
	private bool sendingEvent = false;
	private List<IEventListener> newGameEventListenerList = new List<IEventListener>();
	private List<IEventListener> gameEventListenerList = new List<IEventListener>();
	
	public void AddEventListener(IEventListener eventListener)
	{
		if (sendingEvent)
		{
			if (!newGameEventListenerList.Contains(eventListener))
			{
				newGameEventListenerList.Add(eventListener);
				#if DEBUG_EVENTMANAGER
				Debug.Log("added pending eventListener : " + eventListener + " " + eventListener.GetHashCode());
				#endif // DEBUG_EVENTMANAGER
			}
		}
		else
		{
			if (!gameEventListenerList.Contains(eventListener))
			{
				gameEventListenerList.Add(eventListener);
				#if DEBUG_EVENTMANAGER
				Debug.Log("added eventListener : " + eventListener + " " + eventListener.GetHashCode());
				#endif // DEBUG_EVENTMANAGER
			}
		}
	}
	
	public void RemoveEventListener(IEventListener eventListener)
	{
		if (eventListener != null)
		{
			#if !DEBUG_EVENTMANAGER
			if (!gameEventListenerList.Remove(eventListener) && !newGameEventListenerList.Remove(eventListener))
			{
				//Debug.LogWarning("Failed to remove eventListener: " + eventListener);
			}
			#else // !DEBUG_EVENTMANAGER
			if (!gameEventListenerList.Remove(eventListener))
			{
				if (!newGameEventListenerList.Remove(eventListener))
				{
					Debug.LogWarning("Failed to remove : " + eventListener);
					for (int i = 0; i < gameEventListenerList.Count; ++i)
					{
						Debug.Log("eventListener : " + eventListener + " " + eventListener.GetHashCode());
					}
				}
				else
				{
					Debug.Log("removed pending eventListener : " + eventListener + " " + eventListener.GetHashCode());
				}
			}
			else
			{
				Debug.Log("removed eventListener : " + eventListener + " " + eventListener.GetHashCode());
			}
			#endif // !DEBUG_EVENTMANAGER
		}
	}
	
	public bool Contains(IEventListener eventListener)
	{
		return gameEventListenerList.Contains(eventListener) || newGameEventListenerList.Contains(eventListener);
	}
	
	public void SendGameEvent(GameEventType gameEventType, params System.Object[] args)
	{
		//EventLogger.Log(EventLogType.GameEvent, gameEventType.ToString());
		if (newGameEventListenerList.Count > 0)
		{
			for (int i = 0; i < newGameEventListenerList.Count; ++i)
			{
				gameEventListenerList.Add(newGameEventListenerList[i]);
				#if DEBUG_EVENTMANAGER
				Debug.Log("added eventListener : " + newGameEventListenerList[i] + " " + newGameEventListenerList[i].GetHashCode());
				#endif // DEBUG_EVENTMANAGER
			}
			newGameEventListenerList.Clear();
		}
		
		sendingEvent = true;
		IEventListener[] eventListenerList = gameEventListenerList.ToArray();
		for (int i = 0; i < eventListenerList.Length; ++i)
		{
			eventListenerList[i].OnHandleEvent(gameEventType, args);
		}
		
		//for (int i = 0; i < gameEventListenerList.Count; ++i)
		//{
		//    gameEventListenerList[i].OnHandleEvent(gameEventType, args);
		//}
		sendingEvent = false;
	}
	
	// That could be a way to do that - it compiles to Flash as well
	public static GameEventType GetGameEventFromString(string text)
	{
		var values = System.Enum.GetValues(typeof(GameEventType));
		foreach(GameEventType e in values)
		{
			if(text == e.ToString())
				return e;
		}
		return GameEventType.None;
	}
	
}
