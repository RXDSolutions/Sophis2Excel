﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Windows;
using RxdSolutions.FusionLink.Properties;

namespace RxdSolutions.FusionLink.Client
{
    public class DiagnosticsViewModel : INotifyPropertyChanged
    {
        private readonly ServiceHost _serviceHost;
        private readonly RealTimeDataServer _dataServer;

        private int _portfolioSubscriptionCount;
        private int _systemSubscriptionCount;
        private int _positionSubscriptionCount;
        private int _portfolioPropertySubscriptionCount;
        private int _instrumentPropertySubscriptionCount;
        private int _clientCount;
        private int _lastTimeTaken;
        private long _refreshTimeTaken;
        private int _numberOfRefreshes;
        
        public DiagnosticsViewModel(ServiceHost serviceHost, RealTimeDataServer dataServer)
        {
            _serviceHost = serviceHost;

            _dataServer = dataServer;
            _dataServer.OnSubscriptionChanged += OnSubscriptionChanged;
            _dataServer.OnClientConnectionChanged += OnClientConnectionChanged;
            _dataServer.OnDataReceived += OnDataReceived;
            _dataServer.OnPublishQueueChanged += OnPublishQueueChanged;
            _dataServer.OnStatusChanged += OnStatusChanged;
            
            PortfolioSubscriptionCount = _dataServer.PortfolioValueSubscriptionCount;
            InstrumentPropertySubscriptionCount = _dataServer.InstrumentPropertySubscriptionCount;
            SystemSubscriptionCount = _dataServer.SystemValueCount;
            PositionSubscriptionCount = _dataServer.PositonValueSubscriptionCount;
            PortfolioPropertySubscriptionCount = _dataServer.PortfolioPropertySubscriptionCount;
            ClientCount = _dataServer.ClientCount;

            ToggleDataServer = new DelegateCommand(ExecuteToggleDataServer);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DelegateCommand ToggleDataServer { get; }

        public int ClientCount
        {
            get { return _clientCount; }
            set
            {
                _clientCount = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get { return _dataServer.IsRunning; }
        }

        public int LastTimeTaken
        {
            get { return _lastTimeTaken; }
            set
            {
                _lastTimeTaken = value;
                OnPropertyChanged();
            }
        }

        public long AverageTimeTaken
        {
            get
            {
                if (_numberOfRefreshes == 0)
                    return 0;

                return Convert.ToInt64(_refreshTimeTaken / _numberOfRefreshes);
            }
        }

        public int PublishQueueCount
        {
            get
            {
                return _dataServer.PublishQueueCount;
            }
        }

        public string ServerUri
        {
            get 
            {
                var listener = _serviceHost.ChannelDispatchers
                                        .OfType<ChannelDispatcher>()
                                        .Where(x => x.BindingName == "http://tempuri.org/:NetTcpBinding")
                                        .First()
                                        .Listener
                                        .Uri
                                        .ToString();

                return listener;
            }
        }

        public bool IsTerminalServices
        {
            get
            {
                return System.Windows.Forms.SystemInformation.TerminalServerSession;
            }
        }

        public int PortfolioSubscriptionCount
        {
            get { return _portfolioSubscriptionCount; }
            set
            {
                _portfolioSubscriptionCount = value;
                OnPropertyChanged();
            }
        }

        public int SystemSubscriptionCount
        {
            get { return _systemSubscriptionCount; }
            set
            {
                _systemSubscriptionCount = value;
                OnPropertyChanged();
            }
        }

        public int PositionSubscriptionCount
        {
            get { return _positionSubscriptionCount; }
            set
            {
                _positionSubscriptionCount = value;
                OnPropertyChanged();
            }
        }

        public int InstrumentPropertySubscriptionCount
        {
            get { return _instrumentPropertySubscriptionCount; }
            set
            {
                _instrumentPropertySubscriptionCount = value;
                OnPropertyChanged();
            }
        }

        public int PortfolioPropertySubscriptionCount
        {
            get { return _portfolioPropertySubscriptionCount; }
            set
            {
                _portfolioPropertySubscriptionCount = value;
                OnPropertyChanged();
            }
        }

        public string ToggleDataServerLabel
        {
            get { return IsRunning ? Resources.StopButtonText : Resources.StartButtonText; }
        }

        private void OnPropertyChanged([CallerMemberName] string callerMemberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerMemberName));
        }

        private void OnDataReceived(object sender, DataAvailableEventArgs e)
        {
            LastTimeTaken = Convert.ToInt32(e.TimeTaken.TotalMilliseconds);
            _refreshTimeTaken += Convert.ToInt32(e.TimeTaken.TotalMilliseconds);
            _numberOfRefreshes++;

            OnPropertyChanged(nameof(AverageTimeTaken));            
        }

        private void OnPublishQueueChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(PublishQueueCount));
        }

        private void OnClientConnectionChanged(object sender, ClientConnectionChangedEventArgs e)
        {
            ClientCount = _dataServer.ClientCount;
        }

        private void OnSubscriptionChanged(object sender, EventArgs e)
        {
            PortfolioSubscriptionCount = _dataServer.PortfolioValueSubscriptionCount;
            SystemSubscriptionCount = _dataServer.SystemValueCount;
            PositionSubscriptionCount = _dataServer.PositonValueSubscriptionCount;
            PortfolioPropertySubscriptionCount = _dataServer.PortfolioPropertySubscriptionCount;
            InstrumentPropertySubscriptionCount = _dataServer.InstrumentPropertySubscriptionCount;
        }

        private void ExecuteToggleDataServer(object sender)
        {
            try
            {
                if (_dataServer == null)
                    return;

                if (_dataServer.IsRunning)
                    _dataServer.Stop();
                else
                    _dataServer.Start();

                OnPropertyChanged(nameof(IsRunning));
                OnPropertyChanged(nameof(ToggleDataServerLabel));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnStatusChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsRunning));
            OnPropertyChanged(nameof(ToggleDataServerLabel));
        }
    }
}
