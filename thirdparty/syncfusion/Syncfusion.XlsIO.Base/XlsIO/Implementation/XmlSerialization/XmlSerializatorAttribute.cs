// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.XmlSerializatorAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

[AttributeUsage(AttributeTargets.Class)]
public sealed class XmlSerializatorAttribute : Attribute
{
  private ExcelXmlSaveType m_saveType;

  private XmlSerializatorAttribute()
  {
  }

  public XmlSerializatorAttribute(ExcelXmlSaveType saveType) => this.m_saveType = saveType;

  public ExcelXmlSaveType SaveType => this.m_saveType;
}
