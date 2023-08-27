using SampleHierarchies.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleHierarchies.Data;

public class Settings : ISettings
{
    #region Properties and Ctor
    
    private string _version;

    public string Version { get => _version; set => _version = value; }
    /// <summary>
    /// Ctor.
    /// </summary>
    public Settings()
    {
        _version = "1.0";
    }

    #endregion // Properties and Ctor
}