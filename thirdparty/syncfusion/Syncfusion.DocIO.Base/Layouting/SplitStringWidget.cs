// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.SplitStringWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class SplitStringWidget : ISplitLeafWidget, ILeafWidget, IWidget
{
  private IStringWidget m_strWidget;
  internal int m_prevWidgetIndex = -1;
  internal int StartIndex;
  internal int Length;
  private byte m_bFlags;

  public SplitStringWidget(IStringWidget strWidget, int startIndex, int length)
  {
    this.m_strWidget = strWidget;
    this.StartIndex = startIndex;
    this.Length = length;
  }

  internal bool IsTrailSpacesWrapped
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public string SplittedText
  {
    get
    {
      return this.StartIndex != int.MinValue || this.Length != int.MinValue ? (this.StartIndex != -1 || this.Length != -1 ? (this.StartIndex == -1 || this.Length != -1 ? (this.Length >= 0 ? (this.StartIndex == -1 || this.StartIndex + this.Length > (this.m_strWidget as WTextRange).Text.Length ? string.Empty : (this.m_strWidget as WTextRange).Text.Substring(this.StartIndex, this.Length)) : string.Empty) : (this.m_strWidget as WTextRange).Text.Substring(this.StartIndex)) : string.Empty) : (string) null;
    }
  }

  public IStringWidget RealStringWidget => this.m_strWidget;

  public string GetText() => this.SplittedText;

  public ILayoutInfo LayoutInfo => this.m_strWidget.LayoutInfo;

  public void InitLayoutInfo(IWidget widget)
  {
  }

  void IWidget.InitLayoutInfo()
  {
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  public ISplitLeafWidget[] SplitBySize(
    DrawingContext dc,
    SizeF offset,
    float clientWidth,
    float clientActiveAreaWidth,
    ref bool isLastWordFit,
    bool isTabStopInterSectingfloattingItem,
    bool isSplitByCharacter,
    bool isFirstItemInLine,
    ref int countForConsecutivelimit)
  {
    return SplitStringWidget.SplitBySize(dc, (double) offset.Width, this.m_strWidget, this, clientWidth, clientActiveAreaWidth, ref isLastWordFit, isTabStopInterSectingfloattingItem, isSplitByCharacter, isFirstItemInLine, ref countForConsecutivelimit);
  }

  public SizeF Measure(DrawingContext dc) => this.m_strWidget.Measure(dc, this.GetText());

  public static ISplitLeafWidget[] SplitBySize(
    DrawingContext dc,
    double offset,
    IStringWidget strWidget,
    SplitStringWidget splitStringWidget,
    float clientWidth,
    float clientActiveAreaWidth,
    ref bool isLastWordFit,
    bool isTabStopInterSectingfloattingItem,
    bool isSplitByCharacter,
    bool isFirstItemInLine,
    ref int countForConsecutivelimit)
  {
    StringFormat format = new StringFormat(StringFormat.GenericTypographic);
    format.FormatFlags &= ~StringFormatFlags.LineLimit;
    format.Alignment = StringAlignment.Near;
    format.LineAlignment = StringAlignment.Near;
    format.Trimming = StringTrimming.Word;
    format.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip;
    string str1 = splitStringWidget != null ? splitStringWidget.SplittedText : (strWidget as WTextRange).Text;
    if (strWidget is WField ? (strWidget as WField).CharacterFormat.AllCaps : (strWidget as WTextRange).CharacterFormat.AllCaps)
      str1 = str1.ToUpper();
    WCharacterFormat charFormat = strWidget is WField ? (strWidget as WField).CharacterFormat : (strWidget as WTextRange).CharacterFormat;
    FontScriptType scriptType = strWidget is WField ? (strWidget as WField).ScriptType : (strWidget as WTextRange).ScriptType;
    Font font = dc.GetFont(scriptType, charFormat, str1);
    Font defaultFont = dc.GetDefaultFont(scriptType, font, charFormat);
    bool isTrailSpacesWrapped = false;
    WParagraph ownerParagraph = (strWidget as ParagraphItem).OwnerParagraph;
    int num = 0;
    bool isAutoHyphenated = false;
    bool flag1 = true;
    if (ownerParagraph != null && ownerParagraph.Document != null)
    {
      num = ownerParagraph.Document.DOP.ConsecHypLim;
      isAutoHyphenated = ownerParagraph.Document.DOP.AutoHyphen;
      flag1 = ownerParagraph.Document.DOP.HyphCapitals;
    }
    if (!flag1 && (strWidget is WTextRange ? (strWidget as WTextRange).CharacterFormat.AllCaps || (strWidget as WTextRange).CharacterFormat.SmallCaps : string.Equals(str1.ToUpper(), str1)))
      isAutoHyphenated = false;
    if (num != 0 && countForConsecutivelimit >= num)
      isAutoHyphenated = false;
    if (Hyphenator.Dictionaries.Count == 0)
      isAutoHyphenated = false;
    StringSplitter stringSplitter = new StringSplitter();
    StringSplitResult stringSplitResult = stringSplitter.Split(str1, font, defaultFont, format, new SizeF((float) offset, float.MaxValue), charFormat, ref isLastWordFit, isTabStopInterSectingfloattingItem, ref isTrailSpacesWrapped, isAutoHyphenated, strWidget);
    stringSplitter.Close();
    if (stringSplitResult.Lines.Length > 0)
    {
      ISplitLeafWidget[] splitLeafWidgetArray = new ISplitLeafWidget[2];
      int length1 = stringSplitResult.Lines[0].Line.Length;
      bool flag2 = false;
      if (isAutoHyphenated && stringSplitResult.Lines[0].Line.EndsWith("-"))
      {
        ++countForConsecutivelimit;
        if (splitStringWidget != null && (splitStringWidget.RealStringWidget as WTextRange).Text[splitStringWidget.StartIndex + (length1 - 1)] != '-')
        {
          string str2 = (splitStringWidget.RealStringWidget as WTextRange).Text.Insert(splitStringWidget.StartIndex + (length1 - 1), "-");
          (splitStringWidget.RealStringWidget as WTextRange).Text = str2;
          flag2 = true;
        }
        else if (length1 - 1 < (strWidget as WTextRange).Text.Length && (strWidget as WTextRange).Text[length1 - 1] != '-')
        {
          string str3 = (strWidget as WTextRange).Text.Insert(length1 - 1, "-");
          (strWidget as WTextRange).Text = str3;
          flag2 = true;
        }
      }
      else
        countForConsecutivelimit = 0;
      if (ownerParagraph.ParagraphFormat.Bidi && !charFormat.Bidi && length1 > 0 && stringSplitResult.Remainder != null && stringSplitResult.Remainder != string.Empty && (int) stringSplitResult.Remainder[0] == (int) ControlChar.SpaceChar && strWidget is WTextRange && ((strWidget as WTextRange).CharacterRange == CharacterRangeType.RTL || (int) stringSplitResult.Lines[0].Line[length1 - 1] == (int) ControlChar.SpaceChar))
      {
        stringSplitResult.Remainder.ToCharArray();
        string str4 = new string(stringSplitResult.Lines[0].Line.ToCharArray());
        bool flag3 = true;
        bool flag4 = false;
        for (int index = str4.Length - 1; index >= 0; --index)
        {
          if ((int) str4[index] != (int) ControlChar.SpaceChar)
          {
            flag3 = false;
            str4 = str4.Remove(index);
          }
          else if (flag3)
            str4 = str4.Remove(index);
          else
            break;
        }
        string str5 = str1.Substring(str4.Length);
        for (int index = 0; index < str5.Length; ++index)
        {
          if (TextSplitter.IsRTLChar(str5[index]))
            flag4 = true;
          else if ((int) str5[index] != (int) ControlChar.SpaceChar)
            break;
        }
        if (str4 != string.Empty && str5 != string.Empty && flag4)
        {
          stringSplitResult.Lines[0].Line = str4;
          stringSplitResult.Remainder = str5;
          length1 = stringSplitResult.Lines[0].Line.Length;
        }
      }
      splitLeafWidgetArray[0] = (ISplitLeafWidget) new SplitStringWidget(strWidget, splitStringWidget != null ? splitStringWidget.StartIndex : 0, stringSplitResult.Lines[0].Line.Length);
      (splitLeafWidgetArray[0] as SplitStringWidget).IsTrailSpacesWrapped = isTrailSpacesWrapped;
      string text = string.Empty;
      int startIndex1 = (splitStringWidget != null ? splitStringWidget.StartIndex : 0) + length1;
      if (stringSplitResult.Remainder == null && stringSplitResult.Lines.Length > 1)
      {
        for (int index = 1; index < stringSplitResult.Count; ++index)
          text = !(stringSplitResult.Lines[index].Line == ControlChar.Space) ? text + ControlChar.LineFeed + stringSplitResult.Lines[index].Line : text + ControlChar.LineFeed;
      }
      else
      {
        int startIndex2 = !isAutoHyphenated || !flag2 ? length1 : length1 - 1;
        text = isAutoHyphenated ? str1.Substring(startIndex2) : stringSplitResult.Remainder;
      }
      if (text == ControlChar.LineFeed || text == ControlChar.ParagraphBreak)
        text = ControlChar.Space;
      if (text != null)
      {
        if (SplitStringWidget.StartsWithExt(text, ControlChar.LineFeed) || SplitStringWidget.StartsWithExt(text, ControlChar.ParagraphBreak))
        {
          text = text.Remove(0, 1);
          if (stringSplitResult.Lines[0].Line != " ")
            ++startIndex1;
        }
        else if (SplitStringWidget.StartsWithExt(text, ControlChar.Space))
        {
          int length2 = text.Length;
          text = text.TrimStart(ControlChar.SpaceChar);
          startIndex1 += length2 - text.Length;
        }
        if (text != string.Empty)
          splitLeafWidgetArray[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex1, text.Length);
      }
      else
        splitLeafWidgetArray[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex1, -1);
      return splitLeafWidgetArray;
    }
    if ((strWidget as WTextRange).Text != null && (strWidget as WTextRange).Text != string.Empty)
    {
      countForConsecutivelimit = 0;
      if ((double) dc.MeasureTextRange(strWidget as WTextRange, stringSplitResult.Remainder).Width > offset && offset != 0.0)
        return SplitStringWidget.SplitByOffset(dc, offset, splitStringWidget != null ? splitStringWidget.StartIndex : 0, strWidget, (StringSplitInfo) null, clientWidth, clientActiveAreaWidth, stringSplitResult.Remainder, isSplitByCharacter, isFirstItemInLine);
    }
    else
      countForConsecutivelimit = 0;
    return (ISplitLeafWidget[]) null;
  }

  public static ISplitLeafWidget[] SplitByOffset(
    DrawingContext dc,
    double offset,
    int startIndex,
    IStringWidget strWidget,
    StringSplitInfo splitInfo,
    float clientWidth,
    float clientActiveAreaWidth,
    string textToSplit,
    bool isSpliByCharacter,
    bool isFirstItemInLine)
  {
    if (splitInfo == null)
      splitInfo = new StringSplitInfo(0, textToSplit.Length - 1);
    int index = strWidget.OffsetToIndex(dc, offset, splitInfo.GetSubstring(textToSplit), clientWidth, clientActiveAreaWidth, isSpliByCharacter);
    if (index > -1 && index < splitInfo.Length - 1)
    {
      ISplitLeafWidget[] splitLeafWidgetArray = new ISplitLeafWidget[2]
      {
        (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex, splitInfo.GetSplitFirstPart(index + 1).LastPos + 1),
        null
      };
      splitLeafWidgetArray[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, (splitLeafWidgetArray[0] as SplitStringWidget).StartIndex + (splitLeafWidgetArray[0] as SplitStringWidget).Length, -1);
      return splitLeafWidgetArray;
    }
    if (index <= -1 || splitInfo.Length - 1 != index || !isFirstItemInLine)
      return (ISplitLeafWidget[]) null;
    ISplitLeafWidget[] splitLeafWidgetArray1 = new ISplitLeafWidget[2]
    {
      (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex, 1),
      null
    };
    if (splitInfo.Length > 1)
      splitLeafWidgetArray1[1] = (ISplitLeafWidget) new SplitStringWidget(strWidget, startIndex + 1, -1);
    return splitLeafWidgetArray1;
  }

  private static bool StartsWithExt(string text, string value) => text.StartsWithExt(value);
}
