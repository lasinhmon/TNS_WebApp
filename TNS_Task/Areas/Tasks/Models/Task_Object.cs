using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Task_Object
    {
        public Task_Record Info { get; set; }
        public List<Comment_Record> ListComment { get; set; }
        public Task_Object()
        {
            Info = new Task_Record();

            ListComment = AccessData.Task_Comment(null);
            //AttactFiles = AccessData.Task_File(null);
        }
        public Task_Object(string Key)
        {
            Info = new Task_Record(Key);
            //Comments = AccessData.Task_Comment(Key);
            //AttactFiles = AccessData.Task_File(Key);
        }
    }
}
