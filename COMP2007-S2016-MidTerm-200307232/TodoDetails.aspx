<%@ Page Title="Todo Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TodoDetails.aspx.cs" Inherits="COMP2007_S2016_MidTerm_200307232.TodoDetails" %>
<%-- 
* @File name : Todo List page 
* @Author : Ritesh Patel (200307232)
* @Website name : MidTerm(http://comp2007-s2016-midterm-200307232.azurewebsites.net/)
* @File description : This is Todo list page which allows pagging, deleting and sorting .
*  
 *  
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="container">
        <div class="row">
            <div class="col-md-offset-3 col-md-6">
                <h1>Todo Details</h1>
                <h5>All Fields are Required</h5>
                <br />
                <div class="form-group">
                    <label class="control-label" for="LastNameTextBox">Todo Name</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TodoNameTextBox" placeholder="Todo Name" required="true"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label class="control-label" for="FirstNameTextBox">Todo Notes</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TodoNotesTextBox" placeholder="Todo Notes" required="true"></asp:TextBox>
                </div>
                 <div class="form-group">
                    <asp:CheckBox runat="server" CssClass="checkbox" ID="CheckboxCompleted" ></asp:CheckBox>
                    <label class="control-label" for="FirstNameTextBox">Completed</label>
                 </div>
                <div class="text-right">
                    <asp:Button Text="Cancel" ID="CancelButton" CssClass="btn btn-warning btn-lg" runat="server" 
                        UseSubmitBehavior="false" CausesValidation="false" OnClick="CancelButton_Click" />
                    <asp:Button Text="Save" ID="SaveButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="SaveButton_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
