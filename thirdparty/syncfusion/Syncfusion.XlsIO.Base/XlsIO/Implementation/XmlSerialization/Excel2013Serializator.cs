// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Excel2013Serializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class Excel2013Serializator(WorkbookImpl book) : Excel2010Serializator(book)
{
  private const string VersionValue = "15.0300";

  public override ExcelVersion Version => ExcelVersion.Excel2013;

  protected override void SerializeAppVersion(XmlWriter writer)
  {
    Excel2007Serializator.SerializeElementString(writer, "AppVersion", "15.0300", (string) null);
  }
}
