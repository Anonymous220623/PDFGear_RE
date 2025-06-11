// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Protection.EncryptManage
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace pdfeditor.Models.Protection;

public class EncryptManage
{
  private bool isHaveUserPassword;
  private bool isChangedPassword;
  public bool IsHaveOwerPassword;
  private bool isRequiredOwerPassword;

  public void Init()
  {
    this.IsRemoveAllPassword = false;
    this.IsHaveUserPassword = false;
    this.IsChangedPassword = false;
    this.UserPassword = string.Empty;
    this.OwerPassword = string.Empty;
  }

  public bool NotInputExistOwerpwd => this.IsHaveOwerPassword && this.IsRequiredOwerPassword;

  public bool IsRemoveAllPassword { get; private set; }

  public string UserPassword { get; private set; }

  public string OwerPassword { get; private set; }

  public void UpdateUserPassword(string pwd) => this.UserPassword = pwd;

  public void UpdateOwerPassword(string pwd)
  {
    this.IsRequiredOwerPassword = false;
    this.OwerPassword = pwd;
  }

  public void SetPassword(string userpassword, string owerpassword = "")
  {
    if (string.IsNullOrWhiteSpace(owerpassword))
      owerpassword = userpassword;
    this.UserPassword = userpassword;
    this.OwerPassword = owerpassword;
    this.IsRemoveAllPassword = false;
    this.IsRequiredOwerPassword = false;
    this.IsHaveUserPassword = true;
    this.IsHaveOwerPassword = true;
    this.IsChangedPassword = true;
  }

  public void RemoveAllPassword()
  {
    this.IsRemoveAllPassword = true;
    this.UserPassword = string.Empty;
    this.OwerPassword = string.Empty;
  }

  public bool IsHaveUserPassword
  {
    get => this.isHaveUserPassword;
    set
    {
      if (!value)
        this.IsHaveOwerPassword = false;
      this.isHaveUserPassword = value;
    }
  }

  public bool IsChangedPassword
  {
    get => this.isChangedPassword;
    set => this.isChangedPassword = value;
  }

  public bool IsRequiredOwerPassword
  {
    get => this.isRequiredOwerPassword;
    set => this.isRequiredOwerPassword = value;
  }
}
