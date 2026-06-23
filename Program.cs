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

                {
                    Console.WriteLine("Image:");
                    Console.WriteLine(pet.Image);
                }

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Feed");
                Console.WriteLine("2. Play");
                Console.WriteLine("3. Save & Exit");
                Console.WriteLine("4. Create Party Room");
                Console.WriteLine("5. Join Party Room");
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
                            Console.WriteLine("Option 4 selected.");

                            try
                            {
                                Console.WriteLine("Creating room...");

                                PetPartyApi api = new PetPartyApi();
                                string roomCode = await api.CreateRoomAsync(pet);

                                Console.WriteLine($"Room created! Code: {roomCode}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Could not create room.");
                                Console.WriteLine(ex.Message);
                            }

                            Console.WriteLine("Press Enter to return to the main menu");
                            Console.ReadLine();
                            break;

                        case "5":
                            try
                            {
                                Console.Write("Enter room code: ");
                                string code = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(code) || code.Length != 6)
                                {
                                    Console.WriteLine("Room code must be 6 characters.");
                                    break;
                                }

                                Console.WriteLine("Joining room...");

                                PetPartyApi api = new PetPartyApi();
                                VisitorPet visitor = await api.JoinRoomAsync(code);

                                if (visitor == null)
                                {
                                    Console.WriteLine("No visitor pet was returned.");
                                }
                                else
                                {
                                    Console.WriteLine($"Visitor joined: {visitor.Name}");
                                    Console.WriteLine($"Visitor image: {visitor.Image}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Could not join room.");
                                Console.WriteLine(ex.Message);
                            }

                            Console.WriteLine("Press Enter to return to the menu...");
                            Console.ReadLine();
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

    