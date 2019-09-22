using UnityEngine;
using System.Collections;

public class TutorialConstants : MonoBehaviour
{
    #region tutorial
    
    public enum TUTORIAL_LOBBY_STATE
    {
        TUTO_LOBBY_NULL = 0,
        TUTO_LOBBY_START = 1,
        TUTO_LOBBY_WELCOME,
        TUTO_LOBBY_CAR_SELECT,
        TUTO_LOBBY_MAIN_VIEW,
        TUTO_LOBBY_TOP_VIEW,
        TUTO_LOBBY_LEFT_ICON,
        TUTO_LOBBY_RIGHT_ICON,
        TUTO_LOBBY_CAR_INFO,
        TUTO_LOBBY_CAR_INFO_STAT,
        TUTO_LOBBY_MATCH_BUTTON,
        TUTO_LOBBY_PAGE_BUTTON,
        TUTO_LOBBY_END,
    };

    public enum TUTORIAL_INGAME_STATE
    {
        TUTO_INGAME_NULL = 0,
        TUTO_INGAME_START = 1,
        TUTO_INGAME_HUD,
        TUTO_INGAME_LANEMOVE_RIGHT,
        TUTO_INGAME_LANEMOVE_RIGHT_FINGER,
        TUTO_INGAME_LANEMOVE_LEFT,
        TUTO_INGAME_LANEMOVE_LEFT_FINGER,
        TUTO_INGAME_LANEMOVE_END,
        TUTO_INGAME_ITEM,
        TUTO_INGAME_ITEM_SET,
        TUTO_INGAME_ITEM_GET,
        TUTO_INGAME_ITEM_FIRE,
        TUTO_INGAME_ITEM_END,
        TUTO_INGAME_ABILITY,
        TUTO_INGAME_ABILITY_SET,
        TUTO_INGAME_ABILITY_GET,
        TUTO_INGAME_ABILITY_SHOT,
        TUTO_INGAME_ABILITY_END,
        TUTO_INGAME_COLLISION,
        TUTO_INGAME_COLLISION_PLAYING,
        TUTO_INGAME_COLLISION_END,
        TUTO_INGAME_END,
    };
    public static TUTORIAL_INGAME_STATE IngameState;
    public static TUTORIAL_LOBBY_STATE LobbyState;

    public static bool _tutorialPlaying = false;
    public static bool _tutorialAbility = false;

    public static int count = 0;
    public const int CARID_MINICOOP = 11100100;
    public const int CARID_BUGGY = 11100200;
    public const int CARID_TURBIN = 11100300;

    public const string TUTORIAL_TAG = "Tutorial";
    public const string TUTORIAL_TAG_INGAMEHUD = "TutorialIngame";
    public const string TUTORIAL_TAG_LOBBYHUD = "TutorialLobby";
    public const string TUTORIAL_SCENE_INGAME = "tutorial_ingame";
    public const string TUTORIAL_PLYAERPREFS_KEY = "TutorialProgressKey";

    public static float TUTORIAL_ITEM_DELAYTIME = 0.5f;

    ////TitleTEXT
    //public const string TUTORIAL_TITLE_LICENSE = "라이센스";
    //public const string TUTORIAL_TITLE_INTRO = "INTRO";
    //public const string TUTORIAL_TITLE_HUD = "HUD";
    //public const string TUTORIAL_TITLE_LANECHANGE = "차선변경";
    //public const string TUTORIAL_TITLE_ITEM = "아이템";
    //public const string TUTORIAL_TITLE_ABILITY = "보유스킬";
    //public const string TUTORIAL_TITLE_COLLISION = "충돌";
    //public const string TUTORIAL_TITLE_END = "튜토리얼 종료";

    ////LOBBY TEXT
    //public static string LOBBY_NICK = "                        ! 만나서 반가워요!!\n 일단 퀵매치를 한번 플레이 해볼까요?";
    //public static string LOBBY_INTRO = "메인 화면도 간단하게 설명 드릴게요!";
    //public static string LOBBY_END = "이제는 다른 사람들과 경쟁하면서 게임을 즐겨보세요!";

    ////INGAME TEXT
    //public const string INGAME_INTRO = "무한질주 경쟁! GTB Racing에 오신 걸 환영해요.\n 게임 방법을 간단히 알려드릴께요.";
    //public const string INGAME_HUD = "경기와 관련된 정보가 표시되는 공간이예요.\n 필요한 정보들이 어디에 표시되는지 확인하세요";
    //public const string INGAME_LANEMOVE_RIGHT = "차선을 변경하려면\n 이동하려는 방향으로 화면을 드래그 하면 되요.\n 우선 오른쪽 차선으로 변경해볼까요?";
    //public const string INGAME_LANEMOVE_LEFT = "잘하셨어요!\n 이번에는 왼쪽 차선으로 변경해볼까요?";
    //public const string INGAME_LANEMOVE_END = "센스가 있으시네요!\n 차선 변경은 게임 중 가장 많이 하는 행동이니 잊지 마세요!";
    //public const string INGAME_ITEM = "아이템 박스에서는 게임 플레이에\n 유용한 아이템을 획득할 수 있어요.\n 마침 앞쪽에 아이템 박스가 보이네요!";
    //public const string INGAME_ITEM_GET = "펀치 아이템을 획득했네요.\n 펀치는 앞쪽의 차량을 공격해서 데미지를 줄 수 있어요.\n 사용해볼까요?";
    //public const string INGAME_ITEM_END = "잘하셨어요!\n 펀치이외에도 많은 아이템들이 있으니\n 게임을 하면서 조금씩 알아가기로 해요.";
    //public const string INGAME_ABILITY = "모든 차량은 게임에 도움이 되는 스킬을 가지고 있어요.\n 이번에는 스킬이 발동되는 것을 볼까요?";
    //public const string INGAME_ABILITY_GET = "패트리어트 스킬이 발동했네요.\n 펀치를 더 강력한 패트리어트로 변경해주는 스킬이예요.\n 사용법은 펀치와 같아요. 사용해볼까요?";
    //public const string INGAME_ABILITY_END = "패트리어트처럼 아이템을 강화하는 스킬이외에도\n 차량의 상태에 따라 발동되는 스킬들도 있으니\n 차량들이 보유한 스킬들을 잘 살펴보세요!";
    //public const string INGAME_COLLISION = "아이템 이외에도 다른 차량을 방해하는 방법으로\n 다른 차량과 부딪히는 충돌이 있어요.\n 차선을 변경해서 옆에 있는 차량과 부딪혀볼까요?";
    //public const string INGAME_COLLISION_END = "다른 차량과 충돌하면 강도의 차이에 따라\n 데미지를 입고 순간적으로 속도 저하가 발생해요.\n 내 차량의 강도가 낮다면 되도록 충돌을 피하세요!";
    //public const string INGAME_TUTORIAL_END = "게임과 관련된 기본 사항을 모두 배우셨어요!\n 다시 메인 화면으로 돌아가볼까요?";

    ////SkipGuide TEXT
    //public const string SKIP_LOBBY_TEXT = "게임 안내 가이드를 건너 뛰시겠습니까?";
    //public const string SKIP_INGAME_TEXT = "게임 플레이 가이드를 건너 뛰시겠습니까?";

    #endregion
}
