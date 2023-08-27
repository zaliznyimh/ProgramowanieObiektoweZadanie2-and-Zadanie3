using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleHierarchies.Data;
using SampleHierarchies.Data.ScreenLines;
using SampleHierarchies.Enums;


namespace SampleHierarchies.Services;

public class ScreenDefinitionService
{

    #region Properties and Ctor

    /// <summary>
    /// Screen Line Helper.
    /// </summary>>
    private ScreenLineHelper? _screenLineHelper;

    #endregion // Ctor

    #region Public Methods

    /// <summary>
    /// Method which reads property from JSON file 
    /// </summary>
    /// <param name="jsonFileName"></param>
    public static ScreenLineHelper? Load(string jsonFileName)
    {
        try
        {
            string jsonString = File.ReadAllText(jsonFileName);
            ScreenLineHelper? screenLineHelper = JsonConvert.DeserializeObject<ScreenLineHelper>(jsonString) ;

            return screenLineHelper;
        }
        catch (Exception ex)
        {
            Console.WriteLine("There is an error during reading json file." + ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Method that outputs strings from JSON file to console
    /// </summary>
    /// <param name="jsonFileName"></param>
    /// <param name="property"></param>
    /// <param name="lineID"></param>
    public string? ShowLines(string jsonFileName, ScreenLineEnum property, int lineID)
    {

        _screenLineHelper = Load(jsonFileName);

        if (_screenLineHelper != null && _screenLineHelper.ScreenLine != null) {
            Console.BackgroundColor = _screenLineHelper.ScreenLine[property].LineEntries[lineID].BackgroundColor;
            Console.ForegroundColor = _screenLineHelper.ScreenLine[property].LineEntries[lineID].ForegroundColor;
            Console.WriteLine(_screenLineHelper.ScreenLine[property].LineEntries[lineID].Text);
            Console.ResetColor();
            return "Ok";
        }
        else 
        {
            Console.WriteLine("Your JSON file is null here.");
            return null;
        }
    }

    #endregion // Public Methods

}