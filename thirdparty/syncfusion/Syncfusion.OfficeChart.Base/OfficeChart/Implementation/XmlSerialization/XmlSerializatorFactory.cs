// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.XmlSerializatorFactory
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces.XmlSerialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal sealed class XmlSerializatorFactory
{
  private static Dictionary<int, IXmlSerializator> s_dicSerializators = new Dictionary<int, IXmlSerializator>();

  static XmlSerializatorFactory()
  {
    XmlSerializatorFactory.RegisterXmlSerializator(OfficeXmlSaveType.DLS, typeof (DLSXmlSerializator));
    XmlSerializatorFactory.RegisterXmlSerializator(OfficeXmlSaveType.MSExcel, typeof (WorkbookXmlSerializator));
  }

  private XmlSerializatorFactory()
  {
  }

  public static void RegisterXmlSerializator(OfficeXmlSaveType saveType, Type type)
  {
    IXmlSerializator xmlSerializator = !(type == (Type) null) ? (IXmlSerializator) Activator.CreateInstance(type) : throw new ArgumentNullException(nameof (type));
    XmlSerializatorFactory.s_dicSerializators.Add((int) saveType, xmlSerializator);
  }

  public static IXmlSerializator GetSerializator(OfficeXmlSaveType saveType)
  {
    return XmlSerializatorFactory.s_dicSerializators[(int) saveType];
  }
}
