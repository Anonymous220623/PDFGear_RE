// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.TrackChangesMarkups
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.Layouting;

internal class TrackChangesMarkups
{
  private RevisionType m_markupType;
  private WTextBody m_changedValue;
  private PointF m_position;
  private LayoutedWidget m_ltWidget;
  private float m_emptySpace;
  private WordDocument m_wordDocument;
  private float m_ballonYPosition;

  internal WordDocument Document => this.m_wordDocument;

  internal float BallonYPosition
  {
    get => this.m_ballonYPosition;
    set => this.m_ballonYPosition = value;
  }

  internal RevisionType TypeOfMarkup
  {
    get => this.m_markupType;
    set => this.m_markupType = value;
  }

  internal WTextBody ChangedValue
  {
    get
    {
      if (this.m_changedValue == null)
        this.m_changedValue = new WSection((IWordDocument) this.m_wordDocument).Body;
      return this.m_changedValue;
    }
    set => this.m_changedValue = value;
  }

  internal PointF Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal LayoutedWidget LtWidget
  {
    get => this.m_ltWidget;
    set => this.m_ltWidget = value;
  }

  internal float EmptySpace
  {
    get => this.m_emptySpace;
    set => this.m_emptySpace = value;
  }

  internal TrackChangesMarkups(WordDocument wordDocument) => this.m_wordDocument = wordDocument;

  internal string GetBalloonValueForMarkupType()
  {
    if (this.TypeOfMarkup == RevisionType.Deletions)
      return "Deleted";
    return this.TypeOfMarkup != RevisionType.Formatting ? "" : "Formatted";
  }

