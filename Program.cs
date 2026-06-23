using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using VirtualPet;


namespace VirtualPetApp
{
    public class Program
    {
        private const string DefaultSaveFile = "pet_save.json";

        public static async Task Main()
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

                    Console.WriteLine("4. Create Party Room");
                    Console.WriteLine("5. Join Party Room");
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

                        case "4":
                            PetPartyApi api = new PetPartyApi();
                            string roomCode = await api.CreateRoomAsync(pet);
                            Console.WriteLine($"Room created! Code: {roomCode}");
                            break;

                        case "5":
                            Console.Write("Enter room code: ");
                            string code = Console.ReadLine();

                            PetPartyApi joinApi = new PetPartyApi();
                            VisitorPet visitor = await joinApi.JoinRoomAsync(code);

                            Console.WriteLine($"Visitor joined: {visitor.Name}");
                            Console.WriteLine($"Visitor image: {visitor.Image}");
                            break;

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

    