using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements that are required to connect to EF DB
using COMP2007_S2016_MidTerm_200307232.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;
/*
* @File name : Todo List page 
* @Author : Ritesh Patel (200307232)
* @Website name : MidTerm(http://comp2007-s2016-midterm-200307232.azurewebsites.net/)
* @File description : This is Todo list page which allows pagging, deleting and sorting .
* 
* 
*/
namespace COMP2007_S2016_MidTerm_200307232
{
    public partial class TodoList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading the page for the first time, populate the student grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "TodoID"; // default sort column
                Session["SortDirection"] = "ASC";
                // Get the Todo data
                this.GetTodos();
            }
        }
        /**
      * <summary>
      * This method gets the todos data from the DB
      * </summary>
      * 
      * @method GetTodos
      * @returns {void}
      */
        private void GetTodos()
        {
            // connect to EF
            using (TodoConnection db = new TodoConnection())
            {
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                // query the todos Table using EF and LINQ
                var Todos = (from alltodos in db.Todos
                                select alltodos);

                // bind the result to the GridView
                TodoGridView.DataSource = Todos.AsQueryable().OrderBy(SortString).ToList();
                TodoGridView.DataBind();

                LabelCount.Text = Todos.Count().ToString();
                
            }
        }
        /**
    * <summary>
    * This event handler allow to change gridview page size
    * </summary>
    * 
    * @method PageSizeDropDownList_SelectedIndexChanged
    * @param {object} sender
    * @param {EventArgs} e
    * @returns {void}
    */
        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set the new Page size
            TodoGridView.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);
          
            // refresh the grid
           this.GetTodos();
        }
        /**
    * <summary>
    * This event handler allow to deleting record
    * </summary>
    * 
    * @method TodoGridView_RowDeleting
    * @param {object} sender
    * @param {GridViewDeleteEventArgs} e
    * @returns {void}
    */
        protected void TodoGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // store which row was clicked
            int selectedRow = e.RowIndex;

            // get the selected TodoID using the Grid's DataKey collection
            int TodoID = Convert.ToInt32(TodoGridView.DataKeys[selectedRow].Values["TodoID"]);

            // use EF to find the selected todo in the DB and remove it
            using (TodoConnection db = new TodoConnection())
            {
                // create object of the todo class and store the query string inside of it
                Todo deletedtodo = (from studentRecords in db.Todos
                                          where studentRecords.TodoID == TodoID
                                          select studentRecords).FirstOrDefault();

                // remove the selected todo from the db
                db.Todos.Remove(deletedtodo);

                // save my changes back to the database
                db.SaveChanges();

                // refresh the grid
                this.GetTodos();
            }
        }
        /**
            * <summary>
            * This event handler allow to change pagging
            * </summary>
            * 
            * @method TodoGridView_PageIndexChanging
            * @param {object} sender
            * @param {GridViewPageEventArgs} e
            * @returns {void}
            */
        protected void TodoGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the new page number
            TodoGridView.PageIndex = e.NewPageIndex;

            // refresh the grid
            this.GetTodos();
        }
        /**
    * <summary>
    * This event handler allows to sorting
    * </summary>
    * 
    * @method TodoGridView_Sorting
    * @param {object} sender
    * @param {GridViewSortEventArgs} e
    * @returns {void}
    */
        protected void TodoGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // get the column to sorty by
            Session["SortColumn"] = e.SortExpression;

            // Refresh the Grid
            this.GetTodos();

            // toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
        }
        /**
    * <summary>
    * This event handler bind link button acordding sorting direction
    * </summary>
    * 
    * @method TodoGridView_RowDataBound
    * @param {object} sender
    * @param {GridViewRowEventArgs} e
    * @returns {void}
    */
        protected void TodoGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header) // if header row has been clicked
                {
                    LinkButton linkbutton = new LinkButton();

                    for (int index = 0; index < TodoGridView.Columns.Count - 1; index++)
                    {
                        if (TodoGridView.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "ASC")
                            {
                                linkbutton.Text = " <i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkbutton.Text = " <i class='fa fa-caret-down fa-lg'></i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkbutton);
                        }
                    }
                }
            }
        }
        /**
    * <summary>
    * This event handler Shows pop-up box with game name and description
    * </summary>
    * 
    * @method lbGame_Click
    * @param {object} sender
    * @param {EventArgs} e
    * @returns {void}
    */
        protected void CheckBoxComplete_CheckedChanged(object sender, EventArgs e)
        {
            
            CheckBox Complete = (CheckBox)sender;
            int TodoID = Convert.ToInt32( Complete.ToolTip.ToString());

            // use EF to find the selected todo in the DB and remove it
            using (TodoConnection db = new TodoConnection())
            {
                // create object of the todo class and store the query string inside of it
                Todo updatedtodo = (from studentRecords in db.Todos
                                    where studentRecords.TodoID == TodoID
                                    select studentRecords).FirstOrDefault();

                if(Complete.Checked==true)
                updatedtodo.Completed = true;
                else
                updatedtodo.Completed = false;
                // save my changes back to the database
                db.SaveChanges();

                // refresh the grid
                this.GetTodos();
            }
        }
    }
}