using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;

namespace SampleHierarchies.Gui;

public class DolphinsScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// IData service.
    /// Screen Definition Service.
    /// </summary>
    private readonly IDataService _dataService;
    private readonly ScreenDefinitionService _screenDefinitionService;
   
    /// <summary>
    /// Property for storing JSON file name.
    /// </summary>
    private readonly string jsonFileName = "EnDolphinsScreenLines.json";

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService"></param>
    /// <param name="screenDefinitionService"></param>
    public DolphinsScreen(IDataService dataService, ScreenDefinitionService screenDefinitionService)
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
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 0); // Screen history: MainScreen -> AnimalsScreen -> MammalsScreen -> DolphinsScreen
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 0); // Your available choices are:
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 1); // 0. Exit
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 2); // 1. List of all dolphins
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 3); // 2. Create a new dolphin
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 4); // 3. Delete existing dolphin
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 5); // 4. Modify existing dolphin
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 6); // Please choose something:

            string? choiceAsString = Console.ReadLine();
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                DolphinsScreenChoices choice = (DolphinsScreenChoices)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case DolphinsScreenChoices.List:
                        DolphinList();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DolphinsScreenChoices.Create:
                        CreateDolphin();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DolphinsScreenChoices.Delete:
                        DeleteDolphin();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DolphinsScreenChoices.Modify:
                        ModifyDolphin();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DolphinsScreenChoices.Exit:
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 7); // Going back to parent menu.
                        Thread.Sleep(500);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 8); // Incorrect choise. Try again
            }
        }
    }

    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// Method for showing list of all dolphins.
    /// </summary>
    private void DolphinList()
    {
        Console.WriteLine();
        if (_dataService?.Animals?.Mammals?.Dolphins is not null &&
            _dataService.Animals.Mammals.Dolphins.Count > 0)
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 0); // Here's a list of dolphins:
            int i = 1;
            foreach (Dolphin dolphin in _dataService.Animals.Mammals.Dolphins.Cast<Dolphin>())
            {
                Console.Write(i);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 1); // - is dolphin's number
                dolphin.Display();
                i++;
            }
        }
        else
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 2); // Not a single dolphin on the list yet.
        }
    }
    /// <summary>
    /// Method which can create dolphins.
    /// </summary>
    private void CreateDolphin()
    {
        try
        {
            Dolphin dolphin = AddEditDolphin();
            _dataService?.Animals?.Mammals?.Dolphins?.Add(dolphin);
            Console.Write(dolphin.Name);
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 0); // dolphin was recently added to a list of dolphins
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 1); // Invalid input. Try again
        }
    }

    /// <summary>
    /// Method for deleting dolphin from the list.
    /// </summary>
    private void DeleteDolphin()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 0); // What is the name of the dolphin you want to delete? 
            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Dolphin? dolphin = (Dolphin?)(_dataService?.Animals?.Mammals?.Dolphins
                ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
            if (dolphin is not null)
            {
                _dataService?.Animals?.Mammals?.Dolphins?.Remove(dolphin);
                Console.WriteLine(dolphin.Name);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 1); // Dolphin with that name was deleted from a list of dolphins
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 2); // Dolphin is not found
            }
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 3); // Invalid input
        }
    }

    /// <summary>
    /// Method for editing some dolphin's properties.
    /// </summary>
    private void ModifyDolphin()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 0); // What is the name of the dolphin you want to edit?: 
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 1); // Please, enter it's name:

            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Dolphin? dolphin = (Dolphin?)(_dataService?.Animals?.Mammals?.Dolphins
                        ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
            if (dolphin is not null)
            {
                Dolphin dolphinModified = AddEditDolphin();
                dolphin.Copy(dolphinModified);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 2); // Dolphin after edit:
                dolphin.Display();
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 3);
            }
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 4); // Incorrect input. Try again.
        }
    }

    /// <summary>
    /// Method which helps with creating and(or) editing dolphins.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private Dolphin AddEditDolphin()
    {
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 0); // What is name of the dolphin
        string? name = Console.ReadLine();
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 1); // What's dolphin's age
        string? ageAsString = Console.ReadLine();

        /// <summary>
        /// Asking about dolphin's property of echolocation.
        /// </summary>

        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 2); // Does the dolphin use echolocation?(Write Yes or No): 
        string? choiseEcholocation = Console.ReadLine();
        string? echolocationDefinition;
        bool useEcholocation;

        if(choiseEcholocation == "Yes")
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 3); // Write how dolphin use echolocation: 
            echolocationDefinition = Console.ReadLine();
            useEcholocation = true;
        }
        else
        {
            useEcholocation = false;
            echolocationDefinition = "Dolphin doesn't use echolocation. ";
        }
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 4); // Desribe dolphin's sociable behavior: 
        string? sociableBehaviorDefinition = Console.ReadLine();


        /// <summary>
        /// Asking user about playful dolphin's behavior.
        /// </summary>
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 5); // Does the dolphin like playing with another dolphins(Yes or No): 
        string? choicePlayfulBehavior = Console.ReadLine();
        string? playfulBehaviorDefinition;
        bool isPlayfulBehavior;
        
        if(choicePlayfulBehavior == "Yes") {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 6); // Write about dolphin's playful behavior:
            playfulBehaviorDefinition = Console.ReadLine();
            isPlayfulBehavior = true;
        }
        else { 
            isPlayfulBehavior = false;
            playfulBehaviorDefinition = "Dolphin doesn't like playing with dolphins. ";
        }
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 7); // Write the size of dolphin's brain(In cubic centimeters): 
        string? largeBrainAsString = Console.ReadLine();

        /// <summary>
        /// Asking user about dolphin's swimming opportunities.
        /// </summary>
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 8); // Can dolphin swim at high speed?(Write Yes or No): 
        string? choiseSwimHighSpeed = Console.ReadLine();
        string? swimmingAtHighSpeedDefinition;
        bool isSwimmingAtHighSpeed;

        if(choiseSwimHighSpeed == "Yes")
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 9); // Write how dolphin swim at high speed: 
            swimmingAtHighSpeedDefinition = Console.ReadLine();
            isSwimmingAtHighSpeed = true;
        }
        else
        {
            swimmingAtHighSpeedDefinition = "Dolphin doesn't like swim at high speed.";
            isSwimmingAtHighSpeed = false;
        }

        if (name is null) { throw new ArgumentNullException(nameof(name)); }
        if (swimmingAtHighSpeedDefinition is null) { throw new ArgumentNullException(nameof(isSwimmingAtHighSpeed)); }
        if (echolocationDefinition is null) { throw new ArgumentNullException(nameof(echolocationDefinition)); }
        if (sociableBehaviorDefinition is null) { throw new ArgumentNullException(nameof(sociableBehaviorDefinition)); }
        if (playfulBehaviorDefinition is null) { throw new ArgumentNullException(nameof(playfulBehaviorDefinition)); }


        int largeBrain = Convert.ToInt32(largeBrainAsString); 
        int age = Convert.ToInt32(ageAsString);
        
        Dolphin dolphin = new Dolphin(name, age, useEcholocation, echolocationDefinition, sociableBehaviorDefinition, isPlayfulBehavior, 
                                      playfulBehaviorDefinition, largeBrain, isSwimmingAtHighSpeed, swimmingAtHighSpeedDefinition);

        return dolphin;
    }

    #endregion // Private Methods
}