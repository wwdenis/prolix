// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Logic
{
    public sealed class ModelDescriptorRule<ModelType>
        where ModelType : class
    {
        public ModelDescriptorRule(Expression<Func<ModelType, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            Condition = expression;
        }

        public ModelDescriptorRule(Expression<Func<ModelType, bool>> expression, string message) : this(expression)
        {
            Message = message;
        }

        public Expression<Func<ModelType, bool>> Condition { get; }

        public string Name { get; set; }

        public string Message { get; set; }

        public bool Validate(ModelType entity)
        {
            var validation = Condition.Compile();
            return validation(entity);
        }
    }
}
