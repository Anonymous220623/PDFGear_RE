// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.JsonAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.LayoutRenderers.Wrappers;
using NLog.Targets;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[NLogConfigurationItem]
public class JsonAttribute
{
  private string _name;
  internal readonly JsonEncodeLayoutRendererWrapper LayoutWrapper = new JsonEncodeLayoutRendererWrapper();

  public JsonAttribute()
    : this((string) null, (Layout) null, true)
  {
  }

  public JsonAttribute(string name, Layout layout)
    : this(name, layout, true)
  {
  }

  public JsonAttribute(string name, Layout layout, bool encode)
  {
    this.Name = name;
    this.Layout = layout;
    this.Encode = encode;
    this.IncludeEmptyValue = false;
  }

  [RequiredParameter]
  public string Name
  {
    get => this._name;
    set
    {
      if (string.IsNullOrEmpty(value))
        this._name = value;
      else if (value.All<char>((Func<char, bool>) (chr => char.IsLetterOrDigit(chr))))
      {
        this._name = value;
      }
      else
      {
        StringBuilder destination = new StringBuilder();
        DefaultJsonSerializer.AppendStringEscape(destination, value, false, false);
        this._name = destination.ToString();
      }
    }
  }

  [RequiredParameter]
  public Layout Layout
  {
    get => this.LayoutWrapper.Inner;
    set => this.LayoutWrapper.Inner = value;
  }

  public bool Encode
  {
    get => this.LayoutWrapper.JsonEncode;
    set => this.LayoutWrapper.JsonEncode = value;
  }

  public bool EscapeUnicode
  {
    get => this.LayoutWrapper.EscapeUnicode;
    set => this.LayoutWrapper.EscapeUnicode = value;
  }

  [DefaultValue(true)]
  public bool EscapeForwardSlash
  {
    get => this.LayoutWrapper.EscapeForwardSlash;
    set => this.LayoutWrapper.EscapeForwardSlash = value;
  }

  public bool IncludeEmptyValue { get; set; }
}
