// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FindUtils
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class FindUtils
{
  internal const string DEF_WHOLE_WORD_BEFORE = "(?<=^|\\W|\\t)";
  internal const string DEF_WHOLE_WORD_AFTER = "(?=$|\\W|\\t)";
  internal const string DEF_WHOLE_WORD_EMPTY = "(?<=^|\\W|\\t)(?=$|\\W|\\t)";

  internal static bool IsPatternEmpty(Regex pattern)
  {
    string str = pattern.ToString();
    return str.Length == 0 || str == "(?<=^|\\W|\\t)(?=$|\\W|\\t)";
  }

  internal static Regex StringToRegex(string given, bool caseSensitive, bool wholeWord)
  {
    given = Regex.Escape(given);
    if (wholeWord)
      given = $"(?<=^|\\W|\\t){given}(?=$|\\W|\\t)";
    return new Regex(given, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
  }

  internal static int GetStartRangeIndex(WParagraph para, int start, out WTextRange tr)
  {
    tr = (WTextRange) null;
    int startRangeIndex = 0;
    int index = 0;
    for (int count = para.Items.Count; index < count; ++index)
    {
      int num1 = 0;
      int num2 = 0;
      if (para.Items[index] is Break && (para.Items[index] as Break).BreakType == BreakType.LineBreak)
      {
        tr = (para.Items[index] as Break).TextRange;
        num1 = (para.Items[index] as Break).StartPos;
        num2 = (para.Items[index] as Break).EndPos - num1;
      }
      else
      {
        tr = para.Items[index] as WTextRange;
        if (tr != null)
        {
          num1 = tr.StartPos;
          num2 = tr.TextLength;
        }
      }
      if (para.Items[index] is InlineContentControl inlineContentControl && inlineContentControl.EndPos >= start)
        return FindUtils.GetStartRangeIndexInInlineContentControl(inlineContentControl.ParagraphItems, start, out tr);
      if (tr != null && num1 + num2 >= start)
      {
        startRangeIndex = index;
        break;
      }
    }
    return startRangeIndex;
  }

  internal static bool EnsureSameOwner(WTextRange startTextRange, WTextRange endTextRange)
  {
    Entity entity1 = startTextRange.Owner is Break ? (startTextRange.Owner as Break).Owner : startTextRange.Owner;
    Entity entity2 = endTextRange.Owner is Break ? (endTextRange.Owner as Break).Owner : endTextRange.Owner;
    int num1 = startTextRange.Owner is Break ? (startTextRange.Owner as Break).Index : startTextRange.Index;
    int num2 = endTextRange.Owner is Break ? (endTextRange.Owner as Break).Index : endTextRange.Index;
    ParagraphItemCollection paragraphItemCollection = entity1 is WParagraph ? (entity1 as WParagraph).Items : (entity1 as InlineContentControl).ParagraphItems;
    if (entity1 == entity2)
    {
      for (int index = num1; index < num2; ++index)
      {
        if (paragraphItemCollection[index] is InlineContentControl)
          return false;
      }
    }
    return entity1 == entity2;
  }

  private static int GetStartRangeIndexInInlineContentControl(
    ParagraphItemCollection paragraphItems,
    int start,
    out WTextRange tr)
  {
    tr = (WTextRange) null;
    int inlineContentControl = 0;
    int index = 0;
    for (int count = paragraphItems.Count; index < count; ++index)
    {
      int num1 = 0;
      int num2 = 0;
      if (paragraphItems[index] is Break && (paragraphItems[index] as Break).BreakType == BreakType.LineBreak)
      {
        tr = (paragraphItems[index] as Break).TextRange;
        num1 = (paragraphItems[index] as Break).StartPos;
        num2 = (paragraphItems[index] as Break).EndPos - num1;
      }
      else
      {
        tr = paragraphItems[index] as WTextRange;
        if (tr != null)
        {
          num1 = tr.StartPos;
          num2 = tr.TextLength;
        }
      }
      if (tr != null && num1 + num2 >= start)
      {
        inlineContentControl = index;
        break;
      }
      if (paragraphItems[index] is InlineContentControl && paragraphItems[index].EndPos >= start)
        return FindUtils.GetStartRangeIndexInInlineContentControl((paragraphItems[index] as InlineContentControl).ParagraphItems, start, out tr);
    }
    return inlineContentControl;
  }
}
