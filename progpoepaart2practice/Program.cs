namespace progpoepaart2practice
{
    using global::ProgPoepart2;
    using System;
    using System.Collections.Generic;


    namespace ProgPoePart2
    {

        class Program
        {

            //List <T> variable
            private static List<Recipe> recipes = new List<Recipe>();
            //Global delegate
            private delegate void WarnIfExceedsCaloriesDel(Recipe recipe);

            static void Main(string[] args)
            {
                //Housekeeping
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Welcome to the recipe app");
                Console.WriteLine("\nYou can use this program to develop and manage recipes.");
                Console.WriteLine("You can add ingredients, quantities, procedures, and other details into this program.");

                Console.WriteLine("\nTo get started, use the following commands:");
                Console.WriteLine("- Type 'new' to create a new recipe");
                Console.WriteLine("- Type'list' to display a list of existing recipes");
                Console.WriteLine("- Type 'select <recipe name>' to view or change a specific recipe");
                Console.WriteLine("- Type 'exit' to exit the program");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nLet's get cooking! Enter a command to begin:");

                //This loop allows users to interactively manage recipes through commands, including creating new recipes, listing existing recipes, selecting a recipe by name, and exiting the application.
                bool running = true;
                while (running)
                {
                    Console.WriteLine("\nEnter a command (new, list, select <recipe name>, exit):");
                    string command = Console.ReadLine().ToLower();

                    switch (command)
                    {
                        case "new":
                            CreateNewRecipe();
                            break;
                        case "list":
                            DisplayRecipeList();
                            break;
                        case var _ when command.StartsWith("select"):
                            string recipeName = command.Substring("select".Length).Trim();
                            SelectRecipe(recipeName);
                            break;
                        case "exit":
                            running = false;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Invalid command. Please try again.");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                }
            }

            //This method enables the user to input details for a new recipe, including ingredients and steps, and provides options for manipulating the recipe before exiting or starting a new one.
            private static void CreateNewRecipe()
            {
                Console.WriteLine("Enter the name of the recipe:");
                string name = Console.ReadLine();

                Recipe recipe = new Recipe(name);
                recipes.Add(recipe);

                Console.WriteLine("Enter the number of ingredients:");
                int numIngredients = int.Parse(Console.ReadLine());

                //Loop to run multiple ingrediants
                for (int i = 0; i < numIngredients; i++)
                {
                    Console.WriteLine($"Enter the name of ingredient {i + 1}:");
                    string ingredientName = Console.ReadLine();

                    Console.WriteLine($"Enter the quantity of ingredient {i + 1}:");
                    double quantity = double.Parse(Console.ReadLine());

                    Console.WriteLine($"Enter the unit of measurement for ingredient {i + 1}:");
                    string selectedMeasurements = SelectMeasurement();
                    string unit = selectedMeasurements;

                    Console.WriteLine($"Enter the number of calories for ingredient {i + 1}:");
                    int calories = int.Parse(Console.ReadLine());

                    Console.WriteLine($"Enter the food group for ingredient {i + 1}:");
                    string foodSelect = SelectFoodGroup();
                    string foodGroup = foodSelect;

                    Ingredient ingredient = new Ingredient(ingredientName, quantity, unit, calories, foodGroup);
                    recipe.AddIngredient(ingredient);
                }

                Console.WriteLine("Enter the number of steps:");
                int numSteps = int.Parse(Console.ReadLine());

                for (int i = 0; i < numSteps; i++)
                {
                    Console.WriteLine($"Enter step {i + 1}:");
                    string step = Console.ReadLine();
                    recipe.AddStep(step);
                }

                recipe.DisplayRecipe();
                Console.WriteLine("Total calaories: " + recipe.TotalCalories);

                //Using delegates
                WarnIfExceedsCaloriesDel cd = new WarnIfExceedsCaloriesDel(WarnIfExceedsCalories);
                cd(recipe);

                //WarnIfExceedsCalories(recipe);


                //Loop to run commands to affect recipe and an exit to return to first menu
                bool running = true;
                while (running)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nEnter a command:\n" +
                           "- 'scale': Scale the quantities of the recipe's ingredients.\n" +
                           "- 'reset': Reset the quantities of the recipe's ingredients to their original values.\n" +
                           "- 'clear': Clear the current recipe and start creating a new one.\n" +
                           "- 'exit': Exit the program.");
                    Console.ForegroundColor = ConsoleColor.White;

                    string command = Console.ReadLine().ToLower();

                    switch (command)
                    {
                        case "scale":
                            Console.WriteLine("Enter a scaling factor (0.5, 2, or 3):");
                            double selectFactor = SelectScalingFactor();
                            double factor = selectFactor;
                            recipe.ScaleRecipe(factor);
                            recipe.DisplayRecipe();
                            break;
                        case "reset":
                            recipe.ResetQuantities();
                            recipe.DisplayRecipe();
                            break;
                        case "clear":
                            recipe.ClearRecipe();
                            Console.WriteLine("Recipe cleared. Enter a new recipe.");
                            CreateNewRecipe();
                            running = false;
                            break;
                        case "exit":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid command. Please try again.");
                            break;
                    }
                }
            }


            //This method sorts and displays the list of recipes in alphabetical order, or outputs a message if there are no recipes available.
            private static void DisplayRecipeList()
            {
                if (recipes.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("No recipes found.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                recipes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));

                Console.WriteLine("Recipe List:");
                foreach (Recipe recipe in recipes)
                {
                    Console.WriteLine(recipe.Name);
                }
            }

            //This method searches for a recipe with a given name (case-insensitive) and, if found, displays the recipe and checks for a calorie limit, or outputs a message if the recipe is not found.
            private static void SelectRecipe(string recipeName)
            {
                Recipe selectedRecipe = recipes.Find(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));

                if (selectedRecipe != null)
                {
                    selectedRecipe.DisplayRecipe();


                    WarnIfExceedsCaloriesDel cd = new WarnIfExceedsCaloriesDel(WarnIfExceedsCalories);
                    cd(selectedRecipe);
                    //WarnIfExceedsCalories(selectedRecipe);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("No recipes found.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            //This method alerts the user with a warning message if a given recipe's total calorie count exceeds 300.
            private static void WarnIfExceedsCalories(Recipe recipe)
            {
                if (recipe.TotalCalories > 300)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Warning: This recipe exceeds 300 calories.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }


            //This method presents a list of measurement options and prompts the user to select one by entering a corresponding number, repeatedly asking for input until a valid choice is made, and then returns the selected measurement as a string.
            private static string SelectMeasurement()
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select a measurement:");
                Console.WriteLine("1. Teaspoon");
                Console.WriteLine("2. Tablespoon");
                Console.WriteLine("3. Cup");
                Console.WriteLine("4. Ounce");
                Console.WriteLine("5. Gram");
                Console.ForegroundColor = ConsoleColor.White;

                while (true)
                {
                    Console.Write("Enter the number of your choice: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                return "teaspoon";
                            case 2:
                                return "tablespoon";
                            case 3:
                                return "cup";
                            case 4:
                                return "ounce";
                            case 5:
                                return "gram";
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The information that you have provided is invalid.Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }



            //This method presents a list of food group options and prompts the user to select one by entering a corresponding number, repeatedly asking for input until a valid choice is made, and then returns the selected food group as a string.
            private static string SelectFoodGroup()
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select a food group:");
                Console.WriteLine("1. Grains");
                Console.WriteLine("2. Fruits");
                Console.WriteLine("3. Vegetables");
                Console.WriteLine("4. Protein");
                Console.WriteLine("5. Dairy");
                Console.ForegroundColor = ConsoleColor.White;

                while (true)
                {
                    Console.Write("Enter the number of your choice: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                return "Grains";
                            case 2:
                                return "Fruits";
                            case 3:
                                return "Vegetables";
                            case 4:
                                return "Protein";
                            case 5:
                                return "Dairy";
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("The information that you have provided is invalid.Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            //This method presents a list of scaling options and prompts the user to select one by entering a corresponding number, repeatedly asking for input until a valid choice is made, and then returns the selected scaling factor as a double value.
            private static double SelectScalingFactor()
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select a scaling option:");
                Console.WriteLine("1. Scale to half (0.5)");
                Console.WriteLine("2. Scale to double (2)");
                Console.WriteLine("3. Scale to triple (3)");
                Console.ForegroundColor = ConsoleColor.White;

                while (true)
                {
                    Console.Write("Enter the number of your choice: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                return 0.5;
                            case 2:
                                return 2;
                            case 3:
                                return 3;
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("The information that you have provided is invalid.Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }


        }
    }
}

    


















