using DrinkElivery.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DrinkElivery.Context
{
    public class UserContext
    {
        private SqlConnection Con = new SqlConnection("Data Source=RICARDO;Initial Catalog=Drinklivery; Integrated Security=True;");
        public void Inserir(User user)
        {
            Con.Open();
            var create = string.Format("INSERT INTO [dbo].[User] ([UserId] ,[Name] ,[Password] ,[Email] ,[Cep]) VALUES ({0},'{1}','{2}','{3}','{4}')", user.UserId, user.Name, user.Password,user.Email, user.Cep);
            SqlCommand command = new SqlCommand();
            command.Connection = Con;
            command.CommandText = create;
            command.ExecuteNonQuery();
            Con.Close();
            
        }
        public List<User> Listar()
        {
            List<User> user = new List<User>();       
            Con.Open();
            var listar = "SELECT [UserId] ,[Name] ,[Password] ,[Email] ,[Cep] FROM[Drinklivery].[dbo].[User]";

            SqlCommand command = new SqlCommand();
            command.Connection = Con;
            command.CommandText = listar;
            var dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                User Temp = new User();
                Temp.Name = dataReader["Name"].ToString();
                Temp.UserId = Convert.ToInt32(dataReader["UserId"].ToString());
                Temp.Password = dataReader["Password"].ToString();
                Temp.Email = dataReader["Email"].ToString();
                Temp.Cep = dataReader["Cep"].ToString();
                user.Add(Temp);
            }

            Con.Close();

            return user;

        }
    }
}