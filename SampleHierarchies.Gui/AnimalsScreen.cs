using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;
using System.Security;

namespace SampleHierarchies.Gui;

/// <summary>
/// Animals main screen.
/// </summary>
public sealed class AnimalsScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// IData service.
    /// Animals screen.
    /// Screen Definition Service.
    /// </summary>
    private readonly IDataService _dataService;
    private readonly MammalsScreen _mammalsScreen;
    private readonly ScreenDefinitionService _screenDefinitionService;

    /// <summary>
    /// Initialising property to contain json file name with lines.
    /// </summary>
    private readonly string jsonFileName = "EnAnimalsScreenLines.json";
    
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService"></param>
    /// <param name="mammalsScreen"></param>
    /// <param name="screenDefinitionService"></param>
    public AnimalsScreen(
        IDataService dataService,
        MammalsScreen mammalsScreen,
        ScreenDefinitionService screenDefinitionService )
    {
        _dataService = dataService;
        _mammalsScreen = mammalsScreen;
        _screenDefinitionService = screenDefinitionService;
    }

    #endregion Properties And Ctor

    #region Public Methods

    public override void Show()
    {
        while (true)
        {
            Console.Clear();
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 0); // Screen history: MainScreen -> AnimalsScreen
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 0); // Your available choices are:
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 1); // 0. Exit
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 2); // 1. Mammals
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 3); // 2. Save to file
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 4); // 3. Read from file
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 5); // Please enter your choice: 

            string? choiceAsString = Console.ReadLine();

            // Validate choice
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                AnimalsScreenChoices choice = (AnimalsScreenChoices)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case AnimalsScreenChoices.Mammals:
                        _mammalsScreen.Show();
                        break;

                    case AnimalsScreenChoices.Read:
                        ReadFromFile();
                        Thread.Sleep(2000);
                        break;

                    case AnimalsScreenChoices.Save:
                        SaveToFile();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Save, 4); // Click any key to continue 
                        Thread.Sleep(2000);
                        break;

                    case AnimalsScreenChoices.Exit:
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 6); // Going back to parent menu.
                        Thread.Sleep(500);
                        return;
                }
            }
            catch
            {
                Console.WriteLine("Invalid choice. Try again.");
            }
        }
    }

    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// Save to file.
    /// </summary>
    private void SaveToFile()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Save, 0); // Save data to file. 
            var fileName = Console.ReadLine();
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            _dataService.Write(fileName);
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Save, 1); // Data succesfull saved to your file.
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Save, 2); // Data saving was not successful.
        }
    }

    /// <summary>
    /// Read data from file.
    /// </summary>
    private void ReadFromFile()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Read, 0); // Read data from file: 
            var fileName = Console.ReadLine();
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            _dataService.Read(fileName);
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Read, 1); // Data reading from your file was successful.
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Read, 2); // Data reading from was your file not successful.
        }
    }

    #endregion // Private Methods
}