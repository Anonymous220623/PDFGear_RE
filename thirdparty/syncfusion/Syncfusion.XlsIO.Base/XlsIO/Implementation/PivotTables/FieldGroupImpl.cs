// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.FieldGroupImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class FieldGroupImpl
{
  private PivotCacheFieldImpl m_pivotCacheField;
  private FieldGroupImpl.FieldRangeGroup m_rangeGroup;
  private FieldGroupImpl.FieldDiscreteRangeGroup m_discreteGroup;
  private bool m_bHasDateTime;
  private bool m_bHasNumber;
  private int m_iParentFieldIndex;
  private bool m_bHasMissingAttribute;

  public int ParentFieldIndex
  {
    get => this.m_iParentFieldIndex;
    internal set => this.m_iParentFieldIndex = value;
  }

  public int PivotCacheFieldIndex => this.m_pivotCacheField.Index;

  public PivotCacheFieldImpl PivotCacheField => this.m_pivotCacheField;

  internal FieldGroupImpl.FieldRangeGroup RangeGroup
  {
    get
    {
      if (this.m_rangeGroup == null)
        this.m_rangeGroup = new FieldGroupImpl.FieldRangeGroup();
      return this.m_rangeGroup;
    }
  }

  public DateTime StartDate
  {
    get => this.RangeGroup.m_startDate;
    set
    {
      this.AutoStartRange = false;
      this.RangeGroup.m_startDate = value;
      this.HasDateTime = true;
    }
  }

  public DateTime EndDate
  {
    get => this.RangeGroup.m_endDate;
    set
    {
      this.RangeGroup.m_endDate = value;
      this.HasDateTime = true;
    }
  }

  public double StartNumber
  {
    get => this.RangeGroup.m_dStartNumber;
    set
    {
      this.RangeGroup.m_dStartNumber = value;
      this.m_bHasNumber = true;
    }
  }

  public double EndNumber
  {
    get => this.RangeGroup.m_dEndNumber;
    set
    {
      this.RangeGroup.m_dEndNumber = value;
      this.m_bHasNumber = true;
    }
  }

  public PivotFieldGroupType GroupBy
  {
    get => this.RangeGroup.m_groupBy;
    set => this.RangeGroup.m_groupBy = value;
  }

  public double GroupInterval
  {
    get => this.RangeGroup.m_dGroupInterval;
    set => this.RangeGroup.m_dGroupInterval = value;
  }

  internal bool HasGroupInterval
  {
    get => this.RangeGroup.m_hasGroupInterval;
    set => this.RangeGroup.m_hasGroupInterval = value;
  }

  public List<string> PivotRangeGroupNames => this.RangeGroup.GroupItems;

  public List<string> PivotDiscreteGroupNames
  {
    get
    {
      if (this.m_discreteGroup == null)
        this.m_discreteGroup = new FieldGroupImpl.FieldDiscreteRangeGroup();
      return this.m_discreteGroup.GroupItems;
    }
  }

  public byte[] DiscreteGroupIndexes
  {
    get => this.m_discreteGroup.Indexes;
    set => this.m_discreteGroup.Indexes = value;
  }

  public bool IsDiscrete => this.m_discreteGroup != null;

  public bool AutoStartRange
  {
    get => this.RangeGroup.m_bAutoStartRange;
    set => this.RangeGroup.m_bAutoStartRange = value;
  }

  public bool AutoEndRange
  {
    get => this.RangeGroup.m_bAutoEndRange;
    set => this.RangeGroup.m_bAutoEndRange = value;
  }

  public bool HasDateTime
  {
    get => this.m_bHasDateTime;
    set => this.m_bHasDateTime = value;
  }

  public bool HasNumber
  {
    get => this.m_bHasNumber;
    set => this.m_bHasNumber = value;
  }

  internal bool HasMissingAttribute
  {
    get => this.m_bHasMissingAttribute;
    set => this.m_bHasMissingAttribute = value;
  }

  public FieldGroupImpl(PivotCacheFieldImpl baseField, int parentFieldIndex)
  {
    this.m_pivotCacheField = baseField != null ? baseField : throw new ArgumentNullException("base Field");
    this.ParentFieldIndex = parentFieldIndex;
    this.EndDate = new DateTime();
    this.StartDate = new DateTime();
    this.AutoEndRange = true;
    this.AutoStartRange = true;
    this.EndNumber = -1.0;
    this.StartNumber = -1.0;
    this.HasDateTime = false;
    this.HasNumber = false;
    this.GroupBy = PivotFieldGroupType.None;
  }

  public FieldGroupImpl(PivotCacheFieldImpl baseField)
    : this(baseField, -1)
  {
  }

  public void FillRangeGroup(string[] values)
  {
    this.PivotRangeGroupNames.AddRange((IEnumerable<string>) values);
  }

  public void FillDiscreteGroup(int[] indexes, string[] groupNames)
  {
    this.PivotDiscreteGroupNames.AddRange((IEnumerable<string>) groupNames);
    this.DiscreteGroupIndexes = new byte[indexes.Length];
    for (int index = 0; index < indexes.Length; ++index)
      this.DiscreteGroupIndexes[index] = (byte) indexes[index];
  }

  internal class FieldRangeGroup
  {
    internal List<string> m_groupItems;
    internal bool m_bAutoStartRange;
    internal bool m_bAutoEndRange;
    internal DateTime m_endDate;
    internal double m_dEndNumber;
    internal PivotFieldGroupType m_groupBy;
    internal double m_dGroupInterval;
    internal DateTime m_startDate;
    internal double m_dStartNumber;
    internal bool m_hasGroupInterval;

    public List<string> GroupItems
    {
      get
      {
        if (this.m_groupItems == null)
          this.m_groupItems = new List<string>();
        return this.m_groupItems;
      }
    }

    public bool AutoStartRange
    {
      get => this.m_bAutoEndRange;
      set => this.m_bAutoEndRange = value;
    }

    public bool AutoEndRange
    {
      get => this.m_bAutoEndRange;
      set => this.m_bAutoEndRange = value;
    }

    public DateTime EndDate
    {
      get => this.m_endDate;
      set => this.m_endDate = value;
    }

    public double EndNumber
    {
      get => this.m_dEndNumber;
      set => this.m_dEndNumber = value;
    }

    public PivotFieldGroupType GroupBy
    {
      get => this.m_groupBy;
      set => this.m_groupBy = value;
    }

    public double GroupInterval
    {
      get => this.m_dGroupInterval;
      set => this.m_dGroupInterval = value;
    }

    public DateTime StartDate
    {
      get => this.m_startDate;
      set => this.m_startDate = value;
    }

    public double StartNumber
    {
      get => this.m_dStartNumber;
      set => this.m_dStartNumber = value;
    }
  }

  private class FieldDiscreteRangeGroup
  {
    internal List<string> m_groupItems;
    private byte[] m_indexes;

    public List<string> GroupItems
    {
      get
      {
        if (this.m_groupItems == null)
          this.m_groupItems = new List<string>();
        return this.m_groupItems;
      }
    }

    public byte[] Indexes
    {
      get => this.m_indexes;
      set => this.m_indexes = value;
    }
  }
}
