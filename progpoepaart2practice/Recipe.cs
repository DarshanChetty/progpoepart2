using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
namespace ProgPoepart2
{
    class Recipe
    {
        //List <T> variables
        private List<Ingredient> ingredients = new List<Ingredient>();
        private List<string> steps = new List<string>();

        //This property is a public read-only property of type 'string' that provides external access to retrieve the value of the property without allowing modifications.
        public string Name { get; }

        //This property calculates and returns the total number of calories for a recipe by summing the calorie values of its ingredients.
        public int TotalCalories
        {
            get
            {
                int totalCalories = 0;
                foreach (Ingredient ingredient in ingredients)
                {
                    totalCalories += ingredient.Calories;
                }
                return totalCalories;
            }
        }

        //This constructor creates a new 'Recipe' object with a specified name and assigns the name to the 'Name' property of the recipe.
        public Recipe(string name)
        {
            Name = name;
        }

        //This method adds the provided 'Ingredient' object to the list of ingredients within the recipe.
        public void AddIngredient(Ingredient ingredient)
        {
            ingredients.Add(ingredient);
        }

        //This method appends the provided 'step' to the list of steps within the recipe.
        public void AddStep(string step)
        {
            steps.Add(step);
        }

        //This method presents the recipe's name, ingredients with their quantities and units, and the steps in a formatted manner when called.
        public void DisplayRecipe()
        {
            Console.WriteLine($"Recipe: {Name}");

            Console.WriteLine("Ingredients:");
            foreach (Ingredient ingredient in ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} {ingredient.Name}");
            }

            Console.WriteLine("\nSteps:");
            for (int i = 0; i < steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {steps[i]}");
            }
        }

        // Scales the quantity of each ingredient in the recipe by a given factor
        public void ScaleRecipe(double factor)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }


        //Resets the quantity of each ingredient in the recipe to its original value
        public void ResetQuantities()
        {
            foreach (Ingredient ingredient in ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity;
            }
        }

        //clears the ingredients list and steps list
        public void ClearRecipe()
        {
            ingredients.Clear();
            steps.Clear();
        }


    }


    //This class encapsulates properties related to an ingredient, such as its name, quantity, unit, calories, food group, and original quantity, along with a constructor to set these properties during object creation.
    class Ingredient
    {
        public string Name { get; }
        public double Quantity { get; set; }
        public string Unit { get; }
        public int Calories { get; }
        public string FoodGroup { get; }
        public double OriginalQuantity { get; set; }

        public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
            OriginalQuantity = quantity;
        }
    }
}
