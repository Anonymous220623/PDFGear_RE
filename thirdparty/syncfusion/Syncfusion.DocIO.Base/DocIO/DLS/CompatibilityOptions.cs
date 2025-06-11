// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CompatibilityOptions
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class CompatibilityOptions
{
  private Dictionary<CompatibilityOption, bool> m_propertiesHash;
  private IWordDocument m_document;

  internal bool this[CompatibilityOption key]
  {
    get => this.GetValue(key);
    set => this.SetValue(key, value);
  }

  internal Dictionary<CompatibilityOption, bool> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<CompatibilityOption, bool>();
      return this.m_propertiesHash;
    }
  }

  internal CompatibilityOptions(IWordDocument document) => this.m_document = document;

  private void SetValue(CompatibilityOption key, bool value)
  {
    DOPDescriptor dop = (this.m_document as WordDocument).DOP;
    switch (key)
    {
      case CompatibilityOption.NoTabForInd:
        dop.Dop2000.Copts.Copts80.Copts60.NoTabForInd = value;
        break;
      case CompatibilityOption.NoSpaceRaiseLower:
        dop.Dop2000.Copts.Copts80.Copts60.NoSpaceRaiseLower = value;
        break;
      case CompatibilityOption.MapPrintTextColor:
        dop.Dop2000.Copts.Copts80.Copts60.MapPrintTextColor = value;
        break;
      case CompatibilityOption.WrapTrailSpaces:
        dop.Dop2000.Copts.Copts80.Copts60.WrapTrailSpaces = value;
        break;
      case CompatibilityOption.NoColumnBalance:
        dop.Dop2000.Copts.Copts80.Copts60.NoColumnBalance = value;
        break;
      case CompatibilityOption.ConvMailMergeEsc:
        dop.Dop2000.Copts.Copts80.Copts60.ConvMailMergeEsc = value;
        break;
      case CompatibilityOption.SuppressSpBfAfterPgBrk:
        dop.Dop2000.Copts.Copts80.Copts60.SuppressSpBfAfterPgBrk = value;
        break;
      case CompatibilityOption.SuppressTopSpacing:
        dop.Dop2000.Copts.Copts80.Copts60.SuppressTopSpacing = value;
        break;
      case CompatibilityOption.OrigWordTableRules:
        dop.Dop2000.Copts.Copts80.Copts60.OrigWordTableRules = value;
        break;
      case CompatibilityOption.TruncDxaExpand:
        dop.Dop2000.Copts.Copts80.TruncDxaExpand = value;
        break;
      case CompatibilityOption.ShowBreaksInFrames:
        dop.Dop2000.Copts.Copts80.Copts60.ShowBreaksInFrames = value;
        break;
      case CompatibilityOption.SwapBordersFacingPgs:
        dop.Dop2000.Copts.Copts80.Copts60.SwapBordersFacingPgs = value;
        break;
      case CompatibilityOption.LeaveBackslashAlone:
        dop.Dop2000.Copts.Copts80.Copts60.LeaveBackslashAlone = value;
        break;
      case CompatibilityOption.ExpShRtn:
        dop.Dop2000.Copts.Copts80.Copts60.ExpShRtn = value;
        break;
      case CompatibilityOption.DntULTrlSpc:
        dop.Dop2000.Copts.Copts80.Copts60.DntULTrlSpc = value;
        break;
      case CompatibilityOption.DntBlnSbDbWid:
        dop.Dop2000.Copts.Copts80.Copts60.DntBlnSbDbWid = value;
        break;
      case CompatibilityOption.SuppressTopSpacingMac5:
        dop.Dop2000.Copts.Copts80.SuppressTopSpacingMac5 = value;
        break;
      case CompatibilityOption.F2ptExtLeadingOnly:
        dop.Dop2000.Copts.Copts80.F2ptExtLeadingOnly = value;
        break;
      case CompatibilityOption.PrintBodyBeforeHdr:
        dop.Dop2000.Copts.Copts80.PrintBodyBeforeHdr = value;
        break;
      case CompatibilityOption.NoExtLeading:
        dop.Dop2000.Copts.Copts80.NoExtLeading = value;
        break;
      case CompatibilityOption.DontMakeSpaceForUL:
        dop.Dop2000.Copts.Copts80.DontMakeSpaceForUL = value;
        break;
      case CompatibilityOption.MWSmallCaps:
        dop.Dop2000.Copts.Copts80.MWSmallCaps = value;
        break;
      case CompatibilityOption.ExtraAfter:
        dop.Dop2000.Copts.Copts80.ExtraAfter = value;
        break;
      case CompatibilityOption.TruncFontHeight:
        dop.Dop2000.Copts.Copts80.TruncFontHeight = value;
        break;
      case CompatibilityOption.SubOnSize:
        dop.Dop2000.Copts.Copts80.SubOnSize = value;
        break;
      case CompatibilityOption.PrintMet:
        dop.Dop2000.Copts.Copts80.PrintMet = value;
        break;
      case CompatibilityOption.WW6BorderRules:
        dop.Dop2000.Copts.Copts80.WW6BorderRules = value;
        break;
      case CompatibilityOption.ExactOnTop:
        dop.Dop2000.Copts.Copts80.ExactOnTop = value;
        break;
      case CompatibilityOption.WPSpace:
        dop.Dop2000.Copts.Copts80.WPSpace = value;
        break;
      case CompatibilityOption.WPJust:
        dop.Dop2000.Copts.Copts80.WPJust = value;
        break;
      case CompatibilityOption.LineWrapLikeWord6:
        dop.Dop2000.Copts.Copts80.LineWrapLikeWord6 = value;
        break;
      case CompatibilityOption.SpLayoutLikeWW8:
        dop.Dop2000.Copts.SpLayoutLikeWW8 = value;
        break;
      case CompatibilityOption.FtnLayoutLikeWW8:
        dop.Dop2000.Copts.FtnLayoutLikeWW8 = value;
        break;
      case CompatibilityOption.DontUseHTMLParagraphAutoSpacing:
        dop.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing = value;
        break;
      case CompatibilityOption.DontAdjustLineHeightInTable:
        dop.Dop2000.Copts.DontAdjustLineHeightInTable = value;
        break;
      case CompatibilityOption.ForgetLastTabAlign:
        dop.Dop2000.Copts.ForgetLastTabAlign = value;
        break;
      case CompatibilityOption.UseAutospaceForFullWidthAlpha:
        dop.Dop2000.Copts.UseAutospaceForFullWidthAlpha = value;
        break;
      case CompatibilityOption.AlignTablesRowByRow:
        dop.Dop2000.Copts.AlignTablesRowByRow = value;
        break;
      case CompatibilityOption.LayoutRawTableWidth:
        dop.Dop2000.Copts.LayoutRawTableWidth = value;
        break;
      case CompatibilityOption.LayoutTableRowsApart:
        dop.Dop2000.Copts.LayoutTableRowsApart = value;
        break;
      case CompatibilityOption.UseWord97LineBreakingRules:
        dop.Dop2000.Copts.UseWord97LineBreakingRules = value;
        break;
      case CompatibilityOption.DontBreakWrappedTables:
        dop.Dop2000.Copts.DontBreakWrappedTables = value;
        break;
      case CompatibilityOption.DontSnapToGridInCell:
        dop.Dop2000.Copts.DontSnapToGridInCell = value;
        break;
      case CompatibilityOption.DontAllowFieldEndSelect:
        dop.Dop2000.Copts.DontAllowFieldEndSelect = value;
        break;
      case CompatibilityOption.ApplyBreakingRules:
        dop.Dop2000.Copts.ApplyBreakingRules = value;
        break;
      case CompatibilityOption.DontWrapTextWithPunct:
        dop.Dop2000.Copts.DontWrapTextWithPunct = value;
        break;
      case CompatibilityOption.DontUseAsianBreakRules:
        dop.Dop2000.Copts.DontUseAsianBreakRules = value;
        break;
      case CompatibilityOption.UseWord2002TableStyleRules:
        dop.Dop2000.Copts.UseWord2002TableStyleRules = value;
        break;
      case CompatibilityOption.GrowAutoFit:
        dop.Dop2000.Copts.GrowAutoFit = value;
        break;
      case CompatibilityOption.UseNormalStyleForList:
        dop.Dop2000.Copts.UseNormalStyleForList = value;
        break;
      case CompatibilityOption.DontUseIndentAsNumberingTabStop:
        dop.Dop2000.Copts.DontUseIndentAsNumberingTabStop = value;
        break;
      case CompatibilityOption.FELineBreak11:
        dop.Dop2000.Copts.FELineBreak11 = value;
        break;
      case CompatibilityOption.AllowSpaceOfSameStyleInTable:
        dop.Dop2000.Copts.AllowSpaceOfSameStyleInTable = value;
        break;
      case CompatibilityOption.WW11IndentRules:
        dop.Dop2000.Copts.WW11IndentRules = value;
        break;
      case CompatibilityOption.DontAutofitConstrainedTables:
        dop.Dop2000.Copts.DontAutofitConstrainedTables = value;
        break;
      case CompatibilityOption.AutofitLikeWW11:
        dop.Dop2000.Copts.AutofitLikeWW11 = value;
        break;
      case CompatibilityOption.UnderlineTabInNumList:
        dop.Dop2000.Copts.UnderlineTabInNumList = value;
        break;
      case CompatibilityOption.HangulWidthLikeWW11:
        dop.Dop2000.Copts.HangulWidthLikeWW11 = value;
        break;
      case CompatibilityOption.SplitPgBreakAndParaMark:
        dop.Dop2000.Copts.SplitPgBreakAndParaMark = value;
        break;
      case CompatibilityOption.DontVertAlignCellWithSp:
        dop.Dop2000.Copts.DontVertAlignCellWithSp = value;
        break;
      case CompatibilityOption.DontBreakConstrainedForcedTables:
        dop.Dop2000.Copts.DontBreakConstrainedForcedTables = value;
        break;
      case CompatibilityOption.DontVertAlignInTxbx:
        dop.Dop2000.Copts.DontVertAlignInTxbx = value;
        break;
      case CompatibilityOption.Word11KerningPairs:
        dop.Dop2000.Copts.Word11KerningPairs = value;
        break;
      case CompatibilityOption.CachedColBalance:
        dop.Dop2000.Copts.CachedColBalance = value;
        break;
      default:
        if (this.PropertiesHash.ContainsKey(key))
        {
          this.PropertiesHash[key] = value;
          break;
        }
        this.PropertiesHash.Add(key, value);
        break;
    }
  }

  private bool GetValue(CompatibilityOption key)
  {
    DOPDescriptor dop = (this.m_document as WordDocument).DOP;
    switch (key)
    {
      case CompatibilityOption.NoTabForInd:
        return dop.Dop2000.Copts.Copts80.Copts60.NoTabForInd;
      case CompatibilityOption.NoSpaceRaiseLower:
        return dop.Dop2000.Copts.Copts80.Copts60.NoSpaceRaiseLower;
      case CompatibilityOption.MapPrintTextColor:
        return dop.Dop2000.Copts.Copts80.Copts60.MapPrintTextColor;
      case CompatibilityOption.WrapTrailSpaces:
        return dop.Dop2000.Copts.Copts80.Copts60.WrapTrailSpaces;
      case CompatibilityOption.NoColumnBalance:
        return dop.Dop2000.Copts.Copts80.Copts60.NoColumnBalance;
      case CompatibilityOption.ConvMailMergeEsc:
        return dop.Dop2000.Copts.Copts80.Copts60.ConvMailMergeEsc;
      case CompatibilityOption.SuppressSpBfAfterPgBrk:
        return dop.Dop2000.Copts.Copts80.Copts60.SuppressSpBfAfterPgBrk;
      case CompatibilityOption.SuppressTopSpacing:
        return dop.Dop2000.Copts.Copts80.Copts60.SuppressTopSpacing;
      case CompatibilityOption.OrigWordTableRules:
        return dop.Dop2000.Copts.Copts80.Copts60.OrigWordTableRules;
      case CompatibilityOption.TruncDxaExpand:
        return dop.Dop2000.Copts.Copts80.TruncDxaExpand;
      case CompatibilityOption.ShowBreaksInFrames:
        return dop.Dop2000.Copts.Copts80.Copts60.ShowBreaksInFrames;
      case CompatibilityOption.SwapBordersFacingPgs:
        return dop.Dop2000.Copts.Copts80.Copts60.SwapBordersFacingPgs;
      case CompatibilityOption.LeaveBackslashAlone:
        return dop.Dop2000.Copts.Copts80.Copts60.LeaveBackslashAlone;
      case CompatibilityOption.ExpShRtn:
        return dop.Dop2000.Copts.Copts80.Copts60.ExpShRtn;
      case CompatibilityOption.DntULTrlSpc:
        return dop.Dop2000.Copts.Copts80.Copts60.DntULTrlSpc;
      case CompatibilityOption.DntBlnSbDbWid:
        return dop.Dop2000.Copts.Copts80.Copts60.DntBlnSbDbWid;
      case CompatibilityOption.SuppressTopSpacingMac5:
        return dop.Dop2000.Copts.Copts80.SuppressTopSpacingMac5;
      case CompatibilityOption.F2ptExtLeadingOnly:
        return dop.Dop2000.Copts.Copts80.F2ptExtLeadingOnly;
      case CompatibilityOption.PrintBodyBeforeHdr:
        return dop.Dop2000.Copts.Copts80.PrintBodyBeforeHdr;
      case CompatibilityOption.NoExtLeading:
        return dop.Dop2000.Copts.Copts80.NoExtLeading;
      case CompatibilityOption.DontMakeSpaceForUL:
        return dop.Dop2000.Copts.Copts80.DontMakeSpaceForUL;
      case CompatibilityOption.MWSmallCaps:
        return dop.Dop2000.Copts.Copts80.MWSmallCaps;
      case CompatibilityOption.ExtraAfter:
        return dop.Dop2000.Copts.Copts80.ExtraAfter;
      case CompatibilityOption.TruncFontHeight:
        return dop.Dop2000.Copts.Copts80.TruncFontHeight;
      case CompatibilityOption.SubOnSize:
        return dop.Dop2000.Copts.Copts80.SubOnSize;
      case CompatibilityOption.PrintMet:
        return dop.Dop2000.Copts.Copts80.PrintMet;
      case CompatibilityOption.WW6BorderRules:
        return dop.Dop2000.Copts.Copts80.WW6BorderRules;
      case CompatibilityOption.ExactOnTop:
        return dop.Dop2000.Copts.Copts80.ExactOnTop;
      case CompatibilityOption.WPSpace:
        return dop.Dop2000.Copts.Copts80.WPSpace;
      case CompatibilityOption.WPJust:
        return dop.Dop2000.Copts.Copts80.WPJust;
      case CompatibilityOption.LineWrapLikeWord6:
        return dop.Dop2000.Copts.Copts80.LineWrapLikeWord6;
      case CompatibilityOption.SpLayoutLikeWW8:
        return dop.Dop2000.Copts.SpLayoutLikeWW8;
      case CompatibilityOption.FtnLayoutLikeWW8:
        return dop.Dop2000.Copts.FtnLayoutLikeWW8;
      case CompatibilityOption.DontUseHTMLParagraphAutoSpacing:
        return (this.m_document as WordDocument).WordVersion <= (ushort) 193 && (this.m_document as WordDocument).WordVersion != (ushort) 0 || dop.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing;
      case CompatibilityOption.DontAdjustLineHeightInTable:
        return dop.Dop2000.Copts.DontAdjustLineHeightInTable;
      case CompatibilityOption.ForgetLastTabAlign:
        return dop.Dop2000.Copts.ForgetLastTabAlign;
      case CompatibilityOption.UseAutospaceForFullWidthAlpha:
        return dop.Dop2000.Copts.UseAutospaceForFullWidthAlpha;
      case CompatibilityOption.AlignTablesRowByRow:
        return dop.Dop2000.Copts.AlignTablesRowByRow;
      case CompatibilityOption.LayoutRawTableWidth:
        return dop.Dop2000.Copts.LayoutRawTableWidth;
      case CompatibilityOption.LayoutTableRowsApart:
        return dop.Dop2000.Copts.LayoutTableRowsApart;
      case CompatibilityOption.UseWord97LineBreakingRules:
        return dop.Dop2000.Copts.UseWord97LineBreakingRules;
      case CompatibilityOption.DontBreakWrappedTables:
        return dop.Dop2000.Copts.DontBreakWrappedTables;
      case CompatibilityOption.DontSnapToGridInCell:
        return dop.Dop2000.Copts.DontSnapToGridInCell;
      case CompatibilityOption.DontAllowFieldEndSelect:
        return dop.Dop2000.Copts.DontAllowFieldEndSelect;
      case CompatibilityOption.ApplyBreakingRules:
        return dop.Dop2000.Copts.ApplyBreakingRules;
      case CompatibilityOption.DontWrapTextWithPunct:
        return dop.Dop2000.Copts.DontWrapTextWithPunct;
      case CompatibilityOption.DontUseAsianBreakRules:
        return dop.Dop2000.Copts.DontUseAsianBreakRules;
      case CompatibilityOption.UseWord2002TableStyleRules:
        return dop.Dop2000.Copts.UseWord2002TableStyleRules;
      case CompatibilityOption.GrowAutoFit:
        return dop.Dop2000.Copts.GrowAutoFit;
      case CompatibilityOption.UseNormalStyleForList:
        return dop.Dop2000.Copts.UseNormalStyleForList;
      case CompatibilityOption.DontUseIndentAsNumberingTabStop:
        return dop.Dop2000.Copts.DontUseIndentAsNumberingTabStop;
      case CompatibilityOption.FELineBreak11:
        return dop.Dop2000.Copts.FELineBreak11;
      case CompatibilityOption.AllowSpaceOfSameStyleInTable:
        return dop.Dop2000.Copts.AllowSpaceOfSameStyleInTable;
      case CompatibilityOption.WW11IndentRules:
        return dop.Dop2000.Copts.WW11IndentRules;
      case CompatibilityOption.DontAutofitConstrainedTables:
        return dop.Dop2000.Copts.DontAutofitConstrainedTables;
      case CompatibilityOption.AutofitLikeWW11:
        return dop.Dop2000.Copts.AutofitLikeWW11;
      case CompatibilityOption.UnderlineTabInNumList:
        return dop.Dop2000.Copts.UnderlineTabInNumList;
      case CompatibilityOption.HangulWidthLikeWW11:
        return dop.Dop2000.Copts.HangulWidthLikeWW11;
      case CompatibilityOption.SplitPgBreakAndParaMark:
        return (this.m_document as WordDocument).WordVersion <= (ushort) 268 && (this.m_document as WordDocument).WordVersion != (ushort) 0 || dop.Dop2000.Copts.SplitPgBreakAndParaMark;
      case CompatibilityOption.DontVertAlignCellWithSp:
        return dop.Dop2000.Copts.DontVertAlignCellWithSp;
      case CompatibilityOption.DontBreakConstrainedForcedTables:
        return dop.Dop2000.Copts.DontBreakConstrainedForcedTables;
      case CompatibilityOption.DontVertAlignInTxbx:
        return dop.Dop2000.Copts.DontVertAlignInTxbx;
      case CompatibilityOption.Word11KerningPairs:
        return dop.Dop2000.Copts.Word11KerningPairs;
      case CompatibilityOption.CachedColBalance:
        return dop.Dop2000.Copts.CachedColBalance;
      default:
        return this.PropertiesHash.ContainsKey(key) && this.PropertiesHash[key];
    }
  }

  internal void Close()
  {
    this.m_document = (IWordDocument) null;
    if (this.m_propertiesHash == null)
      return;
    this.m_propertiesHash.Clear();
    this.m_propertiesHash = (Dictionary<CompatibilityOption, bool>) null;
  }
}
