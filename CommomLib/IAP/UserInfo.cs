// Decompiled with JetBrains decompiler
// Type: CommomLib.IAP.UserInfo
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;

#nullable disable
namespace CommomLib.IAP;

public class UserInfo
{
  public string Id { get; set; }

  public string Email { get; set; }

  public string RoleName { get; set; }

  public int RoleWeight { get; set; }

  public bool Premium { get; set; }

  public DateTime? ExpireTime { get; set; }

  public bool IsSubscription { get; set; }
}
