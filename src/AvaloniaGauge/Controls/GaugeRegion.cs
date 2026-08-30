using Avalonia.Media;

namespace AvaloniaGauge.Controls;

public sealed class GaugeRegion
{
    /// <summary>
    /// Region start in the Gauge's value space.
    /// </summary>
    public double Start { get; init; }

    /// <summary>
    /// Region end in the Gauge's value space.
    /// </summary>
    public double End { get; init; }

    /// <summary>
    /// Brush used to render the region.
    /// </summary>
    public IBrush? Color { get; init; }

    /// <summary>
    /// Region thickness.
    /// A value less than or equal to zero uses the Gauge's RegionThickness.
    /// </summary>
    public double Thickness { get; init; }
}
