using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Beck_Honors_SQL_Generator.Logic_Layer;

namespace Beck_Honors_SQL_Generator
{
    internal class Data_Objects
    {
        public static class database
        {
            public static String print_database_header()
            {
                //Databae header to ensure we don't run into conflicts.
                String database_name = settings.database_name;
                String header_text = "DROP DATABASE IF EXISTS " + database_name + ";\n"
                    + "CREATE DATABASE " + database_name + ";\n"
                    + "USE " + database_name + ";\n";
                return header_text;
            }
        }
        public class Column
        {

            public String column_name { get; set; }
            public String data_type { get; set; }
            public int length { get; set; }
            public String default_value { get; set; }
            public Boolean isIidentity { get; set; }
            public int id_start { get; set; }
            public int id_increment { get; set; }

            public Boolean isNullable { get; set; }
            public string index { get; set; }

            public Boolean isUnique { get; set; }
            public Boolean isPrimaryKey { get; set; }
            public Boolean isForeignKey { get; set; }
            public String integrity { get; set; }
            public String fkReferences { get; set; }
            public String description { get; set; }

            public String columnText{ get; set; }
            public List<String> primaryKeys { set; get; }
            public List<String> foreignKeys { set; get; }
            //public String length_text = "";



            public Column(string column_name, string data_type, int length, string default_value, Boolean identity, int id_start, int id_increment,
                Boolean nullable, string index, Boolean unique, Boolean primary_key, Boolean foreign_key, string integrity, string references, string description)
            {
                this.column_name = column_name;
                this.data_type = data_type;
                this.length = length;
                this.default_value = default_value;
                this.isIidentity = identity;
                this.id_start = id_start;
                this.id_increment = id_increment;
                this.isNullable = nullable;
                this.index = index;
                this.isUnique = unique;
                this.isPrimaryKey = primary_key;
                this.isForeignKey = foreign_key;
                this.integrity = integrity;
                this.fkReferences = references;
                this.description = description;
                if (data_type.Equals("nvarchar") && length == 0) { length = 50; }
                if (data_type.Equals("date")) { length = 0; }
               
            }

            public String column_and_key_gen()
            {
                primaryKeys = new List<String>();
                foreignKeys = new List<String>();
                String Column_text = "";
                String length_text;
                if (length == 0) { length_text = ""; }
                else { length_text = "(" + length + ")"; }
                Column_text = Column_text + column_name + "\t";
                Column_text = Column_text + data_type + length_text + "\t";
                if (isNullable) { Column_text = Column_text + "null\t"; }
                else { Column_text = Column_text + "not null\t"; }
                //if (auto_increment.Equals('Y') || auto_increment.Equals('y')) { Column_text = Column_text + "auto_increment\t"; }
                if (isUnique) { Column_text = Column_text + "unique\t"; }
                if (isPrimaryKey) { primaryKeys.Add(column_name); }
                if (isForeignKey) { foreignKeys.Add(fkReferences); }
                Column_text = Column_text + "comment \'" + description + "\'\n";
                return Column_text;

            }

            public String audit_column_gen()
            {
                String Column_text = "";
                String length_text;
                if (length == 0) { length_text = ""; }
                else { length_text = "(" + length + ")"; }
                Column_text = Column_text + column_name + "\t";
                Column_text = Column_text + data_type + length_text + "\t";
                if (isNullable)  { Column_text = Column_text + "null\t"; }
                else { Column_text = Column_text + "not null\t"; }
                if (isUnique) { Column_text = Column_text + "unique\t"; }
                Column_text = Column_text + "comment \'" + description + "\'\n";
                return Column_text;


            }

        }
        public class table
        {
            public String name { set; get; }
            public header Header { set; get; }
            public List<Column> columns { set; get; }
            public List<String> primary_keys { set; get; }
            public List<String> foreign_keys { set; get; }
            public table(String name, List<Column> Columns)
            {
                this.name = name;
                this.columns = columns;

            }


