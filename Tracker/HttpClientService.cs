using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class HttpClientService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "https://localhost:5001/api/task"; // Ganti port jika beda

    public HttpClientService()
    {
        _client = new HttpClient();
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        var tasks = await _client.GetFromJsonAsync<List<TaskModel>>(_baseUrl);
        return tasks ?? new List<TaskModel>();
    }

    public async Task<bool> CreateTaskAsync(TaskModel task)
    {
        var response = await _client.PostAsJsonAsync(_baseUrl, task);
        return response.IsSuccessStatusCode;
    }
}
