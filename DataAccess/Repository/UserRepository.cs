﻿using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public void AddUser(Users user) => UserDAO.Instance.AddUser(user);

        public void AddVolunteers(Volunteers volunteers) => UserDAO.Instance.AddVolunteers(volunteers);

        public void ChangePass(Users users) => UserDAO.Instance.ChangePass(users);

        public Users checkpass(string email, string pass) => UserDAO.Instance.checkpass(email, pass);

        public Users GetDefaultMember(string email, string password) => UserDAO.Instance.GetDefaultMember(email, password);

        public ViewHistory gethistory(int id) => UserDAO.Instance.gethistory(id);

        public Users getUserid(string email) => UserDAO.Instance.getUserid(email);

        public Users Login(string email, string password) => UserDAO.Instance.Login(email, password);

    }
}
