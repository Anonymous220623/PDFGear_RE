// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.CalcSheetList
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Calculate;

public class CalcSheetList : ArrayList
{
  private static int nextCalcSheetNumber;
  private CalcWorkbook workBook;

  public CalcSheetList()
  {
  }

  public CalcSheetList(CalcSheet[] list, CalcWorkbook parentWorkBook)
  {
    if (list != null)
    {
      foreach (object obj in list)
      {
        base.Add(obj);
        ++CalcSheetList.nextCalcSheetNumber;
      }
    }
    this.workBook = parentWorkBook;
  }

  public CalcSheet this[int i]
  {
    get => (CalcSheet) base[i];
    set => this[i] = (object) value;
  }

  public CalcSheet this[string sheetName]
  {
    get
    {
      int index = this.NameToIndex(sheetName);
      return index != -1 ? (CalcSheet) base[index] : throw new ArgumentOutOfRangeException($"{sheetName} not found.");
    }
    set
    {
      int index = this.NameToIndex(sheetName);
      if (index == -1)
        throw new ArgumentOutOfRangeException($"{sheetName} not found.");
      this[index] = (object) value;
    }
  }

  public new int Add(object o)
  {
    if (!(o is CalcSheet calcSheet))
      throw new ArgumentException("Must add a CalcSheet object");
    CalcEngine calcEngine;
    if (this.Count == 0)
    {
      CalcEngine.ResetSheetFamilyID();
      calcEngine = new CalcEngine((ICalcData) calcSheet);
      calcEngine.UseDependencies = true;
      if (this.workBook.sheetfamilyID == -1)
        this.workBook.sheetfamilyID = CalcEngine.CreateSheetFamilyID();
    }
    else
      calcEngine = this[0].engine;
    calcSheet.engine = calcEngine;
    string name = calcSheet.Name;
    calcEngine.RegisterGridAsSheet(name, (ICalcData) calcSheet, this.workBook.sheetfamilyID);
    this.workBook.sheetNames.Add((object) name);
    int nextCalcSheetNumber = CalcSheetList.nextCalcSheetNumber;
    ++CalcSheetList.nextCalcSheetNumber;
    this.workBook.idLookUp.Add((object) name.ToLower(), (object) nextCalcSheetNumber);
    return base.Add((object) calcSheet);
  }

  public new void Insert(int index, object o) => throw new NotImplementedException(nameof (Insert));

  public new void InsertRange(int index, ICollection c)
  {
    throw new NotImplementedException(nameof (InsertRange));
  }

  public int NameToIndex(string sheetName)
  {
    int index = -1;
    string lower = sheetName.ToLower();
    for (int i = 0; i < this.Count; ++i)
    {
      if (this[i].Name.ToLower() == lower)
      {
        index = i;
        break;
      }
    }
    return index;
  }

  public new void Remove(object o)
  {
    if (!(o is CalcSheet model))
      throw new ArgumentException("Must add a CalcSheet object");
    this.workBook.sheetNames.Remove((object) model.Name);
    this.workBook.idLookUp.Remove((object) model.Name.ToLower());
    GridSheetFamilyItem sheetFamilyItem = CalcEngine.GetSheetFamilyItem((ICalcData) model);
    if (sheetFamilyItem.SheetNameToToken.ContainsKey((object) model.Name.ToUpper()))
      sheetFamilyItem.SheetNameToToken.Remove((object) model.Name.ToUpper());
    base.Remove(o);
  }

  public new void RemoveAt(int index) => this.Remove((object) this[index]);

  public CalcSheet[] ToArray() => (CalcSheet[]) this.ToArray(typeof (CalcSheet));
}
