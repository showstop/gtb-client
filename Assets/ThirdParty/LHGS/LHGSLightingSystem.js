//LHGS Lighting Script v1.0 created by: Lukas Hanak - Please support next developing for unity free and do not share this. Thanks! 
#pragma strict
#if UNITY_EDITOR
@script ExecuteInEditMode()

class baseParametersCL{
	class SkySettingsCL{
	var LHGS_SkyColor : Color = new Color32(215, 223, 226, 255);
	var LHGS_AmbientLight : Color = new Color32(26, 44, 63, 255);
	var LHGS_Intensity : float = 1;}
	var SkySettings : SkySettingsCL = new SkySettingsCL();

	class AmbientOcclusionCL{
	var AOcclusionIntensity : float = 0.3;
	var AOmaxDistance : float = 0.5;
	}
	
	var AmbientOcclusion : AmbientOcclusionCL = new AmbientOcclusionCL();
	
	class DirectLightCL{
	var CreateDirectLight : boolean = false;
	}
	
	var DirectLight : DirectLightCL = new DirectLightCL();

}

var baseParameters : baseParametersCL = new baseParametersCL();

class skySystemCL{
var hemisphereSkyModel : boolean = true;
var sphereSkyModel : boolean = false;
}

var  skySystem : skySystemCL = new skySystemCL();

class lightDistributionMethodCL{
var solvingClassic : boolean = true;
var solvingIrregular : boolean = false;
var solvingBalanced : boolean = false;
}

var  lightDistributionMethod : lightDistributionMethodCL = new lightDistributionMethodCL();

class lightingModelQualityCL{
var ExtremeQualityModel : boolean;
var HighQualityModel : boolean;
var StandardQualityModel : boolean = true;
var LowQualityModel : boolean;
var veryLowQualityModel : boolean;
var sourceOfRays : int;
}

var  lightingModelQuality : lightingModelQualityCL = new lightingModelQualityCL();

class resetToDefaultCL{
var deleteAll : boolean = false;
var reset : boolean = false;
}

var  resetToDefault : resetToDefaultCL = new resetToDefaultCL();


class renderingCL{
var LightMapResolution : int = 30;
var useSettings : boolean = false;
var RenderLightmap : boolean = false;
var StopRendering : boolean = false;
}

var  rendering : renderingCL = new renderingCL();

private var lightingSolverCustomFactor : float = 1; // higher value - lowest quality
static var lhgsEmpty: GameObject;
private var qSelect : int;
private var sSelect : int = 0;
private var mSelect : int = 0;


function Start () {
}

function Update () {

selectQualitytModel();
	
	if(rendering.useSettings == true)
	{
	LHGS();
    	
    rendering.useSettings = false;
	}

	if(baseParameters.DirectLight.CreateDirectLight == true)
	{
		lhgsEmpty = GameObject.Find("<LHGS Lighting System>");
		var findDirectL : GameObject;
	    findDirectL = GameObject.Find("DirectLight");
	    
    	if(findDirectL == null)
    	{
		var directLight : GameObject;  
		directLight = new GameObject("DirectLight");
		directLight.transform.Translate(lhgsEmpty.transform.position);
		directLight.transform.Rotate(45,45,0);
		directLight.AddComponent(Light);
    	directLight.GetComponent.<Light>().type = LightType.Directional;
    	directLight.GetComponent.<Light>().shadows = LightShadows.Soft;
    	directLight.GetComponent.<Light>().color = Color32(167, 124, 44, 255);
    	directLight.GetComponent.<Light>().intensity = 0.5;
		}
		
		baseParameters.DirectLight.CreateDirectLight = false;
	}
	
	// reset all settings to default values
	if(resetToDefault.reset == true)
	{
	lightingModelQuality.StandardQualityModel = true;
	lightingModelQuality.ExtremeQualityModel = false;
	lightingModelQuality.HighQualityModel = false;
	lightingModelQuality.LowQualityModel = false;
	lightingModelQuality.veryLowQualityModel = false;
	skySystem.hemisphereSkyModel = true;
	baseParameters.SkySettings.LHGS_SkyColor = Color32(215, 223, 226, 255);
	baseParameters.SkySettings.LHGS_Intensity = 1;
	baseParameters.SkySettings.LHGS_AmbientLight = Color32(26, 44, 63, 255);
	RenderSettings.ambientLight = baseParameters.SkySettings.LHGS_AmbientLight;
	baseParameters.AmbientOcclusion.AOmaxDistance = 0.5;
	baseParameters.AmbientOcclusion.AOcclusionIntensity = 0.3;
	rendering.LightMapResolution = 30;
	resetToDefault.reset = false;	
	}
	
	// delete all ray sources
	if(resetToDefault.deleteAll == true)
	{
	deleteAllLights();	
	resetToDefault.deleteAll = false;	
	}
	
	// start rendering
	if(rendering.RenderLightmap == true)
	{
	LHGS();
	Lightmapping.BakeAsync();
	rendering.RenderLightmap = false;
	}
	
	// stop rendering
	if(rendering.StopRendering == true)
	{
	Lightmapping.Cancel();
	rendering.StopRendering = false;
	}
	
	
	// disable when is active play mode
	if (Application.isPlaying) 
	{
    for(var activeFakeGIlights : GameObject in GameObject.FindObjectsOfType(GameObject))
		{
    		if(activeFakeGIlights.name == "SkyRay")
    		{
    		activeFakeGIlights.SetActive(false);
    		}
		}
    }
}

