using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VirtualPetApp;


namespace VirtualPet
{
    public class PetPartyApi
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://virtualpetparty.up.railway.app")
        };

        private const string ApiKey = "?api-key=foo";

        public async Task<string> CreateRoomAsync(Pet pet)
        {
            var data = new
            {
                name = pet.Name,
                image = pet.Image
            };

            HttpResponseMessage response =
                await client.PostAsJsonAsync("/api/room/create" + ApiKey, data);

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)

            {
                throw new Exception(result);
            }
            return result.Trim('"');
        }

        public async Task<VisitorPet> JoinRoomAsync(string roomId)
        {
            HttpResponseMessage response =
                await client.PostAsync($"/api/room/join/{roomId}{ApiKey}", null);

            string json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(json);
            }

            RoomResponse room = JsonSerializer.Deserialize<RoomResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (room == null || room.Visitor == null)
            {
                throw new Exception("The API did not return a visitor pet.");
            }

            return room.Visitor;
        }
        
    }
}

public class RoomResponse
    {
        public VisitorPet Visitor { get; set; }
    }

    public class VisitorPet
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }




