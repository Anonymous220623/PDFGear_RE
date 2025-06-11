// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampTokenInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampTokenInformation
{
  private TimeStampData m_timeStampData;
  private DateTime m_generalizedTime;

  internal TimeStampTokenInformation(TimeStampData timeStampData)
  {
    this.m_timeStampData = timeStampData;
    try
    {
      this.m_generalizedTime = timeStampData.GeneralizedTime.ToDateTime();
    }
    catch (Exception ex)
    {
      throw new Exception("Invalid entry");
    }
  }

  internal DateTime GeneralizedTime => this.m_generalizedTime;

  internal string Policy => this.m_timeStampData.Policy.ID;

  internal string MessageImprintAlgOid => this.m_timeStampData.MessageImprint.HashAlgorithm;

  internal TimeStampData TimeStampData => this.m_timeStampData;
}
