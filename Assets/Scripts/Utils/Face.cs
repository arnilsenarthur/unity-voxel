using System;

namespace VoxelEngine.Utils
{
    [System.Flags]
    public enum Face
    {
        XP = 1,
        YP = 2,
        ZP = 4,

        XN = 8,
        YN = 16,
        ZN = 32,

        X = XP | XN,
        Y = YP | YN,
        Z = ZP | ZN,

        All = X | Y | Z,
        None = 0
    }
}