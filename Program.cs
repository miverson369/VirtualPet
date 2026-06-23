using System;
using System.IO;
using System.Text.Json;


namespace VirtualPetApp
{
    public class Program
    {
        private const string DefaultSaveFile = "pet_save.json";

        public static void Main()
        {
            string saveFile = DefaultSaveFile;

            // Check if saved pet exists
            Pet pet = Pet.Load(saveFile);

            if (pet == null)
            {
                Console.Write("Enter your pet's name: ");
                string name = Console.ReadLine();
                pet = new Pet(name);
                Console.WriteLine("New pet created!");
            }

            while (true)
            {

                Console.Clear();
                Console.WriteLine($"Name: {pet.Name}");
                Console.WriteLine($"Age: {pet.Age}");
                Console.WriteLine($"Hunger: {pet.Hunger}");
                Console.WriteLine($"Happiness: {pet.Happiness}");
                Console.WriteLine($"Mood: {pet.CheckMood()}"); // if you prefer the string-returning helper
                if (!string.IsNullOrEmpty(pet.Image))
                {
                    Console.WriteLine("Image:");
                    Console.WriteLine(pet.Image);
                }

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Feed");
                Console.WriteLine("2. Play");
                Console.WriteLine("3. Save & Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                {
                    switch (choice)
                    {
                        case "1":
                            pet.Feed();
                            break;
                        case "2":
                            pet.Play();
                            break;
                        case "3":
                            pet.Save(saveFile);
                            Console.WriteLine("Game saved. Goodbye!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    pet.Age++;
                    pet.UpdateMood();
                    pet.UpdateStage();
                }
            }
        }
    }
}

    