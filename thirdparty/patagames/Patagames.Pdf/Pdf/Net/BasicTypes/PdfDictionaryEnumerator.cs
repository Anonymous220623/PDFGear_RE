// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfDictionaryEnumerator
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

internal class PdfDictionaryEnumerator : 
  IEnumerator<KeyValuePair<string, PdfTypeBase>>,
  IDisposable,
  IEnumerator
{
  private PdfTypeDictionary _dict;
  private ICollection<string> _keys;
  private IEnumerator<string> _it;

  public PdfDictionaryEnumerator(PdfTypeDictionary dictionary)
  {
    this._keys = dictionary.Keys;
    this._dict = dictionary;
    this._it = this._keys.GetEnumerator();
  }

  public KeyValuePair<string, PdfTypeBase> Current
  {
    get
    {
      return this._it.Current == null ? new KeyValuePair<string, PdfTypeBase>() : new KeyValuePair<string, PdfTypeBase>(this._it.Current, this._dict[this._it.Current]);
    }
  }

  object IEnumerator.Current => (object) this.Current;

  public void Dispose()
  {
  }

  public bool MoveNext() => this._it.MoveNext();

  public void Reset() => this._it.Reset();
}
