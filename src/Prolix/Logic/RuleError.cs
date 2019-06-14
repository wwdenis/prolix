// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.



namespace Prolix.Logic
{
    /// <summary>
    /// Rule error associated to model property
    /// </summary>
    public sealed class RuleError
    {
        public RuleError()
        {
        }

        public RuleError(string name, string message)
        {
            Name = name;
            Message = message;
        }

        /// <summary>
        /// The property name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The error message
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Name, Message);
        }
    }
}
