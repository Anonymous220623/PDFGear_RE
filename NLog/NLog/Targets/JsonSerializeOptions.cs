// Decompiled with JetBrains decompiler
// Type: NLog.Targets.JsonSerializeOptions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets;

public class JsonSerializeOptions
{
  [DefaultValue(true)]
  public bool QuoteKeys { get; set; }

  public IFormatProvider FormatProvider { get; set; }

  public string Format { get; set; }

  [DefaultValue(false)]
  public bool EscapeUnicode { get; set; }

  [DefaultValue(true)]
  public bool EscapeForwardSlash { get; set; } = true;

  [DefaultValue(false)]
  public bool EnumAsInteger { get; set; }

  [DefaultValue(false)]
  public bool SanitizeDictionaryKeys { get; set; }

  [DefaultValue(10)]
  public int MaxRecursionLimit { get; set; }

  public JsonSerializeOptions()
  {
    this.QuoteKeys = true;
    this.MaxRecursionLimit = 10;
  }
}
