using System.ComponentModel.DataAnnotations.Schema;
using static Beck_Honors_SQL_Generator.Data_Objects;

namespace Beck_Honors_SQL_Generator
{
    public static class Data_Access
    {
        // to learn the file path
        public static string update_path = settings.update_notes_path;

        // to open the file
        static StreamWriter writer = new StreamWriter(update_path, true);

        //to append data
        public static bool appendData(String s)
        {
            String append = DateTime.Now.ToString() + "\t " + s;
            writer.WriteLine(append);
            writer.Flush();

            return true;
        }

        // to close
        public static void closestream()
        {
            writer.Flush();
        }
    }
    public class file_read
    {
        String readpath = settings.applicationPath;

        public static void readdata()
        {
            int skip_count = 0;
            // Read the file selected by user and setup various varables
            StreamReader SqlBuddy = new StreamReader(settings.applicationPath);
            string ln;
            string tablename = " ";
            string description;
            char[] separator = { '\t' };
            char[] audit_seperator = { ' ' };
            List<Column> Columns = new List<Column>();
            int count = 0;
            SqlBuddy.ReadLine();
            //skip first line since it's just heading data
            // read until the end
            while ((ln = SqlBuddy.ReadLine()) != null)
            {

                string[] parts;
                //split each line in the file into it's component parts
                parts = ln.Split(separator);
                try
                {
                    //catch a fully blank line and ignore it
                    if (parts[0].Length == 0 && parts[1].Length == 0)
                    {
                        skip_count++;
                        continue;

                    }
                }
                catch { IndexOutOfRangeException e; }


                try
                {
                    //parts[1] is blank for the first Column of a table, so this catches the table name
                    if (parts[1].Length == 0 && count == 0)
                    {

                        tablename = parts[0];
                        description = parts[14];
                        count++;
                    }

                    //this means we are moving onto a new table, so it creates the old table, then clears the Columns array

                    else if (parts[1].Length == 0 && count > 0)
                    {
                        table t = new table(tablename, Columns);
                        header h = new header(tablename, parts[14]);
                        t.Header = h;
                        List<Column> Columnsfortable = new List<Column>();
                        foreach (Column _Column in t.columns)
                        {
                            Columnsfortable.Add(_Column);
                        }
                        t.columns = Columnsfortable;

                        Columns.Clear();
                        tablename = parts[0];
                        description = parts[14];
                        database.allTables.Add(t);
                        count++;
                    }
                }
                catch { IndexOutOfRangeException ex; }
                //if parts[1] is greater than zero, that means we are reading a Column object which has a data type. 
                //read all Column objects and place them into an array of Columns, to later be turned into a table

                if (parts[1].Length > 0)
                {
                    //remove any extra punctionation excel added and setup various variables
                    String Column_name = parts[0].Replace("\"", "").Trim(); 
                    int length;
                    int start;
                    int increment;                 

                    //read each part of the Column 
                    String data_type = parts[1].Replace("\"", "");
                    Int32.TryParse(parts[2], out length);
                    String default_value = parts[3];
                    Boolean isIdentity = false;
                    Boolean isNullable = false;
                    Boolean isUnique = false;
                    Boolean isPrimaryKey= false;
                    Boolean isForeignKey = false;
                    if (parts[4].Equals("yes") || parts[4].Equals("Yes")) { isIdentity = true; }
                    
                    Int32.TryParse(parts[5], out start);
                    Int32.TryParse(parts[6], out increment);

                    //nullable relates to parts 7
                    if (parts[7].Equals("yes") || parts[7].Equals("Yes")) { isNullable = true; }
                    String index = parts[8];
                    //unique relates to parts 9
                    if (parts[9].Equals("yes") || parts[9].Equals("Yes")) { isUnique = true; }
                    //pk relates to parts 10
                    if (parts[10].Equals("Yes") || parts[10].Equals("yes")) { isPrimaryKey = true; }
                    //fk releates to parts 11
                    if (parts[11].Equals("Yes") || parts[11].Equals("yes")) { isForeignKey = true; }



                    String integrity = parts[12];
                    String references = parts[13];

                    description = parts[14];
                    //create the Column
                    Column _Column = new Column(Column_name, data_type, length, default_value, isIdentity, start, increment,
                         isNullable, index, isUnique, isPrimaryKey, isForeignKey, integrity, references, description);
                    //add Column to Column array
                    Columns.Add(_Column);
                }

            }
            //figure out how many tables were created
            settings.table_count = database.allTables.Count;
            //generate default options for all tables
            settings.generate_options();
        }
    }
    public static class file_write
    {
        //the output file is in the same folder as the input folder, just with "sql" appended to the file name
        static string newPath = settings.applicationPath.Substring(0, settings.applicationPath.Length - 4) + "sql.txt";
        public static StreamWriter WriteBuddy = new StreamWriter(newPath);
    }
}

