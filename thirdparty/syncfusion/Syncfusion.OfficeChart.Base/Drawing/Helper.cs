// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.Helper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.Drawing;

internal static class Helper
{
  internal static Dictionary<string, int> columnAttributes;

  internal static string GetPlacementType(PlacementType placementType)
  {
    switch (placementType)
    {
      case PlacementType.FreeFloating:
        return "absolute";
      case PlacementType.Move:
        return "oneCell";
      case PlacementType.MoveAndSize:
        return "twoCell";
      default:
        throw new ArgumentException("Invalid PlacementType val");
    }
  }

  internal static PlacementType GetPlacementType(string placementString)
  {
    switch (placementString)
    {
      case "absolute":
        return PlacementType.FreeFloating;
      case "oneCell":
        return PlacementType.Move;
      case "twoCell":
        return PlacementType.MoveAndSize;
      default:
        return PlacementType.MoveAndSize;
    }
  }

  internal static OfficeUnderline GetOfficeUnderlineType(string value)
  {
    switch (value)
    {
      case "none":
        return OfficeUnderline.None;
      case "dbl":
        return OfficeUnderline.Double;
      case "sng":
        return OfficeUnderline.Single;
      case "heavy":
        return OfficeUnderline.Heavy;
      case "dotted":
        return OfficeUnderline.Dotted;
      case "dottedHeavy":
        return OfficeUnderline.DottedHeavy;
      case "dash":
        return OfficeUnderline.Dash;
      case "dashHeavy":
        return OfficeUnderline.DashHeavy;
      case "dashLong":
        return OfficeUnderline.DashLong;
      case "dashLongHeavy":
        return OfficeUnderline.DashLongHeavy;
      case "dotDash":
        return OfficeUnderline.DotDash;
      case "dotDashHeavy":
        return OfficeUnderline.DotDashHeavy;
      case "dotDotDash":
        return OfficeUnderline.DotDotDash;
      case "dotDotDashHeavy":
        return OfficeUnderline.DotDotDashHeavy;
      case "wavy":
        return OfficeUnderline.Wavy;
      case "wavyHeavy":
        return OfficeUnderline.WavyHeavy;
      case "wavyDbl":
        return OfficeUnderline.WavyDouble;
      case "words":
        return OfficeUnderline.Words;
      default:
        return OfficeUnderline.Single;
    }
  }

  internal static string ToString(OfficeUnderline officeUnderlineType)
  {
    switch (officeUnderlineType)
    {
      case OfficeUnderline.None:
        return "none";
      case OfficeUnderline.Single:
        return "sng";
      case OfficeUnderline.Double:
        return "dbl";
      case OfficeUnderline.Dash:
        return "dash";
      case OfficeUnderline.DotDotDashHeavy:
        return "dotDotDashHeavy";
      case OfficeUnderline.DotDashHeavy:
        return "dotDashHeavy";
      case OfficeUnderline.DashHeavy:
        return "dashHeavy";
      case OfficeUnderline.DashLong:
        return "dashLong";
      case OfficeUnderline.DashLongHeavy:
        return "dashLongHeavy";
      case OfficeUnderline.DotDash:
        return "dotDash";
      case OfficeUnderline.DotDotDash:
        return "dotDotDash";
      case OfficeUnderline.Dotted:
        return "dotted";
      case OfficeUnderline.DottedHeavy:
        return "dottedHeavy";
      case OfficeUnderline.Heavy:
        return "heavy";
      case OfficeUnderline.Wavy:
        return "wavy";
      case OfficeUnderline.WavyDouble:
        return "wavyDbl";
      case OfficeUnderline.WavyHeavy:
        return "wavyHeavy";
      case OfficeUnderline.Words:
        return "words";
      default:
        throw new ArgumentException("Invalid OfficeUnderlineType value");
    }
  }

