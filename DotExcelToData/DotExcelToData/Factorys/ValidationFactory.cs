﻿using Dot.Tools.ETD.Utils;
using Dot.Tools.ETD.Validations;
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
        public static List<IValidation> ParseRules(string multiRule)
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
                IValidation validation = ParseRule(rule);
                if(validation == null)
                {
                    validation = new ErrorValidation();
                    validation.SetRule(rule);
                }
                validations.Add(validation);
            }

            return validations;
        }

        public static IValidation ParseRule(string rule)
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
                if(type == null)
                {
                    return null;
                }
                IValidation validation = (IValidation)Activator.CreateInstance(type);
                validation.SetRule(rule);
                return validation;
            }

            return null;
        }
    }
}
