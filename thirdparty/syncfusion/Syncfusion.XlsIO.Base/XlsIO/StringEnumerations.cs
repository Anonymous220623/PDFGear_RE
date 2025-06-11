// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.StringEnumerations
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO;

public class StringEnumerations
{
  private const string Solid = "solid";
  private const string Dash = "dash";
  private const string DashDot = "dashDot";
  private const string LongDash = "lgDash";
  private const string SystemDash = "sysDash";
  private const string SystemDot = "sysDot";
  private const string LongDashDot = "lgDashDot";
  private const string LongDashDotDot = "lgDashDotDot";
  private Dictionary<string, ExcelShapeDashLineStyle> s_dicLineStyleXmlToEnum = new Dictionary<string, ExcelShapeDashLineStyle>();
  private Dictionary<ExcelShapeDashLineStyle, string> s_dicLineStyleEnumToXml = new Dictionary<ExcelShapeDashLineStyle, string>();

  internal StringEnumerations()
  {
    this.s_dicLineStyleXmlToEnum.Add("solid", ExcelShapeDashLineStyle.Solid);
    this.s_dicLineStyleXmlToEnum.Add("dash", ExcelShapeDashLineStyle.Dashed);
    this.s_dicLineStyleXmlToEnum.Add("dashDot", ExcelShapeDashLineStyle.Dash_Dot);
    this.s_dicLineStyleXmlToEnum.Add("lgDash", ExcelShapeDashLineStyle.Medium_Dashed);
    this.s_dicLineStyleXmlToEnum.Add("sysDash", ExcelShapeDashLineStyle.Dotted);
    this.s_dicLineStyleXmlToEnum.Add("sysDot", ExcelShapeDashLineStyle.Dotted_Round);
    this.s_dicLineStyleXmlToEnum.Add("lgDashDot", ExcelShapeDashLineStyle.Medium_Dash_Dot);
    this.s_dicLineStyleXmlToEnum.Add("lgDashDotDot", ExcelShapeDashLineStyle.Dash_Dot_Dot);
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Solid, "solid");
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Dashed, "dash");
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Dash_Dot, "dashDot");
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Dotted, "sysDash");
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Dotted_Round, "sysDot");
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Medium_Dashed, "lgDash");
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Medium_Dash_Dot, "lgDashDot");
    this.s_dicLineStyleEnumToXml.Add(ExcelShapeDashLineStyle.Dash_Dot_Dot, "lgDashDotDot");
  }

  internal Dictionary<string, ExcelShapeDashLineStyle> LineDashTypeXmltoEnum
  {
    get => this.s_dicLineStyleXmlToEnum;
  }

  internal Dictionary<ExcelShapeDashLineStyle, string> LineDashTypeEnumToXml
  {
    get => this.s_dicLineStyleEnumToXml;
  }
}