  internal static string GetOfficeChartType(OfficeChartType officeChartType)
  {
    switch (officeChartType)
    {
      case OfficeChartType.Bar_Clustered:
        return "barChart";
      case OfficeChartType.Bar_Clustered_3D:
        return "bar3DChart";
      case OfficeChartType.Line:
        return "lineChart";
      case OfficeChartType.Line_3D:
        return "line3DChart";
      case OfficeChartType.Pie:
        return "pieChart";
      case OfficeChartType.Pie_3D:
        return "pie3DChart";
      case OfficeChartType.PieOfPie:
        return "pie";
      case OfficeChartType.Pie_Exploded:
      case OfficeChartType.Pie_Exploded_3D:
        return "explosion";
      case OfficeChartType.Pie_Bar:
        return "bar";
      case OfficeChartType.Scatter_Markers:
      case OfficeChartType.Scatter_SmoothedLine_Markers:
      case OfficeChartType.Scatter_SmoothedLine:
      case OfficeChartType.Scatter_Line_Markers:
      case OfficeChartType.Scatter_Line:
        return "scatterChart";
      case OfficeChartType.Area:
        return "areaChart";
      case OfficeChartType.Area_3D:
        return "area3DChart";
      case OfficeChartType.Doughnut:
      case OfficeChartType.Doughnut_Exploded:
        return "doughnutChart";
      case OfficeChartType.Radar:
        return "radarChart";
      case OfficeChartType.Surface_3D:
      case OfficeChartType.Surface_NoColor_3D:
        return "surface3DChart";
      case OfficeChartType.Surface_Contour:
      case OfficeChartType.Surface_NoColor_Contour:
        return "surfaceChart";
      case OfficeChartType.Bubble:
        return "bubbleChart";
      case OfficeChartType.Bubble_3D:
        return "bubble3D";
      case OfficeChartType.Stock_HighLowClose:
      case OfficeChartType.Stock_OpenHighLowClose:
      case OfficeChartType.Stock_VolumeHighLowClose:
      case OfficeChartType.Stock_VolumeOpenHighLowClose:
        return "stockChart";
      default:
        return (string) null;
    }
  }

  internal static string GetVerticalFlowType(TextVertOverflowType textVertOverflowType)
  {
    switch (textVertOverflowType)
    {
      case TextVertOverflowType.Ellipsis:
        return "ellipsis";
      case TextVertOverflowType.Clip:
        return "clip";
      default:
        return "overflow";
    }
  }

  internal static string GetHorizontalFlowType(TextHorzOverflowType textHorzOverflowType)
  {
    return textHorzOverflowType == TextHorzOverflowType.Clip ? "clip" : "overflow";
  }

  internal static TextVertOverflowType GetVerticalFlowType(string value)
  {
    switch (value)
    {
      case "clip":
        return TextVertOverflowType.Clip;
      case "ellipsis":
        return TextVertOverflowType.Ellipsis;
      default:
        return TextVertOverflowType.OverFlow;
    }
  }

  internal static TextHorzOverflowType GetHorizontalFlowType(string value)
  {
    switch (value)
    {
      case "clip":
        return TextHorzOverflowType.Clip;
      default:
        return TextHorzOverflowType.OverFlow;
    }
  }

  internal static TextDirection SetTextDirection(string textVerticalType)
  {
    switch (textVerticalType)
    {
      case "horz":
        return TextDirection.Horizontal;
      case "vert":
        return TextDirection.RotateAllText90;
      case "vert270":
        return TextDirection.RotateAllText270;
      case "wordArtVert":
        return TextDirection.StackedLeftToRight;
      case "wordArtVertRtl":
        return TextDirection.StackedRightToLeft;
      default:
        return TextDirection.Horizontal;
    }
  }

