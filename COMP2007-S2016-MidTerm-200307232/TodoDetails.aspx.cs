using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
// using statements required for EF DB access
using COMP2007_S2016_MidTerm_200307232.Models;
using System.Web.ModelBinding;

/*
* @File name : Todo Details page 
* @Author : Ritesh Patel (200307232)
* @Website name : MidTerm(http://comp2007-s2016-midterm-200307232.azurewebsites.net/)
* @File description : This is Todo details page which allows to update and add.
* 
* 
*/

namespace COMP2007_S2016_MidTerm_200307232
{
    public partial class TodoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetTodo();
            }
        }
        /**
      * <summary>
      * This method gets the todo data from the DB
      * </summary>
      * 
      * @method GetTodo
      * @returns {void}
      */
        private void GetTodo()
        {
            // populate teh form with existing data from the database
            int TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

            // connect to the EF DB
            using (TodoConnection db = new TodoConnection())
            {
                // populate a todo object instance with the todoID from the URL Parameter
                Todo updatedTodo = (from todo in db.Todos
                                          where todo.TodoID == TodoID
                                          select todo).FirstOrDefault();

                // map the todo properties to the form controls
                if (updatedTodo != null)
                {
                    TodoNameTextBox.Text = updatedTodo.TodoName;
                    TodoNotesTextBox.Text = updatedTodo.TodoNotes;
                    CheckboxCompleted.Checked =Convert.ToBoolean( updatedTodo.Completed);
                }
            }
        }
        /**
    * <summary>
    * This event handler cancel from and redirect to todolists
    * </summary>
    * 
    * @method CancelButton_Click
    * @param {object} sender
    * @param {EventArgs} e
    * @returns {void}
    */
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Students page
            Response.Redirect("~/TodoList.aspx");
        }
        /**
    * <summary>
    * This event handler allow to add or update record
    * </summary>
    * 
    * @method SaveButton_Click
    * @param {object} sender
    * @param {EventArgs} e
    * @returns {void}
    */
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use EF to connect to the server
            using (TodoConnection db = new TodoConnection())
            {
                // use the todo model to create a new todo object and
                // save a new record
                Todo newTodo = new Todo();

                int TodoID = 0;

                if (Request.QueryString.Count > 0) // our URL has a TodoID in it
                {
                    // get the id from the URL
                    TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

                    // get the current todo from EF DB
                    newTodo = (from todo in db.Todos
                                  where todo.TodoID == TodoID
                                  select todo).FirstOrDefault();
                }

                // add form data to the new todo record
                newTodo.TodoName = TodoNameTextBox.Text;
                newTodo.TodoNotes = TodoNotesTextBox.Text;
                if (CheckboxCompleted.Checked == true)
                    newTodo.Completed = true;
                else
                    newTodo.Completed = false;
                // use LINQ to ADO.NET to add / insert new todo into the database

                if (TodoID == 0)
                {
                    db.Todos.Add(newTodo);
                }


                // save our changes - also updates and inserts
                db.SaveChanges();

                // Redirect back to the updated students page
                Response.Redirect("~/TodoList.aspx");
            }
        }
    }
}