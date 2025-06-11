// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Exceptions.XmlReadingException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Exceptions;

[Serializable]
public class XmlReadingException : ApplicationException
{
  private const string DEF_ERROR = "Some problem occured during parse.";

  public XmlReadingException()
    : base("Some problem occured during parse.")
  {
  }

  public XmlReadingException(string message)
    : base("Some problem occured during parse.. Error message: " + message)
  {
  }

  public XmlReadingException(string strBlock, string strDescription)
    : base($"Exception occured in {strBlock} of xml structure. Error message: {strDescription}")
  {
  }
}
