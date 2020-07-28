using System;
using System.Collections.Generic;
using System.Text;

namespace CustomShop
{
    enum Rights
    {
        User,
        Admin
    }
    class User<T>
    {
        public T Login { get; set; }
        public T Password { get; set; }
        public T Name { get; set; }
        public T Surname { get; set; }
        public Rights Right { get; set; }
        public bool LoginState { get; set; }

        public User(T Login, T Password)
        {
            this.Login = Login;
            this.Password = Password;
            LoginState = false;
            Right = Rights.User;
        }
        public User(T Login, T Password, Rights right)
        {
            this.Login = Login;
            this.Password = Password;
            LoginState = false;
            Right = right;
        }
        public User()
        {
            LoginState = false;
        }
        public User(T Login, T Password, T Name, T Surname)
        {
            this.Login = Login;
            this.Password = Password;
            this.Name = Name;
            this.Surname = Surname;
            LoginState = false;
            Right = Rights.User;
        }

        private bool IsLogged()
        {
            return LoginState;
        }
    }
}
