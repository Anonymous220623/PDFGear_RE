// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.LicenseInfo
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;

#nullable disable
namespace CommomLib.Commom;

public class LicenseInfo
{
  public DateTimeOffset acquiredDate { get; set; }

  public string acquisitionType { get; set; }

  public DateTimeOffset endDate { get; set; }

  public string id { get; set; }

  public string legacyOfferInstanceId { get; set; }

  public string legacyProductId { get; set; }

  public string localTicketReference { get; set; }

  public string modifiedDate { get; set; }

  public string purchasedCountry { get; set; }

  public string productFamily { get; set; }

  public string productId { get; set; }

  public string productKind { get; set; }

  public string sharingSource { get; set; }

  public string skuId { get; set; }

  public string startDate { get; set; }

  public string status { get; set; }

  public string devOfferId { get; set; }

  public int quantity { get; set; }

  public string transactionId { get; set; }
}
