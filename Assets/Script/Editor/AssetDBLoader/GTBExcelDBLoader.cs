using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class GTBExcelDBLoader : AssetPostprocessor
{
    private static readonly string filePath = "../doc/gtb_data1.xlsx";
    private static readonly string[] sheetNames = { "Vehicles", };
    private static readonly string exportRootPath = "Assets/GameAsset/GTBData";    

    [MenuItem("YP Tools/AssetBundle/ExcelImport")]
    public static void ExcelDBImport()
    {
        using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            IWorkbook book = new XSSFWorkbook(fs);

            if (false == ImportVehicleDB(book.GetSheet("Vehicles")))
            {
                Debug.LogError("[GTBExcelDBLoader] Vehicles sheet not found");
                return;
            }
            if( false == ImportSpecialAbilityDB(book.GetSheet("SpecialAbility")))
            {
                Debug.LogError("[GTBExcelDBLoader] SpecialAbility sheet not found");
                return;
            }
        }
    }

    private static bool ImportVehicleDB(ISheet sheet)
    {
        if (sheet == null)  return false;

        var exportPath = exportRootPath + "/" + sheet.SheetName + AssetConfig.AssetBundleExtension;
        var data = (VehicleDB)AssetDatabase.LoadAssetAtPath(exportPath, typeof(VehicleDB));
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<VehicleDB>();
            AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
            data.hideFlags = HideFlags.NotEditable;
        }
        data.param.Clear();

        // add infomation
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            ICell cell = null;

            var p = new VehicleDB.Param();
            cell = row.GetCell(0); p.ID = GetCellIntValue(cell);
            if (0 == p.ID)
            {
                break;
            }

            cell = row.GetCell(1); p.Name = GetCellStringValue(cell);
            cell = row.GetCell(8); p.ACC = GetCellIntValue(cell);
            cell = row.GetCell(9); p.SPD = GetCellIntValue(cell);
            cell = row.GetCell(10); p.POW = GetCellIntValue(cell);
            cell = row.GetCell(11); p.HP = GetCellIntValue(cell);
            cell = row.GetCell(12); p.SKILL01 = GetCellIntValue(cell);
            cell = row.GetCell(13); p.SKILL02 = GetCellIntValue(cell);
            cell = row.GetCell(14); p.SKILL03 = GetCellIntValue(cell);            
            cell = row.GetCell(15); p.BodyModel = GetCellStringValue(cell);
            cell = row.GetCell(16); p.TireModel = GetCellStringValue(cell);
            cell = row.GetCell(17); p.IntegratedMat = GetCellBooleanValue(cell);
            cell = row.GetCell(18); p.BodyMaterial = GetCellStringValue(cell);
            cell = row.GetCell(19); p.TireMaterial = GetCellStringValue(cell);
            cell = row.GetCell(20); p.WindowMat = GetCellStringValue(cell);
            cell = row.GetCell(21); p.MetalMat = GetCellStringValue(cell);

            p.Log();
            data.param.Add(p);
        }

        // save scriptable object
        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
        EditorUtility.SetDirty(data);
        Debug.Log("[GTBExcelDBLoader] Vehicles DB scriptable-object created.");

        return true;
    }

    private static bool ImportSpecialAbilityDB(ISheet sheet)
    {
        if (null == sheet)
        {
            return false;
        }

        string exportPath = exportRootPath + "/" + sheet.SheetName + AssetConfig.AssetBundleExtension;
        SpecialAbilityDB dbAsset = AssetDatabase.LoadAssetAtPath(exportPath, typeof(SpecialAbilityDB)) as SpecialAbilityDB;
        if (null == dbAsset)
        {
            dbAsset = ScriptableObject.CreateInstance<SpecialAbilityDB>();
            AssetDatabase.CreateAsset((ScriptableObject)dbAsset, exportPath);
            dbAsset.hideFlags = HideFlags.NotEditable;
        }
        dbAsset._data.Clear();
        
        for (int index = 1; index <= sheet.LastRowNum; index++)
        {
            IRow row = sheet.GetRow(index);
            ICell cell = null;
            SpecialAbilityData data = new SpecialAbilityData();

            cell = row.GetCell(0);
            data._id = GetCellIntValue(cell);

            cell = row.GetCell(3);
            data._unlockLevel = GetCellIntValue(cell);

            cell = row.GetCell(4);
            data._price = GetCellIntValue(cell);

            cell = row.GetCell(5);
            data._maxLevel = GetCellIntValue(cell);

            data._levelUpPrice = new int[data._maxLevel - 1];
            for (int lIndex = 1; lIndex < data._maxLevel; ++lIndex)
            {
                cell = row.GetCell(5 + lIndex);
                data._levelUpPrice[lIndex - 1] = GetCellIntValue(cell);
            }

            dbAsset._data.Add(data);
        }        

        EditorUtility.SetDirty(dbAsset);
        Debug.Log("[GTBExcelDBLoader] SpecialABility DB scriptable-object created.");

        return true;
    }

    private static int GetCellIntValue(ICell aCellData)
    {
        if( null == aCellData)
        {
            return 0;
        }

        return (int)aCellData.NumericCellValue;
    }

    private static string GetCellStringValue(ICell aCellData)
    {
        if (null == aCellData)
        {
            return "";
        }

        return aCellData.StringCellValue;
    }

    private static bool GetCellBooleanValue(ICell aCellData)
    {
        if (null == aCellData)
        {
            return false;
        }

        return aCellData.BooleanCellValue;
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook book = null;
                if (Path.GetExtension(filePath) == ".xls")
                {
                    book = new HSSFWorkbook(stream);
                }
                else
                {
                    book = new XSSFWorkbook(stream);
                }

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/ExcelFile/" + sheetName + ".asset";

                    // check scriptable object
                    var data = (VehicleDB)AssetDatabase.LoadAssetAtPath(exportPath, typeof(VehicleDB));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<VehicleDB>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

                    // check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                    // add infomation
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;

                        var p = new VehicleDB.Param();

                        cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(1); p.Name = (cell == null ? "" : cell.StringCellValue);
                        cell = row.GetCell(2); p.SKILL01 = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(3); p.SKILL02 = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(4); p.SKILL03 = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(5); p.ACC = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(6); p.SPD = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(7); p.POW = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(8); p.HP = (int)(cell == null ? 0 : cell.NumericCellValue);
                        cell = row.GetCell(9); p.BodyModel = (cell == null ? "" : cell.StringCellValue);
                        cell = row.GetCell(10); p.TireModel = (cell == null ? "" : cell.StringCellValue);
                        cell = row.GetCell(11); p.IntegratedMat = (cell == null ? false : cell.BooleanCellValue);
                        cell = row.GetCell(12); p.BodyMaterial = (cell == null ? "" : cell.StringCellValue);
                        cell = row.GetCell(13); p.TireMaterial = (cell == null ? "" : cell.StringCellValue);
                        cell = row.GetCell(14); p.WindowMat = (cell == null ? "" : cell.StringCellValue);
                        cell = row.GetCell(15); p.MetalMat = (cell == null ? "" : cell.StringCellValue);

                        data.param.Add(p);
                    }

                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }

}
