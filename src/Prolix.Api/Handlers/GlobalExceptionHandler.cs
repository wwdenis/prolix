// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

using Prolix.Api.Results;
using Prolix.Core.Data;
using Prolix.Core.Logic;

namespace Prolix.Api.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        const string APPLICATION = "Prolix";
        static bool _hasEventLogAccess;

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }

        static GlobalExceptionHandler()
        {
            LogToEvent("Application startup");
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            try
            {
                var scope = context.Request.GetDependencyScope();
                var db = scope.GetService(typeof(IDbContext)) as IDbContext;
                db?.Rollback();
            }
            catch
            {
                // Do nothing
            }

            var ex = context.Exception;

            if (ex is RuleException)
            {
                // Business vallidation errors
                var ruleEx = ex as RuleException;
                var rule = ruleEx.Rule ?? new RuleValidation();

                if (string.IsNullOrWhiteSpace(rule.Message))
                    rule.Message = ruleEx.Message;

                context.Result = new RuleValidationResult(context.Request, ruleEx.Rule);
            }
            else
            {
                LogException(ex);
                
                // TODO: Output an Error Id
                context.Result = new InternalServerErrorResult(context.Request);
            }
        }

        static void LogException(Exception ex)
        {
            bool eventSuccess = LogToEvent(ex.Message, EventLogEntryType.Error);

            if (!eventSuccess)
                LogToFile(ex);
        }

        static bool LogToEvent(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            if (!_hasEventLogAccess)
                return false;

            try
            {
                if (!EventLog.SourceExists(APPLICATION))
                    EventLog.CreateEventSource(APPLICATION, "Application");

                EventLog.WriteEntry(APPLICATION, message, type);

                _hasEventLogAccess = true;
                return true;
            }
            catch (Exception)
            {
                _hasEventLogAccess = false;
                return false;
            }
        }

        static bool LogToFile(Exception ex)
        {
            try
            {
                string name = string.Format("{0}_{1:yyyyMMdd}.txt", APPLICATION, DateTime.Now);
                string file = Path.Combine(Path.GetTempPath(), name);

                string line = new string('*', 40);
                var builder = new StringBuilder();

                builder.AppendLine(line);
                builder.AppendLine(string.Format("Date: {0}", DateTime.Now));
                builder.AppendLine(ex.ToString());

                var contents = builder.ToString();

                File.AppendAllText(file, contents);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
