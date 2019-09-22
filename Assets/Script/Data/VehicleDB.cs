using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VehicleDB : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int ID;
		public string Name;
		public int SKILL01;
		public int SKILL02;
		public int SKILL03;
		public int ACC;
		public int SPD;
		public int POW;
		public int HP;
		public string BodyModel;
		public string TireModel;
		public bool IntegratedMat;
		public string BodyMaterial;
		public string TireMaterial;
		public string WindowMat;
		public string MetalMat;

        public void Log()
        {
            YPLog.Log("==== vechicle DB Param [ID = " + ID + "] ====");
            YPLog.Log("Name = " + Name + ", skill 1 = " + SKILL01 + ", skill 2 = " + SKILL02 + ", skill 3 = " + SKILL03);
            YPLog.Log("ACC = " + ACC + ", SPD = " + SPD + ", POW = " + POW + ", HP = " + HP);
            YPLog.Log("BodyModel = " + BodyModel + ", TireModel = " + TireModel + ", IntegratedMat = " + IntegratedMat);
            YPLog.Log("BodyMaterial = " + BodyMaterial + ", TireMaterial = " + TireMaterial + ", WindowMat = " + WindowMat + ", MetalMat = " + MetalMat);
        }
    }
}