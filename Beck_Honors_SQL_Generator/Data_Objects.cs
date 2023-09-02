using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Beck_Honors_SQL_Generator.Logic_Layer;

namespace Beck_Honors_SQL_Generator
{
    internal class Data_Objects
    {
        public class Row
        {

            public String row_name { get; set; }
            public String data_type { get; set; }
            public int length { get; set; }
            public String default_value { get; set; }
            public String identity { get; set; }
            public int start { get; set; }
            public int increment { get; set; }

            public char nullable { get; set; }
            public string index { get; set; }

            public char unique { get; set; }
            public char primary_key { get; set; }
            public String foreign_key { get; set; }
            public String integrity { get; set; }
            public String references { get; set; }
            public String description { get; set; }

            public String row_text { get; set; }
            public List<String> primary_keys { set; get; }
            public List<String> foreign_keys { set; get; }
            public String length_text = "";


            public Row(string row_name, string data_type, int length, string default_value, string identity, int start, int increment,
                char nullable, string index, char unique, char primary_key, string foreign_key, string integrity, string references, string description)
            {
                this.row_name = row_name;
                this.data_type = data_type;
                this.length = length;
                this.default_value = default_value;
                this.identity = identity;
                this.start = start;
                this.increment = increment;
                this.nullable = nullable;
                this.index = index;
                this.unique = unique;
                this.primary_key = primary_key;
                this.foreign_key = foreign_key;
                this.integrity = integrity;
                this.references = references;
                this.description = description;
                if (data_type.Equals("nvarchar") && length == 0) { length = 50; }
                if (data_type.Equals("date")) { length = 0; }
                if (length == 0)
                {
                    length_text = "";
                }
                if (length > 0)
                {
                    length_text = "(" + length + ")";
                };
            }

            public String row_and_key_gen()
            {
                primary_keys = new List<String>();
                foreign_keys = new List<String>();
                String row_text = "";
                row_text = row_text + row_name + "\t";
                row_text = row_text + data_type + length_text + "\t";
                if (nullable.Equals('Y') || nullable.Equals('y')) { row_text = row_text + "null\t"; }
                else { row_text = row_text + "not null\t"; }
                //if (auto_increment.Equals('Y') || auto_increment.Equals('y')) { row_text = row_text + "auto_increment\t"; }
                if (unique.Equals('Y') || unique.Equals('y')) { row_text = row_text + "unique\t"; }
                if (primary_key.Equals('Y') || primary_key.Equals('y')) { primary_keys.Add(row_name); }
                if (foreign_key.Length > 1) { foreign_keys.Add(foreign_key); }
                row_text = row_text + "comment \'" + description + "\'\n";
                return row_text;

            }

            public String audit_row_gen()
            {
                String row_text = "";
                row_text = row_text + row_name + "\t";
                row_text = row_text + data_type + length_text + "\t";
                if (nullable.Equals('Y') || nullable.Equals('y')) { row_text = row_text + "null\t"; }
                else { row_text = row_text + "not null\t"; }
                if (unique.Equals('Y') || unique.Equals('y')) { row_text = row_text + "unique\t"; }
                row_text = row_text + "comment \'" + description + "\'\n";
                return row_text;


            }

        }
        public class table
        {
            public String name { set; get; }
            public header Header { set; get; }
            public List<Row> rows { set; get; }
            public List<String> primary_keys { set; get; }
            public List<String> foreign_keys { set; get; }
            public table(String name, List<Row> rows)
            {
                this.name = name;
                this.rows = rows;

            }


