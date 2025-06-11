// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.QName
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Impl;

public sealed class QName
{
  public QName(string qname)
  {
    int length = qname.IndexOf(':');
    if (length >= 0)
    {
      this.Prefix = qname.Substring(0, length);
      this.LocalName = qname.Substring(length + 1);
    }
    else
    {
      this.Prefix = string.Empty;
      this.LocalName = qname;
    }
  }

  public QName(string prefix, string localName)
  {
    this.Prefix = prefix;
    this.LocalName = localName;
  }

  public bool HasPrefix => !string.IsNullOrEmpty(this.Prefix);

  public string LocalName { get; }

  public string Prefix { get; }
}
