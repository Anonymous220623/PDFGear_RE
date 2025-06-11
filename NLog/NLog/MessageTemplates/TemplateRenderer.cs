// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.TemplateRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.MessageTemplates;

internal static class TemplateRenderer
{
  public static void Render(
    this string template,
    IFormatProvider formatProvider,
    object[] parameters,
    bool forceTemplateRenderer,
    StringBuilder sb,
    out IList<MessageTemplateParameter> messageTemplateParameters)
  {
    int startIndex = 0;
    int length1 = 0;
    int holeStartPosition = 0;
    messageTemplateParameters = (IList<MessageTemplateParameter>) null;
    int length2 = sb.Length;
    TemplateEnumerator templateEnumerator = new TemplateEnumerator(template);
    while (templateEnumerator.MoveNext())
    {
      if (length1 == 0 && !forceTemplateRenderer && templateEnumerator.Current.MaybePositionalTemplate && sb.Length == length2)
      {
        sb.AppendFormat(formatProvider, template, parameters);
        return;
      }
      Literal literal = templateEnumerator.Current.Literal;
      sb.Append(template, startIndex, literal.Print);
      int num = startIndex + literal.Print;
      if (literal.Skip == 0)
      {
        startIndex = num + 1;
      }
      else
      {
        startIndex = num + literal.Skip;
        Hole hole = templateEnumerator.Current.Hole;
        if (hole.Alignment != (short) 0)
          holeStartPosition = sb.Length;
        if (hole.Index != (short) -1 && messageTemplateParameters == null)
        {
          ++length1;
          TemplateRenderer.RenderHole(sb, hole, formatProvider, parameters[(int) hole.Index], true);
        }
        else
        {
          object parameter = parameters[length1];
          if (messageTemplateParameters == null)
          {
            messageTemplateParameters = (IList<MessageTemplateParameter>) new MessageTemplateParameter[parameters.Length];
            if (length1 != 0)
            {
              templateEnumerator = new TemplateEnumerator(template);
              sb.Length = length2;
              length1 = 0;
              startIndex = 0;
              continue;
            }
          }
          messageTemplateParameters[length1++] = new MessageTemplateParameter(hole.Name, parameter, hole.Format, hole.CaptureType);
          TemplateRenderer.RenderHole(sb, hole, formatProvider, parameter);
        }
        if (hole.Alignment != (short) 0)
          TemplateRenderer.RenderPadding(sb, (int) hole.Alignment, holeStartPosition);
      }
    }
    if (messageTemplateParameters == null || length1 == messageTemplateParameters.Count)
      return;
    MessageTemplateParameter[] templateParameterArray = new MessageTemplateParameter[length1];
    for (int index = 0; index < templateParameterArray.Length; ++index)
      templateParameterArray[index] = messageTemplateParameters[index];
    messageTemplateParameters = (IList<MessageTemplateParameter>) templateParameterArray;
  }

  public static void Render(
    this Template template,
    StringBuilder sb,
    IFormatProvider formatProvider,
    object[] parameters)
  {
    int startIndex = 0;
    int index = 0;
    int holeStartPosition = 0;
    foreach (Literal literal in template.Literals)
    {
      sb.Append(template.Value, startIndex, literal.Print);
      int num = startIndex + literal.Print;
      if (literal.Skip == 0)
      {
        startIndex = num + 1;
      }
      else
      {
        startIndex = num + literal.Skip;
        Hole hole = template.Holes[index];
        if (hole.Alignment != (short) 0)
          holeStartPosition = sb.Length;
        object obj = template.IsPositional ? parameters[(int) hole.Index] : parameters[index];
        ++index;
        TemplateRenderer.RenderHole(sb, hole, formatProvider, obj, template.IsPositional);
        if (hole.Alignment != (short) 0)
          TemplateRenderer.RenderPadding(sb, (int) hole.Alignment, holeStartPosition);
      }
    }
  }

  private static void RenderHole(
    StringBuilder sb,
    Hole hole,
    IFormatProvider formatProvider,
    object value,
    bool legacy = false)
  {
    TemplateRenderer.RenderHole(sb, hole.CaptureType, hole.Format, formatProvider, value, legacy);
  }

  public static void RenderHole(
    StringBuilder sb,
    CaptureType captureType,
    string holeFormat,
    IFormatProvider formatProvider,
    object value,
    bool legacy = false)
  {
    if (value == null)
      sb.Append("NULL");
    else if (captureType == CaptureType.Normal & legacy)
      ValueFormatter.FormatToString(value, holeFormat, formatProvider, sb);
    else
      ValueFormatter.Instance.FormatValue(value, holeFormat, captureType, formatProvider, sb);
  }

  private static void RenderPadding(StringBuilder sb, int holeAlignment, int holeStartPosition)
  {
    int length = sb.Length - holeStartPosition;
    int repeatCount = Math.Abs(holeAlignment) - length;
    if (repeatCount <= 0)
      return;
    if (holeAlignment < 0 || length == 0)
    {
      sb.Append(' ', repeatCount);
    }
    else
    {
      string str = sb.ToString(holeStartPosition, length);
      sb.Length = holeStartPosition;
      sb.Append(' ', repeatCount);
      sb.Append(str);
    }
  }
}
