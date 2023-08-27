using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Data.ScreenLines;

public class ScreenLineEntry
{
    /// <summary>
    /// Properties for lines which will be outputted to the console
    /// Backgroundcolor
    /// ForegroundColor
    /// Text
    /// </summary>
    public ConsoleColor BackgroundColor { get; set; }
    public ConsoleColor ForegroundColor { get; set; }
    public string? Text { get; set; }

}