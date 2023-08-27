using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;
using System.Diagnostics;

namespace SampleHierarchies.Gui;

/// <summary>
/// Mammals main screen.
/// </summary>
public sealed class DogsScreen : Screen
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
    private readonly string jsonFileName = "EnDogsScreenLines.json";


    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService"></param>
    /// <param name="screenDefinitionService"></param>
    public DogsScreen(IDataService dataService,
           ScreenDefinitionService screenDefinitionService)
    {
        _dataService = dataService;
        _screenDefinitionService = screenDefinitionService;

    }

    #endregion // Properties And Ctor

    #region Public Methods

    /// <inheritdoc/>
    public override void Show()
    {
        while (true)
        {

            Console.Clear();
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 0); // Screen history: MainScreen -> AnimalsScreen -> MammalsScreen -> DogsScreen
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 0); // Your available choices are:
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 1); //  0. Exit
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 2); //  1. List all dogs
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 3); //  2. Create a new dog
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 4); //  3. Delete existing dog
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 5); //  4. Modify existing dog
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 6); //  Please enter your choise: 

            string? choiceAsString = Console.ReadLine();

            // Validate choice
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                DogsScreenChoices choice = (DogsScreenChoices)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case DogsScreenChoices.List:
                        ListDogs();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DogsScreenChoices.Create:
                        AddDog();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DogsScreenChoices.Delete:
                        DeleteDog();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DogsScreenChoices.Modify:
                        EditDogMain();
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Menu, 1); // Press any key to continue
                        Console.ReadKey();
                        break;

                    case DogsScreenChoices.Exit:
                        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 7); // Going back to parent menu.
                        Thread.Sleep(500);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Show, 8); // Invalid choice. Try again.
            }
        }
    }

    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// List all dogs.
    /// </summary>
    private void ListDogs()
    {
        if (_dataService?.Animals?.Mammals?.Dogs is not null &&
            _dataService.Animals.Mammals.Dogs.Count > 0)
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 0); // Here's the list of all dogs
            int i = 1;
            foreach (Dog dog in _dataService.Animals.Mammals.Dogs.Cast<Dog>())
            {
                Console.Write(i);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 1); // - is dog's number
                dog.Display();
                i++;
            }
        }
        else
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.List, 2); // The list of dogs is empty
        }
    }

    /// <summary>
    /// Add a dog.
    /// </summary>
    private void AddDog()
    {
        try
        {
            Dog dog = AddEditDog();
            _dataService?.Animals?.Mammals?.Dogs?.Add(dog);
            Console.Write(dog.Name);
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 0); // dog was recently added to a list of dogs 
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Add, 1); // Invalid input.
        }
    }

    /// <summary>
    /// Deletes a dog.
    /// </summary>
    private void DeleteDog()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 0); // What is the name of the dog you want to delete?
            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Dog? dog = (Dog?)(_dataService?.Animals?.Mammals?.Dogs
                       ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
            if (dog is not null)
            {
                _dataService?.Animals?.Mammals?.Dogs?.Remove(dog);
                Console.WriteLine(dog.Name);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 1); // Dog with that name was deleted from a list of dogs
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 2); // Dog wasn't found 
            }
        }
        catch
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Delete, 3); // Invalid input
        }
    }

    /// <summary>
    /// Edits an existing dog after choice made.
    /// </summary>
    private void EditDogMain()
    {
        try
        {
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 0); // What is the name of the dog you want to edit?
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 1); // Please, enter it's name
            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Dog? dog = (Dog?)(_dataService?.Animals?.Mammals?.Dogs
                       ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
            if (dog is not null)
            {
                Dog dogEdited = AddEditDog();
                dog.Copy(dogEdited);
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 2); // Dog after editing
                dog.Display();
            }
            else
            {
                _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 3); // Dog was not found
            }

            Console.ResetColor();
        }
        catch
        { 
            _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.Modify, 4);  // Invalid input. Try again.
        }
    }

    /// <summary>
    /// Add/edit specific dog.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private Dog AddEditDog()
    {
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 0); // What name of the dog?
        string? name = Console.ReadLine();
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 1); // What is the dog's age?
        string? ageAsString = Console.ReadLine();
        _screenDefinitionService.ShowLines(jsonFileName, ScreenLineEnum.AddEdit, 2); // What is the dog's breed? 
        string? breed = Console.ReadLine();

        if (name is null) { throw new ArgumentNullException(nameof(name)); }
        if (ageAsString is null) { throw new ArgumentNullException(nameof(ageAsString)); }
        if (breed is null) { throw new ArgumentNullException(nameof(breed)); }

        int age = Int32.Parse(ageAsString);
        Dog dog = new Dog(name, age, breed);

        return dog;
    }

    #endregion // Private Methods
}