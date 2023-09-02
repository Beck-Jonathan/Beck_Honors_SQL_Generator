using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Beck_Honors_SQL_Generator { 
    public static class Data_Access
    {
        // to learn the file path
        public static string update_path = settings.update_notes_path;

        // to open the file
        static StreamWriter writer = new StreamWriter(update_path,true);

        //to append data
        public static bool appendData(String s) {
            String append = DateTime.Now.ToString() + "\t "+s;
            writer.WriteLine(append);
            writer.Close();
            
            return true;
        }

        // to close
        public static void closestream() {
            writer.Flush();
        }
    }
}
