// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XPath.XmpPath
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace XmpCore.Impl.XPath;

public sealed class XmpPath
{
  public const int StepSchema = 0;
  public const int StepRootProp = 1;
  private readonly List<XmpPathSegment> _segments = new List<XmpPathSegment>(5);

  public void Add(XmpPathSegment segment) => this._segments.Add(segment);

  public XmpPathSegment GetSegment(int index) => this._segments[index];

  public int Size() => this._segments.Count;

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 1; index < this.Size(); ++index)
    {
      stringBuilder.Append((object) this.GetSegment(index));
      if (index < this.Size() - 1)
      {
        switch (this.GetSegment(index + 1).Kind)
        {
          case XmpPathStepType.StructFieldStep:
          case XmpPathStepType.QualifierStep:
            stringBuilder.Append('/');
            continue;
          default:
            continue;
        }
      }
    }
    return stringBuilder.ToString();
  }
}
