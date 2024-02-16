// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using System.Text;

var firstNameAppendix = new List<string>(["Leia",
    "Sadie",
    "Jose",
    "Sara",
    "Frank",
    "Dewey",
    "Tomas",
    "Joel",
    "Lukas",
    "Carlos"]);

var lastNameAppendix = new List<string>(["Liberty",
    "Ray",
    "Harrison",
    "Ronan",
    "Drew",
    "Powell",
    "Larsen",
    "Chan",
    "Anderson"]);

var currentId = 1;

var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5286/");

var tasks = new List<Task>();
for (int i = 0; i < 10; i++)
{
    var stringContent = new StringContent(MountRequest(), UnicodeEncoding.UTF8, "application/json");
    tasks.Add(client.PostAsync("customers", stringContent));
}
await Task.WhenAll(tasks);


string MountRequest()
{
    Random r = new Random();
    var data = new List<object>
    {
        new
        {
            Age = r.Next(10, 90),
            FirstName = firstNameAppendix[r.Next(firstNameAppendix.Count)],
            LastName = lastNameAppendix[r.Next(lastNameAppendix.Count)],
            Id = currentId++
        },
        new
        {
            Age = r.Next(10, 90),
            FirstName = firstNameAppendix[r.Next(firstNameAppendix.Count)],
            LastName = lastNameAppendix[r.Next(lastNameAppendix.Count)],
            Id = currentId++
        }
    };
    return JsonConvert.SerializeObject(data);
}


