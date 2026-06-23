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

            string roomCode = await response.Content.ReadAsStringAsync();
            return roomCode.Trim('"');
        }
              
        public async Task<VisitorPet> JoinRoomAsync(string roomId)
        {
            var data = new { };

            HttpResponseMessage response =
                await client.PostAsJsonAsync("/api/room/join/" + roomId + ApiKey, data);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            RoomResponse room = JsonSerializer.Deserialize<RoomResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return room.Visitor;
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
}


