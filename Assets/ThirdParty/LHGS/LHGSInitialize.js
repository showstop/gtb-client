@script ExecuteInEditMode()
#if UNITY_EDITOR
    
    class Testovaci extends EditorWindow {
    
        @MenuItem("Window/LHGS Lighting System")
        static function Init() {
        	var findBaseObject = GameObject.Find("<LHGS Lighting System>");
    			if(findBaseObject == null)
    			{
    			var lhgsEmptyCreate : GameObject;
        		lhgsEmptyCreate = new GameObject("<LHGS Lighting System>");
        		lhgsEmptyCreate.transform.position = Vector3(0,10,0);
        		var fbs : LHGSLightingSystem;
    			fbs = lhgsEmptyCreate.AddComponent(LHGSLightingSystem);
    			}
		}
    }

#endif