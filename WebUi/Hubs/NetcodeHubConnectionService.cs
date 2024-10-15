using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebUi.Hubs
{
    public class NetcodeHubConnectionService
    {
        private readonly HubConnection _hubConnection;

        public NetcodeHubConnectionService(NavigationManager navigationManager) // Le paramètre est injecté ici
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("/communicationhub")) // Utilisation de l'instance injectée de navigationManager
                .Build();
            _hubConnection.StartAsync();
        }

        public HubConnection GetHubConnection() => _hubConnection;
    }
}
