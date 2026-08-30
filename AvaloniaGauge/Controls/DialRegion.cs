using Avalonia.Media;

namespace AvaloniaGauge.Controls;

public sealed class DialRegion
{
    /// <summary>
    /// Region start in the Dial's value space.
    /// </summary>
    public double Start { get; init; }

    /// <summary>
    /// Region end in the Dial's value space.
    /// </summary>
    public double End { get; init; }

    /// <summary>
    /// Brush used to render the region.
    /// </summary>
    public IBrush? Color { get; init; }

    /// <summary>
    /// Region thickness.
    /// A value less than or equal to zero uses the Dial's RegionThickness.
    /// </summary>
    public double Thickness { get; init; }
}
