using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shuriken.Models.Animation
{
    public enum AnimationType : uint
    {
        None        = 0,
        Unknown     = 1,
        XPosition   = 2,
        YPosition   = 4,
        Rotation    = 8,
        XScale      = 16,
        YScale      = 32,
        SubImage    = 64,
        Color       = 128,
        GradientTL  = 256,
        GradientBL  = 512,
        GradientTR  = 1024,
        GradientBR  = 2048,
        ZPosition = 4096,
        ZScale = 8192,
        ColorRed = 16384,
        ColorGreen = 32768,
        ColorBlue = 65536,
        ColorAlpha = 131072
    }
}
