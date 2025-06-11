// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.XmlSerializatorFactory
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces.XmlSerialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public sealed class XmlSerializatorFactory
{
  private static Dictionary<int, IXmlSerializator> s_dicSerializators = new Dictionary<int, IXmlSerializator>();

  static XmlSerializatorFactory()
  {
    XmlSerializatorFactory.RegisterXmlSerializator(ExcelXmlSaveType.DLS, typeof (DLSXmlSerializator));
    XmlSerializatorFactory.RegisterXmlSerializator(ExcelXmlSaveType.MSExcel, typeof (WorkbookXmlSerializator));
  }

  private XmlSerializatorFactory()
  {
  }

  public static void RegisterXmlSerializator(ExcelXmlSaveType saveType, Type type)
  {
    IXmlSerializator xmlSerializator = !(type == (Type) null) ? (IXmlSerializator) Activator.CreateInstance(type) : throw new ArgumentNullException(nameof (type));
    XmlSerializatorFactory.s_dicSerializators.Add((int) saveType, xmlSerializator);
  }

  public static IXmlSerializator GetSerializator(ExcelXmlSaveType saveType)
  {
    return XmlSerializatorFactory.s_dicSerializators[(int) saveType];
  }
}
