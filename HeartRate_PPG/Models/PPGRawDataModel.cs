using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRate_PPG.Models
{
    public class PPGRawDataModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int TestCaseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AccelerationX { get; set; }
        public int AccelerationY { get; set; }
        public int AccelerationZ { get; set; }
        public int Green { get; set; }
        public string Timestamp { get; set; }
        public int Accuracy { get; set; }
        public int Index { get; set; }
    }

    public class TestCase
    {
        [PrimaryKey, AutoIncrement]
        public int TestCaseId { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
