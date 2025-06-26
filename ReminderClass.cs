using System.Collections;
using System.Text.RegularExpressions;

namespace poe_part3
{
    public class ReminderClass
    {

        //delaring a global variable
        private string DescriptionReturn = string.Empty;
        private ArrayList TaskDescription = new ArrayList();
        private ArrayList TaskDate = new ArrayList();


        //get users tasks
        public string UserTask(string Task)
        {
            //replace task 
            string replace_AddTask = Task.Replace("add task", "");

            //asign task with the description
            DescriptionReturn = replace_AddTask;

            //returning the message
            return "task added successfully, with the task description " + DescriptionReturn + " \nWould you like a reminder?";
        }//end of method

        //method to set the reminder for today 
        public string TodayReminder()
        {
            //get todays date
            DateTime currentDate = DateTime.Now.Date;

            //storing todays task
            TaskDescription.Add(DescriptionReturn);

            TaskDate.Add(currentDate);

            return "Great, I will remind you about " + DescriptionReturn + " today";
        }//end of method

        //method to check if the task was added before
        public string TaskCheck(string TaskChecking)
        {
            //if we checking the task based on either it was added or not
            if (TaskChecking == "check" && DescriptionReturn != "")
            {
                return "found";
            }
            else if (TaskChecking == "clear")
            {
                DescriptionReturn = string.Empty;



            }

            return "";

        }//end of method

        //method for getting the number of days
        public string GetNumberOfDays(string days)
        {
            //get number for the users input
            string collected_number = Regex.Replace(days, @"[^\d]", "");

            return collected_number;

        }//end of method

        //method for setting remind in days
        public string ReminderInDays(string reminderDay)
        {
            //get the date forward
            DateTime currentDate = DateTime.Now.Date;

            //setting the format
            string correctFormat = currentDate.ToString("yyyy-MM-dd");

            //getting the day number from the correct format
            string dayNumber = correctFormat.Substring(8, 2);

            //getting the date from year to month
            string yearMonth = correctFormat.Substring(0, 8);

            //calculate the number of days
            int totalDays = int.Parse(dayNumber) + int.Parse(reminderDay);

            //finally calculated date
            string calculatedDate = yearMonth + totalDays;

            //storing the task
            TaskDescription.Add(DescriptionReturn);

            TaskDate.Add(calculatedDate);

            return "Great, I will remind you within " + reminderDay + " day(s) on the " + calculatedDate;



        }

        //method to return all the reminders
        public List<string> allReminders()
        {

            //temporary array to store all descriptions and dates
            List<string> storeToReturn = new List<string>();
            DateTime currentDate = DateTime.Now.Date;
            string format_date = currentDate.ToString("yyyy-MM-dd");




            //loop in both arrays desc and dates
            for (int count_date = 0; count_date < TaskDate.Count; count_date++)
            {

                //then inner loop / and check the date
                if (TaskDate[count_date] == format_date && !TaskDate[count_date].ToString().Contains(" [done]"))
                {
                    string hold_temp = "ChatBot AI" + TaskDescription[count_date] + " due today on " + format_date;
                    if (!storeToReturn.Contains(hold_temp))
                    {
                        //then add
                        storeToReturn.Add(hold_temp);
                    }



                }
                else if (!TaskDate[count_date].ToString().Contains(" [done]"))
                {
                    string hold_temp = "ChatBot AI: " + TaskDescription[count_date] + " this task is due on " + TaskDate[count_date] + " , the system will remind you";
                    if (!storeToReturn.Contains(hold_temp))
                    {
                        //then add
                        storeToReturn.Add(hold_temp);
                    }
                }
                else if (TaskDate[count_date].ToString().Contains(" [done]"))
                {
                    string temp = TaskDate[count_date].ToString().Replace(" [done]", "");
                    string hold_temp = "ChatBot AI: " + TaskDescription[count_date] + " this task is due on " + temp + " , the system will remind you [done]";
                    if (!storeToReturn.Contains(hold_temp))
                    {
                        //then add
                        storeToReturn.Add(hold_temp);
                    }
                }



            }//end of the if statement

            //then return
            return storeToReturn;
        }



        //mark desc done and date
        public void mark_done(int indexes)
        {
            //TaskDescription[indexes] = TaskDescription[indexes] + " [done]";
            TaskDate[indexes] = TaskDate[indexes] + " [done]";


        }



        public void delete_marked(int index)
        {

            //delete the found value
            TaskDate.RemoveAt(index);
            TaskDescription.RemoveAt(index);

        }

        public List<string> getTasksDueToday()
        {
            List<string> dueToday = new List<string>();
            DateTime currentDate = DateTime.Now.Date;
            string formatDate = currentDate.ToString("yyyy-MM-dd");

            for (int i = 0; i < TaskDate.Count; i++)
            {
                string rawDate = TaskDate[i].ToString();
                string description = TaskDescription[i].ToString();

                bool isDone = rawDate.Contains(" [done]");
                string cleanedDate = rawDate.Replace(" [done]", "");

                // Match today and skip [done] tasks
                if (cleanedDate == formatDate && !isDone)
                {
                    string reminder = $"ChatBot AI: {description} is due today on {cleanedDate}";
                    dueToday.Add(reminder);
                }
            }

            return dueToday;
        }



    }
}