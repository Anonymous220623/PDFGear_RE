// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorFamily
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public class ColorFamily : 
  IReadOnlyList<ColorValue>,
  IReadOnlyCollection<ColorValue>,
  IEnumerable<ColorValue>,
  IEnumerable
{
  private List<ColorValue> colorValues;

  public ColorFamily(ColorValue header, IEnumerable<ColorValue> colorValues)
  {
    this.colorValues = colorValues.ToList<ColorValue>();
    this.Header = header;
  }

  public ColorValue Header { get; }

  public ColorValue this[int index] => this.colorValues[index];

  public int Count => this.colorValues.Count;

  public IEnumerator<ColorValue> GetEnumerator()
  {
    return ((IEnumerable<ColorValue>) this.colorValues).GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.colorValues).GetEnumerator();
}
