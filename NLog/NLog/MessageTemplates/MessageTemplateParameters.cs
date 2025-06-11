// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.MessageTemplateParameters
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NLog.MessageTemplates;

public sealed class MessageTemplateParameters : IEnumerable<MessageTemplateParameter>, IEnumerable
{
  internal static readonly MessageTemplateParameters Empty = new MessageTemplateParameters(string.Empty, ArrayHelper.Empty<object>());
  private readonly IList<MessageTemplateParameter> _parameters;

  public IEnumerator<MessageTemplateParameter> GetEnumerator() => this._parameters.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._parameters.GetEnumerator();

  public MessageTemplateParameter this[int index] => this._parameters[index];

  public int Count => this._parameters.Count;

  public bool IsPositional { get; }

  internal bool IsValidTemplate { get; }

  internal MessageTemplateParameters(string message, object[] parameters)
  {
    bool flag = parameters != null && parameters.Length != 0;
    bool isPositional = flag;
    bool isValidTemplate = !flag;
    this._parameters = flag ? MessageTemplateParameters.ParseMessageTemplate(message, parameters, out isPositional, out isValidTemplate) : (IList<MessageTemplateParameter>) ArrayHelper.Empty<MessageTemplateParameter>();
    this.IsPositional = isPositional;
    this.IsValidTemplate = isValidTemplate;
  }

  internal MessageTemplateParameters(
    IList<MessageTemplateParameter> templateParameters,
    string message,
    object[] parameters)
  {
    this._parameters = (IList<MessageTemplateParameter>) ((object) templateParameters ?? (object) ArrayHelper.Empty<MessageTemplateParameter>());
    if (parameters == null || this._parameters.Count == parameters.Length)
      return;
    this.IsValidTemplate = false;
  }

  private static IList<MessageTemplateParameter> ParseMessageTemplate(
    string template,
    object[] parameters,
    out bool isPositional,
    out bool isValidTemplate)
  {
    isPositional = true;
    isValidTemplate = true;
    List<MessageTemplateParameter> messageTemplate = new List<MessageTemplateParameter>(parameters.Length);
    try
    {
      short num = 0;
      TemplateEnumerator templateEnumerator = new TemplateEnumerator(template);
      while (templateEnumerator.MoveNext())
      {
        if (templateEnumerator.Current.Literal.Skip != 0)
        {
          Hole hole = templateEnumerator.Current.Hole;
          if (hole.Index != (short) -1 & isPositional)
          {
            num = MessageTemplateParameters.GetMaxHoleIndex(num, hole.Index);
            object holeValueSafe = MessageTemplateParameters.GetHoleValueSafe(parameters, hole.Index, ref isValidTemplate);
            messageTemplate.Add(new MessageTemplateParameter(hole.Name, holeValueSafe, hole.Format, hole.CaptureType));
          }
          else
          {
            if (isPositional)
            {
              isPositional = false;
              if (num != (short) 0)
              {
                templateEnumerator = new TemplateEnumerator(template);
                num = (short) 0;
                messageTemplate.Clear();
                continue;
              }
            }
            object holeValueSafe = MessageTemplateParameters.GetHoleValueSafe(parameters, num, ref isValidTemplate);
            messageTemplate.Add(new MessageTemplateParameter(hole.Name, holeValueSafe, hole.Format, hole.CaptureType));
            ++num;
          }
        }
      }
      if (isPositional)
      {
        if (messageTemplate.Count < parameters.Length || (int) num != parameters.Length)
          isValidTemplate = false;
      }
      else if (messageTemplate.Count != parameters.Length)
        isValidTemplate = false;
      return (IList<MessageTemplateParameter>) messageTemplate;
    }
    catch (Exception ex)
    {
      isValidTemplate = false;
      InternalLogger.Warn(ex, "Error when parsing a message.");
      return (IList<MessageTemplateParameter>) messageTemplate;
    }
  }

  private static short GetMaxHoleIndex(short maxHoleIndex, short holeIndex)
  {
    if (maxHoleIndex == (short) 0)
      ++maxHoleIndex;
    if ((int) maxHoleIndex <= (int) holeIndex)
    {
      maxHoleIndex = holeIndex;
      ++maxHoleIndex;
    }
    return maxHoleIndex;
  }

  private static object GetHoleValueSafe(
    object[] parameters,
    short holeIndex,
    ref bool isValidTemplate)
  {
    if (parameters.Length > (int) holeIndex)
      return parameters[(int) holeIndex];
    isValidTemplate = false;
    return (object) null;
  }
}
