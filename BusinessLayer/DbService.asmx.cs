using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Serialization;
using CommonLayer;

namespace BusinessLayer
{
    /// <summary>
    /// Summary description for DbService
    /// </summary>

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DbService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateUser(string name, string surname, string username, string password, string type)
        {
            int id = -1;
            if (Convert.ToBoolean(type))
            {
                id = DataLayer.LecturerRepository.Instance.getUserTypeByName("Lecturer");
            }
            else
            {
                id = DataLayer.LecturerRepository.Instance.getUserTypeByName("Student");
            }
            string message = DataLayer.LecturerRepository.Instance.UpdateUser(name, surname, username, password, id);
            Context.Response.Write(message);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUserTypeByID(string id)
        {

            var getUser = DataLayer.LecturerRepository.Instance.getUserTypeByid(Convert.ToInt32(id));

            Context.Response.Write(getUser);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUser(string username)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            CommonLayer.user data = new CommonLayer.user();
            var getUser = DataLayer.LecturerRepository.Instance.GetUserByUsername(username);

            data.username = getUser.username;
            data.password = getUser.password;
            data.name = getUser.name;
            data.surname = getUser.surname;
            data.userTypeID = getUser.userTypeID;
            Context.Response.Write(js.Serialize(data));
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void AddUser(string name, string surname, string username, string password, string type)
        {
            int id = -1;
            if (Convert.ToBoolean(type))
            {
                id = DataLayer.LecturerRepository.Instance.getUserTypeByName("Lecturer");
            }
            else
            {
                id = DataLayer.LecturerRepository.Instance.getUserTypeByName("Student");
            }
            string message = DataLayer.LecturerRepository.Instance.AddUser(name, surname, username, password, id);
            Context.Response.Write(message);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUsers()
        {
            string message = "<table class='userTable'><tr><th>Select</th><th>Username</th><th>Name</th><th>Surname</th><th>Password</th><th>User Type</th><th>Command</th></tr>";
            List<user> users = DataLayer.LecturerRepository.Instance.GetUsers();
            foreach (user us in users)
            {
                message += "<tr><td><input type='checkbox' class='checks' check='false' id=" + us.username + " /></td><td>" + us.username + "</td><td>" + us.name + "</td><td>" + us.surname + "</td><td>" + us.password + "</td><td>" + us.userType.type + "</td><td><input type='button' value='Edit' class='editButton' id=" + us.username + " /></td></tr>";
            }
            message += "</table>";
            Context.Response.Write(message);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void DeleteUsers(string usernames)
        {

            var json = new JavaScriptSerializer();
            var data = json.Deserialize<string[]>(usernames);

            bool success = true;
            foreach (string username in data)
            {
                if (success && !DataLayer.LecturerRepository.Instance.DeleteUser(username))
                    success = false;
            }

            string message = "Deleted Sucessfully :)";
            if (!success)
                message = "Sorry but an error occured :/";

            Context.Response.Write(message);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void AddQuiz(string quizName)
        {
            if (!String.IsNullOrEmpty(quizName))
            {
                string message = DataLayer.LecturerRepository.Instance.AddQuiz(quizName);

                Context.Response.Write(message);
            }

            else
            {
                Context.Response.Write("Quiz name is either empty or null");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetQuizs()
        {
            string message = "<table class='userTable'><tr><th>Name</th><th>Command</th></tr>";
            List<quiz> qz = DataLayer.LecturerRepository.Instance.GetAllQuiz();

            foreach (quiz q in qz)
            {
                message += "<tr><td>" + q.quizName + "</td><td><input type='button' value='Edit' class='quizEditButton' id=" + q.quizID + " /></td></tr>";
            }
            message += "</table>";

            Context.Response.Write(message);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetQuiz(string id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            CommonLayer.quiz data = new CommonLayer.quiz();
            var getQuiz = DataLayer.LecturerRepository.Instance.getQuizById(int.Parse(id));

            data.quizName = getQuiz.quizName;
            data.quizID = getQuiz.quizID;

            Context.Response.Write(js.Serialize(data));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetQuizsDropDown()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            var getList = DataLayer.LecturerRepository.Instance.GetAllQuiz();
            var data = new List<quiz>();

            foreach (quiz q in getList)
            {
                var tq = new quiz();

                tq.quizName = q.quizName;
                tq.quizID = q.quizID;

                data.Add(tq);
            }

            Context.Response.Write(js.Serialize(data));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateQuiz(string id, string quizName)
        {
            bool success = DataLayer.LecturerRepository.Instance.updateQuizName(int.Parse(id), quizName);

            string message = "Updated Successfuly";

            if (!success)
            {
                message = "Not updated";
            }

            Context.Response.Write(message);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void AddQuestion(string name, string quizid, string answers)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string[] answersl = js.Deserialize<string[]>(answers);
            string message = DataLayer.LecturerRepository.Instance.addQuestion(name, int.Parse(quizid), answersl.ToList());

            Context.Response.Write(message);
        } 


    }
}
