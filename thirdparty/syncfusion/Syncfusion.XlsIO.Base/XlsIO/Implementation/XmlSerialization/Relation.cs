// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Relation
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class Relation : ICloneable
{
  private string m_strTarget;
  private string m_strType;
  private bool m_bIsExternal;

  private Relation()
  {
  }

  public Relation(string target, string type)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    if (type == null)
      throw new ArgumentNullException(nameof (type));
    this.m_strTarget = target;
    this.m_strType = type;
  }

  public Relation(string target, string type, bool isExternal)
    : this(target, type)
  {
    this.m_bIsExternal = isExternal;
  }

  public string Target => this.m_strTarget;

  public string Type => this.m_strType;

  public bool IsExternal => this.m_bIsExternal;

  public object Clone() => this.MemberwiseClone();
}
