// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.TemplateParser
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.MessageTemplates;

internal static class TemplateParser
{
  public static Template Parse(string template)
  {
    if (template == null)
      throw new ArgumentNullException(nameof (template));
    bool isPositional = true;
    List<Literal> literals = new List<Literal>();
    List<Hole> holes = new List<Hole>();
    TemplateEnumerator templateEnumerator = new TemplateEnumerator(template);
    while (templateEnumerator.MoveNext())
    {
      if (templateEnumerator.Current.Literal.Skip == 0)
      {
        literals.Add(templateEnumerator.Current.Literal);
      }
      else
      {
        literals.Add(templateEnumerator.Current.Literal);
        holes.Add(templateEnumerator.Current.Hole);
        if (templateEnumerator.Current.Hole.Index == (short) -1)
          isPositional = false;
      }
    }
    return new Template(template, isPositional, literals, holes);
  }
}
