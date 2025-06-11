// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.XlsxSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

internal class XlsxSerializator : Excel2016Serializator
{
  private const string VersionValue = "16.0300";

  public override ExcelVersion Version => ExcelVersion.Xlsx;

  internal XlsxSerializator(WorkbookImpl book)
    : base(book)
  {
  }

  protected override void SerializeAppVersion(XmlWriter writer)
  {
    Excel2007Serializator.SerializeElementString(writer, "AppVersion", "16.0300", (string) null);
  }
}
