using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Data;
using Newtonsoft.Json.Linq;
using SampleHierarchies.Services;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;

namespace SampleHierarchies.Gui;

/// <summary>
/// Application main screen.
/// </summary>
public sealed class MainScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// Settings service.
    /// Animals screen.
    /// </summary>
    private readonly ScreenDefinitionService _screenDefinitionService;
    private readonly AnimalsScreen _animalsScreen;
    
    /// <summary>
    /// Proprty which contain JSON file name.
    /// </summary>
    private readonly string jsonFileName = "EnMainScreenLines.json";

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="animalsScreen"></param>
    /// <param name="screenDefinitionService"></param>
    public MainScreen(
        AnimalsScreen animalsScreen,
        ScreenDefinitionService screenDefinitionService)
    {
        _animalsScreen = animalsScreen;
        _screenDefinitionService = screenDefinitionService;
    }

    #endregion Properties And Ctor

    #region Public Methods

    public override void Show()
    {
        while (true)
        {
            Console.Clear();
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 0); // Screen history: MainScreen
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 0); // Your available choices are:
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 1); // 0. Exit
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 2); // 1. Animals
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 3); // 2. Settings 
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 4); // Please enter your choice:

            string? choiceAsString = Console.ReadLine();
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                MainScreenChoices choice = (MainScreenChoices)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case MainScreenChoices.Animals:
                        _animalsScreen.Show();
                        break;

                    case MainScreenChoices.Settings:
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 5); // Sorry, but you can't change any settings in program 
                        Console.ReadKey();
                        break;

                    case MainScreenChoices.Exit:
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 6); // Goodbye!
                        Thread.Sleep(500);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 7); // Invalid choice. Try again.
                Thread.Sleep(1000);
            }
        }
    }

    #endregion // Public Methods

}