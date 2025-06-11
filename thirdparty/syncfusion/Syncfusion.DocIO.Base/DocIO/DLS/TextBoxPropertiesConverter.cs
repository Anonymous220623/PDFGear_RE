// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextBoxPropertiesConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.DataStreamParser;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class TextBoxPropertiesConverter
{
  public static void Export(TextBoxProps txbxProps, WTextBoxFormat txbxFormat)
  {
    txbxFormat.HorizontalPosition = (float) txbxProps.XaLeft / 20f;
    txbxFormat.VerticalPosition = (float) txbxProps.YaTop / 20f;
    txbxFormat.Width = (float) txbxProps.Width / 20f;
    txbxFormat.Height = (float) txbxProps.Height / 20f;
    if (txbxProps.LeftMargin != uint.MaxValue)
      txbxFormat.InternalMargin.Left = (float) txbxProps.LeftMargin / 12700f;
    if (txbxProps.RightMargin != uint.MaxValue)
      txbxFormat.InternalMargin.Right = (float) txbxProps.RightMargin / 12700f;
    if (txbxProps.TopMargin != uint.MaxValue)
      txbxFormat.InternalMargin.Top = (float) txbxProps.TopMargin / 12700f;
    if (txbxProps.BottomMargin != uint.MaxValue)
      txbxFormat.InternalMargin.Bottom = (float) txbxProps.BottomMargin / 12700f;
    txbxFormat.HorizontalAlignment = txbxProps.HorizontalAlignment;
    txbxFormat.VerticalAlignment = txbxProps.VerticalAlignment;
    txbxFormat.HorizontalOrigin = txbxProps.RelHrzPos;
    txbxFormat.VerticalOrigin = txbxProps.RelVrtPos;
    txbxFormat.FillColor = txbxProps.FillColor;
    txbxFormat.LineColor = txbxProps.LineColor;
    txbxFormat.LineDashing = txbxProps.LineDashing;
    txbxFormat.LineStyle = txbxProps.LineStyle;
    txbxFormat.LineWidth = txbxProps.TxbxLineWidth;
    txbxFormat.NoLine = txbxProps.NoLine;
    txbxFormat.AutoFit = txbxProps.FitShapeToText;
    txbxFormat.SetTextWrappingStyleValue(txbxProps.TextWrappingStyle);
    txbxFormat.TextWrappingType = txbxProps.TextWrappingType;
    txbxFormat.WrappingMode = txbxProps.WrapText;
    txbxFormat.IsBelowText = txbxProps.IsBelowText;
    txbxFormat.TextBoxIdentificator = txbxProps.TXID;
    txbxFormat.IsHeaderTextBox = txbxProps.IsHeaderShape;
    txbxFormat.TextBoxShapeID = txbxProps.Spid;
  }

  public static void Import(WTextBoxFormat txbxFormat, TextBoxProps txbxProps)
  {
    txbxProps.XaLeft = (int) Math.Round((double) txbxFormat.HorizontalPosition * 20.0);
    txbxProps.YaTop = (int) Math.Round((double) txbxFormat.VerticalPosition * 20.0);
    txbxProps.Width = (int) Math.Round((double) txbxFormat.Width * 20.0);
    txbxProps.Height = (int) Math.Round((double) txbxFormat.Height * 20.0);
    if (txbxFormat.HorizontalOrigin == HorizontalOrigin.LeftMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.RightMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.InsideMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.OutsideMargin)
      txbxProps.RelHrzPos = HorizontalOrigin.Margin;
    else
      txbxProps.RelHrzPos = txbxFormat.HorizontalOrigin;
    if (txbxFormat.VerticalOrigin == VerticalOrigin.TopMargin || txbxFormat.VerticalOrigin == VerticalOrigin.BottomMargin || txbxFormat.VerticalOrigin == VerticalOrigin.InsideMargin || txbxFormat.VerticalOrigin == VerticalOrigin.OutsideMargin)
      txbxProps.RelVrtPos = VerticalOrigin.Page;
    else
      txbxProps.RelVrtPos = txbxFormat.VerticalOrigin;
    txbxProps.HorizontalAlignment = txbxFormat.HorizontalAlignment;
    txbxProps.VerticalAlignment = txbxFormat.VerticalAlignment;
    if ((double) txbxFormat.InternalMargin.Left != 7.0869998931884766)
      txbxProps.LeftMargin = (uint) Math.Round((double) txbxFormat.InternalMargin.Left * 12700.0);
    if ((double) txbxFormat.InternalMargin.Right != 7.0869998931884766)
      txbxProps.RightMargin = (uint) Math.Round((double) txbxFormat.InternalMargin.Right * 12700.0);
    if ((double) txbxFormat.InternalMargin.Top != 3.684999942779541)
      txbxProps.TopMargin = (uint) Math.Round((double) txbxFormat.InternalMargin.Top * 12700.0);
    if ((double) txbxFormat.InternalMargin.Bottom != 3.684999942779541)
      txbxProps.BottomMargin = (uint) Math.Round((double) txbxFormat.InternalMargin.Bottom * 12700.0);
    txbxProps.FillColor = txbxFormat.FillColor;
    txbxProps.LineColor = txbxFormat.LineColor;
    txbxProps.LineDashing = txbxFormat.LineDashing;
    txbxProps.LineStyle = txbxFormat.LineStyle;
    txbxProps.TxbxLineWidth = txbxFormat.LineWidth;
    txbxProps.NoLine = txbxFormat.NoLine;
    txbxProps.FitShapeToText = txbxFormat.AutoFit;
    txbxProps.TextWrappingStyle = txbxFormat.TextWrappingStyle;
    txbxProps.TextWrappingType = txbxFormat.TextWrappingType;
    txbxProps.WrapText = txbxFormat.WrappingMode;
    txbxProps.IsBelowText = txbxFormat.IsBelowText;
    txbxProps.Spid = txbxFormat.TextBoxShapeID;
    txbxProps.TXID = txbxFormat.TextBoxIdentificator;
  }

  internal static void Export(
    MsofbtSpContainer txbxContainer,
    FileShapeAddress fspa,
    WTextBoxFormat txbxFormat,
    bool skipPositionOrigins)
  {
    txbxFormat.HorizontalPosition = (float) fspa.XaLeft / 20f;
    txbxFormat.VerticalPosition = (float) fspa.YaTop / 20f;
    if (!skipPositionOrigins)
    {
      txbxFormat.HorizontalOrigin = fspa.RelHrzPos;
      txbxFormat.VerticalOrigin = fspa.RelVrtPos;
    }
    txbxFormat.Width = (float) fspa.Width / 20f;
    txbxFormat.Height = (float) fspa.Height / 20f;
    txbxFormat.SetTextWrappingStyleValue(fspa.TextWrappingStyle);
    txbxFormat.TextWrappingType = fspa.TextWrappingType;
    txbxFormat.IsHeaderTextBox = fspa.IsHeaderShape;
    txbxFormat.TextBoxShapeID = fspa.Spid;
    byte[] complexPropValue = txbxContainer.GetComplexPropValue(896);
    if (complexPropValue != null)
    {
      string str = Encoding.Unicode.GetString(complexPropValue);
      if (str.Contains("\0"))
        str = str.TrimEnd(new char[1]);
      txbxFormat.Name = str;
    }
    if ((txbxFormat.TextWrappingStyle == TextWrappingStyle.Tight || txbxFormat.TextWrappingStyle == TextWrappingStyle.Through) && txbxContainer.ShapeOptions.Properties.Contains(899))
    {
      txbxFormat.WrapPolygon = new WrapPolygon();
      txbxFormat.WrapPolygon.Edited = false;
      for (int index = 0; index < txbxContainer.ShapeOptions.WrapPolygonVertices.Coords.Count; ++index)
        txbxFormat.WrapPolygon.Vertices.Add(txbxContainer.ShapeOptions.WrapPolygonVertices.Coords[index]);
    }
    if (txbxContainer.ShapePosition != null && txbxContainer.ShapePosition.Properties.ContainsKey(959))
      txbxFormat.AllowInCell = txbxContainer.ShapePosition.AllowInTableCell;
    else if (txbxContainer.ShapeOptions != null && txbxContainer.ShapeOptions.Properties.ContainsKey(959))
      txbxFormat.AllowInCell = txbxContainer.ShapeOptions.AllowInTableCell;
    txbxFormat.UpdateFillEffects(txbxContainer, txbxFormat.Document);
    uint propertyValue1 = txbxContainer.GetPropertyValue(459);
    if (propertyValue1 != uint.MaxValue)
      txbxFormat.LineWidth = (float) propertyValue1 / 12700f;
    uint propertyValue2 = txbxContainer.GetPropertyValue(461);
    if (propertyValue2 != uint.MaxValue)
      txbxFormat.LineStyle = (TextBoxLineStyle) propertyValue2;
    uint propertyValue3 = txbxContainer.GetPropertyValue(462);
    if (propertyValue3 != uint.MaxValue)
      txbxFormat.LineDashing = (LineDashing) propertyValue3;
    uint propertyValue4 = txbxContainer.GetPropertyValue(133);
    if (propertyValue4 != uint.MaxValue)
      txbxFormat.WrappingMode = (WrapMode) propertyValue4;
    switch (txbxContainer.GetPropertyValue(136))
    {
      case 1:
        txbxFormat.TextDirection = TextDirection.VerticalFarEast;
        goto case uint.MaxValue;
      case 2:
        txbxFormat.TextDirection = TextDirection.VerticalBottomToTop;
        goto case uint.MaxValue;
      case 3:
        txbxFormat.TextDirection = TextDirection.VerticalTopToBottom;
        goto case uint.MaxValue;
      case 4:
        txbxFormat.TextDirection = TextDirection.HorizontalFarEast;
        goto case uint.MaxValue;
      case 5:
        txbxFormat.TextDirection = TextDirection.Vertical;
        goto case uint.MaxValue;
      case uint.MaxValue:
        uint propertyValue5 = txbxContainer.GetPropertyValue(385);
        if (propertyValue5 != uint.MaxValue && txbxFormat.FillEfects.Type == BackgroundType.NoBackground)
          txbxFormat.FillColor = WordColor.ConvertRGBToColor(propertyValue5);
        uint propertyValue6 = txbxContainer.GetPropertyValue(448);
        if (propertyValue6 != uint.MaxValue)
          txbxFormat.LineColor = WordColor.ConvertRGBToColor(propertyValue6);
        if (((int) txbxContainer.GetPropertyValue(447) & 16 /*0x10*/) != 16 /*0x10*/)
          txbxFormat.FillColor = Color.Empty;
        uint propertyValue7 = txbxContainer.GetPropertyValue(191);
        if (propertyValue7 != uint.MaxValue)
          txbxFormat.AutoFit = ((int) propertyValue7 & 2) != 0;
        uint propertyValue8 = txbxContainer.GetPropertyValue(511 /*0x01FF*/);
        if (propertyValue8 != uint.MaxValue)
          txbxFormat.NoLine = ((int) propertyValue8 & 8) == 0;
        txbxFormat.TextBoxIdentificator = (float) txbxContainer.GetPropertyValue(128 /*0x80*/);
        uint propertyValue9 = txbxContainer.GetPropertyValue(959);
        txbxFormat.IsBelowText = propertyValue9 != uint.MaxValue && ((int) propertyValue9 & 32 /*0x20*/) == 32 /*0x20*/;
        if (txbxContainer.ShapePosition != null)
          TextBoxPropertiesConverter.ExportPosition(txbxContainer, txbxFormat);
        TextBoxPropertiesConverter.ExportIntMargin(txbxContainer, txbxFormat);
        break;
      default:
        txbxFormat.TextDirection = TextDirection.Horizontal;
        goto case uint.MaxValue;
    }
  }

  internal static void Import(FileShapeAddress fspa, WTextBoxFormat txbxFormat)
  {
    fspa.XaLeft = (int) Math.Round((double) txbxFormat.HorizontalPosition * 20.0);
    fspa.YaTop = (int) Math.Round((double) txbxFormat.VerticalPosition * 20.0);
    fspa.Width = (int) Math.Round((double) txbxFormat.Width * 20.0);
    fspa.Height = (int) Math.Round((double) txbxFormat.Height * 20.0);
    fspa.RelHrzPos = txbxFormat.HorizontalOrigin == HorizontalOrigin.LeftMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.RightMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.InsideMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.OutsideMargin ? HorizontalOrigin.Margin : txbxFormat.HorizontalOrigin;
    if (txbxFormat.VerticalOrigin == VerticalOrigin.TopMargin || txbxFormat.VerticalOrigin == VerticalOrigin.BottomMargin || txbxFormat.VerticalOrigin == VerticalOrigin.InsideMargin || txbxFormat.VerticalOrigin == VerticalOrigin.OutsideMargin)
    {
      fspa.RelVrtPos = VerticalOrigin.Page;
      if ((double) txbxFormat.VerticalPosition < 0.0)
        fspa.YaTop = 0;
    }
    else
      fspa.RelVrtPos = txbxFormat.VerticalOrigin;
    fspa.TextWrappingStyle = txbxFormat.TextWrappingStyle;
    fspa.TextWrappingType = txbxFormat.TextWrappingType;
    fspa.IsBelowText = txbxFormat.IsBelowText;
    fspa.Spid = txbxFormat.TextBoxShapeID;
  }

  private static void ExportPosition(MsofbtSpContainer txbxContainer, WTextBoxFormat txbxFormat)
  {
    if (txbxContainer.ShapePosition.XAlign != uint.MaxValue)
      txbxFormat.HorizontalAlignment = (ShapeHorizontalAlignment) txbxContainer.ShapePosition.XAlign;
    if (txbxContainer.ShapePosition.YAlign != uint.MaxValue)
      txbxFormat.VerticalAlignment = (ShapeVerticalAlignment) txbxContainer.ShapePosition.YAlign;
    if (txbxContainer.ShapePosition.XRelTo != uint.MaxValue)
      txbxFormat.HorizontalOrigin = (HorizontalOrigin) txbxContainer.ShapePosition.XRelTo;
    if (txbxContainer.ShapePosition.YRelTo != uint.MaxValue)
      txbxFormat.VerticalOrigin = (VerticalOrigin) txbxContainer.ShapePosition.YRelTo;
    if (txbxContainer.ShapeOptions.DistanceFromRight != uint.MaxValue)
      txbxFormat.WrapDistanceRight = (float) txbxContainer.ShapeOptions.DistanceFromRight / 12700f;
    if (txbxContainer.ShapeOptions.DistanceFromLeft != uint.MaxValue)
      txbxFormat.WrapDistanceLeft = (float) txbxContainer.ShapeOptions.DistanceFromLeft / 12700f;
    if (txbxContainer.ShapeOptions.DistanceFromBottom != uint.MaxValue)
      txbxFormat.WrapDistanceBottom = (float) txbxContainer.ShapeOptions.DistanceFromBottom / 12700f;
    if (txbxContainer.ShapeOptions.DistanceFromTop == uint.MaxValue)
      return;
    txbxFormat.WrapDistanceTop = (float) txbxContainer.ShapeOptions.DistanceFromTop / 12700f;
  }

  private static void ExportIntMargin(MsofbtSpContainer txbxContainer, WTextBoxFormat txbxFormat)
  {
    uint propertyValue1 = txbxContainer.GetPropertyValue(129);
    if (propertyValue1 != uint.MaxValue)
      txbxFormat.InternalMargin.Left = (float) propertyValue1 / 12700f;
    uint propertyValue2 = txbxContainer.GetPropertyValue(131);
    if (propertyValue2 != uint.MaxValue)
      txbxFormat.InternalMargin.Right = (float) propertyValue2 / 12700f;
    uint propertyValue3 = txbxContainer.GetPropertyValue(130);
    if (propertyValue3 != uint.MaxValue)
      txbxFormat.InternalMargin.Top = (float) propertyValue3 / 12700f;
    uint propertyValue4 = txbxContainer.GetPropertyValue(132);
    if (propertyValue4 == uint.MaxValue)
      return;
    txbxFormat.InternalMargin.Bottom = (float) propertyValue4 / 12700f;
  }
}
