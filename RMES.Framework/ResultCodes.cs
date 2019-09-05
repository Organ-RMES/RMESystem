using System;
using System.Collections.Generic;
using System.Text;

namespace RMES.Framework
{
    public class ResultCodes
    {
        public static int Ok => 200;

        public static int UnAuthentication => 10401;

        public static int UnAuthorization => 10403;

        public static int NotFound => 10404;

        public static int BadRequest => 10400;

        public static int Exception => 10500;

        public static int DbFail => 10600;
    }
}
