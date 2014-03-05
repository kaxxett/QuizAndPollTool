using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class Connection
    {
        public CommonLayer.quizPollDBEntities Entity {get; set; }

        public Connection()
        {
            Entity = new CommonLayer.quizPollDBEntities();
        }
    }
}
