// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.PivotCacheFieldsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class PivotCacheFieldsCollection : CollectionBase<PivotCacheFieldImpl>
{
  public PivotCacheFieldImpl this[string name]
  {
    get
    {
      foreach (PivotCacheFieldImpl inner in this.InnerList)
      {
        if (inner.Name == name)
          return inner;
      }
      return (PivotCacheFieldImpl) null;
    }
  }

  [CLSCompliant(false)]
  public void Parse(BiffReader reader, int iFieldsNumber)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    for (int index = 0; index < iFieldsNumber; ++index)
    {
      if (reader.PeekRecordType() != TBIFFRecord.PivotField)
        throw new UnexpectedRecordException();
      this.Add(new PivotCacheFieldImpl(reader));
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int i = 0;
    for (int count = this.Count; i < count; ++i)
      this[i].Serialize(records);
  }

  public int Add(PivotCacheFieldImpl field)
  {
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    base.Add(field);
    int num = this.Count - 1;
    field.Index = num;
    return num;
  }

  public PivotCacheFieldImpl AddNewField(string strName)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty");
      default:
        PivotCacheFieldImpl field = new PivotCacheFieldImpl();
        field.Name = strName;
        if (this[field.Name] != null)
        {
          int num = 1;
          string name;
          for (name = field.Name; this[name] != null; name = field.Name + num.ToString())
            ++num;
          field.Name = name;
        }
        this.Add(field);
        return field;
    }
  }

  public PivotCacheFieldImpl AddNewField(string strName, string formula)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty");
      default:
        PivotCacheFieldImpl field = new PivotCacheFieldImpl();
        field.Name = strName;
        field.Formula = formula;
        this.Add(field);
        return field;
    }
  }

  public int GetOrdinaryFieldCount()
  {
    int ordinaryFieldCount = 0;
    foreach (PivotCacheFieldImpl inner in this.InnerList)
    {
      bool flag = true;
      if (inner.IsFieldGroup)
        flag = !inner.FieldGroup.IsDiscrete;
      if (!inner.IsFormulaField && flag)
        ++ordinaryFieldCount;
    }
    return ordinaryFieldCount;
  }
}