            public String gen_primary_keys()
            {
                String key_string = ",CONSTRAINT " + name + "_PK PRIMARY KEY (";
                int count = 0;
                foreach (Column c in columns)
                {
                    foreach (string s in c.primaryKeys)
                    {
                        if (count > 0) { key_string = key_string + " , "; }
                        key_string = key_string + s;
                        count++;
                    }
                }
                key_string = key_string + ")\n";

                return key_string;
            }
            public String gen_foreign_keys()
            {
                foreign_keys = new List<String>();
                String output_keys = "";
                foreach (Column c in columns)
                {
                    foreach (string s in c.foreignKeys)
                    {
                        String[] chunks = s.Split(' ');
                        String second_table = chunks[1];
                        String formatted_key = ",CONSTRAINT " + name + "_" + second_table + " foreign key (" + c.column_name + ") " + s + "\n";

                        foreign_keys.Add(formatted_key);
                    }
                };

                foreach (string tuv in foreign_keys)
                {
                    String s = tuv;
                    output_keys = output_keys + s;
                }

                output_keys = output_keys + ");";
                return output_keys;
            }

            public String gen_header()
            {
                Header.table_name = this.name;
                return this.Header.full_header_gen();
            }

            public String audit_gen_header()
            {
                Header.table_name = this.name;
                return this.Header.audit_header_gen();
            }
            public String gen_columns()
            {

                int count = 0;
                String x = this.gen_header();
                x = x + "\n";
                foreach (Column c in columns)
                {
                    String Columntext = c.column_and_key_gen();
                    if (count > 0) { x = x + ","; }
                    x = x + Columntext;
                    count++;
                }
                ;


                return x;
            }

            public String gen_audit_table()
            {
                int count = 0;
                String x = this.audit_gen_header();
                x = x + "\n";
                foreach (Column c in columns)
                {
                    String columntext = c.audit_column_gen();
                    if (count > 0) { x = x + ","; }
                    x = x + columntext;
                    count++;
                }
                x = x + ",action_type VARCHAR(50) NOT NULL COMMENT 'insert update or delete'\n" +
                    ", action_date DATETIME NOT NULL COMMENT 'when it happened'\n" +
                    ", action_user VARCHAR(255) NOT NULL COMMENT 'Who did it'\n";
                x = x + ");\n";


                return x;
            }
            public String gen_update()
            {
                
                String comment_text = comment_box_gen.comment_box(name, 3);
                String function_text =
                     "DROP PROCEDURE IF EXISTS sp_update_" + name + ";\n"
                    + "DELIMITER $$\n"
                    + "CREATE PROCEDURE sp_update_" + name + "\n"
                    + "(";
                int count = 0;
                String comma = "";
                foreach (Column c in columns)
                {
                    String length_text;
                    if (c.length == 0) { length_text = ""; }
                    else { length_text = "(" + c.length + ")"; }
                    if (count > 0) { comma = ","; }
                    String add = comma + "in " + c.column_name + "_param " + c.data_type + length_text + "\n";
                    function_text = function_text + add;
                    count++;
                }
                function_text = function_text + ")\n" +
                    "begin \n" +
                    "declare sql_error TINYINT DEFAULT FALSE;\n" +
                    "declare update_count tinyint default 0;\n" +
                    "DECLARE CONTINUE HANDLER FOR SQLEXCEPTION\n" +
                    "SET sql_error = true;\n" +
                    "START TRANSACTION;\n" +
                    "UPDATE " + name + "\n set "
                    ;
                comma = "";
                count = 0;
                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    if (!c.isPrimaryKey)
                    {
                        String add = comma + c.column_name + " = " + c.column_name + "_param\n";
                        function_text = function_text + add;
                        count++;
                    }
                }
                int keys_count = 0;
                String initial_word = "WHERE ";
                foreach (Column c in columns)
                {
                    if (c.isPrimaryKey)
                    {
                        if (keys_count > 0) { initial_word = "AND "; }
                        String add = initial_word + c.column_name + "=" + c.column_name + "_param\n";
                        function_text = function_text + add;
                        keys_count++;
                    }
                }
                function_text = function_text + "\n" +
                   " ; if sql_error = FALSE then \n" +
                   " SET update_count = Column_count(); \n" +
                   " COMMIT;\n" +
                   " ELSE\n" +
                   " SET update_count = 0;\n" +
                   " ROLLBACK;\n" +
                   " END IF;\n" +
                   " select update_count as 'update count'\n" +
                   " ; END $$\n" +
                   " DELIMITER ;\n";