function deleteAllLights()
{
for(var LHGSLightsForDelete : GameObject in GameObject.FindObjectsOfType(GameObject))
	{
    	if(LHGSLightsForDelete.name == "SkyRay")
    	{
        DestroyImmediate(LHGSLightsForDelete);
    	}
	}
}

function selectQualitytModel()
{
	if(qSelect != 4){
		if(lightingModelQuality.ExtremeQualityModel == true)
		{
		lightingSolverCustomFactor = 1.5;
		qSelect = 4;
		lightingModelQuality.ExtremeQualityModel = true;
		lightingModelQuality.HighQualityModel = false;
		lightingModelQuality.StandardQualityModel = false;
		lightingModelQuality.LowQualityModel = false;
		lightingModelQuality.veryLowQualityModel = false;
		}
	}

	if(qSelect != 3){
		if(lightingModelQuality.HighQualityModel == true)
		{
		lightingSolverCustomFactor = 2;
		qSelect = 3;
		lightingModelQuality.ExtremeQualityModel = false;
		lightingModelQuality.HighQualityModel = true;
		lightingModelQuality.StandardQualityModel = false;
		lightingModelQuality.LowQualityModel = false;
		lightingModelQuality.veryLowQualityModel = false;
		}
	}
	
	if(qSelect != 2){
		if(lightingModelQuality.StandardQualityModel == true)
		{
		lightingSolverCustomFactor = 4;
		qSelect = 2;
		lightingModelQuality.ExtremeQualityModel = false;
		lightingModelQuality.HighQualityModel = false;
		lightingModelQuality.StandardQualityModel = true;
		lightingModelQuality.LowQualityModel = false;
		lightingModelQuality.veryLowQualityModel = false;
		}
	
	}
	
	if(qSelect != 1){
		if(lightingModelQuality.LowQualityModel == true)
		{
		lightingSolverCustomFactor = 5;
		qSelect = 1;
		lightingModelQuality.LowQualityModel = true;
		lightingModelQuality.ExtremeQualityModel = false;
		lightingModelQuality.HighQualityModel = false;
		lightingModelQuality.StandardQualityModel = false;
		lightingModelQuality.veryLowQualityModel = false;
		}
	}
	
	if(qSelect != 0){
		if(lightingModelQuality.veryLowQualityModel == true)
		{
		lightingSolverCustomFactor = 7.5;
		qSelect = 0;
		lightingModelQuality.LowQualityModel = false;
		lightingModelQuality.ExtremeQualityModel = false;
		lightingModelQuality.HighQualityModel = false;
		lightingModelQuality.StandardQualityModel = false;
		lightingModelQuality.veryLowQualityModel = true;
		}
	}
	
	if((lightingModelQuality.ExtremeQualityModel == false)&&(lightingModelQuality.HighQualityModel == false)&&(lightingModelQuality.StandardQualityModel == false)&&(lightingModelQuality.LowQualityModel == false)&&(lightingModelQuality.veryLowQualityModel == false))
	{
	lightingSolverCustomFactor = 4;
		qSelect = 2;
		lightingModelQuality.ExtremeQualityModel = false;
		lightingModelQuality.HighQualityModel = false;
		lightingModelQuality.StandardQualityModel = true;
		lightingModelQuality.LowQualityModel = false;
		lightingModelQuality.veryLowQualityModel = false;
	}
	
	if(sSelect != 0){
		if(skySystem.hemisphereSkyModel == true)
		{
		sSelect = 0;
		skySystem.sphereSkyModel = false;
		}
	}
	
	if(sSelect != 1){
		if(skySystem.sphereSkyModel == true)
		{
		sSelect = 1;
		skySystem.hemisphereSkyModel = false;
		}
	}
	
	if((skySystem.sphereSkyModel == false)&&(skySystem.hemisphereSkyModel == false))
	{
	skySystem.hemisphereSkyModel = true;
	sSelect = 0;
	skySystem.sphereSkyModel = false;
	}
	
	if(mSelect != 0){
		if(lightDistributionMethod.solvingClassic == true)
		{
		mSelect = 0;
		lightDistributionMethod.solvingIrregular = false;
		lightDistributionMethod.solvingBalanced = false;
		}
	}
	
	if(mSelect != 1){
		if(lightDistributionMethod.solvingIrregular == true)
		{
		mSelect = 1;
		lightDistributionMethod.solvingClassic = false;
		lightDistributionMethod.solvingBalanced = false;
		}
	}
	
	if(mSelect != 2){
		if(lightDistributionMethod.solvingBalanced == true)
		{
		mSelect = 2;
		lightDistributionMethod.solvingClassic = false;
		lightDistributionMethod.solvingIrregular = false;
		}
	}
	
	if((lightDistributionMethod.solvingIrregular == false)&&(lightDistributionMethod.solvingClassic == false)&&(lightDistributionMethod.solvingBalanced == false))
	{
	lightDistributionMethod.solvingClassic = true;
	mSelect = 0;
	lightDistributionMethod.solvingIrregular = false;
	}
}

