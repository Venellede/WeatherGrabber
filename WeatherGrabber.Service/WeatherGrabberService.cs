using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace WeatherGrabber.Service
{
    public partial class WeatherGrabberService : ServiceBase
    {
        CancellationTokenSource cancellationTokenSource { get; } = new CancellationTokenSource();
        Logger _logger;

        public WeatherGrabberService()
        {
            InitializeComponent();
            _logger = new LogFactory().GetLogger("WeatherGrabber");
        }

        protected override void OnStart(string[] args)
        {
            _logger.Info("Starting process...");
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var grabber = new Grabber();
                    var token = cancellationTokenSource.Token;
                    while (!token.IsCancellationRequested)
                    {
                        await grabber.DoWork(token);
                        await Task.Delay(grabber.Timeout, token);
                    }
                }
                catch(Exception e)
                {
                    _logger.Fatal(e, "Can't process more");
                }
            }, cancellationTokenSource.Token);
        }

        protected override void OnStop()
        {
            _logger.Info("Stopping process...");
            cancellationTokenSource.Cancel();
        }
    }
}