                String full_text = comment_text + function_text;
                return full_text;

            }

            public String gen_delete()
            {

                String comment_text = comment_box_gen.comment_box(name, 4);
                String function_text =
                     "DROP PROCEDURE IF EXISTS sp_delete_" + name + ";\n"
                    + "DELIMITER $$\n"
                    + "CREATE PROCEDURE sp_delete_" + name + "\n"
                    + "(";
                int count = 0;
                String comma = "";
                comma = "";
                count = 0;
                foreach (Column c in columns)
                {
                    String length_text;
                    if (c.length == 0) { length_text = ""; }
                    else { length_text = "(" + c.length + ")"; }
                    if (count > 0) { comma = ","; }
                    if (c.isPrimaryKey)
                    {
                        String add = comma + c.column_name + "_param " + c.data_type + length_text + "\n";
                        function_text = function_text + add;
                        count++;
                    }
                }
                count = 0;
                function_text = function_text + ")\n" +
                    "begin \n" +
                    "declare sql_error TINYINT DEFAULT FALSE;\n" +
                    "declare update_count tinyint default 0;\n" +
                    "DECLARE CONTINUE HANDLER FOR SQLEXCEPTION\n" +
                    "SET sql_error = true;\n" +
                    "START TRANSACTION;\n" +
                    "DELETE FROM " + name + "\n  "
                    ;
                comma = "";
                int keys_count = 0;
                String initial_word = "WHERE ";
                foreach (Column c in columns)
                {
                    if (c.isPrimaryKey)
                    {
                        if (keys_count > 0) { initial_word = "AND "; }
                        String add = initial_word + c.column_name + "=" + c.column_name + "_param\n";
                        function_text = function_text + add;
                        keys_count++;
                    }
                }
                function_text = function_text + "\n" +
                   " ; if sql_error = FALSE then \n" +
                   " SET update_count = Column_count(); \n" +
                   " COMMIT;\n" +
                   " ELSE\n" +
                   " SET update_count = 0;\n" +
                   " ROLLBACK;\n" +
                   " END IF;\n" +
                   " select update_count as 'update count'\n" +
                   " ; END $$\n" +
                   " DELIMITER ;\n";

                String full_text = comment_text + function_text;
                return full_text;




            }

            public String gen_retreive_by_key()
            {

                String comment_text = comment_box_gen.comment_box(name, 5);
                String function_text =
                     "DROP PROCEDURE IF EXISTS sp_retreive_by_pk_" + name + ";\n"
                    + "DELIMITER $$\n"
                    + "CREATE PROCEDURE sp_retreive_by_pk_" + name + "\n"
                    + "(\n";

                int count = 0;
                String comma = "";
                comma = "";
                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    String length_text;
                    if (c.length == 0) { length_text = ""; }
                    else { length_text = "(" + c.length + ")"; }
                    if (c.isPrimaryKey)
                    {
                        String add = comma + c.column_name + "_param " + c.data_type + length_text + "\n";
                        function_text = function_text + add;
                        count++;
                    }
                }
                function_text = function_text + ")";

                count = 0;
                comma = "";
                function_text = function_text + "\n Begin \n select \n";
                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + c.column_name + " \n";
                    count++;
                }
                function_text = function_text + "\n FROM " + name + "\n";
                String initial_word = "where ";
                int keys_count = 0;
                foreach (Column c in columns)
                {
                    if (c.isPrimaryKey)
                    {
                        if (keys_count > 0) { initial_word = "AND "; }
                        String add = initial_word + c.column_name + "=" + c.column_name + "_param\n";
                        function_text = function_text + add;
                        keys_count++;
                    }
                }

                function_text = function_text + " ; END $$\n" +
                   " DELIMITER ;\n";



