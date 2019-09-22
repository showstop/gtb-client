using System;


namespace NetworkError
{
	class Common {
		
		internal const short SUCCESS = 1;
		internal const short INTERNAL_DB_ERROR = 1001;
		internal const short NETWORK_ERROR = 1002;
		internal const short EXPIRED_SESSION = 1003;
		internal const short TOO_MANY_PLAYER = 1004;
		internal const short MISMATCH_VERSION = 1005;
		internal const short MAINTENANCE = 1006;
	}
	
	class Auth {
		
		internal const short INVALID_RETURN_DATA = 2001;
		internal const short INVALID_ACCESS_TOKEN = 2002;
		internal const short INAPPROPRIATE_NICK = 2003;
		internal const short ANOTHER_USER_ALREADY_USING_NICK = 2004;
	}
	
	class Billing {
		
		internal const short INVALID_BILLING_CHECK_STR = 3001;
		internal const short INVALID_PRODUCT_ID = 3002;
		internal const short INVALID_RECEIPT = 3003;
	}
	
	class Event {
		
		internal const short INVALID_COUPON = 4001;
		internal const short ALREADY_USED_COUPON = 4002;
		internal const short EXPIRED_COUPON = 4003;
		internal const short ALREADY_USED_ANOTHER_COUPON = 4004;
	}
	
	class Game {
		
		internal const short INVALID_SERVICE_REQUEST = 10001;
		internal const short EXECUTE_PLAY_SERVER_FAILED = 10002;
		internal const short ALREADY_REGISTERED_MATCH_MAKER_POOL = 10003;
		internal const short NOT_EXIST_PLAYER_UUID = 10004;
		internal const short NEED_GAME_TICKET = 10005;
		internal const short INVALID_PROP_CHANGE_REQUEST = 10006;
		internal const short CANNOT_INVITE_PLAYER = 10007;
		internal const short CANNOT_ACCEPT_INVITATION = 10008;
		internal const short REJECT_INVITATION = 10009;
		internal const short INVITATION_ROOM_WAS_DESTROYED = 10010;
	}
	
	class Item {
		
		internal const short NOT_EXIST_ITEM_NO = 30001;
		internal const short INVALID_ITEM_PROPERTY = 30002;
		internal const short INSUFFICIENT_GOLD = 30003;
		internal const short INSUFFICIENT_CASH = 30004;
		internal const short HAVE_NO_ITEM = 30005;
		internal const short INVALID_PARTS_SLOT = 30006;
		internal const short NO_SALES_ITEM = 30007;
		internal const short HIGHER_LEVEL_REQUIRED = 30008;
		internal const short ALREADY_HAVE_ITEM = 30009;
		internal const short ITEM_IS_ALREADY_MAX_LEVEL = 30010;
		internal const short ITEM_STRENGTHEN_IS_FAILED = 30011;
		internal const short NOT_ON_CONDITIONS = 30012;
		internal const short SLOT_IS_ALREADY_OPENED = 30013;
		internal const short CANNOT_SELL_ATTACHED_ITEM = 30014;
		internal const short ALL_ITEM_IS_ATTACHED = 30015;
		internal const short EXCLUSIVE_ITEM_ATTACHED = 30016;
		internal const short CANNOT_RESELL_ITEM = 30017;
		internal const short EXCLUSIVE_ITEM_CANNOT_BUY = 30018;
	}
	
}

