// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Excel2010Serializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal class Excel2010Serializator(WorkbookImpl book) : Excel2007Serializator(book)
{
  private const string VersionValue = "14.0300";
  public const string DataBarUri = "{B025F937-C7B1-47D3-B67F-A62EFF666E3E}";
  public const string DataBarExtUri = "{78C0D931-6437-407d-A8EE-F0AAD7539E65}";

  public override OfficeVersion Version => OfficeVersion.Excel2010;

  protected override void SerilaizeExtensions(XmlWriter writer, WorksheetImpl sheet)
  {
  }

  public void SerializeSparklineGroups(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteAttributeString("xmlns", "xm", (string) null, "http://schemas.microsoft.com/office/excel/2006/main");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  protected override void SerializeAppVersion(XmlWriter writer)
  {
    Excel2007Serializator.SerializeElementString(writer, "AppVersion", "14.0300", (string) null);
  }

  public new void SerializeRgbColor(XmlWriter writer, string tagName, ChartColor color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    int num = color.Value;
    writer.WriteStartElement("x14", tagName, (string) null);
    writer.WriteAttributeString("rgb", num.ToString("X8"));
    Excel2007Serializator.SerializeAttribute(writer, "tint", color.Tint, 0.0);
    writer.WriteEndElement();
  }
}
