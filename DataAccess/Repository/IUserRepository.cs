﻿using BusinessObject.Model;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUserRepository
    {
        Users Login(string email, string password);
        void AddUser(Users user);
        Users getUserid(string email);
        void AddVolunteers(Volunteers volunteers);
        Users GetDefaultMember(string email, string password);
        void ChangePass(Users users);
        Users checkpass(string email, string pass);
        ViewHistory gethistory(int id);
        Users getProfile(int id);
        Task<string> forgotpass(string email, string content);
        void addnotification(Notification notification);
        void updateProfile(Users users);
        void updateDeactive(Users users);
        Users getUsersid(int id);
    }
}
