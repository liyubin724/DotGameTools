using Dot.Tools.ETD.Utils;
using Dot.Tools.ETD.Validations;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Factorys
{
    public class ValidationFactory
    {
        private static readonly string VALIDATION_NAME_REGEX = @"(?<name>[A-Za-z]+)";

        /// <summary>
        /// 
        /// For Example:NotNull@IntRange{100,100}@Unique
        /// </summary>
        /// <param name="multiRule"></param>
        /// <returns></returns>
        public static List<IValidation> GetValidations(string multiRule,IEIContext context)
        {
            List<IValidation> validations = new List<IValidation>();
            if(string.IsNullOrEmpty(multiRule))
            {
                return validations;
            }
            multiRule = multiRule.Trim();
            if(string.IsNullOrEmpty(multiRule))
            {
                return validations;
            }
            string[] rules = multiRule.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if(rules == null || rules.Length == 0)
            {
                return validations;
            }

            foreach(var rule in rules)
            {
                IValidation validation = GetValidation(rule);
                if(validation == null)
                {
                    validation = new ErrorValidation();
                }
                EIUtil.Inject(context,validation);

                validation.SetData(rule);
                validations.Add(validation);
            }

            return validations;
        }

        public static IValidation GetValidation(string rule)
        {
            if(string.IsNullOrEmpty(rule))
            {
                return null;
            }
            rule = rule.Trim();
            if(string.IsNullOrEmpty(rule))
            {
                return null;
            }

            Match match = new Regex(VALIDATION_NAME_REGEX).Match(rule);
            if(match.Success)
            {
                string validationName = match.Groups["name"].Value;
                if(string.IsNullOrEmpty(validationName))
                {
                    return null;
                }
                validationName += "Validation";
                Type type = AssemblyUtil.GetTypeByName(validationName, true);
                if(type == null || !typeof(IValidation).IsAssignableFrom(type))
                {
                    return null;
                }
                return (IValidation)Activator.CreateInstance(type); ;
            }

            return null;
        }
    }
}
