using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleHierarchies.Enums;

namespace SampleHierarchies.Data.ScreenLines;

public class ScreenLineHelper
{

    /// <summary>
    /// Dictionary which contain value and lines
    /// </summary>
    public Dictionary<ScreenLineEnum, ScreenDefinition>? ScreenLine { get; set;}

}