// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolix.Core.Identity
{
    public class IdentityException : Exception
    {
        public IdentityException(string message) : base(message)
        {
        }

        public IdentityException(IdentityError reason)
        {
            Reason = reason;
        }

        public IdentityException(IdentityError reason, IEnumerable<string> messages) : this(reason)
        {
            Messages = messages?.ToArray() ?? new string[0];
        }

        public IdentityException(IEnumerable<string> messages) : this(IdentityError.Generic, messages)
        {
        }

        public IdentityException(string message, IEnumerable<string> messages) : this(message)
        {
            Messages = messages?.ToArray() ?? new string[0];
        }

        public string[] Messages { get; set; }

        public IdentityError Reason { get; set; }

        public string TranslatedReason()
        {
            string message = "Invalid request.";

            switch (Reason)
            {
                case IdentityError.AccountNotFound:
                    message = "User not found";
                    break;
                case IdentityError.AccountNotActive:
                    message = "The account is deactivated. Please contact your system administrator to activate it.";
                    break;
                case IdentityError.AccountLocked:
                    message = "You have exceeded the maximum number of attempts at this time.";
                    break;
                case IdentityError.InvalidPassword:
                    message = "Invalid password. Try again.";
                    break;
            }

            return message;
        }
    }
}