function LHGS()
{
	lhgsEmpty = GameObject.Find("<LHGS Lighting System>");
	// clear scene
	deleteAllLights();
	lightingModelQuality.sourceOfRays = 0;
	
	// render settings
	RenderSettings.ambientLight = baseParameters.SkySettings.LHGS_AmbientLight;
	LightmapEditorSettings.aoMaxDistance = baseParameters.AmbientOcclusion.AOmaxDistance;
	LightmapEditorSettings.aoAmount = baseParameters.AmbientOcclusion.AOcclusionIntensity;
	LightmapEditorSettings.realtimeResolution = rendering.LightMapResolution;
	
	// light generator	
	var LHGSLight : GameObject;  
	var skySolver : int;
	
	if(lightDistributionMethod.solvingIrregular == true){
		if(skySystem.hemisphereSkyModel != true)
		{
		skySolver = -64/lightingSolverCustomFactor;
		}else{
		skySolver = 0;
		}
	
		for(var rayIV = skySolver; rayIV < 72/lightingSolverCustomFactor*1.5; rayIV++)
		{
			if(( rayIV < 48/lightingSolverCustomFactor)&&(rayIV > skySolver/lightingSolverCustomFactor)){
				for(var rayIH = 0; rayIH < (120/lightingSolverCustomFactor); rayIH++)
				{
				LHGSLight = new GameObject("SkyRay");
 				LHGSLight.transform.parent = lhgsEmpty.transform;
 				LHGSLight.transform.Translate(lhgsEmpty.transform.position);
 				LHGSLight.transform.Rotate(rayIV*1.25*lightingSolverCustomFactor/1.5,rayIV+rayIH*(3*lightingSolverCustomFactor),0);
				LHGSLight.AddComponent(Light);
    			LHGSLight.GetComponent.<Light>().type = LightType.Directional;
    			LHGSLight.GetComponent.<Light>().shadows = LightShadows.Soft;
    			LHGSLight.hideFlags = HideFlags.HideInInspector;
    			LHGSLight.hideFlags = HideFlags.HideInHierarchy;
    			LHGSLight.GetComponent.<Light>().color = baseParameters.SkySettings.LHGS_SkyColor;
    			lightingModelQuality.sourceOfRays += 1;
    			}
    		}
    	
    		if(( rayIV > 52/lightingSolverCustomFactor*1.5)&&(rayIV < 68/lightingSolverCustomFactor*1.5))
    		{
				for(var rayIHa = 0; rayIHa < (72/lightingSolverCustomFactor); rayIHa++)
				{
				LHGSLight = new GameObject("SkyRay");
 				LHGSLight.transform.parent = lhgsEmpty.transform;
 				LHGSLight.transform.Translate(lhgsEmpty.transform.position);
 				LHGSLight.transform.Rotate(rayIV*1.25*lightingSolverCustomFactor/1.5,rayIV+rayIHa*(5*lightingSolverCustomFactor),0);
				LHGSLight.AddComponent(Light);
    			LHGSLight.GetComponent.<Light>().type = LightType.Directional;
    			LHGSLight.GetComponent.<Light>().shadows = LightShadows.Soft;
    			LHGSLight.hideFlags = HideFlags.HideInInspector;
    			LHGSLight.hideFlags = HideFlags.HideInHierarchy;
    			LHGSLight.GetComponent.<Light>().color = baseParameters.SkySettings.LHGS_SkyColor;
    			lightingModelQuality.sourceOfRays += 1;
    			}
    		}
    	}
    }
    	
    	if(lightDistributionMethod.solvingClassic== true)
    	{
    		if(skySystem.hemisphereSkyModel != true)
			{
			skySolver = -64;
			}else{
			skySolver = 0;
			}
	
			for(var rayCV = skySolver; rayCV < 64/lightingSolverCustomFactor; rayCV++)
			{
				for(var rayCH = 0; rayCH < 120/lightingSolverCustomFactor; rayCH++)
				{
				LHGSLight = new GameObject("SkyRay");
 				LHGSLight.transform.parent = lhgsEmpty.transform;
 				LHGSLight.transform.Translate(lhgsEmpty.transform.position);
 				LHGSLight.transform.Rotate(rayCV*1.25*lightingSolverCustomFactor,rayCV+rayCH*3 *lightingSolverCustomFactor,0);
				LHGSLight.AddComponent(Light);
    			LHGSLight.GetComponent.<Light>().type = LightType.Directional;
    			LHGSLight.GetComponent.<Light>().shadows = LightShadows.Soft;
    			LHGSLight.hideFlags = HideFlags.HideInInspector;
    			LHGSLight.hideFlags = HideFlags.HideInHierarchy;
    			LHGSLight.GetComponent.<Light>().color = baseParameters.SkySettings.LHGS_SkyColor;
    			lightingModelQuality.sourceOfRays += 1;
    			}
    		}
    	}
    	
    	if(lightDistributionMethod.solvingBalanced == true)
    	{
    		if(skySystem.hemisphereSkyModel != true)
			{
			skySolver = -24;
			}else{
			skySolver = 0;
			}
	
			for(var rayBV = skySolver; rayBV < 24/lightingSolverCustomFactor; rayBV++)
			{
				for(var rayBH = 0; rayBH < 180/lightingSolverCustomFactor; rayBH++)
				{
				LHGSLight = new GameObject("SkyRay");
 				LHGSLight.transform.parent = lhgsEmpty.transform;
 				LHGSLight.transform.Translate(lhgsEmpty.transform.position);
 				LHGSLight.transform.Rotate(rayBV*2.5*lightingSolverCustomFactor,rayBV+rayBH*2*lightingSolverCustomFactor,0);
				LHGSLight.AddComponent(Light);
    			LHGSLight.GetComponent.<Light>().type = LightType.Directional;
    			LHGSLight.GetComponent.<Light>().shadows = LightShadows.Soft;
    			LHGSLight.hideFlags = HideFlags.HideInInspector;
    			LHGSLight.hideFlags = HideFlags.HideInHierarchy;
    			LHGSLight.GetComponent.<Light>().color = baseParameters.SkySettings.LHGS_SkyColor;
    			lightingModelQuality.sourceOfRays += 1;
    			}
    		
    		if(( rayBV > 0/lightingSolverCustomFactor*1.5)&&(rayBV < 12/lightingSolverCustomFactor*1.5))
    		{
    			for(var rayBHb = 0; rayBHb < 180/lightingSolverCustomFactor; rayBHb++)
				{
				LHGSLight = new GameObject("SkyRay");
 				LHGSLight.transform.parent = lhgsEmpty.transform;
 				LHGSLight.transform.Translate(lhgsEmpty.transform.position);
 				LHGSLight.transform.Rotate(rayBV*2.5*lightingSolverCustomFactor-5,rayBV+rayBHb*2*lightingSolverCustomFactor,0);
				LHGSLight.AddComponent(Light);
    			LHGSLight.GetComponent.<Light>().type = LightType.Directional;
    			LHGSLight.GetComponent.<Light>().shadows = LightShadows.Soft;
    			LHGSLight.hideFlags = HideFlags.HideInInspector;
    			LHGSLight.hideFlags = HideFlags.HideInHierarchy;
    			LHGSLight.GetComponent.<Light>().color = baseParameters.SkySettings.LHGS_SkyColor;
    			lightingModelQuality.sourceOfRays += 1;
    			}
    		}
		}
	}
    
    
    for(var LHGSLightsIntensityChange : GameObject in GameObject.FindObjectsOfType(GameObject))
	{
    	if(LHGSLightsIntensityChange.name == "SkyRay")
    	{
    	LHGSLightsIntensityChange.GetComponent.<Light>().intensity = (((baseParameters.SkySettings.LHGS_Intensity/1.5)))/lightingModelQuality.sourceOfRays;
    	
    	}
 	}
}


