// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.StringEnumerations
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart;

internal class StringEnumerations
{
  private const string Solid = "solid";
  private const string Dash = "dash";
  private const string DashDot = "dashDot";
  private const string LongDash = "lgDash";
  private const string SystemDash = "sysDash";
  private const string SystemDot = "sysDot";
  private const string LongDashDot = "lgDashDot";
  private const string LongDashDotDot = "lgDashDotDot";
  private Dictionary<string, OfficeShapeDashLineStyle> s_dicLineStyleXmlToEnum = new Dictionary<string, OfficeShapeDashLineStyle>();
  private Dictionary<OfficeShapeDashLineStyle, string> s_dicLineStyleEnumToXml = new Dictionary<OfficeShapeDashLineStyle, string>();

  internal StringEnumerations()
  {
    this.s_dicLineStyleXmlToEnum.Add("solid", OfficeShapeDashLineStyle.Solid);
    this.s_dicLineStyleXmlToEnum.Add("dash", OfficeShapeDashLineStyle.Dashed);
    this.s_dicLineStyleXmlToEnum.Add("dashDot", OfficeShapeDashLineStyle.Dash_Dot);
    this.s_dicLineStyleXmlToEnum.Add("lgDash", OfficeShapeDashLineStyle.Medium_Dashed);
    this.s_dicLineStyleXmlToEnum.Add("sysDash", OfficeShapeDashLineStyle.Dotted);
    this.s_dicLineStyleXmlToEnum.Add("sysDot", OfficeShapeDashLineStyle.Dotted_Round);
    this.s_dicLineStyleXmlToEnum.Add("lgDashDot", OfficeShapeDashLineStyle.Medium_Dash_Dot);
    this.s_dicLineStyleXmlToEnum.Add("lgDashDotDot", OfficeShapeDashLineStyle.Dash_Dot_Dot);
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Solid, "solid");
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Dashed, "dash");
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Dash_Dot, "dashDot");
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Dotted, "sysDash");
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Dotted_Round, "sysDot");
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Medium_Dashed, "lgDash");
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Medium_Dash_Dot, "lgDashDot");
    this.s_dicLineStyleEnumToXml.Add(OfficeShapeDashLineStyle.Dash_Dot_Dot, "lgDashDotDot");
  }

  internal Dictionary<string, OfficeShapeDashLineStyle> LineDashTypeXmltoEnum
  {
    get => this.s_dicLineStyleXmlToEnum;
  }

  internal Dictionary<OfficeShapeDashLineStyle, string> LineDashTypeEnumToXml
  {
    get => this.s_dicLineStyleEnumToXml;
  }
}
