// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.CalcSheet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Syncfusion.Calculate;

[Serializable]
public class CalcSheet : ISheetData, ICalcData, ISerializable
{
  private bool calcSuspended = true;
  internal object[,] data;
  [ThreadStatic]
  private static char delimiter = '\t';
  internal CalcEngine engine;
  private bool inSerialization;
  private bool lockSheetChanges;
  private string name = "";
  private int VersionNumber;

  public CalcSheet() => this.data = (object[,]) null;

  public CalcSheet(int rows, int cols) => this.data = new object[rows, cols];

  protected CalcSheet(SerializationInfo info, StreamingContext context)
  {
    this.name = (string) info.GetValue(nameof (name), typeof (string));
    int length1 = (int) info.GetValue("rowCount", typeof (int));
    int length2 = (int) info.GetValue("colCount", typeof (int));
    this.data = new object[length1, length2];
    string str = (string) info.GetValue(nameof (data), typeof (string));
    int startIndex = 0;
    for (int index1 = 0; index1 < length1; ++index1)
    {
      for (int index2 = 0; index2 < length2; ++index2)
      {
        int length3 = str.Substring(startIndex).IndexOf(CalcSheet.delimiter);
        this.data[index1, index2] = (object) str.Substring(startIndex, length3);
        startIndex = startIndex + length3 + 1;
      }
    }
  }

  public event ValueChangedEventHandler CalculatedValueChanged;

  public event ValueChangedEventHandler ValueChanged;

  public bool CalculationsSuspended
  {
    get => this.calcSuspended;
    set => this.calcSuspended = value;
  }

  public int ColCount => this.data.GetLength(1);

  public static char Delimter
  {
    get => CalcSheet.delimiter;
    set => CalcSheet.delimiter = value;
  }

  public CalcEngine Engine
  {
    get => this.engine;
    set => this.Engine = value;
  }

  public bool LockSheetChanges
  {
    get => this.lockSheetChanges;
    set => this.lockSheetChanges = value;
  }

  public string Name
  {
    get => this.name;
    set => this.name = value;
  }

  public int RowCount => this.data.GetLength(0);

  public object this[int row, int col]
  {
    get => this.GetValueRowCol(row, col);
    set => this.SetValue(row, col, value.ToString());
  }

  public static CalcSheet CreateSheetFromFile(string fileName)
  {
    CalcSheet sheetFromFile = (CalcSheet) null;
    try
    {
      using (StreamReader sr = new StreamReader(fileName))
      {
        int.Parse(sr.ReadLine());
        sheetFromFile = CalcSheet.ReadSSS(sr);
        sr.Close();
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
    return sheetFromFile;
  }

  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("name", (object) this.name);
    info.AddValue("rowCount", this.RowCount);
    info.AddValue("colCount", this.ColCount);
    this.inSerialization = true;
    StringBuilder stringBuilder = new StringBuilder();
    for (int row = 1; row <= this.RowCount; ++row)
    {
      for (int col = 1; col <= this.ColCount; ++col)
      {
        object valueRowCol = this.GetValueRowCol(row, col);
        stringBuilder.AppendFormat("{0}{1}", valueRowCol, (object) CalcSheet.delimiter);
      }
    }
    info.AddValue("data", (object) stringBuilder.ToString());
    this.inSerialization = false;
  }

  public virtual object GetValueRowCol(int row, int col)
  {
    if (row > this.data.GetLength(0) || col > this.data.GetLength(1))
      return (object) "0";
    if (this.inSerialization && this.engine != null)
    {
      string formulaRowCol = this.engine.GetFormulaRowCol((ICalcData) this, row, col);
      if (formulaRowCol.Length > 0)
        return (object) formulaRowCol;
    }
    return this.data[row - 1, col - 1];
  }

  protected virtual void OnCalculatedValueChanged(ValueChangedEventArgs e)
  {
    if (this.CalculatedValueChanged == null)
      return;
    this.CalculatedValueChanged((object) this, e);
  }

  protected virtual void OnValueChanged(ValueChangedEventArgs e)
  {
    if (this.ValueChanged == null)
      return;
    this.ValueChanged((object) this, e);
  }

  public static CalcSheet ReadSSS(StreamReader sr)
  {
    int rows = int.Parse(sr.ReadLine());
    int cols = int.Parse(sr.ReadLine());
    CalcSheet calcSheet = new CalcSheet(rows, cols);
    calcSheet.name = sr.ReadLine();
    for (int index1 = 0; index1 < rows; ++index1)
    {
      string str1 = sr.ReadLine();
      int index2 = 0;
      string str2 = str1;
      char[] chArray = new char[1]{ CalcSheet.delimiter };
      foreach (string str3 in str2.Split(chArray))
      {
        calcSheet.data[index1, index2] = (object) str3;
        ++index2;
      }
    }
    return calcSheet;
  }

  public virtual void SetValue(int row, int col, string val)
  {
    if (this.LockSheetChanges)
      return;
    this.SetValueRowCol((object) val, row, col);
    if (this.CalculationsSuspended)
      return;
    this.Engine.GetFormulaText(ref val);
    this.OnValueChanged(new ValueChangedEventArgs(row, col, val));
  }

  public virtual void SetValueRowCol(object value, int row, int col)
  {
    if (this.LockSheetChanges)
      return;
    this.data[row - 1, col - 1] = value;
    if (this.CalculatedValueChanged == null)
      return;
    this.OnCalculatedValueChanged(new ValueChangedEventArgs(row, col, value.ToString()));
  }

  public virtual void WireParentObject()
  {
  }

  public int GetFirstRow() => 1;

  public int GetLastRow() => this.RowCount;

  public int GetRowCount() => this.RowCount;

  public int GetFirstColumn() => 1;

  public int GetLastColumn() => this.ColCount;

  public int GetColumnCount() => this.ColCount;

  public void WriteSheetToFile(string fileName)
  {
    try
    {
      using (StreamWriter sw = new StreamWriter(fileName))
      {
        sw.WriteLine(this.VersionNumber);
        this.WriteSSS(sw);
        sw.Close();
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine((object) ex);
    }
  }

  public void WriteSSS(StreamWriter sw, bool valuesOnly)
  {
    int length1 = this.data.GetLength(0);
    int length2 = this.data.GetLength(1);
    sw.WriteLine(length1);
    sw.WriteLine(length2);
    sw.WriteLine(this.name);
    for (int index1 = 0; index1 < length1; ++index1)
    {
      for (int index2 = 0; index2 < length2; ++index2)
      {
        if (index2 > 0)
          sw.Write(CalcSheet.delimiter);
        if (!valuesOnly)
        {
          string formulaRowCol = this.engine.GetFormulaRowCol((ICalcData) this, index1 + 1, index2 + 1);
          if (formulaRowCol.Length > 0)
          {
            sw.Write(formulaRowCol);
            continue;
          }
        }
        sw.Write(this.data[index1, index2]);
      }
      sw.WriteLine("");
    }
  }

  public void WriteSSS(StreamWriter sw) => this.WriteSSS(sw, false);

  public void WriteValuesToFile(string fileName)
  {
    try
    {
      using (StreamWriter sw = new StreamWriter(fileName))
      {
        this.WriteSSS(sw, true);
        sw.Close();
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine((object) ex);
    }
  }
}
