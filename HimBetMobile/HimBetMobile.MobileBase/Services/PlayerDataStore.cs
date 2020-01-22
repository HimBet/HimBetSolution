using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using HimBetMobile.Models;
using HemBit.Model;
using System.Net;

namespace HimBetMobile.Services
{
    public class PlayerDataStore : IDataStore<Player>
    {
        HttpClient client;
        IEnumerable<Player> items;

        public PlayerDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

            items = new List<Player>();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<IEnumerable<Player>> GetItemsAsync(bool forceRefresh = false)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (forceRefresh && IsConnected)
            {
                var json = await client.GetStringAsync($"api/Player");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Player>>(json));
            }

            return items;
        }

        public async Task<Player> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                var json = await client.GetStringAsync($"api/Player/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Player>(json));
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Player player)
        {
            if (player == null || !IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(player);

            var response = await client.PostAsync($"api/Player", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Player player)
        {
            if (player == null || player.Id == null || !IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(player);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/Player/{player.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/Player/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
