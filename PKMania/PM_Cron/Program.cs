using PM_Backend.Hubs;
using PM_BLL.Interfaces;
using PM_BLL.Services;
namespace PM_Cron
{
    public class Program
    {
        //private readonly PkHub _hub;
        //public static AuthService _authService;
        //public Program(PkHub hub, IAuthService authService)
        //{
        //    _hub = hub;
        //    _authService = (AuthService?)authService;
        //}

        static void Main(string[] args)
        {
            Console.WriteLine("Init cron...");
            Task.Delay(5000).Wait();
            //Task ts = PM_Backend.Hubs.PkHub.SendMsgToAllAsync("Cron job initialisé!");
            //PM_BLL.Services.AuthService.testCron();
            //Console.WriteLine("Cron job initialisé...");
        }

      
    }


}