  internal void DisplayBalloonValueCFormat(
    FontScriptType scriptType,
    Dictionary<int, object> newpropertyhash,
    WCharacterFormat characterformat,
    ref Dictionary<int, string> hierarchyOrder)
  {
    bool isTextureRead = false;
    bool isBackcolorRead = false;
    bool isForecolorRead = false;
    string shadingFormattingText = "";
    string[] shadingFormattings = new string[3];
    foreach (KeyValuePair<int, object> keyValuePair in newpropertyhash)
    {
      switch (keyValuePair.Key)
      {
        case 1:
          if (!hierarchyOrder.ContainsKey(11) && characterformat.TextColor != Color.Empty)
          {
            hierarchyOrder.Add(11, "Font color:" + this.GetColorName(characterformat.TextColor));
            continue;
          }
          continue;
        case 2:
          if (!hierarchyOrder.ContainsKey(0))
          {
            hierarchyOrder.Add(0, characterformat.GetFontNameToRender(scriptType));
            continue;
          }
          continue;
        case 3:
          if (!hierarchyOrder.ContainsKey(2))
          {
            hierarchyOrder.Add(2, characterformat.FontSize.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " pt");
            continue;
          }
          continue;
        case 4:
          if (!hierarchyOrder.ContainsKey(4))
          {
            hierarchyOrder.Add(4, "Bold");
            continue;
          }
          continue;
        case 5:
          if (!hierarchyOrder.ContainsKey(5))
          {
            hierarchyOrder.Add(5, "Italic");
            continue;
          }
          continue;
        case 6:
          if (!hierarchyOrder.ContainsKey(12))
          {
            hierarchyOrder.Add(12, "Strikethrough");
            continue;
          }
          continue;
        case 7:
          if (!hierarchyOrder.ContainsKey(10))
          {
            hierarchyOrder.Add(10, "Underline");
            continue;
          }
          continue;
        case 9:
          isBackcolorRead = true;
          this.BackgroundColorFormatting(this.GetColorName(characterformat.TextBackgroundColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isForecolorRead);
          continue;
        case 10:
          if (characterformat.SubSuperScript == SubSuperScript.SubScript && !hierarchyOrder.ContainsKey(15))
            hierarchyOrder.Add(15, "Subscript");
          if (characterformat.SubSuperScript == SubSuperScript.SuperScript && !hierarchyOrder.ContainsKey(14))
          {
            hierarchyOrder.Add(14, "Superscript");
            continue;
          }
          continue;
        case 14:
          if (!hierarchyOrder.ContainsKey(13))
          {
            hierarchyOrder.Add(13, "Double strikethrough");
            continue;
          }
          continue;
        case 17:
          if ((double) characterformat.Position < 0.0)
          {
            if (!hierarchyOrder.ContainsKey(21))
            {
              hierarchyOrder.Add(21, $"Lowered by {Math.Abs(characterformat.Position).ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
              continue;
            }
            continue;
          }
          if (!hierarchyOrder.ContainsKey(21))
          {
            hierarchyOrder.Add(21, $"Raised by {characterformat.Position.ToString((IFormatProvider) CultureInfo.InvariantCulture)}pt");
            continue;
          }
          continue;
        case 18:
          if ((double) characterformat.CharacterSpacing < 0.0)
          {
            hierarchyOrder.Add(20, $"Condensed  by {Math.Abs(characterformat.CharacterSpacing).ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
            continue;
          }
          hierarchyOrder.Add(20, $"Expanded  by {characterformat.CharacterSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
          continue;
        case 50:
          if (!hierarchyOrder.ContainsKey(24))
          {
            hierarchyOrder.Add(24, "Shadow");
            continue;
          }
          continue;
        case 51:
          if (!hierarchyOrder.ContainsKey(32 /*0x20*/))
          {
            hierarchyOrder.Add(32 /*0x20*/, "Emboss");
            continue;
          }
          continue;
        case 52:
          if (!hierarchyOrder.ContainsKey(33))
          {
            hierarchyOrder.Add(33, "Engrave");
            continue;
          }
          continue;
        case 53:
          if (!hierarchyOrder.ContainsKey(18))
          {
            hierarchyOrder.Add(18, "Hidden");
            continue;
          }
          continue;
        case 54:
          if (!hierarchyOrder.ContainsKey(17))
          {
            hierarchyOrder.Add(17, "All caps");
            continue;
          }
          continue;
        case 55:
          if (!hierarchyOrder.ContainsKey(16 /*0x10*/))
          {
            hierarchyOrder.Add(16 /*0x10*/, "Small caps");
            continue;
          }
          continue;
        case 58:
          if (!hierarchyOrder.ContainsKey(34))
          {
            hierarchyOrder.Add(34, "Right- to- left");
            continue;
          }
          continue;
        case 59:
          if (!hierarchyOrder.ContainsKey(7))
          {
            hierarchyOrder.Add(7, "Bold");
            continue;
          }
          continue;
        case 60:
          if (!hierarchyOrder.ContainsKey(8))
          {
            hierarchyOrder.Add(8, "Italic");
            continue;
          }
          continue;
        case 61:
          if (!hierarchyOrder.ContainsKey(1))
          {
            hierarchyOrder.Add(1, characterformat.GetFontNameToRender(scriptType));
            continue;
          }
          continue;
        case 62:
          if (!hierarchyOrder.ContainsKey(3))
          {
            hierarchyOrder.Add(3, characterformat.FontSize.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " pt");
            continue;
          }
          continue;
        case 63 /*0x3F*/:
          if (!hierarchyOrder.ContainsKey(22))
          {
            hierarchyOrder.Add(22, "Highlight");
            continue;
          }
          continue;
        case 71:
          if (!hierarchyOrder.ContainsKey(23))
          {
            hierarchyOrder.Add(23, "Text Outline");
            continue;
          }
          continue;
        case 73:
          if (!hierarchyOrder.ContainsKey(35))
          {
            hierarchyOrder.Add(35, this.GetDisplayNameOfLocale(characterformat.LocaleIdASCII));
            continue;
          }
          continue;
        case 75:
          string str1 = "(Complex) " + this.GetDisplayNameOfLocale(characterformat.LocaleIdASCII);
          if (!hierarchyOrder.ContainsKey(36))
          {
            hierarchyOrder.Add(36, str1);
            continue;
          }
          continue;
        case 76:
          if (characterformat.NoProof && !hierarchyOrder.ContainsKey(37))
          {
            hierarchyOrder.Add(37, "Do not check spelling and grammar");
            continue;
          }
          continue;
        case 77:
          isForecolorRead = true;
          this.ForecolorFormatting(this.GetColorName(characterformat.ForeColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isBackcolorRead);
          continue;
        case 78:
          isTextureRead = true;
          this.TextureStyleFormatting(this.GetTextureStyleText(characterformat.TextureStyle), hierarchyOrder, shadingFormattings, shadingFormattingText, isBackcolorRead, isForecolorRead);
          continue;
        case 79:
          if (characterformat.EmphasisType != EmphasisType.NoEmphasis)
          {
            switch (characterformat.EmphasisType)
            {
              case EmphasisType.Dot:
                if (!hierarchyOrder.ContainsKey(38))
                {
                  hierarchyOrder.Add(38, "Dot");
                  continue;
                }
                continue;
              case EmphasisType.Comma:
                if (!hierarchyOrder.ContainsKey(38))
                {
                  hierarchyOrder.Add(38, "Comma");
                  continue;
                }
                continue;
              case EmphasisType.Circle:
                if (!hierarchyOrder.ContainsKey(38))
                {
                  hierarchyOrder.Add(38, "Strikethrough");
                  continue;
                }
                continue;
              case EmphasisType.UnderDot:
                if (!hierarchyOrder.ContainsKey(38))
                {
                  hierarchyOrder.Add(38, "Outline");
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
          else
            continue;
        case 80 /*0x50*/:
          if (characterformat.TextEffect != TextEffect.None)
          {
            switch (characterformat.TextEffect)
            {
              case TextEffect.LasVegasLights:
                if (!hierarchyOrder.ContainsKey(39))
                {
                  hierarchyOrder.Add(39, "Las Vegas Lights");
                  continue;
                }
                continue;
              case TextEffect.BlinkingBackground:
                if (!hierarchyOrder.ContainsKey(39))
                {
                  hierarchyOrder.Add(39, "Blinking Background");
                  continue;
                }
                continue;
              case TextEffect.SparkleText:
                if (!hierarchyOrder.ContainsKey(39))
                {
                  hierarchyOrder.Add(39, "Sparkle Text");
                  continue;
                }
                continue;
              case TextEffect.MarchingBlackAnts:
                if (!hierarchyOrder.ContainsKey(39))
                {
                  hierarchyOrder.Add(39, "Marching Black Ants");
                  continue;
                }
                continue;
              case TextEffect.MarchingRedAnts:
                if (!hierarchyOrder.ContainsKey(39))
                {
                  hierarchyOrder.Add(39, "Marching Red Ants");
                  continue;
                }
                continue;
              case TextEffect.Shimmer:
                if (!hierarchyOrder.ContainsKey(39))
                {
                  hierarchyOrder.Add(39, "Shimmer");
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
          else
            continue;
        case 90:
          string colorName = this.GetColorName(characterformat.UnderlineColor);
          if (!hierarchyOrder.ContainsKey(10))
          {
            hierarchyOrder.Add(10, "Underline color:" + colorName);
            continue;
          }
          continue;
        case 120:
          if (!hierarchyOrder.ContainsKey(28))
          {
            hierarchyOrder.Add(28, "Contextual Alternates");
            continue;
          }
          continue;
        case 121:
          if (characterformat.Ligatures != LigatureType.None)
          {
            string str2 = "Ligatures: ";
            switch (characterformat.Ligatures)
            {
              case LigatureType.Standard:
                str2 += "Standard";
                break;
              case LigatureType.Contextual:
                str2 += "Contextual";
                break;
              case LigatureType.StandardContextual:
                str2 += "Standard + Contextual";
                break;
              case LigatureType.Historical:
                str2 += "Historical";
                break;
              case LigatureType.StandardHistorical:
                str2 += "Standard + Historical";
                break;
              case LigatureType.ContextualHistorical:
                str2 += "Contextual + Historical";
                break;
              case LigatureType.StandardContextualHistorical:
                str2 += "Standard + Contextual + Historical";
                break;
              case LigatureType.Discretional:
                str2 += "Discretional";
                break;
              case LigatureType.StandardDiscretional:
                str2 += "Standard + Discretional";
                break;
              case LigatureType.ContextualDiscretional:
                str2 += "Contextual + Discretional";
                break;
              case LigatureType.StandardContextualDiscretional:
                str2 += "Standard + Contextual +Discretional";
                break;
              case LigatureType.HistoricalDiscretional:
                str2 += "Historical + Discretional";
                break;
              case LigatureType.StandardHistoricalDiscretional:
                str2 += "Standard + Historical + Discretional";
                break;
              case LigatureType.ContextualHistoricalDiscretional:
                str2 += "Contextual + Historical + Discretional";
                break;
              case LigatureType.All:
                str2 += "All";
                break;
            }
            if (!hierarchyOrder.ContainsKey(25))
            {
              hierarchyOrder.Add(25, str2);
              continue;
            }
            continue;
          }
          continue;
        case 122:
          if (characterformat.NumberForm != NumberFormType.Default)
          {
            string str3 = "Number Forms: ";
            switch (characterformat.NumberForm)
            {
              case NumberFormType.Lining:
                str3 += "Lining";
                break;
              case NumberFormType.OldStyle:
                str3 += "Oldstyle";
                break;
            }
            if (!hierarchyOrder.ContainsKey(26))
            {
              hierarchyOrder.Add(26, str3);
              continue;
            }
            continue;
          }
          continue;
        case 123:
          if (characterformat.NumberSpacing != NumberSpacingType.Default)
          {
            string str4 = "Number Spacing: ";
            switch (characterformat.NumberSpacing)
            {
              case NumberSpacingType.Proportional:
                str4 += "Proportional";
                break;
              case NumberSpacingType.Tabular:
                str4 += "Tabular";
                break;
            }
            if (!hierarchyOrder.ContainsKey(27))
            {
              hierarchyOrder.Add(27, str4);
              continue;
            }
            continue;
          }
          continue;
        case 124:
          string str5 = "Stylistic Set " + (object) (int) characterformat.StylisticSet;
          if (!hierarchyOrder.ContainsKey(29))
          {
            hierarchyOrder.Add(29, str5);
            continue;
          }
          continue;
        case 125:
          if (!hierarchyOrder.ContainsKey(21))
          {
            hierarchyOrder.Add(21, $"Kern at {characterformat.Kern.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
            continue;
          }
          continue;
        case (int) sbyte.MaxValue:
          if (!hierarchyOrder.ContainsKey(19))
          {
            hierarchyOrder.Add(19, $"Character scale: {characterformat.Scaling.ToString((IFormatProvider) CultureInfo.InvariantCulture)}%");
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  internal void DisplayBalloonValueforRemovedCFormat(
    Dictionary<int, object> newpropertyhash,
    WCharacterFormat characterformat,
    ref Dictionary<int, string> hierarchyOrder)
  {
    string[] shadingFormattings = new string[3];
    bool isTextureRead = false;
    bool isBackcolorRead = false;
    bool isForecolorRead = false;
    string shadingFormattingText = "";
    foreach (KeyValuePair<int, object> keyValuePair in newpropertyhash)
    {
      switch (keyValuePair.Key)
      {
        case 4:
          if (!hierarchyOrder.ContainsKey(4))
          {
            hierarchyOrder.Add(4, "Not Bold");
            continue;
          }
          continue;
        case 5:
          if (!hierarchyOrder.ContainsKey(5))
          {
            hierarchyOrder.Add(5, "Not Italic");
            continue;
          }
          continue;
        case 6:
          if (!hierarchyOrder.ContainsKey(12))
          {
            hierarchyOrder.Add(12, "Not Strikethrough");
            continue;
          }
          continue;
        case 7:
          if (!hierarchyOrder.ContainsKey(10))
          {
            hierarchyOrder.Add(10, "No underline");
            continue;
          }
          continue;
        case 9:
          isBackcolorRead = true;
          this.BackgroundColorFormatting(this.GetColorName(characterformat.TextBackgroundColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isForecolorRead);
          continue;
        case 10:
          if (characterformat.SubSuperScript == SubSuperScript.None && !hierarchyOrder.ContainsKey(15))
          {
            hierarchyOrder.Add(15, "Not Superscript/ Subscript");
            continue;
          }
          continue;
        case 14:
          if (!hierarchyOrder.ContainsKey(13))
          {
            hierarchyOrder.Add(13, "Not Double strikethrough");
            continue;
          }
          continue;
        case 17:
          if (!hierarchyOrder.ContainsKey(21))
          {
            hierarchyOrder.Add(21, "Not Raised by / Lowered by");
            continue;
          }
          continue;
        case 18:
          if (!hierarchyOrder.ContainsKey(20))
          {
            hierarchyOrder.Add(20, "Not Expanded by/ Condensed by");
            continue;
          }
          continue;
        case 50:
          if (!hierarchyOrder.ContainsKey(24))
          {
            hierarchyOrder.Add(24, "Not Shadow");
            continue;
          }
          continue;
        case 51:
          if (!hierarchyOrder.ContainsKey(32 /*0x20*/))
          {
            hierarchyOrder.Add(32 /*0x20*/, "Not Emboss");
            continue;
          }
          continue;
        case 52:
          if (!hierarchyOrder.ContainsKey(33))
          {
            hierarchyOrder.Add(33, "Not Engrave");
            continue;
          }
          continue;
        case 53:
          if (!hierarchyOrder.ContainsKey(18))
          {
            hierarchyOrder.Add(18, "Not Hidden");
            continue;
          }
          continue;
        case 54:
          if (!hierarchyOrder.ContainsKey(17))
          {
            hierarchyOrder.Add(17, "Not All caps");
            continue;
          }
          continue;
        case 55:
          if (!hierarchyOrder.ContainsKey(16 /*0x10*/))
          {
            hierarchyOrder.Add(16 /*0x10*/, "Not Small caps");
            continue;
          }
          continue;
        case 58:
          if (!hierarchyOrder.ContainsKey(34))
          {
            hierarchyOrder.Add(34, "Left-to-right");
            continue;
          }
          continue;
        case 59:
          if (!hierarchyOrder.ContainsKey(7))
          {
            hierarchyOrder.Add(7, "Complex: Not Bold");
            continue;
          }
          continue;
        case 60:
          if (!hierarchyOrder.ContainsKey(8))
          {
            hierarchyOrder.Add(8, "Not Italic");
            continue;
          }
          continue;
        case 63 /*0x3F*/:
          if (!hierarchyOrder.ContainsKey(22))
          {
            hierarchyOrder.Add(22, "Not Highlight");
            continue;
          }
          continue;
        case 71:
          if (!hierarchyOrder.ContainsKey(23))
          {
            hierarchyOrder.Add(23, "Text Outline");
            continue;
          }
          continue;
        case 76:
          if (!characterformat.NoProof && !hierarchyOrder.ContainsKey(37))
          {
            hierarchyOrder.Add(37, "Check spelling and grammar");
            continue;
          }
          continue;
        case 77:
          isForecolorRead = true;
          this.ForecolorFormatting(this.GetColorName(characterformat.ForeColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isBackcolorRead);
          continue;
        case 78:
          isTextureRead = true;
          this.TextureStyleFormatting(this.GetTextureStyleText(characterformat.TextureStyle), hierarchyOrder, shadingFormattings, shadingFormattingText, isBackcolorRead, isForecolorRead);
          continue;
        case 79:
          switch (characterformat.EmphasisType)
          {
            case EmphasisType.NoEmphasis:
              if (!hierarchyOrder.ContainsKey(38))
              {
                hierarchyOrder.Add(38, "No emphasis mark");
                continue;
              }
              continue;
            case EmphasisType.Dot:
              if (!hierarchyOrder.ContainsKey(38))
              {
                hierarchyOrder.Add(38, "Dot");
                continue;
              }
              continue;
            case EmphasisType.Comma:
              if (!hierarchyOrder.ContainsKey(38))
              {
                hierarchyOrder.Add(38, "Comma");
                continue;
              }
              continue;
            case EmphasisType.Circle:
              if (!hierarchyOrder.ContainsKey(38))
              {
                hierarchyOrder.Add(38, "Strikethrough");
                continue;
              }
              continue;
            case EmphasisType.UnderDot:
              if (!hierarchyOrder.ContainsKey(38))
              {
                hierarchyOrder.Add(38, "Outline");
                continue;
              }
              continue;
            default:
              continue;
          }
        case 80 /*0x50*/:
          switch (characterformat.TextEffect)
          {
            case TextEffect.None:
              if (!hierarchyOrder.ContainsKey(39))
              {
                hierarchyOrder.Add(39, "None");
                continue;
              }
              continue;
            case TextEffect.LasVegasLights:
              if (!hierarchyOrder.ContainsKey(39))
              {
                hierarchyOrder.Add(39, "Las Vegas Lights");
                continue;
              }
              continue;
            case TextEffect.BlinkingBackground:
              if (!hierarchyOrder.ContainsKey(39))
              {
                hierarchyOrder.Add(39, "Blinking Background");
                continue;
              }
              continue;
            case TextEffect.SparkleText:
              if (!hierarchyOrder.ContainsKey(39))
              {
                hierarchyOrder.Add(39, "Sparkle Text");
                continue;
              }
              continue;
            case TextEffect.MarchingBlackAnts:
              if (!hierarchyOrder.ContainsKey(39))
              {
                hierarchyOrder.Add(39, "Marching Black Ants");
                continue;
              }
              continue;
            case TextEffect.MarchingRedAnts:
              if (!hierarchyOrder.ContainsKey(39))
              {
                hierarchyOrder.Add(39, "Marching Red Ants");
                continue;
              }
              continue;
            case TextEffect.Shimmer:
              if (!hierarchyOrder.ContainsKey(39))
              {
                hierarchyOrder.Add(39, "Shimmer");
                continue;
              }
              continue;
            default:
              continue;
          }
        case 120:
          if (!hierarchyOrder.ContainsKey(28))
          {
            hierarchyOrder.Add(28, "Not Contextual Alternates");
            continue;
          }
          continue;
        case 121:
          string str1 = "Ligatures: ";
          switch (characterformat.Ligatures)
          {
            case LigatureType.None:
              str1 += "None";
              break;
            case LigatureType.Standard:
              str1 += "Standard";
              break;
            case LigatureType.Contextual:
              str1 += "Contextual";
              break;
            case LigatureType.StandardContextual:
              str1 += "Standard + Contextual";
              break;
            case LigatureType.Historical:
              str1 += "Historical";
              break;
            case LigatureType.StandardHistorical:
              str1 += "Standard + Historical";
              break;
            case LigatureType.ContextualHistorical:
              str1 += "Contextual + Historical";
              break;
            case LigatureType.StandardContextualHistorical:
              str1 += "Standard + Contextual + Historical";
              break;
            case LigatureType.Discretional:
              str1 += "Discretional";
              break;
            case LigatureType.StandardDiscretional:
              str1 += "Standard + Discretional";
              break;
            case LigatureType.ContextualDiscretional:
              str1 += "Contextual + Discretional";
              break;
            case LigatureType.StandardContextualDiscretional:
              str1 += "Standard + Contextual +Discretional";
              break;
            case LigatureType.HistoricalDiscretional:
              str1 += "Historical + Discretional";
              break;
            case LigatureType.StandardHistoricalDiscretional:
              str1 += "Standard + Historical + Discretional";
              break;
            case LigatureType.ContextualHistoricalDiscretional:
              str1 += "Contextual + Historical + Discretional";
              break;
            case LigatureType.All:
              str1 += "All";
              break;
          }
          if (!hierarchyOrder.ContainsKey(25))
          {
            hierarchyOrder.Add(25, str1);
            continue;
          }
          continue;
        case 122:
          string str2 = "Number Forms: ";
          switch (characterformat.NumberForm)
          {
            case NumberFormType.Default:
              str2 += "Default";
              break;
            case NumberFormType.Lining:
              str2 += "Lining";
              break;
            case NumberFormType.OldStyle:
              str2 += "Oldstyle";
              break;
          }
          if (!hierarchyOrder.ContainsKey(26))
          {
            hierarchyOrder.Add(26, str2);
            continue;
          }
          continue;
        case 123:
          string str3 = "Number Spacing: ";
          switch (characterformat.NumberSpacing)
          {
            case NumberSpacingType.Default:
              str3 += "Default";
              break;
            case NumberSpacingType.Proportional:
              str3 += "Proportional";
              break;
            case NumberSpacingType.Tabular:
              str3 += "Tabular";
              break;
          }
          if (!hierarchyOrder.ContainsKey(27))
          {
            hierarchyOrder.Add(27, str3);
            continue;
          }
          continue;
        case 124:
          if (characterformat.StylisticSet != StylisticSetType.StylisticSetDefault)
          {
            string str4 = "Stylistic Set: " + (object) (int) characterformat.StylisticSet;
            if (!hierarchyOrder.ContainsKey(29))
            {
              hierarchyOrder.Add(29, str4);
              continue;
            }
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private string GetDisplayNameOfLocale(short localeIdASCII)
  {
    string name = "";
    if (Enum.IsDefined(typeof (LocaleIDs), (object) (int) localeIdASCII))
      name = ((LocaleIDs) localeIdASCII).ToString().Replace("_", "-");
    return !string.IsNullOrEmpty(name) ? new CultureInfo(name).DisplayName : "";
  }

  private string GetTextureStyleText(TextureStyle textureStyle)
  {
    switch (textureStyle)
    {
      case TextureStyle.TextureSolid:
        return "Solid (100%)";
      case TextureStyle.Texture5Percent:
        return "5%";
      case TextureStyle.Texture10Percent:
        return "10%";
      case TextureStyle.Texture20Percent:
        return "20%";
      case TextureStyle.Texture25Percent:
        return "25%";
      case TextureStyle.Texture30Percent:
        return "30%";
      case TextureStyle.Texture40Percent:
        return "40%";
      case TextureStyle.Texture50Percent:
        return "50%";
      case TextureStyle.Texture60Percent:
        return "60%";
      case TextureStyle.Texture70Percent:
        return "70%";
      case TextureStyle.Texture75Percent:
        return "75%";
      case TextureStyle.Texture80Percent:
        return "80%";
      case TextureStyle.Texture90Percent:
        return "90%";
      case TextureStyle.TextureDarkHorizontal:
        return "DK Horizontal";
      case TextureStyle.TextureDarkVertical:
        return "Dk Vertical";
      case TextureStyle.TextureDarkDiagonalDown:
        return "Dk Dwn Diagonal";
      case TextureStyle.TextureDarkDiagonalUp:
        return "Dk Up Diagonal";
      case TextureStyle.TextureDarkCross:
        return "Dk Grid";
      case TextureStyle.TextureDarkDiagonalCross:
        return "Dk Trellis";
      case TextureStyle.TextureHorizontal:
        return "Lt Horizontal";
      case TextureStyle.TextureVertical:
        return "Lt Vertical";
      case TextureStyle.TextureDiagonalDown:
        return "Lt Dwn Diagonal";
      case TextureStyle.TextureDiagonalUp:
        return "Lt Up Diagonal";
      case TextureStyle.TextureCross:
        return "Lt Grid";
      case TextureStyle.TextureDiagonalCross:
        return "Lt Trellis";
      case TextureStyle.Texture12Pt5Percent:
        return 12.5f.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "%";
      case TextureStyle.Texture15Percent:
        return "15%";
      case TextureStyle.Texture35Percent:
        return "35%";
      case TextureStyle.Texture37Pt5Percent:
        return 37.5f.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "%";
      case TextureStyle.Texture45Percent:
        return "45%";
      case TextureStyle.Texture55Percent:
        return "55%";
      case TextureStyle.Texture62Pt5Percent:
        return 62.5f.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "%";
      case TextureStyle.Texture65Percent:
        return "65%";
      case TextureStyle.Texture85Percent:
        return "85%";
      case TextureStyle.Texture87Pt5Percent:
        return 87.5f.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "%";
      case TextureStyle.Texture95Percent:
        return "95%";
      default:
        return "Clear";
    }
  }

  private string GenerateShadingFormattingText(
    string textureStyle,
    string foreColor,
    string textBgColor)
  {
    string shadingFormattingText = "";
    if (textureStyle != null)
    {
      if (textureStyle.Equals("Clear", StringComparison.OrdinalIgnoreCase))
      {
        if (string.IsNullOrEmpty(foreColor) && !string.IsNullOrEmpty(textBgColor))
          shadingFormattingText = $"Pattern: {textureStyle}({textBgColor})";
        else
          shadingFormattingText = "Pattern:" + textureStyle;
      }
      else if (string.IsNullOrEmpty(foreColor) && string.IsNullOrEmpty(textBgColor))
        shadingFormattingText = "Pattern: " + textureStyle;
      else if (!string.IsNullOrEmpty(foreColor) && !string.IsNullOrEmpty(textBgColor))
        shadingFormattingText = $"Pattern: {textureStyle}({foreColor}Foreground,{textBgColor}Background)";
      else if (string.IsNullOrEmpty(foreColor) && !string.IsNullOrEmpty(textBgColor))
        shadingFormattingText = $"Pattern: {textureStyle}( Auto Foreground,{textBgColor}Background)";
      else if (!string.IsNullOrEmpty(foreColor) && string.IsNullOrEmpty(textBgColor))
        shadingFormattingText = $"Pattern: {textureStyle}({foreColor}Foreground,Auto Background)";
    }
    return shadingFormattingText;
  }

  private string GetColorName(Color colorValue)
  {
    switch (colorValue.Name.ToLower())
    {
      case "ffc00000":
        return "Dark Red";
      case "ffff0000":
        return "Red";
      case "ffffc000":
        return "Orange";
      case "ffffff00":
        return "Yellow";
      case "ff92d050":
        return "Light Green";
      case "ff00b050":
        return "Green";
      case "ff00b0f0":
        return "Light Blue";
      case "ff0070c0":
        return "Blue";
      case "ff002060":
        return "Dark Blue";
      case "ff7030a0":
        return "Purple";
      default:
        if (!(colorValue != Color.Empty))
          return "";
        return $"Custom Color(RGB({(object) colorValue.R},{(object) colorValue.G},{(object) colorValue.B}))";
    }
  }

  internal void DisplayBalloonValueForPFormat(
    Dictionary<int, object> newpropertyhash,
    WParagraphFormat paragraphFormat,
    ref Dictionary<int, string> hierarchyOrder)
  {
    string[] shadingFormattings = new string[3];
    string shadingFormattingText = "";
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    bool isTextureRead = false;
    bool isBackcolorRead = false;
    bool isForecolorRead = false;
    foreach (KeyValuePair<int, object> keyValuePair in newpropertyhash)
    {
      switch (keyValuePair.Key)
      {
        case 0:
          this.GenerateAlignmentFormattingText(paragraphFormat, hierarchyOrder);
          continue;
        case 2:
        case 3:
        case 5:
        case 85:
        case 86:
        case 87:
          if (!flag1)
          {
            if (!hierarchyOrder.ContainsKey(32 /*0x20*/))
              hierarchyOrder.Add(32 /*0x20*/, "Indent: " + this.GenerateIndentsFormattingText(newpropertyhash, paragraphFormat));
            flag1 = true;
            continue;
          }
          continue;
        case 6:
          if (!hierarchyOrder.ContainsKey(9))
          {
            hierarchyOrder.Add(9, "Keep lines together");
            continue;
          }
          continue;
        case 8:
        case 9:
        case 54:
        case 55:
        case 90:
        case 91:
          if (!flag2)
          {
            if (!hierarchyOrder.ContainsKey(33))
              hierarchyOrder.Add(33, "Space " + this.GetSpacingChangesText(newpropertyhash, paragraphFormat));
            flag2 = true;
            continue;
          }
          continue;
        case 10:
          if (!hierarchyOrder.ContainsKey(10))
          {
            hierarchyOrder.Add(10, "Keep with next");
            continue;
          }
          continue;
        case 11:
          if (!hierarchyOrder.ContainsKey(11))
          {
            hierarchyOrder.Add(11, "No widow/orphan control");
            continue;
          }
          continue;
        case 12:
          if (!hierarchyOrder.ContainsKey(12))
          {
            hierarchyOrder.Add(12, "Page break before");
            continue;
          }
          continue;
        case 20:
        case 57:
        case 58:
        case 59:
        case 60:
        case 61:
        case 62:
        case 63 /*0x3F*/:
        case 64 /*0x40*/:
        case 66:
        case 67:
        case 93:
        case 94:
          if (!flag4)
          {
            string borderChangesText = this.GetBorderChangesText(paragraphFormat);
            if (borderChangesText != "" && !hierarchyOrder.ContainsKey(36))
              hierarchyOrder.Add(36, "Border: " + borderChangesText);
            flag4 = true;
            continue;
          }
          continue;
        case 21:
          isBackcolorRead = true;
          this.BackgroundColorFormatting(this.GetColorName(paragraphFormat.BackColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isForecolorRead);
          continue;
        case 30:
          if (paragraphFormat.Tabs.Count > 0)
          {
            string str = "Tab stops: ";
            for (int index = 0; index < paragraphFormat.Tabs.Count; ++index)
            {
              Tab tab = paragraphFormat.Tabs[index];
              str = $"{str}{tab.Position.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt, {this.GetTabAlignmentText(tab.Justification)}";
              if (tab.TabLeader != Syncfusion.DocIO.DLS.TabLeader.NoLeader)
                str += ", Leader: ...";
              if (index != paragraphFormat.Tabs.Count - 1)
                str += " + ";
            }
            if (!hierarchyOrder.ContainsKey(31 /*0x1F*/))
            {
              hierarchyOrder.Add(31 /*0x1F*/, str);
              continue;
            }
            continue;
          }
          continue;
        case 31 /*0x1F*/:
          if (!hierarchyOrder.ContainsKey(16 /*0x10*/))
          {
            hierarchyOrder.Add(16 /*0x10*/, "Right- to- left");
            continue;
          }
          continue;
        case 32 /*0x20*/:
          isForecolorRead = true;
          this.ForecolorFormatting(this.GetColorName(paragraphFormat.ForeColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isBackcolorRead);
          continue;
        case 33:
          isTextureRead = true;
          this.TextureStyleFormatting(this.GetTextureStyleText(paragraphFormat.TextureStyle), hierarchyOrder, shadingFormattings, shadingFormattingText, isBackcolorRead, isForecolorRead);
          continue;
        case 34:
          if (paragraphFormat.BaseLineAlignment != BaseLineAlignment.Auto && !hierarchyOrder.ContainsKey(23))
          {
            hierarchyOrder.Add(23, "Font Alignment: " + paragraphFormat.BaseLineAlignment.ToString());
            continue;
          }
          continue;
        case 37:
          switch (paragraphFormat.TextboxTightWrap)
          {
            case TextboxTightWrapOptions.AllLines:
              if (!hierarchyOrder.ContainsKey(24))
              {
                hierarchyOrder.Add(24, "Textbox Tight Wrap: Heading will be collapsed by default on open");
                continue;
              }
              continue;
            case TextboxTightWrapOptions.FirstAndLastLine:
              if (!hierarchyOrder.ContainsKey(25))
              {
                hierarchyOrder.Add(25, "Textbox Tight Wrap: All");
                continue;
              }
              continue;
            case TextboxTightWrapOptions.FirstLineOnly:
              if (!hierarchyOrder.ContainsKey(26))
              {
                hierarchyOrder.Add(26, "Textbox Tight Wrap: First and last lines");
                continue;
              }
              continue;
            case TextboxTightWrapOptions.LastLineOnly:
              if (!hierarchyOrder.ContainsKey(27))
              {
                hierarchyOrder.Add(27, "Textbox Tight Wrap: First line only");
                continue;
              }
              continue;
            default:
              continue;
          }
        case 38:
          if (!hierarchyOrder.ContainsKey(28))
          {
            hierarchyOrder.Add(28, "Suppress line numbers");
            continue;
          }
          continue;
        case 39:
        case 71:
        case 72:
        case 73:
        case 74:
        case 76:
        case 77:
        case 83:
        case 84:
        case 88:
          if (!flag3)
          {
            if (!hierarchyOrder.ContainsKey(115))
              hierarchyOrder.Add(115, this.GetFrameFormattingText(newpropertyhash, paragraphFormat));
            flag3 = true;
            continue;
          }
          continue;
        case 41:
          if (!hierarchyOrder.ContainsKey(29))
          {
            hierarchyOrder.Add(29, "Don�t allow hanging punctuation");
            continue;
          }
          continue;
        case 42:
          if (!hierarchyOrder.ContainsKey(30))
          {
            hierarchyOrder.Add(30, "Compress initial punctuation");
            continue;
          }
          continue;
        case 52:
        case 53:
          if (!hierarchyOrder.ContainsKey(34))
          {
            hierarchyOrder.Add(34, $"Line Spacing: {(paragraphFormat.LineSpacingRule == LineSpacingRule.AtLeast ? "At least" : paragraphFormat.LineSpacingRule.ToString())} {paragraphFormat.LineSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
            continue;
          }
          continue;
        case 56:
          if (!hierarchyOrder.ContainsKey(35))
          {
            hierarchyOrder.Add(35, "Outline: " + (object) paragraphFormat.OutlineLevel);
            continue;
          }
          continue;
        case 75:
          if (!hierarchyOrder.ContainsKey(22))
          {
            hierarchyOrder.Add(22, "Don't swap indents on facing pages");
            continue;
          }
          continue;
        case 78:
          if (paragraphFormat.SuppressAutoHyphens && !hierarchyOrder.ContainsKey(20))
          {
            hierarchyOrder.Add(20, "Don't hyphenate");
            continue;
          }
          continue;
        case 81:
          if (!paragraphFormat.AutoSpaceDE && !hierarchyOrder.ContainsKey(17))
          {
            hierarchyOrder.Add(17, "Don't adjust space between Latin and Asian text");
            continue;
          }
          continue;
        case 82:
          if (!paragraphFormat.AutoSpaceDN && !hierarchyOrder.ContainsKey(18))
          {
            hierarchyOrder.Add(18, "Don't adjust space between Asian text and numbers");
            continue;
          }
          continue;
        case 92:
          if (!hierarchyOrder.ContainsKey(19))
          {
            hierarchyOrder.Add(19, "Don't add space between paragraphs of the same style");
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private void GenerateAlignmentFormattingText(
    WParagraphFormat paragraphFormat,
    Dictionary<int, string> hierarchyOrder)
  {
    switch (paragraphFormat.LogicalJustification)
    {
      case HorizontalAlignment.Left:
        if (hierarchyOrder.ContainsKey(137))
          break;
        hierarchyOrder.Add(137, "Left");
        break;
      case HorizontalAlignment.Center:
        if (hierarchyOrder.ContainsKey(112 /*0x70*/))
          break;
        hierarchyOrder.Add(112 /*0x70*/, "Centered");
        break;
      case HorizontalAlignment.Right:
        if (hierarchyOrder.ContainsKey(2))
          break;
        hierarchyOrder.Add(2, "Right");
        break;
      case HorizontalAlignment.Justify:
        if (hierarchyOrder.ContainsKey(3))
          break;
        hierarchyOrder.Add(3, "Justified");
        break;
      case HorizontalAlignment.Distribute:
        if (hierarchyOrder.ContainsKey(4))
          break;
        hierarchyOrder.Add(4, "Distributed");
        break;
      case HorizontalAlignment.JustifyMedium:
        if (hierarchyOrder.ContainsKey(5))
          break;
        hierarchyOrder.Add(5, "Justify Medium");
        break;
      case HorizontalAlignment.JustifyHigh:
        if (hierarchyOrder.ContainsKey(6))
          break;
        hierarchyOrder.Add(6, "Justify High");
        break;
      case HorizontalAlignment.JustifyLow:
        if (hierarchyOrder.ContainsKey(7))
          break;
        hierarchyOrder.Add(7, "Justify Low");
        break;
      case HorizontalAlignment.ThaiJustify:
        if (hierarchyOrder.ContainsKey(8))
          break;
        hierarchyOrder.Add(8, "Thai Distributed Justification");
        break;
    }
  }

  internal void DisplayBalloonValueForRemovedPFormat(
    Dictionary<int, object> newpropertyhash,
    WParagraphFormat paragraphFormat,
    ref Dictionary<int, string> hierarchyOrder)
  {
    string[] shadingFormattings = new string[3];
    string shadingFormattingText = "";
    bool flag1 = false;
    bool flag2 = false;
    bool isTextureRead = false;
    bool isBackcolorRead = false;
    bool isForecolorRead = false;
    foreach (KeyValuePair<int, object> keyValuePair in newpropertyhash)
    {
      switch (keyValuePair.Key)
      {
        case 0:
          this.GenerateAlignmentFormattingText(paragraphFormat, hierarchyOrder);
          continue;
        case 2:
        case 3:
        case 5:
        case 85:
        case 86:
        case 87:
          if (!flag1)
          {
            if (!hierarchyOrder.ContainsKey(32 /*0x20*/))
              hierarchyOrder.Add(32 /*0x20*/, "Indent: " + this.GenerateIndentsFormattingText(newpropertyhash, paragraphFormat));
            flag1 = true;
            continue;
          }
          continue;
        case 6:
          if (!hierarchyOrder.ContainsKey(39))
          {
            hierarchyOrder.Add(39, "Don't Keep lines together");
            continue;
          }
          continue;
        case 10:
          if (!hierarchyOrder.ContainsKey(39))
          {
            hierarchyOrder.Add(39, "Don't Keep with next");
            continue;
          }
          continue;
        case 11:
          if (!hierarchyOrder.ContainsKey(102))
          {
            hierarchyOrder.Add(102, "Widow/orphan control");
            continue;
          }
          continue;
        case 12:
          if (!hierarchyOrder.ContainsKey(103))
          {
            hierarchyOrder.Add(103, "No Page break before");
            continue;
          }
          continue;
        case 21:
          isBackcolorRead = true;
          this.BackgroundColorFormatting(this.GetColorName(paragraphFormat.BackColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isForecolorRead);
          continue;
        case 30:
          string str = "Tab stops:Not at ";
          for (int index = 0; index < paragraphFormat.Tabs.Count; ++index)
          {
            Tab tab = paragraphFormat.Tabs[index];
            str = $"{str}{tab.Position.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt ";
          }
          if (!hierarchyOrder.ContainsKey(114))
          {
            hierarchyOrder.Add(114, str);
            continue;
          }
          continue;
        case 31 /*0x1F*/:
          if (!hierarchyOrder.ContainsKey(104))
          {
            hierarchyOrder.Add(104, "Left-to-right");
            continue;
          }
          continue;
        case 32 /*0x20*/:
          isForecolorRead = true;
          this.ForecolorFormatting(this.GetColorName(paragraphFormat.ForeColor), hierarchyOrder, shadingFormattings, shadingFormattingText, isTextureRead, isBackcolorRead);
          continue;
        case 33:
          isTextureRead = true;
          this.TextureStyleFormatting(this.GetTextureStyleText(paragraphFormat.TextureStyle), hierarchyOrder, shadingFormattings, shadingFormattingText, isBackcolorRead, isForecolorRead);
          continue;
        case 37:
          switch (paragraphFormat.TextboxTightWrap)
          {
            case TextboxTightWrapOptions.None:
              if (!hierarchyOrder.ContainsKey((int) sbyte.MaxValue))
              {
                hierarchyOrder.Add((int) sbyte.MaxValue, "Textbox Tight Wrap: None");
                continue;
              }
              continue;
            case TextboxTightWrapOptions.AllLines:
              if (!hierarchyOrder.ContainsKey(24))
              {
                hierarchyOrder.Add(24, "Textbox Tight Wrap: Heading will be collapsed by default on open");
                continue;
              }
              continue;
            case TextboxTightWrapOptions.FirstAndLastLine:
              if (!hierarchyOrder.ContainsKey(25))
              {
                hierarchyOrder.Add(25, "Textbox Tight Wrap: All");
                continue;
              }
              continue;
            case TextboxTightWrapOptions.FirstLineOnly:
              if (!hierarchyOrder.ContainsKey(26))
              {
                hierarchyOrder.Add(26, "Textbox Tight Wrap: First and last lines");
                continue;
              }
              continue;
            case TextboxTightWrapOptions.LastLineOnly:
              if (!hierarchyOrder.ContainsKey(27))
              {
                hierarchyOrder.Add(27, "Textbox Tight Wrap: First line only");
                continue;
              }
              continue;
            default:
              continue;
          }
        case 38:
          if (!hierarchyOrder.ContainsKey(111))
          {
            hierarchyOrder.Add(111, "Don't Suppress line numbers");
            continue;
          }
          continue;
        case 39:
        case 71:
        case 72:
        case 73:
        case 74:
        case 76:
        case 77:
        case 83:
        case 84:
        case 88:
          if (!flag2)
          {
            if (!hierarchyOrder.ContainsKey(115))
              hierarchyOrder.Add(115, this.GetFrameFormattingText(newpropertyhash, paragraphFormat));
            flag2 = true;
            continue;
          }
          continue;
        case 41:
          if (!hierarchyOrder.ContainsKey(112 /*0x70*/))
          {
            hierarchyOrder.Add(112 /*0x70*/, "Allow hanging punctuation");
            continue;
          }
          continue;
        case 42:
          if (!hierarchyOrder.ContainsKey(113))
          {
            hierarchyOrder.Add(113, "Don't Compress initial punctuation");
            continue;
          }
          continue;
        case 56:
          if (!hierarchyOrder.ContainsKey(35))
          {
            hierarchyOrder.Add(35, "Outline: " + (object) paragraphFormat.OutlineLevel);
            continue;
          }
          continue;
        case 75:
          if (!hierarchyOrder.ContainsKey(110))
          {
            hierarchyOrder.Add(110, "Not Don't swap indents on facing pages");
            continue;
          }
          continue;
        case 78:
          if (!paragraphFormat.SuppressAutoHyphens && !hierarchyOrder.ContainsKey(109))
          {
            hierarchyOrder.Add(109, "Hyphenate");
            continue;
          }
          continue;
        case 81:
          if (!paragraphFormat.AutoSpaceDE && !hierarchyOrder.ContainsKey(105))
          {
            hierarchyOrder.Add(105, "Adjust space between Latin and Asian text");
            continue;
          }
          continue;
        case 82:
          if (!paragraphFormat.AutoSpaceDN && !hierarchyOrder.ContainsKey(106))
          {
            hierarchyOrder.Add(106, "Adjust space between Asian text and numbers");
            continue;
          }
          continue;
        case 92:
          if (!hierarchyOrder.ContainsKey(107))
          {
            hierarchyOrder.Add(107, "Add space between paragraphs of the same style");
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private void TextureStyleFormatting(
    string trackChangesTextureName,
    Dictionary<int, string> hierarchyOrder,
    string[] shadingFormattings,
    string shadingFormattingText,
    bool isBackcolorRead,
    bool isForecolorRead)
  {
    shadingFormattings[0] = trackChangesTextureName;
    if (isForecolorRead && isBackcolorRead)
      shadingFormattingText = this.GenerateShadingFormattingText(shadingFormattings[0], shadingFormattings[1], shadingFormattings[2]);
    if (string.IsNullOrEmpty(shadingFormattingText) || hierarchyOrder.ContainsKey(15))
      return;
    hierarchyOrder.Add(15, shadingFormattingText);
  }

  private void BackgroundColorFormatting(
    string trackChangesBackColorName,
    Dictionary<int, string> hierarchyOrder,
    string[] shadingFormattings,
    string shadingFormattingText,
    bool isTextureRead,
    bool isForecolorRead)
  {
    shadingFormattings[2] = trackChangesBackColorName;
    if (isTextureRead && isForecolorRead)
      shadingFormattingText = this.GenerateShadingFormattingText(shadingFormattings[0], shadingFormattings[1], shadingFormattings[2]);
    if (string.IsNullOrEmpty(shadingFormattingText) || hierarchyOrder.ContainsKey(14))
      return;
    hierarchyOrder.Add(14, shadingFormattingText);
  }

  private void ForecolorFormatting(
    string trackChangesForeColorName,
    Dictionary<int, string> hierarchyOrder,
    string[] shadingFormattings,
    string shadingFormattingText,
    bool isTextureRead,
    bool isBackcolorRead)
  {
    shadingFormattings[1] = trackChangesForeColorName;
    if (isTextureRead && isBackcolorRead)
      shadingFormattingText = this.GenerateShadingFormattingText(shadingFormattings[0], shadingFormattings[1], shadingFormattings[2]);
    if (string.IsNullOrEmpty(shadingFormattingText) || hierarchyOrder.ContainsKey(13))
      return;
    hierarchyOrder.Add(13, shadingFormattingText);
  }

  private string GetBorderChangesText(WParagraphFormat paragraphFormat)
  {
    List<string> stringList = new List<string>();
    string borderChangesText = "";
    if ((double) paragraphFormat.Borders.Top.LineWidth > 0.0)
      stringList.Add($"Top:({(object) paragraphFormat.Borders.Top.BorderType},{this.GetColorName(paragraphFormat.Borders.Top.Color)}, {paragraphFormat.Borders.Top.LineWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt Line Width)");
    if ((double) paragraphFormat.Borders.Bottom.LineWidth > 0.0)
      stringList.Add($"Bottom:({(object) paragraphFormat.Borders.Bottom.BorderType}, {this.GetColorName(paragraphFormat.Borders.Top.Color)}, {paragraphFormat.Borders.Bottom.LineWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt Line Width)");
    if ((double) paragraphFormat.Borders.Left.LineWidth > 0.0)
      stringList.Add($"Left:({(object) paragraphFormat.Borders.Left.BorderType}, {this.GetColorName(paragraphFormat.Borders.Top.Color)}, {paragraphFormat.Borders.Left.LineWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt Line Width)");
    if ((double) paragraphFormat.Borders.Right.LineWidth > 0.0)
      stringList.Add($"Right:({(object) paragraphFormat.Borders.Right.BorderType}, {this.GetColorName(paragraphFormat.Borders.Top.Color)}, {paragraphFormat.Borders.Right.LineWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt Line Width)");
    if ((double) paragraphFormat.Borders.Vertical.LineWidth > 0.0)
      stringList.Add($"Bar:({(object) paragraphFormat.Borders.Vertical.BorderType}, {this.GetColorName(paragraphFormat.Borders.Top.Color)}, {paragraphFormat.Borders.Vertical.LineWidth.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt Line Width)");
    if (stringList.Count > 1)
    {
      for (int index = 0; index < stringList.Count - 1; ++index)
        borderChangesText = $"{borderChangesText}{stringList[index]}, ";
      return borderChangesText;
    }
    return stringList.Count != 0 ? stringList[0] : "";
  }

  private string GetSpacingChangesText(
    Dictionary<int, object> newpropertyhash,
    WParagraphFormat paragraphFormat)
  {
    List<string> stringList = new List<string>();
    if (newpropertyhash.ContainsKey(54))
      stringList.Add($"Before Auto: {paragraphFormat.SpaceBeforeAuto.ToString()} pt");
    else if (newpropertyhash.ContainsKey(8))
      stringList.Add($"Before: {paragraphFormat.BeforeSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
    else if (newpropertyhash.ContainsKey(90))
      stringList.Add($"Before: {paragraphFormat.BeforeLines.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
    if (newpropertyhash.ContainsKey(55))
      stringList.Add($"Auto After: {paragraphFormat.SpaceAfterAuto.ToString()} pt");
    else if (newpropertyhash.ContainsKey(9))
      stringList.Add($"After: {paragraphFormat.AfterSpacing.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
    else if (newpropertyhash.ContainsKey(91))
      stringList.Add($"After: {paragraphFormat.AfterLines.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt");
    return stringList.Count < 2 ? stringList[0] : $"{stringList[0]},{stringList[1]}";
  }

  private string GetFrameFormattingText(
    Dictionary<int, object> newpropertyhash,
    WParagraphFormat paragraphFormat)
  {
    string frameFormattingText = "";
    string[] strArray1 = new string[7];
    if (newpropertyhash.ContainsKey(71) || newpropertyhash.ContainsKey(72) || newpropertyhash.ContainsKey(73) || newpropertyhash.ContainsKey(74) || newpropertyhash.ContainsKey(83) || newpropertyhash.ContainsKey(84))
    {
      strArray1[0] = "Position: ";
      if (newpropertyhash.ContainsKey(71) || newpropertyhash.ContainsKey(72))
      {
        string[] strArray2;
        string str = $"{(strArray2 = strArray1)[0]}Horizontal: {this.GetFrameXFormattedText(paragraphFormat.FrameX)}, Relative to:{((FrameHorzAnchor) paragraphFormat.FrameHorizontalPos).ToString()}, Vertical: {this.GetFrameYFormattedText(paragraphFormat.FrameY)}, Relative to:{((FrameVertAnchor) paragraphFormat.FrameVerticalPos).ToString()}";
        strArray2[0] = str;
      }
      if (newpropertyhash.ContainsKey(83))
        strArray1[1] = $"Horizontal: {(object) paragraphFormat.FrameHorizontalDistanceFromText} pt";
      if (newpropertyhash.ContainsKey(84))
        strArray1[2] = $"Vertical: {(object) paragraphFormat.FrameVerticalDistanceFromText} pt";
    }
    if (newpropertyhash.ContainsKey(76) && (double) paragraphFormat.FrameWidth != 0.0)
      strArray1[3] = $"Width: Exactly {(object) paragraphFormat.FrameWidth} pt";
    if (newpropertyhash.ContainsKey(77))
    {
      bool flag = ((int) (ushort) Math.Round((double) paragraphFormat.FrameHeight * 20.0) & 32768 /*0x8000*/) != 0;
      string str = flag ? (!(flag & (double) paragraphFormat.FrameHeight == 0.0) ? "At least " : "") : "Exact ";
      strArray1[4] = !string.IsNullOrEmpty(str) ? $"Height: {str}{paragraphFormat.FrameHeight.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt" : "";
    }
    if (newpropertyhash.ContainsKey(88))
      strArray1[5] = this.GetFrameWrappingFormmattedText(paragraphFormat.WrapFrameAround);
    if (newpropertyhash.ContainsKey(39))
      strArray1[6] = paragraphFormat.LockFrameAnchor ? "Lock Anchor" : "";
    for (int index = 0; index < strArray1.Length; ++index)
      frameFormattingText += index == strArray1.Length - 1 ? (!string.IsNullOrEmpty(strArray1[index]) ? strArray1[index] + ", " : "") : strArray1[index];
    return frameFormattingText;
  }

  private string GetFrameWrappingFormmattedText(FrameWrapMode wrapFrameAround)
  {
    string wrappingFormmattedText = "";
    switch (wrapFrameAround)
    {
      case FrameWrapMode.NotBeside:
        wrappingFormmattedText = "No Wrapping";
        goto case FrameWrapMode.Around;
      case FrameWrapMode.Around:
      case FrameWrapMode.Tight:
      case FrameWrapMode.Through:
        return wrappingFormmattedText;
      default:
        wrappingFormmattedText = "";
        goto case FrameWrapMode.Around;
    }
  }

  private string GetFrameYFormattedText(float frameY)
  {
    return (double) frameY != -4.0 && (double) frameY != -12.0 && (double) frameY != -8.0 && (double) frameY != -16.0 && (double) frameY != -20.0 && (double) frameY != 0.0 ? frameY.ToString((IFormatProvider) CultureInfo.InvariantCulture) : ((FrameVerticalPosition) frameY).ToString();
  }

  private string GetFrameXFormattedText(float frameX)
  {
    return (double) frameX != -8.0 && (double) frameX != -4.0 && (double) frameX != -12.0 && (double) frameX != -16.0 && (double) frameX != 0.0 ? frameX.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " pt" : ((PageNumberAlignment) frameX).ToString();
  }

  private string GenerateIndentsFormattingText(
    Dictionary<int, object> newpropertyhash,
    WParagraphFormat paragraphFormat)
  {
    string[] strArray = new string[6];
    string indentsFormattingText = "";
    if (newpropertyhash.ContainsKey(2) && newpropertyhash.ContainsKey(5) && (double) paragraphFormat.FirstLineIndent < 0.0)
      strArray[0] = $"Before: {(-1f * paragraphFormat.FirstLineIndent + paragraphFormat.LeftIndent).ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    else if (newpropertyhash.ContainsKey(2))
      strArray[0] = $"Left: {paragraphFormat.LeftIndent.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    if (newpropertyhash.ContainsKey(5))
      strArray[1] = (double) paragraphFormat.FirstLineIndent < 0.0 ? "Hanging " : $"First line: {paragraphFormat.FirstLineIndent.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    if (newpropertyhash.ContainsKey(3))
      strArray[2] = $"Right: {paragraphFormat.RightIndent.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    if (newpropertyhash.ContainsKey(85))
      strArray[3] = $"Left {paragraphFormat.LeftIndentChars.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    if (newpropertyhash.ContainsKey(86))
      strArray[4] = $"First line:{paragraphFormat.FirstLineIndentChars.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    if (newpropertyhash.ContainsKey(87))
      strArray[5] = $"Right {paragraphFormat.RightIndentChars.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (!string.IsNullOrEmpty(strArray[index]))
        indentsFormattingText += index == strArray.Length - 1 ? strArray[index] : strArray[index] + ",";
    }
    return indentsFormattingText;
  }

  private string GetTabAlignmentText(Syncfusion.DocIO.DLS.TabJustification justification)
  {
    switch (justification)
    {
      case Syncfusion.DocIO.DLS.TabJustification.Left:
      case Syncfusion.DocIO.DLS.TabJustification.Centered:
      case Syncfusion.DocIO.DLS.TabJustification.Right:
      case Syncfusion.DocIO.DLS.TabJustification.Bar:
        justification.ToString();
        break;
    }
    return "";
  }

  internal string DisplayBalloonValueForListFormat(
    Dictionary<int, object> newpropertyhash,
    WListFormat listFormat)
  {
    string[] strArray = new string[7];
    WListLevel currentListLevel = listFormat.CurrentListLevel;
    ListStyle currentListStyle = listFormat.CurrentListStyle;
    string str = "";
    if (listFormat.ListType == ListType.Numbered && !currentListStyle.IsHybrid)
      strArray[0] = "Numbered";
    else if (listFormat.ListType == ListType.Numbered)
      strArray[0] = "Outline Numbered";
    else if (listFormat.ListType == ListType.Bulleted)
      strArray[0] = "Bulleted";
    strArray[1] = "Level: " + (listFormat.ListLevelNumber + 1).ToString();
    if (listFormat.ListType == ListType.Numbered)
    {
      strArray[2] = this.GetListPatternFormattedText(currentListLevel.PatternType);
      strArray[3] = "Start at: " + (currentListLevel.StartAt + 1).ToString();
    }
    strArray[4] = "Alignment: " + currentListLevel.NumberAlignment.ToString();
    strArray[5] = $"Aliged at: {(currentListLevel.TextPosition + currentListLevel.NumberPosition).ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    strArray[6] = $"Indent at: {currentListLevel.TextPosition.ToString((IFormatProvider) CultureInfo.InvariantCulture)} pt";
    for (int index = 0; index < strArray.Length; ++index)
      str += index != strArray.Length - 1 ? (!string.IsNullOrEmpty(strArray[index]) ? strArray[index] + "+ " : "") : strArray[index];
    return "List Paragraph, " + str;
  }

  private string GetListPatternFormattedText(ListPatternType patternType)
  {
    string str = "";
    switch (patternType)
    {
      case ListPatternType.Arabic:
        str = "1, 2, 3, ...";
        break;
      case ListPatternType.UpRoman:
        str = "I, II, III, ...";
        break;
      case ListPatternType.LowRoman:
        str = "i, ii, iii, ...";
        break;
      case ListPatternType.UpLetter:
        str = "A, B, C, ...";
        break;
      case ListPatternType.LowLetter:
        str = "a, b, c, ...";
        break;
      case ListPatternType.Ordinal:
        str += "1st, 2nd, 3rd, ...";
        break;
      case ListPatternType.Number:
        str = "One, Two, Three, ...";
        break;
      case ListPatternType.OrdinalText:
        str = " First, Second, Third, ...";
        break;
      case ListPatternType.LeadingZero:
        str = "01, 02, 03, ...";
        break;
    }
    return string.IsNullOrEmpty(str) ? "" : "Numbering Style: " + str;
  }

  internal void AppendInDeletionBalloon(WTextRange textRange)
  {
    this.ChangedValue.LastParagraph.AppendText(textRange.Text).ApplyCharacterFormat(textRange.CharacterFormat);
    (this.ChangedValue.LastParagraph.Items.LastItem as WTextRange).CharacterFormat.FontSize = 10f;
    (this.ChangedValue.LastParagraph.Items.LastItem as WTextRange).CharacterFormat.IsDeleteRevision = false;
  }

  internal string ConvertDictionaryValuesToString(Dictionary<int, string> hierarchyOrder)
  {
    StringBuilder stringBuilder = new StringBuilder();
    bool flag1 = false;
    bool flag2 = false;
    int count = hierarchyOrder.Count;
    for (int key = 0; key < 150; ++key)
    {
      if (hierarchyOrder.ContainsKey(key))
      {
        if (!flag1 && key >= 4 && key <= 6)
        {
          stringBuilder.Append("Font: ");
          flag1 = true;
        }
        if (!flag2 && (key == 7 || key == 8 || key == 1 || key == 3))
        {
          if (!string.IsNullOrEmpty(stringBuilder.ToString()))
            stringBuilder.Append("Complex Script Font: ");
          else
            stringBuilder.Append("Complex Script Font: ");
          flag2 = true;
        }
        string str = "";
        hierarchyOrder.TryGetValue(key, out str);
        if (!string.IsNullOrEmpty(str))
        {
          if (count != 1)
          {
            stringBuilder.Append($" {str},");
            --count;
          }
          else
            stringBuilder.Append(" " + str);
        }
      }
    }
    return stringBuilder.ToString();
  }
}
