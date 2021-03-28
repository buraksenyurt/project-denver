﻿using Denver.BLL;
using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Denver.NextUI
{
    public partial class PersonManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLHumanResource bLLHumanResource = new BLLHumanResource();
            Dictionary<string,string> locations= bLLHumanResource.LoadWorkLocations();
            drpDownListWorkLocation.Items.Clear();
            foreach(KeyValuePair<string,string> keyValuePair in locations)
            {
                ListItem item = new ListItem();
                item.Value = keyValuePair.Value;
                item.Text = keyValuePair.Key;
                drpDownListWorkLocation.Items.Add(item);
            }

            PopulatePersonelListes();
        }

        private void PopulatePersonelListes()
        {
            BLLHumanResource bLLHumanResource = new BLLHumanResource();
            DataSet set= bLLHumanResource.LoadAllPersons(1,100);
            GridViewPersonelListesi.DataSource = set.Tables[0];
            GridViewPersonelListesi.DataBind();
            Session[SessionKeys.AllPerson] = set.Tables[0];
        }

        protected void MultiView1_ActiveViewChanged(object sender, EventArgs e)
        {

        }

        protected void btnSaveNewPersonToTheDatabase_Click(object sender, EventArgs e)
        {
            DALPerson dal = new DALPerson();
            Person person = new Person();
            person.Email = txtMail.Text;
            person.Name = txtFirstName.Text;
            person.MidName = txtMiddleName.Text;
            person.LastName = txtSurname.Text;
            person.Location = FindEnumValue(drpDownListWorkLocation.SelectedValue);
            person.Salary = 1000;
            person.WorkStartDate = calendarWorkStartDateOfPerson.SelectedDate;
            bool result=CheckIfPersonnelIsExist(person);
            if (result)
            {
                RetCode retCode = dal.Add(person);
                lblProcessResult.Text = "Yeni personel ekleme işi tamamlandı";
            }
            else
            {
                lblProcessResult.Text = "Personel zaten var. Tekrardan ekleyemem"; 
                // Ürünü Çinlilere de satacağız. Bu text'leri nasıl çevireceğiz?
            }
        }

        private bool CheckIfPersonnelIsExist(Person person)
        {
            DALPerson dal = new DALPerson();
            return dal.IsExistPerson(person);
        }

        private WorkLocation FindEnumValue(string selectedValue)
        {
            string[] names=Enum.GetNames(typeof(WorkLocation));
            WorkLocation result=WorkLocation.Office;
            for (int i = 0; i < names.Length-1; i++)
            {
                if (names[i] == selectedValue)
                     result=(WorkLocation)Enum.Parse(typeof(WorkLocation), names[i]);
            }
            return result;
        }
    }
}