// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BerTagHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BerTagHelper : IAsn1Tag, IAsn1
{
  private bool m_isConstructed;
  private int m_tagNumber;
  private Asn1Parser m_helper;

  internal bool Constructed => this.m_isConstructed;

  public int TagNumber => this.m_tagNumber;

  internal BerTagHelper(bool isConstructed, int tagNumber, Asn1Parser helper)
  {
    this.m_isConstructed = isConstructed;
    this.m_tagNumber = tagNumber;
    this.m_helper = helper;
  }

  public IAsn1 GetParser(int tagNumber, bool isExplicit)
  {
    if (!isExplicit)
      return this.m_helper.ReadImplicit(this.m_isConstructed, tagNumber);
    if (!this.m_isConstructed)
      throw new IOException("Implicit tags identified");
    return this.m_helper.ReadObject();
  }

  public Asn1 GetAsn1()
  {
    try
    {
      return this.m_helper.ReadTaggedObject(this.m_isConstructed, this.m_tagNumber);
    }
    catch (IOException ex)
    {
      throw new Exception(ex.Message);
    }
  }
}
