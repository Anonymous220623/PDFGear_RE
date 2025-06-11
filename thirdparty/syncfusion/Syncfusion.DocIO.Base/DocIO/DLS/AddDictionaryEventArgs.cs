// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.AddDictionaryEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class AddDictionaryEventArgs : EventArgs
{
  private string m_LanguageCode;
  private Stream m_DictionaryStream;

  public string LanguageCode => this.m_LanguageCode;

  public Stream DictionaryStream
  {
    get => this.m_DictionaryStream;
    set => this.m_DictionaryStream = value;
  }

  internal AddDictionaryEventArgs(string orignalLanguagecode, string alternateLanguagecode)
  {
    this.m_LanguageCode = orignalLanguagecode;
  }
}