                String full_text = comment_text + function_text;
                return full_text;
            }


            public String gen_retreive_by_all()
            {
                
                String comment_text = comment_box_gen.comment_box(name, 6);
                String function_text =
                     "DROP PROCEDURE IF EXISTS sp_retreive_by_all_" + name + ";\n"
                    + "DELIMITER $$\n"
                    + "CREATE PROCEDURE sp_retreive_by_all_" + name + "()\n";

                int count = 0;
                String comma = "";
                comma = "";
                count = 0;
                function_text = function_text + "begin \n SELECT \n";
                count = 0;
                comma = "";

                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + "\n" + comma + c.column_name;
                    count++;
                }
                function_text = function_text + "\n FROM " + name + "\n ;\n END $$ \n DELIMITER ;\n";


                String full_text = comment_text + function_text;
                return full_text;
            }
            public string gen_insert()
            {
                String comment_text = comment_box_gen.comment_box(name, 7);
                String function_text =
                     "DROP PROCEDURE IF EXISTS sp_insert_" + name + ";\n"
                    + "DELIMITER $$\n"
                    + "CREATE PROCEDURE sp_insert_" + name + "(\n";

                int count = 0;
                String comma = "";
                foreach (Column c in columns)
                {
                    String length_text;
                    if (c.length == 0) { length_text = ""; }
                    else { length_text = "(" + c.length + ")"; }
                    if (count > 0) { comma = ","; }
                    String add = comma + "in " + c.column_name + "_param " + c.data_type + length_text + "\n";
                    function_text = function_text + add;
                    count++;
                }
                function_text = function_text + ")\n" +
                    "begin \n" +
                    "declare sql_error TINYINT DEFAULT FALSE;\n" +
                    "declare update_count tinyint default 0;\n" +
                    "DECLARE CONTINUE HANDLER FOR SQLEXCEPTION\n" +
                    "SET sql_error = true;\n" +
                    "START TRANSACTION;\n" +
                    "INSERT INTO  " + name + "\n values \n("
                    ;
                count = 0;
                comma = "";
                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    String add = comma + c.column_name + "_param" + "\n";
                    function_text = function_text + add;
                    count++;
                }

                function_text = function_text + ")\n" +
                   " ; if sql_error = FALSE then \n" +
                   " SET update_count = Column_count(); \n" +
                   " COMMIT;\n" +
                   " ELSE\n" +
                   " SET update_count = 0;\n" +
                   " ROLLBACK;\n" +
                   " END IF;\n" +
                   " select update_count as 'update count'\n" +
                   " ; END $$\n" +
                   " DELIMITER ;\n";




                String full_text = comment_text + function_text;
                return full_text;
            }
            public String gen_update_trigger()
            {
                String comment_text = comment_box_gen.comment_box(name, 8);
                String function_text = "DELIMITER $$\n"
                    + "DROP TRIGGER IF EXISTS tr_" + name + "_after_update $$\n"
                    + "CREATE TRIGGER tr_" + name + "_after_update\n"
                    + "AFTER UPDATE ON " + name + "\n"
                    + "for each Column\n"
                    + "begin\n"
                    + "insert into" + name + "_audit (\n";
                int count = 0;
                String comma = "";

                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + c.column_name + " \n";
                    count++;
                }
                function_text = function_text + "\n, action_type"
                    + "\n, action_date"
                     + "\n, action_user"
                    + "\n) values(\n";
                count = 0;
                comma = "";

                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + "new." + c.column_name + " \n";
                    count++;
                }
                function_text = function_text + "\n , 'update'-- action_type"
                     + "\n, NOW()-- action_date"
                    + "\n,  CURRENT_USER()-- action_user"
                    + "\n)"
                    + "\n;"
                    + "\nend  $$"
                    + "\nDELIMITER ;"
                    + "\n   ;\n";

                String full_text = comment_text + function_text;
                return full_text;
            }
            public String gen_insert_trigger()
            {
                String comment_text = comment_box_gen.comment_box(name, 9);
                String function_text = "DELIMITER $$\n"
                    + "DROP TRIGGER IF EXISTS tr_" + name + "_after_insert $$\n"
                    + "CREATE TRIGGER tr_" + name + "_after_insert\n"
                    + "AFTER insert ON " + name + "\n"
                    + "for each Column\n"
                    + "begin\n"
                    + "insert into" + name + "_audit (\n";
                int count = 0;
                String comma = "";

                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + c.column_name + " \n";
                    count++;
                }
                function_text = function_text + "\n, action_type"
                    + "\n, action_date"
                     + "\n, action_user"
                    + "\n) values(\n";
                count = 0;
                comma = "";

                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + "new." + c.column_name + " \n";
                    count++;
                }
                function_text = function_text + "\n , 'insert'-- action_type"
                     + "\n, NOW()-- action_date"
                    + "\n,  CURRENT_USER()-- action_user"
                    + "\n)"
                    + "\n;"
                    + "\nend  $$"
                    + "\nDELIMITER ;"
                    + "\n   ;\n";

                String full_text = comment_text + function_text;
                return full_text;
            }

            public String gen_delete_trigger()
            {
                String comment_text = comment_box_gen.comment_box(name, 10);
                String function_text = "DELIMITER $$\n"
                    + "DROP TRIGGER IF EXISTS tr_" + name + "_after_delete $$\n"
                    + "CREATE TRIGGER tr_" + name + "_after_delete\n"
                    + "AFTER delete ON " + name + "\n"
                    + "for each Column\n"
                    + "begin\n"
                    + "insert into" + name + "_audit (\n";
                int count = 0;
                String comma = "";

                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + c.column_name + " \n";
                    count++;
                }
                function_text = function_text + "\n, action_type"
                    + "\n, action_date"
                     + "\n, action_user"
                    + "\n) values(\n";
                count = 0;
                comma = "";

                foreach (Column c in columns)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + "old." + c.column_name + " \n";
                    count++;
                }
                function_text = function_text + "\n , 'delete'-- action_type"
                     + "\n, NOW()-- action_date"
                    + "\n,  CURRENT_USER()-- action_user"
                    + "\n)"
                    + "\n;"
                    + "\nend  $$"
                    + "\nDELIMITER ;"
                    + "\n   ;\n";

                String full_text = comment_text + function_text;
                return full_text;
            }

            public String gen_functions()
            {
                string x = " ";

                return x;


            }

        }
        public class header
        {
            public String table_name { get; set; }
            public String description { get; set; }
            public string full_header { get; private set; }


            public header(String table_name, String description)
            {
                full_header = "";
            }
            public String full_header_gen()
            {
                full_header = "";
                full_header = comment_box_gen.comment_box(table_name, 1) +
                    "\n\n" +
                    "DROP TABLE IF EXISTS " + table_name + ";\n\n" +
                    "CREATE TABLE " + table_name + "(\n\n"
                    ;


                return full_header;
            }

            public string audit_header_gen()
            {
                String audit_header = "";
                audit_header = comment_box_gen.comment_box(table_name, 2) +
                    "\n\n" +
                    "DROP TABLE IF EXISTS " + table_name + "_audit;\n\n" +
                    "CREATE TABLE " + table_name + "_audit(\n\n"
                    ;


                return audit_header;

            }

        }
        public class data_tables
        {
            public static List<table> all_tables = new List<table>();
        }
        public class comment_box_gen
        {
            //creates various commeent boxes based on table name and function type.
            public static String comment_box(String table, int type)
            {
                String full_comment_box = " ";
                String start_stars = "/******************\n";
                String end_stars = "\n***************/\n";
                String middle = " ";
                if (type == 1)
                {
                    middle = "Create the " + table + " table";

                }
                if (type == 2)
                {
                    middle = "Create the " + table + " Audit table";
                }
                if (type == 3)
                {
                    middle = "Create the update script for the " + table + " table";
                }
                if (type == 4)
                {
                    middle = "Create the delete script for the " + table + " table";
                }
                if (type == 5)
                {
                    middle = "Create the retreive by key script for the " + table + " table";
                }

                if (type == 6)
                {
                    middle = "Create the retreive by all script for the " + table + " table";
                }
                if (type == 7)
                {
                    middle = "Create the insert script for the " + table + " table";
                }
                if (type == 8)
                {
                    middle = "Create the insert trigger script for the " + table + " table";
                }
                if (type == 9)
                {
                    middle = "Create the update trigger script for the " + table + " table";
                }

                if (type == 10)
                {
                    middle = "Create the delete trigger script for the " + table + " table";
                }


                full_comment_box = start_stars + middle + end_stars;
                return full_comment_box;
            }
        }

    }
}


