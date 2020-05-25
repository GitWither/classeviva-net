﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClassevivaNet
{
    public class TextGrade : Grade
    {
        public string Value { get; }

        public TextGrade(string value, string comment, DateTime date, string subject, string type) : base(comment, date, subject, type)
        {
            Value = value;
        }

        public override string GetGradeString()
        {
            return Value;
        }
    }
}
