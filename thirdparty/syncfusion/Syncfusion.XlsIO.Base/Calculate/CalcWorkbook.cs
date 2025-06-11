// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.CalcWorkbook
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

#nullable disable
namespace Syncfusion.Calculate;

[Serializable]
public class CalcWorkbook : ISerializable
{
  private CalcSheetList calcSheetList;
  private CalcEngine engine;
  internal Hashtable idLookUp;
  internal int sheetfamilyID = -1;
  [Obsolete("This field will be removed in a future version. Please use the CalcSheetList property instead.", false)]
  public ArrayList sheetNames;
  private int VersionNumber;

  public CalcWorkbook(CalcSheet[] calcSheets, Hashtable namedRanges)
  {
    this.calcSheetList = new CalcSheetList(calcSheets, this);
    int count = this.calcSheetList.Count;
    this.sheetNames = new ArrayList(count);
    this.idLookUp = new Hashtable();
    this.InitCalcWorkbook(count);
    if (count <= 0)
      return;
    Hashtable hashtable = new Hashtable();
    if (namedRanges != null)
    {
      foreach (string key in (IEnumerable) namedRanges.Keys)
        hashtable.Add((object) key.ToUpper(CultureInfo.InvariantCulture), namedRanges[(object) key]);
    }
    this.engine.NamedRanges = hashtable;
  }

  protected CalcWorkbook(SerializationInfo info, StreamingContext context)
  {
    this.calcSheetList = new CalcSheetList((CalcSheet[]) info.GetValue(nameof (calcSheets), typeof (CalcSheet[])), this);
    Hashtable hashtable = (Hashtable) info.GetValue("namedRanges", typeof (Hashtable));
    int count = this.calcSheetList.Count;
    this.idLookUp = new Hashtable();
    this.sheetNames = new ArrayList(count);
    this.InitCalcWorkbook(count);
    this.engine.NamedRanges = hashtable;
  }

  public CalcSheetList CalcSheetList
  {
    get => this.calcSheetList;
    set => this.calcSheetList = value;
  }

  [Obsolete("This property will be removed in a future version. Please use the CalcSheetList property instead.", false)]
  public CalcSheet[] calcSheets => (CalcSheet[]) this.calcSheetList.ToArray(typeof (CalcSheet));

  public CalcEngine Engine
  {
    get => this.engine;
    set
    {
      if (this.engine != null)
        return;
      this.engine = value;
    }
  }

  public int SheetCount => this.calcSheetList.Count;

  public CalcSheet this[string sheetName]
  {
    get => this.calcSheetList[this.GetSheetID(sheetName)];
    set => this.calcSheetList[this.GetSheetID(sheetName)].data = value.data;
  }

  public CalcSheet this[int sheetIndex]
  {
    get => this.calcSheetList[sheetIndex];
    set => this.calcSheetList[sheetIndex].data = value.data;
  }

  public virtual void CalculateAll()
  {
    foreach (CalcSheet calcSheet in (ArrayList) this.calcSheetList)
      calcSheet.CalculationsSuspended = false;
    foreach (CalcSheet calcSheet in (ArrayList) this.calcSheetList)
    {
      calcSheet.Engine.UpdateCalcID();
      for (int row = 1; row <= calcSheet.RowCount; ++row)
      {
        for (int col = 1; col <= calcSheet.ColCount; ++col)
        {
          object obj = calcSheet[row, col];
          if (obj != null)
          {
            string str = obj.ToString();
            if (str.Length > 0 && (int) str[0] == (int) CalcEngine.FormulaCharacter)
              calcSheet[row, col] = (object) str;
          }
        }
      }
    }
  }

