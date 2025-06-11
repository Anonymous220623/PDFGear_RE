// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfSecurity
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class PdfSecurity
{
  private string m_ownerPassword;
  private string m_userPassword;
  private PdfEncryptor m_encryptor;
  internal bool m_modifiedSecurity;
  internal bool m_encryptOnlyAttachment;
  internal PdfEncryptionOptions m_encryptionOption;

  public string OwnerPassword
  {
    get => this.m_encryptOnlyAttachment ? string.Empty : this.m_encryptor.OwnerPassword;
    set
    {
      if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
        throw new Exception($"Document encryption is not allowed with{PdfDocument.ConformanceLevel.ToString()} Conformance documents.");
      this.m_encryptor.OwnerPassword = value;
      this.m_encryptor.Encrypt = true;
      this.m_modifiedSecurity = true;
    }
  }

  public string UserPassword
  {
    get => this.m_encryptor.UserPassword;
    set
    {
      if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
        throw new Exception($"Document encryption is not allowed with{PdfDocument.ConformanceLevel.ToString()} Conformance documents.");
      this.m_encryptor.UserPassword = value;
      this.m_encryptor.Encrypt = true;
      this.m_modifiedSecurity = true;
    }
  }

  public PdfPermissionsFlags Permissions
  {
    get => this.m_encryptor.Permissions;
    set
    {
      if (this.m_encryptor.Permissions != value)
        this.m_encryptor.Permissions = value;
      this.m_encryptor.Encrypt = true;
      this.m_modifiedSecurity = true;
    }
  }

  internal PdfEncryptor Encryptor
  {
    get => this.m_encryptor;
    set => this.m_encryptor = value;
  }

  public PdfEncryptionKeySize KeySize
  {
    get => this.m_encryptor.CryptographicAlgorithm;
    set
    {
      this.m_encryptor.CryptographicAlgorithm = value;
      this.m_encryptor.Encrypt = true;
      this.m_modifiedSecurity = true;
    }
  }

  public PdfEncryptionAlgorithm Algorithm
  {
    get => this.m_encryptor.EncryptionAlgorithm;
    set
    {
      this.m_encryptor.EncryptionAlgorithm = value;
      this.m_encryptor.Encrypt = true;
      this.m_modifiedSecurity = true;
    }
  }

  internal bool Enabled
  {
    get => this.m_encryptor.Encrypt;
    set => this.m_encryptor.Encrypt = value;
  }

  internal bool EncryptOnlyAttachment
  {
    get => this.m_encryptOnlyAttachment;
    set
    {
      this.m_encryptOnlyAttachment = value;
      this.m_encryptor.EncryptOnlyAttachment = value;
      this.m_modifiedSecurity = true;
    }
  }

  public PdfEncryptionOptions EncryptionOptions
  {
    get => this.m_encryptionOption;
    set
    {
      this.m_encryptionOption = value;
      this.m_encryptor.Encrypt = true;
      this.m_modifiedSecurity = true;
      if (PdfEncryptionOptions.EncryptOnlyAttachments == value)
      {
        this.EncryptOnlyAttachment = true;
        this.m_encryptor.EncryptMetaData = false;
      }
      else if (PdfEncryptionOptions.EncryptAllContentsExceptMetadata == value)
      {
        this.m_encryptor.EncryptMetaData = false;
        this.EncryptOnlyAttachment = false;
      }
      else
      {
        this.m_encryptor.EncryptMetaData = true;
        this.EncryptOnlyAttachment = false;
      }
    }
  }

  public PdfSecurity()
  {
    this.m_ownerPassword = string.Empty;
    this.m_userPassword = string.Empty;
    this.m_encryptor = new PdfEncryptor();
  }

  public PdfPermissionsFlags SetPermissions(PdfPermissionsFlags flags)
  {
    this.Permissions |= flags;
    return this.Permissions;
  }

  public PdfPermissionsFlags ResetPermissions(PdfPermissionsFlags flags)
  {
    this.Permissions &= ~flags;
    return this.Permissions;
  }
}
