using System.Net.NetworkInformation;

namespace Beck_Honors_SQL_Generator
{
    public static class settings
    {
        public static String update_notes_path = "C:\\Users\\jjbec\\source\\repos\\Beck_Honors_SQL_Generator\\change_log.txt";
        public static String applicationPath;
        public static String databaseName;
        public static bool TSQLMode;

        public static String path;
        public static int table_count;
        public static List<List<Boolean>> all_options = new List<List<Boolean>>();
        public static List<String> table_names = new List<String>();
        //public static int table_count;
        public static void generate_options()
        {
            for (int i = 0; i < table_count; i++)
            {
                List<Boolean> options = new List<Boolean>();
                for (int j = 0; j < 3; j++)
                {
                    options.Add(true);
                }
                for (int k = 3; k < 12; k++)
                {
                    options.Add(false);
                }
                all_options.Add(options);
            }

        }

    }
}
