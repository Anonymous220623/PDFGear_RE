// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Exceptions.XmlReadingException
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Exceptions;

[Serializable]
internal class XmlReadingException : ApplicationException
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