  internal static void SetAnchorPosition(TextFrame txtFrame, string anchorType, bool anchorCtrl)
  {
    switch (txtFrame.TextDirection)
    {
      case TextDirection.Horizontal:
        switch (anchorType)
        {
          case null:
            return;
          case "t":
            if (anchorCtrl)
            {
              txtFrame.VerticalAlignment = OfficeVerticalAlignment.TopCentered;
              return;
            }
            txtFrame.VerticalAlignment = OfficeVerticalAlignment.Top;
            return;
          case "ctr":
            if (anchorCtrl)
            {
              txtFrame.VerticalAlignment = OfficeVerticalAlignment.MiddleCentered;
              return;
            }
            txtFrame.VerticalAlignment = OfficeVerticalAlignment.Middle;
            return;
          case "b":
            if (anchorCtrl)
            {
              txtFrame.VerticalAlignment = OfficeVerticalAlignment.BottomCentered;
              return;
            }
            txtFrame.VerticalAlignment = OfficeVerticalAlignment.Bottom;
            return;
          default:
            return;
        }
      case TextDirection.RotateAllText90:
      case TextDirection.StackedRightToLeft:
        switch (anchorType)
        {
          case null:
            return;
          case "t":
            if (anchorCtrl)
            {
              txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.RightMiddle;
              return;
            }
            txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.Right;
            return;
          case "ctr":
            if (anchorCtrl)
            {
              txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.CenterMiddle;
              return;
            }
            txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.Center;
            return;
          case "b":
            if (anchorCtrl)
            {
              txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.LeftMiddle;
              return;
            }
            txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.Left;
            return;
          default:
            return;
        }
      case TextDirection.RotateAllText270:
      case TextDirection.StackedLeftToRight:
        switch (anchorType)
        {
          case null:
            return;
          case "t":
            if (anchorCtrl)
            {
              txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.LeftMiddle;
              return;
            }
            txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.Left;
            return;
          case "ctr":
            if (anchorCtrl)
            {
              txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.CenterMiddle;
              return;
            }
            txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.Center;
            return;
          case "b":
            if (anchorCtrl)
            {
              txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.RightMiddle;
              return;
            }
            txtFrame.HorizontalAlignment = OfficeHorizontalAlignment.Right;
            return;
          default:
            return;
        }
    }
  }

  internal static double ParseDouble(string value)
  {
    return double.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static int ParseInt(string value)
  {
    return int.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string ToString(double value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string ToString(int value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static bool ParseBoolen(string value)
  {
    if (value.Length != 1)
      return string.Compare(value, "true") == 0;
    switch (value)
    {
      case "1":
      case "t":
      case "T":
        return true;
      default:
        return false;
    }
  }

  internal static short ParseShort(string value)
  {
    return short.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static int ConvertEmuToOffset(int emuValue, int resolution)
  {
    return (int) ((double) emuValue / 12700.0 / 72.0 * (double) resolution + 0.5);
  }

  internal static int ConvertOffsetToEMU(int offsetValue, int resolution)
  {
    return (int) ((double) offsetValue * 72.0 / (double) resolution * 12700.0 + 0.5);
  }

  internal static AnchorType GetAnchorType(string anchorType)
  {
    switch (anchorType)
    {
      case "oneCellAnchor":
        return AnchorType.OneCell;
      case "absoluteAnchor":
        return AnchorType.Absolute;
      case "relSizeAnchor":
        return AnchorType.RelSize;
      default:
        return AnchorType.TwoCell;
    }
  }

  internal static string GetAnchorTypeString(AnchorType anchorType)
  {
    switch (anchorType)
    {
      case AnchorType.Absolute:
        return "absoluteAnchor";
      case AnchorType.RelSize:
        return "relSizeAnchor";
      case AnchorType.OneCell:
        return "oneCellAnchor";
      default:
        return "twoCellAnchor";
    }
  }

  internal static double EmuToPoint(int emu) => Convert.ToDouble((double) emu / 12700.0);

  internal static int PointToEmu(double point) => (int) (point * 12700.0);
}
