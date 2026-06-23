using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VirtualPetApp
{
    public class SaveManager
    {
        private const string FileName = "pet.json";

            // Save the pet to a JSON file
            public static bool SavePet(Pet pet)
        {
                try
                {
                    if (pet == null)
                        throw new ArgumentNullException(nameof(pet), "Pet cannot be null.");

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Converters = { new JsonStringEnumConverter() }
                    };

                    string json = JsonSerializer.Serialize(pet, options);
                    File.WriteAllText(FileName, json);

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving pet: {ex.Message}");
                    return false;
                }
            }

            // Load the pet from a JSON file
            public static Pet LoadPet()
            {
                try
                {
                    if (!File.Exists(FileName))
                        return null;

                    string json = File.ReadAllText(FileName);

                    var options = new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() }
                    };

                    Pet loadedPet = JsonSerializer.Deserialize<Pet>(json, options);

                    return loadedPet;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading pet: {ex.Message}");
                    return null;
                }
            }
        }
    }

