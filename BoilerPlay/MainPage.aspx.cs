﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using BoilerPlay.Database;
using System.Web.Services;
using System.Threading.Tasks;

namespace BoilerPlay
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Cookies.ReadCookie(this.Request, this.Response)))
            {
                Response.Redirect("LoginPage.aspx");
            }
            // This is a Cookie DO NOT DELETE MONAL+
            successMessage.Visible = false;
            allEventsBtn.Enabled = false;

            foreach (var sport in Database.DatabaseOptions.Posts_Sports.SportsCombinations)
                SportFilter.Items.Add(sport);
            //Cookies.ReadCookie(this.Request,this.Response);

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["myEvents"]))
                {
                    if (Request.QueryString["myEvents"] == "true")
                    {
                        myEventBtn_Click(new object(), EventArgs.Empty);
                    }
                    else
                        SetCards();
                }
                else
                    SetCards();
            }



        }
        private void SetCards(int index = -1)
        {
            if (index == -1)
                MainPageGlobals.Posts = HelloWorldQueryMethods.GetAllPosts();

            string AccountID = Cookies.ReadCookie(this.Request, this.Response);
            string[] involvementsForAccount = HelloWorldQueryMethods.GetAllInvolvementsForAccount(AccountID);

            int count = MainPageGlobals.Posts.Length;
            if (count > 10)
                count = 10;

            for (int x = 0; x < 10; x++)
            {
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("card" + x)).Visible = false;
                ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button1" + x)).Visible = false;
            }
            for (int x = 0; x < count; x++)
            {
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("card" + x)).Visible = true;

                string peopleText;
                int totalNumberOfPeopleNeeded = MainPageGlobals.Posts[x].NumberNeeded;
                int currentNumber = HelloWorldQueryMethods.GetNumberOfPeopleInEvent(MainPageGlobals.Posts[x].PostID);
                if (index == x)
                {
                    peopleText = String.Format("People Commited: {0}/{1}", currentNumber + 1, totalNumberOfPeopleNeeded);
                    HelloWorldQueryMethods.Involvement involvement = new HelloWorldQueryMethods.Involvement
                    {
                        AccountsID = AccountID,
                        IsHost = false,
                        Posts_PostID = MainPageGlobals.Posts[x].PostID
                    };
                    HelloWorldQueryMethods.InsertInvolvement(involvement);
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).Disabled = true;
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).InnerText = "Currently Joined";
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button1" + x)).Visible = true;
                }
                else
                    peopleText = String.Format("People Commited: {0}/{1}", currentNumber, totalNumberOfPeopleNeeded);

                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("DatePrint" + x)).InnerText = MainPageGlobals.Posts[x].DateTime.ToString("MM/dd/yyyy hh:mm tt");
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("Location" + x)).InnerText = MainPageGlobals.Posts[x].Location;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("Proficiency" + x)).InnerText = MainPageGlobals.Posts[x].Proficiency;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("People" + x)).InnerText = peopleText;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("Description" + x)).InnerText = MainPageGlobals.Posts[x].Desc;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("Gender" + x)).InnerText = "Gender: " + MainPageGlobals.Posts[x].Gender;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("CardTitle" + x)).InnerText = MainPageGlobals.Posts[x].Title;

                //UpdateMainPageGlobals
                if (involvementsForAccount.Contains(MainPageGlobals.Posts[x].PostID))
                {
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).Disabled = true;
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).InnerText = "Currently Joined";
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button1" + x)).Visible = true;
                }
                else if (currentNumber >= totalNumberOfPeopleNeeded)
                {
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).Disabled = true;
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).InnerText = "Event Full";
                }
                else if (index != x)
                {
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).Disabled = false;
                    ((System.Web.UI.HtmlControls.HtmlButton)this.FindControl("button" + x)).InnerText = "Join Event";
                }
            }
        }
        private void SetSuccessMessage()
        {
            successMessage.Visible = true;
            successPrint.InnerText = "You have been successfully added to the event";
        }
        protected void button0_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 0].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(0);
            SetSuccessMessage();
        }
        protected void button1_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 1].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(1);
            SetSuccessMessage();
        }

        protected void button2_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 2].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(2);
            SetSuccessMessage();
        }

        protected void button3_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 3].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(3);
            SetSuccessMessage();
        }

        protected void button4_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 4].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(4);
            SetSuccessMessage();
        }

        protected void button5_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 5].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(5);
            SetSuccessMessage();
        }

        protected void button6_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 6].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(6);
            SetSuccessMessage();
        }

        protected void button7_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 7].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(7);
            SetSuccessMessage();
        }

        protected void button8_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 8].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(8);
            SetSuccessMessage();
        }

        protected void button9_ServerClick(object sender, EventArgs e)
        {
            string PostID = MainPageGlobals.Posts[(MainPageGlobals.CurrentPage * 10) + 9].PostID;
            //HelloWorldQueryMethods.IncrementNumberOfAttendees(PostID);
            SetCards(9);
            SetSuccessMessage();
        }

        protected void createEventBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Create_Event");
        }
        protected void myEventBtn_Click(object sender, EventArgs e)
        {
            allEventsBtn.Enabled = true;
            myEventBtn.Enabled = false;

            string accountID = Cookies.ReadCookie(this.Request, this.Response);
            var involvements = HelloWorldQueryMethods.GetAllInvolvements(accountID);
            if (involvements.Length > 0)
            {
                string cmdBase = String.Format("WHERE PostID = '{0}'", involvements[0].Posts_PostID);
                for (int x = 1; x < involvements.Length; x++)
                {
                    string postID = involvements[x].Posts_PostID;
                    cmdBase += String.Format(" || PostID = '{0}'", postID);
                }
                cmdBase += ";";

                var output = HelloWorldQueryMethods.GetAllPosts(cmdBase);

                MainPageGlobals.Posts = output;
                SetCards(-2);
            }
            else
            {
                MainPageGlobals.Posts = new HelloWorldQueryMethods.Posts[0];
                SetCards(-2);
            }
        }
        protected void profileBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Personal_profile.aspx");
        }

        protected void allEventsBtn_Click(object sender, EventArgs e)
        {
            allEventsBtn.Enabled = false;
            myEventBtn.Enabled = true;

            SetCards();
        }

        protected void button10_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(0);
        }

        protected void button11_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(1);
        }

        protected void button12_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(2);
        }

        protected void button13_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(3);
        }

        protected void button14_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(4);
        }

        protected void button15_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(5);
        }

        protected void button16_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(6);
        }

        protected void button17_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(7);
        }

        protected void button18_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(8);
        }

        protected void button19_ServerClick(object sender, EventArgs e)
        {
            UnSubscribeFromEvent(9);
        }
        private void UnSubscribeFromEvent(int cardNo)
        {
            string postID = MainPageGlobals.Posts[cardNo].PostID;
            string accountID = Cookies.ReadCookie(this.Request, this.Response);

            HelloWorldQueryMethods.DeleteInvolvement(postID, accountID);
            SetCards(-1);
        }

        protected void logOutBtn_Click1(object sender, EventArgs e)
        {
            Cookies.DeleteCookie(this.Request, this.Response);
            Response.Redirect("LoginPage.aspx");
        }

        protected void filterBtn_Click(object sender, EventArgs e)
        {
            var allValues = BoilerPlay.Database.Query.ExecuteReturnCommand("SELECT * FROM HelloWorld.Posts;");

            String sportsBox = SportFilter.Value;
            String profeciencyBox = ProficiencyFilter.Value;
            String genderBox = GenderFilter.Value;
            String dateBox = Calendar1.SelectedDate.ToString("yyyy-MM-dd");

            String timeBox1 = TimeStartFilter.Text; //accept as hh:mm (smaller)
            String timeBox2 = TimeEndFilter.Text; //accept as hh:mm (bigger value)



            //DataTable dateFromDatabase = BoilerPlay.Database.Query.ExecuteReturnCommand("SELECT HelloWorld.Posts.DateTime FROM HelloWorld.Posts");
            string[] dates = new string[allValues.Rows.Count];
            for (int x = 0; x < allValues.Rows.Count; x++)
            {
                dates[x] = DateTime.Parse(allValues.Rows[x].ItemArray[3].ToString()).ToString("yyyy-MM-dd hh:mm");//.Substring(0, 10);
                                                                     //if(!dates[x].Equals(dateBox))
                                                                     //{
                                                                     //allValues.Rows.RemoveAt(x);
                                                                     //x--;
                                                                     //}
            }

            string GenderEquals = "=";
            if (String.IsNullOrWhiteSpace(genderBox))
                GenderEquals = "!=";

            string ProficencyEquals = "=";
            if (String.IsNullOrWhiteSpace(profeciencyBox))
                ProficencyEquals = "!=";

            string SportsEquals = "=";
            if (String.IsNullOrWhiteSpace(sportsBox))
                SportsEquals = "!=";

            string cmdString = "SELECT * FROM HelloWorld.Posts WHERE Gender " + GenderEquals + "'" + genderBox + "' && Proficiency " + ProficencyEquals + " '" + profeciencyBox + "' && Title " + SportsEquals + " '" + sportsBox + "';";
            var tempValues = BoilerPlay.Database.Query.ExecuteReturnCommand(cmdString);


            int count = dates.Length;
            int numberRemoved = 0;
            for (int i = 0; i < count; i++)
            {
                if (!String.IsNullOrWhiteSpace(dateBox))
                {
                    string subString = dates[i].Substring(0, 10);
                    if (subString.Equals(dateBox))
                    {
                        if (String.IsNullOrWhiteSpace(timeBox2) != true && String.IsNullOrWhiteSpace(timeBox1) != true)
                        {
                            if ((Int32.Parse(timeBox2.Substring(0, 2)) >= (Int32.Parse((dates[i].Substring(11, 13))))))
                            {
                                if ((Int32.Parse(timeBox1.Substring(0, 2)) <= (Int32.Parse((dates[i].Substring(11, 13))))))
                                {
                                    if (Int32.Parse(timeBox2.Substring(3, 5)) >= (Int32.Parse((dates[i].Substring(13, 15)))))
                                    {
                                        if ((Int32.Parse(timeBox1.Substring(3, 5)) <= (Int32.Parse((dates[i].Substring(13, 15))))))
                                        {
                                            tempValues.Rows.RemoveAt(i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tempValues.Rows.RemoveAt(i - numberRemoved);
                        numberRemoved++;
                    }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(timeBox2) != true && String.IsNullOrWhiteSpace(timeBox1) != true)
                    {
                        if ((Int32.Parse(timeBox2.Substring(0, 2)) >= (Int32.Parse((dates[i].Substring(11, 13))))))
                        {
                            if ((Int32.Parse(timeBox1.Substring(0, 2)) <= (Int32.Parse((dates[i].Substring(11, 13))))))
                            {
                                if (Int32.Parse(timeBox2.Substring(3, 5)) >= (Int32.Parse((dates[i].Substring(13, 15)))))
                                {
                                    if ((Int32.Parse(timeBox1.Substring(3, 5)) <= (Int32.Parse((dates[i].Substring(13, 15))))))
                                    {
                                        tempValues.Rows.RemoveAt(i);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            HelloWorldQueryMethods.Posts[] posts = new HelloWorldQueryMethods.Posts[tempValues.Rows.Count];

            for(int x = 0; x < tempValues.Rows.Count; x++)
            {
                posts[x].PostID = tempValues.Rows[x].ItemArray[0].ToString();
                posts[x].Title = tempValues.Rows[x].ItemArray[1].ToString();
                posts[x].Posts_Name = tempValues.Rows[x].ItemArray[2].ToString();
                posts[x].DateTime = DateTime.Parse(tempValues.Rows[x].ItemArray[3].ToString());
                posts[x].Gender = tempValues.Rows[x].ItemArray[4].ToString();
                posts[x].Desc = tempValues.Rows[x].ItemArray[5].ToString();
                posts[x].Location = tempValues.Rows[x].ItemArray[6].ToString();
                posts[x].NumberNeeded = Convert.ToInt32(tempValues.Rows[x].ItemArray[7].ToString());
                posts[x].Proficiency = tempValues.Rows[x].ItemArray[8].ToString();
            }
            MainPageGlobals.Posts = posts;

            SetCards(-2);
        }

        protected void ClearCalendarBtn_Click(object sender, EventArgs e)
        {
            Calendar1.SelectedDates.Clear();
        }
    }
}