            public String gen_primary_keys()
            {
                String key_string = ",CONSTRAINT " + name + "_PK PRIMARY KEY (";
                int count = 0;
                foreach (Row r in rows)
                {
                    foreach (string s in r.primary_keys)
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
                foreach (Row r in rows)
                {
                    foreach (string s in r.foreign_keys)
                    {
                        String[] chunks = s.Split(' ');
                        String second_table = chunks[1];
                        String formatted_key = ",CONSTRAINT " + name + "_" + second_table + " foreign key (" + r.row_name + ") " + s + "\n";

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
            public String gen_rows()
            {

                int count = 0;
                String x = this.gen_header();
                x = x + "\n";
                foreach (Row r in rows)
                {
                    String rowtext = r.row_and_key_gen();
                    if (count > 0) { x = x + ","; }
                    x = x + rowtext;
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
                foreach (Row r in rows)
                {
                    String rowtext = r.audit_row_gen();
                    if (count > 0) { x = x + ","; }
                    x = x + rowtext;
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
                String x = " ";
                String comment_text = comment_box_gen.comment_box(name, 3);
                String function_text =
                     "DROP PROCEDURE IF EXISTS sp_update_" + name + ";\n"
                    + "DELIMITER $$\n"
                    + "CREATE PROCEDURE sp_update_" + name + "\n"
                    + "(";
                int count = 0;
                String comma = "";
                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    String add = comma + "in " + r.row_name + "_param " + r.data_type + r.length_text + "\n";
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
                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    if (!r.primary_key.Equals('y') && !r.primary_key.Equals('Y'))
                    {
                        String add = comma + r.row_name + " = " + r.row_name + "_param\n";
                        function_text = function_text + add;
                        count++;
                    }
                }
                int keys_count = 0;
                String initial_word = "WHERE ";
                foreach (Row r in rows)
                {
                    if (r.primary_key.Equals('y') || r.primary_key.Equals('Y'))
                    {
                        if (keys_count > 0) { initial_word = "AND "; }
                        String add = initial_word + r.row_name + "=" + r.row_name + "_param\n";
                        function_text = function_text + add;
                        keys_count++;
                    }
                }
                function_text = function_text + "\n" +
                   " ; if sql_error = FALSE then \n" +
                   " SET update_count = row_count(); \n" +
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
                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    if (r.primary_key.Equals('y') || r.primary_key.Equals('Y'))
                    {
                        String add = comma + r.row_name + "_param " + r.data_type + r.length_text + "\n";
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
                foreach (Row r in rows)
                {
                    if (r.primary_key.Equals('y') || r.primary_key.Equals('Y'))
                    {
                        if (keys_count > 0) { initial_word = "AND "; }
                        String add = initial_word + r.row_name + "=" + r.row_name + "_param\n";
                        function_text = function_text + add;
                        keys_count++;
                    }
                }
                function_text = function_text + "\n" +
                   " ; if sql_error = FALSE then \n" +
                   " SET update_count = row_count(); \n" +
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
                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    if (r.primary_key.Equals('y') || r.primary_key.Equals('Y'))
                    {
                        String add = comma + r.row_name + "_param " + r.data_type + r.length_text + "\n";
                        function_text = function_text + add;
                        count++;
                    }
                }
                function_text = function_text + ")";

                count = 0;
                comma = "";
                function_text = function_text + "\n Begin \n select \n";
                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + r.row_name + " \n";
                    count++;
                }
                function_text = function_text + "\n FROM " + name + "\n";
                String initial_word = "where ";
                int keys_count = 0;
                foreach (Row r in rows)
                {
                    if (r.primary_key.Equals('y') || r.primary_key.Equals('Y'))
                    {
                        if (keys_count > 0) { initial_word = "AND "; }
                        String add = initial_word + r.row_name + "=" + r.row_name + "_param\n";
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
                String gx = " ";
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

                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + "\n" + comma + r.row_name;
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
                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    String add = comma + "in " + r.row_name + "_param " + r.data_type + r.length_text + "\n";
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
                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    String add = comma + r.row_name + "_param" + "\n";
                    function_text = function_text + add;
                    count++;
                }

                function_text = function_text + ")\n" +
                   " ; if sql_error = FALSE then \n" +
                   " SET update_count = row_count(); \n" +
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
                    + "for each row\n"
                    + "begin\n"
                    + "insert into" + name + "_audit (\n";
                int count = 0;
                String comma = "";

                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + r.row_name + " \n";
                    count++;
                }
                function_text = function_text + "\n, action_type"
                    + "\n, action_date"
                     + "\n, action_user"
                    + "\n) values(\n";
                count = 0;
                comma = "";

                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + "new." + r.row_name + " \n";
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
                    + "for each row\n"
                    + "begin\n"
                    + "insert into" + name + "_audit (\n";
                int count = 0;
                String comma = "";

                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + r.row_name + " \n";
                    count++;
                }
                function_text = function_text + "\n, action_type"
                    + "\n, action_date"
                     + "\n, action_user"
                    + "\n) values(\n";
                count = 0;
                comma = "";

                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + "new." + r.row_name + " \n";
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
                    + "for each row\n"
                    + "begin\n"
                    + "insert into" + name + "_audit (\n";
                int count = 0;
                String comma = "";

                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + r.row_name + " \n";
                    count++;
                }
                function_text = function_text + "\n, action_type"
                    + "\n, action_date"
                     + "\n, action_user"
                    + "\n) values(\n";
                count = 0;
                comma = "";

                foreach (Row r in rows)
                {
                    if (count > 0) { comma = ","; }
                    function_text = function_text + comma + "old." + r.row_name + " \n";
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
    }
}


