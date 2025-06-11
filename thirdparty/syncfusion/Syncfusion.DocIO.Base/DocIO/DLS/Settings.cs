// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Settings
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Settings
{
  private CompatibilityOptions m_compatibilityOptions;
  private CompatibilityMode m_CompatibilityMode = CompatibilityMode.Word2013;
  private WordDocument m_document;
  private byte m_bFlags;
  private string m_hashValue;
  private string m_saltValue;
  private string m_cryptProviderTypeValue = "rsaFull";
  private string m_cryptAlgorithmClassValue = "hash";
  private string m_cryptAlgorithmTypeValue = "typeAny";
  private string m_cryptAlgorithmSidValue = 4.ToString();
  private string m_cryptSpinCountValue = 100000.ToString();
  private string m_duplicateListStyleNames = string.Empty;
  private WCharacterFormat m_themeFontLanguages;
  private OfficeMathProperties m_mathProperties;

  internal CompatibilityOptions CompatibilityOptions
  {
    get
    {
      if (this.m_compatibilityOptions == null)
        this.m_compatibilityOptions = new CompatibilityOptions((IWordDocument) this.m_document);
      return this.m_compatibilityOptions;
    }
  }

  public CompatibilityMode CompatibilityMode
  {
    get => this.m_CompatibilityMode;
    set
    {
      this.CompatibilityModeEnabled = true;
      this.m_CompatibilityMode = value;
    }
  }

  internal bool CompatibilityModeEnabled
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public bool MaintainFormattingOnFieldUpdate
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public bool UpdateResultOnFieldCodeChange
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public bool DisableMovingEntireField
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  public bool DisplayBackgrounds
  {
    get => this.m_document != null && this.m_document.DOP.Dop2003.DispBkSpSaved;
    set
    {
      if (this.m_document == null || value == this.m_document.DOP.Dop2003.DispBkSpSaved)
        return;
      this.m_document.DOP.Dop2003.DispBkSpSaved = value;
    }
  }

  internal string HashValue
  {
    get => this.m_hashValue;
    set => this.m_hashValue = value;
  }

  internal string SaltValue
  {
    get => this.m_saltValue;
    set => this.m_saltValue = value;
  }

  internal string CryptProviderTypeValue
  {
    get => this.m_cryptProviderTypeValue;
    set => this.m_cryptProviderTypeValue = value;
  }

  internal string CryptAlgorithmSidValue
  {
    get => this.m_cryptAlgorithmSidValue;
    set => this.m_cryptAlgorithmSidValue = value;
  }

  internal string CryptAlgorithmClassValue
  {
    get => this.m_cryptAlgorithmClassValue;
    set => this.m_cryptAlgorithmClassValue = value;
  }

  internal string CryptAlgorithmTypeValue
  {
    get => this.m_cryptAlgorithmTypeValue;
    set => this.m_cryptAlgorithmTypeValue = value;
  }

  internal string CryptSpinCountValue
  {
    get => this.m_cryptSpinCountValue;
    set => this.m_cryptSpinCountValue = value;
  }

  internal bool IsOptimizedForBrowser
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal string DuplicateListStyleNames
  {
    get => this.m_duplicateListStyleNames;
    set => this.m_duplicateListStyleNames = value;
  }

  internal WCharacterFormat ThemeFontLanguages
  {
    get => this.m_themeFontLanguages;
    set => this.m_themeFontLanguages = value;
  }

  public bool MaintainImportedListCache
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set
    {
      if (!value)
        this.DuplicateListStyleNames = string.Empty;
      this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
    }
  }

  public bool SkipIncrementalSaveValidation
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  internal OfficeMathProperties MathProperties
  {
    get => this.m_mathProperties;
    set => this.m_mathProperties = value;
  }

  public bool PreserveOleImageAsImage
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  internal Settings(WordDocument document)
  {
    this.m_document = document;
    this.UpdateResultOnFieldCodeChange = true;
    this.IsOptimizedForBrowser = true;
  }

  internal void Close()
  {
    this.m_document = (WordDocument) null;
    if (this.m_compatibilityOptions == null)
      return;
    this.m_compatibilityOptions.Close();
    this.m_compatibilityOptions = (CompatibilityOptions) null;
  }

  internal void SetCompatibilityModeValue(CompatibilityMode value)
  {
    this.m_CompatibilityMode = value;
  }
}
