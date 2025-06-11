// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Excel2013Serializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal class Excel2013Serializator(WorkbookImpl book) : Excel2010Serializator(book)
{
  private const string VersionValue = "15.0300";

  public override OfficeVersion Version => OfficeVersion.Excel2013;

  protected override void SerializeAppVersion(XmlWriter writer)
  {
    Excel2007Serializator.SerializeElementString(writer, "AppVersion", "15.0300", (string) null);
  }
}
