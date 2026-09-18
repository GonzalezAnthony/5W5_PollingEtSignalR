using labo.signalr.api.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace labo.signalr.api.Hubs
{
    public class TaskHub : Hub
    {
        ApplicationDbContext _context;

        public TaskHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            await Clients.Caller.SendAsync("TaskList", _context.UselessTasks.ToList());
        }

        public async Task AddTask(string task)
        {
            _context.UselessTasks.Add(new Models.UselessTask() { Text = task });
            _context.SaveChanges();
            await Clients.All.SendAsync("TaskList", _context.UselessTasks.ToList());
        }

        public async Task CompleteTask(int taskId)
        {
            var task = _context.UselessTasks.Single(t => t.Id == taskId);
            task.Completed = true;
            _context.SaveChanges();
            await Clients.All.SendAsync("TaskList", _context.UselessTasks.ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            base.OnDisconnectedAsync(exception);
            // TODO: Ajouter votre logique

        }
    }
}