  public void ClearFormulas(CalcSheet sheet)
  {
    DateTime now = DateTime.Now;
    string str = $"!{this.GetSheetID(sheet.Name)}!";
    ArrayList arrayList1 = new ArrayList();
    foreach (string key in (IEnumerable) this.Engine.FormulaInfoTable.Keys)
    {
      if (key.StartsWith(str))
        arrayList1.Add((object) key);
    }
    foreach (string key1 in arrayList1)
    {
      this.Engine.FormulaInfoTable.Remove((object) key1);
      ArrayList arrayList2 = new ArrayList();
      foreach (string key2 in (IEnumerable) this.Engine.DependentCells.Keys)
      {
        if (key2.StartsWith(str))
          arrayList2.Add((object) key2);
      }
      foreach (string key3 in arrayList2)
        this.Engine.DependentCells.Remove((object) key3);
      arrayList2.Clear();
      foreach (string key4 in (IEnumerable) this.Engine.DependentFormulaCells.Keys)
      {
        if (key4.StartsWith(str))
          arrayList2.Add((object) key4);
      }
      foreach (string key5 in arrayList2)
        this.Engine.DependentFormulaCells.Remove((object) key5);
    }
  }

  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("calcSheets", (object) this.calcSheetList.ToArray());
    info.AddValue("namedRanges", (object) this.Engine.NamedRanges);
  }

  public int GetSheetID(string sheetName)
  {
    return !this.idLookUp.ContainsKey((object) sheetName.ToLower()) ? -1 : (int) this.idLookUp[(object) sheetName.ToLower()];
  }

  private void GetSheetRowCol(string key, out int sheet, out int row, out int col)
  {
    key = key.Substring(1);
    int length = key.IndexOf('!');
    sheet = int.Parse(key.Substring(0, length));
    key = key.Substring(length + 1);
    row = this.Engine.RowIndex(key);
    col = this.Engine.ColIndex(key);
  }

  private void InitCalcWorkbook(int sheetCount)
  {
    CalcEngine.ResetSheetFamilyID();
    if (sheetCount <= 0)
      return;
    this.engine = new CalcEngine((ICalcData) this.calcSheetList[0]);
    this.engine.UseDependencies = true;
    this.sheetfamilyID = CalcEngine.CreateSheetFamilyID();
    for (int i = 0; i < sheetCount; ++i)
    {
      string name = this.calcSheetList[i].Name;
      this.engine.RegisterGridAsSheet(name, (ICalcData) this.calcSheetList[i], this.sheetfamilyID);
      this.sheetNames.Add((object) name);
      this.idLookUp.Add((object) name.ToLower(), (object) i);
      this.calcSheetList[i].engine = this.engine;
    }
  }

  public static CalcWorkbook ReadSSS(string fileName)
  {
    CalcWorkbook calcWorkbook = (CalcWorkbook) null;
    try
    {
      using (StreamReader sr = new StreamReader(fileName))
      {
        int.Parse(sr.ReadLine());
        int length = int.Parse(sr.ReadLine());
        int capacity = int.Parse(sr.ReadLine());
        Hashtable namedRanges = new Hashtable(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          string[] strArray = sr.ReadLine().Split('\t');
          namedRanges.Add((object) strArray[0], (object) strArray[1]);
        }
        CalcSheet[] calcSheets = new CalcSheet[length];
        for (int index = 0; index < length; ++index)
          calcSheets[index] = CalcSheet.ReadSSS(sr);
        sr.Close();
        calcWorkbook = new CalcWorkbook(calcSheets, namedRanges);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
    return calcWorkbook;
  }

  public void WriteSSS(string fileName)
  {
    try
    {
      using (StreamWriter sw = new StreamWriter(fileName))
      {
        sw.WriteLine(this.VersionNumber);
        int count = this.calcSheetList.Count;
        sw.WriteLine(count);
        sw.WriteLine(this.Engine.NamedRanges.Count);
        foreach (string key in (IEnumerable) this.Engine.NamedRanges.Keys)
        {
          sw.Write(key);
          sw.Write('\t');
          sw.WriteLine(this.Engine.NamedRanges[(object) key]);
        }
        for (int i = 0; i < count; ++i)
          this.calcSheetList[i].WriteSSS(sw);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine((object) ex);
    }
  }
}
