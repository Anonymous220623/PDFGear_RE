// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.FieldInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class FieldInfo : IComparable
{
  public FieldTypes FieldType { get; set; }

  public string Name { get; set; }

  public string Expression { get; set; }

  public string Format { get; set; }

  public override bool Equals(object obj)
  {
    switch (obj)
    {
      case FieldInfo fieldInfo:
        return this.Name == fieldInfo.Name;
      case string _:
        return this.Name == obj.ToString();
      default:
        return false;
    }
  }

  public override int GetHashCode() => this.Name != null ? this.Name.GetHashCode() : 0;

  public int CompareTo(object obj) => obj == null ? 1 : this.Name.CompareTo(((FieldInfo) obj).Name);
}
