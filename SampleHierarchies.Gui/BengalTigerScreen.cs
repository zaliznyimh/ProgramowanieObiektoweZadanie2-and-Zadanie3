using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;
using System.Linq.Expressions;

namespace SampleHierarchies.Gui;
public class BengalTigerScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// Data service.
    /// Screen Definition Service.
    /// </summary>
    private readonly IDataService _dataService;
    private readonly ScreenDefinitionService _screenDefinitionService;

    /// <summary>
    /// Property for storing JSON file name.
    /// </summary>
    private readonly string jsonFileName = "EnTigersScreenLines.json";

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService"></param>
    /// <param name="screenDefinitionService"></param>
    public BengalTigerScreen(IDataService dataService, ScreenDefinitionService screenDefinitionService)
    {
        _dataService = dataService;
        _screenDefinitionService = screenDefinitionService;
    }

    #endregion // Properties And Ctor

    #region Public Methods

    public override void Show()
    {
        while (true)
        {
            Console.Clear();
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 0); // Screen history: MainScreen -> AnimalsScreen -> MammalsScreen -> BengalTigersScreen
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 0); // Your available choices are:
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 1); // 0. Exit
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 2); // 1. List of all bengal tigers
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 3); // 2. Create a new bengal tiger
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 4); // 3. Delete existing bengal tiger
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 5); // 4. Modify existing bengal tiger
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 6); // Please choose something:

            string? choiceAsString = Console.ReadLine();

            /// <summary>
            /// Function for displaying the bengal tiger screen
            /// </summary>
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                BengalTigerScreenChoice choice = (BengalTigerScreenChoice)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case BengalTigerScreenChoice.List:
                        BengalTigerList();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case BengalTigerScreenChoice.Create:
                        CreateBengalTiger();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case BengalTigerScreenChoice.Delete:
                        DeleteBengalTiger();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case BengalTigerScreenChoice.Modify:
                        ModifyBengalTiger();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case BengalTigerScreenChoice.Exit:
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 7); // Goingback to parent menu
                        Thread.Sleep(500);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 8); // Incorrect choise. Try again
                Thread.Sleep(1000);
            }
        }
    }

    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// Method for showing list of all bengal tigers
    /// </summary>
    private void BengalTigerList()
    {
        Console.WriteLine();
        if (_dataService?.Animals?.Mammals?.BengalTigers is not null &&
            _dataService.Animals.Mammals.BengalTigers.Count > 0)
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 0); // Here's a list of bengal tigers:
            int i = 1;
            foreach (BengalTiger tiger in _dataService.Animals.Mammals.BengalTigers.Cast<BengalTiger>())
            {
                Console.Write(i);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 1); // - is bengal tiger's number.
                tiger.Display();
                i++;
            }
        }
        else
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 2); // Not a single bengal tiger on the list yet.
        }
    }

    /// <summary>
    /// Method which can create bengal tiger
    /// </summary>
    private void CreateBengalTiger()
    {
        try
        {
            BengalTiger tiger = AddEditBengalTiger();
            _dataService?.Animals?.Mammals?.BengalTigers?.Add(tiger);
            Console.Write(tiger.Name);
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 0); // bengal tiger was recently added to a list of Bengal tigers
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 1); // Invalid input. Try again
        }
    }

    /// <summary>
    /// Method for deleting bengal tiger from the list
    /// </summary>
    private void DeleteBengalTiger()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 0); // What is the name of the Bengal tiger you want to delete? 
            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            BengalTiger? tiger = (BengalTiger?)(_dataService?.Animals?.Mammals?.BengalTigers
                ?.FirstOrDefault(t => t is not null && string.Equals(t.Name, name)));
            if (tiger is not null)
            {
                _dataService?.Animals?.Mammals?.BengalTigers?.Remove(tiger);
                Console.WriteLine(tiger.Name);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 1); // Bengal tiger with that name was deleted from a list of Bengal tigers
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 2); // Bengal tiger is not found
            }
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 3); // Invalid input
        }
    }

    /// <summary>
    /// Method for editing some bengal properties
    /// </summary>
    private void ModifyBengalTiger()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 0); // What is the name of the bengal tiger you want to edit?: 
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 1); // Please, enter it's name:

            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            BengalTiger? tiger = (BengalTiger?)(_dataService?.Animals?.Mammals?.BengalTigers
                        ?.FirstOrDefault(t => t is not null && string.Equals(t.Name, name)));
            if (tiger is not null)
            {
                BengalTiger tigerModified = AddEditBengalTiger();
                tiger.Copy(tigerModified);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 2); // Bengal tiger after edit:
                tiger.Display();
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 3); // Bengal tiger was not found
            }
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 4); // Incorrect input. Try again.
        }
    }

    /// <summary>
    /// Method which help with creating/edditing BengalTigers
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private BengalTiger AddEditBengalTiger()
    {
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 0); // Write the name of bengal tiger: 
        string? name = Console.ReadLine();

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 1); // Write tiger's age: 
        string? ageAsString = Console.ReadLine();

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 2); // Is your bengal tiger predator?(Write Yes or No): 
        string? choiseApexPredator = Console.ReadLine();
        bool isApexPredator = false;
        string? apexPredatorDefinition = "";

        switch (choiseApexPredator)
        {
            case "Yes":
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 3); // Write how do hunting abilities manifest themselves: 
                isApexPredator = true;
                apexPredatorDefinition = Console.ReadLine();
                break;
            case "No":
                apexPredatorDefinition = "Bengal tiger doesn't show it's apex predator features";
                break;
            default:
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 4); // Invalid input.
                break;
        }

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 5); // Write the size of animal in metres: 
        string? largeSizeAsString = Console.ReadLine();

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 6); // Desribe the comouflage fur:
        string? commouflageFur = Console.ReadLine();

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 7); // Does tiger has powerful legs?(Write Yes or No): 
        string? choisePowerfulLegs = Console.ReadLine();
        bool isPowerfulLegs = false;
        string? powerfulLegsDefinition = " - ";

        switch (choisePowerfulLegs)
        {
            case "Yes":
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 8); // Write how powerful legs help tiger in life:
                isPowerfulLegs = true;
                powerfulLegsDefinition = Console.ReadLine();
                break;
            case "No":
                powerfulLegsDefinition = "Bengal tiger has weak legs";
                break;
            default:
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 4); // Invalid input.
                break;
        }

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 9); // Does the bengal tiger lives alone?(Write Yes or No): 
        string? choiseSolitaryBehavior = Console.ReadLine();
        bool isSolitaryBehavior = false;
        string? solitaryBehaviorDefinition = " - ";

        switch (choiseSolitaryBehavior)
        {
            case "Yes":
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 10); // Desribe his solitary behavior: 
                isSolitaryBehavior = true;
                solitaryBehaviorDefinition = Console.ReadLine();
                break;
            case "No":
                solitaryBehaviorDefinition = "Bengal tiger has weak legs ";
                break;
            default:
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 4); // Invalid input. 
                break;
        }

        if (name is null) { throw new ArgumentNullException(nameof(name)); }
        if (ageAsString is null) { throw new ArgumentNullException(nameof(ageAsString)); }
        if (apexPredatorDefinition is null) { throw new ArgumentNullException(nameof(apexPredatorDefinition)); }
        if (largeSizeAsString is null) { throw new ArgumentNullException(nameof(largeSizeAsString)); }
        if (commouflageFur is null) { throw new ArgumentNullException(nameof(commouflageFur)); }
        if (powerfulLegsDefinition is null) { throw new ArgumentNullException(nameof(powerfulLegsDefinition)); }
        if (solitaryBehaviorDefinition is null) { throw new ArgumentNullException(nameof(solitaryBehaviorDefinition)); }

        int age = Convert.ToInt32(ageAsString);
        float largeSize = float.Parse(largeSizeAsString);

        BengalTiger tiger = new BengalTiger(name, age, isApexPredator, apexPredatorDefinition, largeSize, commouflageFur, isPowerfulLegs,
                                            powerfulLegsDefinition, isSolitaryBehavior, solitaryBehaviorDefinition);

        return tiger;
    }

    #endregion // Private Methods

}