using HeartRate_PPG.Services;
using System;

using Tizen.Wearable.CircularUI.Forms;

using Xamarin.Forms;

namespace HeartRate_PPG.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private string _caseNumber;
        public string CaseNumber
        {
            get { return _caseNumber; }
            set { _caseNumber = value; }
        }
        private HeartRateMonitorService _heartRate;
        private void Button_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text == "Start")
            {
                button.Text = "Stop";
                _heartRate = new HeartRateMonitorService();
                CaseNumber = _heartRate.Start().ToString();
                CaseTest.Text ="TestCase Number: " +CaseNumber;
                CaseTest.IsVisible = true;
            }
            else
            {
                button.Text = "Start";
                _heartRate.Stop();
                CaseTest.IsVisible = false;

                if (_heartRate != null)
                {
                    _heartRate.Dispose();
                }
            }
        }
    }
}
