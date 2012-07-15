using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MessageRouter.Server.WpfServer.Controller;

namespace MessageRouter.Server.WpfServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        #region Fields

        private readonly WpfServerController _controller = new WpfServerController();

        #endregion

        #region Dependency Properties

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public static readonly DependencyProperty ServerStatusProperty = DependencyProperty.Register(
            "ServerStatusProperty",
            typeof(ServerStatus),
            typeof(MainWindow),
            new PropertyMetadata(ServerStatus.Loading));

        public ServerStatus ServerStatus
        {
            set 
            { 
                SetValue(ServerStatusProperty, value);
                NotifyPropertyChanged("ServerStatus");
            }
            get { return (ServerStatus)GetValue(ServerStatusProperty); }
        }

        #endregion

        #region Constructors

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(StartMachinery));
            MouseDoubleClick += OnMouseDoubleClick;
        }

        #endregion

        #region Private Methods

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _controller.OpenLogsFolder();
        }

        private void StartMachinery()
        {
            if  (_controller.Start())
            {
                ServerStatus = ServerStatus.Running;
                ToolTip = "Message router is running...\n\nDouble click to open logs folder...";
            }
            else
            {
                ServerStatus = ServerStatus.FailedToStart;
                ToolTip = "Message router is failed with error.\n\nDouble click to open logs folder...";
            }
            
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            MouseDoubleClick -= OnMouseDoubleClick;
            _controller.TearDown();
            _controller.Dispose();
            Close();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion
    }

    public class ServerStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var typedValue = ServerStatus.Loading;
            if (value != null)
            {
                typedValue = (ServerStatus) value;
            }

            switch (typedValue)
            {
                case ServerStatus.FailedToStart:
                    return Colors.Red;
                case ServerStatus.Loading:
                    return Colors.Gainsboro;
                case ServerStatus.Running:
                    return Colors.DodgerBlue;
                default:
                    throw new NotSupportedException(string.Format("Unknown status: {0}", typedValue));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("The method or operation is not supported.");
        }
    }

    public class ServerStatusDarkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var typedValue = ServerStatus.Loading;
            if (value != null)
            {
                typedValue = (ServerStatus)value;
            }

            switch (typedValue)
            {
                case ServerStatus.FailedToStart:
                    return Colors.DarkRed;
                case ServerStatus.Loading:
                    return Colors.Gray;
                case ServerStatus.Running:
                    return Colors.Navy;
                default:
                    throw new NotSupportedException(string.Format("Unknown status: {0}", typedValue));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("The method or operation is not supported.");
        }
    }

    public enum ServerStatus
    {
        Loading,
        Running,
        FailedToStart
    }
}
