// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartSerializatorCommon
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Drawing;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Interfaces.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartSerializatorCommon
{
  private static Dictionary<OfficeChartLinePattern, KeyValuePair<string, string>> s_dicLinePatterns = new Dictionary<OfficeChartLinePattern, KeyValuePair<string, string>>();
  public static string[][] OuterAttributeArray = new string[9][];
  public static string[][] InnerAttributeArray = new string[9][];
  public static string[][] PerspectiveAttributeArray = new string[6][];
  public static string[][] BevelProperties = new string[13][];
  public static string[][] MaterialProperties = new string[11][];
  public static string[][] LightingProperties = new string[15][];

  static ChartSerializatorCommon()
  {
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.Solid, new KeyValuePair<string, string>("solid", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.Dash, new KeyValuePair<string, string>("dash", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.Dot, new KeyValuePair<string, string>("sysDash", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.CircleDot, new KeyValuePair<string, string>("sysDot", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.DashDot, new KeyValuePair<string, string>("dashDot", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.DashDotDot, new KeyValuePair<string, string>("lgDashDotDot", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.LongDash, new KeyValuePair<string, string>("lgDash", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.LongDashDot, new KeyValuePair<string, string>("lgDashDot", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.LongDashDotDot, new KeyValuePair<string, string>("lgDashDotDot", string.Empty));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.DarkGray, new KeyValuePair<string, string>("solid", "pct75"));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.MediumGray, new KeyValuePair<string, string>("solid", "pct50"));
    ChartSerializatorCommon.s_dicLinePatterns.Add(OfficeChartLinePattern.LightGray, new KeyValuePair<string, string>("solid", "pct25"));
    ChartSerializatorCommon.OuterAttributeArray[0] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "null",
      "l",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[1] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "2700000",
      "tl",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[2] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "5400000",
      "t",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[3] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "13500000",
      "br",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[4] = new string[7]
    {
      "63500",
      "102000",
      "102000",
      "null",
      "null",
      "ctr",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[5] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "16200000",
      "null",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[6] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "10800000",
      "r",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[7] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "18900000",
      "bl",
      "0"
    };
    ChartSerializatorCommon.OuterAttributeArray[8] = new string[7]
    {
      "50800",
      "null",
      "null",
      "38100",
      "8100000",
      "tr",
      "0"
    };
    ChartSerializatorCommon.InnerAttributeArray[0] = new string[3]
    {
      "63500",
      "50800",
      "8100000"
    };
    ChartSerializatorCommon.InnerAttributeArray[1] = new string[3]
    {
      "63500",
      "50800",
      "16200000"
    };
    ChartSerializatorCommon.InnerAttributeArray[2] = new string[3]
    {
      "63500",
      "50800",
      "null"
    };
    ChartSerializatorCommon.InnerAttributeArray[3] = new string[3]
    {
      "63500",
      "50800",
      "10800000"
    };
    ChartSerializatorCommon.InnerAttributeArray[4] = new string[3]
    {
      "63500",
      "50800",
      "18900000"
    };
    ChartSerializatorCommon.InnerAttributeArray[5] = new string[3]
    {
      "63500",
      "50800",
      "2700000"
    };
    ChartSerializatorCommon.InnerAttributeArray[6] = new string[3]
    {
      "114300",
      "null",
      "null"
    };
    ChartSerializatorCommon.InnerAttributeArray[7] = new string[3]
    {
      "63500",
      "50800",
      "5400000"
    };
    ChartSerializatorCommon.InnerAttributeArray[8] = new string[3]
    {
      "63500",
      "50800",
      "13500000"
    };
    ChartSerializatorCommon.PerspectiveAttributeArray[0] = new string[8]
    {
      "null",
      "null",
      "null",
      "null",
      "null",
      "null",
      "null",
      "null"
    };
    ChartSerializatorCommon.PerspectiveAttributeArray[1] = new string[8]
    {
      "76200",
      "18900000",
      "null",
      "23000",
      "null",
      "-1200000",
      "bl",
      "0"
    };
    ChartSerializatorCommon.PerspectiveAttributeArray[2] = new string[8]
    {
      "76200",
      "2700000",
      "12700",
      "-23000",
      "null",
      "-800400",
      "bl",
      "0"
    };
    ChartSerializatorCommon.PerspectiveAttributeArray[3] = new string[8]
    {
      "76200",
      "13500000",
      "null",
      "23000",
      "null",
      "1200000",
      "br",
      "0"
    };
    ChartSerializatorCommon.PerspectiveAttributeArray[4] = new string[8]
    {
      "76200",
      "8100000",
      "12700",
      "-23000",
      "null",
      "800400",
      "br",
      "0"
    };
    ChartSerializatorCommon.PerspectiveAttributeArray[5] = new string[8]
    {
      "152400",
      "5400000",
      "317500",
      "-19000",
      "90000",
      "null",
      "null",
      "0"
    };
    ChartSerializatorCommon.BevelProperties[0] = new string[3]
    {
      "null",
      "null",
      "null"
    };
    ChartSerializatorCommon.BevelProperties[1] = new string[3]
    {
      "null",
      "null",
      "angle"
    };
    ChartSerializatorCommon.BevelProperties[2] = new string[3]
    {
      "114300",
      "null",
      "artDeco"
    };
    ChartSerializatorCommon.BevelProperties[3] = new string[3]
    {
      "null",
      "null",
      "null"
    };
    ChartSerializatorCommon.BevelProperties[4] = new string[3]
    {
      "null",
      "null",
      "convex"
    };
    ChartSerializatorCommon.BevelProperties[5] = new string[3]
    {
      "165100",
      "null",
      "coolSlant"
    };
    ChartSerializatorCommon.BevelProperties[6] = new string[3]
    {
      "139700",
      "null",
      "cross"
    };
    ChartSerializatorCommon.BevelProperties[7] = new string[3]
    {
      "139700",
      "139700",
      "divot"
    };
    ChartSerializatorCommon.BevelProperties[8] = new string[3]
    {
      "114300",
      "null",
      "hardEdge"
    };
    ChartSerializatorCommon.BevelProperties[9] = new string[3]
    {
      "null",
      "null",
      "relaxedInset"
    };
    ChartSerializatorCommon.BevelProperties[10] = new string[3]
    {
      "101600",
      "null",
      "riblet"
    };
    ChartSerializatorCommon.BevelProperties[11] = new string[3]
    {
      "null",
      "null",
      "slope"
    };
    ChartSerializatorCommon.BevelProperties[12] = new string[3]
    {
      "152400",
      "50800",
      "softRound"
    };
    ChartSerializatorCommon.MaterialProperties[0] = new string[1]
    {
      "matte"
    };
    ChartSerializatorCommon.MaterialProperties[1] = new string[1]
    {
      "null"
    };
    ChartSerializatorCommon.MaterialProperties[2] = new string[1]
    {
      "plastic"
    };
    ChartSerializatorCommon.MaterialProperties[3] = new string[1]
    {
      "metal"
    };
    ChartSerializatorCommon.MaterialProperties[4] = new string[1]
    {
      "dkEdge"
    };
    ChartSerializatorCommon.MaterialProperties[5] = new string[1]
    {
      "softEdge"
    };
    ChartSerializatorCommon.MaterialProperties[6] = new string[1]
    {
      "flat"
    };
    ChartSerializatorCommon.MaterialProperties[7] = new string[1]
    {
      "legacyWireframe"
    };
    ChartSerializatorCommon.MaterialProperties[8] = new string[1]
    {
      "powder"
    };
    ChartSerializatorCommon.MaterialProperties[9] = new string[1]
    {
      "translucentPowder"
    };
    ChartSerializatorCommon.MaterialProperties[10] = new string[1]
    {
      "matte"
    };
    ChartSerializatorCommon.LightingProperties[0] = new string[1]
    {
      "threePt"
    };
    ChartSerializatorCommon.LightingProperties[1] = new string[1]
    {
      "balanced"
    };
    ChartSerializatorCommon.LightingProperties[2] = new string[1]
    {
      "brightRoom"
    };
    ChartSerializatorCommon.LightingProperties[3] = new string[1]
    {
      "chilly"
    };
    ChartSerializatorCommon.LightingProperties[4] = new string[1]
    {
      "contrasting"
    };
    ChartSerializatorCommon.LightingProperties[5] = new string[1]
    {
      "flat"
    };
    ChartSerializatorCommon.LightingProperties[6] = new string[1]
    {
      "flood"
    };
    ChartSerializatorCommon.LightingProperties[7] = new string[1]
    {
      "freezing"
    };
    ChartSerializatorCommon.LightingProperties[8] = new string[1]
    {
      "glow"
    };
    ChartSerializatorCommon.LightingProperties[9] = new string[1]
    {
      "harsh"
    };
    ChartSerializatorCommon.LightingProperties[10] = new string[1]
    {
      "morning"
    };
    ChartSerializatorCommon.LightingProperties[11] = new string[1]
    {
      "soft"
    };
    ChartSerializatorCommon.LightingProperties[12] = new string[1]
    {
      "sunrise"
    };
    ChartSerializatorCommon.LightingProperties[13] = new string[1]
    {
      "sunset"
    };
    ChartSerializatorCommon.LightingProperties[14] = new string[1]
    {
      "twoPt"
    };
  }

  [SecurityCritical]
  public static void SerializeFrameFormat(
    XmlWriter writer,
    IOfficeChartFillBorder format,
    ChartImpl chart,
    bool isRoundCorners)
  {
    ChartSerializatorCommon.SerializeFrameFormat(writer, format, chart, isRoundCorners, false);
  }

  [SecurityCritical]
  public static void SerializeFrameFormat(
    XmlWriter writer,
    IOfficeChartFillBorder format,
    ChartImpl chart,
    bool isRoundCorners,
    bool serializeLineAutoValues)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (format == null)
      return;
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    RelationCollection relations = chart.Relations;
    ChartSerializatorCommon.SerializeFrameFormat(writer, format, parentHolder, relations, isRoundCorners, serializeLineAutoValues);
  }

  [SecurityCritical]
  public static void SerializeFrameFormat(
    XmlWriter writer,
    IOfficeChartFillBorder format,
    FileDataHolder holder,
    RelationCollection relations,
    bool isRoundCorners,
    bool serilaizeLineAutoValues)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      return;
    if (ChartImpl.IsChartExSerieType(((ChartImpl) (format as CommonObject).FindParent(typeof (ChartImpl))).ChartType))
      writer.WriteStartElement("spPr", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    else
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (format.HasInterior)
    {
      if (!format.Interior.UseAutomaticFormat && (format.Interior.Pattern != OfficePattern.None || format.Fill.FillType != OfficeFillType.Pattern && format.Fill.FillType != OfficeFillType.SolidColor))
      {
        if (format.Fill is IInternalFill fill)
          ChartSerializatorCommon.SerializeFill(writer, fill, holder, relations);
      }
      else if (format.Interior.Pattern == OfficePattern.None)
        writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    }
    if (format.HasLineProperties)
    {
      IOfficeChartBorder lineProperties = format.LineProperties;
      if (!lineProperties.AutoFormat)
        ChartSerializatorCommon.SerializeLineProperties(writer, lineProperties, isRoundCorners, (IWorkbook) holder.Workbook, serilaizeLineAutoValues);
    }
    if (format.HasShadowProperties)
    {
      IShadow shadow = format.Shadow;
      ChartSerializatorCommon.SerializeShadow(writer, shadow, format.Shadow.HasCustomShadowStyle);
    }
    else
    {
      IShadow shadow = format.Shadow;
      ChartSerializatorCommon.SerializeShadow(writer, shadow, format.Shadow.HasCustomShadowStyle);
    }
    if (format.Has3dProperties)
    {
      IThreeDFormat threeD = format.ThreeD;
      ChartSerializatorCommon.Serialize3D(writer, threeD);
    }
    writer.WriteEndElement();
  }

  public static void SerializeShadow(XmlWriter writer, IShadow shadow, bool CustomShadow)
  {
    if (shadow.ShadowInnerPresets != Office2007ChartPresetsInner.NoShadow)
    {
      int inner = (int) (shadow.ShadowInnerPresets - 1);
      ChartSerializatorCommon.SerializeInner(writer, inner, CustomShadow, shadow);
    }
    else if (shadow.ShadowOuterPresets != Office2007ChartPresetsOuter.NoShadow)
    {
      int outer = (int) (shadow.ShadowOuterPresets - 1);
      ChartSerializatorCommon.SerailizeOuter(writer, outer, CustomShadow, shadow);
    }
    else if (shadow.ShadowPerspectivePresets != Office2007ChartPresetsPerspective.NoShadow)
    {
      int perspectivePresets = (int) shadow.ShadowPerspectivePresets;
      ChartSerializatorCommon.SerializePerspective(writer, perspectivePresets, CustomShadow, shadow);
    }
    else
    {
      if (!shadow.HasCustomShadowStyle)
        return;
      writer.WriteStartElement("effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      ChartSerializatorCommon.SerializeCustomShadowProperties(writer, shadow);
      writer.WriteEndElement();
    }
  }

  internal static void SerializeCustomShadowProperties(XmlWriter writer, IShadow shadow)
  {
    if (shadow.Blur == 0 && shadow.Size == 0 && shadow.Distance == 0 && shadow.Angle == 0 && shadow.Transparency == -1)
      return;
    writer.WriteStartElement("outerShdw", "http://schemas.openxmlformats.org/drawingml/2006/main");
    string[] strArray = new string[8]
    {
      "50800",
      "100000",
      "100000",
      "50800",
      "5400000",
      "ctr",
      "0",
      "43137"
    };
    writer.WriteAttributeString("blurRad", shadow.Blur != 0 ? (shadow.Blur * 12700).ToString() : strArray[0]);
    writer.WriteAttributeString("sx", shadow.Size != 0 ? (shadow.Size * 1000).ToString() : strArray[1]);
    writer.WriteAttributeString("sy", shadow.Size != 0 ? (shadow.Size * 1000).ToString() : strArray[2]);
    writer.WriteAttributeString("dist", shadow.Distance != 0 ? (shadow.Distance * 12700).ToString() : strArray[3]);
    writer.WriteAttributeString("dir", shadow.Angle != 0 ? (shadow.Angle * 60000).ToString() : strArray[4]);
    writer.WriteAttributeString("algn", strArray[5]);
    writer.WriteAttributeString("rotWithShape", strArray[6].ToString());
    writer.WriteStartElement("srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", (shadow.ShadowColor.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6"));
    writer.WriteStartElement("alpha", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", shadow.Transparency != -1 ? ((100 - shadow.Transparency) * 1000).ToString() : strArray[7]);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerializeInner(
    XmlWriter writer,
    int inner,
    bool CustomShadow,
    IShadow Shadow)
  {
    writer.WriteStartElement("effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("innerShdw", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (CustomShadow)
      writer.WriteAttributeString("blurRad", (Shadow.Blur * 12700).ToString());
    else
      writer.WriteAttributeString("blurRad", ChartSerializatorCommon.InnerAttributeArray[inner][0].ToString());
    if (CustomShadow)
      writer.WriteAttributeString("dist", (Shadow.Distance * 12700).ToString());
    else if (!ChartSerializatorCommon.InnerAttributeArray[inner][1].Equals("null"))
      writer.WriteAttributeString("dist", ChartSerializatorCommon.InnerAttributeArray[inner][1].ToString());
    if (CustomShadow)
      writer.WriteAttributeString("dir", (Shadow.Angle * 60000).ToString());
    else if (!ChartSerializatorCommon.InnerAttributeArray[inner][2].Equals("null"))
      writer.WriteAttributeString("dir", ChartSerializatorCommon.InnerAttributeArray[inner][2].ToString());
    writer.WriteStartElement("srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", (Shadow.ShadowColor.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6"));
    if (Shadow.Transparency != -1)
    {
      writer.WriteStartElement("alpha", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", ((100 - Shadow.Transparency) * 1000).ToString());
      writer.WriteEndElement();
    }
    else if (!CustomShadow && inner != 6)
    {
      writer.WriteStartElement("alpha", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", "50000");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerailizeOuter(
    XmlWriter writer,
    int outer,
    bool CustomShadow,
    IShadow Shadow)
  {
    writer.WriteStartElement("effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("outerShdw", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (CustomShadow)
      writer.WriteAttributeString("blurRad", (Shadow.Blur * 12700).ToString());
    else
      writer.WriteAttributeString("blurRad", ChartSerializatorCommon.OuterAttributeArray[outer][0].ToString());
    if (CustomShadow)
    {
      if (Shadow.Size != 0)
      {
        writer.WriteAttributeString("sx", (Shadow.Size * 1000).ToString());
        writer.WriteAttributeString("sy", (Shadow.Size * 1000).ToString());
      }
    }
    else
    {
      if (!ChartSerializatorCommon.OuterAttributeArray[outer][1].Equals("null"))
        writer.WriteAttributeString("sx", ChartSerializatorCommon.OuterAttributeArray[outer][1].ToString());
      if (!ChartSerializatorCommon.OuterAttributeArray[outer][2].Equals("null"))
        writer.WriteAttributeString("sy", ChartSerializatorCommon.OuterAttributeArray[outer][2].ToString());
    }
    if (CustomShadow)
      writer.WriteAttributeString("dist", (Shadow.Distance * 12700).ToString());
    else if (!ChartSerializatorCommon.OuterAttributeArray[outer][3].Equals("null"))
      writer.WriteAttributeString("dist", ChartSerializatorCommon.OuterAttributeArray[outer][3].ToString());
    if (CustomShadow)
      writer.WriteAttributeString("dir", (Shadow.Angle * 60000).ToString());
    else if (!ChartSerializatorCommon.OuterAttributeArray[outer][4].Equals("null"))
      writer.WriteAttributeString("dir", ChartSerializatorCommon.OuterAttributeArray[outer][4].ToString());
    if (!ChartSerializatorCommon.OuterAttributeArray[outer][5].Equals("null"))
      writer.WriteAttributeString("algn", ChartSerializatorCommon.OuterAttributeArray[outer][5].ToString());
    writer.WriteAttributeString("rotWithShape", ChartSerializatorCommon.OuterAttributeArray[outer][6].ToString());
    writer.WriteStartElement("srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", (Shadow.ShadowColor.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6"));
    ChartSerializatorCommon.SerializeSchemeColor(writer, Shadow);
    if (Shadow.Transparency != -1)
    {
      writer.WriteStartElement("alpha", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", ((100 - Shadow.Transparency) * 1000).ToString());
      writer.WriteEndElement();
    }
    else if (!CustomShadow)
    {
      writer.WriteStartElement("alpha", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", "40000");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private static void SerializeSchemeColor(XmlWriter writer, IShadow shadow)
  {
    ChartColor colorObject = (shadow as ShadowImpl).ColorObject;
    if (colorObject.Tint > 0.0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "tint", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.Tint);
    if (colorObject.Saturation > 0.0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "satMod", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.Saturation);
    if (colorObject.Luminance > 0.0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "lumMod", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.Luminance);
    if (colorObject.LuminanceOffSet <= 0.0)
      return;
    ChartSerializatorCommon.SerializeDoubleValueTag(writer, "lumOff", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.LuminanceOffSet);
  }

  public static void SerializePerspective(
    XmlWriter writer,
    int perspective,
    bool CustomShadow,
    IShadow Shadow)
  {
    writer.WriteStartElement("effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("outerShdw", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (CustomShadow)
    {
      writer.WriteAttributeString("blurRad", (Shadow.Blur * 12700).ToString());
      writer.WriteAttributeString("dir", (Shadow.Angle * 60000).ToString());
    }
    else
    {
      writer.WriteAttributeString("blurRad", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][0].ToString());
      writer.WriteAttributeString("dir", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][1].ToString());
    }
    if (CustomShadow)
      writer.WriteAttributeString("dist", (Shadow.Distance * 12700).ToString());
    else if (!ChartSerializatorCommon.PerspectiveAttributeArray[perspective][2].Equals("null"))
      writer.WriteAttributeString("dist", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][2].ToString());
    if (CustomShadow)
    {
      if (Shadow.Size != 0)
      {
        writer.WriteAttributeString("sy", (Shadow.Size * 1000).ToString());
        writer.WriteAttributeString("sx", (Shadow.Size * 1000).ToString());
      }
    }
    else
    {
      if (!ChartSerializatorCommon.PerspectiveAttributeArray[perspective][3].Equals("null"))
        writer.WriteAttributeString("sy", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][3].ToString());
      if (!ChartSerializatorCommon.PerspectiveAttributeArray[perspective][4].Equals("null"))
        writer.WriteAttributeString("sx", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][4].ToString());
    }
    if (!ChartSerializatorCommon.PerspectiveAttributeArray[perspective][5].Equals("null"))
      writer.WriteAttributeString("kx", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][5].ToString());
    if (!ChartSerializatorCommon.PerspectiveAttributeArray[perspective][6].Equals("null"))
      writer.WriteAttributeString("algn", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][6].ToString());
    writer.WriteAttributeString("rotWithShape", ChartSerializatorCommon.PerspectiveAttributeArray[perspective][7].ToString());
    writer.WriteStartElement("srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", (Shadow.ShadowColor.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6"));
    if (Shadow.Transparency != -1)
    {
      writer.WriteStartElement("alpha", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", ((100 - Shadow.Transparency) * 1000).ToString());
      writer.WriteEndElement();
    }
    else if (!CustomShadow)
    {
      writer.WriteStartElement("alpha", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (perspective != 4)
        writer.WriteAttributeString("val", "20000");
      else
        writer.WriteAttributeString("val", "15000");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void Serialize3D(XmlWriter writer, IThreeDFormat Three_D)
  {
    bool flag1 = Three_D.BevelBottom == Office2007ChartBevelProperties.NoAngle;
    bool flag2 = Three_D.BevelTop == Office2007ChartBevelProperties.NoAngle;
    bool flag3 = Three_D.Material == Office2007ChartMaterialProperties.NoEffect;
    ThreeDFormatImpl threeDFormatImpl = Three_D as ThreeDFormatImpl;
    bool flag4 = !threeDFormatImpl.IsBevelBottomHeightSet && !threeDFormatImpl.IsBevelBottomWidthSet;
    bool flag5 = !threeDFormatImpl.IsBevelTopHeightSet && !threeDFormatImpl.IsBevelTopWidthSet;
    if (flag1 && flag2 && flag3 && flag4 && flag5)
      return;
    writer.WriteStartElement("scene3d", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("camera", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("prst", "orthographicFront");
    writer.WriteEndElement();
    writer.WriteStartElement("lightRig", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (Three_D.Lighting != Office2007ChartLightingProperties.ThreePoint)
    {
      int lighting = (int) Three_D.Lighting;
      ChartSerializatorCommon.SerializeLight(writer, lighting);
    }
    else
      writer.WriteAttributeString("rig", ChartSerializatorCommon.LightingProperties[0][0].ToString());
    writer.WriteAttributeString("dir", "t");
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (Three_D.Material != Office2007ChartMaterialProperties.NoEffect)
    {
      writer.WriteStartElement("sp3d", "http://schemas.openxmlformats.org/drawingml/2006/main");
      int material = (int) (Three_D.Material - 1);
      ChartSerializatorCommon.SerializeMaterial(writer, material);
    }
    else
      writer.WriteStartElement("sp3d", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (Three_D.BevelTop != Office2007ChartBevelProperties.NoAngle)
    {
      int bevelTop = (int) Three_D.BevelTop;
      ChartSerializatorCommon.SerializeTopBevel(writer, bevelTop, threeDFormatImpl);
    }
    else if (!flag5)
      ChartSerializatorCommon.SerializeTopBevel(writer, threeDFormatImpl, threeDFormatImpl.PresetShape);
    if (Three_D.BevelBottom != Office2007ChartBevelProperties.NoAngle)
    {
      int bevelBottom = (int) Three_D.BevelBottom;
      ChartSerializatorCommon.SerializeBottomBevel(writer, bevelBottom, threeDFormatImpl);
    }
    else if (!flag4)
      ChartSerializatorCommon.SerializeBottomBevel(writer, threeDFormatImpl);
    writer.WriteEndElement();
  }

  public static void SerializeLight(XmlWriter writer, int light)
  {
    writer.WriteAttributeString("rig", ChartSerializatorCommon.LightingProperties[light][0].ToString());
  }

  public static void SerializeMaterial(XmlWriter writer, int material)
  {
    writer.WriteAttributeString("prstMaterial", ChartSerializatorCommon.MaterialProperties[material][0].ToString());
  }

  internal static void SerializeTopBevel(
    XmlWriter writer,
    int bevel,
    ThreeDFormatImpl threeDFormatImpl)
  {
    writer.WriteStartElement("bevelT", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (threeDFormatImpl.IsBevelTopWidthSet)
      writer.WriteAttributeString("w", (threeDFormatImpl.BevelTopWidth * 12700).ToString());
    else if (!ChartSerializatorCommon.BevelProperties[bevel][0].Equals("null"))
      writer.WriteAttributeString("w", ChartSerializatorCommon.BevelProperties[bevel][0].ToString());
    if (threeDFormatImpl.IsBevelTopHeightSet)
      writer.WriteAttributeString("h", (threeDFormatImpl.BevelTopHeight * 12700).ToString());
    else if (!ChartSerializatorCommon.BevelProperties[bevel][1].Equals("null"))
      writer.WriteAttributeString("h", ChartSerializatorCommon.BevelProperties[bevel][1].ToString());
    if (!ChartSerializatorCommon.BevelProperties[bevel][2].Equals("null"))
      writer.WriteAttributeString("prst", ChartSerializatorCommon.BevelProperties[bevel][2].ToString());
    writer.WriteEndElement();
  }

  public static void SerializeBottomBevel(
    XmlWriter writer,
    int bevel,
    ThreeDFormatImpl threeDFormatImpl)
  {
    writer.WriteStartElement("bevelB", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (threeDFormatImpl.IsBevelBottomWidthSet)
      writer.WriteAttributeString("w", (threeDFormatImpl.BevelBottomWidth * 12700).ToString());
    else if (!ChartSerializatorCommon.BevelProperties[bevel][0].Equals("null"))
      writer.WriteAttributeString("w", ChartSerializatorCommon.BevelProperties[bevel][0].ToString());
    if (threeDFormatImpl.IsBevelBottomHeightSet)
      writer.WriteAttributeString("h", (threeDFormatImpl.BevelBottomHeight * 12700).ToString());
    else if (!ChartSerializatorCommon.BevelProperties[bevel][1].Equals("null"))
      writer.WriteAttributeString("h", ChartSerializatorCommon.BevelProperties[bevel][1].ToString());
    if (!ChartSerializatorCommon.BevelProperties[bevel][2].Equals("null"))
      writer.WriteAttributeString("prst", ChartSerializatorCommon.BevelProperties[bevel][2].ToString());
    writer.WriteEndElement();
  }

  [SecurityCritical]
  internal static void SerializeFill(
    XmlWriter writer,
    IInternalFill fill,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (fill == null)
      return;
    switch (fill.FillType)
    {
      case OfficeFillType.SolidColor:
        ChartSerializatorCommon.SerializeSolidFill(writer, fill.ForeColorObject, false, (IWorkbook) holder.Workbook, 1.0 - fill.Transparency);
        break;
      case OfficeFillType.Pattern:
        ChartSerializatorCommon.SerializePatternFill(writer, fill.ForeColorObject, false, fill.BackColorObject, false, fill.Pattern, (IWorkbook) holder.Workbook);
        break;
      case OfficeFillType.Texture:
        ChartSerializatorCommon.SerializeTextureFill(writer, (IOfficeFill) fill, holder, relations);
        break;
      case OfficeFillType.Picture:
        CommonObject commonObject = fill as CommonObject;
        ChartImpl chart = (ChartImpl) null;
        if (commonObject != null)
          chart = commonObject.FindParent(typeof (ChartImpl)) as ChartImpl;
        ChartSerializatorCommon.SerializePictureFill(writer, fill.Picture, holder, relations, (IInternalFill) (fill as ShapeFillImpl), chart);
        break;
      case OfficeFillType.Gradient:
        ChartSerializatorCommon.SerializeGradientFill(writer, (IOfficeFill) fill, (IWorkbook) holder.Workbook);
        break;
      default:
        writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
        break;
    }
  }

  [SecurityCritical]
  public static void SerializeTextArea(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    WorkbookImpl book,
    RelationCollection relations,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartTextAreaImpl chartTextAreaImpl = textArea != null ? textArea as ChartTextAreaImpl : throw new ArgumentNullException(nameof (textArea));
    FileDataHolder dataHolder = book.DataHolder;
    bool flag = false;
    writer.WriteStartElement("title", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (chartTextAreaImpl.Text != "Chart Title" && chartTextAreaImpl.Text != null)
    {
      ChartSerializatorCommon.SerializeTextAreaText(writer, textArea, (IWorkbook) book, defaultFontSize);
      flag = true;
    }
    ChartSerializatorCommon.SerializeLayout(writer, (object) textArea);
    ChartSerializatorCommon.SerializeValueTag(writer, "overlay", chartTextAreaImpl.Overlay ? "1" : "0");
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) textArea.FrameFormat, dataHolder, relations, false, false);
    if (chartTextAreaImpl.ParagraphType == ChartParagraphType.CustomDefault || !flag && chartTextAreaImpl.ParagraphType == ChartParagraphType.Default && chartTextAreaImpl.ShowBoldProperties)
      ChartSerializatorCommon.SerializeDefaultTextFormatting(writer, (IOfficeFont) textArea, (IWorkbook) book, defaultFontSize, true, 0, Excel2007TextRotation.horz, "http://schemas.openxmlformats.org/drawingml/2006/chart", false, false);
    writer.WriteEndElement();
  }

  public static void SerializeValueTag(XmlWriter writer, string tagName, string value)
  {
    ChartSerializatorCommon.SerializeValueTag(writer, tagName, "http://schemas.openxmlformats.org/drawingml/2006/chart", value);
  }

  public static void SerializeDoubleValueTag(XmlWriter writer, string tagName, double value)
  {
    ChartSerializatorCommon.SerializeDoubleValueTag(writer, tagName, "http://schemas.openxmlformats.org/drawingml/2006/chart", value);
  }

  public static void SerializeValueTag(
    XmlWriter writer,
    string tagName,
    string tagNamespace,
    string value)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null)
      throw new ArgumentNullException(nameof (tagName));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    writer.WriteStartElement(tagName, tagNamespace);
    writer.WriteAttributeString("val", value);
    writer.WriteEndElement();
  }

  public static void SerializeDoubleValueTag(
    XmlWriter writer,
    string tagName,
    string tagNamespace,
    double value)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null)
      throw new ArgumentNullException(nameof (tagName));
    writer.WriteStartElement(tagName, tagNamespace);
    writer.WriteStartAttribute("val");
    writer.WriteValue(value);
    writer.WriteEndAttribute();
    writer.WriteEndElement();
  }

  public static void SerializeBoolValueTag(XmlWriter writer, string tagName, bool value)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null)
      throw new ArgumentNullException(nameof (tagName));
    writer.WriteStartElement(tagName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string str = value ? "1" : "0";
    writer.WriteAttributeString("val", str);
    writer.WriteEndElement();
  }

  public static void SerializeLineProperties(
    XmlWriter writer,
    IOfficeChartBorder border,
    IWorkbook book)
  {
    ChartSerializatorCommon.SerializeLineProperties(writer, border, false, book, false);
  }

  public static void SerializePatternFill(
    XmlWriter writer,
    ChartColor color,
    bool bAutoColor,
    string strDash2007,
    string strPreset,
    IWorkbook book,
    double Alphavalue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (strPreset == null || strPreset.Length == 0)
    {
      ChartSerializatorCommon.SerializeSolidFill(writer, color, bAutoColor, book, Alphavalue);
    }
    else
    {
      ChartColor backColor = new ChartColor(ColorExtension.White);
      ChartSerializatorCommon.SerializePatternFill(writer, color, bAutoColor, backColor, bAutoColor, strPreset, book, Alphavalue);
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "prstDash", "http://schemas.openxmlformats.org/drawingml/2006/main", strDash2007);
  }

  public static void SerializePatternFill(
    XmlWriter writer,
    ChartColor foreColor,
    bool isAutoFore,
    ChartColor backColor,
    bool isAutoBack,
    string strPreset,
    IWorkbook book,
    double Alphavalue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (strPreset == null || strPreset.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (strPreset));
    writer.WriteStartElement("pattFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("prst", strPreset);
    if (!isAutoFore)
    {
      writer.WriteStartElement("fgClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      ChartSerializatorCommon.SerializeRgbColor(writer, foreColor.GetRGB(book), Alphavalue);
      writer.WriteEndElement();
    }
    if (!isAutoBack)
    {
      writer.WriteStartElement("bgClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      ChartSerializatorCommon.SerializeRgbColor(writer, backColor.GetRGB(book), Alphavalue);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  public static void SerializePatternFill(
    XmlWriter writer,
    ChartColor foreColor,
    bool isAutoFore,
    ChartColor backColor,
    bool isAutoBack,
    OfficeGradientPattern pattern,
    IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("pattFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Excel2007GradientPattern excel2007GradientPattern = (Excel2007GradientPattern) pattern;
    writer.WriteAttributeString("prst", excel2007GradientPattern.ToString());
    if (!isAutoFore)
    {
      writer.WriteStartElement("fgClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      ChartSerializatorCommon.SerializeRgbColor(writer, foreColor.GetRGB(book));
      writer.WriteEndElement();
    }
    if (!isAutoBack)
    {
      writer.WriteStartElement("bgClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      ChartSerializatorCommon.SerializeRgbColor(writer, backColor.GetRGB(book));
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  public static void SerializeSolidFill(
    XmlWriter writer,
    ChartColor color,
    bool isAutoColor,
    IWorkbook book,
    double alphavalue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!isAutoColor)
      ChartSerializatorCommon.SerializeRgbColor(writer, color.GetRGB(book), alphavalue);
    writer.WriteEndElement();
  }

  public static void SerializeRgbColor(XmlWriter writer, Color color)
  {
    ChartSerializatorCommon.SerializeRgbColor(writer, color, -1.0);
  }

  public static void SerializeRgbColor(
    XmlWriter writer,
    OfficeKnownColors colorIndex,
    IWorkbook book)
  {
    ChartSerializatorCommon.SerializeRgbColor(writer, book.GetPaletteColor(colorIndex), -1.0);
  }

  public static void SerializeRgbColor(XmlWriter writer, Color color, double alphaValue)
  {
    int int32 = Convert.ToInt32(alphaValue * 100000.0);
    ChartSerializatorCommon.SerializeRgbColor(writer, color, int32, -1, -1);
  }

  public static void SerializeRgbColor(
    XmlWriter writer,
    Color color,
    int alpha,
    int tint,
    int shade)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num = 100000;
    writer.WriteStartElement("srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", (color.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6"));
    if (alpha != 100000 && alpha >= 0 && alpha <= num)
    {
      writer.WriteStartElement(nameof (alpha), "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", alpha.ToString());
      writer.WriteEndElement();
    }
    if (shade >= 0)
    {
      writer.WriteElementString("gamma", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
      writer.WriteStartElement(nameof (shade), "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", shade.ToString());
      writer.WriteEndElement();
      writer.WriteElementString("invGamma", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    }
    else if (tint >= 0)
    {
      writer.WriteElementString("gamma", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
      writer.WriteStartElement(nameof (tint), "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", tint.ToString());
      writer.WriteEndElement();
      writer.WriteElementString("invGamma", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    }
    writer.WriteEndElement();
  }

  private static void SerializeLineProperties(
    XmlWriter writer,
    IOfficeChartBorder border,
    bool bRoundCorners,
    IWorkbook book,
    bool serializeAutoFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (border == null)
      return;
    ChartBorderImpl chartBorderImpl = border as ChartBorderImpl;
    writer.WriteStartElement("ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!chartBorderImpl.AutoFormat || serializeAutoFormat)
    {
      if (chartBorderImpl.LineWeightString != null)
        writer.WriteAttributeString("w", chartBorderImpl.LineWeightString);
      else if ((short) border.LineWeight != (short) -1)
      {
        int num = ((int) (short) border.LineWeight + 1) * 12700;
        writer.WriteAttributeString("w", num.ToString());
      }
      else if (!chartBorderImpl.HasLineProperties)
        writer.WriteAttributeString("w", "3175");
      OfficeChartLinePattern linePattern = border.LinePattern;
      if (((ChartBorderImpl) border).HasLineProperties || ((WorkbookImpl) book).IsCreated || ((WorkbookImpl) book).IsConverted)
      {
        if (chartBorderImpl.HasGradientFill)
        {
          ChartSerializatorCommon.SerializeGradientFill(writer, (IOfficeFill) chartBorderImpl.Fill, book);
        }
        else
        {
          switch (linePattern)
          {
            case OfficeChartLinePattern.Solid:
              ChartSerializatorCommon.SerializeSolidFill(writer, (ChartColor) border.LineColor, border.IsAutoLineColor, book, 1.0 - border.Transparency);
              writer.WriteStartElement("prstDash", "http://schemas.openxmlformats.org/drawingml/2006/main");
              writer.WriteAttributeString("val", "solid");
              writer.WriteEndElement();
              break;
            case OfficeChartLinePattern.None:
              writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
              break;
            default:
              KeyValuePair<string, string> dicLinePattern = ChartSerializatorCommon.s_dicLinePatterns[linePattern];
              string key = dicLinePattern.Key;
              string strPreset = dicLinePattern.Value;
              ChartSerializatorCommon.SerializePatternFill(writer, (ChartColor) border.LineColor, border.IsAutoLineColor, key, strPreset, book, 1.0 - border.Transparency);
              break;
          }
        }
      }
      ChartSerializatorCommon.SerializeJoinType(writer, chartBorderImpl.JoinType);
    }
    writer.WriteEndElement();
  }

  private static void SerializeJoinType(XmlWriter writer, Excel2007BorderJoinType joinType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string localName = (string) null;
    switch (joinType)
    {
      case Excel2007BorderJoinType.Round:
        localName = "round";
        break;
      case Excel2007BorderJoinType.Bevel:
        localName = "bevel";
        break;
      case Excel2007BorderJoinType.Mitter:
        localName = "miter";
        break;
    }
    if (localName == null)
      return;
    writer.WriteElementString(localName, "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
  }

  [SecurityCritical]
  internal static Stream GetImageStream(Image image)
  {
    ImageFormat rawFormat = image.RawFormat;
    MemoryStream imageStream;
    if (rawFormat.Equals((object) ImageFormat.Emf) || rawFormat.Equals((object) ImageFormat.Wmf))
    {
      MemoryStream memoryStream = new MemoryStream();
      imageStream = MsoMetafilePicture.SerializeMetafile(image);
    }
    else
    {
      imageStream = new MemoryStream();
      image.Save((Stream) imageStream, rawFormat);
    }
    return (Stream) imageStream;
  }

  [SecurityCritical]
  private static void SerializePictureFill(
    XmlWriter writer,
    Image image,
    FileDataHolder holder,
    RelationCollection relations,
    bool tile,
    ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    string str = relations != null ? relations.GenerateRelationId() : throw new ArgumentNullException(nameof (relations));
    if (chart != null)
    {
      if (!chart.RelationPreservedStreamCollection.ContainsKey(str))
        chart.RelationPreservedStreamCollection.Add(str, ChartSerializatorCommon.GetImageStream(image));
      relations[str] = new Relation("", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    }
    writer.WriteStartElement("blipFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("blip", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str);
    writer.WriteEndElement();
    if (tile)
    {
      writer.WriteStartElement(nameof (tile), "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("tx", "0");
      writer.WriteAttributeString("ty", "0");
      writer.WriteAttributeString("sx", "100000");
      writer.WriteAttributeString("sy", "100000");
      writer.WriteAttributeString("flip", "none");
      writer.WriteAttributeString("algn", "tl");
      writer.WriteEndElement();
    }
    else
    {
      writer.WriteStartElement("stretch", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteElementString("fillRect", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private static void SerializePictureFill(
    XmlWriter writer,
    Image image,
    FileDataHolder holder,
    RelationCollection relations,
    IInternalFill fill,
    ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    string str = relations != null ? relations.GenerateRelationId() : throw new ArgumentNullException(nameof (relations));
    if (chart != null)
    {
      if (!chart.RelationPreservedStreamCollection.ContainsKey(str))
        chart.RelationPreservedStreamCollection.Add(str, ChartSerializatorCommon.GetImageStream(image));
      relations[str] = new Relation("", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    }
    writer.WriteStartElement("blipFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("blip", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str);
    if ((double) fill.TransparencyColor != 0.0)
    {
      writer.WriteStartElement("alphaModFix", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("amt", ((int) Math.Round((1.0 - (double) fill.TransparencyColor) * 100000.0)).ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    if (fill.Tile)
    {
      writer.WriteStartElement("tile", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("tx", (Math.Round((double) fill.TextureOffsetX) * 12700.0).ToString());
      writer.WriteAttributeString("ty", (Math.Round((double) fill.TextureOffsetY) * 12700.0).ToString());
      writer.WriteAttributeString("sx", (Math.Round((double) fill.TextureHorizontalScale) * 100000.0).ToString());
      writer.WriteAttributeString("sy", (Math.Round((double) fill.TextureVerticalScale) * 100000.0).ToString());
      writer.WriteAttributeString("flip", fill.TileFlipping);
      writer.WriteAttributeString("algn", fill.Alignment);
      writer.WriteEndElement();
    }
    else
    {
      writer.WriteStartElement("srcRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Rectangle sourceRect = (fill as ShapeFillImpl).SourceRect;
      if (sourceRect.Right != 0)
        writer.WriteAttributeString("r", sourceRect.Right.ToString());
      if (sourceRect.Bottom != 0)
        writer.WriteAttributeString("b", sourceRect.Bottom.ToString());
      if (sourceRect.Left != 0)
        writer.WriteAttributeString("l", sourceRect.Left.ToString());
      if (sourceRect.Top != 0)
        writer.WriteAttributeString("t", sourceRect.Top.ToString());
      writer.WriteEndElement();
      writer.WriteStartElement("stretch", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteStartElement("fillRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Rectangle fillRect = (fill as ShapeFillImpl).FillRect;
      if (fillRect.Right != 0)
        writer.WriteAttributeString("r", fillRect.Right.ToString());
      if (fillRect.Bottom != 0)
        writer.WriteAttributeString("b", fillRect.Bottom.ToString());
      if (fillRect.Left != 0)
        writer.WriteAttributeString("l", fillRect.Left.ToString());
      if (fillRect.Top != 0)
        writer.WriteAttributeString("t", fillRect.Top.ToString());
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private static void SerializeTextureFill(
    XmlWriter writer,
    IOfficeFill fill,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (fill == null)
      throw new ArgumentNullException(nameof (fill));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    OfficeTexture texture = fill.Texture;
    Image image;
    if (texture != OfficeTexture.User_Defined)
    {
      byte[] resData = ShapeFillImpl.GetResData("Text" + ((int) texture).ToString());
      byte[] numArray = new byte[resData.Length - 25];
      Array.Copy((Array) resData, 25, (Array) numArray, 0, numArray.Length);
      MemoryStream ms = new MemoryStream();
      ShapeFillImpl.UpdateBitMapHederToStream(ms, resData);
      ms.Write(numArray, 0, numArray.Length);
      image = ApplicationImpl.CreateImage((Stream) ms);
    }
    else
      image = fill.Picture;
    CommonObject commonObject = fill as CommonObject;
    ChartImpl chart = (ChartImpl) null;
    if (commonObject != null)
      chart = commonObject.FindParent(typeof (ChartImpl)) as ChartImpl;
    ChartSerializatorCommon.SerializePictureFill(writer, image, holder, relations, (fill as IInternalFill).Tile, chart);
  }

  private static void SerializeGradientFill(XmlWriter writer, IOfficeFill fill, IWorkbook book)
  {
    ShapeFillImpl shapeFillImpl = (ShapeFillImpl) fill;
    GradientStops gradientStops = shapeFillImpl.GradientStops;
    GradientStops preservedGradient = shapeFillImpl.PreservedGradient;
    GradientSerializator gradientSerializator = new GradientSerializator();
    if (preservedGradient != null)
    {
      gradientStops = preservedGradient;
      Rectangle tileRect = gradientStops.TileRect;
      gradientStops.TileRect = shapeFillImpl.PreservedGradient.TileRect;
    }
    gradientSerializator.Serialize(writer, gradientStops, book);
  }

  private static bool HasSchemaColor(GradientStops stops)
  {
    for (int index = 0; index < stops.Count; ++index)
    {
      if (stops[index].ColorObject.IsSchemeColor)
        return true;
    }
    return false;
  }

  private void SerializeTextProperties(XmlWriter writer, IOfficeChartTextArea textArea)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  internal static void SerializeDefaultTextFormatting(
    XmlWriter writer,
    IOfficeFont textFormatting,
    IWorkbook book,
    double defaultFontSize,
    bool isAutoTextRotation,
    int rotationAngle,
    Excel2007TextRotation textRotation,
    string nameSpace,
    bool isChartExText,
    bool isEndParagraph)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (textFormatting == null)
      return;
    writer.WriteStartElement("txPr", nameSpace);
    writer.WriteStartElement("bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!isAutoTextRotation && nameSpace == "http://schemas.microsoft.com/office/drawing/2014/chartex")
    {
      int num = rotationAngle * 60000;
      writer.WriteAttributeString("rot", num.ToString());
      writer.WriteAttributeString("vert", textRotation.ToString());
    }
    writer.WriteEndElement();
    writer.WriteStartElement("lstStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    ChartSerializatorCommon.SerializeParagraphRunProperites(writer, textFormatting, "defRPr", book, defaultFontSize);
    writer.WriteEndElement();
    if (isChartExText)
    {
      ChartTextAreaImpl textArea = textFormatting as ChartTextAreaImpl;
      writer.WriteStartElement("r", "http://schemas.openxmlformats.org/drawingml/2006/main");
      ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IOfficeFont) textArea, "rPr", book, defaultFontSize);
      writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteString(textArea.Text);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    if (isEndParagraph)
      ChartSerializatorCommon.SerializeParagraphRunProperites(writer, textFormatting, "endParaRPr", book, defaultFontSize);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerializeTextAreaText(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    IWorkbook book,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    writer.WriteStartElement("tx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    bool flag = false;
    string[] strCache = (string[]) null;
    if (textArea is ChartTextAreaImpl)
    {
      ChartTextAreaImpl chartTextAreaImpl = textArea as ChartTextAreaImpl;
      strCache = chartTextAreaImpl.StringCache;
      flag = chartTextAreaImpl.IsFormula;
    }
    else if (textArea is ChartDataLabelsImpl)
    {
      ChartDataLabelsImpl chartDataLabelsImpl = textArea as ChartDataLabelsImpl;
      strCache = chartDataLabelsImpl.StringCache;
      flag = chartDataLabelsImpl.IsFormula;
    }
    if (flag)
      ChartSerializatorCommon.SerializeStringReference(writer, textArea, strCache);
    else
      ChartSerializatorCommon.SerializeRichText(writer, textArea, book, "rich", defaultFontSize);
    writer.WriteEndElement();
  }

  public static void SerializeRichText(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    IWorkbook book,
    string tagName,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    bool flag = false;
    if (textArea is ChartTextAreaImpl)
      flag = ChartImpl.IsChartExSerieType(((ChartImpl) (textArea as ChartTextAreaImpl).FindParent(typeof (ChartImpl))).ChartType);
    if (flag)
      writer.WriteStartElement(tagName, "http://schemas.microsoft.com/office/drawing/2014/chartex");
    else
      writer.WriteStartElement(tagName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeBodyProperties(writer, textArea);
    ChartSerializatorCommon.SerializeListStyles(writer, textArea);
    ChartSerializatorCommon.SerializeParagraphs(writer, textArea, book, defaultFontSize);
    writer.WriteEndElement();
  }

  private static void SerializeBodyProperties(XmlWriter writer, IOfficeChartTextArea textArea)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    writer.WriteStartElement("bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    IInternalOfficeChartTextArea officeChartTextArea = textArea as IInternalOfficeChartTextArea;
    if (officeChartTextArea.HasTextRotation)
    {
      int num = officeChartTextArea.TextRotationAngle * 60000;
      writer.WriteAttributeString("rot", num.ToString());
      if (officeChartTextArea is ChartTextAreaImpl)
        writer.WriteAttributeString("vert", (officeChartTextArea as ChartTextAreaImpl).TextRotation.ToString());
      else if (textArea is ChartDataLabelsImpl)
        writer.WriteAttributeString("vert", (textArea as ChartDataLabelsImpl).TextRotation.ToString());
    }
    writer.WriteEndElement();
  }

  private static void SerializeListStyles(XmlWriter writer, IOfficeChartTextArea textArea)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    writer.WriteStartElement("lstStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializeParagraphs(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    IWorkbook book,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    switch (textArea)
    {
      case null:
        throw new ArgumentNullException(nameof (textArea));
      case ChartTextAreaImpl _ when (textArea as ChartTextAreaImpl).ChartAlRuns != null && (textArea as ChartTextAreaImpl).ChartAlRuns.Runs != null && (textArea as ChartTextAreaImpl).ChartAlRuns.Runs.Length > 0:
        ChartSerializatorCommon.Serialize_TextArea_RichTextParagraph(writer, textArea, book, defaultFontSize);
        break;
      case ChartDataLabelsImpl _ when (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns != null && (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns.Runs != null && (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns.Runs.Length > 0:
        ChartSerializatorCommon.Serialize_DataLabel_RichTextParagraph(writer, textArea, book, defaultFontSize);
        break;
      default:
        string[] strArray = textArea.Text.Split('\n');
        int index = 0;
        for (int length = strArray.Length; index < length; ++index)
          ChartSerializatorCommon.SerializeSingleParagraph(writer, textArea, strArray[index], book, defaultFontSize);
        break;
    }
  }

  private static void Serialize_TextArea_RichTextParagraph(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    IWorkbook book,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    int startRunProperty = 0;
    if ((textArea as ChartTextAreaImpl).DefaultParagarphProperties.Count > 1)
    {
      foreach (IInternalOfficeChartTextArea paragarphProperty in (IEnumerable<IInternalOfficeChartTextArea>) (textArea as ChartTextAreaImpl).DefaultParagarphProperties)
      {
        ChartSerializatorCommon.Serialize_TextArea_RichTextSeparateParagraph(writer, textArea, book, defaultFontSize, paragarphProperty, startRunProperty);
        startRunProperty = (paragarphProperty as ChartTextAreaImpl).ChartAlRuns.Runs.Length;
      }
    }
    else
      ChartSerializatorCommon.Serialize_TextArea_RichTextSeparateParagraph(writer, textArea, book, defaultFontSize, (IInternalOfficeChartTextArea) null, startRunProperty);
  }

  private static void Serialize_TextArea_RichTextSeparateParagraph(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    IWorkbook book,
    double defaultFontSize,
    IInternalOfficeChartTextArea defaultParagaphProperties,
    int startRunProperty)
  {
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    int length1;
    if (defaultParagaphProperties != null)
    {
      ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IOfficeFont) defaultParagaphProperties, "defRPr", book, defaultFontSize);
      length1 = (defaultParagaphProperties as ChartTextAreaImpl).ChartAlRuns.Runs.Length;
    }
    else
    {
      ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IOfficeFont) textArea, "defRPr", book, defaultFontSize);
      length1 = (textArea as ChartTextAreaImpl).ChartAlRuns.Runs.Length;
    }
    writer.WriteEndElement();
    for (int index1 = startRunProperty; index1 < length1; ++index1)
    {
      writer.WriteStartElement("r", "http://schemas.openxmlformats.org/drawingml/2006/main");
      int firstCharIndex = (int) (textArea as ChartTextAreaImpl).ChartAlRuns.Runs[index1].FirstCharIndex;
      int length2 = index1 >= (textArea as ChartTextAreaImpl).ChartAlRuns.Runs.Length - 1 ? textArea.Text.Length - (int) (textArea as ChartTextAreaImpl).ChartAlRuns.Runs[index1].FirstCharIndex : (int) (textArea as ChartTextAreaImpl).ChartAlRuns.Runs[index1 + 1].FirstCharIndex - firstCharIndex;
      if ((textArea as ChartTextAreaImpl).ChartAlRuns.Runs[index1].HasNewParagarphStart)
        --length2;
      string text = textArea.Text.Substring(firstCharIndex, length2);
      if (index1 == 0 && (textArea as ChartTextAreaImpl).ChartAlRuns.Runs.Length < 2)
      {
        int index2 = (textArea as IInternalFont).Index;
        (textArea as ChartTextAreaImpl).SetFontIndex(index2);
      }
      else
        (textArea as ChartTextAreaImpl).SetFontIndex((int) (textArea as ChartTextAreaImpl).ChartAlRuns.Runs[index1].FontIndex);
      ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IOfficeFont) textArea, "rPr", book, defaultFontSize);
      writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteString(text);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void Serialize_DataLabel_RichTextParagraph(
    XmlWriter writer,
    IOfficeChartTextArea chartTextArea,
    IWorkbook book,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chartTextArea == null)
      throw new ArgumentNullException("textArea");
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    ChartDataLabelsImpl chartDataLabelsImpl = chartTextArea as ChartDataLabelsImpl;
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("defRPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
    writer.WriteEndElement();
    ChartTextAreaImpl textArea = (chartTextArea as ChartDataLabelsImpl).TextArea;
    int length1 = textArea.ChartAlRuns.Runs.Length;
    bool flag = false;
    string fldElementType = (string) null;
    for (int index = 0; index < length1; ++index)
    {
      int firstCharIndex = (int) textArea.ChartAlRuns.Runs[index].FirstCharIndex;
      int length2 = index >= length1 - 1 ? textArea.Text.Length - (int) textArea.ChartAlRuns.Runs[index].FirstCharIndex : (int) textArea.ChartAlRuns.Runs[index + 1].FirstCharIndex - firstCharIndex;
      string text = textArea.Text.Substring(firstCharIndex, length2);
      if (!string.IsNullOrEmpty(text))
        flag = ChartSerializatorCommon.CheckSerializeFldElement(chartDataLabelsImpl, text, out fldElementType);
      if (flag)
      {
        writer.WriteStartElement("fld", "http://schemas.openxmlformats.org/drawingml/2006/main");
        writer.WriteAttributeString("id", "{C1C5B820-BA97-46F5-89C0-B4B82AE36AEC}");
        writer.WriteAttributeString("type", fldElementType);
      }
      else
        writer.WriteStartElement("r", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (length1 > 1)
        textArea.SetFontIndex((int) textArea.ChartAlRuns.Runs[index].FontIndex);
      ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IOfficeFont) textArea, "rPr", book, defaultFontSize);
      writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteString(text);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static bool CheckSerializeFldElement(
    ChartDataLabelsImpl chartDataLabelsImpl,
    string text,
    out string fldElementType)
  {
    fldElementType = (string) null;
    if (chartDataLabelsImpl.IsValueFromCells && text.Equals("[CELLRANGE]"))
    {
      fldElementType = "CELLRANGE";
      return true;
    }
    if (chartDataLabelsImpl.IsSeriesName && text.Equals("[SERIES NAME]"))
    {
      fldElementType = "SERIESNAME";
      return true;
    }
    if (chartDataLabelsImpl.IsCategoryName && text.Equals("[CATEGORY NAME]"))
    {
      fldElementType = "CATEGORYNAME";
      return true;
    }
    if (chartDataLabelsImpl.IsCategoryName && text.Equals("[X VALUE]"))
    {
      fldElementType = "XVALUE";
      return true;
    }
    if (chartDataLabelsImpl.IsValue && text.Equals("[VALUE]"))
    {
      fldElementType = "VALUE";
      return true;
    }
    if (chartDataLabelsImpl.IsValue && text.Equals("[Y VALUE]"))
    {
      fldElementType = "YVALUE";
      return true;
    }
    if (!chartDataLabelsImpl.IsPercentage || !text.Equals("[PERCENTAGE]"))
      return false;
    fldElementType = "PERCENTAGE";
    return true;
  }

  private static void SerializeSingleParagraph(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    string paragraphText,
    IWorkbook book,
    double defaultFontSize)
  {
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("defRPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (textArea.Size != defaultFontSize || textArea is ChartTextAreaImpl && (textArea as ChartTextAreaImpl).ShowSizeProperties)
    {
      int num = (int) (textArea.Size * 100.0);
      writer.WriteAttributeString("sz", num.ToString());
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("r", "http://schemas.openxmlformats.org/drawingml/2006/main");
    ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IOfficeFont) textArea, "rPr", book, defaultFontSize);
    writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteString(paragraphText);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerializeParagraphRunProperites(
    XmlWriter writer,
    IOfficeFont textArea,
    string mainTagName,
    IWorkbook book,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (mainTagName == null || mainTagName.Length == 0)
      throw new ArgumentException(nameof (mainTagName));
    writer.WriteStartElement(mainTagName, "http://schemas.openxmlformats.org/drawingml/2006/main");
    string str1 = textArea.Bold ? "1" : "0";
    string str2 = textArea.Italic ? "1" : "0";
    if (textArea is IInternalFont internalFont)
    {
      string language = internalFont.Font.Language;
      if (language != null)
        writer.WriteAttributeString("lang", language);
    }
    if (textArea.Bold || internalFont.Font.m_textSettings != null && internalFont.Font.m_textSettings.Bold.HasValue || internalFont is ChartDataLabelsImpl && (internalFont as ChartDataLabelsImpl).ShowBoldProperties || internalFont is ChartTextAreaImpl && (internalFont as ChartTextAreaImpl).ShowBoldProperties)
      writer.WriteAttributeString("b", str1);
    if (textArea.Italic || internalFont.Font.m_textSettings != null && internalFont.Font.m_textSettings.Italic.HasValue)
      writer.WriteAttributeString("i", str2);
    if (textArea.Strikethrough)
      writer.WriteAttributeString("strike", "sngStrike");
    if (textArea.Size == defaultFontSize)
    {
      if (internalFont.Font.m_textSettings != null)
      {
        bool? showSizeProperties = internalFont.Font.m_textSettings.ShowSizeProperties;
        if ((!showSizeProperties.GetValueOrDefault() ? 0 : (showSizeProperties.HasValue ? 1 : 0)) != 0)
          goto label_19;
      }
      if ((!(internalFont is ChartDataLabelsImpl) || !(internalFont as ChartDataLabelsImpl).ShowSizeProperties) && (!(internalFont is ChartTextAreaImpl) || !(internalFont as ChartTextAreaImpl).ShowSizeProperties))
        goto label_20;
    }
label_19:
    int num1 = (int) (textArea.Size * 100.0);
    writer.WriteAttributeString("sz", num1.ToString());
label_20:
    if (textArea.Underline != OfficeUnderline.None)
    {
      string str3 = Helper.ToString(textArea.Underline);
      writer.WriteAttributeString("u", str3);
    }
    int num2 = 0;
    string str4;
    if (textArea.Superscript || textArea.Subscript)
    {
      switch (textArea)
      {
        case ChartTextAreaImpl _ when (textArea as ChartTextAreaImpl).Font != null:
          str4 = !(textArea as ChartTextAreaImpl).IsBaselineWithPercentage ? (textArea as ChartTextAreaImpl).Font.BaseLine.ToString() : ((textArea as ChartTextAreaImpl).Font.BaseLine * 100).ToString() + "%";
          break;
        case FontWrapper _:
          str4 = (textArea as FontWrapper).Baseline.ToString();
          break;
        default:
          str4 = num2.ToString();
          break;
      }
    }
    else
      str4 = num2.ToString();
    writer.WriteAttributeString("baseline", str4);
    if (!textArea.IsAutoColor || textArea is FontWrapper && (book as WorkbookImpl).IsConverted)
    {
      writer.WriteStartElement("solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      ChartSerializatorCommon.SerializeRgbColor(writer, textArea.RGBColor, -100000, -1, -1);
      writer.WriteEndElement();
    }
    if (textArea is FontWrapper)
    {
      FontImpl font = (textArea as FontWrapper).Font;
      if (font.PreservedElements != null)
      {
        if (font.PreservedElements.ContainsKey("ln"))
          ChartSerializatorCommon.WriteRawData(writer, font.PreservedElements, "ln");
        if (font.PreservedElements.ContainsKey("gradFill"))
          ChartSerializatorCommon.WriteRawData(writer, font.PreservedElements, "gradFill");
        if (font.PreservedElements.ContainsKey("effectLst"))
          ChartSerializatorCommon.WriteRawData(writer, font.PreservedElements, "effectLst");
      }
    }
    if (textArea.FontName != "Calibri")
    {
      writer.WriteStartElement("latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("typeface", textArea.FontName);
      writer.WriteEndElement();
      writer.WriteStartElement("ea", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("typeface", textArea.FontName);
      writer.WriteEndElement();
      writer.WriteStartElement("cs", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("typeface", textArea.FontName);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  internal static void WriteRawData(
    XmlWriter xmlWriter,
    Dictionary<string, Stream> preservedStream,
    string elementName)
  {
    if (!preservedStream.ContainsKey(elementName))
      return;
    using (Stream input = preservedStream[elementName])
    {
      if (input == null || input.Length <= 0L)
        return;
      input.Position = 0L;
      using (XmlReader reader = XmlReader.Create(input))
      {
        while (reader.LocalName != elementName)
          reader.Read();
        xmlWriter.WriteNode(reader, false);
      }
    }
  }

  private static void SerializeStringReference(
    XmlWriter writer,
    IOfficeChartTextArea textArea,
    string[] strCache)
  {
    string str = textArea.Text;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (str[0] == '=')
      str = UtilityMethods.RemoveFirstCharUnsafe(str);
    writer.WriteStartElement("strRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", str);
    if (strCache != null && strCache.Length > 0)
    {
      writer.WriteStartElement(nameof (strCache), "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", strCache.Length.ToString());
      for (int index = 0; index < strCache.Length; ++index)
      {
        writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteAttributeString("idx", index.ToString());
        writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", strCache[index]);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    else
      writer.WriteElementString(nameof (strCache), "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    writer.WriteEndElement();
  }

  public static void SerializeLayout(XmlWriter writer, object textArea)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    IOfficeChartLayout officeChartLayout = (IOfficeChartLayout) null;
    if (textArea is ChartTextAreaImpl)
      officeChartLayout = (textArea as ChartTextAreaImpl).Layout;
    else if (textArea is ChartPlotAreaImpl)
      officeChartLayout = (textArea as ChartPlotAreaImpl).Layout;
    else if (textArea is ChartDataLabelsImpl)
      officeChartLayout = (textArea as ChartDataLabelsImpl).Layout;
    else if (textArea is ChartLegendImpl)
      officeChartLayout = (textArea as ChartLegendImpl).Layout;
    if (officeChartLayout == null)
      return;
    writer.WriteStartElement("layout", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (officeChartLayout.ManualLayout != null && !ChartSerializatorCommon.IsAutoManualLayout(officeChartLayout.ManualLayout))
      ChartSerializatorCommon.SerializeManualLayout(writer, officeChartLayout.ManualLayout);
    writer.WriteEndElement();
  }

  private static bool IsAutoManualLayout(IOfficeChartManualLayout manualLayout)
  {
    if (manualLayout == null)
      throw new ArgumentNullException(nameof (manualLayout));
    if (manualLayout != null)
    {
      int layoutTarget = (int) manualLayout.LayoutTarget;
      if (manualLayout.LayoutTarget == LayoutTargets.auto)
      {
        int leftMode = (int) manualLayout.LeftMode;
        if (manualLayout.LeftMode == LayoutModes.auto)
        {
          int topMode = (int) manualLayout.TopMode;
          if (manualLayout.TopMode == LayoutModes.auto)
          {
            int widthMode = (int) manualLayout.WidthMode;
            if (manualLayout.WidthMode == LayoutModes.auto)
            {
              int heightMode = (int) manualLayout.HeightMode;
              if (manualLayout.HeightMode == LayoutModes.auto)
              {
                double left = manualLayout.Left;
                double top = manualLayout.Top;
                if (manualLayout.Left == 0.0 && manualLayout.Top == 0.0)
                {
                  double width = manualLayout.Width;
                  double height = manualLayout.Height;
                  if (manualLayout.Width == 0.0 && manualLayout.Height == 0.0)
                    goto label_11;
                }
              }
            }
          }
        }
      }
      return false;
    }
label_11:
    return true;
  }

  public static void SerializeManualLayout(XmlWriter writer, IOfficeChartManualLayout manualLayout)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (manualLayout == null)
      throw new ArgumentNullException(nameof (manualLayout));
    if (manualLayout == null)
      return;
    writer.WriteStartElement(nameof (manualLayout), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int layoutTarget = (int) manualLayout.LayoutTarget;
    if (manualLayout.LayoutTarget != LayoutTargets.auto)
      ChartSerializatorCommon.SerializeValueTag(writer, "layoutTarget", manualLayout.LayoutTarget.ToString());
    int leftMode = (int) manualLayout.LeftMode;
    if (manualLayout.LeftMode != LayoutModes.auto)
      ChartSerializatorCommon.SerializeValueTag(writer, "xMode", manualLayout.LeftMode.ToString());
    int topMode = (int) manualLayout.TopMode;
    if (manualLayout.TopMode != LayoutModes.auto)
      ChartSerializatorCommon.SerializeValueTag(writer, "yMode", manualLayout.TopMode.ToString());
    int widthMode = (int) manualLayout.WidthMode;
    if (manualLayout.WidthMode != LayoutModes.auto)
      ChartSerializatorCommon.SerializeValueTag(writer, "wMode", manualLayout.WidthMode.ToString());
    int heightMode = (int) manualLayout.HeightMode;
    if (manualLayout.HeightMode != LayoutModes.auto)
      ChartSerializatorCommon.SerializeValueTag(writer, "hMode", manualLayout.HeightMode.ToString());
    double left = manualLayout.Left;
    double top = manualLayout.Top;
    if (manualLayout.Left != 0.0 || manualLayout.Top != 0.0)
    {
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "x", manualLayout.Left);
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "y", manualLayout.Top);
    }
    double width = manualLayout.Width;
    double height = manualLayout.Height;
    if (manualLayout.Width != 0.0 || manualLayout.Height != 0.0)
    {
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "w", manualLayout.Width);
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "h", manualLayout.Height);
    }
    writer.WriteEndElement();
  }

  internal static void SerializeBottomBevel(XmlWriter writer, ThreeDFormatImpl threeDFormatImpl)
  {
    writer.WriteStartElement("bevelB", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (threeDFormatImpl.IsBevelBottomHeightSet)
      writer.WriteAttributeString("h", (threeDFormatImpl.BevelBottomHeight * 12700).ToString());
    if (threeDFormatImpl.IsBevelBottomWidthSet)
      writer.WriteAttributeString("w", (threeDFormatImpl.BevelBottomWidth * 12700).ToString());
    writer.WriteEndElement();
  }

  internal static void SerializeTopBevel(
    XmlWriter writer,
    ThreeDFormatImpl threeDFormatImpl,
    string presetShape)
  {
    writer.WriteStartElement("bevelT", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (threeDFormatImpl.IsBevelTopHeightSet)
      writer.WriteAttributeString("h", (threeDFormatImpl.BevelTopHeight * 12700).ToString());
    if (threeDFormatImpl.IsBevelTopWidthSet)
      writer.WriteAttributeString("w", (threeDFormatImpl.BevelTopWidth * 12700).ToString());
    if (presetShape != null)
      writer.WriteAttributeString("prst", presetShape);
    writer.WriteEndElement();
  }
}
