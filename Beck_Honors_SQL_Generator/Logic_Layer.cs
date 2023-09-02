namespace Beck_Honors_SQL_Generator
{
    internal class Logic_Layer
    {
        public static void pass_note(String s)
        {
            Data_Access.appendData(s);
        }
        public class comment_box_gen
        {
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
