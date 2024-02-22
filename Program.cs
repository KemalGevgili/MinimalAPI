using BusinessLayer.Models;
using BusinessLayer.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var userService = new UserService();

app.MapGet("/", () => "Welcome to TAIMinAPI!");

app.MapGet("/api/users", async context =>
{
    var users = userService.GetAllUsers();
    await context.Response.WriteAsJsonAsync(users);
});

app.MapGet("/api/users/{id}", async context =>
{
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    var user = userService.GetUserById(id);

    if (user != null)
    {
        await context.Response.WriteAsJsonAsync(user);
    }
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync($"User with ID {id} not found.");
    }
});

app.MapGet("/api/users/add/{name}/{surname}", context =>
{
    var name = context.Request.RouteValues["name"].ToString();
    var surname = context.Request.RouteValues["surname"].ToString();

    var newUser = new UserModel { Name = name, Surname = surname };
    userService.AddUser(newUser);

    return context.Response.WriteAsync($"User added successfully.");
});

app.MapGet("/api/users/update/{id}/{name}/{surname}", context =>
{
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    var name = context.Request.RouteValues["name"].ToString();
    var surname = context.Request.RouteValues["surname"].ToString();

    var existingUser = userService.GetUserById(id);

    if (existingUser != null)
    {
        existingUser.Name = name;
        existingUser.Surname = surname;
        userService.UpdateUser(existingUser);

        return context.Response.WriteAsync($"User with ID: {existingUser.Id} updated successfully.");
    }
    else
    {
        context.Response.StatusCode = 404;
        return context.Response.WriteAsync($"User with ID {id} not found.");
    }
});

app.MapGet("/api/users/delete/{id}", context =>
{
    var id = int.Parse(context.Request.RouteValues["id"].ToString());

    var existingUser = userService.GetUserById(id);

    if (existingUser != null)
    {
        userService.DeleteUser(id);
        context.Response.StatusCode = 200;
        return context.Response.WriteAsync($"User with {id} is deleted successfully.");
    }
    else
    {
        context.Response.StatusCode = 404;
        return context.Response.WriteAsync($"User with ID {id} not found.");
    }
});

var port = 5001; // Replace with your desired port
app.Run($"http://localhost:{port}");

