using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Data.Mammals;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;

namespace SampleHierarchies.Gui;

/// <summary>
/// Wolves screen
/// </summary>
public class WolfsScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// Data service,
    /// SettingsService
    /// </summary>
    private readonly IDataService _dataService;
    private readonly ScreenDefinitionService _screenDefinitionService;  

    private readonly string jsonFileName = "EnWolfsScreenLines.json";


    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService">Data service reference</param>
    public WolfsScreen(IDataService dataService, ScreenDefinitionService screenDefinitionService)
    {
        _dataService = dataService;
        _screenDefinitionService = screenDefinitionService;
    }

    #endregion // Properties And Ctor

    #region Public Methods

    /// <summary>
    /// Method for showing wolf's main screen
    /// </summary>
    public override void Show()
    {
        while (true)
        {
            Console.Clear();
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 0); // Screen history: MainScreen -> AnimalsScreen -> MammalsScreen -> WolfsScreen
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 0); // Your available choices are:
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 1); // 0. Exit
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 2); // 1. List all wolfs
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 3); // 2. Create a new wolf
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 4); // 3. Delete existing wolf
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 5); // 4. Modify existing wolf
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 6); // Please choose something: 

            string? choiceAsString = Console.ReadLine();
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                WolfScreenChoise choice = (WolfScreenChoise)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case WolfScreenChoise.List:
                        WolfList();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case WolfScreenChoise.Create:
                        CreateWolf();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case WolfScreenChoise.Delete:
                        DeleteWolf();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case WolfScreenChoise.Modify:
                        ModifyWolf();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ResetColor();
                        Console.ReadKey();
                        break;

                    case WolfScreenChoise.Exit:
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 7); // Goingback to parent menu. 
                        Console.ResetColor();
                        Thread.Sleep(500);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 8); // Invalid choise. Try again.
                Console.ResetColor();
                Thread.Sleep(1000);
            }
        }
    }

    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// Method for showing list of all wolves
    /// </summary>
    private void WolfList()
    {
        Console.WriteLine();
        if (_dataService?.Animals?.Mammals?.Wolves is not null &&
            _dataService.Animals.Mammals.Wolves.Count > 0)
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 0); // Here's a list of wolves:
            int i = 1;
            foreach (Wolf wolf in _dataService.Animals.Mammals.Wolves.Cast<Wolf>())
            {
                Console.Write(i);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 1); // - is wolf's number
                wolf.Display();
                i++;
            }
        }
        else
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 2); // Not a single wolf on the list yet.
        }
    }

    /// <summary>
    /// Method which create wolves
    /// </summary>
    private void CreateWolf()
    {
        try
        {
            Wolf wolf = AddEditWolf();
            _dataService?.Animals?.Mammals?.Wolves?.Add(wolf);
            Console.Write(wolf.Name);
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 0); // this wolf was recently added to a list of wolfs
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 1); // Invalid input. Try again
        }
    }

    /// <summary>
    /// Method for deleting wolf from the list
    /// </summary>
    private void DeleteWolf()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 0); // What is the name of the wolf you want to delete? 
            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Wolf? wolf = (Wolf?)(_dataService?.Animals?.Mammals?.Wolves
                ?.FirstOrDefault(w => w is not null && string.Equals(w.Name, name)));
            if (wolf is not null)
            {
                _dataService?.Animals?.Mammals?.Wolves?.Remove(wolf);
                Console.WriteLine(wolf.Name);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 1); // Wolf with that name was deleted from a list of dogs 
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 2); // Wolf is not found
            }
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 3); // Invalid input
        }
    }

    /// <summary>
    /// Method for editing exiting wolf 
    /// </summary>
    private void ModifyWolf()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 0); // What is the name of the wolf you want to edit?: 
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 1); // Please, enter it's name:

            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Wolf? wolf = (Wolf?)(_dataService?.Animals?.Mammals?.Wolves
                        ?.FirstOrDefault(w => w is not null && string.Equals(w.Name, name)));
            if (wolf is not null)
            {
                Wolf wolfModified = AddEditWolf();
                wolf.Copy(wolfModified);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 2); // Wolf after edit:
                wolf.Display();
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 3); // Wolf was not found
            }
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 4); // Incorrect input. Try again.
        }
    }

    /// <summary>
    /// Method which helps create and(or) edit wolves 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private Wolf AddEditWolf()
    {
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 0); // What is name of the wolf
        string? name = Console.ReadLine();

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 1); // What is the wolf's age?
        string? ageAsString = Console.ReadLine();

        ///<summary>
        /// Asking user for hunting features
        ///</summary>
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 2); // Does it hunts in group?(Write Yes or No):  
        string? choisePackHunting = Console.ReadLine();
        string? packHuntingDefinition = " - ";
        bool isPackHunter = false;
        switch (choisePackHunting)
        {
            case "Yes":
                isPackHunter = true;
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 3); // Write how wolf hunting if group: 
                packHuntingDefinition = Console.ReadLine();
                break;
            case "No":
                packHuntingDefinition = "Wolf doesn't hunt in group.";
                break;
            default:
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 4); // Incorrect input
                break;
        }

        /// <summary>
        /// Asking user about wolf's communication
        /// </summary>
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 5); // Does it communicate by howl(Please write Yes or No): 
        string? choiseCommunicate = Console.ReadLine();
        string? communicationDefinition = " - ";
        bool isCommunicating = false;
        switch (choiseCommunicate)
        {
            case "Yes":
                isCommunicating = true;
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 6); // Write how wolf communicate with howl: 
                communicationDefinition = Console.ReadLine();
                break;
            case "No":
                communicationDefinition = "Wolf doesn't communicate by using howl. It communicate's with gestures and smells";
                break;
            default:
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 4); // Incorrect input
                break;
        }

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 7); // What does it eat?
        string? diet = Console.ReadLine();

        /// <summary>
        /// Asking user about paws
        /// </summary>
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 8); // Does the wolf have stong paws?(Please write Yes or No): 
        string? choisePaws = Console.ReadLine();
        string? pawsDefinition = "-";
        bool isPaws = false;
        switch (choisePaws)
        {
            case "Yes":
                isPaws = true;
                pawsDefinition = "Strong paws help the wolf to get better food and fight for territory";
                break;
            case "No":
                pawsDefinition = "Wolf doesn't have strong paws";
                break;
            default:
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 4); // Incorrect input
                break;
        }

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 9); // Decribe how it helps good sence of smell?: 
        string? senceOfSmell = Console.ReadLine();

        if (name is null) { throw new ArgumentNullException(nameof(name)); }
        if (ageAsString is null) { throw new ArgumentNullException(nameof(ageAsString)); }
        if (diet is null) { throw new ArgumentNullException(nameof(diet)); }
        if (senceOfSmell is null) { throw new ArgumentNullException(nameof(senceOfSmell)); }
        if (packHuntingDefinition is null) { throw new ArgumentNullException(nameof(packHuntingDefinition)); }
        if (communicationDefinition is null) { throw new ArgumentNullException(nameof(communicationDefinition)); }

        int age = Int32.Parse(ageAsString);

        Wolf wolf = new Wolf(name, age, isPackHunter, packHuntingDefinition, isCommunicating, communicationDefinition,
                             diet, isPaws, pawsDefinition, senceOfSmell);

        return wolf;
    }

    #endregion // Private Methods
}