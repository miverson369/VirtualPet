using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using VirtualPetApp;

namespace VirtualPetApp
{
    public class Pet

    {
        public string Name { get; set; }
        public int Hunger { get; set; } 
        public int Happiness { get; set; }
        public int Age { get; set; }
        public PetMood Mood { get; set; }
        public PetStage Stage { get; set; }
        public string Image { get; set; } = "(^_^) happy pet";
        public Pet()
        { }


        public Pet(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Pet" : name;
            Age = 0;
            Mood = PetMood.Happy;
            Stage = PetStage.Baby;

        }
     
        public void Feed()
        {
            Hunger = Math.Max(Hunger - 20, 0);
            Happiness = Math.Min(Happiness + 5, 100);
            UpdateMood();
            UpdateImage();
        }
        public void Play()
        {
            Happiness = Math.Min(Happiness + 15, 100);
            Hunger = Math.Min(Hunger + 10, 100);
            UpdateMood();
            UpdateImage();
        }
        public void PassTime()
        {
            Hunger = Math.Min(Hunger + 10, 100);
            Age++;
            UpdateMood();
            UpdateStage();
            UpdateImage();
        }
        public void UpdateMood()
        {
            if (Hunger >= 80)
                Mood = PetMood.Hungry;
            else if (Happiness <= 30)
                Mood = PetMood.Sad;
            else
                Mood = PetMood.Happy;
        }
        public void UpdateStage()
        {
            if (Age < 5)
                Stage = PetStage.Baby;
            else if (Age < 10)
                Stage = PetStage.Teen;
            else
                Stage = PetStage.Adult;
        } 
        public void UpdateImage()
        {
            if (Mood == PetMood.Hungry)
                Image = "(>_<) hungry pet";
            else if (Mood == PetMood.Sad)
                Image = "(-_-) sad pet";
            else
                Image = "(^_^) happy pet";
        }

        // Check and describe mood
        public string CheckMood()
        {
            if (Happiness > 70) return $"{Name} is happy! 😊";
            else if (Happiness > 40) return $"{Name} is okay. 😐";
            else return $"{Name} is sad... 😢";
        }

        // Update pet state over time
        public void Tick()
        {
            Hunger += 5;       // gets hungrier over time
            Happiness -= 3;    // loses happiness over time
            if (Hunger > 100) Hunger = 100;
            if (Happiness < 0) Happiness = 0;
        }
        public static Pet Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return null;

                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<Pet>(json, options);
            }
            catch
            {
                // If deserialization fails, return null so the caller can create a new pet
                return null;
            }
        }
        public void Save(string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(path, json);
        }
    }
}

    


    


