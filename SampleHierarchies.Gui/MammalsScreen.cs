using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;

namespace SampleHierarchies.Gui;

/// <summary>
/// Mammals main screen.
/// </summary>
public sealed class MammalsScreen : Screen
{
    #region Properties And Ctor
    
    /// <summary>
    /// Dogs screen.
    /// Wolfs screen.
    /// Dolphins screen.
    /// Bengal Tiger screen.
    /// Screen Definition Service.
    /// </summary>
    private readonly DogsScreen _dogsScreen;
    private readonly WolfsScreen _wolfsScreen;
    private readonly DolphinsScreen _dolphinsScreen;
    private readonly BengalTigerScreen _bengalTigerScreen;
    private readonly ScreenDefinitionService _screenDefinitionService;

    /// <summary>
    /// Property which contain JSON file name.
    /// </summary>
    private readonly string jsonfileName = "EnMammalsScreenLines.json";

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dogsScreen"></param>
    /// <param name="wolfsScreen"></param>
    /// <param name="dolphinsScreen"></param>
    /// <param name="bengalTigerScreen"></param>
    /// <param name="screenDefinitionService"></param>
    public MammalsScreen(DogsScreen dogsScreen,
           WolfsScreen wolfsScreen,
           DolphinsScreen dolphinsScreen,
           BengalTigerScreen bengalTigerScreen,
           ScreenDefinitionService screenDefinitionService)
    {
           _dogsScreen = dogsScreen;
           _wolfsScreen = wolfsScreen;
           _dolphinsScreen = dolphinsScreen;
           _bengalTigerScreen = bengalTigerScreen;
           _screenDefinitionService = screenDefinitionService;
    }

    #endregion Properties And Ctor

    #region Public Methods

    public override void Show()
    {
        while (true)
        {
            Console.Clear();
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Menu, 0); // Screen history: MainScreen -> AnimalsScreen -> MammalsScreen
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 0); // Your available choices are: 
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 1); // 0. Exit 
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 2); // 1. Dogs 
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 3); // 2. Wolves(Wolfs)
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 4); // 3. Dolphins
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 5); // 4. Bengal Tiger
            _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 6); // Please enter your choice: 

            string? choiceAsString = Console.ReadLine();

            /// <summary>
            /// Selection of animal screens
            /// </summary>
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                MammalsScreenChoices choice = (MammalsScreenChoices)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case MammalsScreenChoices.Dogs:
                        _dogsScreen.Show();
                        break;

                    case MammalsScreenChoices.Wolf:
                        _wolfsScreen.Show(); 
                        break;
                    
                    case MammalsScreenChoices.Dolphin:
                        _dolphinsScreen.Show();
                        break;

                    case MammalsScreenChoices.BengalTiger:
                        _bengalTigerScreen.Show();
                        break;

                    case MammalsScreenChoices.Exit:
                        _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 7); // Going back to parent menu
                        Thread.Sleep(500);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.ShowLines(jsonfileName, ScreenLineEnum.Show, 8); // Invalid choice. Try again.
                Thread.Sleep(500);
            }
        }
    }

    #endregion // Public Methods

}