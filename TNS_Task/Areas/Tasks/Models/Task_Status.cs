using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Task_Status
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int All { get; set; }
        public int NotStarted { get; set; }
        public int NotStarted_Percent { get; set; }
        public int InProcess { get; set; }
        public int InProcess_Percent { get; set; }
        public int Completed { get; set; }
        public int[] Completed_Priority { get; set; }
        public int[] Completed_PriorityPercent { get; set; }
        public int Completed_Percent { get; set; }
        public int Pending { get; set; }
        public int Pending_Percent { get; set; }
        public int Cancel { get; set; }
        public int Cancel_Percent { get; set; }
        public Task_Status(int month, int year, string EmployeeKey)
        {
            this.Month = month;
            this.Year = year;

            All = Models.Report.Task_AmountStatus(Month, Year, EmployeeKey);
            NotStarted = Models.Report.Task_AmountStatus(Month, Year, 1, EmployeeKey);
            InProcess = Models.Report.Task_AmountStatus(Month, Year, 2, EmployeeKey);
            Completed = Models.Report.Task_AmountStatus(Month, Year, 3, EmployeeKey);

            Completed_Priority = new int[5];
            Completed_PriorityPercent = new int[5];
            if (Completed > 0)
            {
                for (int i = 0; i < Completed_Priority.Length; i++)
                {
                    Completed_Priority[i] = Models.Report.Task_AmountStatus(Month, Year, 3, i + 1, EmployeeKey);
                    Completed_PriorityPercent[i] = Completed_Priority[i] * 100 / Completed;
                }

            }

            Pending = Models.Report.Task_AmountStatus(Month, Year, 4, EmployeeKey);
            Cancel = Models.Report.Task_AmountStatus(Month, Year, 5, EmployeeKey);
            if (All > 0)
            {
                NotStarted_Percent = NotStarted * 100 / All;
                InProcess_Percent = InProcess * 100 / All;
                Completed_Percent = Completed * 100 / All;
                Pending_Percent = Pending * 100 / All;
                Cancel_Percent = Cancel * 100 / All;
            }


        }

    }
}
