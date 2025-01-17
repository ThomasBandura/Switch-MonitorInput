// This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// See the LICENSE file for details.

namespace Switch_MonitorInput;

/// <summary>
/// Represents a rectangle defined by the coordinates of its upper-left corner (left, top) 
/// and its lower-right corner (right, bottom).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    /// <summary>
    /// The x-coordinate of the upper-left corner of the rectangle.
    /// </summary>
    public int left;

    /// <summary>
    /// The y-coordinate of the upper-left corner of the rectangle.
    /// </summary>
    public int top;

    /// <summary>
    /// The x-coordinate of the lower-right corner of the rectangle.
    /// </summary>
    public int right;

    /// <summary>
    /// The y-coordinate of the lower-right corner of the rectangle.
    /// </summary>
    public int bottom;
}