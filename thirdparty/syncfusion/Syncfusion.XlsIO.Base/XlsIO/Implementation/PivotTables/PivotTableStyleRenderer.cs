// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableStyleRenderer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotTableStyleRenderer
{
  private IWorksheet wkSheet;

  internal PivotTableStyleRenderer()
  {
  }

  internal PivotTableStyleRenderer(IWorksheet worksheet) => this.wkSheet = worksheet;

  internal ExtendedFormatImpl ApplyStyles(
    PivotBuiltInStyles? BuildinStyle,
    PivotTableParts tableParts)
  {
    ExtendedFormatImpl extendedFormatImpl = new ExtendedFormatImpl(this.wkSheet.Application, (object) this.wkSheet.Workbook);
    WorkbookImpl workbook = this.wkSheet.Workbook as WorkbookImpl;
    FontImpl font = (FontImpl) workbook.CreateFont((IFont) null, false);
    FontImpl fontImpl = (FontImpl) workbook.InnerFonts.Add((IFont) font);
    extendedFormatImpl.FontIndex = fontImpl.Font.Index;
    ref PivotBuiltInStyles? local = ref BuildinStyle;
    PivotBuiltInStyles valueOrDefault = local.GetValueOrDefault();
    if (local.HasValue)
    {
      switch (valueOrDefault)
      {
        case PivotBuiltInStyles.PivotStyleMedium28:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(252, 213, 180);
            extendedFormatImpl.Color = Color.FromArgb(252, 213, 180);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium27:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(183, 222, 232);
            extendedFormatImpl.Color = Color.FromArgb(183, 222, 232);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium26:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.FontName = "Accent 2";
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(204, 192 /*0xC0*/, 218);
            extendedFormatImpl.Color = Color.FromArgb(204, 192 /*0xC0*/, 218);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium25:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(215, 228, 188);
            extendedFormatImpl.Color = Color.FromArgb(215, 228, 188);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium24:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.FontName = "Accent 2";
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium23:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(184, 204, 228);
            extendedFormatImpl.Color = Color.FromArgb(184, 204, 228);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium22:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(191, 191, 191);
            extendedFormatImpl.Color = Color.FromArgb(191, 191, 191);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium21:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(252, 213, 180);
            extendedFormatImpl.Color = Color.FromArgb(252, 213, 180);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium20:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(183, 222, 232);
            extendedFormatImpl.Color = Color.FromArgb(183, 222, 232);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium19:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(204, 192 /*0xC0*/, 218);
            extendedFormatImpl.Color = Color.FromArgb(204, 192 /*0xC0*/, 218);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium18:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(216, 228, 188);
            extendedFormatImpl.Color = Color.FromArgb(216, 228, 188);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium17:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium16:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(184, 204, 228);
            extendedFormatImpl.Color = Color.FromArgb(184, 204, 228);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 242, 242);
            extendedFormatImpl.Color = Color.FromArgb(242, 242, 242);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium15:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 242, 242);
            extendedFormatImpl.Color = Color.FromArgb(242, 242, 242);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(191, 191, 191);
            extendedFormatImpl.Color = Color.FromArgb(191, 191, 191);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium14:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Color = Color.FromArgb(247, 150, 70);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(252, 213, 180);
            extendedFormatImpl.Color = Color.FromArgb(252, 213, 180);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Hair;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium13:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Color = Color.FromArgb(75, 172, 198);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(183, 222, 232);
            extendedFormatImpl.Color = Color.FromArgb(183, 222, 232);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Hair;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium12:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 100, 162);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(204, 192 /*0xC0*/, 218);
            extendedFormatImpl.Color = Color.FromArgb(204, 192 /*0xC0*/, 218);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Hair;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium11:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Color = Color.FromArgb(155, 187, 89);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(216, 228, 188);
            extendedFormatImpl.Color = Color.FromArgb(216, 228, 188);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Hair;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium10:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Color = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Hair;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium9:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Color = Color.FromArgb(79, 129, 189);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(184, 204, 228);
            extendedFormatImpl.Color = Color.FromArgb(184, 204, 228);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium8:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(191, 191, 191);
            extendedFormatImpl.Color = Color.FromArgb(191, 191, 191);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium7:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Color = Color.FromArgb(226, 107, 10);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Color = Color.FromArgb(250, 191, 143);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(252, 213, 180);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Color = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium6:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Color = Color.FromArgb(49, 134, 155);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Color = Color.FromArgb(146, 205, 220);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(183, 222, 232);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Color = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium5:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(96 /*0x60*/, 72, 122);
            extendedFormatImpl.Color = Color.FromArgb(96 /*0x60*/, 72, 122);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(177, 160 /*0xA0*/, 199);
            extendedFormatImpl.Color = Color.FromArgb(177, 160 /*0xA0*/, 199);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(204, 192 /*0xC0*/, 218);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(177, 160 /*0xA0*/, 199);
            extendedFormatImpl.Color = Color.FromArgb(177, 160 /*0xA0*/, 199);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium4:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Color = Color.FromArgb(118, 147, 60);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Color = Color.FromArgb(196, 215, 155);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(216, 228, 188);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.PatternColor = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium3:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Color = Color.FromArgb(150, 54, 52);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Color = Color.FromArgb(218, 150, 148);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Color = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium2:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Color = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.PatternColor = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Color = Color.FromArgb(149, 179, 215);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(54, 96 /*0x60*/, 145);
            extendedFormatImpl.Color = Color.FromArgb(54, 96 /*0x60*/, 145);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Color = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleMedium1:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Color = Color.FromArgb(166, 166, 166);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Color = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Hair;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight28:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight27:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight26:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight25:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight24:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight23:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight22:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight21:
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight20:
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight19:
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(177, 160 /*0xA0*/, 199);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(177, 160 /*0xA0*/, 199);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight18:
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight17:
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight16:
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.FillPattern = ExcelPattern.Solid;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight15:
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight14:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.ColumnSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight13:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.ColumnSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight12:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.ColumnSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight11:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.ColumnSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight10:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.ColumnSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight9:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading3 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight8:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight7:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight6:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight5:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight4:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight3:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight2:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleLight1:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.PatternColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark28:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Color = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Color = Color.FromArgb(226, 107, 10);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Color = Color.FromArgb(226, 107, 10);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark27:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Color = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Color = Color.FromArgb(49, 134, 155);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Color = Color.FromArgb(49, 134, 155);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark26:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Color = Color.FromArgb(96 /*0x60*/, 73, 122);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Color = Color.FromArgb(96 /*0x60*/, 73, 122);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark25:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Color = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Color = Color.FromArgb(118, 147, 60);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Color = Color.FromArgb(118, 147, 60);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark24:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Color = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Color = Color.FromArgb(150, 54, 52);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Color = Color.FromArgb(150, 54, 52);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark23:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Color = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Color = Color.FromArgb(54, 96 /*0x60*/, 146);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Color = Color.FromArgb(54, 96 /*0x60*/, 146);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark22:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          }
          if ((PivotTableParts.FirstColumn & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          if ((PivotTableParts.FirstHeaderCell & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.SubtotalRow2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark21:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(247, 150, 70);
            extendedFormatImpl.Color = Color.FromArgb(247, 150, 70);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.PatternColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Color = Color.FromArgb(226, 107, 10);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(226, 107, 10);
            extendedFormatImpl.Color = Color.FromArgb(226, 107, 10);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark20:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(75, 172, 198);
            extendedFormatImpl.Color = Color.FromArgb(75, 172, 198);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.PatternColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Color = Color.FromArgb(49, 134, 155);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(49, 134, 155);
            extendedFormatImpl.Color = Color.FromArgb(49, 134, 155);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark19:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 100, 162);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 100, 162);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.PatternColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Color = Color.FromArgb(96 /*0x60*/, 73, 122);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(96 /*0x60*/, 73, 122);
            extendedFormatImpl.Color = Color.FromArgb(96 /*0x60*/, 73, 122);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark18:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(155, 187, 89);
            extendedFormatImpl.Color = Color.FromArgb(155, 187, 89);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.PatternColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Color = Color.FromArgb(118, 147, 60);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(118, 147, 60);
            extendedFormatImpl.Color = Color.FromArgb(118, 147, 60);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark17:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            extendedFormatImpl.Color = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.PatternColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Color = Color.FromArgb(150, 54, 52);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(150, 54, 52);
            extendedFormatImpl.Color = Color.FromArgb(150, 54, 52);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark16:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(79, 129, 189);
            extendedFormatImpl.Color = Color.FromArgb(79, 129, 189);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.PatternColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Color = Color.FromArgb(54, 96 /*0x60*/, 146);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(54, 96 /*0x60*/, 146);
            extendedFormatImpl.Color = Color.FromArgb(54, 96 /*0x60*/, 146);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark15:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(140, 140, 140);
            extendedFormatImpl.Color = Color.FromArgb(140, 140, 140);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalColumn1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.ColumnSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Color = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark14:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(252, 213, 180);
            extendedFormatImpl.Color = Color.FromArgb(252, 213, 180);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.PatternColor = Color.FromArgb(252, 213, 180);
            extendedFormatImpl.Color = Color.FromArgb(252, 213, 180);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark13:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(183, 222, 232);
            extendedFormatImpl.Color = Color.FromArgb(183, 222, 232);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.PatternColor = Color.FromArgb(183, 222, 232);
            extendedFormatImpl.Color = Color.FromArgb(183, 222, 232);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark12:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(204, 192 /*0xC0*/, 218);
            extendedFormatImpl.Color = Color.FromArgb(204, 192 /*0xC0*/, 218);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.PatternColor = Color.FromArgb(204, 192 /*0xC0*/, 218);
            extendedFormatImpl.Color = Color.FromArgb(204, 192 /*0xC0*/, 218);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark11:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(216, 228, 188);
            extendedFormatImpl.Color = Color.FromArgb(216, 228, 188);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.PatternColor = Color.FromArgb(216, 228, 188);
            extendedFormatImpl.Color = Color.FromArgb(216, 228, 188);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark10:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark9:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(184, 204, 228);
            extendedFormatImpl.Color = Color.FromArgb(184, 204, 228);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.PatternColor = Color.FromArgb(184, 204, 228);
            extendedFormatImpl.Color = Color.FromArgb(184, 204, 228);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark8:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(191, 191, 191);
            extendedFormatImpl.Color = Color.FromArgb(191, 191, 191);
          }
          int num1 = (int) (PivotTableParts.ColumnSubHeading2 & tableParts);
          int num2 = (int) (PivotTableParts.ColumnSubHeading3 & tableParts);
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.PatternColor = Color.FromArgb(191, 191, 191);
            extendedFormatImpl.Color = Color.FromArgb(191, 191, 191);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
            extendedFormatImpl.Font.Bold = true;
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark7:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(252, 213, 180);
            extendedFormatImpl.Color = Color.FromArgb(252, 213, 180);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(151, 71, 6);
            extendedFormatImpl.Color = Color.FromArgb(151, 71, 6);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(250, 191, 143);
            extendedFormatImpl.PatternColor = Color.FromArgb(253, 233, 217);
            extendedFormatImpl.Color = Color.FromArgb(253, 233, 217);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(151, 71, 6);
            extendedFormatImpl.PatternColor = Color.FromArgb(151, 71, 6);
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(253, 233, 217);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark6:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(183, 222, 232);
            extendedFormatImpl.Color = Color.FromArgb(183, 222, 232);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(33, 89, 103);
            extendedFormatImpl.Color = Color.FromArgb(33, 89, 103);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(146, 205, 220);
            extendedFormatImpl.PatternColor = Color.FromArgb(218, 238, 243);
            extendedFormatImpl.Color = Color.FromArgb(218, 238, 243);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(33, 89, 103);
            extendedFormatImpl.PatternColor = Color.FromArgb(33, 89, 103);
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 238, 243);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark5:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(204, 192 /*0xC0*/, 218);
            extendedFormatImpl.Color = Color.FromArgb(204, 192 /*0xC0*/, 218);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 49, 81);
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 49, 81);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(177, 160 /*0xA0*/, 199);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(177, 160 /*0xA0*/, 199);
            extendedFormatImpl.PatternColor = Color.FromArgb(228, 223, 236);
            extendedFormatImpl.Color = Color.FromArgb(228, 223, 236);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(64 /*0x40*/, 49, 81);
            extendedFormatImpl.PatternColor = Color.FromArgb(64 /*0x40*/, 49, 81);
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(228, 223, 236);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark4:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(216, 228, 188);
            extendedFormatImpl.Color = Color.FromArgb(216, 228, 188);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(79, 98, 40);
            extendedFormatImpl.Color = Color.FromArgb(79, 98, 40);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(196, 215, 155);
            extendedFormatImpl.PatternColor = Color.FromArgb(235, 241, 222);
            extendedFormatImpl.Color = Color.FromArgb(235, 241, 222);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(79, 98, 40);
            extendedFormatImpl.PatternColor = Color.FromArgb(79, 98, 40);
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(235, 241, 222);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark3:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(230, 184, 183);
            extendedFormatImpl.Color = Color.FromArgb(230, 184, 183);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(99, 37, 35);
            extendedFormatImpl.Color = Color.FromArgb(99, 37, 35);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 150, 148);
            extendedFormatImpl.PatternColor = Color.FromArgb(242, 220, 219);
            extendedFormatImpl.Color = Color.FromArgb(242, 220, 219);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(99, 37, 35);
            extendedFormatImpl.PatternColor = Color.FromArgb(99, 37, 35);
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(242, 220, 219);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark2:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(184, 204, 228);
            extendedFormatImpl.Color = Color.FromArgb(184, 204, 228);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(36, 64 /*0x40*/, 98);
            extendedFormatImpl.Color = Color.FromArgb(36, 64 /*0x40*/, 98);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(149, 179, 215);
            extendedFormatImpl.PatternColor = Color.FromArgb(220, 230, 241);
            extendedFormatImpl.Color = Color.FromArgb(220, 230, 241);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(36, 64 /*0x40*/, 98);
            extendedFormatImpl.PatternColor = Color.FromArgb(36, 64 /*0x40*/, 98);
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(220, 230, 241);
            break;
          }
          break;
        case PivotBuiltInStyles.PivotStyleDark1:
          if ((PivotTableParts.WholeTable & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.PatternColor = Color.FromArgb(191, 191, 191);
            extendedFormatImpl.Color = Color.FromArgb(191, 191, 191);
          }
          if ((PivotTableParts.HeaderRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          if ((PivotTableParts.SubtotalRow1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
          }
          if ((PivotTableParts.RowSubHeading1 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(166, 166, 166);
            extendedFormatImpl.PatternColor = Color.FromArgb(217, 217, 217);
            extendedFormatImpl.Color = Color.FromArgb(217, 217, 217);
          }
          if ((PivotTableParts.RowSubHeading2 & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb(0, 0, 0);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Font.Bold = true;
          }
          if ((PivotTableParts.GrandTotalRow & tableParts) != (PivotTableParts) 0)
          {
            extendedFormatImpl.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            extendedFormatImpl.Font.FontName = "Calibri";
            extendedFormatImpl.Font.Size = 11.0;
            extendedFormatImpl.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            extendedFormatImpl.Font.Bold = true;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            extendedFormatImpl.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(217, 217, 217);
            break;
          }
          break;
      }
    }
    return extendedFormatImpl;
  }

  internal void DrawPivotBorder(PivotTableLayout layout, PivotBuiltInStyles? buildInStyles)
  {
    bool flag1 = true;
    bool flag2 = false;
    List<int> intList = new List<int>();
    ref PivotBuiltInStyles? local = ref buildInStyles;
    PivotBuiltInStyles valueOrDefault = local.GetValueOrDefault();
    if (local.HasValue)
    {
      switch (valueOrDefault)
      {
        case PivotBuiltInStyles.PivotStyleMedium28:
          int rowIndex1 = 0;
          int rowIndex2 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex2 <= maxRowCount; ++rowIndex2)
          {
            int colIndex = 0;
            for (int index = layout[rowIndex2].Count - 1; colIndex <= index; ++colIndex)
            {
              if ((layout[rowIndex2, colIndex].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex2 > rowIndex1)
                rowIndex1 = rowIndex2;
            }
          }
          int colIndex1 = 0;
          for (int index = layout[rowIndex1].Count - 1; colIndex1 <= index; ++colIndex1)
          {
            layout[rowIndex1, colIndex1].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex1, colIndex1].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium27:
          int rowIndex3 = 0;
          int rowIndex4 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex4 <= maxRowCount; ++rowIndex4)
          {
            int colIndex2 = 0;
            for (int index = layout[rowIndex4].Count - 1; colIndex2 <= index; ++colIndex2)
            {
              if ((layout[rowIndex4, colIndex2].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex4 > rowIndex3)
                rowIndex3 = rowIndex4;
            }
          }
          int colIndex3 = 0;
          for (int index = layout[rowIndex3].Count - 1; colIndex3 <= index; ++colIndex3)
          {
            layout[rowIndex3, colIndex3].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex3, colIndex3].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium26:
          int rowIndex5 = 0;
          int rowIndex6 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex6 <= maxRowCount; ++rowIndex6)
          {
            int colIndex4 = 0;
            for (int index = layout[rowIndex6].Count - 1; colIndex4 <= index; ++colIndex4)
            {
              if ((layout[rowIndex6, colIndex4].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex6 > rowIndex5)
                rowIndex5 = rowIndex6;
            }
          }
          int colIndex5 = 0;
          for (int index = layout[rowIndex5].Count - 1; colIndex5 <= index; ++colIndex5)
          {
            layout[rowIndex5, colIndex5].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex5, colIndex5].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium25:
          int rowIndex7 = 0;
          int rowIndex8 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex8 <= maxRowCount; ++rowIndex8)
          {
            int colIndex6 = 0;
            for (int index = layout[rowIndex8].Count - 1; colIndex6 <= index; ++colIndex6)
            {
              if ((layout[rowIndex8, colIndex6].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex8 > rowIndex7)
                rowIndex7 = rowIndex8;
            }
          }
          int colIndex7 = 0;
          for (int index = layout[rowIndex7].Count - 1; colIndex7 <= index; ++colIndex7)
          {
            layout[rowIndex7, colIndex7].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex7, colIndex7].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium24:
          int rowIndex9 = 0;
          int rowIndex10 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex10 <= maxRowCount; ++rowIndex10)
          {
            int colIndex8 = 0;
            for (int index = layout[rowIndex10].Count - 1; colIndex8 <= index; ++colIndex8)
            {
              if ((layout[rowIndex10, colIndex8].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex10 > rowIndex9)
                rowIndex9 = rowIndex10;
            }
          }
          int colIndex9 = 0;
          for (int index = layout[rowIndex9].Count - 1; colIndex9 <= index; ++colIndex9)
          {
            layout[rowIndex9, colIndex9].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex9, colIndex9].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium23:
          int rowIndex11 = 0;
          int rowIndex12 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex12 <= maxRowCount; ++rowIndex12)
          {
            int colIndex10 = 0;
            for (int index = layout[rowIndex12].Count - 1; colIndex10 <= index; ++colIndex10)
            {
              if ((layout[rowIndex12, colIndex10].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex12 > rowIndex11)
                rowIndex11 = rowIndex12;
            }
          }
          int colIndex11 = 0;
          for (int index = layout[rowIndex11].Count - 1; colIndex11 <= index; ++colIndex11)
          {
            layout[rowIndex11, colIndex11].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex11, colIndex11].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium22:
          int rowIndex13 = 0;
          int rowIndex14 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex14 <= maxRowCount; ++rowIndex14)
          {
            int colIndex12 = 0;
            for (int index = layout[rowIndex14].Count - 1; colIndex12 <= index; ++colIndex12)
            {
              if ((layout[rowIndex14, colIndex12].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex14 > rowIndex13)
                rowIndex13 = rowIndex14;
            }
          }
          int colIndex13 = 0;
          for (int index = layout[rowIndex13].Count - 1; colIndex13 <= index; ++colIndex13)
          {
            layout[rowIndex13, colIndex13].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex13, colIndex13].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium21:
          int rowIndex15 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex15 <= maxRowCount; ++rowIndex15)
          {
            int colIndex14 = 0;
            for (int index = layout[rowIndex15].Count - 1; colIndex14 <= index; ++colIndex14)
            {
              if ((layout[rowIndex15, colIndex14].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                if (flag1)
                {
                  layout[rowIndex15, colIndex14].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex15, colIndex14].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(165, 165, 165);
                  flag2 = true;
                }
                layout[rowIndex15, colIndex14].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex15, colIndex14].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex15, colIndex14].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex15, colIndex14].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium20:
          int rowIndex16 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex16 <= maxRowCount; ++rowIndex16)
          {
            int colIndex15 = 0;
            for (int index = layout[rowIndex16].Count - 1; colIndex15 <= index; ++colIndex15)
            {
              if ((layout[rowIndex16, colIndex15].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                if (flag1)
                {
                  layout[rowIndex16, colIndex15].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex16, colIndex15].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(165, 165, 165);
                  flag2 = true;
                }
                layout[rowIndex16, colIndex15].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex16, colIndex15].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex16, colIndex15].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex16, colIndex15].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium19:
          int rowIndex17 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex17 <= maxRowCount; ++rowIndex17)
          {
            int colIndex16 = 0;
            for (int index = layout[rowIndex17].Count - 1; colIndex16 <= index; ++colIndex16)
            {
              if ((layout[rowIndex17, colIndex16].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                if (flag1)
                {
                  layout[rowIndex17, colIndex16].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex17, colIndex16].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(165, 165, 165);
                  flag2 = true;
                }
                layout[rowIndex17, colIndex16].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex17, colIndex16].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex17, colIndex16].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex17, colIndex16].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium18:
          int rowIndex18 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex18 <= maxRowCount; ++rowIndex18)
          {
            int colIndex17 = 0;
            for (int index = layout[rowIndex18].Count - 1; colIndex17 <= index; ++colIndex17)
            {
              if ((layout[rowIndex18, colIndex17].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                if (flag1)
                {
                  layout[rowIndex18, colIndex17].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex18, colIndex17].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(165, 165, 165);
                  flag2 = true;
                }
                layout[rowIndex18, colIndex17].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex18, colIndex17].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex18, colIndex17].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex18, colIndex17].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium17:
          int rowIndex19 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex19 <= maxRowCount; ++rowIndex19)
          {
            int colIndex18 = 0;
            for (int index = layout[rowIndex19].Count - 1; colIndex18 <= index; ++colIndex18)
            {
              if ((layout[rowIndex19, colIndex18].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                if (flag1)
                {
                  layout[rowIndex19, colIndex18].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex19, colIndex18].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(165, 165, 165);
                  flag2 = true;
                }
                layout[rowIndex19, colIndex18].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex19, colIndex18].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex19, colIndex18].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex19, colIndex18].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium16:
          int rowIndex20 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex20 <= maxRowCount; ++rowIndex20)
          {
            int colIndex19 = 0;
            for (int index = layout[rowIndex20].Count - 1; colIndex19 <= index; ++colIndex19)
            {
              if ((layout[rowIndex20, colIndex19].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                if (flag1)
                {
                  layout[rowIndex20, colIndex19].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex20, colIndex19].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(165, 165, 165);
                  flag2 = true;
                }
                layout[rowIndex20, colIndex19].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex20, colIndex19].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex20, colIndex19].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex20, colIndex19].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium15:
          int rowIndex21 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex21 <= maxRowCount; ++rowIndex21)
          {
            int colIndex20 = 0;
            for (int index = layout[rowIndex21].Count - 1; colIndex20 <= index; ++colIndex20)
            {
              if ((layout[rowIndex21, colIndex20].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                if (flag1)
                {
                  layout[rowIndex21, colIndex20].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex21, colIndex20].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(165, 165, 165);
                  flag2 = true;
                }
                layout[rowIndex21, colIndex20].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex21, colIndex20].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex21, colIndex20].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(165, 165, 165);
                layout[rowIndex21, colIndex20].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium14:
          int rowIndex22 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex22 <= maxRowCount; ++rowIndex22)
          {
            int colIndex21 = 0;
            for (int index = layout[rowIndex22].Count - 1; colIndex21 <= index; ++colIndex21)
            {
              if ((layout[rowIndex22, colIndex21].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                intList.Add(colIndex21);
                if (flag1)
                {
                  layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(252, 213, 180);
                  flag2 = true;
                }
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(252, 213, 180);
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(252, 213, 180);
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex22 == 0)
              {
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(226, 107, 10);
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (intList.Contains(colIndex21))
              {
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(252, 213, 180);
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(252, 213, 180);
                layout[rowIndex22, colIndex21].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium13:
          int rowIndex23 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex23 <= maxRowCount; ++rowIndex23)
          {
            int colIndex22 = 0;
            for (int index = layout[rowIndex23].Count - 1; colIndex22 <= index; ++colIndex22)
            {
              if ((layout[rowIndex23, colIndex22].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                intList.Add(colIndex22);
                if (flag1)
                {
                  layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(183, 222, 232);
                  flag2 = true;
                }
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(183, 222, 232);
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(183, 222, 232);
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex23 == 0)
              {
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(49, 134, 155);
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (intList.Contains(colIndex22))
              {
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(183, 222, 232);
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(183, 222, 232);
                layout[rowIndex23, colIndex22].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium12:
          int rowIndex24 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex24 <= maxRowCount; ++rowIndex24)
          {
            int colIndex23 = 0;
            for (int index = layout[rowIndex24].Count - 1; colIndex23 <= index; ++colIndex23)
            {
              if ((layout[rowIndex24, colIndex23].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                intList.Add(colIndex23);
                if (flag1)
                {
                  layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(204, 192 /*0xC0*/, 218);
                  flag2 = true;
                }
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(204, 192 /*0xC0*/, 218);
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(204, 192 /*0xC0*/, 218);
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex24 == 0)
              {
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(96 /*0x60*/, 73, 122);
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (intList.Contains(colIndex23))
              {
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(204, 192 /*0xC0*/, 218);
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(204, 192 /*0xC0*/, 218);
                layout[rowIndex24, colIndex23].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium11:
          int rowIndex25 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex25 <= maxRowCount; ++rowIndex25)
          {
            int colIndex24 = 0;
            for (int index = layout[rowIndex25].Count - 1; colIndex24 <= index; ++colIndex24)
            {
              if ((layout[rowIndex25, colIndex24].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                intList.Add(colIndex24);
                if (flag1)
                {
                  layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(216, 228, 188);
                  flag2 = true;
                }
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(216, 228, 188);
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(216, 228, 188);
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex25 == 0)
              {
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(118, 147, 60);
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (intList.Contains(colIndex24))
              {
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(216, 228, 188);
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(216, 228, 188);
                layout[rowIndex25, colIndex24].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium10:
          int rowIndex26 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex26 <= maxRowCount; ++rowIndex26)
          {
            int colIndex25 = 0;
            for (int index = layout[rowIndex26].Count - 1; colIndex25 <= index; ++colIndex25)
            {
              if ((layout[rowIndex26, colIndex25].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                intList.Add(colIndex25);
                if (flag1)
                {
                  layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(230, 184, 183);
                  flag2 = true;
                }
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(230, 184, 183);
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(230, 184, 183);
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex26 == 0)
              {
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(150, 54, 52);
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (intList.Contains(colIndex25))
              {
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(230, 184, 183);
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(230, 184, 183);
                layout[rowIndex26, colIndex25].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium9:
          int rowIndex27 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex27 <= maxRowCount; ++rowIndex27)
          {
            int colIndex26 = 0;
            for (int index = layout[rowIndex27].Count - 1; colIndex26 <= index; ++colIndex26)
            {
              if ((layout[rowIndex27, colIndex26].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                intList.Add(colIndex26);
                if (flag1)
                {
                  layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(184, 204, 228);
                  flag2 = true;
                }
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(184, 204, 228);
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(184, 204, 228);
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex27 == 0)
              {
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(54, 96 /*0x60*/, 146);
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (intList.Contains(colIndex26))
              {
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(184, 204, 228);
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(184, 204, 228);
                layout[rowIndex27, colIndex26].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium8:
          int rowIndex28 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex28 <= maxRowCount; ++rowIndex28)
          {
            int colIndex27 = 0;
            for (int index = layout[rowIndex28].Count - 1; colIndex27 <= index; ++colIndex27)
            {
              if ((layout[rowIndex28, colIndex27].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
              {
                intList.Add(colIndex27);
                if (flag1)
                {
                  layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                  layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(191, 191, 191);
                  flag2 = true;
                }
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(191, 191, 191);
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(191, 191, 191);
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex28 == 0)
              {
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (intList.Contains(colIndex27))
              {
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(191, 191, 191);
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(191, 191, 191);
                layout[rowIndex28, colIndex27].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
            }
            if (flag2)
              flag1 = false;
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium7:
          int colIndex28 = 0;
          for (int maxColumnCount = layout.maxColumnCount; colIndex28 <= maxColumnCount; ++colIndex28)
          {
            int rowIndex29 = 0;
            for (int maxRowCount = layout.maxRowCount; rowIndex29 <= maxRowCount; ++rowIndex29)
            {
              if (colIndex28 <= layout[rowIndex29].Count - 1 && (layout[rowIndex29, colIndex28].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex29, colIndex28].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex29, colIndex28].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0 && colIndex28 != layout.maxColumnCount)
              {
                layout[rowIndex29, colIndex28].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(252, 213, 180);
                layout[rowIndex29, colIndex28].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium6:
          int colIndex29 = 0;
          for (int maxColumnCount = layout.maxColumnCount; colIndex29 <= maxColumnCount; ++colIndex29)
          {
            int rowIndex30 = 0;
            for (int maxRowCount = layout.maxRowCount; rowIndex30 <= maxRowCount; ++rowIndex30)
            {
              if (colIndex29 <= layout[rowIndex30].Count - 1 && (layout[rowIndex30, colIndex29].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex30, colIndex29].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex30, colIndex29].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0 && colIndex29 != layout.maxColumnCount)
              {
                layout[rowIndex30, colIndex29].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(183, 222, 232);
                layout[rowIndex30, colIndex29].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium5:
          int colIndex30 = 0;
          for (int maxColumnCount = layout.maxColumnCount; colIndex30 <= maxColumnCount; ++colIndex30)
          {
            int rowIndex31 = 0;
            for (int maxRowCount = layout.maxRowCount; rowIndex31 <= maxRowCount; ++rowIndex31)
            {
              if (colIndex30 <= layout[rowIndex31].Count - 1 && (layout[rowIndex31, colIndex30].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex31, colIndex30].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex31, colIndex30].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0 && colIndex30 != layout.maxColumnCount)
              {
                layout[rowIndex31, colIndex30].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(204, 192 /*0xC0*/, 218);
                layout[rowIndex31, colIndex30].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium4:
          int colIndex31 = 0;
          for (int maxColumnCount = layout.maxColumnCount; colIndex31 <= maxColumnCount; ++colIndex31)
          {
            int rowIndex32 = 0;
            for (int maxRowCount = layout.maxRowCount; rowIndex32 <= maxRowCount; ++rowIndex32)
            {
              if (colIndex31 <= layout[rowIndex32].Count - 1 && (layout[rowIndex32, colIndex31].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex32, colIndex31].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex32, colIndex31].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0 && colIndex31 != layout.maxColumnCount)
              {
                layout[rowIndex32, colIndex31].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(216, 228, 188);
                layout[rowIndex32, colIndex31].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium3:
          int colIndex32 = 0;
          for (int maxColumnCount = layout.maxColumnCount; colIndex32 <= maxColumnCount; ++colIndex32)
          {
            int rowIndex33 = 0;
            for (int maxRowCount = layout.maxRowCount; rowIndex33 <= maxRowCount; ++rowIndex33)
            {
              if (colIndex32 <= layout[rowIndex33].Count - 1 && (layout[rowIndex33, colIndex32].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex33, colIndex32].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex33, colIndex32].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0 && colIndex32 != layout.maxColumnCount)
              {
                layout[rowIndex33, colIndex32].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(230, 184, 183);
                layout[rowIndex33, colIndex32].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium2:
          int colIndex33 = 0;
          for (int maxColumnCount = layout.maxColumnCount; colIndex33 <= maxColumnCount; ++colIndex33)
          {
            int rowIndex34 = 0;
            for (int maxRowCount = layout.maxRowCount; rowIndex34 <= maxRowCount; ++rowIndex34)
            {
              if (colIndex33 <= layout[rowIndex34].Count - 1 && (layout[rowIndex34, colIndex33].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex34, colIndex33].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex34, colIndex33].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0 && colIndex33 != layout.maxColumnCount)
              {
                layout[rowIndex34, colIndex33].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(184, 204, 228);
                layout[rowIndex34, colIndex33].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          return;
        case PivotBuiltInStyles.PivotStyleMedium1:
          int colIndex34 = 0;
          for (int maxColumnCount = layout.maxColumnCount; colIndex34 <= maxColumnCount; ++colIndex34)
          {
            int rowIndex35 = 0;
            for (int maxRowCount = layout.maxRowCount; rowIndex35 <= maxRowCount; ++rowIndex35)
            {
              if (colIndex34 <= layout[rowIndex35].Count - 1 && (layout[rowIndex35, colIndex34].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex35, colIndex34].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex35, colIndex34].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0 && colIndex34 != layout.maxColumnCount)
              {
                layout[rowIndex35, colIndex34].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
                layout[rowIndex35, colIndex34].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight28:
          int rowIndex36 = 0;
          int rowIndex37 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex37 <= maxRowCount; ++rowIndex37)
          {
            int colIndex35 = 0;
            for (int index = layout[rowIndex37].Count - 1; colIndex35 <= index; ++colIndex35)
            {
              if ((layout[rowIndex37, colIndex35].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex37 > rowIndex36)
                rowIndex36 = rowIndex37;
              if (colIndex35 == 0)
              {
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(247, 150, 70);
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (colIndex35 == layout.maxColumnCount)
              {
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(247, 150, 70);
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex37 == 0)
              {
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex37 == layout.maxRowCount)
              {
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
                layout[rowIndex37, colIndex35].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          int colIndex36 = 0;
          for (int index = layout[rowIndex36].Count - 1; colIndex36 <= index; ++colIndex36)
          {
            layout[rowIndex36, colIndex36].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
            layout[rowIndex36, colIndex36].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight27:
          int rowIndex38 = 0;
          int rowIndex39 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex39 <= maxRowCount; ++rowIndex39)
          {
            int colIndex37 = 0;
            for (int index = layout[rowIndex39].Count - 1; colIndex37 <= index; ++colIndex37)
            {
              if ((layout[rowIndex39, colIndex37].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex39 > rowIndex38)
                rowIndex38 = rowIndex39;
              if (colIndex37 == 0)
              {
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(75, 172, 198);
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (colIndex37 == layout.maxColumnCount)
              {
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(75, 172, 198);
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex39 == 0)
              {
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex39 == layout.maxRowCount)
              {
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
                layout[rowIndex39, colIndex37].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          int colIndex38 = 0;
          for (int index = layout[rowIndex38].Count - 1; colIndex38 <= index; ++colIndex38)
          {
            layout[rowIndex38, colIndex38].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
            layout[rowIndex38, colIndex38].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight26:
          int rowIndex40 = 0;
          int rowIndex41 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex41 <= maxRowCount; ++rowIndex41)
          {
            int colIndex39 = 0;
            for (int index = layout[rowIndex41].Count - 1; colIndex39 <= index; ++colIndex39)
            {
              if ((layout[rowIndex41, colIndex39].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex41 > rowIndex40)
                rowIndex40 = rowIndex41;
              if (colIndex39 == 0)
              {
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (colIndex39 == layout.maxColumnCount)
              {
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex41 == 0)
              {
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex41 == layout.maxRowCount)
              {
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
                layout[rowIndex41, colIndex39].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          int colIndex40 = 0;
          for (int index = layout[rowIndex40].Count - 1; colIndex40 <= index; ++colIndex40)
          {
            layout[rowIndex40, colIndex40].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            layout[rowIndex40, colIndex40].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight25:
          int rowIndex42 = 0;
          int rowIndex43 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex43 <= maxRowCount; ++rowIndex43)
          {
            int colIndex41 = 0;
            for (int index = layout[rowIndex43].Count - 1; colIndex41 <= index; ++colIndex41)
            {
              if ((layout[rowIndex43, colIndex41].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex43 > rowIndex42)
                rowIndex42 = rowIndex43;
              if (colIndex41 == 0)
              {
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(155, 187, 89);
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (colIndex41 == layout.maxColumnCount)
              {
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(155, 187, 89);
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex43 == 0)
              {
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex43 == layout.maxRowCount)
              {
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
                layout[rowIndex43, colIndex41].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          int colIndex42 = 0;
          for (int index = layout[rowIndex42].Count - 1; colIndex42 <= index; ++colIndex42)
          {
            layout[rowIndex42, colIndex42].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
            layout[rowIndex42, colIndex42].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight24:
          int rowIndex44 = 0;
          int rowIndex45 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex45 <= maxRowCount; ++rowIndex45)
          {
            int colIndex43 = 0;
            for (int index = layout[rowIndex45].Count - 1; colIndex43 <= index; ++colIndex43)
            {
              if ((layout[rowIndex45, colIndex43].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex45 > rowIndex44)
                rowIndex44 = rowIndex45;
              if (colIndex43 == 0)
              {
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (colIndex43 == layout.maxColumnCount)
              {
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex45 == 0)
              {
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex45 == layout.maxRowCount)
              {
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
                layout[rowIndex45, colIndex43].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          int colIndex44 = 0;
          for (int index = layout[rowIndex44].Count - 1; colIndex44 <= index; ++colIndex44)
          {
            layout[rowIndex44, colIndex44].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            layout[rowIndex44, colIndex44].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight23:
          int rowIndex46 = 0;
          int rowIndex47 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex47 <= maxRowCount; ++rowIndex47)
          {
            int colIndex45 = 0;
            for (int index = layout[rowIndex47].Count - 1; colIndex45 <= index; ++colIndex45)
            {
              if ((layout[rowIndex47, colIndex45].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex47 > rowIndex46)
                rowIndex46 = rowIndex47;
              if (colIndex45 == 0)
              {
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(79, 129, 189);
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (colIndex45 == layout.maxColumnCount)
              {
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(79, 129, 189);
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex47 == 0)
              {
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex47 == layout.maxRowCount)
              {
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
                layout[rowIndex47, colIndex45].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          int colIndex46 = 0;
          for (int index = layout[rowIndex46].Count - 1; colIndex46 <= index; ++colIndex46)
          {
            layout[rowIndex46, colIndex46].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
            layout[rowIndex46, colIndex46].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight22:
          int rowIndex48 = 0;
          int rowIndex49 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex49 <= maxRowCount; ++rowIndex49)
          {
            int colIndex47 = 0;
            for (int index = layout[rowIndex49].Count - 1; colIndex47 <= index; ++colIndex47)
            {
              if ((layout[rowIndex49, colIndex47].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex49 > rowIndex48)
                rowIndex48 = rowIndex49;
              if (colIndex47 == 0)
              {
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
              }
              if (colIndex47 == layout.maxColumnCount)
              {
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex49 == 0)
              {
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
              }
              if (rowIndex49 == layout.maxRowCount)
              {
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex49, colIndex47].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          int colIndex48 = 0;
          for (int index = layout[rowIndex48].Count - 1; colIndex48 <= index; ++colIndex48)
          {
            layout[rowIndex48, colIndex48].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            layout[rowIndex48, colIndex48].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight21:
          int rowIndex50 = 0;
          int rowIndex51 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex51 <= maxRowCount; ++rowIndex51)
          {
            int colIndex49 = 0;
            for (int index = layout[rowIndex51].Count - 1; colIndex49 <= index; ++colIndex49)
            {
              if ((layout[rowIndex51, colIndex49].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex51 > rowIndex50)
                rowIndex50 = rowIndex51;
            }
          }
          int colIndex50 = 0;
          for (int index = layout[rowIndex50].Count - 1; colIndex50 <= index; ++colIndex50)
          {
            layout[rowIndex50, colIndex50].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex50, colIndex50].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight20:
          int rowIndex52 = 0;
          int rowIndex53 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex53 <= maxRowCount; ++rowIndex53)
          {
            int colIndex51 = 0;
            for (int index = layout[rowIndex53].Count - 1; colIndex51 <= index; ++colIndex51)
            {
              if ((layout[rowIndex53, colIndex51].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex53 > rowIndex52)
                rowIndex52 = rowIndex53;
            }
          }
          int colIndex52 = 0;
          for (int index = layout[rowIndex52].Count - 1; colIndex52 <= index; ++colIndex52)
          {
            layout[rowIndex52, colIndex52].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex52, colIndex52].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(146, 205, 220);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight19:
          int rowIndex54 = 0;
          int rowIndex55 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex55 <= maxRowCount; ++rowIndex55)
          {
            int colIndex53 = 0;
            for (int index = layout[rowIndex55].Count - 1; colIndex53 <= index; ++colIndex53)
            {
              if ((layout[rowIndex55, colIndex53].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex55 > rowIndex54)
                rowIndex54 = rowIndex55;
            }
          }
          int colIndex54 = 0;
          for (int index = layout[rowIndex54].Count - 1; colIndex54 <= index; ++colIndex54)
          {
            layout[rowIndex54, colIndex54].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex54, colIndex54].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(177, 160 /*0xA0*/, 199);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight18:
          int rowIndex56 = 0;
          int rowIndex57 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex57 <= maxRowCount; ++rowIndex57)
          {
            int colIndex55 = 0;
            for (int index = layout[rowIndex57].Count - 1; colIndex55 <= index; ++colIndex55)
            {
              if ((layout[rowIndex57, colIndex55].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex57 > rowIndex56)
                rowIndex56 = rowIndex57;
            }
          }
          int colIndex56 = 0;
          for (int index = layout[rowIndex56].Count - 1; colIndex56 <= index; ++colIndex56)
          {
            layout[rowIndex56, colIndex56].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex56, colIndex56].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight17:
          int rowIndex58 = 0;
          int rowIndex59 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex59 <= maxRowCount; ++rowIndex59)
          {
            int colIndex57 = 0;
            for (int index = layout[rowIndex59].Count - 1; colIndex57 <= index; ++colIndex57)
            {
              if ((layout[rowIndex59, colIndex57].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex59 > rowIndex58)
                rowIndex58 = rowIndex59;
            }
          }
          int colIndex58 = 0;
          for (int index = layout[rowIndex58].Count - 1; colIndex58 <= index; ++colIndex58)
          {
            layout[rowIndex58, colIndex58].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex58, colIndex58].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight16:
          int rowIndex60 = 0;
          int rowIndex61 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex61 <= maxRowCount; ++rowIndex61)
          {
            int colIndex59 = 0;
            for (int index = layout[rowIndex61].Count - 1; colIndex59 <= index; ++colIndex59)
            {
              if ((layout[rowIndex61, colIndex59].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex61 > rowIndex60)
                rowIndex60 = rowIndex61;
            }
          }
          int colIndex60 = 0;
          for (int index = layout[rowIndex60].Count - 1; colIndex60 <= index; ++colIndex60)
          {
            layout[rowIndex60, colIndex60].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex60, colIndex60].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight15:
          int rowIndex62 = 0;
          int rowIndex63 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex63 <= maxRowCount; ++rowIndex63)
          {
            int colIndex61 = 0;
            for (int index = layout[rowIndex63].Count - 1; colIndex61 <= index; ++colIndex61)
            {
              if ((layout[rowIndex63, colIndex61].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex63 > rowIndex62)
                rowIndex62 = rowIndex63;
            }
          }
          for (int colIndex62 = 0; colIndex62 <= layout.maxColumnCount; ++colIndex62)
          {
            layout[rowIndex62, colIndex62].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex62, colIndex62].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight14:
          int rowIndex64 = 0;
          int rowIndex65 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex65 <= maxRowCount; ++rowIndex65)
          {
            int colIndex63 = 0;
            for (int index = layout[rowIndex65].Count - 1; colIndex63 <= index; ++colIndex63)
            {
              if ((layout[rowIndex65, colIndex63].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex65 > rowIndex64)
                  rowIndex64 = rowIndex65;
                layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
              }
              if ((layout[rowIndex65, colIndex63].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex63 == 0)
                {
                  layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(247, 150, 70);
                }
                if (colIndex63 == layout.maxColumnCount)
                {
                  layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(247, 150, 70);
                }
              }
              if (colIndex63 == layout.maxColumnCount)
              {
                layout[rowIndex64, colIndex63].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex64, colIndex63].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(247, 150, 70);
              }
              else if (colIndex63 == 0)
              {
                layout[rowIndex64, colIndex63].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex64, colIndex63].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(247, 150, 70);
              }
              if ((layout[rowIndex65, colIndex63].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex65 + 1, colIndex63].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) == (PivotTableParts) 0 && (layout[rowIndex65, colIndex63].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) == (PivotTableParts) 0 && (layout[rowIndex65, colIndex63].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex65, colIndex63].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0)
              {
                layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
                layout[rowIndex65, colIndex63].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          for (int colIndex64 = 0; colIndex64 <= layout.maxColumnCount; ++colIndex64)
          {
            layout[0, colIndex64].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thick;
            layout[0, colIndex64].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
            layout[rowIndex64, colIndex64].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            layout[rowIndex64, colIndex64].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight13:
          int rowIndex66 = 0;
          int rowIndex67 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex67 <= maxRowCount; ++rowIndex67)
          {
            int colIndex65 = 0;
            for (int index = layout[rowIndex67].Count - 1; colIndex65 <= index; ++colIndex65)
            {
              if ((layout[rowIndex67, colIndex65].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex67 > rowIndex66)
                  rowIndex66 = rowIndex67;
                layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
              }
              if ((layout[rowIndex67, colIndex65].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex65 == 0)
                {
                  layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(75, 172, 198);
                }
                if (colIndex65 == layout.maxColumnCount)
                {
                  layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(75, 172, 198);
                }
              }
              if (colIndex65 == layout.maxColumnCount)
              {
                layout[rowIndex66, colIndex65].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex66, colIndex65].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(75, 172, 198);
              }
              else if (colIndex65 == 0)
              {
                layout[rowIndex66, colIndex65].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex66, colIndex65].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(75, 172, 198);
              }
              if ((layout[rowIndex67, colIndex65].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex67 + 1, colIndex65].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) == (PivotTableParts) 0 && (layout[rowIndex67, colIndex65].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) == (PivotTableParts) 0 && (layout[rowIndex67, colIndex65].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex67, colIndex65].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0)
              {
                layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
                layout[rowIndex67, colIndex65].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          for (int colIndex66 = 0; colIndex66 <= layout.maxColumnCount; ++colIndex66)
          {
            layout[0, colIndex66].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thick;
            layout[0, colIndex66].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
            layout[rowIndex66, colIndex66].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            layout[rowIndex66, colIndex66].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight12:
          int rowIndex68 = 0;
          int rowIndex69 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex69 <= maxRowCount; ++rowIndex69)
          {
            int colIndex67 = 0;
            for (int index = layout[rowIndex69].Count - 1; colIndex67 <= index; ++colIndex67)
            {
              if ((layout[rowIndex69, colIndex67].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex69 > rowIndex68)
                  rowIndex68 = rowIndex69;
                layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
              }
              if ((layout[rowIndex69, colIndex67].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex67 == 0)
                {
                  layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
                }
                if (colIndex67 == layout.maxColumnCount)
                {
                  layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
                }
              }
              if (colIndex67 == layout.maxColumnCount)
              {
                layout[rowIndex68, colIndex67].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex68, colIndex67].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
              }
              else if (colIndex67 == 0)
              {
                layout[rowIndex68, colIndex67].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex68, colIndex67].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
              }
              if ((layout[rowIndex69, colIndex67].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex69 + 1, colIndex67].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) == (PivotTableParts) 0 && (layout[rowIndex69, colIndex67].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) == (PivotTableParts) 0 && (layout[rowIndex69, colIndex67].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex69, colIndex67].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0)
              {
                layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
                layout[rowIndex69, colIndex67].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          for (int colIndex68 = 0; colIndex68 <= layout.maxColumnCount; ++colIndex68)
          {
            layout[0, colIndex68].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thick;
            layout[0, colIndex68].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            layout[rowIndex68, colIndex68].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            layout[rowIndex68, colIndex68].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight11:
          int rowIndex70 = 0;
          int rowIndex71 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex71 <= maxRowCount; ++rowIndex71)
          {
            int colIndex69 = 0;
            for (int index = layout[rowIndex71].Count - 1; colIndex69 <= index; ++colIndex69)
            {
              if ((layout[rowIndex71, colIndex69].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex71 > rowIndex70)
                  rowIndex70 = rowIndex71;
                layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
              }
              if ((layout[rowIndex71, colIndex69].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex69 == 0)
                {
                  layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(155, 187, 89);
                }
                if (colIndex69 == layout.maxColumnCount)
                {
                  layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(155, 187, 89);
                }
              }
              if (colIndex69 == layout.maxColumnCount)
              {
                layout[rowIndex70, colIndex69].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex70, colIndex69].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(155, 187, 89);
              }
              else if (colIndex69 == 0)
              {
                layout[rowIndex70, colIndex69].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex70, colIndex69].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(155, 187, 89);
              }
              if ((layout[rowIndex71, colIndex69].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex71 + 1, colIndex69].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) == (PivotTableParts) 0 && (layout[rowIndex71, colIndex69].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) == (PivotTableParts) 0 && (layout[rowIndex71, colIndex69].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex71, colIndex69].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0)
              {
                layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
                layout[rowIndex71, colIndex69].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          for (int colIndex70 = 0; colIndex70 <= layout.maxColumnCount; ++colIndex70)
          {
            layout[0, colIndex70].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thick;
            layout[0, colIndex70].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
            layout[rowIndex70, colIndex70].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            layout[rowIndex70, colIndex70].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight10:
          int rowIndex72 = 0;
          int rowIndex73 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex73 <= maxRowCount; ++rowIndex73)
          {
            int colIndex71 = 0;
            for (int index = layout[rowIndex73].Count - 1; colIndex71 <= index; ++colIndex71)
            {
              if ((layout[rowIndex73, colIndex71].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex73 > rowIndex72)
                  rowIndex72 = rowIndex73;
                layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
              }
              if ((layout[rowIndex73, colIndex71].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex71 == 0)
                {
                  layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
                }
                if (colIndex71 == layout.maxColumnCount)
                {
                  layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
                }
              }
              if (colIndex71 == layout.maxColumnCount)
              {
                layout[rowIndex72, colIndex71].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex72, colIndex71].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
              }
              else if (colIndex71 == 0)
              {
                layout[rowIndex72, colIndex71].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex72, colIndex71].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
              }
              if ((layout[rowIndex73, colIndex71].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex73 + 1, colIndex71].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) == (PivotTableParts) 0 && (layout[rowIndex73, colIndex71].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) == (PivotTableParts) 0 && (layout[rowIndex73, colIndex71].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex73, colIndex71].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0)
              {
                layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
                layout[rowIndex73, colIndex71].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          for (int colIndex72 = 0; colIndex72 <= layout.maxColumnCount; ++colIndex72)
          {
            layout[0, colIndex72].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thick;
            layout[0, colIndex72].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            layout[rowIndex72, colIndex72].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            layout[rowIndex72, colIndex72].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight9:
          int rowIndex74 = 0;
          int rowIndex75 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex75 <= maxRowCount; ++rowIndex75)
          {
            int colIndex73 = 0;
            for (int index = layout[rowIndex75].Count - 1; colIndex73 <= index; ++colIndex73)
            {
              if ((layout[rowIndex75, colIndex73].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex75 > rowIndex74)
                  rowIndex74 = rowIndex75;
                layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
              }
              if ((layout[rowIndex75, colIndex73].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex73 == 0)
                {
                  layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(79, 129, 189);
                }
                if (colIndex73 == layout.maxColumnCount)
                {
                  layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(79, 129, 189);
                }
              }
              if (colIndex73 == layout.maxColumnCount)
              {
                layout[rowIndex74, colIndex73].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex74, colIndex73].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(79, 129, 189);
              }
              else if (colIndex73 == 0)
              {
                layout[rowIndex74, colIndex73].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex74, colIndex73].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(79, 129, 189);
              }
              if ((layout[rowIndex75, colIndex73].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex75 + 1, colIndex73].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) == (PivotTableParts) 0 && (layout[rowIndex75, colIndex73].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) == (PivotTableParts) 0 && (layout[rowIndex75, colIndex73].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex75, colIndex73].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0)
              {
                layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
                layout[rowIndex75, colIndex73].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          for (int colIndex74 = 0; colIndex74 <= layout.maxColumnCount; ++colIndex74)
          {
            layout[0, colIndex74].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thick;
            layout[0, colIndex74].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
            layout[rowIndex74, colIndex74].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            layout[rowIndex74, colIndex74].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight8:
          int rowIndex76 = 0;
          int rowIndex77 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex77 <= maxRowCount; ++rowIndex77)
          {
            int colIndex75 = 0;
            for (int index = layout[rowIndex77].Count - 1; colIndex75 <= index; ++colIndex75)
            {
              if ((layout[rowIndex77, colIndex75].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex77 > rowIndex76)
                  rowIndex76 = rowIndex77;
                layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if ((layout[rowIndex77, colIndex75].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex75 == 0)
                {
                  layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex75 == layout.maxColumnCount)
                {
                  layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex75 == layout.maxColumnCount)
              {
                layout[rowIndex76, colIndex75].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex76, colIndex75].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex75 == 0)
              {
                layout[rowIndex76, colIndex75].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex76, colIndex75].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if ((layout[rowIndex77, colIndex75].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) != (PivotTableParts) 0 && (layout[rowIndex77 + 1, colIndex75].PivotTablePartStyle & PivotTableParts.ColumnSubHeading1) == (PivotTableParts) 0 && (layout[rowIndex77, colIndex75].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) == (PivotTableParts) 0 && (layout[rowIndex77, colIndex75].PivotTablePartStyle & PivotTableParts.SubtotalColumn2) == (PivotTableParts) 0 && (layout[rowIndex77, colIndex75].PivotTablePartStyle & PivotTableParts.SubtotalColumn3) == (PivotTableParts) 0)
              {
                layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
                layout[rowIndex77, colIndex75].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
              }
            }
          }
          for (int colIndex76 = 0; colIndex76 <= layout.maxColumnCount; ++colIndex76)
          {
            layout[0, colIndex76].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thick;
            layout[0, colIndex76].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            layout[rowIndex76, colIndex76].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            layout[rowIndex76, colIndex76].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight7:
          int rowIndex78 = 0;
          int rowIndex79 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex79 <= maxRowCount; ++rowIndex79)
          {
            int colIndex77 = 0;
            for (int index = layout[rowIndex79].Count - 1; colIndex77 <= index; ++colIndex77)
            {
              if ((layout[rowIndex79, colIndex77].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex79 > rowIndex78)
                rowIndex78 = rowIndex79;
            }
          }
          for (int colIndex78 = 0; colIndex78 <= layout.maxColumnCount; ++colIndex78)
          {
            layout[0, colIndex78].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[0, colIndex78].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
            layout[rowIndex78, colIndex78].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex78, colIndex78].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight6:
          int rowIndex80 = 0;
          int rowIndex81 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex81 <= maxRowCount; ++rowIndex81)
          {
            int colIndex79 = 0;
            for (int index = layout[rowIndex81].Count - 1; colIndex79 <= index; ++colIndex79)
            {
              if ((layout[rowIndex81, colIndex79].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex81 > rowIndex80)
                rowIndex80 = rowIndex81;
            }
          }
          for (int colIndex80 = 0; colIndex80 <= layout.maxColumnCount; ++colIndex80)
          {
            layout[0, colIndex80].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[0, colIndex80].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
            layout[rowIndex80, colIndex80].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex80, colIndex80].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight5:
          int rowIndex82 = 0;
          int rowIndex83 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex83 <= maxRowCount; ++rowIndex83)
          {
            int colIndex81 = 0;
            for (int index = layout[rowIndex83].Count - 1; colIndex81 <= index; ++colIndex81)
            {
              if ((layout[rowIndex83, colIndex81].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex83 > rowIndex82)
                rowIndex82 = rowIndex83;
            }
          }
          for (int colIndex82 = 0; colIndex82 <= layout.maxColumnCount; ++colIndex82)
          {
            layout[0, colIndex82].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[0, colIndex82].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
            layout[rowIndex82, colIndex82].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex82, colIndex82].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight4:
          int rowIndex84 = 0;
          int rowIndex85 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex85 <= maxRowCount; ++rowIndex85)
          {
            int colIndex83 = 0;
            for (int index = layout[rowIndex85].Count - 1; colIndex83 <= index; ++colIndex83)
            {
              if ((layout[rowIndex85, colIndex83].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex85 > rowIndex84)
                rowIndex84 = rowIndex85;
            }
          }
          for (int colIndex84 = 0; colIndex84 <= layout.maxColumnCount; ++colIndex84)
          {
            layout[0, colIndex84].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[0, colIndex84].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
            layout[rowIndex84, colIndex84].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex84, colIndex84].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight3:
          int rowIndex86 = 0;
          int rowIndex87 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex87 <= maxRowCount; ++rowIndex87)
          {
            int colIndex85 = 0;
            for (int index = layout[rowIndex87].Count - 1; colIndex85 <= index; ++colIndex85)
            {
              if ((layout[rowIndex87, colIndex85].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex87 > rowIndex86)
                rowIndex86 = rowIndex87;
            }
          }
          for (int colIndex86 = 0; colIndex86 <= layout.maxColumnCount; ++colIndex86)
          {
            layout[0, colIndex86].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[0, colIndex86].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
            layout[rowIndex86, colIndex86].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex86, colIndex86].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight2:
          int rowIndex88 = 0;
          int rowIndex89 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex89 <= maxRowCount; ++rowIndex89)
          {
            int colIndex87 = 0;
            for (int index = layout[rowIndex89].Count - 1; colIndex87 <= index; ++colIndex87)
            {
              if ((layout[rowIndex89, colIndex87].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex89 > rowIndex88)
                rowIndex88 = rowIndex89;
            }
          }
          for (int colIndex88 = 0; colIndex88 <= layout.maxColumnCount; ++colIndex88)
          {
            layout[0, colIndex88].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[0, colIndex88].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
            layout[rowIndex88, colIndex88].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex88, colIndex88].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
          }
          return;
        case PivotBuiltInStyles.PivotStyleLight1:
          int rowIndex90 = 0;
          int rowIndex91 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex91 <= maxRowCount; ++rowIndex91)
          {
            int colIndex89 = 0;
            for (int index = layout[rowIndex91].Count - 1; colIndex89 <= index; ++colIndex89)
            {
              if ((layout[rowIndex91, colIndex89].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex91 > rowIndex90)
                rowIndex90 = rowIndex91;
            }
          }
          for (int colIndex90 = 0; colIndex90 <= layout.maxColumnCount; ++colIndex90)
          {
            layout[0, colIndex90].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[0, colIndex90].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            layout[rowIndex90, colIndex90].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex90, colIndex90].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark28:
          int rowIndex92 = 0;
          int rowIndex93 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex93 <= maxRowCount; ++rowIndex93)
          {
            int colIndex91 = 0;
            for (int index = layout[rowIndex93].Count - 1; colIndex91 <= index; ++colIndex91)
            {
              if ((layout[rowIndex93, colIndex91].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex93 > rowIndex92)
                rowIndex92 = rowIndex93;
            }
          }
          int colIndex92 = 0;
          for (int index = layout[rowIndex92].Count - 1; colIndex92 <= index; ++colIndex92)
          {
            layout[rowIndex92, colIndex92].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            layout[rowIndex92, colIndex92].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark27:
          int rowIndex94 = 0;
          int rowIndex95 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex95 <= maxRowCount; ++rowIndex95)
          {
            int colIndex93 = 0;
            for (int index = layout[rowIndex95].Count - 1; colIndex93 <= index; ++colIndex93)
            {
              if ((layout[rowIndex95, colIndex93].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex95 > rowIndex94)
                rowIndex94 = rowIndex95;
            }
          }
          int colIndex94 = 0;
          for (int index = layout[rowIndex94].Count - 1; colIndex94 <= index; ++colIndex94)
          {
            layout[rowIndex94, colIndex94].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            layout[rowIndex94, colIndex94].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark26:
          int rowIndex96 = 0;
          int rowIndex97 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex97 <= maxRowCount; ++rowIndex97)
          {
            int colIndex95 = 0;
            for (int index = layout[rowIndex97].Count - 1; colIndex95 <= index; ++colIndex95)
            {
              if ((layout[rowIndex97, colIndex95].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex97 > rowIndex96)
                rowIndex96 = rowIndex97;
            }
          }
          int colIndex96 = 0;
          for (int index = layout[rowIndex96].Count - 1; colIndex96 <= index; ++colIndex96)
          {
            layout[rowIndex96, colIndex96].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            layout[rowIndex96, colIndex96].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark25:
          int rowIndex98 = 0;
          int rowIndex99 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex99 <= maxRowCount; ++rowIndex99)
          {
            int colIndex97 = 0;
            for (int index = layout[rowIndex99].Count - 1; colIndex97 <= index; ++colIndex97)
            {
              if ((layout[rowIndex99, colIndex97].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex99 > rowIndex98)
                rowIndex98 = rowIndex99;
            }
          }
          int colIndex98 = 0;
          for (int index = layout[rowIndex98].Count - 1; colIndex98 <= index; ++colIndex98)
          {
            layout[rowIndex98, colIndex98].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            layout[rowIndex98, colIndex98].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark24:
          int rowIndex100 = 0;
          int rowIndex101 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex101 <= maxRowCount; ++rowIndex101)
          {
            int colIndex99 = 0;
            for (int index = layout[rowIndex101].Count - 1; colIndex99 <= index; ++colIndex99)
            {
              if ((layout[rowIndex101, colIndex99].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex101 > rowIndex100)
                rowIndex100 = rowIndex101;
            }
          }
          int colIndex100 = 0;
          for (int index = layout[rowIndex100].Count - 1; colIndex100 <= index; ++colIndex100)
          {
            layout[rowIndex100, colIndex100].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            layout[rowIndex100, colIndex100].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark23:
          int rowIndex102 = 0;
          int rowIndex103 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex103 <= maxRowCount; ++rowIndex103)
          {
            int colIndex101 = 0;
            for (int index = layout[rowIndex103].Count - 1; colIndex101 <= index; ++colIndex101)
            {
              if ((layout[rowIndex103, colIndex101].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex103 > rowIndex102)
                rowIndex102 = rowIndex103;
            }
          }
          int colIndex102 = 0;
          for (int index = layout[rowIndex102].Count - 1; colIndex102 <= index; ++colIndex102)
          {
            layout[rowIndex102, colIndex102].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            layout[rowIndex102, colIndex102].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark22:
          int rowIndex104 = 0;
          int rowIndex105 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex105 <= maxRowCount; ++rowIndex105)
          {
            int colIndex103 = 0;
            for (int index = layout[rowIndex105].Count - 1; colIndex103 <= index; ++colIndex103)
            {
              if ((layout[rowIndex105, colIndex103].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex105 > rowIndex104)
                rowIndex104 = rowIndex105;
            }
          }
          int colIndex104 = 0;
          for (int index = layout[rowIndex104].Count - 1; colIndex104 <= index; ++colIndex104)
          {
            layout[rowIndex104, colIndex104].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            layout[rowIndex104, colIndex104].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark14:
          int rowIndex106 = 0;
          int rowIndex107 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex107 <= maxRowCount; ++rowIndex107)
          {
            int colIndex105 = 0;
            for (int index = layout[rowIndex107].Count - 1; colIndex105 <= index; ++colIndex105)
            {
              if ((layout[rowIndex107, colIndex105].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex107 > rowIndex106)
                  rowIndex106 = rowIndex107;
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
              }
              if (colIndex105 == 0)
              {
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
              if (colIndex105 == layout.maxColumnCount)
              {
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex107 == 0)
              {
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex107 == layout.maxRowCount)
              {
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
              }
              if ((layout[rowIndex107, colIndex105].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex105 == 0)
                {
                  layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex105 == layout.maxColumnCount)
                {
                  layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex105 == layout.maxColumnCount)
              {
                layout[rowIndex106, colIndex105].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex106, colIndex105].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex105 == 0)
              {
                layout[rowIndex106, colIndex105].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex106, colIndex105].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if (((layout[rowIndex107, colIndex105].PivotTablePartStyle & PivotTableParts.ColumnSubHeading2) != (PivotTableParts) 0 || (layout[rowIndex107, colIndex105].PivotTablePartStyle & PivotTableParts.ColumnSubHeading3) != (PivotTableParts) 0) && layout[rowIndex107, colIndex105].Value != "")
              {
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(253, 233, 217);
                layout[rowIndex107, colIndex105].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
            }
          }
          int colIndex106 = 0;
          for (int index = layout[0].Count - 1; colIndex106 <= index; ++colIndex106)
          {
            layout[0, colIndex106].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            layout[0, colIndex106].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark13:
          int rowIndex108 = 0;
          int rowIndex109 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex109 <= maxRowCount; ++rowIndex109)
          {
            int colIndex107 = 0;
            for (int index = layout[rowIndex109].Count - 1; colIndex107 <= index; ++colIndex107)
            {
              if ((layout[rowIndex109, colIndex107].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex109 > rowIndex108)
                  rowIndex108 = rowIndex109;
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
              }
              if (colIndex107 == 0)
              {
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
              if (colIndex107 == layout.maxColumnCount)
              {
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex109 == 0)
              {
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex109 == layout.maxRowCount)
              {
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
              }
              if ((layout[rowIndex109, colIndex107].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex107 == 0)
                {
                  layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex107 == layout.maxColumnCount)
                {
                  layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex107 == layout.maxColumnCount)
              {
                layout[rowIndex108, colIndex107].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex108, colIndex107].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex107 == 0)
              {
                layout[rowIndex108, colIndex107].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex108, colIndex107].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if (((layout[rowIndex109, colIndex107].PivotTablePartStyle & PivotTableParts.ColumnSubHeading2) != (PivotTableParts) 0 || (layout[rowIndex109, colIndex107].PivotTablePartStyle & PivotTableParts.ColumnSubHeading3) != (PivotTableParts) 0) && layout[rowIndex109, colIndex107].Value != "")
              {
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 238, 243);
                layout[rowIndex109, colIndex107].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
            }
          }
          int colIndex108 = 0;
          for (int index = layout[0].Count - 1; colIndex108 <= index; ++colIndex108)
          {
            layout[0, colIndex108].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            layout[0, colIndex108].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark12:
          int rowIndex110 = 0;
          int rowIndex111 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex111 <= maxRowCount; ++rowIndex111)
          {
            int colIndex109 = 0;
            for (int index = layout[rowIndex111].Count - 1; colIndex109 <= index; ++colIndex109)
            {
              if ((layout[rowIndex111, colIndex109].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex111 > rowIndex110)
                  rowIndex110 = rowIndex111;
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
              }
              if (colIndex109 == 0)
              {
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
              if (colIndex109 == layout.maxColumnCount)
              {
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex111 == 0)
              {
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex111 == layout.maxRowCount)
              {
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
              }
              if ((layout[rowIndex111, colIndex109].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex109 == 0)
                {
                  layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex109 == layout.maxColumnCount)
                {
                  layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex109 == layout.maxColumnCount)
              {
                layout[rowIndex110, colIndex109].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex110, colIndex109].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex109 == 0)
              {
                layout[rowIndex110, colIndex109].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex110, colIndex109].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if (((layout[rowIndex111, colIndex109].PivotTablePartStyle & PivotTableParts.ColumnSubHeading2) != (PivotTableParts) 0 || (layout[rowIndex111, colIndex109].PivotTablePartStyle & PivotTableParts.ColumnSubHeading3) != (PivotTableParts) 0) && layout[rowIndex111, colIndex109].Value != "")
              {
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(228, 223, 236);
                layout[rowIndex111, colIndex109].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
            }
          }
          int colIndex110 = 0;
          for (int index = layout[0].Count - 1; colIndex110 <= index; ++colIndex110)
          {
            layout[0, colIndex110].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            layout[0, colIndex110].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark11:
          int rowIndex112 = 0;
          int rowIndex113 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex113 <= maxRowCount; ++rowIndex113)
          {
            int colIndex111 = 0;
            for (int index = layout[rowIndex113].Count - 1; colIndex111 <= index; ++colIndex111)
            {
              if ((layout[rowIndex113, colIndex111].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex113 > rowIndex112)
                  rowIndex112 = rowIndex113;
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
              }
              if (colIndex111 == 0)
              {
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
              if (colIndex111 == layout.maxColumnCount)
              {
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex113 == 0)
              {
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex113 == layout.maxRowCount)
              {
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
              }
              if ((layout[rowIndex113, colIndex111].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex111 == 0)
                {
                  layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex111 == layout.maxColumnCount)
                {
                  layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex111 == layout.maxColumnCount)
              {
                layout[rowIndex112, colIndex111].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex112, colIndex111].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex111 == 0)
              {
                layout[rowIndex112, colIndex111].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex112, colIndex111].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if (((layout[rowIndex113, colIndex111].PivotTablePartStyle & PivotTableParts.ColumnSubHeading2) != (PivotTableParts) 0 || (layout[rowIndex113, colIndex111].PivotTablePartStyle & PivotTableParts.ColumnSubHeading3) != (PivotTableParts) 0) && layout[rowIndex113, colIndex111].Value != "")
              {
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(235, 242, 222);
                layout[rowIndex113, colIndex111].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
            }
          }
          int colIndex112 = 0;
          for (int index = layout[0].Count - 1; colIndex112 <= index; ++colIndex112)
          {
            layout[0, colIndex112].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            layout[0, colIndex112].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark10:
          int rowIndex114 = 0;
          int rowIndex115 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex115 <= maxRowCount; ++rowIndex115)
          {
            int colIndex113 = 0;
            for (int index = layout[rowIndex115].Count - 1; colIndex113 <= index; ++colIndex113)
            {
              if ((layout[rowIndex115, colIndex113].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex115 > rowIndex114)
                  rowIndex114 = rowIndex115;
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
              }
              if (colIndex113 == 0)
              {
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
              if (colIndex113 == layout.maxColumnCount)
              {
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex115 == 0)
              {
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex115 == layout.maxRowCount)
              {
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
              }
              if ((layout[rowIndex115, colIndex113].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex113 == 0)
                {
                  layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex113 == layout.maxColumnCount)
                {
                  layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex113 == layout.maxColumnCount)
              {
                layout[rowIndex114, colIndex113].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex114, colIndex113].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex113 == 0)
              {
                layout[rowIndex114, colIndex113].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex114, colIndex113].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if (((layout[rowIndex115, colIndex113].PivotTablePartStyle & PivotTableParts.ColumnSubHeading2) != (PivotTableParts) 0 || (layout[rowIndex115, colIndex113].PivotTablePartStyle & PivotTableParts.ColumnSubHeading3) != (PivotTableParts) 0) && layout[rowIndex115, colIndex113].Value != "")
              {
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(242, 220, 219);
                layout[rowIndex115, colIndex113].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
            }
          }
          int colIndex114 = 0;
          for (int index = layout[0].Count - 1; colIndex114 <= index; ++colIndex114)
          {
            layout[0, colIndex114].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            layout[0, colIndex114].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark9:
          int rowIndex116 = 0;
          int rowIndex117 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex117 <= maxRowCount; ++rowIndex117)
          {
            int colIndex115 = 0;
            for (int index = layout[rowIndex117].Count - 1; colIndex115 <= index; ++colIndex115)
            {
              if ((layout[rowIndex117, colIndex115].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex117 > rowIndex116)
                  rowIndex116 = rowIndex117;
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
              }
              if (colIndex115 == 0)
              {
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
              if (colIndex115 == layout.maxColumnCount)
              {
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex117 == 0)
              {
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex117 == layout.maxRowCount)
              {
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
              }
              if ((layout[rowIndex117, colIndex115].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex115 == 0)
                {
                  layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex115 == layout.maxColumnCount)
                {
                  layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex115 == layout.maxColumnCount)
              {
                layout[rowIndex116, colIndex115].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex116, colIndex115].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex115 == 0)
              {
                layout[rowIndex116, colIndex115].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex116, colIndex115].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if (((layout[rowIndex117, colIndex115].PivotTablePartStyle & PivotTableParts.ColumnSubHeading2) != (PivotTableParts) 0 || (layout[rowIndex117, colIndex115].PivotTablePartStyle & PivotTableParts.ColumnSubHeading3) != (PivotTableParts) 0) && layout[rowIndex117, colIndex115].Value != "")
              {
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(220, 230, 241);
                layout[rowIndex117, colIndex115].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
            }
          }
          int colIndex116 = 0;
          for (int index = layout[0].Count - 1; colIndex116 <= index; ++colIndex116)
          {
            layout[0, colIndex116].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            layout[0, colIndex116].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
        case PivotBuiltInStyles.PivotStyleDark8:
          int rowIndex118 = 0;
          int rowIndex119 = 0;
          for (int maxRowCount = layout.maxRowCount; rowIndex119 <= maxRowCount; ++rowIndex119)
          {
            int colIndex117 = 0;
            for (int index = layout[rowIndex119].Count - 1; colIndex117 <= index; ++colIndex117)
            {
              if ((layout[rowIndex119, colIndex117].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
              {
                if (rowIndex119 > rowIndex118)
                  rowIndex118 = rowIndex119;
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
              }
              if (colIndex117 == 0)
              {
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
              }
              if (colIndex117 == layout.maxColumnCount)
              {
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex119 == 0)
              {
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
              if (rowIndex119 == layout.maxRowCount)
              {
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
              }
              if ((layout[rowIndex119, colIndex117].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
              {
                if (colIndex117 == 0)
                {
                  layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
                if (colIndex117 == layout.maxColumnCount)
                {
                  layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                  layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                }
              }
              if (colIndex117 == layout.maxColumnCount)
              {
                layout[rowIndex118, colIndex117].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex118, colIndex117].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              else if (colIndex117 == 0)
              {
                layout[rowIndex118, colIndex117].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
                layout[rowIndex118, colIndex117].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
              }
              if (((layout[rowIndex119, colIndex117].PivotTablePartStyle & PivotTableParts.ColumnSubHeading2) != (PivotTableParts) 0 || (layout[rowIndex119, colIndex117].PivotTablePartStyle & PivotTableParts.ColumnSubHeading3) != (PivotTableParts) 0) && layout[rowIndex119, colIndex117].Value != "")
              {
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(191, 191, 191);
                layout[rowIndex119, colIndex117].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
              }
            }
          }
          int colIndex118 = 0;
          for (int index = layout[0].Count - 1; colIndex118 <= index; ++colIndex118)
          {
            layout[0, colIndex118].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            layout[0, colIndex118].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
          return;
      }
    }
    int rowIndex120 = 0;
    int rowIndex121 = 0;
    for (int maxRowCount = layout.maxRowCount; rowIndex121 <= maxRowCount; ++rowIndex121)
    {
      int colIndex119 = 0;
      for (int index = layout[rowIndex121].Count - 1; colIndex119 <= index; ++colIndex119)
      {
        if ((layout[rowIndex121, colIndex119].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0 && rowIndex121 > rowIndex120)
          rowIndex120 = rowIndex121;
      }
    }
    int rowIndex122 = 0;
    for (int maxRowCount = layout.maxRowCount; rowIndex122 <= maxRowCount; ++rowIndex122)
    {
      int colIndex120 = 0;
      for (int index = layout[rowIndex122].Count - 1; colIndex120 <= index; ++colIndex120)
      {
        if ((layout[rowIndex122, colIndex120].PivotTablePartStyle & PivotTableParts.HeaderRow) != (PivotTableParts) 0)
        {
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        }
        if ((layout[rowIndex122, colIndex120].PivotTablePartStyle & PivotTableParts.FirstColumn) != (PivotTableParts) 0)
        {
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        }
        if (colIndex120 == index)
        {
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          if ((layout[rowIndex122, colIndex120].PivotTablePartStyle & PivotTableParts.GrandTotalColumn) != (PivotTableParts) 0)
          {
            layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
        }
        if (rowIndex122 == maxRowCount)
        {
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          if ((layout[rowIndex122, colIndex120].PivotTablePartStyle & PivotTableParts.GrandTotalRow) != (PivotTableParts) 0)
          {
            layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            layout[rowIndex122, colIndex120].XF.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          }
        }
      }
    }
    if (layout == null || layout.maxColumnCount <= 0)
      return;
    int colIndex121 = 0;
    for (int index = layout[rowIndex120].Count - 1; colIndex121 <= index; ++colIndex121)
    {
      layout[rowIndex120, colIndex121].XF.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
      layout[rowIndex120, colIndex121].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
    }
  }

  internal ExtendedFormatImpl GetPageFilterLabel(PivotBuiltInStyles? buildInStyles)
  {
    ExtendedFormatImpl pageFilterLabel = new ExtendedFormatImpl(this.wkSheet.Application, (object) this.wkSheet.Workbook);
    WorkbookImpl workbook = this.wkSheet.Workbook as WorkbookImpl;
    FontImpl font = (FontImpl) workbook.CreateFont((IFont) null, false);
    FontImpl fontImpl = (FontImpl) workbook.InnerFonts.Add((IFont) font);
    pageFilterLabel.FontIndex = fontImpl.Font.Index;
    ref PivotBuiltInStyles? local = ref buildInStyles;
    PivotBuiltInStyles valueOrDefault = local.GetValueOrDefault();
    if (local.HasValue)
    {
      switch (valueOrDefault)
      {
        case PivotBuiltInStyles.PivotStyleMedium28:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(252, 213, 180);
          pageFilterLabel.Color = Color.FromArgb(252, 213, 180);
          break;
        case PivotBuiltInStyles.PivotStyleMedium27:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(183, 222, 232);
          pageFilterLabel.Color = Color.FromArgb(183, 222, 232);
          break;
        case PivotBuiltInStyles.PivotStyleMedium26:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Color = Color.FromArgb(204, 192 /*0xC0*/, 218);
          pageFilterLabel.PatternColor = Color.FromArgb(204, 192 /*0xC0*/, 218);
          break;
        case PivotBuiltInStyles.PivotStyleMedium25:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(215, 228, 188);
          pageFilterLabel.Color = Color.FromArgb(215, 228, 188);
          break;
        case PivotBuiltInStyles.PivotStyleMedium24:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(230, 184, 183);
          pageFilterLabel.Color = Color.FromArgb(230, 184, 183);
          break;
        case PivotBuiltInStyles.PivotStyleMedium23:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(184, 204, 228);
          pageFilterLabel.Color = Color.FromArgb(184, 204, 228);
          break;
        case PivotBuiltInStyles.PivotStyleMedium22:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(191, 191, 191);
          pageFilterLabel.Color = Color.FromArgb(191, 191, 191);
          break;
        case PivotBuiltInStyles.PivotStyleMedium21:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Color = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium20:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Color = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium19:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterLabel.Color = Color.FromArgb(228, 223, 236);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium18:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterLabel.Color = Color.FromArgb(235, 241, 222);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium17:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterLabel.Color = Color.FromArgb(242, 220, 219);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium16:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterLabel.Color = Color.FromArgb(220, 230, 241);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium15:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(217, 217, 217);
          pageFilterLabel.Color = Color.FromArgb(217, 217, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium14:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Color = Color.FromArgb(253, 233, 217);
          break;
        case PivotBuiltInStyles.PivotStyleMedium13:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Color = Color.FromArgb(218, 238, 243);
          break;
        case PivotBuiltInStyles.PivotStyleMedium12:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterLabel.Color = Color.FromArgb(228, 223, 236);
          break;
        case PivotBuiltInStyles.PivotStyleMedium11:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterLabel.Color = Color.FromArgb(235, 241, 222);
          break;
        case PivotBuiltInStyles.PivotStyleMedium10:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterLabel.Color = Color.FromArgb(242, 220, 219);
          break;
        case PivotBuiltInStyles.PivotStyleMedium9:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterLabel.Color = Color.FromArgb(220, 230, 241);
          break;
        case PivotBuiltInStyles.PivotStyleMedium8:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Color = Color.FromArgb(217, 217, 217);
          pageFilterLabel.PatternColor = Color.FromArgb(217, 217, 217);
          break;
        case PivotBuiltInStyles.PivotStyleMedium7:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium6:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium5:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(228, 223, 236);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium4:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(235, 241, 222);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium3:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(242, 220, 219);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium2:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(220, 230, 241);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium1:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(217, 217, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight28:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(226, 107, 10);
          break;
        case PivotBuiltInStyles.PivotStyleLight27:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
          break;
        case PivotBuiltInStyles.PivotStyleLight26:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
          break;
        case PivotBuiltInStyles.PivotStyleLight25:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(118, 147, 60);
          break;
        case PivotBuiltInStyles.PivotStyleLight24:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(150, 54, 52);
          break;
        case PivotBuiltInStyles.PivotStyleLight23:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
          break;
        case PivotBuiltInStyles.PivotStyleLight22:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(0, 0, 0);
          break;
        case PivotBuiltInStyles.PivotStyleLight21:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Color = Color.FromArgb(253, 233, 217);
          break;
        case PivotBuiltInStyles.PivotStyleLight20:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Color = Color.FromArgb(146, 205, 220);
          pageFilterLabel.PatternColor = Color.FromArgb(146, 205, 220);
          break;
        case PivotBuiltInStyles.PivotStyleLight19:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(177, 160 /*0xA0*/, 199);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Color = Color.FromArgb(228, 223, 236);
          pageFilterLabel.PatternColor = Color.FromArgb(228, 223, 236);
          break;
        case PivotBuiltInStyles.PivotStyleLight18:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Color = Color.FromArgb(235, 241, 222);
          pageFilterLabel.PatternColor = Color.FromArgb(235, 241, 222);
          break;
        case PivotBuiltInStyles.PivotStyleLight17:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Color = Color.FromArgb(242, 220, 219);
          pageFilterLabel.PatternColor = Color.FromArgb(242, 220, 219);
          break;
        case PivotBuiltInStyles.PivotStyleLight16:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Color = Color.FromArgb(220, 230, 241);
          pageFilterLabel.PatternColor = Color.FromArgb(220, 230, 241);
          break;
        case PivotBuiltInStyles.PivotStyleLight15:
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Color = Color.FromArgb(217, 217, 217);
          pageFilterLabel.PatternColor = Color.FromArgb(217, 217, 217);
          break;
        case PivotBuiltInStyles.PivotStyleLight14:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(226, 107, 10);
          break;
        case PivotBuiltInStyles.PivotStyleLight13:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(49, 134, 155);
          break;
        case PivotBuiltInStyles.PivotStyleLight12:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
          break;
        case PivotBuiltInStyles.PivotStyleLight11:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(118, 147, 60);
          break;
        case PivotBuiltInStyles.PivotStyleLight10:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(150, 54, 52);
          break;
        case PivotBuiltInStyles.PivotStyleLight9:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
          break;
        case PivotBuiltInStyles.PivotStyleLight8:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          break;
        case PivotBuiltInStyles.PivotStyleLight7:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight6:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight5:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight4:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight3:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight2:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight1:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark28:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.Color = Color.FromArgb(226, 107, 10);
          pageFilterLabel.PatternColor = Color.FromArgb(226, 107, 10);
          pageFilterLabel.Color = Color.FromArgb(226, 107, 10);
          break;
        case PivotBuiltInStyles.PivotStyleDark27:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(49, 134, 155);
          pageFilterLabel.Color = Color.FromArgb(49, 134, 155);
          break;
        case PivotBuiltInStyles.PivotStyleDark26:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(96 /*0x60*/, 73, 122);
          pageFilterLabel.Color = Color.FromArgb(96 /*0x60*/, 73, 122);
          break;
        case PivotBuiltInStyles.PivotStyleDark25:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(118, 147, 60);
          pageFilterLabel.Color = Color.FromArgb(118, 147, 60);
          break;
        case PivotBuiltInStyles.PivotStyleDark24:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(150, 54, 52);
          pageFilterLabel.Color = Color.FromArgb(150, 54, 52);
          break;
        case PivotBuiltInStyles.PivotStyleDark23:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(54, 96 /*0x60*/, 146);
          pageFilterLabel.Color = Color.FromArgb(54, 96 /*0x60*/, 146);
          break;
        case PivotBuiltInStyles.PivotStyleDark22:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          break;
        case PivotBuiltInStyles.PivotStyleDark21:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(247, 150, 70);
          pageFilterLabel.Color = Color.FromArgb(247, 150, 70);
          pageFilterLabel.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark20:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(75, 172, 198);
          pageFilterLabel.Color = Color.FromArgb(75, 172, 198);
          pageFilterLabel.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark19:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterLabel.Color = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterLabel.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark18:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(155, 187, 89);
          pageFilterLabel.Color = Color.FromArgb(155, 187, 89);
          pageFilterLabel.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark17:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterLabel.Color = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterLabel.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark16:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(79, 129, 189);
          pageFilterLabel.Color = Color.FromArgb(79, 129, 189);
          pageFilterLabel.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark15:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(140, 140, 140);
          pageFilterLabel.Color = Color.FromArgb(140, 140, 140);
          pageFilterLabel.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark14:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Color = Color.FromArgb(253, 233, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark13:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Color = Color.FromArgb(218, 238, 243);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark12:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterLabel.Color = Color.FromArgb(228, 223, 236);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark11:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterLabel.Color = Color.FromArgb(235, 241, 222);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark10:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterLabel.Color = Color.FromArgb(242, 220, 219);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark9:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterLabel.Color = Color.FromArgb(220, 230, 241);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark8:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(217, 217, 217);
          pageFilterLabel.Color = Color.FromArgb(217, 217, 217);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark7:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(151, 71, 6);
          pageFilterLabel.Color = Color.FromArgb(151, 71, 6);
          break;
        case PivotBuiltInStyles.PivotStyleDark6:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(33, 89, 103);
          pageFilterLabel.Color = Color.FromArgb(33, 89, 103);
          break;
        case PivotBuiltInStyles.PivotStyleDark5:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(64 /*0x40*/, 49, 81);
          pageFilterLabel.Color = Color.FromArgb(64 /*0x40*/, 49, 81);
          break;
        case PivotBuiltInStyles.PivotStyleDark4:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(79, 98, 40);
          pageFilterLabel.Color = Color.FromArgb(79, 98, 40);
          break;
        case PivotBuiltInStyles.PivotStyleDark3:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(99, 37, 35);
          pageFilterLabel.Color = Color.FromArgb(99, 37, 35);
          break;
        case PivotBuiltInStyles.PivotStyleDark2:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(36, 64 /*0x40*/, 98);
          pageFilterLabel.Color = Color.FromArgb(36, 64 /*0x40*/, 98);
          break;
        case PivotBuiltInStyles.PivotStyleDark1:
          pageFilterLabel.Font.FontName = "Calibri";
          pageFilterLabel.Font.Size = 11.0;
          pageFilterLabel.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterLabel.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          break;
      }
    }
    return pageFilterLabel;
  }

  internal ExtendedFormatImpl GetPageFilterValue(PivotBuiltInStyles? buildInStyles)
  {
    ExtendedFormatImpl pageFilterValue = new ExtendedFormatImpl(this.wkSheet.Application, (object) this.wkSheet.Workbook);
    WorkbookImpl workbook = this.wkSheet.Workbook as WorkbookImpl;
    FontImpl font = (FontImpl) workbook.CreateFont((IFont) null, false);
    FontImpl fontImpl = (FontImpl) workbook.InnerFonts.Add((IFont) font);
    pageFilterValue.FontIndex = fontImpl.Font.Index;
    ref PivotBuiltInStyles? local = ref buildInStyles;
    PivotBuiltInStyles valueOrDefault = local.GetValueOrDefault();
    if (local.HasValue)
    {
      switch (valueOrDefault)
      {
        case PivotBuiltInStyles.PivotStyleMedium28:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterValue.Color = Color.FromArgb(253, 233, 217);
          break;
        case PivotBuiltInStyles.PivotStyleMedium27:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(218, 238, 243);
          pageFilterValue.Color = Color.FromArgb(218, 238, 243);
          break;
        case PivotBuiltInStyles.PivotStyleMedium26:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterValue.Color = Color.FromArgb(228, 223, 236);
          break;
        case PivotBuiltInStyles.PivotStyleMedium25:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterValue.Color = Color.FromArgb(235, 241, 222);
          break;
        case PivotBuiltInStyles.PivotStyleMedium24:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterValue.Color = Color.FromArgb(242, 220, 219);
          break;
        case PivotBuiltInStyles.PivotStyleMedium23:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterValue.Color = Color.FromArgb(220, 230, 241);
          break;
        case PivotBuiltInStyles.PivotStyleMedium22:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(217, 217, 217);
          pageFilterValue.Color = Color.FromArgb(217, 217, 217);
          break;
        case PivotBuiltInStyles.PivotStyleMedium21:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterValue.Color = Color.FromArgb(253, 233, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium20:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(218, 238, 243);
          pageFilterValue.Color = Color.FromArgb(218, 238, 243);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium19:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterValue.Color = Color.FromArgb(228, 223, 236);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium18:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterValue.Color = Color.FromArgb(235, 241, 222);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium17:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterValue.Color = Color.FromArgb(242, 220, 219);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium16:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterValue.Color = Color.FromArgb(220, 230, 241);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium15:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(217, 217, 217);
          pageFilterValue.Color = Color.FromArgb(217, 217, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(0, 0, 0);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium14:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterValue.Color = Color.FromArgb(253, 233, 217);
          break;
        case PivotBuiltInStyles.PivotStyleMedium13:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(218, 238, 243);
          pageFilterValue.Color = Color.FromArgb(218, 238, 243);
          break;
        case PivotBuiltInStyles.PivotStyleMedium12:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterValue.Color = Color.FromArgb(228, 223, 236);
          break;
        case PivotBuiltInStyles.PivotStyleMedium11:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterValue.Color = Color.FromArgb(235, 241, 222);
          break;
        case PivotBuiltInStyles.PivotStyleMedium10:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterValue.Color = Color.FromArgb(242, 220, 219);
          break;
        case PivotBuiltInStyles.PivotStyleMedium9:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterValue.Color = Color.FromArgb(220, 230, 241);
          break;
        case PivotBuiltInStyles.PivotStyleMedium8:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(217, 217, 217);
          pageFilterValue.Color = Color.FromArgb(217, 217, 217);
          break;
        case PivotBuiltInStyles.PivotStyleMedium7:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(253, 233, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(253, 233, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium6:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(218, 238, 243);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium5:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(228, 223, 236);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(228, 223, 236);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium4:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(235, 241, 222);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(235, 241, 222);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium3:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(242, 220, 219);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(242, 220, 219);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium2:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(220, 230, 241);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(220, 230, 241);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleMedium1:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(217, 217, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(217, 217, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight28:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(226, 107, 10);
          break;
        case PivotBuiltInStyles.PivotStyleLight27:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
          break;
        case PivotBuiltInStyles.PivotStyleLight26:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
          break;
        case PivotBuiltInStyles.PivotStyleLight25:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(118, 147, 60);
          break;
        case PivotBuiltInStyles.PivotStyleLight24:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(150, 54, 52);
          break;
        case PivotBuiltInStyles.PivotStyleLight23:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
          break;
        case PivotBuiltInStyles.PivotStyleLight22:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          break;
        case PivotBuiltInStyles.PivotStyleLight21:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(250, 191, 143);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterValue.Color = Color.FromArgb(253, 233, 217);
          break;
        case PivotBuiltInStyles.PivotStyleLight20:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 238, 243);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.PatternColor = Color.FromArgb(146, 205, 220);
          pageFilterValue.Color = Color.FromArgb(146, 205, 220);
          break;
        case PivotBuiltInStyles.PivotStyleLight19:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(177, 160 /*0xA0*/, 199);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterValue.Color = Color.FromArgb(228, 223, 236);
          break;
        case PivotBuiltInStyles.PivotStyleLight18:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(196, 215, 155);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterValue.Color = Color.FromArgb(235, 241, 222);
          break;
        case PivotBuiltInStyles.PivotStyleLight17:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(218, 150, 148);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterValue.Color = Color.FromArgb(242, 220, 219);
          break;
        case PivotBuiltInStyles.PivotStyleLight16:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(149, 179, 215);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterValue.Color = Color.FromArgb(220, 230, 241);
          break;
        case PivotBuiltInStyles.PivotStyleLight15:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(166, 166, 166);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.PatternColor = Color.FromArgb(217, 217, 217);
          pageFilterValue.Color = Color.FromArgb(217, 217, 217);
          break;
        case PivotBuiltInStyles.PivotStyleLight14:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(226, 107, 10);
          break;
        case PivotBuiltInStyles.PivotStyleLight13:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(49, 134, 155);
          break;
        case PivotBuiltInStyles.PivotStyleLight12:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(96 /*0x60*/, 73, 122);
          break;
        case PivotBuiltInStyles.PivotStyleLight11:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(118, 147, 60);
          break;
        case PivotBuiltInStyles.PivotStyleLight10:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(150, 54, 52);
          break;
        case PivotBuiltInStyles.PivotStyleLight9:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb(54, 96 /*0x60*/, 146);
          break;
        case PivotBuiltInStyles.PivotStyleLight8:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          break;
        case PivotBuiltInStyles.PivotStyleLight7:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(247, 150, 70);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(247, 150, 70);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight6:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(75, 172, 198);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(75, 172, 198);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight5:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight4:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(155, 187, 89);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(155, 187, 89);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight3:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight2:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(79, 129, 189);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(79, 129, 189);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleLight1:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark28:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(247, 150, 70);
          pageFilterValue.Color = Color.FromArgb(247, 150, 70);
          break;
        case PivotBuiltInStyles.PivotStyleDark27:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(75, 172, 198);
          pageFilterValue.Color = Color.FromArgb(75, 172, 198);
          break;
        case PivotBuiltInStyles.PivotStyleDark26:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterValue.Color = Color.FromArgb(128 /*0x80*/, 100, 162);
          break;
        case PivotBuiltInStyles.PivotStyleDark25:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Color = Color.FromArgb(155, 187, 89);
          pageFilterValue.PatternColor = Color.FromArgb(155, 187, 89);
          break;
        case PivotBuiltInStyles.PivotStyleDark24:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Color = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterValue.PatternColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          break;
        case PivotBuiltInStyles.PivotStyleDark23:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(79, 129, 189);
          pageFilterValue.Color = Color.FromArgb(79, 129, 189);
          break;
        case PivotBuiltInStyles.PivotStyleDark22:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          break;
        case PivotBuiltInStyles.PivotStyleDark21:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(247, 150, 70);
          pageFilterValue.Color = Color.FromArgb(247, 150, 70);
          pageFilterValue.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark20:
          pageFilterValue.PatternColor = Color.FromArgb(75, 172, 198);
          pageFilterValue.Color = Color.FromArgb(75, 172, 198);
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark19:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterValue.Color = Color.FromArgb(128 /*0x80*/, 100, 162);
          pageFilterValue.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark18:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(155, 187, 89);
          pageFilterValue.Color = Color.FromArgb(155, 187, 89);
          pageFilterValue.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark17:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterValue.Color = Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77);
          pageFilterValue.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark16:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(79, 129, 189);
          pageFilterValue.Color = Color.FromArgb(79, 129, 189);
          pageFilterValue.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark15:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(140, 140, 140);
          pageFilterValue.Color = Color.FromArgb(140, 140, 140);
          pageFilterValue.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case PivotBuiltInStyles.PivotStyleDark14:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(253, 233, 217);
          pageFilterValue.Color = Color.FromArgb(253, 233, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark13:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(218, 238, 243);
          pageFilterValue.Color = Color.FromArgb(218, 238, 243);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark12:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(228, 223, 236);
          pageFilterValue.Color = Color.FromArgb(228, 223, 236);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark11:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(235, 241, 222);
          pageFilterValue.Color = Color.FromArgb(235, 241, 222);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark10:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(242, 220, 219);
          pageFilterValue.Color = Color.FromArgb(242, 220, 219);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark9:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(220, 230, 241);
          pageFilterValue.Color = Color.FromArgb(220, 230, 241);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark8:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(217, 217, 217);
          pageFilterValue.Color = Color.FromArgb(217, 217, 217);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
          break;
        case PivotBuiltInStyles.PivotStyleDark7:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(151, 71, 6);
          pageFilterValue.Color = Color.FromArgb(151, 71, 6);
          break;
        case PivotBuiltInStyles.PivotStyleDark6:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(33, 89, 103);
          pageFilterValue.Color = Color.FromArgb(33, 89, 103);
          break;
        case PivotBuiltInStyles.PivotStyleDark5:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(64 /*0x40*/, 49, 81);
          pageFilterValue.Color = Color.FromArgb(64 /*0x40*/, 49, 81);
          break;
        case PivotBuiltInStyles.PivotStyleDark4:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(79, 98, 40);
          pageFilterValue.Color = Color.FromArgb(79, 98, 40);
          break;
        case PivotBuiltInStyles.PivotStyleDark3:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(99, 37, 35);
          pageFilterValue.Color = Color.FromArgb(99, 37, 35);
          break;
        case PivotBuiltInStyles.PivotStyleDark2:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(36, 64 /*0x40*/, 98);
          pageFilterValue.Color = Color.FromArgb(36, 64 /*0x40*/, 98);
          break;
        case PivotBuiltInStyles.PivotStyleDark1:
          pageFilterValue.Font.FontName = "Calibri";
          pageFilterValue.Font.Size = 11.0;
          pageFilterValue.PatternColor = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          pageFilterValue.Color = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
          break;
      }
    }
    return pageFilterValue;
  }
}
