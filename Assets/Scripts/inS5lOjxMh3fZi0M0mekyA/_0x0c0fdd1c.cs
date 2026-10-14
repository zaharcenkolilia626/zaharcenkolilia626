using System;

/// <summary>
/// One chamber's difficulty envelope, serialized in the scene rather than compiled in.
/// The feasibility of every row is a property of these numbers: for each side
/// stoneCount * weightLow &lt;= targetLow and stoneCount * weightHigh &gt;= targetHigh,
/// so the partition below always terminates with an exact, in-band split.
/// </summary>
[Serializable]
public sealed class _0x0c0fdd1c
{
    public int DecoyCount = 2;
    public int WeightLow = 3;
    public int _0x6d4f7050
    {
        get
        {
            return this.LeftCount + this.RightCount + this.DecoyCount;
        }
    }

    public bool _0x99a1c546()
    {
        if (this.LeftCount < 1 || this.RightCount < 1 || this.DecoyCount < 0)
        {
            return false;
        }

        if (this.WeightLow < 1 || this.WeightHigh < this.WeightLow || this.TargetHigh < this.TargetLow)
        {
            return false;
        }

        return this._0x2e1d2036(this.LeftCount) && this._0x2e1d2036(this.RightCount);
    }

    public int Placements = 10;
    public int RightCount = 2;
    public int TargetHigh = 18;
    public int WeightHigh = 9;
    private bool _0x2e1d2036(int _0x1a631371)
    {
        return (_0x1a631371 * this.WeightLow) <= this.TargetLow && (_0x1a631371 * this.WeightHigh) >= this.TargetHigh;
    }

    public int TargetLow = 12;
    public int LeftCount = 2;
}