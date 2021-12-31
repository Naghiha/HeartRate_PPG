﻿using HeartRate_PPG.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tizen.Sensor;

namespace HeartRate_PPG.Services
{
    // For more information about sensors see https://docs.tizen.org/application/dotnet/guides/location-sensors/device-sensors
    public class HeartRateMonitorService : IDisposable
    {
        private HeartRateMonitorLEDGreenBatch _sensor;

        private bool _disposed = false;
        private int _testCase;
        private SQLiteConnection _sQLite;

        /// <summary>
        /// Initializes the sensor
        /// </summary>
        /// <exception cref="NotSupportedException">The device does not support the sensor</exception>
        /// <exception cref="UnauthorizedAccessException">The user does not grant your app access to sensors</exception>
        public HeartRateMonitorService()
        {
            try
            {
                _sQLite = DatabaseService.NewConnection();
                // A NotSupportedException will be thrown if the sensor is not available on the device
                _sensor = new HeartRateMonitorLEDGreenBatch();

                // Add an event handler to the sensor
                _sensor.DataUpdated += sensor_DataUpdated;

                // TODO: Declare how the sensor behaves when the screen turns off or the device goes into power save mode
                // For details see https://docs.tizen.org/application/dotnet/guides/location-sensors/device-sensors
                // _sensor.PausePolicy = SensorPausePolicy.All;

                // TODO: Set the update interval in milliseconds
                // _sensor.Interval = 1000;
            }
            catch (NotSupportedException)
            {
                // TODO: The device does not support the sensor, handle exception as appropriate to your scenario
            }
            catch (UnauthorizedAccessException)
            {
                // TODO: The user does not grant your app access to sensors, handle exception as appropriate to your scenario
            }
        }
        /// <summary>
        /// Called when the heart rate has changed
        /// </summary>
        private void sensor_DataUpdated(object sender, HeartRateMonitorLEDGreenBatchDataUpdatedEventArgs e)
        {
            string data = "";
            foreach (var item in e.Data)
            {
                data += item.AccelerationX.ToString() + ";" + item.AccelerationY.ToString() + ";" + item.AccelerationZ +
                    ";" + item.Green + ";" + item.Timestamp + ";" + ((int)item.Accuracy).ToString() + ';' + item.Index.ToString() + "\n";
                PPGRawDataModel rowDataModel = new PPGRawDataModel();
                rowDataModel.Green = int.Parse(item.Green.ToString());
                rowDataModel.Index = int.Parse(item.Index.ToString());
                rowDataModel.TestCaseId = _testCase;
                rowDataModel.Timestamp = item.Timestamp.ToString();
                rowDataModel.AccelerationX = item.AccelerationX;
                rowDataModel.AccelerationY = item.AccelerationY;
                rowDataModel.AccelerationZ = item.AccelerationZ;
                rowDataModel.Accuracy = ((int)item.Accuracy);
                rowDataModel.CreatedDate = DateTime.Now;
                _sQLite.Insert(rowDataModel);
            }
        }

        ~HeartRateMonitorService()
        {
            Dispose(false);
        }

        /// <summary>
        /// Starts the sensor to receive sensor data
        /// </summary>
        public int Start()
        {
            try
            {
                _sQLite.CreateTable<PPGRawDataModel>();
                _sQLite.CreateTable<TestCase>();
            }
            catch (Exception exp)
            {

                return 0;
            }
            var newCase = new TestCase();
            newCase.RegisterDate = DateTime.Now;
            _sQLite.Insert(newCase);
            _testCase = newCase.TestCaseId;
            
             _sensor.Start();
            return _testCase;
        }

        /// <summary>
        /// Stops receiving sensor data
        /// </summary>
        /// <remarks>
        /// Reduce battery drain by stopping the sensor when it is not needed
        /// </remarks>
        public void Stop()
        {
            _testCase = 0;
            _sensor.Stop();
        }

        /// <summary>
        /// Releases all resources used by the current instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the current instance
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        _sensor.DataUpdated -= sensor_DataUpdated;
                        _sensor.Dispose();
                    }

                    _sensor = null;
                    _disposed = true;
                }
                catch (Exception)
                {

                    
                }
            }
        }

        

    }
}
