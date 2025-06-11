// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.Helper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
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
      case "eaVert":
        return TextDirection.RotateAllText90;
      default:
        return TextDirection.Horizontal;
    }
  }

  internal static void SetAnchorPosition(
    TextBodyPropertiesHolder textProperties,
    string anchorType,
    bool anchorCtrl)
  {
    switch (textProperties.TextDirection)
    {
      case TextDirection.Horizontal:
        switch (anchorType)
        {
          case null:
            return;
          case "t":
            if (anchorCtrl)
            {
              textProperties.VerticalAlignment = ExcelVerticalAlignment.TopCentered;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.VerticalAlignment = ExcelVerticalAlignment.Top;
            return;
          case "ctr":
            if (anchorCtrl)
            {
              textProperties.VerticalAlignment = ExcelVerticalAlignment.MiddleCentered;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.VerticalAlignment = ExcelVerticalAlignment.Middle;
            return;
          case "b":
            if (anchorCtrl)
            {
              textProperties.VerticalAlignment = ExcelVerticalAlignment.BottomCentered;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.VerticalAlignment = ExcelVerticalAlignment.Bottom;
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
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.RightMiddle;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            return;
          case "ctr":
            if (anchorCtrl)
            {
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.CenterMiddle;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return;
          case "b":
            if (anchorCtrl)
            {
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.LeftMiddle;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Left;
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
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.LeftMiddle;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            return;
          case "ctr":
            if (anchorCtrl)
            {
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.CenterMiddle;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return;
          case "b":
            if (anchorCtrl)
            {
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.RightMiddle;
              textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              return;
            }
            textProperties.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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

  internal static double ToDouble(string value)
  {
    return double.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }
}
