using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer;
namespace DataLayer
{
    public class LecturerRepository : Connection
    {

        private static LecturerRepository singleton = null;

        //Creates an instance of the class
        public static LecturerRepository Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new LecturerRepository();
                }
                return singleton;
            }
        }


        //Add new User
        public string AddUser(string name, string surname, string username, string password, int id)
        {
            try
            {
                var checkUser = GetUserByUsername(username);

                if (checkUser != null)
                {
                    throw new Exception("Username already exsist");
                }
                CommonLayer.user u = new CommonLayer.user();
                u.name = name;
                u.surname = surname;
                u.username = username;
                u.password = password;
                u.userTypeID = id;
                this.Entity.users.AddObject(u);
                this.Entity.SaveChanges();
                return "Added Successfully :)";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Get user by username
        public CommonLayer.user GetUserByUsername(string username)
        {
            return this.Entity.users.SingleOrDefault(x => x.username.ToLower() == username.ToLower());
        }

        //Get all users
        public List<CommonLayer.user> GetUsers()
        {
            return this.Entity.users.ToList();
        }

        //Delete User By username
        public bool DeleteUser(string username)
        {
            try
            {
                CommonLayer.user us = GetUserByUsername(username);
                this.Entity.users.DeleteObject(us);
                this.Entity.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Update User 
        public string UpdateUser(string name, string surname, string username, string password, int id)
        {
            try
            {
                CommonLayer.user us = GetUserByUsername(username);
                us.name = name;
                us.surname = surname;
                us.password = password;
                us.userTypeID = id;
                this.Entity.SaveChanges();
                return "Updated :)";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //Add Quiz
        public string AddQuiz(string quizName)
        {
            try
            {
                CommonLayer.quiz quz = new CommonLayer.quiz();
                quz.quizName = quizName;
                this.Entity.quizs.AddObject(quz);
                this.Entity.SaveChanges();
                return "Added sucessfully :)";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Get quiz by id
        public CommonLayer.quiz getQuizById(int id)
        {
            return this.Entity.quizs.SingleOrDefault(x => x.quizID == id);
        }

        // Get all quizes
        public List<CommonLayer.quiz> GetAllQuiz()
        {
            return this.Entity.quizs.ToList();
        }

        //Edit quiz name by id
        public bool updateQuizName(int id, string name)
        {
            try
            {
                CommonLayer.quiz quiz = getQuizById(id);
                quiz.quizName = name;
                this.Entity.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Delete quiz by id
        public string deleteQuiz(int id)
        {
            try
            {
                CommonLayer.quiz quiz = getQuizById(id);
                this.Entity.quizs.DeleteObject(quiz);
                this.Entity.SaveChanges();
                return "Quiz Deleted :)";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Add Answer
        public string AddAnswer(string answer)
        {
            try
            {
                CommonLayer.answer ans = new CommonLayer.answer();
                ans.answer1 = answer;
                this.Entity.answers.AddObject(ans);
                this.Entity.SaveChanges();
                return "Answer Added Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Get Answer by id 
        public CommonLayer.answer getAnswerByID(int id)
        {
            return this.Entity.answers.SingleOrDefault(x => x.answerID == id);
        }

        //Delete Answer by id
        public string deleteAnswerById(int id)
        {
            try
            {
                CommonLayer.answer ans = getAnswerByID(id);
                this.Entity.answers.DeleteObject(ans);
                this.Entity.SaveChanges();
                return "Answer Deleted :)";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Get userType by name
        public int getUserTypeByName(string name)
        {
            return this.Entity.userTypes.SingleOrDefault(x => x.type.ToLower() == name.ToLower()).userTypeID;
        }

        //Get usertype By id
        public string getUserTypeByid(int id)
        {
            return this.Entity.userTypes.SingleOrDefault(x => x.userTypeID == id).type.ToLower();
        }

        //Add Question to database
        public string addQuestion(string questionName, int quizID, List<string> answers)
        {
            try
            { 
                CommonLayer.question quest = new CommonLayer.question();
                quest.question1 = questionName;
                quest.quizID = quizID;
                this.Entity.questions.AddObject(quest);
                this.Entity.SaveChanges();

                int id = -1;
               
                //searxh in list of answers
                foreach (string name in answers)
                {
                    CommonLayer.answer a = new CommonLayer.answer();
                    a.answer1 = name;
                    a.questionID = quest.questionID;
                    this.Entity.answers.AddObject(a);
                    this.Entity.SaveChanges();
                    id= a.answerID;
                }

                // Add questions.

                CommonLayer.question uquest = getQuestionById(quest.questionID);
                uquest.correctAnswerID = id;
                this.Entity.SaveChanges();
               

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        //Get question by id retrun name
        public CommonLayer.question getQuestionById(int id)
        {
            return this.Entity.questions.SingleOrDefault(x => x.questionID == id);
        }

        //Get question by id retrun id
        public int getQuestionByIDID(int id)
        {
            return this.Entity.questions.SingleOrDefault(x => x.questionID == id).questionID;
        }
        //Delete question by id 
        public bool deleteQuestion(int id)
        {
            try
            {
          
            return true;
            }
            catch {
                return false; 
            }
        }
    }
}
