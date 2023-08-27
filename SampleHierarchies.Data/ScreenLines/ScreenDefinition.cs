using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Data.ScreenLines;

public class ScreenDefinition
{
    /// <summary>
    /// List which contain lines from JSON
    /// </summary>
    public List<ScreenLineEntry> LineEntries { get; set; }
    
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="lineEntries"></param>
    public ScreenDefinition(List<ScreenLineEntry> lineEntries)
    {
        LineEntries = lineEntries;
    }
}