function OnDrawGizmos () {
	 
	var findRays = GameObject.Find("SkyRay");
   		if(findRays != null)
    	{ 
		Gizmos.color = baseParameters.SkySettings.LHGS_SkyColor;	
     	Gizmos.DrawLine (Vector3(transform.position.x,transform.position.y,transform.position.z-1), Vector3(transform.position.x,transform.position.y,transform.position.z+1));
    	Gizmos.DrawLine (Vector3(transform.position.x-1,transform.position.y,transform.position.z), Vector3(transform.position.x+1,transform.position.y,transform.position.z));
    	Gizmos.DrawLine (Vector3(transform.position.x,transform.position.y,transform.position.z), Vector3(transform.position.x,transform.position.y+1,transform.position.z));
    	
    	if(skySystem.hemisphereSkyModel != true)
		{
		Gizmos.DrawLine (Vector3(transform.position.x,transform.position.y-1,transform.position.z), Vector3(transform.position.x,transform.position.y,transform.position.z));
   		}
    	
    	Gizmos.color = baseParameters.SkySettings.LHGS_AmbientLight;
    	
    	if(lightingModelQuality.veryLowQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		}
    	
    	if(lightingModelQuality.LowQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		}
		
		if(lightingModelQuality.StandardQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.28,0.28,0.28));
		}
		
		if(lightingModelQuality.HighQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.28,0.28,0.28));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.26,0.26,0.26));
		}
		
		if(lightingModelQuality.ExtremeQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.28,0.28,0.28));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.26,0.26,0.26));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.24,0.24,0.24));
		}
		
    	}else{
    	Gizmos.color = Color.black;
    	Gizmos.DrawLine (Vector3(transform.position.x,transform.position.y,transform.position.z-1), Vector3(transform.position.x,transform.position.y,transform.position.z+1));
    	Gizmos.DrawLine (Vector3(transform.position.x,transform.position.y-1,transform.position.z), Vector3(transform.position.x,transform.position.y+1,transform.position.z));
    	Gizmos.DrawLine (Vector3(transform.position.x-1,transform.position.y,transform.position.z), Vector3(transform.position.x+1,transform.position.y,transform.position.z));
    	
    	if(lightingModelQuality.LowQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		}
		
		if(lightingModelQuality.StandardQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.28,0.28,0.28));
		}
		
		if(lightingModelQuality.HighQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.28,0.28,0.28));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.26,0.26,0.26));
		}
		
		if(lightingModelQuality.ExtremeQualityModel == true)
		{
		Gizmos.DrawWireCube (transform.position, Vector3 (0.3,0.3,0.3));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.28,0.28,0.28));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.26,0.26,0.26));
		Gizmos.DrawWireCube (transform.position, Vector3 (0.24,0.24,0.24));
		}
    	}
}
    
#endif
//LHGS Lighting Script v1.0 created by: Lukas Hanak - Please support next developing for unity free and do not share this. Thanks! 