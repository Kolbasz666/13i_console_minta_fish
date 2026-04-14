using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace fish;

public class ServerConnection
{
    HttpClient client = new();
    string Token = null;
    public ServerConnection(string url)
    {
        //http://127.1.1.1:3000
        client.BaseAddress = new Uri(url);
    }
    public async Task<List<Fish>> GetFish()
    {
        try
        {
            HttpResponseMessage resonse = await client.GetAsync("/fish");
            resonse.EnsureSuccessStatusCode();
            //const result = await response.json()
            string responseString = await resonse.Content.ReadAsStringAsync();
            List<Fish> result = JsonSerializer.Deserialize<List<Fish>>(responseString);
            return result;
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            return null;
        }
    }
    public async Task<List<Fish>> GetMyFish()
    {
        try
        {
            HttpResponseMessage resonse = await client.GetAsync("/myfish");
            resonse.EnsureSuccessStatusCode();
            //const result = await response.json()
            string responseString = await resonse.Content.ReadAsStringAsync();
            List<Fish> result = JsonSerializer.Deserialize<List<Fish>>(responseString);
            return result;
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            return null;
        }
    }
    public async Task PostFish(string name, double weight)
    {
        try
        {
            Fish newFish = new Fish(name, weight);
            string jsonString = JsonSerializer.Serialize(newFish);
            StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage resonse = await client.PostAsync("/fish", sendThis);
            resonse.EnsureSuccessStatusCode();
            //const result = await response.json()
            Console.WriteLine("Sikeres létrehozás :)");
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
        }
    }
    public async Task<bool> Login(string username, string password)
    {
        try
        {
            User newUser = new User(username, password);
            string jsonString = JsonSerializer.Serialize(newUser);
            StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage resonse = await client.PostAsync("/login", sendThis);
            resonse.EnsureSuccessStatusCode();
            string responseString = await resonse.Content.ReadAsStringAsync();
            Token result = JsonSerializer.Deserialize<Token>(responseString);
            client.DefaultRequestHeaders.Add("authorization", result.token);
            return true;
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            return false;
        }
    }
    public void Logout() {
        client.DefaultRequestHeaders.Clear();
    }
}
