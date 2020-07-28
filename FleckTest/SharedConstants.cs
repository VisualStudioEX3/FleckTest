using System;
using System.Collections.Generic;
using System.Text;

namespace FleckTest
{
    public static class SharedConstants
    {
        #region Constants
        public const int LOGIN_ATTEMPS                      = 3;

        public const int BUFFER_SIZE                        = 1024;

        public const int SERVER_USERNAME_AVAILABLE          = 1000;
        public const int SERVER_USERNAME_IN_USE             = 1001;

        public const int CLIENT_CONNECTION_OK               = 2000;
        public const int CLIENT_CONNECTION_ERROR            = 2001;
        public const int CLIENT_CANCELLED_LOGIN             = 2002;
        #endregion
    }
}
