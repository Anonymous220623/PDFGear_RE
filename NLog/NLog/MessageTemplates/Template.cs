// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.Template
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.MessageTemplates;

internal class Template
{
  public string Value { get; }

  public Literal[] Literals { get; }

  public Hole[] Holes { get; }

  public bool IsPositional { get; }

  public Template(string template, bool isPositional, List<Literal> literals, List<Hole> holes)
  {
    this.Value = template;
    this.IsPositional = isPositional;
    this.Literals = literals.ToArray();
    this.Holes = holes.ToArray();
  }

  public Template(string template, bool isPositional, Literal[] literals, Hole[] holes)
  {
    this.Value = template;
    this.IsPositional = isPositional;
    this.Literals = literals;
    this.Holes = holes;
  }

  public string Rebuild()
  {
    StringBuilder sb = new StringBuilder(this.Value.Length);
    int startIndex = 0;
    int num = 0;
    foreach (Literal literal in this.Literals)
    {
      sb.Append(this.Value, startIndex, literal.Print);
      startIndex += literal.Print;
      if (literal.Skip == 0)
      {
        if (startIndex < this.Value.Length)
          sb.Append(this.Value[startIndex++]);
      }
      else
      {
        startIndex += literal.Skip;
        Template.RebuildHole(sb, ref this.Holes[num++]);
      }
    }
    return sb.ToString();
  }

  private static void RebuildHole(StringBuilder sb, ref Hole hole)
  {
    if (hole.CaptureType == CaptureType.Normal)
      sb.Append('{');
    else if (hole.CaptureType == CaptureType.Serialize)
      sb.Append("{@");
    else
      sb.Append("{$");
    sb.Append(hole.Name);
    if (hole.Alignment != (short) 0)
      sb.Append(',').Append(hole.Alignment);
    if (hole.Format != null)
      sb.Append(':').Append(hole.Format.Replace("{", "{{").Replace("}", "}}"));
    sb.Append('}');
  }
}
